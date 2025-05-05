var pickBanHeroes = [];

window.onload = async function () {

    userId = getCookie("user_id");
    if (userId == null || userId == undefined) {
        setUserCookie();
        userId = getCookie("user_id");
    }

    await hubConnection.start();

    await hubConnection.invoke("SendActiveHero")
        .catch(function (err) {
            return console.error(err.toString());
        });

    await hubConnection.invoke("GetHeroList")
        .catch(function (err) {
            return console.error(err.toString());
        });

    await hubConnection.invoke("GetUsersBindings")
        .catch(function (err) {
            return console.error(err.toString());
        });

    fillHeroInfo(null, "Abomination");
    chooseHero(null, "Abomination");
}

hubConnection.on("DrawPickedHeroes", async function (heroes, stage) {
    var pickedHeroes = [];
    const pickBanList = document.getElementById("pickBanList");
    const startBattleBtn = document.getElementById("startBattleBtn");
    while (pickBanList.firstChild) {
        pickBanList.removeChild(pickBanList.firstChild);
    }

    heroes.forEach(hero => {
        const heroDiv = document.createElement("div");
        heroDiv.setAttribute("id", hero.name);
        heroDiv.setAttribute("onclick", "chooseHero(this, '')");
        heroDiv.setAttribute("onmouseenter", "fillHeroInfo(this, '')");

        const heroImg = document.createElement("img");
        heroImg.setAttribute("src", "heroes/" + hero.name + ".png");
        heroImg.setAttribute("style", "width: 100px;");

        heroDiv.classList = "HeroPick";
        if (hero.team != "") {
            heroDiv.classList = "HeroPick Picked" + hero.team;
            pickedHeroes.push(hero);
        }
        if (hero.id == -1)
            heroDiv.classList = "HeroPick Banned";

        heroDiv.appendChild(heroImg);
        pickBanList.appendChild(heroDiv);

        pickBanHeroes.push(hero);
    });

    fillPickedHeroes(pickedHeroes);

    const pickBtn = document.getElementById("pickBtn");
    const pickBanImg = document.getElementById("pickBanImg");

    if (stage == 0) {//Пик
        pickBtn.setAttribute("onclick", "pickHero()");
        pickBanImg.setAttribute("src", "Pick.png");
        startBattleBtn.disabled = true;
        startBattleBtn.setAttribute("style", "filter: grayscale(1);");
    }
    else if (stage == 1) {//Бан
        pickBtn.setAttribute("onclick", "banHero()");
        pickBanImg.setAttribute("src", "Ban.png");
        startBattleBtn.disabled = true;
        startBattleBtn.setAttribute("style", "filter: grayscale(1);");
    }
    else {//Всё готово к сражению
        pickBtn.disabled = true;
        startBattleBtn.disabled = false;
        startBattleBtn.removeAttribute("style");
    }
});

// Заполнить инфу о герое
function fillHeroInfo(_div, _name) {
    let name = _name == "" ? _div.getAttribute("id") : _name;
    var _hero;
    _hero = pickBanHeroes.find(x => x.name == name);

    if (_hero != null) {
        heroInfo = _hero;

        const name = document.getElementById("heroinfo_name");
        name.innerText = _hero.name;

        const iconDiv = document.getElementById("heroinfo_icon");
        iconDiv.setAttribute("style", "background-color: lightgreen;");

        const icon = document.getElementById("heroinfo_icon_img");
        icon.setAttribute("src", "heroes/" + _hero.name + ".png");

        const dmg = document.getElementById("heroinfo_damage");
        dmg.innerText = _hero.dmg + _hero.statsEffect.dmg;

        const range = document.getElementById("heroinfo_range");
        range.innerText = _hero.attackRadius + _hero.statsEffect.attackRadius;

        const armor = document.getElementById("heroinfo_armor");
        armor.innerText = _hero.armor + _hero.statsEffect.armor;

        const resist = document.getElementById("heroinfo_resist");
        resist.innerText = _hero.resist + _hero.statsEffect.resist;

        const hp = document.getElementById("heroinfo_hp");
        hp.innerText = _hero.hp + " / " + _hero.maxHP;

        var hpPercent = 300 * _hero.hp / _hero.maxHP;
        const hpbar = document.getElementById("heroinfo_hpbar");
        hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: lightgreen;");

        const hpbarhp = document.getElementById("hpbar");
        hpbarhp.setAttribute("style", "background-color: lightgreen;");

        var i = 0;
        _hero.skillList.forEach(skill => {
            if (i > 4)
                return;

            const spell = document.getElementById("spellInfo" + i);
            const cdInfo = document.getElementById("cdInfo" + i);
            const spInfo = document.getElementById("spInfo" + i);
            spell.innerText = skill.name;
            if (skill.skillType == 1) {
                spell.setAttribute("class", "spell passive");
            }
            else if (skill.name === "X") {
                spell.setAttribute("class", "spell cooldawn");
                cdInfo.innerText = "";
            }
            else {
                spell.setAttribute("class", "spell");
                cdInfo.innerText = "";
            }
            i++;
        });
    }
}

