export class Forma{
    constructor(container, prodavnica) {
        this.container = container; //formaProdavnicaContainer
        this.prodavnica = prodavnica;
        this.brendovi = [];
        this.velicine = ["Sve", "S", "M", "L"]; //"" oznacava opciju Sve
        this.rezultatiContainer = null;
    }

    async crtaj() {
        const formContainer = document.createElement("div");
        formContainer.classList.add("formContainer");
        this.container.appendChild(formContainer);

        const brendSelect = this.createSelect(formContainer, "brendSelect", "Brend: ");
        const velicinaSelect = this.createSelect(formContainer, "velicinaSelect", "Velicina: ");
        const cenaOdInput = this.createInput(formContainer, "cenaOdInput", "Cena od: ");
        const cenaDoInput = this.createInput(formContainer, "cenaDoInput", "Cena do: ");

        //popuni select polje za brendove
        await this.popuniBrendove();

        for(const b of this.brendovi) {
            const option = document.createElement("option");
            option.value = b;
            option.textContent = b;
            brendSelect.appendChild(option);
        }

        //popuni select polje za velicinu
        for(const v of this.velicine) {
            const option = document.createElement("option");
            option.value = v;
            option.textContent = v;
            velicinaSelect.appendChild(option);
        }

        //button
        const button = document.createElement("button");
        button.textContent = "Nadji";
        button.type = "button";
        button.classList.add("button");
        formContainer.appendChild(button);

        // poseban container samo za rezultate. Potreban da bi se svakim klikom na dugme Nadji refresova-o prikaz
        //Unutar njega ce da bude prodavnicaContainer. Znaci rezultatiContainer je kao wrapper za prodavnicaContainer
        this.rezultatiContainer = document.createElement("div");
        this.rezultatiContainer.classList.add("rezultatiContainer");
        this.container.appendChild(this.rezultatiContainer);

        button.onclick = async() => {

            const prodavnicaId = Number(this.prodavnica.id);
            const brend = brendSelect.value.trim();
            //Ako korisnik moze da ne unese nista u neko polje(ili kod velicine odabere "Sve") treba posebno da se obezbedi da je vrednost null
            // velicina
            let velicina;
            if (velicinaSelect.value === "Sve") {
                velicina = null;
            } else {
                velicina = velicinaSelect.value.trim();
            }

            // cenaOd
            let cenaOd;
            if (cenaOdInput.value.trim() === "") {
                cenaOd = null;
            } else {
                cenaOd = Number(cenaOdInput.value);
            }

            // cenaDo
            let cenaDo;
            if (cenaDoInput.value.trim() === "") {
                cenaDo = null;
            } else {
                cenaDo = Number(cenaDoInput.value);
            }

            const dto = {
                prodavnicaId: prodavnicaId,
                brend: brend,
                velicina: velicina,
                cenaOd: cenaOd,
                cenaDo: cenaDo
            };

            try{
                const result = await fetch("http://localhost:5025/Prodavnica/NadjiArtikal", {
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
                    console.log("fetch() za nalazenje artikla radi!");
                }

                const artikli = await result.json();

                this.obrisiPrethodniPrikaz();
                //velicina mora da se prosledi zbog prikaza kolicine. Kolicina nije ista za Sve, L, M, S
                this.crtajProdavnicu(artikli, velicina);

            }
            catch(e) {
                console.log(e);
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
        redForme.appendChild(label);

        const select = document.createElement("select");
        select.id = name;
        select.name = name;
        redForme.appendChild(select);

        return select;
    }

    createInput(container, name, lblText) {

        const redForme = document.createElement("div");
        redForme.classList.add("redForme");
        container.appendChild(redForme);

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        redForme.appendChild(label);

        const input = document.createElement("input");
        input.id = name;
        input.name = name;
        redForme.appendChild(input);

        return input;
    }

    async popuniBrendove() {
        const idProdavnice = this.prodavnica.id;

        const result = await fetch(`http://localhost:5025/Prodavnica/VratiBrendove/${idProdavnice}`);
        if(!result.ok) {
            const errMessage = await result.text();
            console.error(errMessage);
            return;
        }

        this.brendovi = await result.json();
    }

    obrisiPrethodniPrikaz() {
        if(this.rezultatiContainer) {
            this.rezultatiContainer.innerHTML = "";
        }
    }

    crtajProdavnicu(artikli, velicina) {
        const prodavnicaContainer = document.createElement("div");
        prodavnicaContainer.classList.add("prodavnicaContainer");
        this.rezultatiContainer.appendChild(prodavnicaContainer); //u njemu je lista artikla

        if(!artikli || artikli.length===0) {
            const nema = document.createElement("div");
            nema.textContent = "Nema artikala koji zadovoljavaju kriterijum.";
            prodavnicaContainer.appendChild(nema);
            return;
        }

        for(const a of artikli) {
            const artikalContainer = document.createElement("div");
            artikalContainer.classList.add("artikalContainer");
            prodavnicaContainer.appendChild(artikalContainer); //u njemu su clanovi konkretnog artikla: sifra, cena, itd..

            //kreiranje elemenata koji cine artikal
            const sifra = document.createElement("label");
            sifra.setAttribute("for", "sifra");
            sifra.textContent = a.sifraModela;
            sifra.classList.add("sifra");
            artikalContainer.appendChild(sifra);

            const cena = document.createElement("label");
            cena.setAttribute("for", "cena");
            cena.textContent = a.cena + "RSD";
            cena.classList.add("cena");
            artikalContainer.appendChild(cena);

            const kolicina = document.createElement("label");
            kolicina.setAttribute("for", "kolicina");
            if(velicina==null) //odabrano je sve
            {
                const zbir = a.kolicinaS+a.kolicinaM+a.kolicinaL;
                kolicina.textContent = `Kolicina: ${zbir}`;
            }
            else if(velicina=="L")
            {
                kolicina.textContent = `Kolicina: ${a.kolicinaL}`;
            }
            else if(velicina=="M")
            {
                kolicina.textContent = `Kolicina: ${a.kolicinaM}`;
            }
            else if(velicina=="S")
            {
                kolicina.textContent = `Kolicina: ${a.kolicinaS}`;
            }
            else
            {
                console.error("Nevalidna velicina!");
            }
            kolicina.classList.add("kolicina");
            artikalContainer.appendChild(kolicina);

            //button se prikazuje samo ako je odabrana velicina prikazuje se button
            if(velicina=="S" || velicina=="M" || velicina=="L")
            {
                const button = document.createElement("button");
                button.type = "button";
                button.textContent = "Kupi";
                artikalContainer.appendChild(button);

                button.onclick = async() => {
                    await this.kupiProizvod(a);
                }
            }
        }
    }

    async kupiProizvod(artikal) {
            const artikalUProdajiID = artikal.id;
            const velicina = artikal.velicina; //u drugoj metodi je obezbedjeno da ne moze da bude null

            const dto = {
                artikalUProdajiID: artikalUProdajiID,
                velicina: velicina
            };

            const result = await fetch("http://localhost:5025/Prodavnica/KupiArtikal", {
                method: "PUT",
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
            else {
                console.log("fetch() za kupovinu artikla radi!");
                this.obrisiPrethodniPrikaz();
            }
        }
}