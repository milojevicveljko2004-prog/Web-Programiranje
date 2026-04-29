export class Api { //klasa u kojoj ce da budu svi pozivi ka serveru
    async dodajProizvod(naziv, kategorijaNaziv, cena, kolicina, idProdavnice) {

        const response = await fetch($`http://localhost:5043/Prodavnica/UpisProizvoda/{naziv}/{}/200/15/2`);
    }
}