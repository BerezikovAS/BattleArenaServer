// Получить расстояние от клетки до клетки
function getDistance (_hexSource, _hexTarget) {
    var dist = 0;
    for (let i = 0; i < 3; i++) {
        if (Math.abs(_hexSource.coord[i] - _hexTarget.coord[i]) >= dist)
            dist = Math.abs(_hexSource.coord[i] - _hexTarget.coord[i]);
    }
    return dist;
}

// Получить процент оставшегося ХП
function getPercentHP(_hero) {
    return 115.47 - (_hero.hp / _hero.maxHP) * 115.47;     
}

// Заполнить инфу по активному герою
function feelActiveHeroInfo(_hero) {
    feelAP(_hero.ap);
    feelSpells(_hero)
}

// Заполнить инфу по скилам
function feelSpells(_hero) {
    var i = 0;
    var availableAP = _hero.ap;

    var spLink = _hero.effectList.find(x => x.name === "SpiritLink");
    if (spLink != null) {
        var anotherHero = heroes.find(y => y != undefined && y.effectList.find(z => z.name === "SpiritLink") != null && y.id != _hero.id);
        if (anotherHero != null)
            availableAP += anotherHero.ap;
    }

    _hero.skillList.forEach(skill => {
        if (i > 4)
            return;
        const spell = document.getElementById("spell" + i);
        const cdInfo = document.getElementById("cd" + i);
        const spellDiv = document.getElementById("sp" + i);
        const spellUpg = document.getElementById("upgrade" + i);
        spell.innerText = skill.name;
        spellUpg.title = skill.titleUpg;

        if (skill.skillType == 1) {
            spell.setAttribute("class", "spell passive");
        }
        else if (skill.coolDownNow > 0) {
            spell.setAttribute("class", "spell cooldawn");
            spellDiv.setAttribute("onclick", "");
            cdInfo.innerText = skill.coolDownNow;
        }
        else if (skill.requireAP > availableAP || _hero.effectList.find(x => x.effectTags.find(y => y == 1)) != null) {
            spell.setAttribute("class", "spell cooldawn");
            spellDiv.setAttribute("onclick", "");
            cdInfo.innerText = "";
        }
        else if (skill.skillType == 0) {
            spell.setAttribute("class", "spell");
            spellDiv.setAttribute("onclick", "castSpell(" + i + ")");
            cdInfo.innerText = "";
        }
        i++;
    });
}

// Заполнить очки действий
function feelAP(_ap) {
    const AProw = document.getElementById("AProw");
    while(AProw.firstChild) {
        AProw.removeChild(AProw.firstChild);
    }

    for(var i = 0; i < _ap; i++) {
        const apImg = document.createElement("img");
        apImg.setAttribute("src", "ap.png");
        apImg.setAttribute("style", "width: 40px; padding: 3px");
        AProw.appendChild(apImg);
    }

    var _hero = heroes[idActiveHero];
    var _color = "red";
    var _excolor = "rgb(238, 150, 150)";
    if (_hero != null & _hero.team == "blue") {
        _color = "rgb(0, 99, 248)";
        _excolor = "rgb(135, 165, 228)";
    }

    const iconDiv = document.getElementById("hero_icon");
    iconDiv.setAttribute("style", "margin-bottom: 20px; background-color: " + _color);

    const icon = document.getElementById("hero_icon_img");
    icon.setAttribute("src", "heroes/" + _hero.name + ".png");

    const hp = document.getElementById("heroinfo_hpcur");
    hp.innerText = _hero.hp + " / " + _hero.maxHP;

    var hpPercent = 300 * _hero.hp / _hero.maxHP;
    const hpbar = document.getElementById("heroinfo_hpbarcur");
    hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: " + _color);

    const hpbarhp = document.getElementById("hpbarcur");
    hpbarhp.setAttribute("style", "margin-top: 14px; background-color: " + _excolor);

    const dmg = document.getElementById("active_heroinfo_damage");
    dmg.innerText = _hero.dmg + _hero.statsEffect.dmg;

    const range = document.getElementById("active_heroinfo_range");
    range.innerText = _hero.attackRadius + _hero.statsEffect.attackRadius;

    const armor = document.getElementById("active_heroinfo_armor");
    armor.innerText = _hero.armor + _hero.statsEffect.armor;

    const resist = document.getElementById("active_heroinfo_resist");
    resist.innerText = _hero.resist + _hero.statsEffect.resist;


    let cnt = 1;
    for (var i = 1; i <= 3; i++) {
        const itemBtn = document.getElementById("itemBtn" + i);
        const itemDiv = document.getElementById("itemDiv" + i);
        itemBtn.removeAttribute("src");
        itemDiv.className = "item";
        const cdItem = document.getElementById("cdItem" + i);
        cdItem.innerText = "";
    }

    _hero.items.forEach(item => {
        const itemBtn = document.getElementById("itemBtn" + cnt);
        const itemDiv = document.getElementById("itemDiv" + cnt);
        itemBtn.setAttribute("src", "items/" + item.name + ".png");
        itemBtn.setAttribute("onclick", "castItem('" + item.name + "')");
        itemDiv.classList.add("level" + item.level);
        itemDiv.title = item.description;

        if (item.skill.coolDownNow > 0 || _hero.effectList.find(x => x.effectTags.find(y => y == 20))) {
            itemDiv.classList.add("cooldawn");
            const cdItem = document.getElementById("cdItem" + cnt);
            if (item.skill.coolDownNow > 0)
                cdItem.innerText = item.skill.coolDownNow;
        }
        cnt++;
    });



    fillEffectsOnHeroInfo(_hero, "statusbar");
}

