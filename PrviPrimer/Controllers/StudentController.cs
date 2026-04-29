using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace PrviPrimer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        public FakultetContext _context {get; set;}

        public StudentController(FakultetContext context) //konstruktor
        {
            _context = context;
        }

        [Route("preuzmiPodatke")]
        [HttpGet]
        public ActionResult Preuzmi()
        {
            return Ok(_context.Studenti);
        }

        [Route("PreuzmiSvePodatkeOStudentima")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSve()
        {
            //Eager loading
            var studenti = _context.Studenti
                .Include(p => p.StudentiPredmeti)
                .ThenInclude(p => p.Predmet)
                .Include(p => p.StudentiPredmeti)
                .ThenInclude(p => p.IspitniRok); //bez ToList() ovo je samo upit. Jos nismo nista izvukli

            var rezultat = await studenti.Where(p => p.indeks > 100).ToListAsync();
            return Ok(rezultat);    
        }

        [Route("PreuzmiSve2")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiSvePodatke()
        {
            //Explicit loading
            var student = await _context.Studenti.FirstOrDefaultAsync(s => s.ID == 1); //student sa ID-jem 1

            if(student==null)
            {
                return BadRequest("Student sa zadatim ID-jem ne postoji");
            }    

            await _context.Entry(student)
                .Collection(s => s.StudentiPredmeti) //popunjava se StudentiPredmeti da ne bude null
                .Query()
                .Include(s => s.Predmet)
                .Include(s => s.IspitniRok)
                .LoadAsync();

            //Bolje Query umesto foreach!!!

            return Ok(student);    
        }

        //NAPOMENA: ID ne moze da se promeni, pa se zato trazi ID, jer on ostaje isti i nakon promene.
        [Route("DodajStudenta")]
        [HttpPost]
        public async Task<ActionResult> DodajStudenta([FromBody] Student student)
        {
            if(student.indeks < 1 || student.indeks > 99999)
            {
                return BadRequest("Greska! Pogresan Indeks.");
            }

            if(student.Ime.Length>50 || string.IsNullOrWhiteSpace(student.Ime))
            {
                return BadRequest("Greska! Ime je previse dugo ili je prazno.");
            }

            if(student.Prezime.Length>50 || string.IsNullOrWhiteSpace(student.Prezime))
            {
                return BadRequest("Greska! Prezime je previse dugo ili je prazno.");
            }

            try
            {
                _context.Studenti.Add(student); //ali student jos nije stvarno unet u bazu.
                await _context.SaveChangesAsync(); //tek sad jeste
                return Ok($"Student {student.Ime} {student.Prezime} je dodat u bazu. ID je {student.ID} .");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmeniStudenta/{indeks}/{ime}/{prezime}")]
        [HttpPut]
        public async Task<ActionResult> IzmeniStudenta(int indeks, string ime, string prezime)
        {
            if(indeks<1 || indeks>99999)
            {
                return BadRequest("Prosledjen je neispravan indeks! ");
            }

            try
            {
                //prodji kroz sve studente dok ne nadjes onog sa indeksom indeks
                //Ova metoda se koristi ako NE TRAZIS po primarnom kljucu. Nego kao ovde po indeksu.
                var student = _context.Studenti.Where(s => s.indeks == indeks).FirstOrDefault();
                if(student!=null)
                {
                    student.Ime = ime;
                    student.Prezime = prezime;
                    await _context.SaveChangesAsync();

                    return Ok($"Student sa ID-jem {student.ID} je uspesno izmenjen. \n");
                }
                else
                {
                    return BadRequest($"Student da indeksom {indeks} nije pronadjen u bazi.\n");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PromenaStudentaFromBody")]
        [HttpPut]
        public async Task<ActionResult> PromeniStudentaFromBody([FromBody] Student student)
        {
            try
            {
                //ovo koristis kada trazis ISKLJUCIVO po primarnom kljucu
                var studentZaIzmenu = await _context.Studenti.FindAsync(student.ID);
                if(studentZaIzmenu!=null)
                {
                    studentZaIzmenu.indeks = student.indeks;
                    studentZaIzmenu.Ime = student.Ime;
                    studentZaIzmenu.Prezime = student.Prezime;

                    await _context.SaveChangesAsync();

                    return Ok($"Student sa ID-jem {student.ID} je izmenjen. \n");
                }
                else
                {
                    return BadRequest("Student nije pronadjen u bazi. \n");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzbrisiStudenta/{id}")]
        [HttpDelete]
        public async Task<ActionResult> IzbrisiStudenta(int id)
        {
            if(id<=0)
            {
                return BadRequest("Greska. Neispravan id!");
            }

            try
            {
                var studentZaBrisanje = await _context.Studenti.FindAsync(id);
                if(studentZaBrisanje != null)
                {
                    _context.Studenti.Remove(studentZaBrisanje);
                    await _context.SaveChangesAsync();
                    return Ok($"Obrisan je student sa ID-jem {studentZaBrisanje.ID} .");
                }
                else
                {
                    return BadRequest("Student za brisanje nije nadjen u bazi! ");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Metoda: Vraca studente koji su polozili ispite u odgovarajucim rokovima.
        [HttpGet]
        [Route("StudentiKojiSuPoloziliURoku")]
        public async Task<ActionResult> vratiStudente([FromQuery] int[] rokIds)
        {
            //sa Eager loading:
            var studenti = await _context.Studenti
                .Include(s => s.StudentiPredmeti)
                .ThenInclude(s => s.Predmet)
                .Include(s => s.StudentiPredmeti)
                .ThenInclude(s => s.IspitniRok)
                .Select(s => new //sad sa Select obezbedjujemo sta se zapravo vraca, znaci ne ceo objekat
                {
                    StudentID = s.ID,
                    Ime = s.Ime,
                    Prezime = s.Prezime,
                    BrIndeksa = s.indeks,
                    Ispiti = s.StudentiPredmeti //veza spoja. Ima u sebi IspitniRokID pa se zato ovde filtrira uslov
                            .Where(s => rokIds.Contains(s.IspitniRokID)) //sad se dalje iz klase veze izvlace samo bitna polja
                            .Select(q => new
                            {
                                Predmet = q.Predmet.Naziv,
                                IspitniRok = q.IspitniRok.Naziv,
                                Ocena = q.ocena
                            }).ToList()
                }).ToListAsync();

                return Ok(studenti);
        }

        //Metoda: Vraca informaciju o jednom studentu(na osnovu indeksa) i svim njegovim ocenama iz svih rokova
        [Route("VratiJednogStudenta")]
        [HttpGet]
        public async Task<ActionResult> VratiStudenta([FromQuery] int indeks)
        {
            if(indeks<1 || indeks > 99999)
            {
                return BadRequest("Nevalidan indeks! ");
            }

            //Eager loading
            var student = await _context.Studenti
                .Include(s => s.StudentiPredmeti)
                .ThenInclude(s => s.Predmet)
                .Include(s => s.StudentiPredmeti)
                .ThenInclude(s => s.IspitniRok)
                .Where(s => s.indeks == indeks)
                .Select(s => new
                {
                    StudentID = s.ID,
                    Indeks = s.indeks,
                    Ime = s.Ime,
                    Prezime = s.Prezime,
                    PolozeniIspiti = s.StudentiPredmeti
                        .Where(s=>s.ocena > 5)
                        .Select(q => new
                        {
                            IspitniRok = q.IspitniRok.Naziv,
                            Predmet = q.Predmet.Naziv,
                            Ocena = q.ocena
                        }).ToList()
                }).FirstOrDefaultAsync();

                return Ok(student);
        }
    }
}