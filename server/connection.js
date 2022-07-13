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

        });

        socket.on('join-chat', async (data) =>{
            
        });

        socket.on('leave-chat', async () =>{
            
        });
    }
}