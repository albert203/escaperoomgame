using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
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
    [SerializeField] private static ip;
    [SerializeField] private static port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {   
        // Initialise the Riptide logger class.
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        // Starting the server. With our port and max client count parameters.
        EscaperoomClient = new Client();
        EscaperoomClient.
    }

        private void FixedUpdate()
    {
        // Ensures messages recieved from the server are processed.
        EscaperoomClient.Tick();
    }

    private void OnApplicationQuit()
    {
        // When the application quits, we are going to call the Stop method
        // on our server property.
        EscaperoomClient.Stop();
    }
}
