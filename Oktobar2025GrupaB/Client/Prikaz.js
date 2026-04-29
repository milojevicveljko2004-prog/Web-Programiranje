import {Form} from "./Form.js"
import {Prodavnica} from "./Prodavnica.js"

export class Prikaz {
    constructor(container) {
        this.container = container; //prikazContainer
        this.prodavnice = [];
    }

    async crtaj() {
        await this.pribaviProdavnice();

        for(const p of this.prodavnice) {

            const formProdavnica = document.createElement("div"); //forma sa naslovom + prodavnica
            formProdavnica.classList.add("formProdavnica");
            this.container.appendChild(formProdavnica);

            const formNaslovContainer = document.createElement("div"); //naslov + forma
            formNaslovContainer.classList.add("formNaslovContainer");
            formProdavnica.appendChild(formNaslovContainer);

            const prodavnicaContainer = document.createElement("div"); //prodavnica sa hamburgerima
            prodavnicaContainer.classList.add("prodavnicaContainer");
            formProdavnica.appendChild(prodavnicaContainer);

            const form = new Form(formNaslovContainer, p);
            await form.crtaj();

            const prodavnica = new Prodavnica(prodavnicaContainer, p);
            prodavnica.crtaj();
        }
    }

    async pribaviProdavnice() {
        try{
            const result = await fetch("http://localhost:5142/Hamburger/PribaviProdavnice");

            if(!result.ok) {
                const errText = await result.text();
                console.error(errText);
            }
            else {
                this.prodavnice = await result.json();
                console.log("fetch() za prodavnice radi");
            }
        }
        catch(e)
        {
            console.error(e);
        }
    }
}