// Проверить находятся ли клетки на одной линии
function isOnLine(_hexCaster, _hexTarget) {
    if (getDistance (_hexCaster, _hexTarget) == 0)
        return false;
    for (var i = 0; i < 3; i++) {
        if (_hexTarget.coord[i] == _hexCaster.coord[i])
            return true;
    }
    return false;
}

// Получить направление для клеток на одной линии
function getDirection(_hexCaster, _hexTarget)
{
    var dist = getDistance (_hexCaster, _hexTarget);
    if (dist == 0)
        return false;
    else {
        var coord = new Array();
        coord[0] = (_hexCaster.coord[0] - _hexTarget.coord[0]) / dist;
        coord[1] = (_hexCaster.coord[1] - _hexTarget.coord[1]) / dist;
        coord[2] = (_hexCaster.coord[2] - _hexTarget.coord[2]) / dist;
        return coord;
    }
}

// Получить направление для клеток на одной линии
function getDirectionHex(_hexCaster, _hexTarget) {
    var dist = getDistance(_hexCaster, _hexTarget);
    if (dist == 0)
        return false;
    else {
        var h = [];
        h.coord[0] = (_hexCaster.coord[0] - _hexTarget.coord[0]) / dist;
        h.coord[1] = (_hexCaster.coord[1] - _hexTarget.coord[1]) / dist;
        h.coord[2] = (_hexCaster.coord[2] - _hexTarget.coord[2]) / dist;
        return h;
    }
}


function disableSpells(_spell) {
    for (let index = 1; index <= 4; index++) {
        const element = document.getElementById("sp" + index);
        element.removeAttribute("onclick");

        if (_spell != index)
            element.disable = true;
        else {
            const btn = document.getElementById("spell" + index);
            btn.setAttribute("class", "spellActive");
        }
    }
}

function enableSpells(_hero) {
    for (let index = 1; index <= 4; index++) {
        const element = document.getElementById("sp" + index);
        element.setAttribute("onclick", "castSpell(" + index + ")");
        element.disable = false;
    }
    feelSpells(_hero);
}

function fillPercentResist(isShow)
{
    var _hero = heroes[idActiveHero];
    const armor = document.getElementById("heroinfo_armor");
    const resist = document.getElementById("heroinfo_resist");

    const active_armor = document.getElementById("active_heroinfo_armor");
    const active_resist = document.getElementById("active_heroinfo_resist");

    if (isShow)
    {
        armor.innerText = getPercentResist(heroInfo.armor + heroInfo.statsEffect.armor);
        resist.innerText = getPercentResist(heroInfo.resist + heroInfo.statsEffect.resist);

        active_armor.innerText = getPercentResist(_hero.armor + _hero.statsEffect.armor);
        active_resist.innerText = getPercentResist(_hero.resist + _hero.statsEffect.resist);
    }
    else
    {
        armor.innerText = heroInfo.armor + heroInfo.statsEffect.armor;
        resist.innerText = heroInfo.resist + heroInfo.statsEffect.resist;

        active_armor.innerText = _hero.armor + _hero.statsEffect.armor;
        active_resist.innerText = _hero.resist + _hero.statsEffect.resist;
    }
}


