function httpGet(theUrl)
{
    var xmlHttp = new XMLHttpRequest();
    xmlHttp.open( "GET", theUrl, false ); // false for synchronous request
    xmlHttp.send( null );
    return xmlHttp.responseText;
}
const field = document.getElementById("field");
var hexes = [];
var heroes = [];
var mode = "Move";
var hero;
var idActiveHero = 0;
var idCastingSpell = -1;

window.onload = async function() {
    idActiveHero = await getActiveHero();
    console.log(idActiveHero + "   onload");

    fetch("https://localhost:7241/Field/GetField")
        .then(response => response.json())
        .then(coord => feelField(coord));
}


function feelField(_field) {
    if(_field.length < 1)
        return 0;

    while(field.firstChild) {
        field.removeChild(field.firstChild);
    }
    
    

    hexes = [];
    heroes = [];
    _field.forEach(element => {
        const hex = document.createElement("div");
        hex.setAttribute("id", element.id);
        hex.setAttribute("onclick", "hexClick(this)");

        if(element.hero != null) {

            const teamImg = document.createElement("img");
            if(element.hero.team == "red")
                teamImg.setAttribute("src", "http://i.imgur.com/9HMnxKs.png");
            else
                teamImg.setAttribute("src", "https://celes.club/uploads/posts/2022-11/1667393225_4-celes-club-p-fon-golubogo-tsveta-oboi-4.jpg");
            teamImg.setAttribute("style", "position: absolute; height: " + getPercentHP(element.hero) + "px; width: 100px;");

            const heroImg = document.createElement("img");
            heroImg.setAttribute("src", element.hero.name + ".png");
            heroImg.setAttribute("style", "position: absolute; width: 80px; padding-left: 10px; padding-top: 16px;");

            hex.setAttribute("class", "hero " + element.hero.team);
            hex.setAttribute("onmouseenter", "feelHeroInfo(this)");

            hex.appendChild(teamImg);
            hex.appendChild(heroImg);

            fillEffectsOnHex(hex, element.hero)
            
            element.hero.coordid = element.id;
            heroes.push(element.hero);
        }
        
        field.appendChild(hex);
        hexes.push(element);
    });
    heroes = heroes.toSorted((a, b) => a.id - b.id);
    
    hero = heroes.find(x => x.id === idActiveHero);
    enableUpgrades(hero);
    //btnEndTurn.innerText = hero.ap;
    feelActiveHeroInfo(hero);
console.log(heroes);
    fillFootHovers(hexes[hero.coordid], hexes);
    enableSpells(hero);
}

function hexClick(_hex) {
    console.log(idCastingSpell + "   " + _hex.getAttribute("id"));
    if (idCastingSpell >= 0) {
        console.log("casting spell " + idCastingSpell)
        castSpell(idCastingSpell, _hex.getAttribute("id"));
        return;
    }
    
    var _target = hexes[_hex.getAttribute("id")].hero;
    if (_target == null) {
        console.log("stepping")
        stepHero(hero, _hex);
    } else if (_target.team !== hero.team) {
        console.log("attcking")
        attackHero(hero, _hex);
    } else if (_target.team === hero.team) {
        
    }
}

function cancel() {
    idCastingSpell = -1;
    clearHovers();
    clearHoversActions();
    hero = heroes.find(x => x.id === idActiveHero);
    console.log(hexes[idActiveHero]);
    fillFootHovers(hexes[hero.coordid], hexes);
    enableSpells(hero);
}