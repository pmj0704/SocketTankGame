import WebSocket, {RawData} from "ws"
import PacketManager from "./packetManager";

export default class SocketSession
{
    socket:WebSocket;
    ipAddress:string;
    playerID:number;

    constructor(socket:WebSocket, ipAddress:string, playerID:number, CloseCallback:Function)
    {
        this.socket = socket;
        this.ipAddress = ipAddress;
        this.playerID = playerID;

        this.socket.on("close", (code:number, reason:Buffer)=>{
            console.log(`${this.playerID}님이 ${code}로 종효하였습니다.`);
            CloseCallback();
        });
    }

    getInt16LEFromBuffer(buffer:Buffer):number
    {
        return buffer.readInt16LE();
    }

    //데이터 받기
    receiveMsg(data:RawData):void
    {
        let code:number = this.getInt16LEFromBuffer(data.slice(2,4) as Buffer);
        PacketManager.Instance.handlerMap[code].handleMsg(this, data.slice(4) as Buffer);
    }

    //데이터 보내기
    
    sendData(payload: Uint8Array, msgCode:number):void
    {
        let len:number = payload.length +4; //최종길이 기록

        let lenBuffer:Uint8Array = new Uint8Array(2);
        new DataView(lenBuffer.buffer).setUint16(0, len, true); //리틀 엔디안으로 lenBuffer에 2바이트로 기록

        let msgCodeBuffer: Uint8Array = new Uint8Array(2);
        new DataView(msgCodeBuffer.buffer).setUint16(0, msgCode, true);

        let sendBuffer: Uint8Array = new Uint8Array(len); //길이와 메시지 코드 4바이트를 합친 최종 버퍼 제작
        sendBuffer.set(lenBuffer, 0); //2바이트 기록
        sendBuffer.set(msgCodeBuffer, 2); //코드 버퍼 2바이트 기록
        sendBuffer.set(payload, 4); //데이터 기록하고

        //각 소켓은 자신만의 버퍼가 있어서 send를 하면 sendBuffer 기록하고 끝냄
        this.socket.send(sendBuffer); //보내주면 됨
    }
}