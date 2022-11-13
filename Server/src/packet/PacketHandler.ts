import SocketSession from "../SocketSession";

export interface PacketHandler
{
    handleMsg(session:SocketSession, buffer:Buffer): void;
}