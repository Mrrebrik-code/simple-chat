![Alt-текст](https://github.com/Mrrebrik-code/simple-chat/blob/main/client/Assets/Sprites/Templates/ui_screenshot.png?raw=true "client-ui-template")

# Server (Node.js)

Connecting to server in start.js (Node.js):
```js
  io.on('connection', function(socket){
    let connection = server.connect(socket);

    connection.createEvents();
    
    //Send user id to client callback
    connection.socket.emit("connection-server", connection.user.id);
  });
  
  
  }
```

Create connections to server.js (Node.js):
```js
 connect(socket){
        let server = this;
        let connection = new Connection();
        
        //Create new object user
        let user = new User();
        
        //Initialization connection to current socket
        connection.init(socket, user, server);

        let userId = user.id;
        
        //Add array connections server list
        server.connections[userId] = connection;

        console.log(`User [${userId}] connected to server!`);
        return connection;
    }
  }
```

Create events connection to current socket (Node.js):
```js
  createEvents(){
        let connection = this;
        let socket = connection.socket;
        let server = connection.server;
        let user = connection.user;
        
        //socket events on to current connected server socket
        socket.on('disconnect', () => {
            console.log("User disconnect server!");
        });
  }
```

# Client (Node.js)