// Заполнить инфу о выбранном герое
function chooseHero(_div, _name) {
    let name = _name == "" ? _div.getAttribute("id") : _name;
    var _hero;
    _hero = pickBanHeroes.find(x => x.name == name);

    if (_hero != null) {
        hero = _hero;

        const name = document.getElementById("heroinfo_name1");
        name.innerText = _hero.name;

        const iconDiv = document.getElementById("heroinfo_icon1");
        iconDiv.setAttribute("style", "background-color: lightgreen;");

        const icon = document.getElementById("heroinfo_icon_img1");
        icon.setAttribute("src", "heroes/" + _hero.name + ".png");

        const dmg = document.getElementById("heroinfo_damage1");
        dmg.innerText = _hero.dmg + _hero.statsEffect.dmg;

        const range = document.getElementById("heroinfo_range1");
        range.innerText = _hero.attackRadius + _hero.statsEffect.attackRadius;

        const armor = document.getElementById("heroinfo_armor1");
        armor.innerText = _hero.armor + _hero.statsEffect.armor;

        const resist = document.getElementById("heroinfo_resist1");
        resist.innerText = _hero.resist + _hero.statsEffect.resist;

        const hp = document.getElementById("heroinfo_hp1");
        hp.innerText = _hero.hp + " / " + _hero.maxHP;

        var hpPercent = 300 * _hero.hp / _hero.maxHP;
        const hpbar = document.getElementById("heroinfo_hpbar1");
        hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: lightgreen;");

        const hpbarhp = document.getElementById("hpbar1");
        hpbarhp.setAttribute("style", "background-color: lightgreen;");

        var i = 0;
        _hero.skillList.forEach(skill => {
            if (i > 4)
                return;

            const spell = document.getElementById("spell" + i);
            const spellUpg = document.getElementById("upgrade" + i);
            spellUpg.title = skill.titleUpg;
            spell.innerText = skill.name;
            if (skill.skillType == 1) {
                spell.setAttribute("class", "spell passive");
            }
            else if (skill.name === "X") {
                spell.setAttribute("class", "spell cooldawn");
            }
            else {
                spell.setAttribute("class", "spell");
            }
            i++;
        });

        const pickBtn = document.getElementById("pickBtn");
        if (_hero.id == -1 || _hero.team != "") {
            pickBtn.disabled = true;
            pickBtn.setAttribute("style", "filter: grayscale(1);");
        }
        else {
            pickBtn.disabled = false;
            pickBtn.setAttribute("style", "");
        }
    }
}

function fillPickedHeroes(_heroes) {
    _heroes.sort((h1, h2) => h1.id > h2.id ? 1 : -1);

    for (let x = 1; x <= 6; x++) {
        var margin = x > 1 ? "; margin-left: 6px;" : "";
        const banchHero = document.getElementById("banch_hero" + x);
        const iconBanchHero = document.getElementById("banch_icon" + x);

        banchHero.setAttribute("style", "display: inline-block; background-color: gray; " + margin);
        iconBanchHero.setAttribute("src", "heroes/None.png");
    }

    var ch = 1;
    var reds = 0;
    var blues = 0;
    isNeedRespawnPoint = false;
    _heroes.forEach(_hero => {
        var margin = ch > 1 ? "; margin-left: 6px;" : "";
        var cht = ch;

        var _color = "rgb(0, 99, 248)";
        if (_hero != null & _hero.team == "red") {
            _color = "red";
            cht = ch - reds;
            reds++;
        }
        else {
            cht = 6 - blues;
            blues++;
        }

        const banchHero = document.getElementById("banch_hero" + cht);
        const iconBanchHero = document.getElementById("banch_icon" + cht);

        banchHero.setAttribute("style", "display: inline-block; background-color: " + _color + margin);
        iconBanchHero.setAttribute("src", "heroes/" + _hero.name + ".png");

        ch++;
    });
}