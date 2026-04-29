import {Form} from "./Form.js"
import {Fabrika} from "./Fabrika.js"

export class Prikaz{
    constructor(container) {
        this.container = container; //prikazContainer
        this.fabrike = [];
    }

    async crtaj() {
        await this.pribaviFabrike();

        for(const f of this.fabrike)
        {
            const formaFabrikaContainer = document.createElement("div"); //container u kome ce biti po jedna forma i jedna fabrika(desni deo)
            formaFabrikaContainer.classList.add("formaFabrikaContainer");
            this.container.appendChild(formaFabrikaContainer);

            const form = new Form(formaFabrikaContainer, f);
            await form.crtaj();

            //fabrika crta i zaglavlje koje je iznad i ispod fabriku
            const fabrika = new Fabrika(formaFabrikaContainer, f);
            fabrika.crtaj();
        }
    }

    async pribaviFabrike() {
        try{
            const result = await fetch("http://localhost:5145/Fabrika/VratiSveFabrike");

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
            }

            this.fabrike = await result.json();
        }
        catch(e)
        {
            console.error(e);
        }
    }
}