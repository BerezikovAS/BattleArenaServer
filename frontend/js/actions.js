async function getActiveHero() {
    var _idAH =
    await fetch("https://localhost:7241/Timing/GetActiveHero")
    .then(response => response.json())
    .then(_idActiveHero => {return _idActiveHero});
    console.log("idAH = " + _idAH);
    return _idAH;
}

function stepHero(_hero, _hex) {
    if(_hex != undefined && _hero != undefined) {
        var params = "cur_pos=" + _hero.coordid + "&targer_pos=" + _hex.id;
    
        fetch("https://localhost:7241/Field/StepHero?" + params)
        .then(response => response.json())
        .then(coord => feelField(coord));
    }
}

function attackHero(_hero, _hex) {
    if(_hex != undefined && _hero != undefined) {
        var params = "cur_pos=" + _hero.coordid + "&targer_pos=" + _hex.id;
    
        fetch("https://localhost:7241/Field/AttackHero?" + params)
        .then(response => response.json())
        .then(coord => feelField(coord));
    }
}

function endTurn() {
    fetch("https://localhost:7241/Timing/EndTurn")
    .then(init => getField()) ;
}

function getField() {
    fetch("https://localhost:7241/Field/GetField")
    .then(response => response.json())
    .then(coord => feelField(coord));    
}

function upgradeSkill(_skill) {
    fetch("https://localhost:7241/Field/UpgradeSkill?_caster=" + heroes[idActiveHero].coordid + "&_skill=" + _skill)
    .then(response => response.json())
    .then(succses => getField());
}

function castSpell(_spell, _target = -1)
{
    var _hero = heroes.find(x => x.id === idActiveHero);

    if (_hero.skillList[_spell - 1].requireAP > _hero.ap)
        return;

    if (idCastingSpell != _spell & !_hero.skillList[_spell - 1].nonTarget) {
        idCastingSpell = _spell;
        fillSpellAreaHovers(_spell);
        disableSpells(_spell);

        return;
    }
    var params = "target=" + _target + "&caster=" + _hero.coordid + "&spell=" + _spell;
    clearSpellAreaHovers();
    fillFootHovers(hexes[_hero.coordid], hexes)

    idCastingSpell = -1;
    fetch("https://localhost:7241/Field/SpellCast?" + params)
    .then(response => response.json())
    .then(succses => 
        {
            if(succses)
                getField();
        });    
}