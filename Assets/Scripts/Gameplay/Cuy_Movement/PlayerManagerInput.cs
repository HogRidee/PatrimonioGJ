using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
public class PlayerManagerInput : MonoBehaviour
{
    public GameObject player01Prefab;
    public GameObject player02Prefab;
    private int playerCount = 0;

    private void Awake()
    {
        var manager = GetComponent<PlayerInputManager>();
        manager.playerPrefab = player01Prefab;
        manager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        if (playerCount == 0)
        {
            playerInput.gameObject.name = "Player01";
            playerInput.SwitchCurrentActionMap("Player01");

            if (Gamepad.all.Count == 0)
                playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        }
        else if (playerCount == 1)
        {
            GetComponent<PlayerInputManager>().playerPrefab = player02Prefab;

            playerInput.gameObject.name = "Player02";
            playerInput.SwitchCurrentActionMap("Player02");

            if (Gamepad.all.Count >= 2)
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[1]);
            else if (Gamepad.all.Count == 1)
                playerInput.SwitchCurrentControlScheme("Gamepad", Gamepad.all[0]);
            else
                playerInput.SwitchCurrentControlScheme("Keyboard&Mouse", Keyboard.current, Mouse.current);
        }

        playerCount++;
    }
}
