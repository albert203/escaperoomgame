using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    // Attach the NetworkManager to a gameobject 
    // and access that specific instance anywhere in our code 
    // This will also ensure that there will ever be one instance of
    // the NetworkManager in the scene.
    private static GameLogic _singleton; 

    public static GameLogic Singleton{
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

    // A getter so we can access the prefab elsewhere in our code
    public GameObject PlayerPrefab => playerPrefab;

    // A prefab is a gameobject that we can instantiate. In this case our player object.
    // Hold the playeer prefab
    [Header("Prefabs")]
    [SerializeField] private GameObject playerPrefab;

    private void Awake()
    {
        Singleton = this;
    }
}
