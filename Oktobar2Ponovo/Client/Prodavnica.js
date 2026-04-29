export class Prodavnica{
    constructor(container, prodavnica) {
        this.container = container; //redContainer
        this.prodavnica = prodavnica;
    }

    async crtaj() {
        const prodavnicaContainer = document.createElement("div"); //glavni container jedne prodavnice
        prodavnicaContainer.classList.add("prodavnicaContainer");
        this.container.appendChild(prodavnicaContainer);

        const naslovProdavnica = document.createElement("h3");
        naslovProdavnica.classList.add("naslovProdavnica");
        naslovProdavnica.textContent = "Prodavnica: " + this.prodavnica.naziv;
        prodavnicaContainer.appendChild(naslovProdavnica);

        //sad prikaz proizvoda

        //ako nema proizvoda
        if(!this.prodavnica.proizvodUProdavnici || this.prodavnica.proizvodUProdavnici.length === 0)
        {
            const nema = document.createElement("div");
            nema.classList.add("nema");
            nema.textContent = "Nema proizvoda.";
            prodavnicaContainer.appendChild(nema);
        }

        //Crtanje svakog proizvoda
        for(const p of this.prodavnica.proizvodUProdavnici) {

            const proizvodRed = document.createElement("div");
            proizvodRed.classList.add("proizvodRed");
            prodavnicaContainer.appendChild(proizvodRed);

            // Levi deo: naziv + kolicina + graficki prikaz
            const leviDeoContainer = document.createElement("div");
            leviDeoContainer.classList.add("leviDeoContainer");
            proizvodRed.appendChild(leviDeoContainer);

            const label = document.createElement("label");
            label.textContent = p.proizvod.naziv + ": " + p.kolicina;
            leviDeoContainer.appendChild(label);

            const barContainer = document.createElement("div");
            barContainer.classList.add("barContainer");
            leviDeoContainer.appendChild(barContainer);

            const bar = document.createElement("div");
            bar.classList.add("bar");
            bar.style.width = `${p.kolicina}%`;
            barContainer.appendChild(bar);

           // Desni deo: kolicina labela + input + dugme Prodaj
            const desniContainer = document.createElement("div");
            desniContainer.classList.add("desniContainer");
            proizvodRed.appendChild(desniContainer);

            const lblKolicina = document.createElement("label");
            lblKolicina.textContent = "Kolicina: "
            desniContainer.appendChild(lblKolicina);

            const inputKolicina = document.createElement("input");
            inputKolicina.classList.add("inputKolicina");
            inputKolicina.type = "number";
            inputKolicina.min=1;
            inputKolicina.id = "inputKolicina";
            inputKolicina.name = "inputKolicina";
            desniContainer.appendChild(inputKolicina);

            const button = document.createElement("button");
            button.textContent = "Prodaj";
            button.classList.add("buttonProdaj");
            desniContainer.appendChild(button);

            button.onclick = async() => {
                const kolicina = Number(inputKolicina.value);
                const proizvodID = p.proizvod.id; //!!!Gleda se sta je u DTO klasi, odnosno STA SE PROSLEDJUJE, KOJI ID!!!
                const prodavnicaID = this.prodavnica.id;

                const dto = {
                    kolicina: kolicina,
                    proizvodID: proizvodID,
                    prodavnicaID: prodavnicaID
                };

                const result = await fetch("http://localhost:5083/Prodavnica/ProdajProizvod", {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    body: JSON.stringify(dto)
                }
                );

                if(!result.ok) {
                    const errText = await result.text();
                    console.error(errText);
                }
                else {
                    console.log("fetch() za prodaju radi!");
                }

                location.reload(); //refresh cele stranice
            }
        }


    }
}