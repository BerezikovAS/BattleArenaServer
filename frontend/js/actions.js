async function getActiveHero() {
    var _idAH =
    await fetch("https://localhost:7241/Timing/GetActiveHero")
    .then(response => response.json())
    .then(_idActiveHero => {return _idActiveHero});
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
    .then(response => response.json())
    .then(init => idActiveHero = init)
    .then(init => getField());
}

function getField() {
    //fetch("https://localhost:7241/Field/GetField")
    //.then(response => response.json())
    //.then(coord => feelField(coord));    
}

function upgradeSkill(_skill) {
    fetch("https://localhost:7241/Field/UpgradeSkill?_caster=" + heroes[idActiveHero].coordid + "&_skill=" + _skill)
    .then(response => response.json())
    .then(succses => getField());
}

async function getSpellArea(_target, _caster, _spell) {
    var spellArea = await fetch("https://localhost:7241/Field/GetSpellArea?target=" + _target + "&caster=" + _caster + "&spell=" + _spell)
        .then(response => response.json())
        .then(arr => { return arr; });

    return spellArea;
}

function castSpell(_spell, _target = -1)
{
    var _hero = heroes[idActiveHero];

    if (_hero.skillList[_spell].requireAP > _hero.ap)
        return;

    if (idCastingSpell != _spell & !_hero.skillList[_spell].nonTarget) {
        
        idCastingSpell = _spell;
        fillSpellAreaHovers(_spell);
        feelSpells(_hero);
        disableSpells(_spell);

        return;
    }
    var params = "target=" + _target + "&caster=" + _hero.coordid + "&spell=" + _spell;
    console.log(params);
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