using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI hpText;
    // sets max health
    public void SetMaxHealth(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
        UpdateText(hp);
    }
    // sets hps to current value
    public void SetHealth(float hp)
    {
        slider.value = hp;
        UpdateText(hp);
    }
    // updates text
    void UpdateText(float hp)
    {
        if (hpText != null)
        {
            int current = Mathf.RoundToInt(hp);
            int max = Mathf.RoundToInt(slider.maxValue);

            hpText.text = current + " / " + max;
        }
    }
}