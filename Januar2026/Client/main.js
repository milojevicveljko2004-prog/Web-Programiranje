import {PrikazProdavnica} from "./PrikazProdavnica.js"

console.log("main.js ucitan");

//glavni container
const container = document.createElement("div");
container.classList.add("container");
document.body.appendChild(container);

//u njemu se nalazi skup prodavnica
const prikazContainer = document.createElement("div");
prikazContainer.classList.add("prikazContainer");
container.appendChild(prikazContainer);

const prikaz = new PrikazProdavnica(prikazContainer);
await prikaz.crtaj();