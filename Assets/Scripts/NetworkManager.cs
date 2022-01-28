using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System;
using TMPro;
using Newtonsoft.Json.Linq;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    [Header("Server Setup")]
    [SerializeField]
    private string host = "ggj.jacobcheatley.com";
    [SerializeField]
    private int port = 9001;

    private static string gameVersion = "ALPHA";  // TODO: Automatically produce?

    private byte[] receiveBuffer = new byte[1024];
    private Socket socket;
    private string messageData;

    public static NetworkManager instance;

    public SynchronizationContext mainThreadContext;

    private void Start()
    {
        instance = this;
        mainThreadContext = SynchronizationContext.Current;
    }

    public void Create(string playerName)
    {
        Identify(playerName);
        SendJson(new Dictionary<string, object> { { "action", "create" } });
    }

    public void Join(string playerName, string code)
    {
        Identify(playerName);
        SendJson(new Dictionary<string, object> { { "action", "join" }, { "code", code } });
    }

    private void Identify(string playerName)
    {
        ConnectSocket(host, port);
        if (socket != null)
        {
            SendJson(new Dictionary<string, object> { { "action", "identify" }, { "name", playerName }, { "game_version", gameVersion } });
            socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new System.AsyncCallback(ReceiveCallback), null);
        }
    }

    private void ConnectSocket(string host, int port)
    {
        if (socket != null)
        {
            socket.Close();
            Debug.Log("We reconnected... HMM");
        }

        IPHostEntry hostEntry = Dns.GetHostEntry(host);

        foreach (IPAddress address in hostEntry.AddressList)
        {
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
            {
                socket = tempSocket;
                break;
            }
        }
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        int bytesReceived = socket.EndReceive(ar);

        if (bytesReceived <= 0)
            return; // TODO: Out of messages, closed, whatever

        byte[] receivedData = new byte[bytesReceived];
        Buffer.BlockCopy(receiveBuffer, 0, receivedData, 0, bytesReceived);

        messageData += Encoding.UTF8.GetString(receivedData);
        int endOfMessageIndex;
        while ((endOfMessageIndex = messageData.IndexOf("\n")) > -1)
        {
            ParseMessage(messageData.Substring(0, endOfMessageIndex));
            messageData = messageData.Substring(endOfMessageIndex + 1);
        }

        socket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, new System.AsyncCallback(ReceiveCallback), null);
    }

    private void SendJson(Dictionary<string, object> json)
    {
        socket.Send(Encoding.UTF8.GetBytes($"{JsonConvert.SerializeObject(json)}\n"));
    }


    private void ParseMessage(string messageString)
    {
        Debug.Log($"Received {messageString}");
        var messageObject = JsonConvert.DeserializeObject<Message>(messageString);
        Type properType = messageObject.AppropriateType();
        dynamic t = JsonConvert.DeserializeObject(messageString, properType);
        mainThreadContext.Post(_ =>
        {
            switch (t)
            {
                case CodeMessage message:
                    instance.OnCodeMessage?.Invoke(message);
                    break;
                case MessageMessage message:
                    instance.OnMessageMessage?.Invoke(message);
                    break;
                case ConnectMessage message:
                    instance.OnConnectMessage?.Invoke(message);
                    break;
                case DisconnectMessage message:
                    instance.OnDisconnectMessage?.Invoke(message);
                    break;
                case TurnMessage message:
                    instance.OnTurnMessage?.Invoke(message);
                    break;
                case ErrorMessage message:
                    instance.OnErrorMessage?.Invoke(message);
                    break;
                default:
                    break;
            };
        }, null);
    }

    public delegate void OnCodeMessageDelegate(CodeMessage message);
    public event OnCodeMessageDelegate OnCodeMessage;

    public delegate void OnMessageMessageDelegate(MessageMessage message);
    public event OnMessageMessageDelegate OnMessageMessage;

    public delegate void OnConnectMessageDelegate(ConnectMessage message);
    public event OnConnectMessageDelegate OnConnectMessage;

    public delegate void OnDisconnectMessageDelegate(DisconnectMessage message);
    public event OnDisconnectMessageDelegate OnDisconnectMessage;

    public delegate void OnTurnMessageDelegate(TurnMessage message);
    public event OnTurnMessageDelegate OnTurnMessage;

    public delegate void OnErrorMessageDelegate(ErrorMessage message);
    public event OnErrorMessageDelegate OnErrorMessage;
}
