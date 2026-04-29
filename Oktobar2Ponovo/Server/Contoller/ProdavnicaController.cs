using System.IO.Compression;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controller
{
    [ApiController]
    [Route("[controller]")]
    public class ProdavnicaController : ControllerBase
    {
        public ProdavnicaContext Context {get; set;}

        public ProdavnicaController(ProdavnicaContext context)
        {
            Context = context;
        }

        //prvo su potrebne metode za dodavanje kategorije i prodavnice jer one moraju nekako da postoje u bazi
        //ali nece biti posebne metode DodajProizvod() i dodajProizvodUProdavnicu() nego ce jedna metoda da radi obe stvari
        //zato sto treba da se poklopi sa formom na slici!

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

                return Ok($"Prodavnica {prodavnica.Naziv} je dodata u bazu. ID = {prodavnica.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajKategoriju")]
        [HttpPost]
        public async Task<ActionResult> DodajKategoriju([FromBody] Kategorija kategorija)
        {
            if(kategorija==null || string.IsNullOrWhiteSpace(kategorija.Naziv))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.Kategorije.AddAsync(kategorija);
                await Context.SaveChangesAsync();

                return Ok($"Kategorija {kategorija.Naziv} je dodata u bazu. ID = {kategorija.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //metoda koja istovremeno dodaje u bazu i proizvod i proizvodUProdavnici
        [Route("DodajProizvodUProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajProizvod([FromBody] DodajProizvodDTO dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Naziv) || dto.KategorijaID<0 || dto.Cena<0 || dto.Kolicina<0 || dto.prodavnicaId<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            if(dto.Kolicina>100)
            {
                return BadRequest("Velicina ne sme biti veca od 100!");
            }

            try
            {
                //prvo cu da dodam proizvod u bazu, ako vec ne postoji
                var kategorija = await Context.Kategorije.FindAsync(dto.KategorijaID);
                if(kategorija==null)
                {
                    return BadRequest($"Kategorija {dto.KategorijaID} ne postoji u bazi!");
                }

                var proizvod = await Context.Proizvodi.FirstOrDefaultAsync(p => p.Naziv == dto.Naziv && p.kategorija==kategorija);
                if(proizvod==null) //ne postoji
                {
                    proizvod = new Proizvod
                    {
                        Naziv = dto.Naziv,
                        kategorija = kategorija  
                    };

                    await Context.Proizvodi.AddAsync(proizvod);
                    await Context.SaveChangesAsync();

                    //return Ok($"Dodat proizvod {naziv} u bazu(ali ne jos u prodavnicu). ID = {proizvod.ID}.");
                }

                //sad proizvod dodaj u prodavnicu.

                var prodavnica = await Context.Prodavnice.FindAsync(dto.prodavnicaId);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID = {dto.prodavnicaId} ne postoji u bazi!");
                }

                //da li proizvod vec postoji u prodavnici
                var proizvodUProdavnici = await Context.ProizvodiUProdaji.FirstOrDefaultAsync(p => p.prodavnica == prodavnica && p.proizvod == proizvod
                                                        && p.Cena == dto.Cena); //kolicina nije u proveri jer ona moze da se azurira. Cena ne moze da se azurira

                //ako vec postoji u prodavnici samo mu povecaj kolicinu
                if(proizvodUProdavnici!=null)
                {
                    if(proizvodUProdavnici.Kolicina + dto.Kolicina > 100)
                    {
                        return BadRequest($"Greska. Kolicina proizvoda prevazilazi 100!");
                    }
                    proizvodUProdavnici.Kolicina += dto.Kolicina;
                    await Context.SaveChangesAsync();

                    return Ok($"Proizvod {dto.Naziv} vec postoji u bazi. ID = {proizvod.ID}.");
                }
                else //ako ne postoji u prodavnici, dodaj ga. Validacija za kolicinu>100 je vec obezbedjena na pocetku
                {
                    proizvodUProdavnici = new ProizvodUProdavnici
                    {
                        Cena = dto.Cena,
                        Kolicina = dto.Kolicina,
                        prodavnica = prodavnica,
                        proizvod = proizvod
                    };

                    await Context.ProizvodiUProdaji.AddAsync(proizvodUProdavnici);
                    await Context.SaveChangesAsync();
                    return Ok($"Proizvod {dto.Naziv} dodat u bazu. ID = {proizvod.ID}.");
                }                                        

            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ProdajProizvod")] //azurirace kolicinu
        [HttpPut]
        public async Task<ActionResult> ProdajProizvod([FromBody] ProdajProizvodDTO dto)
        {
            if(dto.Kolicina<0 || dto.ProizvodID<0 || dto.ProdavnicaID<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var proizvod = await Context.Proizvodi.FindAsync(dto.ProizvodID);
                if(proizvod==null)
                {
                    return BadRequest($"Proizvod sa ID={dto.ProizvodID} ne postoji u bazi!");
                }

                var prodavnica = await Context.Prodavnice.FindAsync(dto.ProdavnicaID);
                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={dto.ProdavnicaID} ne postoji u bazi!");
                }

                var proizvodUProdavnici = await Context.ProizvodiUProdaji.FirstOrDefaultAsync(
                                                p => p.proizvod == proizvod &&
                                                p.prodavnica == prodavnica);

                if(proizvodUProdavnici==null)   
                {
                    return BadRequest($"Greska! Trazeni proizvod {proizvod.Naziv} ne postoji u prodavnici {prodavnica.Naziv}");
                }

                if(proizvodUProdavnici.Kolicina<dto.Kolicina)
                {
                    return BadRequest($"Trenutni broj proizvoda u bazi je manji od {dto.Kolicina}");
                }
                else
                {
                    proizvodUProdavnici.Kolicina -= dto.Kolicina;
                    await Context.SaveChangesAsync();
                    return Ok($"Kupili ste {dto.Kolicina} proizvoda. Preostali broj proizvoda u prodavnici je {proizvodUProdavnici.Kolicina}.");
                }                             
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("BrisiProizvod/{id}")]
        [HttpDelete]
        public async Task<ActionResult> BrisiProizvod(int id)
        {
            if(id<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var proizvodZaBrisanje = await Context.ProizvodiUProdaji.FindAsync(id);
                if(proizvodZaBrisanje==null)
                {
                    return BadRequest($"Proizvod sa id={id} ne postoji u bazi!");
                }

                Context.ProizvodiUProdaji.Remove(proizvodZaBrisanje);
                await Context.SaveChangesAsync();
                return Ok($"Proizvod id={proizvodZaBrisanje.ID} je obrisan iz baze.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //preuzimanje kategorija
        [Route("PreuzmiKategorije")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiKategorije()
        {
            try
            {
                var result = await Context.Kategorije.ToListAsync();
                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //preuzimanje prodavnica
        [Route("PreuzmiProdavniceSaProizvodima")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiProdavnice()
        {
            try
            {
                var result = await Context.Prodavnice
                                    .Include(s => s.proizvodiUProdavnici)
                                    .ThenInclude(p => p.proizvod)
                                    .ThenInclude(k => k.kategorija)
                            .Select(s => new
                            {
                                ID = s.ID,
                                Naziv = s.Naziv,
                                BrojTelefona = s.BrojTelefona,
                                Lokacija = s.Lokacija,
                                ProizvodUProdavnici = s.proizvodiUProdavnici.Select(p => new
                                {
                                    ID = p.ID,
                                    Cena = p.Cena,
                                    Kolicina = p.Kolicina,
                                    proizvod = new
                                    {
                                        ID = p.proizvod.ID,
                                        Naziv = p.proizvod.Naziv,
                                        Kategorija = p.proizvod.kategorija.Naziv
                                    }
                                })
                            }).ToListAsync();

                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}