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

        if(chat.data[0].users != null && chat.data[0].users.length != 0){
            let chatData = {
                users: chat.data[0].users
            }

            return chatData;
        } else{
            return null;
        }
    }
}