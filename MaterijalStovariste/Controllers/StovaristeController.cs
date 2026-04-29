using MaterijalStovariste.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace MaterijalStovariste.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StovaristeController : ControllerBase
    {
        public StovaristeContext Context {get; set;}
        public StovaristeController(StovaristeContext context)
        {
            Context = context;
        }

        [Route("DodajStovariste")]
        [HttpPost]
        public async Task<ActionResult> DodajStovariste([FromBody] Stovariste stovariste)
        {
            if(stovariste == null)
            {
                return BadRequest("Prosledjen argument je null!");
            }

            try
            {
                await Context.Stovarista.AddAsync(stovariste);
                await Context.SaveChangesAsync();

                return Ok($"Stovariste {stovariste.Ime} je dodato u bazu. ID = {stovariste.ID}. ");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajMaterijal")]
        [HttpPost]
        public async Task<ActionResult> DodavanjeMaterijala([FromBody] Materijal materijal)
        {
            if(materijal == null)
            {
                return BadRequest($"Prosledjen argument je null!");
            }

            try
            {
                await Context.Materijali.AddAsync(materijal);
                await Context.SaveChangesAsync();

                return Ok($"Materijal {materijal.Naziv} je dodat u bazu podataka. ID = {materijal.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajMaterijalNaStovariste")]
        [HttpPost]
        public async Task<ActionResult> DodajMaterijal([FromBody] DodavanjeMaterijalaDto noviMaterijal)
        {
            if(noviMaterijal == null)
            {
                return BadRequest("Prosledjen argument je null!");
            }

            try
            {
                var stovariste = await Context.Stovarista
                            .FirstOrDefaultAsync(s => s.ID == noviMaterijal.StovaristeID);

                if(stovariste == null)
                {
                    return BadRequest($"Trazeno stovariste ID = {noviMaterijal.StovaristeID} ne postoji u bazi!");
                }   

                var materijal = await Context.Materijali
                            .FirstOrDefaultAsync(s => s.ID == noviMaterijal.MaterijalID);

                if(materijal == null)
                {
                    return BadRequest($"Trazeni materijal ID = {noviMaterijal.MaterijalID} ne postoji u bazi!");
                }

                var isporucenMaterijal = new IsporucenMaterijal
                {
                    DatumIsporuke = noviMaterijal.DatumIsporuke,
                    KolicinaMaterijala = noviMaterijal.KolicinaMaterijala,
                    stovariste = stovariste,
                    materijal = materijal
                };

                await Context.IsporuceniMaterijali.AddAsync(isporucenMaterijal);
                await Context.SaveChangesAsync();

                return Ok($"Isporuceni materijal ID =  {isporucenMaterijal.ID} Kolicina materijala = {isporucenMaterijal.KolicinaMaterijala} je dodat u bazu.");               
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Odredi ukupnu kolicinu materijala na stovaristu
        [Route("UkupnaKolicinaMaterijala/{IdStovariste}")]
        [HttpGet]
        public async Task<ActionResult> UkupnaKolicinaMaterijala([FromRoute] int IdStovariste)
        {
            if(IdStovariste < 0)
            {
                return BadRequest("Id ne moze biti manji od 0!");
            }

            try
            {
                //prvo trebam da nadjem trazeno stovariste
                var stovariste = await Context.Stovarista
                        .Include(s => s.IsporuceniMaterijali)
                        .FirstOrDefaultAsync(s => s.ID == IdStovariste);

                if(stovariste == null)
                {
                    return BadRequest($"Stovariste sa ID-jem {IdStovariste} ne postoji u bazi!");
                }

                int rezultat = stovariste.IsporuceniMaterijali!.Sum(s => s.KolicinaMaterijala);

                return Ok($"Ukupna kolicina materijala za stovariste {stovariste.ID} je {rezultat}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Prodaja odredjene kolicine materijala iz zadatog stovarista
        [Route("ProdajMaterijalSaStovarista/{kolicinaMaterijala}/{IdStovariste}")]
        [HttpPut]
        public async Task<ActionResult> ProdajMaterijal([FromRoute] int kolicinaMaterijala,[FromRoute] int IdStovariste)
        {
            if(kolicinaMaterijala <= 0)
            {
                return BadRequest("Kolicina materijala ne moze biti negativna!");
            }

            if(IdStovariste <= 0)
            {
                return BadRequest("ID stovarista ne moze biti negativan!");
            }

            try
            {
                //prvo pronadji stovariste
                var stovariste = await Context.Stovarista
                        .Include(s => s.IsporuceniMaterijali)
                        .FirstOrDefaultAsync(s => s.ID == IdStovariste);

                if(stovariste == null)
                {
                    return BadRequest($"Stovariste sa ID-jem {IdStovariste} ne postoji u bazi.");
                }

                //za zadato stovariste je potrebno smanjiti njegovu kolicinu materijala 
                //Bira se materijal koji je najduze na stovaristu

                var materijal = stovariste.IsporuceniMaterijali!
                                .OrderBy(s => s.DatumIsporuke)
                                .FirstOrDefault();

                if(materijal==null)
                {
                    return BadRequest("Nijedan isporucen materijal nije pronadjen u stovaristu!");
                }

                if(materijal.KolicinaMaterijala < kolicinaMaterijala)
                {
                    return BadRequest($"Ne mozete prodati {kolicinaMaterijala} materijala jer je ukupan broj materijala u stovaristu {materijal.KolicinaMaterijala}.");
                }                

                materijal.KolicinaMaterijala -= kolicinaMaterijala;
                await Context.SaveChangesAsync(); 

                return Ok($"{kolicinaMaterijala} materijala je prodato iz stovarista {stovariste.Ime}.");                      
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Pronalazenje materijala iz svih dostava koga trenutno ima najvise na stovaristu
        // [Route("PronadjiNajcesciMaterijal")]
        // [HttpGet]
        // public async Task<ActionResult> PronadjiMaterijal(string nazivStovarista)
        // {
        //     if(string.IsNullOrWhiteSpace(nazivStovarista))
        //     {
        //         return BadRequest("Prosledjen je prazan string kao naziv!");
        //     }

        //     try
        //     {
        //         //provera da li uopste postoji trazeno stovariste
        //         var stovariste = await Context.Stovarista
        //                         .Include(s => s.IsporuceniMaterijali)
        //                         .FirstOrDefaultAsync(s => s.Ime == nazivStovarista);

        //         if(stovariste == null)
        //         {
        //             return BadRequest($"Stovariste {nazivStovarista} nije pronadjeno u bazi.");
        //         }

        //         var materijal = stovariste.IsporuceniMaterijali! //prolazi kroz sve dostave(isporucene materijala) na stovaristu
        //                         .OrderByDescending(s => s.KolicinaMaterijala) //Ko ima najvecu kolicinu materijala znaci da ga ima najvise?
        //                         .FirstOrDefault();  

        //         if(materijal == null)
        //         {
        //             return BadRequest($"Na stovaristu {nazivStovarista} nema isporucenih materijala.");
        //         } 

        //         return Ok($"Nadjen je materijal.\n ID = {materijal.ID} \nKolicina materijala = {materijal.KolicinaMaterijala}");                             
        //     }
        //     catch(Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        [Route("PronadjiMaterijal2")]
        [HttpGet]
        public async Task<ActionResult> PronadjiMaterijal(string nazivStovarista)
        {
            if(string.IsNullOrWhiteSpace(nazivStovarista))
            {
                return BadRequest("Prosledjen je prazan string!");
            }

            try
            {
                var stovariste = await Context.Stovarista
                            .Include(s => s.IsporuceniMaterijali)
                            .FirstOrDefaultAsync(s => s.Ime == nazivStovarista);

                if(stovariste == null)
                {
                    return BadRequest("Trazeno stovariste ne postoji u bazi.");
                }

                var rezultat = await Context.IsporuceniMaterijali
                                .Include(s => s.materijal)
                                .Include(s => s.stovariste)
                                .Where(s => s.stovariste!.Ime == nazivStovarista)
                                .GroupBy(s => new{s.materijal!.ID, s.materijal.Naziv})
                                .Select(g => new
                                {
                                    MaterijalID = g.Key.ID,
                                    Naziv = g.Key.Naziv,
                                    UkupnoNaStanju = g.Sum(x => x.KolicinaMaterijala)
                                }).OrderByDescending(x => x.UkupnoNaStanju).FirstOrDefaultAsync();

                if(rezultat == null)
                {
                    return BadRequest("Nije nadjen materijal.");
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