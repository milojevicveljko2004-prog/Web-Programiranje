import { Prodavnica } from "./Prodavnica.js"

console.log("main.js je ucitan");

const container = document.createElement("div");
container.classList.add("container");
document.body.appendChild(container); //glavni container odnosno div - vezan za body

// const formContainer = document.createElement("div"); //kontejner za crtanje forme
// formContainer.classList.add("formContainer");
// container.appendChild(formContainer);

// const shopContainer = document.createElement("div"); //kontejner za crtanje desnog dela - prodavnice
// shopContainer.classList.add("shopContainer");
// container.appendChild(shopContainer);

const prodavnica = new Prodavnica(container);
await prodavnica.crtaj();
