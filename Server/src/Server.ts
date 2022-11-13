import Express, { Application } from 'express';
import {IncomingMessage} from 'http'
import WS, { RawData } from 'ws';
import MapManager, { MapCategory } from './MapManager';
import { tankio } from './packet/packet';
import Path from 'path';
import PacketManager from './packetManager';
import SocketSession from './SocketSession';
import SessionManager from './SessionManager';

const App: Application = Express();


const httpServer = App.listen(50000, ()=>{
    console.log("Server is running on 50000 port");
});

const socketServer : WS.Server = new WS.Server({
    server:httpServer,
    //port:9090
});

//데이터 이니셜랄이저
console.log("Server is running on 50000 port");
PacketManager.Instance = new PacketManager();
MapManager.Instance = new MapManager(Path.join(__dirname, "Tilemap.txt"));
SessionManager.Instance = new SessionManager();

let playerID:number = 1;

socketServer.on("connection", (soc:WS, req:IncomingMessage) => {

    const id = playerID;
    const ip:string = req.connection.remoteAddress as string;

    let session:SocketSession = new SocketSession(soc,ip,id, ()=>{
        SessionManager.Instance.removeSession(id);
    });
    SessionManager.Instance.addSession(session, id); //세션매니저에 추가
    console.log(`${ip}에서 ${id} 플레이어 접속`);

    let spawnPos:tankio.Position = MapManager.Instance.getRandomSafePosition();
    let welcomeMsg = new tankio.S_init({playerId:id, spawnPosition:spawnPos});
    session.sendData(welcomeMsg.serialize(), tankio.MSGID.S_INIT);

    playerID++;

    soc.on("message", (data: RawData, isBinary: boolean)=>{
        if(isBinary)session.receiveMsg(data);
    })
    /*
        종료 코드   
        1000 - 기본 normal closure
        1001 - 연결주체 중에 한 쪽이 말없이 떠남. 유니티 끄거나, 서버 셧다운
        1009 - 메시지가 너무 커서 프로세싱이 안된다.
        1011 - 서버 쪽 에러
    */
});