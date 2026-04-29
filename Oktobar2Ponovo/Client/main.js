import { PrikazProdavnica } from "./PrikazProdavnica.js";

console.log("main.js ucitan.");

const container = document.createElement("div");
container.classList.add("container");
document.body.appendChild(container); //glavni container

const prikazContainer = document.createElement("div"); //sadrzi sve redove
prikazContainer.classList.add("prikazContainer");
container.appendChild(prikazContainer);

const prikaz = new PrikazProdavnica(prikazContainer);
await prikaz.crtaj();

