import SocketSession from "./SocketSession";

interface SessionDictionary{
    [key:number] : SocketSession
}

export default class SessionManager
{
    static Instance : SessionManager;
    sessionMap:SessionDictionary;

    constructor()
    {
        this.sessionMap = {};
    }

    addSession(session:SocketSession, id:number): void
    {
        this.sessionMap[id] = session; //맵에다가 넣는다.
    }
    removeSession(id:number):void
    {
        delete this.sessionMap[id]; //이렇게 하면 키가 삭제된다.
    }
    //브로드캐스트, 유니캐스트, 멀티캐스트
}