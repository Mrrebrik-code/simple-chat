import { createServer } from 'http';
import { Server } from 'socket.io';
import { ServerCore } from './ServerCore.js';

const httpServer = createServer();
const io = new Server(httpServer, {
    pingTimeout: 60000
});

const server = new ServerCore();


io.on('connection', function(socket){
    let connection = server.connect(socket);

    connection.createEvents();
});

httpServer.listen(52300, ()=>{
    console.log('Start server to port: 52300');
});