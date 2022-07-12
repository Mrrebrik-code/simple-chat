import { Connection } from "./Connection.js";
import { User } from "./User.js";


export class ServerCore{
    constructor(database){
        this.connections = [];
        this.database = database;
    }

    connect(socket){
        let server = this;
        let database = server.database;
        let connection = new Connection(database);
        let user = new User();

        connection.init(socket, user, server);

        let userId = user.id;
        server.connections[userId] = connection;

        console.log(`User [${userId}] connected to server!`);
        return connection;
    }
}