export class Form {
    constructor(container, prodavnica) {
        this.container = container; //formDesniDeoContainer
        this.prodavnica = prodavnica;
    }

    async crtaj() {
        //1. Crta formu

        const formContainer = document.createElement("div");
        formContainer.classList.add("formContainer");
        this.container.appendChild(formContainer);

        //mesto, sastojak, kolicina
        const mestoSelect = this.createSelect(formContainer, "mestoSelect", "Mesto: ");
        const sastojakSelect = this.createSelect(formContainer, "sastojakSelect", "Sastojak: ");
        const kolicinaInput = this.createInput(formContainer, "kolicinaInput", "Kolicina: ", "number");

        this.popuniSelectMesto(mestoSelect);
        this.popuniSelectSastojak(sastojakSelect);

        //button
        const button = document.createElement("button");
        button.classList.add("button");
        button.textContent = "Dodaj sastojak";
        formContainer.appendChild(button);

        button.onclick = async() => {
            const mestoID = Number(mestoSelect.value);
            const sastojakUProdavniciID = Number(sastojakSelect.value);
            const kolicina = Number(kolicinaInput.value);

            const dto = {
                mestoID: mestoID,
                sastojakUProdavniciID: sastojakUProdavniciID,
                kolicina: kolicina
            };

            try{
                const result = await fetch("http://localhost:5083/controller/DodajSastojakUSendvic", {
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
                    return;
                }
                else {
                    console.log("fetch() za dugme dodaj radi.");
                }

                location.reload();
            }
            catch(e) {
                console.error(e);
            }
        }

        this.crtajDesniDeo();
    }

    createSelect(container, name, lblText) {

        const redContainer = document.createElement("div");
        redContainer.classList.add("redContainer");
        container.appendChild(redContainer);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        label.classList.add("label");
        redContainer.appendChild(label);

        const select = document.createElement("select");
        select.classList.add("select");
        select.name = name;
        redContainer.appendChild(select);

        return select;
    }

    createInput(container, name, lblText, type) {

        const redContainer = document.createElement("div");
        redContainer.classList.add("redContainer");
        container.appendChild(redContainer);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        label.classList.add("label");
        redContainer.appendChild(label);

        const input = document.createElement("input");
        input.classList.add("input");
        input.name = name;
        redContainer.appendChild(input);

        return input;
    }

    popuniSelectMesto(select) {
        for(const p of this.prodavnica.mesta) {

            const option = document.createElement("option");
            option.value = p.id;
            option.textContent = p.naziv;
            select.appendChild(option);
        }
    }

    popuniSelectSastojak(select) {
        for(const p of this.prodavnica.sastojciUProdavnici) {

            const option = document.createElement("option");
            option.value = p.id;
            option.textContent = p.sastojak.naziv;
            select.appendChild(option);
        }
    }

    crtajDesniDeo() {
        
        const desniDeoContainer = document.createElement("div"); //u njemu ce biti sva mesta
        desniDeoContainer.classList.add("desniDeoContainer");
        this.container.appendChild(desniDeoContainer);

        for(const p of this.prodavnica.mesta) {

            const mestoContainer = document.createElement("div"); //mesto - u njemu ce biti naziv, sastojci, cena, button
            mestoContainer.classList.add("mestoContainer");
            desniDeoContainer.appendChild(mestoContainer);

            const nazivMesta = document.createElement("h3");
            nazivMesta.classList.add("nazivMesta");
            nazivMesta.textContent = p.naziv;
            mestoContainer.appendChild(nazivMesta);

            //sastojci
            for(const r of p.sastojciUSendvicu) {

                const sastojakContainer = document.createElement("div");
                sastojakContainer.classList.add("sastojakContainer");
                sastojakContainer.textContent = `${r.sastojakUProdavnici.sastojak.naziv}`;
                //sastojakContainer.style.height = `${r.cena}px`; //po ceni, ali nije bas pravilno po tekstu zadatka
                mestoContainer.appendChild(sastojakContainer);
            }

            const cenaHamburgera = document.createElement("h2");
            cenaHamburgera.classList.add("cenaHamburgera");
            cenaHamburgera.textContent = p.ukupnaCena;
            mestoContainer.appendChild(cenaHamburgera);

            const buttonNaruci = document.createElement("button");
            buttonNaruci.classList.add("buttonNaruci");
            buttonNaruci.textContent = "Naruci";
            mestoContainer.appendChild(buttonNaruci);

            buttonNaruci.onclick = async() => { //azurira se zarada prodavnice za taj dan

            }
        }
    }
}