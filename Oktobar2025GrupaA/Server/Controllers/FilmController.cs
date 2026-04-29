using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmController : ControllerBase
    {
        public FilmContext Context {get; set;}

        public FilmController(FilmContext context)
        {
            this.Context=context;
        }

        [Route("DodajProdukcijskuKucu")]
        [HttpPost]
        public async Task<ActionResult> DodajProdukcijskuKucu([FromBody] ProdukcijskaKuca pk)
        {
            if(pk==null || string.IsNullOrWhiteSpace(pk.Naziv))
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                await Context.ProdukcijskeKuce.AddAsync(pk);
                await Context.SaveChangesAsync();

                return Ok($"Prod. kuca {pk.Naziv} je dodata u bazu. ID={pk.ID}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Route("DodajKategoriju")]
        [HttpPost]
        public async Task<ActionResult> DodajKategoriju([FromBody] DodajKategorijuDTO dto)
        {
            if(dto==null || string.IsNullOrWhiteSpace(dto.NazivKategorije) || dto.ProdakcijskaKucaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var prodKuca = await Context.ProdukcijskeKuce.FindAsync(dto.ProdakcijskaKucaID);
                if(prodKuca==null)
                {
                    return BadRequest($"Ne postoji produkcijska kuca sa ID={dto.ProdakcijskaKucaID}.");
                }

                var kategorija = new Kategorija
                {
                    Naziv = dto.NazivKategorije,
                    ProdukcijskaKuca = prodKuca
                };                 

                await Context.Kategorije.AddAsync(kategorija);
                await Context.SaveChangesAsync();

                return Ok($"Kategorija {kategorija.Naziv} je dodata u prod. kucu {kategorija.ProdukcijskaKuca.Naziv}. ID={kategorija.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        [Route("DodajFilm")]
        [HttpPost]
        public async Task<ActionResult> DodajFilm([FromBody] DodajFilmDTO dto)
        {
            if(dto==null || string.IsNullOrWhiteSpace(dto.NazivFilma) || dto.KategorijaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var kategorija = await Context.Kategorije.FindAsync(dto.KategorijaID);
                if(kategorija==null)
                {
                    return BadRequest($"Ne postoji kategorija sa ID={dto.KategorijaID} u bazi.");
                }

                var film = new Film
                {
                    Naziv = dto.NazivFilma,
                    Kategorija = kategorija
                };

                await Context.Filmovi.AddAsync(film);
                await Context.SaveChangesAsync();

                return Ok($"Film {film.Naziv} je dodat u kategoriju {film.Kategorija.Naziv}. ID={film.ID}");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Poziva se iz klijenta. Korisnik popuni formu za film i zeli da snimi ocenu
        [Route("DodajOcenu")]
        [HttpPost]
        public async Task<ActionResult> DodajOcenu([FromBody] DodajOcenuDTO dto)
        {
            if(dto==null || dto.FilmID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var film = await Context.Filmovi.FindAsync(dto.FilmID);
                if(film==null)
                {
                    return BadRequest($"Film sa ID={dto.FilmID} nije pronadjen u bazi.");
                }

                var ocena = new Ocena
                {
                    Vrednost = dto.Vrednost,
                    Film = film
                };

                await Context.Ocene.AddAsync(ocena);
                await Context.SaveChangesAsync();

                return Ok($"Ocena {ocena.Vrednost} je dodeljena filmu {ocena.Film.Naziv}.");
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //prvo cu za vezbu da napravim metodu koja vraca sve filmove
        //a onda cu da napravim metodu(ili vise metoda) koja vraca najbolje, srednje i najgore ocenjen film
        [Route("VratiFilmoveSaOcenama/{kategorijaID}")]
        [HttpGet]
        public async Task<ActionResult> VratiFilmove(int kategorijaID)
        {
            if(kategorijaID<=0)
            {
                return BadRequest("Nevalidni argument!");
            }

            try
            {
                var filmovi = await Context.Filmovi
                            .Include(s => s.Kategorija)
                            .Include(s => s.Ocene)
                            .Where(s => s.Kategorija!=null && s.Kategorija.ID == kategorijaID)
                            .ToListAsync();

                var filmoviSaProsOcenom = new List<Object>();
                //Ali iz rezultata hocu da vratim naziv filma, naziv kategorije i prosecnu ocenu za svaki film
                //Svaki film vec ima svoj naziv i kategoriju
                foreach(var p in filmovi) //svaki film ima listu ocena
                {
                    float prosecnaOcena;
                    if(p.Ocene==null || p.Ocene.Count() == 0) //ako film nema nijednu ocenu
                    {
                        prosecnaOcena=0;
                    }
                    else
                    {
                        var zbirOcena = p.Ocene!.Sum(s => s.Vrednost);
                        var brOcena = p.Ocene.Count();

                        prosecnaOcena = (float)zbirOcena/brOcena;
                    }

                        var objForList = new
                        {
                            NazivFilma = p.Naziv,
                            ProsecnaOcena = prosecnaOcena,
                            Kategorija = p.Kategorija!.Naziv
                        };
                        filmoviSaProsOcenom.Add(objForList);
                }

                return Ok(filmoviSaProsOcenom);           
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //metoda koja vraca sve produkcijske kuce zajedno sa kategorijama
        [Route("VratiSveProdKuce")]
        [HttpGet]
        public async Task<ActionResult> VratiKuce()
        {
            try
            {
                var result = await Context.ProdukcijskeKuce
                                .Include(s => s.Kategorije)
                                .ToListAsync();

                return Ok(result.Select(s => new
                {
                    ID = s.ID,
                    naziv = s.Naziv,
                    kategorije = s.Kategorije!.Select(p => new
                    {
                        id = p.ID,
                        naziv = p.Naziv,
                    })
                }).ToList());
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Metoda koja vraca najgori, najbolji i srednji film. Mora jedna metoda!
        //Na ispitu ako se dvoumis za jednu ili vise - uvek pravi jednu!
        [Route("FilmNajgoriSrednjiNajbolji/{kategorijaID}")]
        [HttpGet]
        public async Task<ActionResult> VratiFilmove2(int kategorijaID)
        {
            if(kategorijaID<=0)
            {
                return BadRequest("Nevalidni argument!");
            }

            try
            {
                var sviFilmoviKategorije = await Context.Filmovi
                                        .Include(s => s.Kategorija)
                                        .Include(s => s.Ocene)
                                        .Where(s => s.Kategorija != null && s.Kategorija.ID==kategorijaID)
                                        .ToListAsync();

                var listaFilmovaSaProsOcenama = new List<Object>();

                //prolazak kroz svaki film - racunanje njegove pros. ocene i smestanje u listu
                foreach(var p in sviFilmoviKategorije)
                {
                    float prosOcena = 0;
                    if(p.Ocene==null || p.Ocene.Count()==0)
                    {
                        prosOcena=0;
                    }
                    else
                    {
                        int zbirOcena = p.Ocene.Sum(s => s.Vrednost);
                        int brOcena = p.Ocene.Count();

                        prosOcena = (float)zbirOcena/brOcena;
                    }

                    var objForList = new
                    {
                        Naziv = p.Naziv,
                        Kategorija = p.Kategorija!.Naziv,
                        ProsecnaOcena = prosOcena
                    };

                    listaFilmovaSaProsOcenama.Add(objForList);
                }

                //sad kad imam listu filmova sa njihovim prosecnim ocenama trebam da nadjem najbolji, najgori i srednji 

                var sortiranaLista = listaFilmovaSaProsOcenama.OrderBy(s => ((dynamic)s).ProsecnaOcena).ToList();
                //od najmanjeg do najveceg. Mora ovako sa dynamic

                var najmanji = sortiranaLista.FirstOrDefault();
                var najveci = sortiranaLista.LastOrDefault();
                int IndexSrednji = sortiranaLista.Count() / 2;
                var srednji = sortiranaLista[IndexSrednji];

                var resultList = new List<Object>();
                resultList.Add(najmanji!);
                resultList.Add(srednji);
                resultList.Add(najveci!);

                return Ok(resultList);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //vrati filmove na osnovu kategorije - potrebno za popunjavanje select polja
        [Route("VratiFilmovePoKategoriji/{kategorijaID}")]
        [HttpGet]
        public async Task<ActionResult> VratiFilmoveeee(int kategorijaID)
        {
            if(kategorijaID<=0)
            {
                return BadRequest("Nevalidni argumenti!");
            }

            try
            {
                var kategorija = await Context.Kategorije.FindAsync(kategorijaID);
                if(kategorija==null)
                {
                    return BadRequest($"Kategorija sa ID={kategorijaID} ne postoji");
                }

                var result = await Context.Filmovi
                        .Where(s => s.Kategorija == kategorija)
                        .ToListAsync();

                return Ok(result);        
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}