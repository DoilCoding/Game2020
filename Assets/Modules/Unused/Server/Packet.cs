using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Modules.Server
{
    public struct Packet
    {
        public Packet Decode()
        {
            return new Packet { };
        }


        public byte[] Encode(Packet data)
        {
            return new byte[0];
        }

        public ulong id { get; set; }       // 
        public Type type { get; set; }      // raw or deltas?
        public byte mask { get; set; }      // what values do we have
        public byte received { get; set; }  // what packets the server has seen 

        public enum Type : byte
        {
            Raw,
            Delta
        }
    }
}

/*
    CLIENT SEND 111
    ..
    CLIENT SEND 112
    SERVER SEES 112 and is missing 111 so it wil teleport the client to the position of 111 before simulating movement 112
    NOTE: teleporting can cause exploitations..

    planned movement is movement dir * speed
    for PACKET 111 we can movement dir * (speed*2?)
 

    server movement:
        regular: position + (use player direction input * speed)
        previous packet: position + (use player direction input * (speed * ( currentpacket.Id - this.packetid ) ))

    or combine previous packet and current packet into one?
*/
