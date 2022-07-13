![Alt-текст](https://github.com/Mrrebrik-code/simple-chat/blob/main/client/Assets/Sprites/Templates/ui_screenshot.png?raw=true "client-ui-template")

# Server (Node.js)

| ID | Type | Evenet name | Description | Data|
|:----------------:|:---------:|:---------:|:----------------:|:----------------:|
| 1 | On | connection | Connected new socket | null |
| 2 | On | disconnect | Disconnected socket | null |
| 3 | On | register-account | Create new account to current socket | user object |
| 4 | On | login-account | Logined account to current socket | user object |

| ID | Type | Evenet name | Description | Data|
|:----------------:|:---------:|:---------:|:----------------:|:----------------:|
| 1 | Emit | connection-server | Send to current socket client status successful connected | string |
| 2 | Emit | register-user | Register user succesful | user object |
| 3 | Emit | login-user | Logined user succesful | user object |


Connecting to server in start.js (Node.js):
```js
  io.on('connection', function(socket){
        let connection = server.connect(socket);

        connection.createEvents();
    
        //Send user id to client callback
        connection.socket.emit("connection-server", connection.user.id);
  });
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
```

Database to server realization current supabase service. Create table users: id, userId, nickname, password. 
Creating client supabase api databse realtime:
```js
  constructor(urlSupabase, anonPublicApiKey){
        //Connect to api supabase
        this.supabase = createClient(urlSupabase, anonPublicApiKey);

        console.log("init database!");
    }
```
Creating user to database async/await: 
```js
  async createUser(id, nickname, password){
        let supabase = this.supabase;

        let user = await supabase
        //Target table users
        .from('users')
        
        //adding data to table cells
        .insert(
        [ 
            { 
                userId: id, 
                nickname: nickname,
                password: password
            }
        ]);

        return Boolean(user.data.length);
    }
```
Logined user two step!
1. Checking nickname to database.
```js
  async tryUserNickname(nickname){
        let supabase = this.supabase;

        let user = await supabase
        .from('users')
        //Serrch to nickanme current and return data nickname
        .select('nickname').eq('nickname', nickname);

        return  Boolean(user.data.length);
    }
```
2. Checking current password to user.
```js
  async tryUserPassword(nickname, password){
        let supabase = this.supabase;

        let user = await supabase
        .from('users')
        //Serarch to nickname current and return data userId, password
        .select('userId, password').eq('nickname', nickname);
        
        //Checking password done!
        if(user.data[0].password == password){
        
            //get userId to database and sending to client
            let userData = {
                id: user.data[0].userId,
                nickname: nickname,
                password: password
            }

            return userData;
        }
        else{
            return null;
        }
    }
```

# Client (Node.js)
