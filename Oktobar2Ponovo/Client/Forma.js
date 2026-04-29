export class Forma {
    constructor(container, prodavnica) {
        this.container = container; //redContainer
        this.prodavnica = prodavnica;
        this.kategorije = [];
    }

    async crtaj() {
        const formContainer = document.createElement("div");
        formContainer.classList.add("formContainer");
        this.container.appendChild(formContainer);

        //crtanje elemenata:

        const naslovForma = document.createElement("h3");
        naslovForma.textContent = "Upis proizvoda";
        naslovForma.classList.add("h3");
        formContainer.appendChild(naslovForma);

        const inputNaziv = this.crtajInput(formContainer, "inputNaziv", "Naziv: ", "text");
        const selectKategorija = this.crtajSelect(formContainer, "selectKategorija", "Kategorija: ");
        const inputCena = this.crtajInput(formContainer, "inputCena", "Cena: ", "number");
        const inputKolicina = this.crtajInput(formContainer, "inputKolicina", "Kolicina: ", "number");

        //popuni select polje
        await this.pribaviKategorije();

        for(const k of this.kategorije) {
            const option = document.createElement("option");
            option.value = k.id; //id i naziv zato sto - gleda se kako se zovu polja koja vraca server!!! Mnogo bitno!
            option.textContent = k.naziv;
            option.classList.add("option");
            selectKategorija.appendChild(option);
        }

        const button = document.createElement("button");
        button.textContent = "Dodaj proizvod";
        button.type="button";
        button.classList.add("button");
        formContainer.appendChild(button);

        button.onclick = async () => {

            const naziv = inputNaziv.value.trim();
            const kategorijaId = Number(selectKategorija.value);
            const cena = Number(inputCena.value);
            const kolicina = Number(inputKolicina.value);
            const prodavnicaId = Number(this.prodavnica.id);

            const dto = {
                naziv: naziv,
                kategorijaId: kategorijaId,
                cena: cena,
                kolicina: kolicina,
                prodavnicaId: prodavnicaId
            };

            try {
                const result = await fetch("http://localhost:5083/Prodavnica/DodajProizvodUProdavnicu", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify(dto)
                });

                if(!result.ok) {
                    const errText = await result.text();
                    console.error(errText);
                }
                else{
                    console.log("fetch() za dodavanje proizvoda radi!");
                }

                location.reload() //refresh cele stranice
            }
             catch (e) {
                    console.error(e);
                    alert("Doslo je do greske pri slanju zahteva.");
                }
            };
        
        }

    crtajInput(container, name, textLabel, type) {

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = textLabel;
        redForme.appendChild(label);

        const input = document.createElement("input");
        input.id=name;
        input.name=name;
        input.type=type;
        redForme.appendChild(input);

        return input;
    }

    crtajSelect(container, name, textLabel) {

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = textLabel;
        redForme.appendChild(label);

        const select = document.createElement("select");
        select.id=name;
        select.name=name;
        redForme.appendChild(select);

        return select;
    }

    async pribaviKategorije() {
        try{
            const result = await fetch("http://localhost:5083/Prodavnica/PreuzmiKategorije");

            if(!result.ok) {
                const error = await result.text();
                console.error(error);
            }

            this.kategorije = await result.json();
        }
        catch(e)
        {
            console.log(e);
        }
    }
}