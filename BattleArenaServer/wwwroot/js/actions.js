async function getActiveHero() {
    hubConnection.invoke("SendActiveHero")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function setActiveHero(_idActiveHero) {
    hubConnection.invoke("SetActiveHero", parseInt(_idActiveHero))
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function stepHero(_hero, _hex) {
    if(_hex != undefined && _hero != undefined) {        
        hubConnection.invoke("StepHero", parseInt(_hero.coordid), parseInt(_hex.id))
            .catch(function (err) {
                return console.error(err.toString());
            });
    }
}

function attackHero(_hero, _hex) {
    if(_hex != undefined && _hero != undefined) {
        hubConnection.invoke("AttackHero", parseInt(_hero.coordid), parseInt(_hex.id))
            .catch(function (err) {
                return console.error(err.toString());
            });
    }
}

function endTurn() {
    if (isYourTurn == false)
        return;

    hubConnection.invoke("EndTurn")
        .catch(function (err) {
            return console.error(err.toString());
        });

    hubConnection.invoke("SendActiveHero")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function recreateGame() {
    hubConnection.invoke("RecreateGame")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function getField() {
    hubConnection.invoke("GetField")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function upgradeSkill(_skill) {
    hubConnection.invoke("UpgradeSkill", parseInt(heroes[idActiveHero].coordid), parseInt(_skill))
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function getSpellArea(target, caster, spell) {
    hubConnection.invoke("SendSpellArea", parseInt(target), parseInt(caster), parseInt(spell))
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function setUserBindings(_userId, _team) {
    hubConnection.invoke("BindingUserToTeam", _userId, _team)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function castSpell(_spell, _target = -1)
{
    console.log("castSpell");
    var _hero = heroes[idActiveHero];

    if (_hero.skillList[_spell].requireAP > _hero.ap)
        return;

    if (idCastingSpell != _spell & !_hero.skillList[_spell].nonTarget) {
        
        idCastingSpell = _spell;
        fillSpellAreaHovers(_spell);
        feelSpells(_hero);
        disableSpells(_spell);
        console.log("prepare casting");

        return;
    }
    var params = "target=" + _target + "&caster=" + _hero.coordid + "&spell=" + _spell;
    console.log(params);
    clearSpellAreaHovers();
    fillFootHovers(hexes[_hero.coordid], hexes)

    idCastingSpell = -1;
    console.log("CASTING");

    hubConnection.invoke("SpellCast", parseInt(_target), parseInt(_hero.coordid), parseInt(_spell))
        .catch(function (err) {
            return console.error(err.toString());
        });
}

// Установка cookie при первом посещении
function setUserCookie() {
    const userId = generateUniqueId(); // Генерируем уникальный ID
    const expires = new Date();
    expires.setFullYear(expires.getFullYear() + 1); // Cookie на 1 год

    document.cookie = `user_id=${userId}; expires=${expires.toUTCString()}; path=/`;
    return userId;
}

// Генерация уникального ID
function generateUniqueId() {
    return 'uid-' + Math.random().toString(36).substr(2, 9) + '-' + Date.now();
}

// Чтение cookie по имени
function getCookie(name) {
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        const [cookieName, cookieValue] = cookie.split('=').map(c => c.trim());
        if (cookieName === name) {
            return cookieValue;
        }
    }
    return null;
}

// Проверка наличия cookie пользователя
function getUserId() {
    let userId = getCookie('user_id');
    if (!userId) {
        userId = setUserCookie();
    }
    return userId;
}