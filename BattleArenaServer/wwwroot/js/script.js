const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:8080/fieldhub", { withCredentials: false, mode: 'no-cors' })
    //.withUrl("http://130.193.45.251:8080/fieldhub", { withCredentials: false, mode: 'no-cors' }) //ip виртуалки
    .build();

hubConnection.on("GetField", function (_field) {
    //console.log("Receive field");
    feelField(_field);
});

hubConnection.on("GetVP", function (_VP) {
    showVP(_VP);
});

hubConnection.on("GetBanchHeroes", function (_banchHeroes) {
    //console.log("Receive field");
    fillBanchHeroes(_banchHeroes);
});

hubConnection.on("SetShopItems", function (_items) {
    shopItems = _items;
    fillShop(shopItems);
});

hubConnection.on("BeginBattle", function () {
    window.location.href = 'http://localhost:8080/Battle.html';
});

hubConnection.on("RecreateGame", function () {
    window.location.href = 'http://localhost:8080';
});

hubConnection.on("GetUsersBindings", (_bindings) => {

    var redTeam = document.getElementById("redTeam");
    var blueTeam = document.getElementById("blueTeam");
    var teams = document.getElementById("team_btns");
    var vp_line = document.getElementById("vp_line");
    var vp_line = document.getElementById("vp_line");
    var turn = document.getElementById("turn");
    let isPickBan = vp_line == null;
    let addClass = isPickBan ? "PickBan" : "";
    let addStyle = isPickBan ? "margin-top: -100px; margin-left: 20px;" : "";

    activeTeam = _bindings.activeTeamStr;
    
    if (_bindings.activeTeam == userId) {
        isYourTurn = true;
        turn.innerText = "YOUR TURN";
        if (activeTeam == "red")
            turn.setAttribute("class", "Turn " + addClass + " VictoryRed");
        else
            turn.setAttribute("class", "Turn " + addClass + " VictoryBlue");
    }
    else {
        isYourTurn = false;
        turn.setAttribute("class", "Turn " + addClass + " Gray");
        turn.innerText = "OPPONENT TURN";
    }
    
    if (isPickBan) {
        if (_bindings.redTeam != "")
            redTeam.setAttribute("style", "visibility: hidden;");
        else
            redTeam.setAttribute("style", "visibility: visible;");
    
        if (_bindings.blueTeam != "")
            blueTeam.setAttribute("style", "visibility: hidden; margin-left: 100px;");
        else
            blueTeam.setAttribute("style", "visibility: visible; margin-left: 100px;");

        var startRandomBattle = document.getElementById("startRandomBattleBtn");
        
        if (_bindings.redTeam != "" && _bindings.blueTeam != "") {
            startRandomBattle.removeAttribute("style");
            startRandomBattle.disabled = false;
        }
        else {
            startRandomBattle.setAttribute("style", "filter: grayscale(1);");
            startRandomBattle.disabled = true;
        }
    }

    if (_bindings.redTeam != "" && _bindings.blueTeam != "") {
        teams.setAttribute("style", "visibility: collapse; height: 10px; " + addStyle);
        turn.removeAttribute("style");
        if (!isPickBan)
            vp_line.setAttribute("style", "width: 860px; display: inline-block;");
    }
    else {
        teams.setAttribute("style", "" + addStyle);
        turn.setAttribute("style", "visibility: collapse;");
        if (!isPickBan)
            vp_line.setAttribute("style", "width: 860px; display: inline-block; visibility: collapse;");
    }

    //showVP(_bindings);
});

hubConnection.on("DrawSpellArea", async function (spellArea) {
    //console.log("Receive spell area");
    await drawSpellArea(spellArea);
});

hubConnection.on("GetActiveHero", function (idActHero) {
    //console.log("Receive active hero");
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
var banchHeroes = [];
var mode = "Move";
var hero;
var idActiveHero = 0;
var idCastingSpell = -1;
var heroInfo;
var idHexArea = -1;
var isYourTurn = false;
var userId = "";
var activeTeam = "red";
var isNeedRespawnPoint = false;
var selectedBanchHero = -1;
var shopItems;
var nameCastingItem = "";

window.onload = async function () {

    userId = getCookie("user_id");
    if (userId == null || userId == undefined) {
        setUserCookie();
        userId = getCookie("user_id");
    }
    //console.log("user_id: " + userId);

    await hubConnection.start();

    await hubConnection.invoke("SendActiveHero")
        .catch(function (err) {
            return console.error(err.toString());
        });

    await hubConnection.invoke("GetField")
        .catch(function (err) {
            return console.error(err.toString());
        });

    await hubConnection.invoke("GetUsersBindings")
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

        if (element.vp == 1)
            hex.setAttribute("class", "VP");
        else if (element.vp == 2)
            hex.setAttribute("class", "TwoVP");
        else if (element.teamShop == "red")
            hex.setAttribute("class", "RedShopHex");
        else if (element.teamShop == "blue")
            hex.setAttribute("class", "BlueShopHex");
        else
            hex.setAttribute("class", "NonVP");

        


        if(element.hero != null) {

            const teamImg = document.createElement("img");
            if(element.hero.team == "red")
                teamImg.setAttribute("src", "red_back.png");
            else
                teamImg.setAttribute("src", "blue_back.png");
            teamImg.setAttribute("style", "position: absolute; height: " + getPercentHP(element.hero) + "px; width: 100px;");

            const heroImg = document.createElement("img");
            heroImg.setAttribute("src", "heroes/" + element.hero.name + ".png");
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
            if(element.hero.type !== 1)
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

    hero = heroes[idActiveHero];
    if (hero === undefined) {
        idActiveHero = await getActiveHero();
        hero = heroes[idActiveHero];
    }

    enableUpgrades(hero);
    feelActiveHeroInfo(hero);
    fillFootHovers(hexes[hero.coordid], hexes);
    enableSpells(hero);
}

function hexClick(_hex) {
    if (isYourTurn == false)
        return;

    if (selectedBanchHero != -1) {
        setRespawnedHero(selectedBanchHero, _hex.id);
        selectedBanchHero = -1;
        return;
    }

    selectedBanchHero = -1;

    if (idCastingSpell >= 0) {
        castSpell(idCastingSpell, _hex.getAttribute("id"));
        return;
    }

    if (nameCastingItem != "") {
        castItem(nameCastingItem, _hex.getAttribute("id"));
        return;
    }
    
    var _target = hexes[_hex.getAttribute("id")].hero;

    if (_target != null && _target.team == activeTeam) {
        setActiveHero(_target.id);
    } else if (_target == null) {
        stepHero(hero, _hex);
    } else if (_target.team !== hero.team) {
        attackHero(hero, _hex);
    } else if (_target.team === hero.team) {
        
    }
}

function cancel() {
    idCastingSpell = -1;
    nameCastingItem = ""
    for (let index = 0; index < 5; index++) {
        clearHovers();
    } 
    clearHoversActions();
    hero = heroes[idActiveHero];
    fillFootHovers(hexes[hero.coordid], hexes);
    enableSpells(hero);
}

function bindUserToTeam(_team) {
    setUserBindings(userId, _team);
}

function selectBanchHero(_idBanchHero) {
    if (banchHeroes[_idBanchHero].respawnTime == 1) {
        selectedBanchHero = banchHeroes[_idBanchHero].id;
        getRespawnArea(selectedBanchHero);
    }
}