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
    console.log(_hero);
    feelAP(_hero.ap);
    feelSpells(_hero)
}

// Заполнить инфу по скилам
function feelSpells(_hero) {
    var i = 0;
    _hero.skillList.forEach(skill => {
        const spell = document.getElementById("spell" + i);
        const cdInfo = document.getElementById("cd" + i);
        const spellDiv = document.getElementById("sp" + i);
        const spellUpg = document.getElementById("upgrade" + i);
        spell.innerText = skill.name;
        //spellDiv.title = skill.title;
        spellUpg.title = skill.titleUpg;

        if (skill.skillType == 1) {
            spell.setAttribute("class", "spell passive");
        }
        if (skill.coolDownNow > 0) {
            spell.setAttribute("class", "spell cooldawn");
            spellDiv.setAttribute("onclick", "");
            cdInfo.innerText = skill.coolDownNow;
        }
        else if (skill.requireAP > _hero.ap || _hero.effectList.find(x => x.name === "Silence") != null) {
            spell.setAttribute("class", "spell cooldawn");
            spellDiv.setAttribute("onclick", "");
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
    icon.setAttribute("src", _hero.name + ".png");

    const hp = document.getElementById("heroinfo_hpcur");
    hp.innerText = _hero.hp + " / " + _hero.maxHP;

    var hpPercent = 300 * _hero.hp / _hero.maxHP;
    const hpbar = document.getElementById("heroinfo_hpbarcur");
    hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: " + _color);

    const hpbarhp = document.getElementById("hpbarcur");
    hpbarhp.setAttribute("style", "margin-top: 14px; background-color: " + _excolor);

    fillEffectsOnHeroInfo(_hero, "statusbar");
}

// Получить все клетки попадающие под действие скила
// _spell.area == 0 - 
function getHexesSpellArea(_target, _caster, _spell) {
    var spellArea = [];
    //console.log("spell area: " + _spell.area);

    var spellRange = _spell.range;
    if (hexes[_caster].hero.effectList.find(x => x.name === "Blind") != null)
        spellRange = 1;

    switch (_spell.area) {
        // Ally target
        case 1:
            if (getDistance(hexes[_target], hexes[_caster]) <= spellRange) {
                if (hexes[_target].hero != null && hexes[_target].hero.team == hexes[_caster].hero.team)
                    spellArea.push(_target);
            }
            break;
        // Enemy target
        case 2:
            if (getDistance(hexes[_target], hexes[_caster]) <= spellRange) {
                if (hexes[_target].hero != null && hexes[_target].hero.team != hexes[_caster].hero.team)
                    spellArea.push(_target);
            }
            break;
        // Friend target (not himself)
        case 3:
            let _dist = getDistance(hexes[_target], hexes[_caster]);
            if (_dist <= spellRange && _dist > 0) {
                if (hexes[_target].hero != null && hexes[_target].hero.team == hexes[_caster].hero.team)
                    spellArea.push(_target);
            }
            break;
        // Radius area
        case 4:
            // Тыкать нужно в радиусе досягаемости
            if (getDistance(hexes[_target], hexes[_caster]) <= spellRange) {
                hexes.forEach(hex => {
                    // Нашли нужный гекс, от 
                    if (getDistance(hex, hexes[_target]) <= _spell.radius) {
                        spellArea.push(hex.id);
                    }
                });
            }
            break;
        // Line area
        case 5:
            if (isOnLine(hexes[_target], hexes[_caster]))
            {
                var coord = getDirection(hexes[_target], hexes[_caster]);
                if (coord)
                {
                    for (let i = 1; i < _spell.radius + 1; i++) {
                        var _hex = hexes.find(n => 
                            n.coord[0] == hexes[_caster].coord[0] + i * coord[0] &&
                            n.coord[1] == hexes[_caster].coord[1] + i * coord[1] &&
                            n.coord[2] == hexes[_caster].coord[2] + i * coord[2]);
                        if (_hex)
                            spellArea.push(_hex.id);
                    }
                }
            }
            break;
        // Hero target
        case 7:
            if (getDistance(hexes[_target], hexes[_caster]) <= spellRange) {
                if (hexes[_target].hero != null)
                    spellArea.push(_target);
            }
        // Hero not self target
        case 8:
            if (getDistance(hexes[_target], hexes[_caster]) <= spellRange) {
                if (hexes[_target].hero != null && hexes[_target].hero.id != hexes[_caster].hero.id)
                    spellArea.push(_target);
            }
        default:
            break;
    }

    
    // console.log(spellArea);
    return spellArea;
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
    const armor = document.getElementById("heroinfo_armor");
    const resist = document.getElementById("heroinfo_resist");
    if (isShow)
    {
        armor.innerText = getPercentResist(heroInfo.armor + heroInfo.statsEffect.armor);
        resist.innerText = getPercentResist(heroInfo.resist + heroInfo.statsEffect.resist);
    }
    else
    {
        armor.innerText = heroInfo.armor + heroInfo.statsEffect.armor;
        resist.innerText = heroInfo.resist + heroInfo.statsEffect.resist;
    }
}


// Получить процент сопротивления/брони
function getPercentResist(_value) {
    var percent = Math.round((0.1 * _value) / (1 + 0.1 * _value) * 100, 0) + "%";
    return percent;
}

// Заполнить инфу о герое
function feelHeroInfo(_hex, _hexId) {
    var _hero;
    if (_hexId > 0)
        _hero = hexes[_hexId].hero;
    else
        _hero = hexes[_hex.getAttribute("id")].hero;

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
    if (_hero.type == 0)
        icon.setAttribute("src", _hero.name + ".png");
    else
        icon.setAttribute("src", "obstacles/" + _hero.name + ".png");

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
    hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: " + _color);

    const hpbarhp = document.getElementById("hpbar");
    hpbarhp.setAttribute("style", "background-color: " + _excolor);

    var i = 0;
    _hero.skillList.forEach(skill => {
        const spell = document.getElementById("spellInfo" + i);
        const cdInfo = document.getElementById("cdInfo" + i);
        const spInfo = document.getElementById("spInfo" + i);
        spell.innerText = skill.name;
        //spInfo.title = skill.title;
        if (skill.skillType == 1) {
            spell.setAttribute("class", "spell passive");
        }
        else if (skill.coolDownNow > 0) {
            spell.setAttribute("class", "spell cooldawn");
            cdInfo.innerText = skill.coolDownNow;
        }
        else {
            spell.setAttribute("class", "spell");
            cdInfo.innerText = "";
        }
        i++;
    });

    fillEffectsOnHeroInfo(_hero, "statusbar_info");
}

function fillEffectsOnHeroInfo(_hero, _panel)
{
    const statusBar = document.getElementById(_panel);
    while(statusBar.firstChild) {
        statusBar.removeChild(statusBar.firstChild);
    }

    _hero.effectList.forEach(effect => {
        const status = document.createElement("button");
        status.setAttribute("class", "statusbaritem");
        status.setAttribute("style", "background-image: url(\"effects/" + effect.name + ".png\");");
        status.title = effect.description;
        statusBar.appendChild(status);
    });
}

function fillEffectsOnHex(_hex, _hero)
{
    let i = 0;
    let k = 0;

    _hero.effectList.forEach(ef => {
        let pl = 2 + k * 20;
        let pt = i > 5 ? (26 + (10 - i) * 16 - (4 - k) * 5) : (26 + i * 16 - k * 5);

        const effect = document.createElement("img");
        effect.setAttribute("src", "effects/" + ef.name + ".png");
        effect.setAttribute("style", "position: absolute; padding-left: " + pl + "px; padding-top: " + pt + "px; width: 16px; height: 16px;");
        _hex.appendChild(effect);
        i++;
        if (i > 3 && i <= 7)
            k++;
    });    
}

function enableUpgrades(_hero) {
    let i = 0;
    _hero.skillList.forEach(skill => {
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
    skillInfo.setAttribute("style", "top: " + (coordBox.top + globalThis.scrollY) + "px; left: " + (coordBox.left + globalThis.scrollX - 230) + "px;");
}

function hideSpellInfo()
{
    var skillInfo = document.getElementById("skillInfo");
    skillInfo.setAttribute("class", "skillInfo skillInfoHide");
    skillInfo.setAttribute("style", "top: -100px; left: -100px;");
}