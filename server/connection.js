import { User } from './user.js';

export class Connection{
    constructor(){
        this.socket = "";
        this.user = "";
        this.server = "";

        this.chat = "";
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

        socket.on('disconnect', () => {
            console.log("User disconnect server!");
        });

        socket.on('create-account', (data)=>{

        });
    }
}