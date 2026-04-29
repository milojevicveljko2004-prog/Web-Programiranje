export class Prodavnica {
    constructor(container, prodavnica) {
        this.container = container; //prodavnicaContainer
        this.prodavnica = prodavnica;
    }

    crtaj() {
        for(const p of this.prodavnica.hamburgeri) {

            if(p.prodat==false) //ako hamburger nije prodat
            {
                const hamburgerContainer = document.createElement("div");
                hamburgerContainer.classList.add("hamburgerContainer");
                this.container.appendChild(hamburgerContainer);

                //prilozi u hamburgeru
                for(const s of p.sastojciUHamburgeru)
                {
                    const prilogContainer = document.createElement("div");
                    prilogContainer.classList.add("prilogContainer");
                    hamburgerContainer.appendChild(prilogContainer);

                    //Pravila za crtanje:
                    // Ako je: sir → debljina = 10px i paradajz → debljina = 8px
                    // i imas tri sira i 2 paradajza, onda crtaš:
                    // sir → visina = 30px
                    // paradajz → visina = 16px

                    //prilog ima naziv i ispod njega je visina tj. blokovi
                    const nazivPriloga = document.createElement("div");
                    nazivPriloga.classList.add("nazivPriloga");
                    nazivPriloga.textContent = s.nazivSastojka;
                    prilogContainer.appendChild(nazivPriloga);

                    const visina = s.kolicina * s.debljina;

                    const bar = document.createElement("div");
                    bar.classList.add("bar");
                    bar.style.height = `${visina}px`;
                    prilogContainer.appendChild(bar);
                }

                const buttonKupi = document.createElement("button");
                buttonKupi.classList.add("buttonKupi");
                buttonKupi.textContent = "Kupi";
                hamburgerContainer.appendChild(buttonKupi);

                const nazivHamburgera = document.createElement("p");
                nazivHamburgera.classList.add("nazivHamburgera");
                nazivHamburgera.textContent = p.naziv;
                hamburgerContainer.appendChild(nazivHamburgera);

                buttonKupi.onclick = async() => {
                    try{
                        const result = await fetch(`http://localhost:5142/Hamburger/KupiHamburger/${p.id}`, {
                            method: "PUT"
                        });

                        if(!result.ok) {
                            const errMessage = await result.text();
                            console.error(errMessage);
                        }
                        else {
                            console.log("fetch() za kupovinu radi.");
                            location.reload();
                        }
                    }
                    catch(e)
                    {
                        console.log(e);
                    }
                }
            }
        }
    }
}