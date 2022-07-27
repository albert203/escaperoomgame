using RiptideNetworking;
using System.Collections.Generic;
using UnityEngine;
public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> list = new Dictionary<ushort, Player>();

    // Added properties to store the player ids
    public ushort Id { get; private set; }

    // And also the if the player is local
    public bool IsLocal { get; private set; }

    // And also the name of the player
    // [SerializeField] private PlayerAnimationManager animationManager;
    [SerializeField] private Transform camTransform;

    private string username;

    private void OnDestroy()
    {
        list.Remove(Id);
    }

    private void Move(Vector3 newPosition, Vector3 forward)
    {
        transform.position = newPosition;

        if (!IsLocal)
        {
            camTransform.forward = forward;
            // animationManager.AnimateBasedOnSpeed();
        }
    }

    public static void Spawn(ushort id, string username, Vector3 position)
    {
        Player player;
        if (id == NetworkManager.Singleton.EscaperoomClient.Id)
        {
            player = Instantiate(GameLogic.Singleton.LocalPlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = true;
        }
        else
        {
            player = Instantiate(GameLogic.Singleton.PlayerPrefab, position, Quaternion.identity).GetComponent<Player>();
            player.IsLocal = false;
        }
        player.name = $"Player {id} (username)";
        player.Id = id;
        player.username = username;
        list.Add(id, player);
    }

    #region Messages
    [MessageHandler((ushort)ServerToEscapeRoomClientId.playerSpawned)]
    private static void SpawnPlayer(Message message)
    {
        Spawn(message.GetUShort(), message.GetString(), message.GetVector3());
    }

    [MessageHandler((ushort)ServerToEscapeRoomClientId.playerMovement)]
    private static void PlayerMovement(Message message)
    {
        if (list.TryGetValue(message.GetUShort(), out Player player))
            player.Move(message.GetVector3(), message.GetVector3());
    }
    #endregion
}