import { createClient } from '@supabase/supabase-js';

export class Database{
    constructor(urlSupabase, anonPublicApiKey){
        this.supabase = createClient(urlSupabase, anonPublicApiKey);

        console.log("init database!");
    }

    async createUser(id, nickname, password){
        let supabase = this.supabase;

        let user = await supabase
        .from('users')
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

    async tryUserNickname(nickname){
        let supabase = this.supabase;

        let user = await supabase
        .from('users')
        .select('nickname').eq('nickname', nickname);

        return  Boolean(user.data.length);
    }

    async tryUserPassword(nickname, password){
        let supabase = this.supabase;

        let user = await supabase
        .from('users')
        .select('userId, password').eq('nickname', nickname);
        
        if(user.data[0].password == password){

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

    async createChat(name, password){
        let supabase = this.supabase;

        let chat = await supabase
        .from('chats')
        .insert(
        [ 
            { 
                name: name, 
                password: password
            }
        ]);

        return Boolean(chat.data.length);
    }

    async tryChatName(name){
        let supabase = this.supabase;

        let user = await supabase
        .from('chats')
        .select('name').eq('name', name);

        return  Boolean(user.data.length);
    }

    async tryChatPassword(name, password){
        let supabase = this.supabase;

        let chat = await supabase
        .from('chats')
        .select('password').eq('name', name);
        
        if(chat.data[0].password == password){
            return true;
        }
        else{
            return false;
        }
    }

    async getUsersToChatName(name){
        let supabase = this.supabase;

        let chat = await supabase
        .from('chats')
        .select('users').eq('name', name);
        console.log(chat.data[0]);
        if(chat.data[0].users != null && chat.data[0].users.length != 0){
            let jsonUsers = JSON.parse(chat.data[0].users);

            if(jsonUsers == null){
                return null;
            }

            let chatData;
            console.log("TEST:");
            console.log(jsonUsers);

            if(chat.data[0].users != null){
                chatData = {
                    users: jsonUsers.users
                }
            }
            

            return chatData;
        } else{
            return null;
        }
    }

    async addCurrentUserToChat(nameChat, user){
        let usersData = await this.getUsersToChatName(nameChat);
        let userData = {
            name: user.nickname,
            id: user.id
        };

        if(usersData != null){
            usersData.users.push(userData);
        }
        else{
            usersData = {
                users: [userData]
            };
        }

        console.log(usersData);

        let isAddUsersToChat = await this.addUsersToChat(nameChat, usersData);

        return isAddUsersToChat;
        
    }

    async removeCurrentUserToChat(user){
        let usersData = await this.getUsersToChatName(user.currentChat);
        let usersTemp = [];
        let userData = {
            name: user.nickname,
            id: user.id
        };

        console.log(usersTemp.users);

        if(usersData != null){
            console.log(usersData.users);

            usersTemp = usersData.users.filter(x => {
                return x.id != userData.id;
              });
        }

        if(usersTemp.length === 0){
            usersTemp = null;
        }

        let isAddUsersToChat = await this.addUsersToChat(user.currentChat, usersTemp);

        return isAddUsersToChat;
    }

    async addUsersToChat(name, usersData){
        let supabase = this.supabase;

        let json = JSON.stringify(usersData);

        let chat = await supabase
        .from('chats')
        .update({ users: json })
        .eq('name', name);

        return Boolean(chat.data.length);
    }
}