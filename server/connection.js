import { User } from './User.js';


export class Connection{
    constructor(database){
        this.socket = "";
        this.user = "";
        this.server = "";

        this.chat = "";
        this.database = database;
    }

    init(socket, user, server){
        let connection = this;
        connection.socket = socket;
        connection.user = user;
        connection.server = server;
    }

    createEvents(){
        let connection = this;
        let socket = connection.socket;
        let server = connection.server;
        let user = connection.user;
        let database = connection.database;

        socket.on('disconnect', () => {
            console.log("User disconnect server!");
        });

        socket.on('disconnecting', async ()=>{
            if(user.currentChat != null){
                var isRemoveCurrentUser = await database.removeCurrentUserToChat(user);

                if(isRemoveCurrentUser){
                    let userData = {
                        nickname: user.nickname,
                        id: user.id
                    };

                    let jsonLeaveUser = JSON.stringify(userData);
                    socket.to(user.currentChat).emit("leave-chat-target-user", jsonLeaveUser);

                    socket.leave(user.currentChat);

                    console.log(`Leave current chat name ${user.currentChat}`);
                    user.currentChat = null;
                }else{
                    console.log(`Error Leave chat!`);
                }

            }else{
                console.log("User not chat!");
            }
        });

        socket.on('register-account', async (data) =>{
            let userData = JSON.parse(data);
            userData["Id"] = user.id; 

            let isCreate = await database.createUser(userData.Id, userData.Nickname, userData.Password);
            
            if(isCreate){
                console.log("Create user to database!");

                let json = JSON.stringify(userData);
                socket.emit("register-user", json);
            }else{
                console.log("Error creating user.");
            }
        });

        socket.on('login-account', async (data) =>{
            let userData = JSON.parse(data);
            let nickname = userData["Nickname"];
            let password = userData["Password"];

            let isNickname = await database.tryUserNickname(nickname);
            if(isNickname){
                console.log("Nickname good!");

                let userLogin = await database.tryUserPassword(nickname, password);

                if(userLogin != null){
                    userData["Id"] = userLogin.id;
                    user.id = userLogin.id;
                    user.nickname = nickname;


                    console.log(`User [${user.id}] logined to account!`);

                    let json = JSON.stringify(userData);
                    socket.emit("login-user", json);
                }else{
                    console.log(`Error logined user. Failed password!`);
                }
            }else{
                console.log(`Error logined user. Failed nickname!`);
            }

        });

        socket.on('create-chat', async (data) =>{
            let chatData = JSON.parse(data);

            let chatName = chatData["Name"];
            let chatPassword = chatData["Password"];

            let isCreate = await database.createChat(chatName, chatPassword);
            
            if(isCreate){
                console.log("Create chat to database!");

                let json = JSON.stringify(chatData);
                socket.emit("crate-chat", json);
            }else{
                console.log("Error creating chat.");
            }
        });

        socket.on('join-chat', async (data) =>{
            let chatData = JSON.parse(data);
            let name = chatData["Name"];
            let password = chatData["Password"];

            let chatCallback = {
                name: name,
                password, password
            }

            let isName = await database.tryChatName(name);
            if(isName){
                var isPassword = await database.tryChatPassword(name, password);

                if(isPassword){
                    let isAddUserToChat = await database.addCurrentUserToChat(name, user);

                    if(isAddUserToChat){
                        console.log("Add user to chat!");
                        user.currentChat = name;
                        
                        socket.join(user.currentChat);

                        let userData = {
                            nickname: user.nickname,
                            id: user.id
                        };
    
                        let jsonJoinUser = JSON.stringify(userData);

                        socket.to(user.currentChat).emit("join-chat-target-user", jsonJoinUser);
                    }

                    let usersChat = await database.getUsersToChatName(name);

                    let chatDataTemp = {
                        chat: {
                            nameChat: name,
                            passwordChat: password
                        },
                        usersChat
                    }

                    let json = JSON.stringify(chatDataTemp);
                    socket.emit("join-chat", json);

                }else{
                    console.log("Error joined to chat. Failed password!");
                }
            }else{
                console.log("Error joined to chat. Failed name!");
            }
        });

        socket.on('leave-chat-current-user', async () =>{
            if(user.currentChat != null){
                var isRemoveCurrentUser = await database.removeCurrentUserToChat(user);

                if(isRemoveCurrentUser){
                    let userData = {
                        nickname: user.nickname,
                        id: user.id
                    };

                    let jsonLeaveUser = JSON.stringify(userData);
                    socket.to(user.currentChat).emit("leave-chat-target-user", jsonLeaveUser);

                    socket.leave(user.currentChat);

                    console.log(`Leave current chat name ${user.currentChat}`);
                    user.currentChat = null;
                }else{
                    console.log(`Error Leave chat!`);
                }

            }else{
                console.log("User not chat!");
            }
        });

        socket.on('send-message-chat', async(data) =>{
            let messageData = JSON.parse(data);
            console.log(messageData);

            let textMessage = messageData["Text"];
            let userMessage = {
                nickname: messageData["User"]["Nickname"],
                id: messageData["User"]["Id"]
            }

            console.log(userMessage);

            let json = JSON.stringify(messageData);
            socket.to(user.currentChat).emit('user-message-chat', json);
        });
    }
}