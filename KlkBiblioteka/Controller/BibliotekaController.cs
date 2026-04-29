using KlkBiblioteka.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Models;

namespace KlkBiblioteka.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BibliotekaController : ControllerBase
    {
        public BibliotekaContext Context {get; set;}

        //konstruktor
        public BibliotekaController(BibliotekaContext context)
        {
            Context = context;
        }

        [Route("DodajKnjigu")]
        [HttpPost]
        public async Task<ActionResult> DodajKnjigu([FromBody] Knjiga knjiga)
        {
            if(knjiga == null)
            {
                return BadRequest("Prosledjen argument je null!");
            }

            try
            {
                await Context.Knjige.AddAsync(knjiga);
                await Context.SaveChangesAsync();

                return Ok($"Knjiga {knjiga.Naslov} je dodata u bazu. ID = {knjiga.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajBiblioteku")]
        [HttpPost]
        public async Task<ActionResult> DodajBiblioteku([FromBody] Biblioteka biblioteka)
        {
            if(biblioteka == null)
            {
                return BadRequest("Prosledjeni parametar je null!");
            }

            try
            {
                await Context.Biblioteke.AddAsync(biblioteka);
                await Context.SaveChangesAsync();

                return Ok($"Biblioteka {biblioteka.Ime} je dodata u bazu. ID = {biblioteka.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("IzdajKnjigu")]
        [HttpPost]
        public async Task<ActionResult> DodajIzdavanje([FromQuery] IzdavanjeKnjigeDto novoIzdavanje)
        {
            DateTime NijeVracenaVrednost = new DateTime(3000, 1, 1);

            if(novoIzdavanje == null)
            {
                return BadRequest("Prosledjen parametar je null!");
            }

            try
            {
                var knjiga = await Context.Knjige
                        .FirstOrDefaultAsync(s => s.ID == novoIzdavanje.KnjigaID);

                if(knjiga == null)
                {
                    return BadRequest($"Ne postoji knjiga sa ID-jem {novoIzdavanje.KnjigaID}. ");
                }

                var biblioteka = await Context.Biblioteke
                            .FirstOrDefaultAsync(s => s.ID == novoIzdavanje.BibliotekaID);  

                if(biblioteka == null)
                {
                    return BadRequest($"Ne postoji biblioteka sa ID-jem {novoIzdavanje.BibliotekaID}. ");
                }

                //provera da li je knjiga mozda vec izdata, a nije vracena
                bool VecIzdata = await Context.Izdavanja.AnyAsync( i => 
                    i.knjiga!.ID == novoIzdavanje.KnjigaID &&
                    i.biblioteka!.ID == novoIzdavanje.BibliotekaID &&
                    i.DatumVracanja == NijeVracenaVrednost
                );

                var knjigaKojaSeIzdaje = await Context.Izdavanja
                        .FirstOrDefaultAsync(s => s.knjiga!.ID == novoIzdavanje.KnjigaID
                            && s.biblioteka!.ID == novoIzdavanje.BibliotekaID
                            && s.DatumVracanja == NijeVracenaVrednost);

                if(VecIzdata)
                {
                    return BadRequest($"Knjiga {knjigaKojaSeIzdaje!.knjiga!.Naslov} je vec izdata u ovoj biblioteci!");
                }

                var izdavanje = new Izdavanje
                {
                    DatumIzdavanja = DateTime.Now,
                    DatumVracanja = NijeVracenaVrednost,
                    knjiga = knjiga,
                    biblioteka = biblioteka
                };

                await Context.AddAsync(izdavanje);
                await Context.SaveChangesAsync();

                return Ok($"Novo izdavanje je dodato u bazu. ID = {izdavanje.ID}.");           
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiIzdatuKnjigu")]
        [HttpPut] //zato sto se samo azurira Izdavanje
        public async Task<ActionResult> VracanjeIzdateKnjige([FromBody] VracanjeKnjigeDto izdataKnjiga)
        {
            DateTime nevalidanDatum = new DateTime(3000, 1, 1); 

            if(izdataKnjiga == null)
            {
                return BadRequest("Prosledjen argument je null!");
            }

            try
            {
                var izdavanjeZaAzuriranje = await Context.Izdavanja
                        .FirstOrDefaultAsync(s => s.ID == izdataKnjiga.IzdavanjeID);

                if(izdavanjeZaAzuriranje == null)
                {
                    return BadRequest($"U bazi nije nadjeno izdavanje da ID-jem {izdataKnjiga.IzdavanjeID}.");
                }

                //knjiga je mozda vec vracena:
                if(izdavanjeZaAzuriranje.DatumVracanja != nevalidanDatum)
                {
                    return BadRequest("Ovu knjigu ste vec vratili! Izdavanje je azurirano.");
                }

                izdavanjeZaAzuriranje.DatumVracanja = DateTime.Now;
                await Context.SaveChangesAsync();

                return Ok($"Izdavanje ID = {izdataKnjiga.IzdavanjeID} je azurirano. Datum vracanje je {izdavanjeZaAzuriranje.DatumVracanja}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("UkupanBrojTrenutnoIzdatihKnjiga")]
        [HttpGet]
        public async Task<ActionResult> UkupanBrojIzdatihKnjiga()
        {
            try
            {
                DateTime invalidDate = new DateTime(3000, 1, 1);

                int rezultat = await Context.Izdavanja
                            .Where(s => s.DatumVracanja == invalidDate) //trenutno izdata knjiga
                            .CountAsync();

                return Ok($"Ukupan broj trenutno izdatih knjiga je: {rezultat}");            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Pronalazenje najcitanijeg autora. Autor cije knjige imaju najveci broj izdavanja
        [Route("PronadjiNajcitanijegAutora")]
        [HttpGet]
        public async Task<ActionResult> NajcitanijiAutor()
        {
            try
            {
                if(! await Context.Izdavanja.AnyAsync())
                {
                    return BadRequest("Trenutno nema nijednog izdavanja u bazi.");
                }

                var rezultat = await Context.Knjige
                            .Include(s => s.Izdavanja)
                            .OrderByDescending(s => s.Izdavanja!.Count())
                            .FirstOrDefaultAsync();

                if(rezultat == null)
                {
                    return BadRequest("Nije nadjena nijedna knjiga pa zato i nijedan autor.");
                }

                var autor = new
                {
                    Autor = rezultat.ImeAutora,
                    Knjiga = rezultat.Naslov,
                    BrojIzdavanja = rezultat.Izdavanja!.Count()
                };

                return Ok(autor);   
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}