function getDistance (_hexSource, _hexTarget) {
    var dist = 0;
    for (let i = 0; i < 3; i++) {
        if (Math.abs(_hexSource.coord[i] - _hexTarget.coord[i]) >= dist)
            dist = Math.abs(_hexSource.coord[i] - _hexTarget.coord[i]);
    }
    return dist;
}

function getPercentHP(_hero) {
    return 115.47 - (_hero.hp / _hero.maxHP) * 115.47;     
}

function feelActiveHeroInfo(_hero) {
    console.log(_hero);
    feelAP(_hero.ap);
    feelSpells(_hero)
}

function feelSpells(_hero) {
    var i = 1;
    _hero.skillList.forEach(skill => {
        const spell = document.getElementById("spell" + i);
        const cdInfo = document.getElementById("cd" + i);
        spell.innerText = skill.name;

        if(skill.coolDownNow > 0)
        {
            spell.setAttribute("class", "spell cooldawn");
            cdInfo.innerText = skill.coolDownNow;
        }
        else
        {
            spell.setAttribute("class", "spell");
            cdInfo.innerText = "";
        }
        i++;
    });
}

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

function getHexesSpellArea(_target, _caster, _spell) {
    var spellArea = [];
    if(getDistance(hexes[_target], hexes[_caster]) <= _spell.range) {
        hexes.forEach(hex => {
            if(getDistance(hex, hexes[_target]) <= _spell.radius) {
                spellArea.push(hex.id);
            }
        });
    }
    return spellArea;
}

function getPercentResist(_value) {
    var percent = Math.round((0.1 * _value) / (1 + 0.1 * _value) * 100, 0) + "%";
    return percent;
}

function feelHeroInfo(_hex) {
    var _hero = hexes[_hex.getAttribute("id")].hero;

    const name = document.getElementById("heroinfo_name");
    name.innerText = _hero.name;

    const icon = document.getElementById("heroinfo_icon");    
    icon.setAttribute("style", "background-image: url(" + _hero.name + ".png);");

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
    console.log(hpPercent);
    const hpbar = document.getElementById("heroinfo_hpbar");
    hpbar.setAttribute("style", "width: " + hpPercent + "px;");
}