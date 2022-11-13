import { PacketHandler } from "./packet/PacketHandler";
import {tankio} from "./packet/packet"
//import { CPosHandler } from "./packet/CPosHandler";

interface HandlerDictionary {
    [key:number] : PacketHandler
}

export default class PacketManager
{
    static Instance:PacketManager;
    handlerMap:HandlerDictionary;
    
    constructor ()
    {
        console.log("Package Manager initalize...");
        this.handlerMap = {};
        this.register();
    }
    
    register():void
    {
        //this.handlerMap[tankio.MSGID.C_POS] = new CPosHandler();
    }
}