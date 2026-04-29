using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OktobarTri2025.DTOs;

namespace OktobarTri2025.Controllers
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

        [Route("DodajProizvod")]
        [HttpPost]
        public async Task<ActionResult> DodajProizvod([FromBody] ProizvodDTO dto)
        {
            if(dto==null || string.IsNullOrEmpty(dto.Naziv))
            {
                return BadRequest("Prosledjeni podaci su neispravni!");
            }

            try
            {
                //prvo provera da li postoji kategorija koja je specifirana
                var postoji = await Context.Kategorije.FindAsync(dto.IDKategorije);
                if(postoji == null)
                {
                    return BadRequest($"Ne postoji kategorija sa ID = {dto.IDKategorije}.");
                }

                var proizvod = new Proizvod
                {
                    Naziv = dto.Naziv,
                    kategorija = postoji
                };

                //await Context.Proizvodi.AddAsync(proizvod);
                Context.Proizvodi.Add(proizvod);
                await Context.SaveChangesAsync();

                return Ok($"Proizvod ID={proizvod.ID} je uspesno dodat u bazu!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ObrisiProizvod/{id}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiProizvod(int id)
        {
            try
            {
                var zaBrisanje = await Context.Proizvodi.FindAsync(id);
                if(zaBrisanje==null)
                {
                    return BadRequest($"Proizvod sa ID={id} ne postoji!");
                }

                Context.Proizvodi.Remove(zaBrisanje);
                await Context.SaveChangesAsync();

                return Ok($"Proizvod sa ID={id} je obrisan!");
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
                return BadRequest("Prosledjeni podaci su neispravni!");
            }

            try
            {
                await Context.Kategorije.AddAsync(kategorija);
                await Context.SaveChangesAsync();

                return Ok($"Kategorija ID={kategorija.ID} je uspesno dodata u bazu!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("PreuzmiKategorije")]
        [HttpGet]
        public async Task<ActionResult> PreuzmiKategorije()
        {
            var kategorije = await Context.Kategorije.ToListAsync();
            return Ok(kategorije);
        }

        [Route("DodajProdavnicu")]
        [HttpPost]
        public async Task<ActionResult> DodajProdavnicu([FromBody] Prodavnica prodavnica)
        {
            if(prodavnica==null || string.IsNullOrWhiteSpace(prodavnica.Naziv))
            {
                return BadRequest("Prosledjeni podaci su neispravni!");
            }

            try
            {
                await Context.Prodavnice.AddAsync(prodavnica);
                await Context.SaveChangesAsync();

                return Ok($"Prodavnica ID={prodavnica.ID} je dodata u bazu!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ObrisiKategoriju/{id}")]
        [HttpDelete]
        public async Task<ActionResult> ObrisiKategoriju(int id)
        {
            try
            {
                var zaBrisanje = await Context.Kategorije.FindAsync(id);
                if(zaBrisanje==null)
                {
                    return BadRequest($"Kategorija sa ID={id} ne postoji!");
                }

                Context.Kategorije.Remove(zaBrisanje);
                await Context.SaveChangesAsync();

                return Ok($"Kategorija sa ID={id} je obrisana!");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Dodavanje novog proizvoda u prodavnicu
        [Route("DodajProizvodUProdavnicu/{idProizvod}/{idProdavnica}/{Cena}/{Kolicina}")]
        [HttpPost]
        public async Task<ActionResult> DodajProizvodUProdavnicu(int idProizvod, int idProdavnica, double Cena, int Kolicina)
        {
            if(idProizvod<0 || idProdavnica<0 || Cena<0 || Kolicina<0)
            {
                return BadRequest("Neispravni argumenti!");
            }

            try
            {         
                var proizvod = await Context.Proizvodi.FindAsync(idProizvod);
                var prodavnica = await Context.Prodavnice.FindAsync(idProdavnica);

                if(proizvod==null)
                {
                    return BadRequest($"Proizvod sa ID={idProizvod} ne postoji!");
                }

                if(prodavnica==null)
                {
                    return BadRequest($"Prodavnica sa ID={idProdavnica} ne postoji!");
                }

                var proizvodUProdavnici = new ProizvodUProdavnici
                {
                    proizvod = proizvod,
                    prodavnica = prodavnica,
                    Cena = Cena,
                    Kolicina = Kolicina
                };

                //prvo treba izracunati ukupnu kolicinu da bi se proverilo da li ce ukupna+nova da premasi 100
                var ukupnaKolicina = Context.ProizvodiUProdavnici
                                .Where(p => p.prodavnica.ID == idProdavnica)
                                .Sum(p => p.Kolicina);   

                if(ukupnaKolicina + Kolicina > 100)
                {
                    return BadRequest("Maksimalan broj proizvoda u prodavnici je 100!");
                }    

                await Context.ProizvodiUProdavnici.AddAsync(proizvodUProdavnici);
                await Context.SaveChangesAsync();

                return Ok($"Proizvod {proizvodUProdavnici.proizvod.Naziv} je dodat u prodavnicu {proizvodUProdavnici.prodavnica.Naziv}! ID = {proizvodUProdavnici.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("ProdajProizvod/{idProizvoda}/{kolicinaProizvoda}")]
        [HttpPut]
        public async Task<ActionResult> ProdajProizvod(int idProizvoda, int kolicinaProizvoda)
        {
            if(idProizvoda<0 || kolicinaProizvoda<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var zaProdaju = await Context.ProizvodiUProdavnici.FindAsync(idProizvoda);
                if(zaProdaju==null)
                {
                    return BadRequest($"Proizvod sa ID={idProizvoda} ne postoji u bazi.");
                }

                //proizvod u odredjenoj prodavnici ima kolicinu. Mora da bude veca od kolicine koja treba da se proda.
                if(zaProdaju.Kolicina < kolicinaProizvoda)
                {
                    return BadRequest($"Trazi se veca kolicina proizvoda nego sto ih je u prodavnici!");
                }

                zaProdaju.Kolicina -= kolicinaProizvoda;
                await Context.SaveChangesAsync();

                return Ok($"Uspesno prodato {kolicinaProizvoda} proizvoda.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //vraca sve prodavnice sa njihovim proizvodima
        [Route("VratiSveProdavnice")]
        [HttpGet]
        public async Task<ActionResult> VratiProdavnice()
        {
            try
            {
                var result = await Context.Prodavnice
                            .Include(p => p.ProizvodiUProdavnici)
                            .ThenInclude(pp => pp.proizvod)
                            .ThenInclude(pr => pr.kategorija)
                            .ToListAsync();


                return Ok(result);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Druga metoda - detaljnije vraca. Prepisano od ChatGPT
        [Route("PreuzmiProdavnice")]
[HttpGet]
public async Task<ActionResult> PreuzmiProdavnice()
{
    try
    {
        var prodavnice = await Context.Prodavnice
            .Include(p => p.ProizvodiUProdavnici)
                .ThenInclude(pp => pp.proizvod)
                    .ThenInclude(pr => pr.kategorija)
            .ToListAsync();

        var rezultat = prodavnice.Select(p => new ProdavnicaDTO
        {
            ID = p.ID,
            Naziv = p.Naziv,
            Lokacija = p.Lokacija,
            BrTelefona = p.BrTelefona,
            Proizvodi = p.ProizvodiUProdavnici != null
                ? p.ProizvodiUProdavnici.Select(pp => new ProizvodZaPrikazDTO
                {
                    IDProizvodUProdavnici = pp.ID,
                    IDProizvoda = pp.proizvod != null ? pp.proizvod.ID : 0,
                    NazivProizvoda = pp.proizvod != null ? pp.proizvod.Naziv : "",
                    Kategorija = pp.proizvod != null && pp.proizvod.kategorija != null
                        ? pp.proizvod.kategorija.Naziv
                        : "",
                    Cena = pp.Cena,
                    Kolicina = pp.Kolicina
                }).ToList()
                : new List<ProizvodZaPrikazDTO>()
        }).ToList();

        return Ok(rezultat);
    }
    catch (Exception e)
    {
        return BadRequest(e.Message);
    }
}

        // [Route("VratiIdProizvoda/{nazivProizvoda}")]
        // [HttpGet]
        // public async Task<ActionResult> VratiIdProizvoda(String nazivProizvoda)
        // {
        //     if(string.IsNullOrWhiteSpace(nazivProizvoda))
        //     {
        //         return BadRequest("Nevalidni argumenti!");
        //     }

        //     try
        //     {
        //         var proizvod = await Context.Proizvodi.FindAsync(nazivProizvoda);
        //         if(proizvod==null)
        //         {
        //             return BadRequest($"Ne postoji proizvod sa imenom {nazivProizvoda}");
        //         }

        //         return Ok(proizvod.ID);
        //     }
        //     catch(Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

        [Route("UpisProizvoda/{naziv}/{kategorijaNaziv}/{cena}/{kolicina}/{idProdavnice}")]
        [HttpPost] //idProdavnice se ne dobija iz forme
        public async Task<ActionResult> UpisiProizvod(String naziv, String kategorijaNaziv, double cena, int kolicina, int idProdavnice)
        {
            if(string.IsNullOrWhiteSpace(naziv) || string.IsNullOrWhiteSpace(kategorijaNaziv) || cena<0 || kolicina<0 || idProdavnice<0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                //zapravo treba dodati novi proizvod u prodavnicu, ali na osnovu ovih argumenata
                //Ideja: Na osnovu ovih podataka nadjem idProizvoda i onda imam sve argumente da bi pozvao onu prethodnu metodu za dodavanje proizvoda u prodavnicu
            
                var proizvod = await Context.Proizvodi.FirstOrDefaultAsync(p => p.Naziv == naziv);
                if(proizvod==null)
                {
                    return BadRequest($"Ne postoji proizvod sa imenom {naziv}");
                }
                int idProizvoda = proizvod.ID;

                var kategorija = await Context.Kategorije.FirstOrDefaultAsync(p => p.Naziv == kategorijaNaziv);
                if(kategorija==null)
                {
                    return BadRequest($"Ne postoji kategorija sa imenom {kategorijaNaziv}");
                }
                int idKategorije = kategorija.ID;

                var prodavnica = await Context.Prodavnice.FindAsync(idProdavnice);
                if(prodavnica==null)
                {
                    return BadRequest($"Ne postoji prodavnica sa ID-jem {idProdavnice}");
                }

                var noviProizvodUProdavnici = new ProizvodUProdavnici
                {
                    proizvod = proizvod,
                    prodavnica = prodavnica,
                    Cena = cena,
                    Kolicina = kolicina
                };

                await Context.ProizvodiUProdavnici.AddAsync(noviProizvodUProdavnici);
                await Context.SaveChangesAsync();

                return Ok($"Novi proizvod {naziv} je dodat u prodavnicu {idProdavnice}, ID={noviProizvodUProdavnici.ID}");
                
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}