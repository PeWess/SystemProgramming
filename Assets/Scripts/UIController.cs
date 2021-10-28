using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [SerializeField] private Button _buttonStartServer;
    [SerializeField] private Button _buttonShutDownServer;
    [SerializeField] private Button _buttonConnectClient;
    [SerializeField] private Button _buttonDisconnectClient;
    [SerializeField] private Button _buttonSendMessage;
    [SerializeField] private TMP_InputField _nickName;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextField _textField;
    [SerializeField] private Server _server;
    [SerializeField] private Client _client;

    private void Start()
    {
        _buttonStartServer.onClick.AddListener(() => StartServer());
        _buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
        _buttonConnectClient.onClick.AddListener(() => Connect());
        _buttonDisconnectClient.onClick.AddListener(() => Disconnect());
        _buttonSendMessage.onClick.AddListener(() => SendMessage());
        _client._onMessageReceive += ReceiveMessage;
    }

    private void StartServer()
    {
        _server.StartServer();
    }

    private void ShutDownServer()
    {
        _server.ShutDownServer();
    }

    private void Connect()
    {
        bool nicknameCheck = _server.SetLastNickname(_nickName.text);
        if (nicknameCheck)
        {
            _client.Connect();
            _nickName.text = "";
        }
            
    }

    private void Disconnect()
    {
        _client.Disconnect();
    }

    private void SendMessage()
    {
        _client.SendMessage(_inputField.text);
        _inputField.text = "";
    }

    private void ReceiveMessage(object message)
    {
        _textField.ReceiveMessage(message);
    }
}