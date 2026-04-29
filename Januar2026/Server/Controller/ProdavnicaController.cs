using DTOs;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProdavnicaController : ControllerBase
    {
        public ProdavnicaContext Context {get; set;}

        public ProdavnicaController(ProdavnicaContext context)
        {
            this.Context = context;
        }

        //prvo metode za dodavanje podataka, jer podaci nekako moraju da budu u bazi

        [Route("DodajProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
        {
            if(prodavnica==null || string.IsNullOrWhiteSpace(prodavnica.Naziv))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Prodavnice.AddAsync(prodavnica);
                await Context.SaveChangesAsync();

                return Ok($"Nova prodavnica {prodavnica.Naziv} je dodata u bazu! ID={prodavnica.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajArtikal")]
        [HttpPost]
        public async Task<ActionResult> DodajArtikal([FromBody] Artikal artikal)
        {
            if(artikal==null || string.IsNullOrWhiteSpace(artikal.Brend) || string.IsNullOrWhiteSpace(artikal.SifraModela))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Artikli.AddAsync(artikal);
                await Context.SaveChangesAsync();

                return Ok($"Novi artikal sa sifrom {artikal.SifraModela} je dodat u bazu. ID = {artikal.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajArtikalUProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajArtikalUProdavnicu([FromBody] DodavanjeProizvodaDTO dto)
        {
            if(dto==null || dto.ArtikalID<=0 || dto.ProdavnicaID<=0 || dto.Cena<0 || dto.KolicinaL<0
            || dto.KolicinaM<0 || dto.KolicinaS<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                //prvo provera da li zadata prodavnica i artikal postoje

                var prodavnica = await Context.Prodavnice.FindAsync(dto.ProdavnicaID);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={dto.ProdavnicaID} ne postoji u bazi!");
                }

                var artikal = await Context.Artikli.FindAsync(dto.ArtikalID);
                if(artikal==null)
                {
                    return BadRequest($"Artikal sa ID={dto.ArtikalID} ne postoji u bazi!");
                }

                var noviArtikal = new ArtikalUProdaji
                {
                    Cena = dto.Cena,
                    KolicinaS = dto.KolicinaS,
                    KolicinaM = dto.KolicinaM,
                    KolicinaL = dto.KolicinaL,
                    Artikal = artikal,
                    Prodavnica = prodavnica
                };

                await Context.ArtikliUProdaji.AddAsync(noviArtikal);
                await Context.SaveChangesAsync();

                return Ok($"Artikal sa sifrom {artikal.SifraModela} je dodat u prodavnicu {prodavnica.Naziv}. ID = {noviArtikal.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("NadjiArtikal")]
        [HttpPost]
        public async Task<ActionResult> NadjiArtikal([FromBody] NadjiArtikalDTO dto)
        {
            if(dto==null || string.IsNullOrWhiteSpace(dto.Brend) || dto.CenaOd<0 || dto.CenaDo<0 || dto.ProdavnicaID<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            //Ovo ne treba jer na klijentskoj strani ce se popuniti select polje, to obezbedjuje sta moze da se odabere
            // if(dto.Velicina!='S' && dto.Velicina!='M' && dto.Velicina!='L')
            // {
            //     return BadRequest("Greska! Velicina mora biti S, L ili M.");
            // }

            try
            {
                //Ima vise opcija? Ako je odabrao samo brend jedino filtriranje je po brendu?
                //Kako bira vise stvari treba vise filtriranja sa where

                var upit = Context.ArtikliUProdaji
                                .Include(s => s.Prodavnica)
                                .Include(s => s.Artikal)
                                .Where(s => s.Prodavnica !=null && s.Prodavnica.ID==dto.ProdavnicaID)
                                .Where(s => s.Artikal!=null && s.Artikal.Brend==dto.Brend);

                if(dto.CenaOd.HasValue)
                {
                    upit = upit.Where(s => s.Cena >= dto.CenaOd.Value);
                }

                if(dto.CenaDo.HasValue)
                {
                    upit = upit.Where(s => s.Cena <= dto.CenaDo.Value);
                }

                if(!string.IsNullOrWhiteSpace(dto.Velicina)) //neko je odabrao velicinu S, L ili M
                {
                    if(dto.Velicina == "S")
                    {
                        upit = upit.Where(s => s.KolicinaS>0);
                    }
                    else if(dto.Velicina == "M")
                    {
                        upit = upit.Where(s => s.KolicinaM>0);
                    }
                    else if(dto.Velicina == "L")
                    {
                        upit = upit.Where(s => s.KolicinaL>0);
                    }
                    else
                    {
                        return BadRequest("Velicina mora biti S, M ili L.");
                    }
                }
                else //nije odabrana velicina - znaci prikazi ako je bilo koja kolicina dostupna
                {
                    upit = upit.Where(s => s.KolicinaS>0 || s.KolicinaM>0 || s.KolicinaL>0);
                }

                var result = await upit.Select(s => new
                {
                    ID = s.ID,
                    SifraModela = s.Artikal!.SifraModela,
                    Slika = s.Artikal.Slika,
                    Cena = s.Cena,
                    Velicina = dto.Velicina,
                    KolicinaS = s.KolicinaS,
                    KolicinaL = s.KolicinaL,
                    KolicinaM = s.KolicinaM
                })
                .ToListAsync(); 

                return Ok(result);               
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("KupiArtikal")]
        [HttpPut]
        public async Task<ActionResult> KupiArtikal([FromBody] KupiArtikalDTO dto)
        {
            if(dto==null || dto.ArtikalUProdajiID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var artikal = await Context.ArtikliUProdaji.FindAsync(dto.ArtikalUProdajiID);
                if(artikal==null)
                {
                    return BadRequest($"ArtikalUProdaji ID={dto.ArtikalUProdajiID} ne postoji u bazi!");
                }

                if(dto.Velicina=="S")
                {
                    if(artikal.KolicinaS<=0)
                    {
                        return BadRequest("Artikal nije dostupan u velicini S.");
                    }
                    artikal.KolicinaS--;
                }
                else if(dto.Velicina == "M")
                {
                    if(artikal.KolicinaM<=0)
                    {
                        return BadRequest("Artikal nije dostupan u velicini M.");
                    }
                    artikal.KolicinaM--;
                }
                else if(dto.Velicina=="L")
                {
                    if(artikal.KolicinaL<=0)
                    {
                        return BadRequest("Artikal nije dostupan u velicini L.");
                    }
                    artikal.KolicinaL--;
                }
                else
                {
                    return BadRequest("Greska! Dozvoljene velicine su L, S i M.");
                }

                await Context.SaveChangesAsync();
                return Ok($"Proizvod ID={artikal.ID} je kupljen.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiAkcijskiArtikal/{idProdavnice}")]
        [HttpGet]
        public async Task<ActionResult> VratiAkcijskiArtikal(int idProdavnice)
        { 
            if(idProdavnice<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }
            try
            {
                // var prodavnica = await Context.Prodavnice.FindAsync(idProdavnice);
                // if(prodavnica==null)
                // {
                //     return BadRequest($"Prodavnica sa ID={idProdavnice} ne postoji u bazi.");
                // }

                var akcijski = await Context.ArtikliUProdaji
                                .Include(s => s.Prodavnica)
                                .Include(s => s.Artikal)
                                .Where(s => s.Prodavnica!=null && s.Prodavnica.ID == idProdavnice)
                                .Where(s => s.Artikal!=null)
                                .Where(s => s.KolicinaL+s.KolicinaM+s.KolicinaS>0)
                                .OrderBy(s => s.KolicinaL+s.KolicinaM+s.KolicinaS)
                                .FirstOrDefaultAsync();

                if(akcijski==null || akcijski.Artikal==null)
                {
                    return new JsonResult(null); //Da klijent ne bi dobio nista!!! Mora ovako!
                }

                 return Ok(new
                 {
                     ArtikalUProdavniciID = akcijski.ID,
                    Sifra = akcijski.Artikal.SifraModela,
                    Brend = akcijski.Artikal.Brend,
                    Slika = akcijski.Artikal.Slika,
                    Cena = akcijski.Cena,
                    PrikazCena = akcijski.Cena * 0.5,
                    KolicinaS = akcijski.KolicinaS,
                    KolicinaM = akcijski.KolicinaM,
                    KolicinaL = akcijski.KolicinaL
                 });               
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Vrati prodavnice sa proizvodima
        [Route("VratiProdavniceSaProizvodima")]
        [HttpGet]
        public async Task<ActionResult> VratiProdavniceSaProizvodima()
        {
            try
            {
                var result = await Context.Prodavnice.Include(s => s.ArtikliUProdaji).ToListAsync();
                if(result==null)
                {
                    return BadRequest("Neuspesno pribavljanje prodavnica iz baze!");
                }

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //metoda koja vraca brendove za odredjenu prodavnicu
        [Route("VratiBrendove/{idProdavnice}")]
        [HttpGet]
        public async Task<ActionResult> VratiBrendove(int idProdavnice)
        {
            if(idProdavnice<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var result = await Context.ArtikliUProdaji
                                .Include(s => s.Prodavnica)
                                .Include(s => s.Artikal)
                                .Where(s => s.Prodavnica!=null && s.Prodavnica.ID==idProdavnice)
                                .Where(s => s.Artikal!=null)
                                .Select(s => s.Artikal!.Brend)
                                .Distinct() //da nema duplikata!!!
                                .ToListAsync();

                if(result==null)
                {
                    return BadRequest("Nisu nadjeni brendovi.");
                }                

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}