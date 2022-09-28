$(async function () {
    let userInfo = await getUserInfo();

    let userId = userInfo.id;
    let username = userInfo.name;
    let role = userInfo.role;

    let chatBtn = document.querySelector('#chatBtn');
    let chatContainer = document.querySelector('.box');

    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;

    // Create a function that the hub can call back to display messages.
    chat.client.addPrivateMessage = function (groupname, name, message, roleTest) {
        $('.direct-chat-messages').append(messageTemplate(name, message, roleTest));
    };

    let chatName;

    chat.client.notify = function (groupname) {
        console.log(groupname);
        chatName = groupname;
    }

    chatBtn.addEventListener('click', () => {
        chatContainer.hidden = !chatContainer.hidden;
        chatBtn.hidden = chatBtn.hidden ? false : true;

        // Start the connection.
        $.connection.hub.start().done(function () {
            chat.server.createUserPrivateChat(userId);

            $('#sendmessage').click(function () {
                // Call the Send method on the hub.
                if ($('#message').val()) {
                    chat.server.sendPrivate(chatName, $('#displayname').val(), $('#message').val(), role);

                    let chatContainer = document.querySelector('.direct-chat-messages');
                    chatContainer.scrollTop = chatContainer.scrollHeight;

                }
                // Clear text box and reset focus for next comment.
                $('#message').val('').focus();
            });
        });
    });

    let minimizeBtn = document.querySelector('#minimizeBtn');

    minimizeBtn.addEventListener('click', () => {
        chatContainer.hidden = !chatContainer.hidden;
        chatBtn.hidden = chatBtn.hidden ? false : true;
    });

    // Get the user name and store it to prepend to messages.
    $('#displayname').val(username);
    // Set initial focus to message input box.
    $('#message').focus();


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








































//$(async function () {
//    let userInfo = await getUserInfo();

//    let username = userInfo.name;
//    let role = userInfo.role;

//    let chatBtn = document.querySelector('#chatBtn');
//    let chatContainer = document.querySelector('.box');

//    chatBtn.addEventListener('click', () => {
//        chatContainer.hidden = !chatContainer.hidden;
//        chatBtn.hidden = chatBtn.hidden ? false : true;
//    });

//    let minimizeBtn = document.querySelector('#minimizeBtn');

//    minimizeBtn.addEventListener('click', () => {
//        chatContainer.hidden = !chatContainer.hidden;
//        chatBtn.hidden = chatBtn.hidden ? false : true;
//    });


//    // Reference the auto-generated proxy for the hub.
//    var chat = $.connection.chatHub;
//    // Create a function that the hub can call back to display messages.
//    chat.client.addNewMessageToPage = function (name, message, roleTest) {
//        // Add the message to the page.
//        console.log(roleTest);
//        $('.direct-chat-messages').append(messageTemplate(name, message, roleTest));
//    };


//    // Get the user name and store it to prepend to messages.
//    $('#displayname').val(username);
//    // Set initial focus to message input box.
//    $('#message').focus();
//    // Start the connection.
//    $.connection.hub.start().done(function () {
//        $('#sendmessage').click(function () {
//            // Call the Send method on the hub.
//            if ($('#message').val()) {
//                console.log(role);
//                chat.server.send($('#displayname').val(), $('#message').val(), role);
//                console.log(role);


//                let chatContainer = document.querySelector('.direct-chat-messages');
//                chatContainer.scrollTop = chatContainer.scrollHeight;

//            }
//            // Clear text box and reset focus for next comment.
//            $('#message').val('').focus();



//        });
//    });
//});
//// This optional function html-encodes messages for display in the page.
//function htmlEncode(value) {
//    var encodedValue = $('<div />').text(value).html();
//    return encodedValue;
//}


//async function getUserInfo() {
//    let response = await fetch('https://localhost:44397/Home/GetUserDetails');

//    let userInfo;
//    if (response.ok) {
//        userInfo = await response.json();
//    }
//    console.log(userInfo);
//    return userInfo;
//}


//function messageTemplate(name, message, role) {
//    let date = new Date(Date.now()).toLocaleTimeString('en-US');

//    let userTemplate = `
//                    <div class="direct-chat-msg">
//                        <div class="direct-chat-info clearfix">
//                            <span class="direct-chat-name pull-left">${name}</span>
//                            <span class="direct-chat-timestamp pull-right">${date}</span>
//                        </div>
//                        <!-- /.direct-chat-info -->
//                        <img class="direct-chat-img" src="https://bootdey.com/img/Content/user_1.jpg" alt="Message User Image"><!-- /.direct-chat-img -->
//                            <div class="direct-chat-text">
//                                ${message}
//                            </div>
//                            <!-- /.direct-chat-text -->
//                    </div>
//                    `
//    let adminTemplate = `
//                        <div class="direct-chat-msg right">
//                            <div class="direct-chat-info clearfix">
//                                <span class="direct-chat-name pull-right">${name}</span>
//                                <span class="direct-chat-timestamp pull-left">${date}</span>
//                            </div>
//                            <!-- /.direct-chat-info -->
//                            <img class="direct-chat-img" src="https://bootdey.com/img/Content/user_2.jpg" alt="Message User Image"><!-- /.direct-chat-img -->
//                            <div class="direct-chat-text">
//                                ${message}
//                            </div>
//                            <!-- /.direct-chat-text -->
//                        </div>
//                        `
//    return role === 'client' ? userTemplate : adminTemplate;
//}