// Получить процент сопротивления/брони
function getPercentResist(_value) {
    var percent = Math.round((0.1 * _value) / (1 + 0.1 * _value) * 100, 0) + "%";
    return percent;
}

function fillBanchHeroes(_heroes) {
    var ch = 1;
    isNeedRespawnPoint = false;
    _heroes.forEach(_hero => {
        const banchHero = document.getElementById("banch_hero" + ch);
        const iconBanchHero = document.getElementById("banch_icon" + ch);

        //console.log(_heroes);

        var margin = ch > 1 ? "; margin-left: 6px;" : "";
        var gray = _hero.respawnTime < 1 ? " filter: grayscale(100%);" : "";

        var _color = "rgb(0, 99, 248)";
        if (_hero != null & _hero.team == "red" & gray == "")
            _color = "red";

        banchHero.setAttribute("style", gray + " display: inline-block; background-color: " + _color + margin);
        iconBanchHero.setAttribute("src", "heroes/" + _hero.name + ".png");

        const cdInfo = document.getElementById("cdBanch" + ch);
        if (_hero.respawnTime > 1) {
            cdInfo.innerText = _hero.respawnTime - 1;
        }
        else {
            cdInfo.innerText = "";
        }

        if (_hero.respawnTime == 1) // надо выставить героя
            isNeedRespawnPoint = true;

        banchHeroes[ch] = _hero;

        ch++;
    });
}

