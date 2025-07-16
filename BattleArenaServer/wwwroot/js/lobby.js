const hubConnection = new signalR.HubConnectionBuilder()
    //.withUrl("http://localhost:8080/lobbyhub", { withCredentials: false, mode: 'no-cors' })
    .withUrl("http://10.80.3.137:8080/lobbyhub", { withCredentials: false, mode: 'no-cors' })
    //.withUrl("http://130.193.45.251:8080/lobbyhub", { withCredentials: false, mode: 'no-cors' }) //ip виртуалки
    .build();

hubConnection.on("RefreshLobbies", function (lobbies) {
    fillLobbies(lobbies);
});

async function createLobbyAction(name) {
    hubConnection.invoke("CreateLobby", name)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function deleteLobbyAction(lobbyId) {
    hubConnection.invoke("DeleteLobby", lobbyId)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

async function sendLobbies() {
    hubConnection.invoke("SendLobbies")
        .catch(function (err) {
            return console.error(err.toString());
        });
}

var lobbiyArray = [];
var userId = "";
var nickname = "";
const createLobbyDiv = document.getElementById("createLobby");
const createLobbyName = document.getElementById("createLobbyName");
const enterNicknameForm = document.getElementById("enterNicknameForm");
const enterNickname = document.getElementById("enterNickname");
const userHello = document.getElementById("userHello");

window.onload = async function () {

    userId = getCookie("user_id");
    lobbyId = getCookie("lobbyId");
    nickname = getCookie("nickname");

    if (userId == null || userId == undefined || lobbyId == null || lobbyId == undefined) {
        setUserCookie();
        userId = getCookie("user_id");
    }

    if (nickname == null || nickname == "" || nickname == undefined) {
        enterNicknameForm.style.visibility = 'visible';
        const enterNicknameCancel = document.getElementById("enterNicknameCancel");
        enterNicknameCancel.style.disabled = true;

        userHello.innerText = "Добро пожаловать!";
    }
    else {
        userHello.innerText = "Добро пожаловать, " + nickname + "!";
    }

    await hubConnection.start();
}

function replaceAfter(str, search, replacement) {
    const regex = new RegExp(`${search}=[^;]*;?`, "g");
    return str.replace(regex, `${replacement};`);
}

function createLobby() {

    if (lobbiyArray.find(x => x.creatorId == userId) == undefined)
        createLobbyDiv.style.visibility = 'visible';
}

function createLobbyHandler() {
    const createLobbyErrors = document.getElementById("createLobbyErrors");
    if (createLobbyName.value.length < 3)
        createLobbyErrors.innerText = "Название лобби не может содержать менее 3-х символов!";
    else {
        createLobbyErrors.innerText = "";
        createLobbyAction(createLobbyName.value);
        createLobbyName.value = "";
        createLobbyDiv.style.visibility = 'collapse';
    }
}

function cancelCreateLobby() {
    createLobbyName.value = "";
    createLobbyDiv.style.visibility = 'collapse';
}

function deleteLobby(lobbyId) {
    deleteLobbyAction(lobbyId);
}

function setUserNickname() {
    const enterNicknameErrors = document.getElementById("enterNicknameErrors");
    if (enterNickname.value.length < 3)
        enterNicknameErrors.innerText = "Никнейм не может быть менее 3-х символов!";
    else {
        const expires = new Date();
        expires.setFullYear(expires.getFullYear() + 1); // Cookie на 1 год
        enterNicknameErrors.innerText = "";
        document.cookie = `nickname=${enterNickname.value}; expires=${expires.toUTCString()}; path=/`;
        userHello.innerText = "Добро пожаловать, " + enterNickname.value + "!";
        nickname = enterNickname.value;
        enterNicknameForm.style.visibility = 'collapse';
    }
}

function editUserNickname() {
    enterNickname.value = nickname;
    enterNicknameForm.style.visibility = 'visible';
}

function cancelEditUserNickname() {
    if (nickname != "")
        enterNicknameForm.style.visibility = 'collapse';
}

function fillLobbies(lobbies) {

    //console.log(lobbies);
    const lobbiesDiv = document.getElementById("lobbiesDiv");
    while (lobbiesDiv.firstChild) {
        lobbiesDiv.removeChild(lobbiesDiv.firstChild);
    }

    lobbiyArray = [];
    lobbies.forEach(lobby => {

        lobbiyArray.push(lobby);

        // 1. Создаем основной контейнер с grid
        const container = document.createElement('div');
        container.style.display = 'grid';
        container.style.gridTemplateColumns = '10fr 1fr 3fr 3fr'; // 4 колонки
        container.style.gap = '10px'; // Отступ между элементами
        container.style.alignItems = 'center';
        container.style.padding = '8px';
        container.style.backgroundColor = 'lightgreen';
        container.style.borderRadius = '25px 5px';
        container.style.textShadow = '1px 1px 2px black';
        container.style.fontSize = '22px';
        container.style.fontWeight = 'bold';
        container.style.color = 'white';
        container.style.border = '3px solid rgb(1, 73, 1)';
        container.style.marginBottom = '8px';

        // 2. Добавляем текстовые ячейки
        const textCell1 = document.createElement('div');
        textCell1.textContent = lobby.name;

        const textCell2 = document.createElement('div');
        textCell2.textContent = lobby.countUsers + '/2';

        // 3. Создаем кнопки
        const button1 = document.createElement('button');

        button1.textContent = 'Войти';
        button1.style.display = 'inline-flex';
        button1.style.alignItems = 'center';
        button1.style.justifyContent = 'center';
        button1.style.backgroundColor = 'lightgreen';
        button1.style.border = '2px solid rgb(1, 73, 1)';
        button1.style.borderRadius = '15px 5px';
        button1.style.color = 'white';
        button1.style.textShadow = '1px 1px 2px black';
        button1.style.fontWeight = 'bold';
        button1.style.fontSize = '18px';
        button1.style.height = '44px';
        button1.setAttribute("onclick", "joinLobby('" + lobby.id + "', '" + lobby.isStarted + "')");

        container.appendChild(textCell1);
        container.appendChild(textCell2);
        container.appendChild(button1);

        if (lobby.creatorId == userId) {
            const button2 = document.createElement('button');
            button2.textContent = 'Удалить';
            button2.style.display = 'inline-flex';
            button2.style.alignItems = 'center';
            button2.style.justifyContent = 'center';
            button2.style.backgroundColor = 'lightgreen';
            button2.style.border = '2px solid rgb(1, 73, 1)';
            button2.style.borderRadius = '15px 5px';
            button2.style.color = 'white';
            button2.style.textShadow = '1px 1px 2px black';
            button2.style.fontWeight = 'bold';
            button2.style.fontSize = '18px';
            button2.style.height = '44px';
            button2.setAttribute("onclick", "deleteLobby('" + lobby.id + "')");

            container.appendChild(button2);
        }

        // 4. Добавляем все элементы в контейнер

        lobbiesDiv.appendChild(container);
    });
}

function joinLobby(lobbyId, isStarted) {

    const expires = new Date();
    expires.setFullYear(expires.getFullYear() + 1); // Cookie на 1 год

    document.cookie = `lobbyId=${lobbyId}; expires=${expires.toUTCString()}; path=/`;

    if (isStarted == "true")
        window.location.href = "http://10.80.3.137:8080/Battle.html";
    else
        window.location.href = "http://10.80.3.137:8080/Pick.html";
}