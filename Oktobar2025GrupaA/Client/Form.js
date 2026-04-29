import {Statistika} from "./Statistika.js"

export class Form {
    constructor(container, prodKuca, statistikaContainer) {
        this.container = container; //formNaslovContainer
        this.prodKuca = prodKuca;
        this.statistikaContainer = statistikaContainer;
    }

    async crtaj() {
        //prvo naslov
        const naslovContainer = document.createElement("div");
        naslovContainer.classList.add("naslovContainer");
        this.container.appendChild(naslovContainer);

        const naslov = document.createElement("h2");
        naslov.classList.add("naslov");
        naslov.textContent = `${this.prodKuca.naziv}`;
        naslovContainer.appendChild(naslov);

        //sad forma
        const formContainer = document.createElement("div");
        formContainer.classList.add("formContainer");
        this.container.appendChild(formContainer);

        //alternativa: mozda ovde da kreiram container redForme i da ga prosledim???

        const selectKategorija = this.createSelect(formContainer, "selectKategorija", "Kategorija: ");
        const selectFilm = this.createSelect(formContainer, "selectFilm", "Film: ");
        const inputOcena = this.createInput(formContainer, "inputOcena", "Ocena: ", "number");

        this.popuniSelectPoljeKategorija(selectKategorija);
        await this.popuniSelectPoljeFilmova(selectFilm, selectKategorija.value);

        //u pocetku je popunjeno select polje kategorija i automatski je neka kategorija vec odabrana
        //prema tome odmah moze da se crta statistika
        const statistika = new Statistika(this.statistikaContainer, selectKategorija);
        await statistika.crtaj();

        selectKategorija.onchange = async() => {
            await this.popuniSelectPoljeFilmova(selectFilm, selectKategorija.value);

            const statistika = new Statistika(this.statistikaContainer, selectKategorija);
            await statistika.crtaj(); //kada se odabere kategorija, moze da se crta statistika
        }

        //button
        const btnContainer = document.createElement("div");
        btnContainer.classList.add("btnContainer");
        formContainer.appendChild(btnContainer);

        const button = document.createElement("button");
        button.classList.add("button");
        button.type = "submit";
        button.textContent = "Snimi ocenu";
        btnContainer.appendChild(button);

        button.onclick = async() => {
            const vrednostOcene = Number(inputOcena.value);
            const filmID = Number(selectFilm.value);

            const dto = {
                vrednost: vrednostOcene,
                filmID: filmID
            };

            try{
                const result = await fetch("http://localhost:5253/Film/DodajOcenu", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify(dto)
                });

                if(!result.ok) {
                    const errMessage = await result.text();
                    console.error(errMessage);
                }
                else{
                    console.log("fetch() za upis ocene radi.");
                    location.reload();
                }
            }
            catch(e) {
                console.error(e);
            }
        }
    }

    createSelect(container, name, lblText) {

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        label.name = name;
        redForme.appendChild(label);

        const select = document.createElement("select");
        select.name = name;
        select.classList.add(name);
        redForme.appendChild(select);

        return select;
    }

    createInput(container, name, lblText, type) { 

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        label.name = name;
        redForme.appendChild(label);

        const input = document.createElement("input");
        input.name = name;
        input.type = type;
        input.classList.add(name);
        redForme.appendChild(input);

        return input;
    }

    popuniSelectPoljeKategorija(select) {

        for(const p of this.prodKuca.kategorije) {

            const option = document.createElement("option");
            option.value = p.id;
            option.textContent = p.naziv;
            select.appendChild(option);
        }
    }

    async popuniSelectPoljeFilmova(select, id) {

        let listaFilmova = [];
        const idKategorije = Number(id);
        select.innerHTML = ""; //obrisi stare filmove

        if(idKategorije<=0 || !idKategorije)
            return;

        try{
            const result = await fetch(`http://localhost:5253/Film/VratiFilmovePoKategoriji/${idKategorije}`);

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }
            else{
                listaFilmova = await result.json();
                console.log("fetch() za filmove za select polje radi.");
            }
        }
        catch(e) {
            console.error(e);
        }

        for(const p of listaFilmova) {

            const option = document.createElement("option");
            option.value= p.id;
            option.textContent = p.naziv;
            select.appendChild(option);
        }
    }
}