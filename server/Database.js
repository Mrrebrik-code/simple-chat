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
        .from('chat-users')
        .select('userName, userId').eq('name', name);

        if(chat.data.length == 0){
            return null;
        }else{
            return chat.data;
        }
    }

    async addCurrentUserToChat(nameChat, user){
        let supabase = this.supabase;

        let userData = await supabase
        .from('chat-users')
        .insert(
        [ 
            { 
                name: nameChat, 
                userName: user.nickname,
                userId: user.id
            }
        ]);

        //Todo to remove successful/fail
        return true;
    }

    async removeCurrentUserToChat(user){
        let supabase = this.supabase;

        const chat = await supabase
         .from('chat-users')
         .delete()
         .eq("userId", user.id);

        //Todo to remove successful/fail
        return true;
    }

    async addUsersToChat(name, usersData){
        let supabase = this.supabase;

        //let json = JSON.stringify(usersData);

        let chat = await supabase
        .from('chats')
        .update({ users: usersData })
        .eq('name', name);

        return Boolean(chat.data.length);
    }
}