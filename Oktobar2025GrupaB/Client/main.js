//Zadatak nije ispostovan do kraja. Nemam ovaj deo sa dva hleba
import {Prikaz} from "./Prikaz.js"

console.log("main.js ucitan");

const container = document.createElement("div"); //glavni container
container.classList.add("container");
document.body.appendChild(container);

const prikazContainer = document.createElement("div"); //u njemu ce biti NaslovForm i Prodavnica
prikazContainer.classList.add("prikazContainer");
container.appendChild(prikazContainer);

const prikaz = new Prikaz(prikazContainer);
await prikaz.crtaj();

