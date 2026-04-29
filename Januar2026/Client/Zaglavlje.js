export class Zaglavlje {
    constructor(container, prodavnica) {
        this.container = container; //zaglavljeContainer
        this.prodavnica = prodavnica;
        this.akcijskiArtikal = null;
    }

    async crtaj() {
        const naslovZaglavljaContainer = document.createElement("div");
        naslovZaglavljaContainer.classList.add("naslovZaglavljaContainer");
        this.container.appendChild(naslovZaglavljaContainer);

        const naslovZaglavlja = document.createElement("h3");
        naslovZaglavlja.textContent = this.prodavnica.naziv;
        naslovZaglavlja.classList.add("naslovZaglavlja");
        naslovZaglavljaContainer.appendChild(naslovZaglavlja);

        //za zaglavlje je potrebno da dobijem akcijski artikal, potreban je za textContent labele.
        await this.nadjiAkcijskiArtikal();

        const artikalZaglavlja = document.createElement("label");
        artikalZaglavlja.setAttribute("for", "artikalZaglavlja");
        //koristim sifru, a ne naziv jer ga nisam imao u modelu, a mnogo trebam da menjam, ali treba naziv
        if(this.akcijskiArtikal!=null)
        {
            artikalZaglavlja.textContent = `Akcija: ${this.akcijskiArtikal.sifra} - 50%`;
        }
        else
        {
            artikalZaglavlja.textContent = "Ova prodavnica nema akcijski artikal";
        }
        artikalZaglavlja.classList.add("artikalZaglavlja");
        naslovZaglavljaContainer.appendChild(artikalZaglavlja);

    }

    async nadjiAkcijskiArtikal() {
        try {
            const idProdavnice = this.prodavnica.id;

            const result = await fetch(`http://localhost:5025/Prodavnica/VratiAkcijskiArtikal/${idProdavnice}`);

            if(!result.ok) {
                const errMessage = await result.text();
                console.error(errMessage);
                return;
            }
            else{
                this.akcijskiArtikal = await result.json();
            }
        }
        catch(e) {
            console.error(e);
        }
    }
}