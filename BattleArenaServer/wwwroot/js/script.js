const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/fieldhub")
    .build();

hubConnection.on("GetField", function (_field) {
    console.log("Receive field");
    feelField(_field);
});

hubConnection.on("DrawSpellArea", async function (spellArea) {
    //console.log("Receive spell area");
    await drawSpellArea(spellArea);
});

hubConnection.on("GetActiveHero", function (idActHero) {
    console.log("Receive active hero");
    idActiveHero = idActHero;
    hero = heroes[idActiveHero];
    enableUpgrades(hero);
    feelActiveHeroInfo(hero);
    enableSpells(hero);
    fillFootHovers(hexes[hero.coordid], hexes);
});

const field = document.getElementById("field");
var hexes = [];
var heroes = [];
var mode = "Move";
var hero;
var idActiveHero = 0;
var idCastingSpell = -1;
var heroInfo;
var idHexArea = -1;

window.onload = async function() {
    await hubConnection.start();

    await hubConnection.invoke("SendActiveHero")
        .catch(function (err) {
            return console.error(err.toString());
        });

    await hubConnection.invoke("GetField")
        .catch(function (err) {
            return console.error(err.toString());
        });
    feelHeroInfo(0, hero.coordid);
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
        case "KeyT":
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
            const obstImg = document.createElement("img");
            obstImg.setAttribute("src", "obstacles/" + element.obstacle.name + ".png");
            obstImg.setAttribute("style", "position: absolute; width: 80px; padding-left: 10px; padding-top: 16px;");
            hex.appendChild(obstImg);
        }
        if (element.hero == null) {
            element.surfaces.forEach(
                surface => {
                    const obstImg = document.createElement("img");
                    obstImg.setAttribute("src", "obstacles/" + surface.name + ".png");
                    obstImg.setAttribute("style", "position: absolute; width: 120px; margin-left: -10px; z-index: -4;");
                    hex.appendChild(obstImg);
                }
            );
        }
        
        field.appendChild(hex);
        hexes.push(element);
    });
    //heroes = heroes.toSorted((a, b) => a.id - b.id);
    //idActiveHero = await getActiveHero();

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

    if (_target != null && _target.team == hero.team) {
        setActiveHero(_target.id);
        //idActiveHero = _target.id;
        //hero = _target;
        //enableUpgrades(_target);
        //feelActiveHeroInfo(_target);
        //enableSpells(_target);
        //fillFootHovers(hexes[_target.coordid], hexes);

    } else if (_target == null) {
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