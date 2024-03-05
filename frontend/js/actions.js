function stepHero(_hero, _hex) {
    if(_hex != undefined && _hero != undefined) {
        var params = "_cur_pos=" + _hero.coordid + "&_targer_pos=" + _hex.id;
    
        fetch("https://localhost:7241/Field/StepHero?" + params)
        .then(response => response.json())
        .then(coord => feelField(coord));
    }
}

function attackHero(_hero, _hex) {
    if(_hex != undefined && _hero != undefined) {
        var params = "_cur_pos=" + _hero.coordid + "&_targer_pos=" + _hex.id;
    
        fetch("https://localhost:7241/Field/AttackHero?" + params)
        .then(response => response.json())
        .then(coord => feelField(coord));
    }
}

function endTurn() {
    fetch("https://localhost:7241/Timing/EndTurn")
    .then(response => response.json())
    .then(init => idActiveHero = init)
    .then(init => getField()) ;

}

function getField() {
    fetch("https://localhost:7241/Field/GetField")
    .then(response => response.json())
    .then(coord => feelField(coord));    
}

function castSpell(_spell, _target = -1)
{
    if (idCastingSpell < 0 & !heroes[idActiveHero].skillList[_spell - 1].nonTarget) {
        idCastingSpell = _spell;
        fillSpellAreaHovers(_spell)
        return;
    }
    var params = "_target=" + _target + "&_caster=" + heroes[idActiveHero].coordid + "&_spell=" + _spell;

    idCastingSpell = -1;
    fetch("https://localhost:7241/Field/SpellCast?" + params)
    .then(response => response.json())
    .then(succses => 
        {
            if(succses)
                getField();
        });    
}