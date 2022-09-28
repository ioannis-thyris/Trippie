$(async function () {
    let userInfo = await getUserInfo();

    let userId = userInfo.Id;
    let username = userInfo.name;
    let role = userInfo.role;

    let activeChats = [];
    let activeGroupnames = [];

    let container = document.querySelector('#chats-container');

    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;
    // Create a function that the hub can call back to display messages.
    chat.client.addPrivateMessage = function (groupname, name, message, roleTest) {
        let messages = document.querySelector(`#c-${groupname} .direct-chat-messages`);
        console.log(messages);

        messages.insertAdjacentHTML('beforeend', messageTemplate(name, message, roleTest));
    };


    chat.client.notify = function (groupname) {
        console.log(groupname);

        if (!activeGroupnames.includes(groupname)) {
            activeGroupnames.push(groupname);

            let chatEle = document.createElement('div');
            chatEle.innerHTML = chatTemplate(groupname);

            container.appendChild(chatEle);

            activeChats.push(chatEle);

            console.log(activeChats);

            let nameInput = chatEle.querySelector('.displayname');
            let messageInput = chatEle.querySelector('.message');
            let sendBtn = chatEle.querySelector('.sendmessage');
            let chatName = chatEle.querySelector('.box').id;

            console.log(chatName);

            // Get the user name and store it to prepend to messages.
            nameInput.value = username;
            // Set initial focus to message input box.
            //messageInput.focus();

            sendBtn.addEventListener('click', function () {
                // Call the Send method on the hub.

                if (messageInput.value) {
                    console.log('mpika!!');

                    chat.server.sendPrivate(groupname, nameInput.value, messageInput.value, role);

                    let chatContainer = document.querySelector('.direct-chat-messages');
                    chatContainer.scrollTop = chatContainer.scrollHeight;

                }
                // Clear text box and reset focus for next comment.
                messageInput.value = '';
                messageInput.focus();
            });
        }
    }


    // Start the connection.
    $.connection.hub.start().done(function () {
        chat.server.registerAdminInfo(userId, role);
    });


});
// This optional function html-encodes messages for display in the page.
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}


async function getUserInfo() {
    let response = await fetch('https://localhost:44397/Home/GetUserDetails');

    let userInfo;
    if (response.ok) {
        userInfo = await response.json();
    }
    console.log(userInfo);
    return userInfo;
}


function messageTemplate(name, message, role) {
    let date = new Date(Date.now()).toLocaleTimeString('en-US');

    let userTemplate = `
                    <div class="direct-chat-msg">
                        <div class="direct-chat-info clearfix">
                            <span class="direct-chat-name pull-left">${name}</span>
                            <span class="direct-chat-timestamp pull-right">${date}</span>
                        </div>
                        <!-- /.direct-chat-info -->
                        <img class="direct-chat-img" src="https://bootdey.com/img/Content/user_1.jpg" alt="Message User Image"><!-- /.direct-chat-img -->
                            <div class="direct-chat-text">
                                ${message}
                            </div>
                            <!-- /.direct-chat-text -->
                    </div>
                    `
    let adminTemplate = `
                        <div class="direct-chat-msg right">
                            <div class="direct-chat-info clearfix">
                                <span class="direct-chat-name pull-right">${name}</span>
                                <span class="direct-chat-timestamp pull-left">${date}</span>
                            </div>
                            <!-- /.direct-chat-info -->
                            <img class="direct-chat-img" src="https://bootdey.com/img/Content/user_2.jpg" alt="Message User Image"><!-- /.direct-chat-img -->
                            <div class="direct-chat-text">
                                ${message}
                            </div>
                            <!-- /.direct-chat-text -->
                        </div>
                        `
    return role === 'client' ? userTemplate : adminTemplate;
}


function chatTemplate(groupname) {
    return template = `
                    <div class="box box-admin box-primary direct-chat direct-chat-primary" id="c-${groupname}">
                        <div class="box-header with-border">
                            <h3 class="box-title admin-chat-title">Chat id: ${groupname}</h3>
                        </div>
                        <!-- /.box-header -->
                        <div class="box-body">
                            <!-- Conversations are loaded here -->
                            <div class="direct-chat-messages">

                            </div>
                        </div>
                        <!-- /.box-body -->
                        <div class="box-footer">
                            <div class="input-group">
                                <input type="hidden" class="displayname" />
                                <input type="text" name="message" placeholder="Type Message ..." class="form-control message" >
                                <span class="input-group-btn">
                                    <button class="btn btn-primary btn-flat sendmessage">Send</button>
                                </span>
                            </div>
                        </div>
                        <!-- /.box-footer-->
                    </div>
                    `
}