async function banHero() {
    if (isYourTurn == false)
        return;
    const pickBtn = document.getElementById("pickBtn");
    pickBtn.disabled = true;
    pickBtn.setAttribute("style", "filter: grayscale(1);");
    hubConnection.invoke("BanHero", hero.name)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function pickHero() {
    if (isYourTurn == false)
        return;
    const pickBtn = document.getElementById("pickBtn");
    pickBtn.disabled = true;
    pickBtn.setAttribute("style", "filter: grayscale(1);");
    hubConnection.invoke("PickHero", hero.name)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function getShopItems() {
    hubConnection.invoke("GetShopItems")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function buyItem(itemName) {
    if (isYourTurn == false)
        return;
    hubConnection.invoke("BuyItem", parseInt(hero.id), itemName)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function sellItem(itemName) {
    if (isYourTurn == false)
        return;
    hubConnection.invoke("SellItem", parseInt(hero.id), itemName)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

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
    await getShopItems();
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
    if (isNeedRespawnPoint == true)
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

    window.location.href = 'http://localhost:8080';
}


function beginBattle() {
    hubConnection.invoke("BeginBattle")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function beginRandomBattle() {
    hubConnection.invoke("BeginRandomBattle")
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

function getSpellArea(target, caster, spell, _item) {
    hubConnection.invoke("SendSpellArea", parseInt(target), parseInt(caster), parseInt(spell), _item)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function getRespawnArea(heroId) {
    hubConnection.invoke("SendRespawnArea", parseInt(heroId))
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

function setRespawnedHero(_idHero, _hexId) {
    hubConnection.invoke("RespawnHero", parseInt(_idHero), parseInt(_hexId))
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function castItem(_itemName, _target = -1) {

    idCastingSpell = -1;
    var _hero = heroes[idActiveHero];
    var item = undefined;
    _hero.items.forEach(it => {
        if (it.name == _itemName)
            item = it;
    });

    if (item != undefined && item.skill.coolDownNow > 0) {
        nameCastingItem = "";
        return;
    }

    if ((item != undefined && item.skill.requireAP > _hero.ap) || isYourTurn == false)
        return;

    if (nameCastingItem != _itemName & !item.skill.nonTarget) {

        nameCastingItem = _itemName;
        fillSpellAreaHovers(-1, _itemName);
        //feelSpells(_hero);
        //disableSpells(_spell);

        return;
    }

    clearSpellAreaHovers();
    fillFootHovers(hexes[_hero.coordid], hexes)

    nameCastingItem = "";

    hubConnection.invoke("ItemCast", parseInt(_target), parseInt(_hero.coordid), _itemName)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

function castSpell(_spell, _target = -1)
{
    nameCastingItem = "";
    var _hero = heroes[idActiveHero];

    var availableAP = _hero.ap;
    var spLink = _hero.effectList.find(x => x.name === "SpiritLink");
    if (spLink != null) {
        var anotherHero = heroes.find(y => y != undefined && y.effectList.find(z => z.name === "SpiritLink") != null && y.id != _hero.id);
        if (anotherHero != null)
            availableAP += anotherHero.ap;
    }
    if (_hero.skillList[_spell].requireAP > availableAP || isYourTurn == false)
        return;

    if (idCastingSpell != _spell & !_hero.skillList[_spell].nonTarget) {
        
        idCastingSpell = _spell;
        fillSpellAreaHovers(_spell, "");
        feelSpells(_hero);
        disableSpells(_spell);

        return;
    }

    clearSpellAreaHovers();
    fillFootHovers(hexes[_hero.coordid], hexes)

    idCastingSpell = -1;

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