// Заполнить инфу о герое
function feelHeroInfo(_hex, _hexId) {
    var _hero;
    if (_hexId > 0)
        _hero = hexes[_hexId].hero;
    else
        _hero = hexes[_hex.getAttribute("id")].hero;

    if (_hero != null) {

        heroInfo = _hero;

        var _color = "red";
        var _excolor = "rgb(238, 150, 150)";
        if (_hero != null & _hero.team == "blue") {
            _color = "rgb(0, 99, 248)";
            _excolor = "rgb(135, 165, 228)";
        }

        const name = document.getElementById("heroinfo_name");
        name.innerText = _hero.name;

        const iconDiv = document.getElementById("heroinfo_icon");
        iconDiv.setAttribute("style", "background-color: " + _color);

        const icon = document.getElementById("heroinfo_icon_img");
        if (_hero.type == 1)
            icon.setAttribute("src", "obstacles/" + _hero.name + ".png");
        else
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

        //console.log(_hero);
        var hpPercent = 300 * _hero.hp / _hero.maxHP;
        const hpbar = document.getElementById("heroinfo_hpbar");
        hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: " + _color);

        const hpbarhp = document.getElementById("hpbar");
        hpbarhp.setAttribute("style", "background-color: " + _excolor);

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
            else if (skill.coolDownNow > 0) {
                spell.setAttribute("class", "spell cooldawn");
                cdInfo.innerText = skill.coolDownNow;
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

        let cnt = 1;
        for (var i = 1; i <= 3; i++) {
            const itemBtn = document.getElementById("itemBtnInfo" + i);
            const itemDiv = document.getElementById("itemDivInfo" + i);
            itemBtn.removeAttribute("src");
            itemDiv.className = "item";
            const cdItem = document.getElementById("cdItemInfo" + i);
            cdItem.innerText = "";
        }

        _hero.items.forEach(item => {
            const itemBtn = document.getElementById("itemBtnInfo" + cnt);
            const itemDiv = document.getElementById("itemDivInfo" + cnt);
            itemBtn.setAttribute("src", "items/" + item.name + ".png");
            itemDiv.classList.add("level" + item.level);
            itemDiv.title = item.description;

            if (item.skill.coolDownNow > 0) {
                itemDiv.classList.add("cooldawn");
                const cdItem = document.getElementById("cdItemInfo" + cnt);
                cdItem.innerText = item.skill.coolDownNow;
            }
            cnt++;
        });

        fillEffectsOnHeroInfo(_hero, "statusbar_info");
    }
}

function fillEffectsOnHeroInfo(_hero, _panel)
{
    const statusBar = document.getElementById(_panel);
    while(statusBar.firstChild) {
        statusBar.removeChild(statusBar.firstChild);
    }

    _hero.effectList.forEach(effect => {
        var statusclass = "statusbaritem buff";
        if (effect.type == 1)
            statusclass = "statusbaritem debuff"

        const status = document.createElement("div");
        status.setAttribute("class", statusclass);
        status.setAttribute("style", "background-image: url(\"effects/" + effect.name + ".png\");");
        status.title = effect.description;

        if (effect.name == "PhysShield" || effect.name == "MagicShield" || effect.name == "DmgShield")
            status.innerText = effect.value;
        statusBar.appendChild(status);
    });
}

function fillEffectsOnHex(_hex, _hero)
{
    let i = 0;
    let k = 0;

    var _effects = [];
    _hero.effectList.forEach(ef => {
        if (_effects.find(x => x.name === ef.name) == null)
            _effects.push(ef);
    });


    _effects.forEach(ef => {
        let pl = 2 + k * 20;
        let pt = i > 5 ? (26 + (10 - i) * 16 - (4 - k) * 5) : (26 + i * 16 - k * 5);

        const effect = document.createElement("img");
        effect.setAttribute("src", "effects/" + ef.name + ".png");
        effect.setAttribute("style", "position: absolute; padding-left: " + pl + "px; padding-top: " + pt + "px; width: 16px; height: 16px;");
        _hex.appendChild(effect);
        i++;
        if (i > 3 && i <= 7)
            k++;

        let color = "";
        switch (ef.name) {
            case "Paralysis":
                color = "rgba(45, 0, 255, 0.5);"
                break;
            case "Smoke":
                color = "rgba(0, 0, 0, 0.3);"
                break;
            case "GuardianAngel":
                color = "rgba(255, 255, 255, 0.2);"
                break;
            case "DeepFreeze":
                color = "rgba(39, 234, 245, 0.4);"
                break;
            default:
                break;
        }
        if (color != "") {
            const eff = document.createElement("div");
            eff.setAttribute("style", "position: absolute; width: 102px; height: 117px; background-color: " + color + " margin-left: -2px; margin-top: -1px; opacity 0.3;");
            _hex.appendChild(eff);
        }
    });    
}

function enableUpgrades(_hero) {
    let i = 0;
    _hero.skillList.forEach(skill => {
        if (i > 4)
            return;
        const spellUp = document.getElementById("upgrade" + i);
        if(_hero.upgradePoints > 0 & !skill.upgraded) {
            spellUp.disable = false;
            spellUp.setAttribute("style", "");
        }
        else {
            spellUp.disable = true;
            spellUp.setAttribute("style", "visibility: hidden;");
        }
        i++;
    });
}

function showSpellInfo(_spell, _spellBox, _heroFlag)
{
    var spell = heroInfo.skillList[_spell];
    if (_heroFlag == 0)
        spell = heroes[idActiveHero].skillList[_spell];
    else if (_heroFlag == 2)
        spell = hero.skillList[_spell];

    var skillInfoName = document.getElementById("skillInfoName");
    skillInfoName.innerText = spell.name;

    var skillInfoDescr = document.getElementById("skillInfoDescr");
    skillInfoDescr.innerText = spell.title;

    var skillInfoStatsRange = document.getElementById("skillInfoStatsRange");
    skillInfoStatsRange.innerText = spell.range;

    var skillInfoStatsRadius = document.getElementById("skillInfoStatsRadius");
    if (spell.radius > 0) {
        skillInfoStatsRadius.setAttribute("style", "display: flex; position: absolute; right: 100px;");
        var skillInfoStatsRadiusVal = document.getElementById("skillInfoStatsRadiusVal");
        skillInfoStatsRadiusVal.innerText = spell.radius;
    }
    else
        skillInfoStatsRadius.setAttribute("style", "visibility: hidden;");
    
    var skillInfoStatsCoolDown = document.getElementById("skillInfoStatsCoolDown");
    skillInfoStatsCoolDown.innerText = spell.coolDown;
        
    var skillInfoStatsAP = document.getElementById("skillInfoStatsAP");
    skillInfoStatsAP.innerText = spell.requireAP;

    var skillInfoStatsDmg = document.getElementById("skillInfoStatsDmg");
    if (spell.dmg > 0 || spell.extraDmgStr != "") {
        var color = "red";
        skillInfoStatsDmg.setAttribute("style", "display: flex;");
        var skillInfoStatsDmgVal = document.getElementById("skillInfoStatsDmgVal");
        
        if (spell.dmg > 0)
            skillInfoStatsDmgVal.innerText = spell.dmg;
        else
            skillInfoStatsDmgVal.innerText = spell.extraDmgStr;

        switch (spell.dmgType) {
            case 1:
                color = "blue";
                break;
            case 2:
                color = "yellow; -webkit-text-stroke: 0.2px white";
                break;
            default:
                break;
        }
        skillInfoStatsDmgVal.setAttribute("style", "color: " + color + ";");
    }
    else
        skillInfoStatsDmg.setAttribute("style", "display: flex; visibility: hidden;");



    
    var coordBox = _spellBox.getBoundingClientRect();
    var skillInfo = document.getElementById("skillInfo");

    skillInfo.setAttribute("class", "skillInfo skillInfoShow");
    skillInfo.setAttribute("style", "z-index: 99; top: " + (coordBox.top + globalThis.scrollY) + "px; left: " + (coordBox.left + globalThis.scrollX - 230) + "px;");
}

function hideSpellInfo()
{
    var skillInfo = document.getElementById("skillInfo");
    skillInfo.setAttribute("class", "skillInfo skillInfoHide");
    skillInfo.setAttribute("style", "z-index: -99; top: 0; left: 0; visibility: collapsed;");
}

function getHexesDirections() {
    hexesDir = [];
    hexesDir[0].coord = [1, -1, 0];
    hexesDir[1].coord = [1, 0, -1];
    hexesDir[2].coord = [0, 1, -1];
    hexesDir[3].coord = [-1, 1, 0];
    hexesDir[4].coord = [-1, 0, 1];
    hexesDir[5].coord = [0, -1, 1];
    return hexesDir;
}

function showVP(_VP) {
    var redVP = _VP.redVP;
    var blueVP = _VP.blueVP;

    var redVPText = document.getElementById("vp_red");
    var blueVPText = document.getElementById("vp_blue");
    var vpLine = document.getElementById("vp_line_c");
    var victory = document.getElementById("victory");

    redVPText.innerText = redVP;
    blueVPText.innerText = blueVP;

    var width = 649 * redVP / (redVP + blueVP);
    vpLine.setAttribute("style", "width: " + width + "px; height: 90px; background-color: red;");

    if (redVP >= 100) {
        victory.innerText = "ПОБЕДА КРАСНЫХ!";
        victory.removeAttribute("style");
        victory.setAttribute("class", "Victory VictoryRed");
    }
    else if (blueVP >= 100) {
        victory.innerText = "ПОБЕДА СИНИХ!";
        victory.removeAttribute("style");
        victory.setAttribute("class", "Victory VictoryBlue");
    }
    else
        victory.setAttribute("style", "visibility: collapse;");

    //Также отобразим монетки
    const coins = document.getElementById("team_coins");
    if (activeTeam == "red")
        coins.innerText = _VP.redCoins;
    else
        coins.innerText = _VP.blueCoins;
}

function fillShop(items) {
    const shop = document.getElementById("shop");
    while (shop.firstChild) {
        shop.removeChild(shop.firstChild);
    }

    items.forEach(item => {
        const shIt = document.createElement("div");
        const img = document.createElement("img");
        const btn = document.createElement("div");

        let itemHave = false;
        hero.items.forEach(it => {
            if (it.name == item.name)
                itemHave = true;
        });
        shIt.title = item.description;

        img.setAttribute("style", "width: 80px;");
        img.setAttribute("src", "items/" + item.name + ".png");


        if (itemHave) {
            shIt.setAttribute("class", "ShopItem Sell");
            btn.setAttribute("class", "ShopButton Sell");
            btn.setAttribute("onclick", "sellItem('" + item.name + "')");
            btn.innerText = "SELL " + item.sellCost;
        }
        else if (item.amount <= 0) {
            shIt.setAttribute("class", "ShopItem Sold");
            btn.setAttribute("class", "ShopButton Sold");
            btn.innerText = "SOLD";
        }
        else {
            shIt.setAttribute("class", "ShopItem lvl" + item.level);
            btn.setAttribute("class", "ShopButton");
            btn.setAttribute("onclick", "buyItem('" + item.name + "')");
            btn.innerText = "BUY " + item.cost;
        }
        shop.appendChild(shIt);
        shIt.appendChild(img);
        shIt.appendChild(btn);

    });
}

//1 - показать
//0 - спрятать
function showHideShop(mode) {
    const shop = document.getElementById("shop");
    const btn = document.getElementById("showShop");
    console.log(shop);
    if (mode == 1) {
        shop.setAttribute("style", "visibility: visible;");
        btn.setAttribute("onclick", "showHideShop(0)");
    }
    else {
        shop.setAttribute("style", "visibility: collapse;");
        btn.setAttribute("onclick", "showHideShop(1)");
    }
}