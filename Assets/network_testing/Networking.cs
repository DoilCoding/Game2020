//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using UnityEngine;

//namespace Assets.network_testing
//{
//    public abstract class Networking
//    {
//        public bool IsClient => Application.isBatchMode;
//        public bool IsServer => !IsClient;

//        public static Dictionary<bool, Player> PlayerList { get; } = new Dictionary<bool, Player>();
//    }

//    [SuppressMessage("ReSharper", "InconsistentNaming")]
//    public class Player
//    {
//        public uint ID { get; set; }
//        public GameObject Object { get; set; }
//        public uint LastPacketID { get; set; }
//        public Dictionary<uint, Packet> Packets { get; } = new Dictionary<uint, Packet>();
//    }

//    [SuppressMessage("ReSharper", "InconsistentNaming")]
//    public struct Packet
//    {
//        public uint ID { get; set; }                        // 4
//        public uint Owner { get; set; } // necessary?       // 4
//        public uint ReceivedLastPacket { get; set; }        // 4
//        public byte ReceivedPackets { get; set; }           // 1
//        public byte DataType { get; set; }                  // 1
//        public byte BaseLinesReferences { get; set; }       // 1
//        public byte SkipValues { get; set; }                // 1
//                                                            // 16 for header
//        // position
//        public sbyte PositionX;                             // 1
//        public sbyte PositionY;                             // 1
//        public sbyte PositionZ;                             // 1

//        // rotation
//        public sbyte RotationX;                             // 1
//        public sbyte RotationY;                             // 1
//        public sbyte RotationZ;                             // 1
//        public sbyte RotationW; // necessary?               // 1

//        // raw position
//        public float RawPositionX;                          // 4
//        public float RawPositionY;                          // 4
//        public float RawPositionZ;                          // 4

//        public sbyte RawRotationX;                          // 1
//        public sbyte RawRotationY;                          // 1
//        public sbyte RawRotationZ;                          // 1
//        public sbyte RawRotationW; // necessary?            // 1
//    }                                                       // 23 for body

//                                                            // total: 39 bytes

//    // packet will go through an encoder which will write it all into one byte array
//    // if a value is 0 it will skip that value and add it to the SkipValues argument
//    // this will ensure the byte array will be smaller
//    public static class EncodePacket
//    {


//        // good job figuring this out but we are doing this wrong, we shouldn't just write a function, we need to attempt to implement it right away.
//        // and code what we need.
//        // TODO: write console connect ip:port,
//        // write editor to launch the server

//        public static byte[] Encode(this Packet packet)
//        {
//            var data = new List<byte>();
//            data.AddRange(BitConverter.GetBytes(packet.ID));
//            data.AddRange(BitConverter.GetBytes(packet.Owner));
//            data.AddRange(BitConverter.GetBytes(packet.ReceivedLastPacket));
//            data.Add(packet.DataType);

//            if (packet.DataType == 0)
//            {
//                var (deltaPositionList, skipValues) = ReadPositions(packet.PositionX, packet.PositionY, packet.PositionZ,
//                    packet.RotationX, packet.RotationY, packet.RotationZ, packet.RotationW);

//                data.Add(skipValues);
//                data.AddRange(deltaPositionList);
//            }
//            else
//            {
//                var (rawPositionList, skipValues) = ReadRawPositions(packet.RawPositionX, packet.RawPositionY, packet.RawPositionZ,
//                    packet.RawRotationX, packet.RawRotationY, packet.RawRotationZ, packet.RawRotationW);
//                data.Add(skipValues);
//                data.AddRange(rawPositionList);
//            }

//            data.Add(packet.ReceivedPackets);
//            data.Add(packet.DataType);
//            data.Add(packet.BaseLinesReferences);
//            return data.ToArray();
//        }

//        private static (byte[], byte) ReadPositions(params sbyte[] input)
//        {
//            byte aByte = 0x0;
//            var data = new List<byte>();
//            for (var i = 0; i < 8; i++)
//            {
//                if (i >= input.Length || input[i] == 0)
//                {
//                    Set(ref aByte, i, true);
//                    continue;
//                }

//                data.Add((byte)input[i]);
//            }
//            return (data.ToArray(), aByte);
//        }

//        private static (byte[], byte) ReadRawPositions(params float[] input)
//        {
//            byte aByte = 0x0;
//            var data = new List<byte>();
//            for (var i = 0; i < 8; i++)
//            {
//                if (i >= input.Length || Math.Abs(input[i]) < 0.00001f)
//                {
//                    Set(ref aByte, i, true);
//                    continue;
//                }

//                data.Add((byte)input[i]);
//            }
//            return (data.ToArray(), aByte);
//        }

//        public static void Set(ref byte aByte, int pos, bool value)
//        {
//            if (value)
//            {
//                //left-shift 1, then bitwise OR
//                aByte = (byte)(aByte | (1 << pos));
//            }
//            else
//            {
//                //left-shift 1, then take complement, then bitwise AND
//                aByte = (byte)(aByte & ~(1 << pos));
//            }
//        }

//        public static bool Get(byte aByte, int pos)
//        {
//            //left-shift 1, then bitwise AND, then check for non-zero
//            return ((aByte & (1 << pos)) != 0);
//        }
//    }
//}