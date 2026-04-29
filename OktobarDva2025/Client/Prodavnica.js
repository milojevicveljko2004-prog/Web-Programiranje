
export class Prodavnica
{
    constructor(container) {
        this.container = container;
        this.kategorije = [];
    }

    async crtaj() {
        await this.crtajFormu();
        this.crtajProdavnicu();
    }

    async crtajFormu() {
        //izvlacimo formContainer
        const formContainer = this.container.querySelector(".formContainer");

        //crtamo

        //dodaj naslov
        const naslov = document.createElement("h3");
        naslov.textContent = "Upis proizvoda";
        formContainer.appendChild(naslov);

        //input i select polja
        const inputNaziv = this.dodajInput(formContainer, "inputNaziv", "Naziv: ", "text");
        const selectKategorija = this.dodajSelect(formContainer, "selectKategorija", "Kategorija: ");
        const inputCena = this.dodajInput(formContainer, "inputCena", "Cena: ", "number");
        const inputKolicina = this.dodajInput(formContainer, "inputKolicina", "Kolicina: ", "number");

        //popunjavanje select polja kategorijama
        await this.getKategorije(); //u niz this.kategorije ce biti kategorije

        for(const k of this.kategorije) {

            const opKategorija = document.createElement("option");
            opKategorija.value = k.id;
            opKategorija.textContent = `${k.naziv}`;
            selectKategorija.appendChild(opKategorija);
        }

        //button
        const button = document.createElement("button");
        button.textContent = "Dodaj proizvod";
        formContainer.appendChild(button);

        button.onclick = async() => {
            const result = await fetch("http://localhost:5043/Prodavnica/UpisProizvoda", {
                method: "POST",
                headers: {
                    "Content-type": "application/json",
                    "Accept:": "application/json"
                },
                body: JSON.stringify({  //value ili textContent???
                    naziv: inputNaziv.value,
                    kategorijaNaziv: selectKategorija.value,
                    cena: Number(inputCena.value),
                    kolicina: Number(inputKolicina.value),
                    idProdavnice: 1
                })
            });
        }

    }

    dodajInput(container, name, labeltext, type) {

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);
        
        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = labeltext;
        redForme.appendChild(label);

        const input = document.createElement("input");
        input.type = type;
        input.name = name;
        input.id = name;
        redForme.appendChild(input);

        return input;
    }

    dodajSelect(container, name, textLabel) {

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = textLabel;
        redForme.appendChild(label);

        const select = document.createElement("select");
        select.id = name;
        select.name = name;
        redForme.appendChild(select);

        return select;
    }

    async getKategorije() {
        try{
            const result = await fetch("http://localhost:5043/Prodavnica/PreuzmiKategorije");

            if(!result.ok) {
                const error = await result.text();
                console.error(error);
            }

            const data = await result.json();
            this.kategorije = data;

            // for(const d of data)
            // {
            //     this.kategorije.push(d);
            // }
        }
        catch(e) {
            console.log(`fetch() failed! ${e}`);
        }
    }

    crtajProdavnicu() {
        const shopContainer = this.container.querySelector(".shopContainer");

        //naslov
        const naslov = document.createElement("h3");
        naslov.textContent = "Prodavnica";
        naslov.classList.add("naslov");
        shopContainer.appendChild(naslov);


    }
}