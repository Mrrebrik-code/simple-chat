import { createServer } from 'http';
import { Server } from 'socket.io';
import { ServerCore } from './ServerCore.js';
import { Database } from './Database.js';

const urlSupabase = "https://rxmtgtrpftxiysckdhfs.supabase.co";
const pulicApiKeySupabse = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InJ4bXRndHJwZnR4aXlzY2tkaGZzIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NTYwNTE2ODUsImV4cCI6MTk3MTYyNzY4NX0.NInNqTQ5XskqMu_vT8J3orDBU4k-s5EZj77rE4d4t-I";

const httpServer = createServer();
const io = new Server(httpServer, {
    pingTimeout: 60000
});

const database = new Database(urlSupabase, pulicApiKeySupabse);
const serverCore = new ServerCore(database);


io.on('connection', function(socket){
    let connection = serverCore.connect(socket);

    connection.createEvents();
});

httpServer.listen(80, ()=>{
    console.log('Start server to port: 80');
});