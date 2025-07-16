function sendMessage(message) {
    hubConnection.invoke("SendChatMessage", message)
        .catch(function (err) {
            return console.error(err.toString());
        });
}

hubConnection.on("ReceiveChatMessages", function (messages) {
    const messagesContainer = document.getElementById('messages');

    while (messagesContainer.firstChild) {
        messagesContainer.removeChild(messagesContainer.firstChild);
    }

    messages.forEach(message => {
        const newMessage = document.createElement('div');
        if (userId == message.chatterId) {
            newMessage.classList.add('message', 'sent');
            newMessage.textContent = message.message;
        }
        else {
            newMessage.classList.add('message', 'received');
            newMessage.textContent = message.chatterName + ": " + message.message;
        }
        messagesContainer.appendChild(newMessage);

        // Автоскролл к новому сообщению
        messagesContainer.scrollTop = messagesContainer.scrollHeight;
    });
});


const messageInput = document.getElementById('messageInput');
messageInput.addEventListener('keypress', function (e) {
    if (e.key === 'Enter' && this.value.trim() !== '') {
        sendMessage(this.value);
        this.value = '';
    }
});
