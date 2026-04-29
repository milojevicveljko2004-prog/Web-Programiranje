import {Zaglavlje} from "./Zaglavlje.js"
import {Form} from "./Form.js"

export class Prikaz {
    constructor(container) {
        this.container = container; //prikazContainer
        this.prodavnice = [];
    }

    async crtaj() {

        await this.pribaviProdavnice();

        for(const p of this.prodavnice)
        {
            const prodavnicaContainer = document.createElement("div"); //u njemu ce da budu zaglavlje, forma i desni deo
            prodavnicaContainer.classList.add("prodavnicaContainer");
            this.container.appendChild(prodavnicaContainer);

            const zaglavljeContainer = document.createElement("div"); //u njemu ce da budu naslov i cenaProdavnicePrikaz
            zaglavljeContainer.classList.add("zaglavljeContainer");
            prodavnicaContainer.appendChild(zaglavljeContainer);

            const formDesniDeoContainer = document.createElement("div"); //forma + desni deo
            formDesniDeoContainer.classList.add("formDesniDeoContainer");
            prodavnicaContainer.appendChild(formDesniDeoContainer);

            const zaglavlje = new Zaglavlje(zaglavljeContainer, p);
            await zaglavlje.crtaj();

            const form = new Form(formDesniDeoContainer, p);
            await form.crtaj(); //crta i formu i desni deo
        }


    }

    async pribaviProdavnice() {
        try{
            const result = await fetch("http://localhost:5083/controller/VratiSveProdavnice");

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }
            else {
                this.prodavnice = await result.json();
                console.log("fetch() za prodavnice radi.");
            }
        }
        catch(e) {
            console.error(e);
        }
    }
}