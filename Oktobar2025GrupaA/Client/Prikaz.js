import {Form} from "./Form.js"
import {Statistika} from "./Statistika.js"

export class Prikaz {
    constructor(container) {
        this.container = container; //prikazContainer
        this.produkcijskeKuce = [];
    }

    async crtaj() {
        await this.pribaviProdukcijskeKuce(); //pribavlja ih zajedno sa kategorijama

        for(const p of this.produkcijskeKuce) { //za svaku produkcijsku kucu pravi naslov, formu i statistiku
            
            const prodKucaContainer = document.createElement("div"); //u njoj ce biti forma i statistika
            prodKucaContainer.classList.add("prodKucaContainer");
            this.container.appendChild(prodKucaContainer);

            const formNaslovContainer = document.createElement("div"); //u njoj ce biti naslov i forma
            formNaslovContainer.classList.add("formNaslovContainer");
            prodKucaContainer.appendChild(formNaslovContainer);

            const statistikaContainer = document.createElement("div");
            statistikaContainer.classList.add("statistikaContainer");
            prodKucaContainer.appendChild(statistikaContainer);

            const form = new Form(formNaslovContainer, p, statistikaContainer);
            await form.crtaj(); //crtace i naslov i formu. Kada se odabere kategorija onda crta i statistiku

            // const statistika = new Statistika(statistikaContainer);
            // await statistika.crtaj(); //crta donji deo, tj. statistiku

        }
    }

    async pribaviProdukcijskeKuce() {
        try{
            const result = await fetch("http://localhost:5253/Film/VratiSveProdKuce");

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }
            else {
                this.produkcijskeKuce = await result.json();
                console.log("fetch() za prod. kuce je uspeo!");
            }
        }
        catch(e) {
            console.error(e);
        }
    }
}