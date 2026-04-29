//Zadatak nije zavrsen

import {Prikaz} from "./Prikaz.js"

console.log("main.js ucitan.");

const container = document.createElement("div");
container.classList.add("container");
document.body.appendChild(container);

const prikazContainer = document.createElement("div"); //u njemu ce se nalaziti prodavnice
prikazContainer.classList.add("prikazContainer");
container.appendChild(prikazContainer);

const prikaz = new Prikaz(prikazContainer);
await prikaz.crtaj();