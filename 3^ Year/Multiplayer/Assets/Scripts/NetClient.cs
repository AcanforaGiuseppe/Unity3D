using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

public class NetClient : MonoBehaviour
{
    [SerializeField]
    string serverAddress;

    [SerializeField]
    int serverPort;

    Socket socket;
    IPEndPoint serverEndPoint;

    byte[] packet;

    int state = 0;

    int randomValue;

    [SerializeField]
    GameObject playerPrefab;

    Dictionary<uint, GameObject> playersInGame;

    BinaryWriter packetWriter;

    [SerializeField]
    string playerName;

    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Blocking = false;

        serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);

        packet = new byte[512];

        playersInGame = new Dictionary<uint, GameObject>();

        packetWriter = new BinaryWriter(new MemoryStream(packet));
    }

    void Update()
    {
        if (state == 0) // Sending the first "Handshake" to the Server
        {
            randomValue = UnityEngine.Random.Range(0, 100000);
            BitConverter.GetBytes(randomValue).CopyTo(packet, 0);

            socket.SendTo(packet, 4, SocketFlags.None, serverEndPoint);

            state = 1;
        }
        else if (state == 1)
        {
            EndPoint endPoint = serverEndPoint as EndPoint;
            try
            {
                int rlen = socket.ReceiveFrom(packet, ref endPoint);
                if (rlen == 4)
                {
                    // Giving a response to the Server, after his challenge
                    int serverValue = BitConverter.ToInt32(packet, 0);
                    int totalChallenge = randomValue + serverValue;
                    byte[] challengeBytes = BitConverter.GetBytes(totalChallenge);
                    SHA256 hash = SHA256.Create();
                    byte[] hashedChallenge = hash.ComputeHash(challengeBytes);

                    byte[] playerNameOriginalBytes = Encoding.ASCII.GetBytes(playerName);

                    byte[] playerNameBytes = new byte[8];

                    playerNameOriginalBytes.CopyTo(playerNameBytes, 0);

                    byte[] challengeAndName = new byte[hashedChallenge.Length + playerNameBytes.Length];
                    hashedChallenge.CopyTo(challengeAndName, 0);
                    playerNameBytes.CopyTo(challengeAndName, hashedChallenge.Length);

                    socket.SendTo(challengeAndName, serverEndPoint);
                    state = 2;

                    // Sending a message after the challenge is completed
                    byte[] message = System.Text.Encoding.ASCII.GetBytes("Hello World");
                    socket.SendTo(message, serverEndPoint);
                }
            }
            catch (SocketException)
            {
                return;
            }
        }
        else if (state == 2)
        {
            // Periodically sending transform updates
            packetWriter.BaseStream.Seek(0, SeekOrigin.Begin);
            packetWriter.Write((uint)0);
            packetWriter.Write((uint)0);
            packetWriter.Write(transform.position.x);
            packetWriter.Write(transform.position.z);
            packetWriter.Write(transform.rotation.eulerAngles.y);

            socket.SendTo(packet, (int)packetWriter.BaseStream.Position, SocketFlags.None, serverEndPoint);

            EndPoint endPoint = serverEndPoint as EndPoint;
            // Check if there is something in the socket and eventually manage the packet
            try
            {
                int rlen = socket.ReceiveFrom(packet, ref endPoint);

                if (rlen >= 4)
                    ManagePacket(rlen);
            }
            catch (SocketException)
            {
                return;
            }
        }
    }

    void ManagePacket(int rlen)
    {
        int command = BitConverter.ToInt32(packet, 0);

        if (command == 1)
        {
            if (rlen != 28)
            {
                // TODO - Better to disconnect
                Debug.Log("Unexpected packet size for command 1");
                return;
            }

            uint playerId = BitConverter.ToUInt32(packet, 4);
            float playerX = BitConverter.ToSingle(packet, 8);
            float playerZ = BitConverter.ToSingle(packet, 12);
            float playerYaw = BitConverter.ToSingle(packet, 16);

            GameObject newPlayer = GameObject.Instantiate(playerPrefab);
            newPlayer.transform.position = new Vector3(playerX, 1, playerZ);
            newPlayer.transform.rotation = Quaternion.Euler(0, playerYaw, 0);

            string newPlayerName = Encoding.ASCII.GetString(packet, 20, 8);
            newPlayer.name = newPlayerName;

            newPlayer.GetComponentInChildren<TextMesh>().text = newPlayerName;

            playersInGame.Add(playerId, newPlayer);
        }
        else if (command == 0)
        {
            if (rlen != 20)
            {
                // TODO - Better to disconnect
                Debug.Log("Unexpected packet size for command 0");
                return;
            }

            uint playerId = BitConverter.ToUInt32(packet, 4);
            float playerX = BitConverter.ToSingle(packet, 8);
            float playerZ = BitConverter.ToSingle(packet, 12);
            float playerYaw = BitConverter.ToSingle(packet, 16);

            if (!playersInGame.ContainsKey(playerId))
            {
                Debug.Log("Unknown player");
                return;
            }

            playersInGame[playerId].transform.position = new Vector3(playerX, 1, playerZ);
            playersInGame[playerId].transform.rotation = Quaternion.Euler(0, playerYaw, 0);
        }
        else if (command == 2)
        {
            if (rlen != 8)
            {
                // TODO - Better to disconnect
                Debug.Log("Unexpected packet size for command 2");
                return;
            }

            uint playerId = BitConverter.ToUInt32(packet, 4);


            if (!playersInGame.ContainsKey(playerId))
            {
                Debug.Log("Unknown player");
                return;
            }

            Destroy(playersInGame[playerId]);
            playersInGame.Remove(playerId);
        }
    }

}