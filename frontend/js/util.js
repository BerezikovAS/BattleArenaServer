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
    var i = 1;
    _hero.skillList.forEach(skill => {
        const spell = document.getElementById("spell" + i);
        const cdInfo = document.getElementById("cd" + i);
        const spellDiv = document.getElementById("sp" + i);
        const spellUpg = document.getElementById("upgrade" + i);
        spell.innerText = skill.name;
        spellDiv.title = skill.title;
        spellUpg.title = skill.titleUpg;

        if(skill.coolDownNow > 0) {
            spell.setAttribute("class", "spell cooldawn");
            cdInfo.innerText = skill.coolDownNow;
        }
        else if(skill.requireAP > _hero.ap) {
            spell.setAttribute("class", "spell cooldawn");
        }
        else {
            spell.setAttribute("class", "spell");
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

    const btnEndTurn = document.getElementById("AP");
    while(btnEndTurn.firstChild) {
        btnEndTurn.removeChild(btnEndTurn.firstChild);
    }

    const apImg = document.createElement("img");
    apImg.setAttribute("src", hero.name + ".png");
    apImg.setAttribute("style", "width: 60px; padding: 3px;");
    btnEndTurn.appendChild(apImg);
}

// Получить все клетки попадающие под действие скила
// _spell.area == 0 - 
function getHexesSpellArea(_target, _caster, _spell) {
    var spellArea = [];

    switch (_spell.area) {
        case 1:
            if (getDistance(hexes[_target], hexes[_caster]) <= _spell.range) {
                if (hexes[_target].hero != null && hexes[_target].hero.team == hexes[_caster].hero.team)
                    spellArea.push(_target);
            }
            break;
        case 2:
            if (getDistance(hexes[_target], hexes[_caster]) <= _spell.range) {
                if (hexes[_target].hero != null && hexes[_target].hero.team != hexes[_caster].hero.team)
                    spellArea.push(_target);
            }
            break;
        case 3:
            let _dist = getDistance(hexes[_target], hexes[_caster]);
            if (_dist <= _spell.range && _dist > 0) {
                if (hexes[_target].hero != null && hexes[_target].hero.team == hexes[_caster].hero.team)
                    spellArea.push(_target);
            }
            break;
        case 4:
            // Тыкать нужно в радиусе досягаемости
            if (getDistance(hexes[_target], hexes[_caster]) <= _spell.range) {
                hexes.forEach(hex => {
                    // Нашли нужный гекс, от 
                    if (getDistance(hex, hexes[_target]) <= _spell.radius) {
                        spellArea.push(hex.id);
                    }
                });
            }
            break;
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


// Получить процент сопротивления/брони
function getPercentResist(_value) {
    var percent = Math.round((0.1 * _value) / (1 + 0.1 * _value) * 100, 0) + "%";
    return percent;
}

// Заполнить инфу о герое
function feelHeroInfo(_hex) {    
    var _hero = hexes[_hex.getAttribute("id")].hero;

    var _color = "red";
    var _excolor = "rgb(238, 150, 150)";
    if (_hero != null & _hero.team == "blue") {
        _color = "rgb(0, 81, 255)";
        _excolor = "rgb(135, 165, 228)";
    }

    const name = document.getElementById("heroinfo_name");
    name.innerText = _hero.name;

    const icon = document.getElementById("heroinfo_icon");    
    icon.setAttribute("style", "background-image: url(" + _hero.name + ".png); background-color: "+ _hero.team);

    const dmg = document.getElementById("heroinfo_damage");
    dmg.innerText = _hero.dmg;

    const range = document.getElementById("heroinfo_range");
    range.innerText = _hero.attackRadius;

    const armor = document.getElementById("heroinfo_armor");
    armor.innerText = _hero.armor + " (" + getPercentResist(_hero.armor) + ")";

    const resist = document.getElementById("heroinfo_resist");
    resist.innerText = _hero.resist + " (" + getPercentResist(_hero.resist) + ")";

    const hp = document.getElementById("heroinfo_hp");
    hp.innerText = _hero.hp;

    var hpPercent = 300 * _hero.hp / _hero.maxHP;
    const hpbar = document.getElementById("heroinfo_hpbar");
    hpbar.setAttribute("style", "width: " + hpPercent + "px; background-color: " + _color);

    const hpbarhp = document.getElementById("hpbar");
    hpbarhp.setAttribute("style", "background-color: " + _excolor);
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
        effect.setAttribute("style", "position: absolute; padding-left: " + pl + "px; padding-top: " + pt + "px;");
        _hex.appendChild(effect);
        i++;
        if (i > 3 && i <= 7)
            k++;
    });    
}

function enableUpgrades(_hero) {
    let i = 1;
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