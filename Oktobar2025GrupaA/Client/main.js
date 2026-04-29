import {Prikaz} from "./Prikaz.js"

console.log("Main.js ucitan");

//glavni container
const container = document.createElement("div");
container.classList.add("container");
document.body.appendChild(container);

//prikaz container - u njemu se nalaze produkcijske kuce
const prikazContainer = document.createElement("div");
prikazContainer.classList.add("prikazContainer");
container.appendChild(prikazContainer);

const prikaz = new Prikaz(prikazContainer);
await prikaz.crtaj();