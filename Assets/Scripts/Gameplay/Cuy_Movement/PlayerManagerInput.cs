using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
public class PlayerManagerInput : MonoBehaviour
{
    public PlayerInputManager inputManager;
    public GameObject[] playerPrefabs;

    private int playerIndex = 0;

    private void OnEnable()
    {
        inputManager.onPlayerJoined += OnPlayerJoined;
    }

    private void OnDisable()
    {
        inputManager.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Jugador unido: " + playerInput.playerIndex);

        if (playerIndex < playerPrefabs.Length)
        {
            inputManager.playerPrefab = playerPrefabs[playerIndex];
            playerIndex++;
        }
    }
}
