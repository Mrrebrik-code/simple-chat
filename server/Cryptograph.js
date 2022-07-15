import bcrypt from 'bcrypt';

export class Cryptograph{
    constructor(){
        this.saltRounds = 10;
    }

    async createHashPassword(password){
        let saltRound = this.saltRounds;

        const salt = bcrypt.genSaltSync(saltRound);
        const hash = bcrypt.hashSync(password, salt);

        return hash;
    }

    checkPassword(password, hash){
        return bcrypt.compareSync(password, hash);
    }
} 