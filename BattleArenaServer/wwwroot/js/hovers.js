function mouseAttackHoverIn(_hex) {

    if(hexes[_hex.getAttribute("id")].hero.team !== hero.team)
    {
        const attackImg = document.createElement("img");
        attackImg.setAttribute("src", "Attack.png");
        attackImg.setAttribute("id", "attack" + _hex.getAttribute("id"));
        attackImg.setAttribute("style", "position: absolute; width: 115px; height: 115px; margin-left: -8px");
        _hex.appendChild(attackImg);
    }
}

function mouseAttackHoverOut(_hex) {
    if(hexes[_hex.getAttribute("id")].hero.team !== hero.team)
    {
        const attackImg = document.getElementById("attack" + _hex.getAttribute("id"));
        _hex.removeChild(attackImg);
    }
}

function mouseMoveHoverIn(_hex) {
    const moveImg = document.createElement("img");
    moveImg.setAttribute("src", "Move.png");
    moveImg.setAttribute("id", "move" + _hex.getAttribute("id"));
    moveImg.setAttribute("style", "position: absolute; width: 56px; padding-top: 24px; padding-left: 24px");
    _hex.appendChild(moveImg);
}

function mouseMoveHoverOut(_hex) {
    const moveImg = document.getElementById("move" + _hex.getAttribute("id"));
    if(moveImg != null || moveImg != undefined)
        _hex.removeChild(moveImg);
}


function fillFootHovers(_hex, _hexes) {

    var attackRange = _hex.hero.attackRadius + _hex.hero.statsEffect.attackRadius;
    if (_hex.hero.effectList.find(x => x.name === "Blind") != null)
        attackRange = 1;

    _hexes.forEach(el => {
        if (el.hero == null && getDistance(_hex, el) == 1)
        {
            const _hexHover = document.getElementById(el.id);
            _hexHover.setAttribute("onmouseenter", "mouseMoveHoverIn(this)");
            _hexHover.setAttribute("onmouseleave", "mouseMoveHoverOut(this)");
        } 
        else if (el.hero != null && getDistance(_hex, el) <= attackRange)
        {
            if (el.hero.team != _hex.hero.team) {
                const _hexHover = document.getElementById(el.id);
                _hexHover.setAttribute("onmouseenter", "mouseAttackHoverIn(this); feelHeroInfo(this)");
                _hexHover.setAttribute("onmouseleave", "mouseAttackHoverOut(this)");
            }
        }
        else
        {
            const _hexHover = document.getElementById(el.id);
            _hexHover.setAttribute("onmouseenter", "feelHeroInfo(this, -1)");
            _hexHover.removeAttribute("onmouseleave");
        }
    });
}


// �㭪樨 �뤥����� ������ ����⢨� ᯮᮡ����
function fillSpellAreaHovers(_spell, _item) {
    hexes.forEach(el => {
        const _hexHover = document.getElementById(el.id);
            _hexHover.setAttribute("onmouseenter", "mouseSpellHoverIn(this, " + _spell + ", '" + _item +"')");
    });
}

// �㭪樨 �뤥����� ������ ����⢨� ᯮᮡ����
function clearSpellAreaHovers() {
    hexes.forEach(el => {
        const _hexHover = document.getElementById(el.id);
            _hexHover.removeAttribute("onmouseenter");
            _hexHover.removeAttribute("onmouseleave");
    });
}

function mouseSpellHoverIn(_hex, _spell, _item) {
    if (_hex.id != idHexArea)
        getSpellArea(_hex.getAttribute("id"), heroes[idActiveHero].coordid, _spell, _item);
    idHexArea = _hex.id;
}

async function drawSpellArea(spellArea) {
    for (let index = 0; index < 5; index++) {
        clearHovers();
    }
    spellArea.forEach(el => {
        const _hexHover = document.getElementById(el);
        const attackImg = document.createElement("img");
        attackImg.setAttribute("src", "Attack.png");
        attackImg.setAttribute("class", "SpellHover");
        attackImg.setAttribute("style", "position: absolute; width: 115px; height: 115px; margin-left: -8px");
        _hexHover.appendChild(attackImg);
    });
}

function mouseSpellHoverOut(_hex, _spell) {
    for (let index = 0; index < 5; index++) {
        clearHovers();
    }
}

function clearHovers() {
    const attackImg = document.getElementsByClassName("SpellHover");
    for (let el of attackImg) {
        el.remove();
    }
}

function clearHoversActions() {
    hexes.forEach(el => {
        const _hexHover = document.getElementById(el.id);
        _hexHover.removeAttribute("onmouseenter");
        _hexHover.removeAttribute("onmouseleave");
    });
}
