using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Attach the NetworkManager to a gameobject 
    // and access that specific instance anywhere in our code 
    // This will also ensure that there will ever be one instance of
    // the NetworkManager in the scene.
    private static UIManager _singleton;
    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private InputField usernameField;

    private void Awake()
    {
        Singleton = this;
    }

    public void ConnectClicked()
    {
        // We want to disable interaction with the username field
        usernameField.interactable = false;

        // Connect the UI and call the NetworkManager.Connect method.
        connectUI.SetActive(false);

        NetworkManager.Singleton.Connect();
    }

    // Make a method to reset our UI, If we get disconnected.
    public void BackToMain()
    {
        usernameField.interactable = true;
        connectUI.SetActive(true);
    }

    // Can be reliable or unreliable, both have advantages and disadvantages.
    // Reliable is more efficient, but less efficient if the connection is lost.
    // Unreliable is more efficient if the connection is lost, but less efficient. 
    // if unreliable player movement lost messages will be ignored, but this is not 
    // so important as a new player position is likely to be sent stright after
    // a unsername is more important and should require reliable messages.
    public void SendName()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.name);
        message.AddString(usernameField.text);
        NetworkManager.Singleton.Client.Send(message);
    }
}
