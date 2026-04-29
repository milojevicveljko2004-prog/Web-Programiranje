using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models;
using RekorderZadatak.Code;

namespace RekorderZadatak.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RekorderController : ControllerBase
    {
        public TakmicenjeContext Context {get; set;}

        //konstruktor
        public RekorderController(TakmicenjeContext context)
        {
            Context = context;
        }

        [Route("DodajRekordera")]
        [HttpPost]
        public async Task<ActionResult> DodajRekordera([FromBody] Rekorder rekorder)
        {
            if(string.IsNullOrWhiteSpace(rekorder.Ime))
            {
                return BadRequest("Ime ne sme biti prazno!");
            }

            if(string.IsNullOrWhiteSpace(rekorder.Prezime))
            {
                return BadRequest("Prezime ne sme biti prazno!");
            }

            try
            {
                await Context.Rekorderi.AddAsync(rekorder);
                await Context.SaveChangesAsync();
                return Ok($"Rekorder {rekorder.Ime} {rekorder.Prezime} je dodat u bazu. ID = {rekorder.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajDisciplinu")]
        [HttpPost]
        public async Task<ActionResult> DodajDisciplinu([FromBody] Disciplina disciplina)
        {
            if(string.IsNullOrWhiteSpace(disciplina.Naziv))
            {
                return BadRequest("Naziv discipline ne sme biti prazan!");
            }

            try
            {
                await Context.Discipline.AddAsync(disciplina);
                await Context.SaveChangesAsync();
                return Ok($"Disciplina {disciplina.Naziv} je dodata u bazu. ID = {disciplina.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ObrisiDisciplinu")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiDisciplinu([FromQuery] int id)
        {
            if(id<0)
            {
                return BadRequest("Ne postoji negativan id! ");
            }

            try
            {
                var disciplinaZaBrisanje = await Context.Discipline
                                .Include(s => s.rekordi)
                                .Where(s => s.ID == id)
                                .FirstOrDefaultAsync();

                if(disciplinaZaBrisanje == null)
                {
                    return BadRequest($"U bazi ne postoji disciplina sa ID-jem {id}.");
                }

                Context.Discipline.Remove(disciplinaZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Disciplina {disciplinaZaBrisanje.Naziv} je obrisana iz baze.");  
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzmeniRekordera")]
        [HttpPut]
        public async Task<ActionResult> IzmeniRekordera([FromQuery] int id, [FromQuery] Rekorder noviRekorder)
        {
            if(id<0)
            {
                return BadRequest("Ne postoji negativan id! ");
            }

            if(noviRekorder == null)
            {
                return BadRequest("Nije prosledjen novi rekorder! ");
            }

            try
            {
                var rekorderZaIzmenu = await Context.Rekorderi.FindAsync(id);
                                    // .Include(s => s.rekordi)
                                    // .Where(s => s.ID == id)
                                    // .FirstOrDefaultAsync();

                if(rekorderZaIzmenu == null)
                {
                    return BadRequest($"U bazi ne postoji rekorder sa ID-jem {id}.");
                }

                if (string.IsNullOrWhiteSpace(noviRekorder.Ime) || string.IsNullOrWhiteSpace(noviRekorder.Prezime))
        return BadRequest("Ime i prezime su obavezni.");

                rekorderZaIzmenu.Ime = noviRekorder.Ime;
                rekorderZaIzmenu.Prezime = noviRekorder.Prezime;
                rekorderZaIzmenu.DatumRodjenja = noviRekorder.DatumRodjenja;
                rekorderZaIzmenu.Pol = noviRekorder.Pol;
                rekorderZaIzmenu.Sport = noviRekorder.Sport;

                await Context.SaveChangesAsync();

                return Ok(rekorderZaIzmenu);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajRekord")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeRekorda([FromBody] PodaciRekord noviRekord)
        {
            if(noviRekord == null)
            {
                return BadRequest("Nista nije uneto! ");
            }

            if(noviRekord.DisciplinaID < 0 || noviRekord.RekorderID < 0)
            {
                return BadRequest("ID ne sme biti manji od 0! ");
            }

            if(string.IsNullOrWhiteSpace(noviRekord.InformacijaOTakmicenju))
            {
                return BadRequest("Informacija o takmicenju ne sme biti prazna! ");
            }

            try
            {
                var rekorder = await Context.Rekorderi.FindAsync(noviRekord.RekorderID);
                var disciplina = await Context.Discipline.FindAsync(noviRekord.DisciplinaID);

                if(rekorder == null)
                {
                    return BadRequest("Trazeni rekorder ne postoji u bazi.");
                }
                if(disciplina == null)
                {
                    return BadRequest("Trazena disciplina ne postoji u bazi.");
                }

                var rekord = new Rekord
                {
                    InformacijaOTakmicenju = noviRekord.InformacijaOTakmicenju,
                    Datum = noviRekord.Datum,
                    rekorder = rekorder,
                    disciplina = disciplina
                };

                await Context.Rekordi.AddAsync(rekord);
                await Context.SaveChangesAsync();
                return Ok($"Novi rekord ID={rekord.ID} je dodat u bazu.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ObrisiRekord/{id}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiRekord(int id)
        {
            if(id<0)
            {
                return BadRequest("id ne sme biti manji od 0! ");
            }

            try
            {
                var rekordZaBrisanje = await Context.Rekordi.FindAsync(id);
                if(rekordZaBrisanje == null)
                {
                    return BadRequest($"Rekord za ID-jem {id} ne postoji u bazi.");
                }

                Context.Rekordi.Remove(rekordZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Rekord sa ID-jem {id} je obrisan.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Pronadji rekordera sa najvecim brojem rekorda pocevsi od zadatog datuma
        [Route("PronadjiRekordera_SaNajviseRekorda/{datum}")]
        [HttpGet]
        public async Task<ActionResult> PronadjiNajboljegRekordera(DateTime datum)
        {
            try{
            var takmicar = await Context.Rekorderi
                    .Include(s => s.rekordi)
                    .Select(s => new
                    {
                        Ime = s.Ime,
                        Prezime = s.Prezime,
                        BrojRekorda = s.rekordi!.Count(q => q.Datum >= datum)
                    }).OrderByDescending(s => s.BrojRekorda)
                      .FirstOrDefaultAsync();

            if(takmicar == null)
            {
                return BadRequest("Takmicar nije pronadjen!");
            }

            if(takmicar.BrojRekorda == 0)
                {
                    return BadRequest("Nijedan takmicar nema rekorde u zadatom periodu?");
                }

            return Ok($"Takmicar sa najvise rekorda je {takmicar.Ime} {takmicar.Prezime} Broj rekorda = {takmicar.BrojRekorda}.");    
             
        }
        catch(Exception e)
        {
             return BadRequest(e.Message);   
        }
    }

    //Pronalazenje najstarije discipline koja ima manje od n rekorda
        [HttpGet]
        [Route("VratiNajstarijuDisciplinu")]
        public async Task<ActionResult> NajstarijaDisciplina(int n)
        {
            if(n<0)
            {
                return BadRequest("Prosledjen broj n ne sme biti negativan!");
            }

            try
            {
                var rezultat = await Context.Discipline
                            .Include(s => s.rekordi)
                            .Select(s => new
                            {
                                Naziv = s.Naziv,
                                Starost = s.DatumOd,
                                BrojRekorda = s.rekordi!.Count()
                            }).Where(s => s.BrojRekorda < n)
                            .OrderBy(s => s.Starost) //Ili mozda OrderByDescending?
                            .FirstOrDefaultAsync(); 

                if(rezultat == null)
                {
                    return BadRequest("Disciplina je null, nije pronadjena.");
                }

                return Ok(rezultat);            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    
    }
}