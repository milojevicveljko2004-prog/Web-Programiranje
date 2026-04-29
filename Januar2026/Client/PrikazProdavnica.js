import {Forma} from "./Forma.js"
import {Zaglavlje} from "./Zaglavlje.js"

export class PrikazProdavnica {
    constructor(container) {
        this.container = container; //prikazContainer
        this.prodavnice = [];
    }

    async crtaj() {
        await this.pribaviProdavnice();

        for(const p of this.prodavnice) {

            const redContainer = document.createElement("div");
            redContainer.classList.add("redContainer");
            this.container.appendChild(redContainer); //delice se na zaglavlje i prodavnicu

            const zaglavljeContainer = document.createElement("div");
            zaglavljeContainer.classList.add("zaglavljeContainer");
            redContainer.appendChild(zaglavljeContainer); //zaglavlje - crta naslov i akciju

            const formaProdavnicaContainer = document.createElement("div");
            formaProdavnicaContainer.classList.add("formaProdavnicaContainer");
            redContainer.appendChild(formaProdavnicaContainer); //crta formu i prodavnicu

            //prvo treba da se crta zaglavlje da bi bilo iznad

            const zaglavlje = new Zaglavlje(zaglavljeContainer, p);
            await zaglavlje.crtaj();

            const forma = new Forma(formaProdavnicaContainer, p);
            await forma.crtaj(); //crta i formu i prodavnicu pored

            // const prodavnica = new Prodavnica(redContainer, p);
            // await prodavnica.crtaj();

            // const zaglavlje = new Zaglavlje(redContainer, p);
            // await zaglavlje.crtaj();
        }
    }

    async pribaviProdavnice() {
        try{
            const result = await fetch("http://localhost:5025/Prodavnica/VratiProdavniceSaProizvodima");

            if(!result.ok) {
                const errMessage = await result.error();
                console.error(errMessage);
            }

            this.prodavnice = await result.json();
        }
        catch(e)
        {
            console.error(e);
        }
    }
}