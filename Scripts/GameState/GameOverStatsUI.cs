using TMPro;
using UnityEngine;

public class GameOverStatsUI : MonoBehaviour
{
    public TextMeshProUGUI statsText;

    void Start()
    {
        // player instance
        Player p = Player.Instance;
        // stats
        statsText.text =
            "FINAL RUN STATS\n\n" +
            "Wave Reached: " + p.currentWave+1 + "\n" +
            "Enemies Killed: " + p.enemiesKilled + "\n" +
            "Bosses Killed: " + p.bossesKilled + "\n\n" +

            "Damage Dealt: " + p.totalDamageDealt + "\n" +
            "Damage Received: " + p.totalDamageReceived + "\n" +
            "Health Regenerated: " + p.totalHealthRegenerated + "\n" +
            "Potions Used: " + p.potionsUsed + "\n\n" +

            "Final Strength: " + p.strength.ToString("F1") + "\n" +
            "Final Max Health: " + p.maxHealth + "\n" +
            "Final Speed: " + p.speed;
    }
}