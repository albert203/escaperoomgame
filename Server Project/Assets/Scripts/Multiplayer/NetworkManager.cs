using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public enum EscapeRoomClientToServerID : ushort
{
    name = 1,
}

public class NetworkManager : MonoBehaviour
{
    // Attach the NetworkManager to a gameobject 
    // and access that specific instance anywhere in our code 
    // This will also ensure that there will ever be one instance of
    // the NetworkManager in the scene.
    private static NetworkManager _singleton; 

    public static NetworkManager Singleton{
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            } 
            else if(_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicates");
                Destroy(value);
            }
        }
    }

    // We are going to need a server property 
    public Server Server { get; private set; }

    // We are going to need a field for the port and max client count
    // ushort can declare var storing a short integer. between 0 and 65,535. 
    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    // In the start method, we are going to intialise the Riplogger class
    // This will allow us to see the Riptide log messages in the unity console.
    // Then we are going to assign a value to our server property, and call the 
    // Start method to start the server.
    private void Start()
    {   
        // Initialise the Riptide logger class.
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        // Starting the server. With our port and max client count parameters.
        Server = new Server();
        Server.Start(port, maxClientCount);
        Server.ClientDisconnected += PlayerLeft;
    }

    private void FixedUpdate()
    {
        // Ensures messages recieved from the server are processed.
        Server.Tick();
    }

    private void OnApplicationQuit()
    {
        // When the application quits, we are going to call the Stop method
        // on our server property.
        Server.Stop();
    }

    // Destroy a Player Object when the client discconects.
    private void PlayerLeft(object sender, ClientDisconnectedEventArgs e)
    {
        // When a player disconnects, we are going to remove them from the list.
        Destroy(Player.list[e.Id].gameObject);
    }
}
