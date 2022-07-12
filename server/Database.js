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
}