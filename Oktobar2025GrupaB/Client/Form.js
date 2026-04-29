export class Form {
    constructor(container, prodavnica) {
        this.container = container; //formNaslovContainer
        this.prodavnica = prodavnica;
        this.sastojci = [];
    }

    async crtaj() {

        const naslov = document.createElement("h3");
        naslov.classList.add("naslov");
        naslov.textContent = `Odabir sastojaka ${this.prodavnica.naziv}:`;
        this.container.appendChild(naslov);

        const formContainer = document.createElement("div");
        formContainer.classList.add("formContainer");
        this.container.appendChild(formContainer);

        //sastojak, hamburger, kolicina
        const sastojakSelect = this.createSelect(formContainer, "sastojakSelect", "Sastojak: ");
        const hamburgerSelect = this.createSelect(formContainer, "hamburgerSelect", "Hamburger: ");
        const kolicinaInput = this.createInput(formContainer, "kolicinaInput", "Kolicina: ", "number");

        this.popuniSelectZaHamburgere(hamburgerSelect);
        await this.popuniSelectZaSastojke(sastojakSelect);

        // hamburgerSelect.onchange = async () => {
        //     await this.popuniSelectZaSastojke(sastojakSelect);
        // }

        //button
        const button = document.createElement("button");
        button.classList.add("button");
        button.textContent = "Dodaj";
        formContainer.appendChild(button);

        button.onclick = async() => {

            try{
                const sastojakID = Number(sastojakSelect.value);
                const hamburgerID = Number(hamburgerSelect.value);
                const kolicina = Number(kolicinaInput.value);

                const dto =  {
                    sastojakID: sastojakID,
                    hamburgerID: hamburgerID,
                    kolicina: kolicina
                };

                const result = await fetch("http://localhost:5142/Hamburger/DodajSastojakUHamburger", {
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
                else {
                    console.log("fetch() za dugme dodaj radi.");
                    location.reload();
                }
            }
            catch(e) {
                console.error(e);
            }
        }

    }

    createSelect(container, name, lbltext) {

        const redContainer = document.createElement("div");
        redContainer.classList.add("redContainer");
        container.appendChild(redContainer);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lbltext;
        redContainer.appendChild(label);

        const select = document.createElement("select");
        select.name = name;
        select.classList.add("select");
        redContainer.appendChild(select);

        return select;
    }

    createInput(container, name, lbltext, type) {

        const redContainer = document.createElement("div");
        redContainer.classList.add("redContainer");
        container.appendChild(redContainer);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lbltext;
        redContainer.appendChild(label);

        const input = document.createElement("input");
        input.classList.add("input");
        input.name = name;
        input.type = type;
        redContainer.appendChild(input);

        return input;
    }

    popuniSelectZaHamburgere(select) {

        for(const p of this.prodavnica.hamburgeri) {

            const option = document.createElement("option");
            option.value = p.id;
            option.textContent = p.naziv;
            select.appendChild(option);
        }
    }

    async popuniSelectZaSastojke(select) { //, hamburgerSelect

        //ovako bi bilo da u select polju trebaju da budu sastojci koji su trenutno u hamburgeru...
        //select.innerHTML = "";

        // const sviHamburgeriProdavnice = this.prodavnica.hamburgeri;

        // for(const p of sviHamburgeriProdavnice)
        // {
        //     if(p.id == Number(hamburgerSelect.value)) //na osnovu odabranog select Hamburger
        //     {
        //         const sastojci = p.sastojciUHamburgeru;

        //         for(const s of sastojci)
        //         {
        //             const option = document.createElement("option");
        //             option.value = s.id;
        //             option.textContent = s.nazivSastojka;
        //             select.appendChild(option);
        //         }
        //     }
        // }

        try{
            const result = await fetch("http://localhost:5142/Hamburger/VratiSveSastojke");

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }
            else {
                const sastojci = await result.json();

                for(const p of sastojci) {

                    const option = document.createElement("option");
                    option.value = p.id;
                    option.textContent = p.naziv;
                    select.appendChild(option);
                }
            }
        }
        catch(e)
        {
            console.error(e);
        }
    }
}