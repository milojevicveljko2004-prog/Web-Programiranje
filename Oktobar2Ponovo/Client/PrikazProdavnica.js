import {Forma} from "./Forma.js"
import {Prodavnica} from "./Prodavnica.js"

export class PrikazProdavnica{
    constructor(container) {
        this.container = container; //prikazContainer
        this.prodavnice = [];
    }

    async crtaj() {
        this.container.innerHTML = "";

        const result = await fetch("http://localhost:5083/Prodavnica/PreuzmiProdavniceSaProizvodima");
        if(!result.ok) {
            const error = await result.error();
            console.error(error);
        }

        this.prodavnice = await result.json();

        //za svaku prodavnicu se crta jedna forma i jedna prodavnica
        for(const p of this.prodavnice) {

            const redContainer = document.createElement("redContainer"); //jedan red = forma + prodavnica
            redContainer.classList.add("redContainer");
            this.container.appendChild(redContainer);

            const forma = new Forma(redContainer, p);
            await forma.crtaj();

            const prodavnica = new Prodavnica(redContainer, p);
            await prodavnica.crtaj();
        }
    }
}