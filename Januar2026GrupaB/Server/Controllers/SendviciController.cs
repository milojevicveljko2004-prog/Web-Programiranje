using System.IO.Compression;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("controller")]
    public class SendviciController : ControllerBase
    {
        public SendviciContext Context {get; set;}

        public SendviciController(SendviciContext context)
        {
            this.Context = context;
        }

        [Route("DodajProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
        {
            if(string.IsNullOrWhiteSpace(prodavnica.Naziv))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Prodavnice.AddAsync(prodavnica);
                await Context.SaveChangesAsync();

                return Ok($"Prodavnica {prodavnica.Naziv} je dodata u bazu ID={prodavnica.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajMestoUProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajMesto([FromBody] DodajMestoDTO dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Naziv) || dto.ProdavnicaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var prodavnica = await Context.Prodavnice.FindAsync(dto.ProdavnicaID);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica da ID={dto.ProdavnicaID} ne postoji u bazi.");
                }

                var mesto = new Mesto
                {
                    Naziv = dto.Naziv,
                    Prodavnica = prodavnica
                };

                await Context.Mesta.AddAsync(mesto);
                await Context.SaveChangesAsync();

                return Ok($"Mesto {mesto.Naziv} je dodato u bazu. ID={mesto.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajSastojak")]
        [HttpPost]
        public async Task<ActionResult> DodajSastojak([FromBody] Sastojak sastojak)
        {
            if(string.IsNullOrWhiteSpace(sastojak.Naziv))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Sastojci.AddAsync(sastojak);
                await Context.SaveChangesAsync();

                return Ok($"Sastojak {sastojak.Naziv} je dodat u bazu. ID={sastojak.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Dodaj sastojak u prodavnicu. Ali ne i u mesto! Posebna metoda je za to...
        [Route("DodajSastojakUProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajSastojakUProdavnicu([FromBody] SastojakZaProdavnicuDTO dto)
        {
            if(dto.Cena<0 || dto.Kolicina<0 || dto.ProdavnicaID<=0 || dto.SastojakID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var prodavnica = await Context.Prodavnice.FindAsync(dto.ProdavnicaID);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={dto.ProdavnicaID} ne postoji u bazi.");
                }

                var sastojak = await Context.Sastojci.FindAsync(dto.SastojakID);
                if(sastojak==null)
                {
                    return BadRequest($"Sastojak sa ID={dto.SastojakID} ne postoji u bazi.");
                }

                var sastojakZaProdavnicu = new SastojakUProdavnici
                {
                    Prodavnica = prodavnica,
                    Sastojak = sastojak,
                    Kolicina = dto.Kolicina,
                    Cena = dto.Cena
                };

                await Context.SastojciUProdavnici.AddAsync(sastojakZaProdavnicu);
                await Context.SaveChangesAsync();

                return Ok($"Sastojak {sastojakZaProdavnicu.Sastojak.Naziv} je dodat u prodavnicu {sastojakZaProdavnicu.Prodavnica.Naziv}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //vrati sva mesta - potrebno za select polje na klijentu
        [Route("VratiMestaProdavnice/{prodavnicaID}")]
        [HttpGet]
        public async Task<ActionResult> VratiMestaProdavnice(int prodavnicaID)
        {
            if(prodavnicaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var prodavnica = await Context.Prodavnice.FindAsync(prodavnicaID);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={prodavnicaID} ne postoji u bazi.");
                }

                var result = await Context.Mesta
                            .Include(s => s.Prodavnica)
                            .Where(s => s.Prodavnica!=null && s.Prodavnica==prodavnica)
                            .ToListAsync();

                return Ok(result);            
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //vrati sastojke koji su u prodavnici - potrebno za popunjavanje select polja na klijentu
        [Route("VratiSastojkeUProdavnici/{prodavnicaID}")]
        [HttpGet]
        public async Task<ActionResult> VratiSastojkeUProdavnici(int prodavnicaID)
        {
           if(prodavnicaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var prodavnica = await Context.Prodavnice.FindAsync(prodavnicaID);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={prodavnicaID} ne postoji u bazi.");
                }

                var result = await Context.SastojciUProdavnici
                                .Include(s => s.Prodavnica)
                                .Where(s => s.Prodavnica!=null && s.Prodavnica==prodavnica)
                                .Include(s => s.Sastojak)
                                .ToListAsync();


                return Ok(result);                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //na klijentu je ovo za dugme dodaj
        [Route("DodajSastojakUSendvic")] //odnosno mesto. 1 mesto=1 sendvic
        [HttpPost]
        public async Task<ActionResult> DodajSastojakUSendvic([FromBody] SastojakZaSendvicDTO dto)
        {
            if(dto.Kolicina<0 || dto.MestoID<=0 || dto.SastojakUProdavniciID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var mesto = await Context.Mesta.FindAsync(dto.MestoID);
                if(mesto==null)
                {
                    return BadRequest($"Mesto sa ID={dto.MestoID} ne postoji u bazi.");
                }

                var sastojakUProdavnici = await Context.SastojciUProdavnici.FindAsync(dto.SastojakUProdavniciID);
                if(sastojakUProdavnici==null)
                {
                    return BadRequest($"Sastojak u prodavnici sa ID={dto.SastojakUProdavniciID} ne postoji u bazi.");
                }

                var sastojakUSendvicu = new SastojakUSendvicu
                {
                    Kolicina = dto.Kolicina,
                    Mesto = mesto,
                    SastojakUProdavnici = sastojakUProdavnici
                };

                await Context.SastojciUSendvicu.AddAsync(sastojakUSendvicu);
                await Context.SaveChangesAsync();

                return Ok($"Sastojak je dodat u sendvic {mesto.Naziv}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiCenuSendvica/{mestoID}")]
        [HttpGet] 
        public async Task<ActionResult> VratiCenu(int mestoID)
        {
            if(mestoID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                //da prodjem kroz sve sastojke koji su u njemu i saberem cene

                var mesto = await Context.Mesta! //za odredjeno mesto
                                    .Include(s => s.SastojciUSendvicu!)
                                    .ThenInclude(r => r.SastojakUProdavnici) //cena je tek ovde, pa zato mora
                                    .Where(s => s.ID==mestoID)
                                    .FirstOrDefaultAsync();

                if (mesto == null)
                {
                    return BadRequest($"Ne postoji mesto sa ID={mestoID}.");
                }

                int cena=0;
                foreach(var p in mesto.SastojciUSendvicu!) //za svaki sastojak sendvica
                {
                    cena += p.SastojakUProdavnici!.Cena;
                }

                mesto.UkupnaCena = cena;
                await Context.SaveChangesAsync();

                return Ok(cena);                   
                                    
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiCenuProdavnice/{idProdavnice}")]
        [HttpGet]
        public async Task<ActionResult> VratiCenuProdavnice(int idProdavnice)
        {
            if(idProdavnice<=0)
            {
                return BadRequest("Nevalidni argumenti");
            }

            try
            {
                var prodavnica = await Context.Prodavnice
                                .Include(s => s.Mesta)
                                .Where(s => s.ID==idProdavnice)
                                .FirstOrDefaultAsync();
                
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={idProdavnice} ne postoji.");
                }                

                //cenu dobijam tako sto prodjem kroz sva mesta i saberem cene svih njih. Svako mesto ima ukunu cenu.
                int ukupnaCena=0;

                if(prodavnica.Mesta==null || prodavnica.Mesta.Count()==0)
                {
                    return Ok(ukupnaCena);
                }

                foreach(var p in prodavnica.Mesta)
                {
                    ukupnaCena += p.UkupnaCena;
                }

                return Ok(ukupnaCena);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("VratiSveProdavnice")]
        [HttpGet]
        public async Task<ActionResult> VratiSveProdavnice()
        {
            try
            {
                var result = await Context.Prodavnice
                            .Include(s => s.SastojciUProdavnici!)
                            .ThenInclude(p => p.Sastojak!)
                            .Include(s => s.Mesta!)
                            .ThenInclude(p => p.SastojciUSendvicu!)
                            .ThenInclude(z => z.SastojakUProdavnici)
                            .ThenInclude(n => n!.Sastojak)
                            .ToListAsync();

                return Ok(result.Select(s => new
                {
                    ID = s.ID,
                    Naziv = s.Naziv,
                    SastojciUProdavnici = s.SastojciUProdavnici!.Select(p => new
                    {
                        ID = p.ID,
                        Cena = p.Cena,
                        Kolicina = p.Kolicina,
                        Sastojak = p.Sastojak,
                    }).ToList(),
                    Mesta = s.Mesta!.Select(r => new
                    {
                        ID = r.ID,
                        Naziv = r.Naziv,
                        UkupnaCena = r.UkupnaCena,
                        SastojciUSendvicu = r.SastojciUSendvicu!.Select(z => new
                        {
                            ID = z.ID,
                            Kolicina = z.Kolicina,
                            SastojakUProdavnici = z.SastojakUProdavnici
                        }).ToList()
                    }).ToList()
                }));            

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // [Route("Isporuci")]
        // [HttpPut]
        // public async Task<ActionResult> Isporuci()
    }
}