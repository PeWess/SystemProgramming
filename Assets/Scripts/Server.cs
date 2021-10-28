using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int _maxConnection = 10;
    private int _port = 5805;

    private int _hostID;
    private int _reliableChannel;

    private bool _isStarted = false;
    private byte _error;

    private List<int> _connectionIDs = new List<int>();
    private Dictionary<int, string> _nickNames = new Dictionary<int, string>();
    private string _lastNickName;

    public void StartServer()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        _reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(cc, _maxConnection);
        _hostID = NetworkTransport.AddHost(topology, _port);

        _isStarted = true;
    }

    public void Update()
    {
        if (!_isStarted) return;

        int recHostID;
        int connectionID;
        int channelID;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        NetworkEventType recData = NetworkTransport.Receive(out recHostID, out connectionID,
            out channelID, recBuffer, bufferSize, out dataSize, out _error);

        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                
                case NetworkEventType.ConnectEvent:
                    _connectionIDs.Add(connectionID);
                    if (!_nickNames.ContainsKey(connectionID))
                    {
                        _nickNames.Add(connectionID, _lastNickName);
                    }
                    SendMessageToAll($"Player {_nickNames[connectionID]} has connected");
                    Debug.Log($"Player {_nickNames[connectionID]} has connected");
                    break;
                
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    SendMessageToAll($"Player {_nickNames[connectionID]}:{message}");
                    Debug.Log($"Player {_nickNames[connectionID]}:{message}");
                    break;
                
                case NetworkEventType.DisconnectEvent:
                    _connectionIDs.Remove(connectionID);
                    SendMessageToAll($"Player {_nickNames[connectionID]} has disconnected");
                    Debug.Log($"Player {_nickNames[connectionID]} has disconnected");
                    break;
                
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            
            recData = NetworkTransport.Receive(out recHostID, out connectionID,
                out channelID, recBuffer, bufferSize, out dataSize, out _error);
        }
    }

    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(_hostID, connectionID, _reliableChannel, buffer, 
            message.Length * sizeof(char), out _error);
        if((NetworkError)_error != NetworkError.Ok)
            Debug.Log((NetworkError)_error);
    }

    public void SendMessageToAll(string message)
    {
        for (int i = 0; i < _connectionIDs.Count; i++)
        {
            SendMessage(message, _connectionIDs[i]);
        }
    }

    public bool SetLastNickname(string nickName)
    {
        if (nickName == "" || _nickNames.ContainsValue(nickName))
        {
            Debug.Log("Nickname was already taken or nickname field is empty");
            return false;
        }
        _lastNickName = nickName;
        return true;
    }

    public void ShutDownServer()
    {
        if (!_isStarted) return;

        NetworkTransport.RemoveHost(_hostID);
        NetworkTransport.Shutdown();
        _isStarted = false;
    }
}
