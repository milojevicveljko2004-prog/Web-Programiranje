export class Zaglavlje {
    constructor(container, prodavnica) {
        this.container = container; //zaglavljeContainer
        this.prodavnica = prodavnica;
    }

    async crtaj() {

        const naslovProdavniceContainer = document.createElement("div");
        naslovProdavniceContainer.classList.add("naslovProdavniceContainer");
        this.container.appendChild(naslovProdavniceContainer);

        const naslov = document.createElement("h3");
        naslov.classList.add("naslov");
        naslov.textContent = this.prodavnica.naziv;
        naslovProdavniceContainer.appendChild(naslov);

        const cenaValue = await this.pribaviCenuProdavnice();

        const cenaProdavniceContainer = document.createElement("div");
        cenaProdavniceContainer.classList.add("cenaProdavniceContainer");
        cenaProdavniceContainer.textContent = `Dnevni promet: ${cenaValue} RSD`;
        this.container.appendChild(cenaProdavniceContainer);
    }

    async pribaviCenuProdavnice() {
        try{
            const result = await fetch(`http://localhost:5083/controller/VratiCenuProdavnice/${this.prodavnica.id}`);

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }
            else {
                console.log("fetch() za cenu prodavnice radi.");
                const cena = await result.text();
                return cena;
            }
        }
        catch(e) {
            console.error(e);
        }
    }
}