export class Form{
    constructor(container, fabrika) {
        this.container = container; //formaFabrikaContainer
        this.fabrika = fabrika;
        this.boje = [];
    }

    async crtaj() {
        const formContainer = document.createElement("div"); //container u kome ce biti elementi forme
        formContainer.classList.add("formContainer");
        this.container.appendChild(formContainer);

        const selectBoja = this.DodajSelect(formContainer, "selectBoja", "Boja");
        const inputKolicina = this.DodajInput(formContainer, "inputKolicina", "Kolicina", "number");

        //popuni select polje
        await this.pribaviBoje();

        for(const b of this.boje) 
        {
            const option = document.createElement("option");
            option.value = b.id;
            option.textContent = b.naziv;
            selectBoja.appendChild(option);
        }

        //dodaj button
        const button = document.createElement("button");
        button.classList.add("button");
        button.textContent = "Dodaj";
        formContainer.appendChild(button);

        button.onclick = async() => {
            const fabrikaID = Number(this.fabrika.id);
            const bojaID = Number(selectBoja.value);
            const kolicina = Number(inputKolicina.value);

            const dto = {
                fabrikaID: fabrikaID,
                bojaID: bojaID,
                kolicina: kolicina
            };

            try{
                const result = await fetch("http://localhost:5145/Fabrika/DodajBojuUKontejner", {
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
                else
                {
                    console.log("fetch() za dodavanje boje radi!");
                }

                //location.reload(); //refresh stranice
            }
            catch(e)
            {
                console.error(e);
            }
        }
        
    }

    DodajSelect(container, name, lblText) {

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        container.appendChild(label);

        const select = document.createElement("select");
        select.classList.add(name);
        select.name = name;
        // select.id = name;
        container.appendChild(select);

        return select;
    }

    DodajInput(container, name, lblText, type) {

        const label = document.createElement("label");
        label.setAttribute("for", name);
        label.textContent = lblText;
        container.appendChild(label);

        const input = document.createElement("input");
        input.classList.add(name);
        input.type = type;
        input.name = name;
        // input.id = name;
        container.appendChild(input);

        return input;
    }

    async pribaviBoje() {
        try{
            const result = await fetch("http://localhost:5145/Fabrika/PribaviBoje");

            if(!result.ok){
                const errMessage = await result.text();
                console.error(errMessage);
            }

            this.boje = await result.json();
        }
        catch(e)
        {
            console.error(e);
        }
    }
}