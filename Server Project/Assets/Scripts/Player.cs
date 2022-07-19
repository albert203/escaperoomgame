using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Need a way to access players by their ID so we are going to create a static disctionary
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();
    public ushort ID { get; private set; }
    public string Username { get; private set; }

    private void OnDestroy()
    {
        // When a player is destroyed, we are going to remove them from the list.
        list.Remove(ID);
    }

    public static void Spawn(ushort id, string username)
    {
        // Instatiating a Player object.
        Player player = Instantiate(GameLogic.Singleton.PlayerPrefab, new Vector3(0f, 1f, 0f), Quaternion.identity).GetComponent<Player>();
        // Doing a check to see if player clicks connect without inputting a username. 
        // condition is set before the ?, if it is true player.name = "Guest", if it is false the the player.name = username.
        // if the string is null in the connect screen then the player.name = "Guest", if it is not null then the player.name = username.
        player.name = $"Player {id} ({(string.IsNullOrEmpty(username) ? "Guest" : username)}";
        // Assigning the ID and Username properties to the player.
        player.ID = id;
        player.Username = string.IsNullOrEmpty(username) ? $"Guest {id}" : username;

        list.Add(id, player);

        // this is the same as writing an if else statement like this:
        // if (string.IsNullOrEmpty(username))
        // {
        //     player.name = $"Player {id} Guest";
        // }
        // else
        // {
        //     player.name = $"Player {id} {username}";
        // }
    }

    // We are going to need a message with a MessageHadler attribute
    // This attribute lets riptide know that messages with the given 
    // should be handled with the following method
    [MessageHandler((ushort)EscapeRoomClientToServerID.name)]
    // MessageHandler methods must be static and can be private
    private static void Name(ushort fromClientId, Message message)
    {
        Spawn(fromClientId, message.GetString());
    }

}
