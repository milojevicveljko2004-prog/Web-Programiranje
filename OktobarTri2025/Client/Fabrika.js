export class Fabrika{
    constructor(container, fabrika) {
        this.container = container; //formaFabrikaContainer
        this.fabrika = fabrika;
    }

    crtaj() {
        const fabrikaZaglavljeContainer = document.createElement("div"); //kontejner u kome su zaglavlje i ispod njega fabrika
        fabrikaZaglavljeContainer.classList.add("fabrikaZaglavljeContainer");
        this.container.appendChild(fabrikaZaglavljeContainer);

        var ukupanKapacitet = 0;
        for(const k of this.fabrika.kontejneri)
        {
            ukupanKapacitet += k.trenutniKapacitet;
        }

        const zaglavlje = document.createElement("h3");
        zaglavlje.classList.add("zaglavlje");
        zaglavlje.textContent = `Fabrika "${this.fabrika.naziv}" - ${this.fabrika.brKontejnera} kontejnera - kapaciteta ${ukupanKapacitet}`;
        fabrikaZaglavljeContainer.appendChild(zaglavlje);

        const fabrikaContainer = document.createElement("div"); //fabrika container u kome su kontejneri
        fabrikaContainer.classList.add("fabrikaContainer");
        fabrikaZaglavljeContainer.appendChild(fabrikaContainer);

        for(const p of this.fabrika.kontejneri) //svaki kontejner dodaj u fabrikaContainer
        {
            const kontejnerContainer = document.createElement("div");
            kontejnerContainer.classList.add("kontejnerContainer");
            kontejnerContainer.style.backgroundColor = `${p.boja.naziv}`;
            kontejnerContainer.textContent = `${p.trenutniKapacitet} (${p.maxKapacitet})`;
            fabrikaContainer.appendChild(kontejnerContainer);
        }
    }
}