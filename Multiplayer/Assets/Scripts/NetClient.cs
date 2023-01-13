using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Security.Cryptography;

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

    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Blocking = false;

        serverEndPoint = new IPEndPoint(IPAddress.Parse(serverAddress), serverPort);

        packet = new byte[512];
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
                    socket.SendTo(hashedChallenge, serverEndPoint);
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
    }

}