export class Statistika {
    constructor(container, select) {
        this.container = container; //statistikaContainer
        this.select = select;
        this.filmovi = [];
    }

    async crtaj() {
        this.container.innerHTML = ""; //brisanje prethodnog sadrzaja
        await this.pribaviFilmove();

        for(const p of this.filmovi) {

            const filmContainer = document.createElement("div");
            filmContainer.classList.add("filmContainer");
            this.container.appendChild(filmContainer); //u ovom containeru treba da budu ocena, bar i nazivFilma jedan ispod drugog
        
            const ocenaFilma = document.createElement("p");
            ocenaFilma.classList.add("ocenaFilma");
            ocenaFilma.textContent = p.prosecnaOcena;
            filmContainer.appendChild(ocenaFilma);

            const barContainer = document.createElement("div");
            barContainer.classList.add("barContainer");
            barContainer.style.height = `${p.prosecnaOcena*20}px`;
            barContainer.style.backgroundColor = "black";
            filmContainer.appendChild(barContainer);

            const nazivFilma = document.createElement("p");
            nazivFilma.classList.add("nazivFilma");
            nazivFilma.textContent = p.nazivFilma;
            filmContainer.appendChild(nazivFilma);
        }
    }

    async pribaviFilmove() {
        try{
            //ovaj fetch treba po tekstu zadatka: http://localhost:5253/Film/FilmNajgoriSrednjiNajbolji/{kategorijaID}
            //ali za vezbu moze i: http://localhost:5253/Film/VratiFilmoveSaOcenama/${Number(this.select.value)}
            const result = await fetch(`http://localhost:5253/Film/FilmNajgoriSrednjiNajbolji/${Number(this.select.value)}`);

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }
            else {
                this.filmovi = await result.json();
                console.log("fetch() za statistiku radi.");
            }
        }
        catch(e) {
            console.error(e);
        }
    }
}