using UnityEngine;
using TMPro;

public class StatBoardUI : MonoBehaviour
{
    //health
    [Header("Health")]
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI currentHealthText;
    // stats
    [Header("Stats")]
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI strengthText;
    // actions
    [Header("Actions")]
    public TextMeshProUGUI actionsText;
    // updates
    public void UpdateBoard(
        int maxHealth,
        int currentHealth,
        int speed,
        float strength,
        int actions)
    { // null exceptions logs
        if (maxHealthText != null)
            maxHealthText.text = "Max HP: " + maxHealth;

        if (currentHealthText != null)
            currentHealthText.text = "HP: " + currentHealth;

        if (speedText != null)
            speedText.text = "Speed: " + speed;

        if (strengthText != null)
            strengthText.text = "Strength: " + strength;

        if (actionsText != null)
            actionsText.text = "Turns Left: " + actions;
    }
}