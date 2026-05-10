using UnityEngine;
// spawns instance of a player
public class PlayerSpawner : MonoBehaviour
{
    public Player playerPrefab;
    // awake
    void Awake()
    {
        if (Player.Instance == null)
        {
            Instantiate(playerPrefab);
        }
    }
}