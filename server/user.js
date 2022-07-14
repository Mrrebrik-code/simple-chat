import shortId from "shortid";

export class User{
    constructor(){
        this.id = shortId.generate();
        this.nickname = "nickname";

        this.currentChat = null;
    }
}