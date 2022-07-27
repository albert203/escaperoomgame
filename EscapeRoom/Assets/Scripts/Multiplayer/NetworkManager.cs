using System;
using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

// This enum will contain all the ids for messages 
// we send from the client to the server
public enum ServerToEscapeRoomClientId : ushort
{
    playerSpawned = 1,
    playerMovement,
}

public enum EscapeRoomClientToServerId : ushort
{
    name = 1,
    input,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;
    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client EscaperoomClient { get; private set; }

    // Next we need two fields for the ip and port to connect to
    // SerializeField is a Unity attribute that allows us to 
    // inspect the field in the inspector even when var types are private.
    // when a var type is private, it is not visible in the inspector &
    // cannot be accessed by other scripts.

    // Serialization is the process of taking an object in ram (classes, fields, etc...) 
    // and making a disk representation of it which can be recreated 
    // at any point in the future. When you apply the SerializeField attribute 
    // to a field, it tells the unity engine to save/restore it's state to/from disk. 
    // You mostly use serialization for the editor, and especially when building your 
    // own editor windows and inspectors.
    [SerializeField] private string ip;
    [SerializeField] private ushort port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {   
        // Initialise the Riptide logger class.
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        // Starting the server. With our port and max client count parameters & including
        // The methods we made to handle errors and feedback
        EscaperoomClient = new Client();
        EscaperoomClient.Connected += DidConnect;
        EscaperoomClient.ConnectionFailed += FailedToConnect;
        EscaperoomClient.ClientDisconnected += PlayerLeft;
        EscaperoomClient.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        // Ensures messages recieved from the server are processed.
        EscaperoomClient.Tick();
    }

    private void OnApplicationQuit()
    {
        // .Disconnect() is a method that closes the connection to the server.
        EscaperoomClient.Disconnect();
    }

    public void Connect()
    {
        EscaperoomClient.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        // Calling the UImanager SendName method
        UIManager.Singleton.SendName();
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }

      private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        Destroy(Player.list[e.Id].gameObject);
    }

    // The reason we have two of these methods is because we are going 
    // we are going to add them as suscribers of seperate client events
    // and be addingmore code that is not identical 
    
    // public void DidConnect(object sender, EventArgs e)
    // {
    //     // Calling the UImanager SendName method
    //     UIManager.Singleton.SendName();
    // }
   
    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.BackToMain();
    }
} 
    



