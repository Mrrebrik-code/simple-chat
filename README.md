# simple-chat
- simple chat to unity and node.js server


Connecting to server (C#):
```c#

  private SocketManager _socketManager = null;
  [SerializeField] private string _address = "http://localhost:52300";

  public void ConnectedToServer()
  {
      SocketOptions options = new SocketOptions()
      {
          Reconnection = _isReconection
      };
      _socketManager = new SocketManager(new System.Uri(_address), options);
  }
```
Connecting to server in start.js (Node.js):
```js
  io.on('connection', function(socket){
    let connection = server.connect(socket);

    connection.createEvents();
    
    connection.socket.emit("connection-server", connection.user.id);
  });
  
  
  }
```
Create events connection to current socket (Node.js):
```js
  createEvents(){
        let connection = this;
        let socket = connection.socket;
        let server = connection.server;
        let user = connection.user;

        socket.on('disconnect', () => {
            console.log("User disconnect server!");
        });
  }
```
