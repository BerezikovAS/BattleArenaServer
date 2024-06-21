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
var heroInfo;

window.onload = async function() {
    idActiveHero = await getActiveHero();
    console.log(idActiveHero + "   onload");

    fetch("https://localhost:7241/Field/GetField")
        .then(response => response.json())
        .then(coord => feelField(coord))
        .then(coord => feelHeroInfo(null, heroes[idActiveHero].hexId));
}

window.addEventListener("keydown", keyPressed);
window.addEventListener("keyup", keyPressed);
window.addEventListener("keypress", skillPressed);
 
function keyPressed(e){
    switch(e.key){
         
        case "Control":
            if (e.type == "keydown")
                fillPercentResist(true);
            else
                fillPercentResist(false);
            break;
    }
}

function skillPressed(e){
   switch(e.code){
        case "Digit1":
            castSpell(1);
           break;
        case "Digit2":
            castSpell(2);
           break;
        case "Digit3":
            castSpell(3);
           break;
        case "Digit4":
            castSpell(4);
           break;
        case "KeyR":
            cancel();
           break;
        case "Space":
            endTurn();
           break;
   }
}

async function feelField(_field) {
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
            if(element.hero.type == 1) {
                heroImg.setAttribute("src", "obstacles/" + element.hero.name + ".png");
                heroImg.setAttribute("style", "position: absolute; width: 64px; padding-left: 16px; padding-top: 24px;");
            }

            hex.setAttribute("class", "hero " + element.hero.team);
            hex.setAttribute("onmouseenter", "feelHeroInfo(this, -1)");

            hex.appendChild(teamImg);
            hex.appendChild(heroImg);

            fillEffectsOnHex(hex, element.hero)
            
            element.hero.coordid = element.id;
            if(element.hero.type === 0)
                heroes[element.hero.id] = element.hero;
        }
        if (element.obstacle != null) {
            console.log(element.obstacle);
            const obstImg = document.createElement("img");
            obstImg.setAttribute("src", "obstacles/" + element.obstacle.name + ".png");
            obstImg.setAttribute("style", "position: absolute; width: 80px; padding-left: 10px; padding-top: 16px;");
            hex.appendChild(obstImg);
        }
        
        field.appendChild(hex);
        hexes.push(element);
    });
    //heroes = heroes.toSorted((a, b) => a.id - b.id);
    idActiveHero = await getActiveHero();

    hero = heroes[idActiveHero];


    enableUpgrades(hero);
    //btnEndTurn.innerText = hero.ap;
    feelActiveHeroInfo(hero);
    fillFootHovers(hexes[hero.coordid], hexes);
    enableSpells(hero);
}

function hexClick(_hex) {
    if (idCastingSpell >= 0) {
        castSpell(idCastingSpell, _hex.getAttribute("id"));
        return;
    }
    
    var _target = hexes[_hex.getAttribute("id")].hero;
    if (_target == null) {
        stepHero(hero, _hex);
    } else if (_target.team !== hero.team) {
        attackHero(hero, _hex);
    } else if (_target.team === hero.team) {
        
    }
}

function cancel() {
    idCastingSpell = -1;
    for (let index = 0; index < 5; index++) {
        clearHovers();
    } 
    clearHoversActions();
    hero = heroes[idActiveHero];
    console.log(hexes[idActiveHero]);
    fillFootHovers(hexes[hero.coordid], hexes);
    enableSpells(hero);
}