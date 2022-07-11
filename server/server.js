import { Connection } from "./connection.js";
import { User } from "./user.js";

export class ServerCore{
    constructor(){
        this.connections = [];
    }

    connect(socket){
        let server = this;
        let connection = new Connection();
        let user = new User();

        connection.init(socket, user, server);

        let userId = user.id;
        server.connections[userId] = connection;

        console.log(`User [${userId}] connected to server!`);
        return connection;
    }
}