using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
// decision manager
public class DecisionManager : MonoBehaviour
{
    private bool isTransitioning = false;
    // rest option
    public void Rest(Button clickedButton)
    {
        // is it transitioning
        if (isTransitioning) return;
        // decision check
        if (!TryUseDecision()) return;
        clickedButton.interactable = false;
        int oldHealth = Player.Instance.currentHealth;
        // heal calculation
        int healAmount = (int)(Player.Instance.maxHealth * 0.15f);
        Player.Instance.currentHealth += healAmount;
        // clamp health
        if (Player.Instance.currentHealth > Player.Instance.maxHealth)
            Player.Instance.currentHealth = Player.Instance.maxHealth;
        int actualHeal = Player.Instance.currentHealth - oldHealth;
        StartCoroutine(RestSequence(actualHeal));
    }

    // search item
    public void SearchItem(Button clickedButton)
    {
        // checks if transitioning
        if (isTransitioning) return;
        // try decision
        if (!TryUseDecision()) return;
        clickedButton.interactable = false;
        // roll random value
        float roll = Random.value;
        // if roll is 0.35 and below you get a heal potion
        if (roll <= 0.35f)
        {
            Debug.Log("Got heal potion!");
            Player.Instance.healthPotions++;
            StartCoroutine(SearchSequence(true));
        }
        else
        {
            Debug.Log("Nothing Found");
            StartCoroutine(SearchSequence(false));
        }
    }
    // next enemy
    public void NextEnemy(Button clickedButton)
    {
        // if transitioning is true return
        if (isTransitioning) return;
        // set button interaction to false
        // if cant use decision return
        if (!TryUseDecision()) return;
        clickedButton.interactable = false;
        Player.Instance.enemiesDefeated--;
        StartCoroutine(EnemySequence());
    }
    // exit
    public void Exit()
    {
        if (isTransitioning) return;

        isTransitioning = true;

        Debug.Log("Exit selected");

        StartCoroutine(ExitDelay());
    }
    // apply rewards after beating enemy
    void ApplyRewards()
    {
        Player.Instance.strength += 0.2f;
        Player.Instance.maxHealth += 100;
        Player.Instance.currentHealth += 100;
        StartCoroutine(LoadCombatDelay());
    }
    // apply small rewards
    void ApplySmallRewards()
    {
        Player.Instance.strength += 0.1f;
        Player.Instance.maxHealth += 50;
        Player.Instance.currentHealth += 50;

        StartCoroutine(LoadCombatDelay());
    }
    // adds delay to combat
    IEnumerator LoadCombatDelay()
    {
        isTransitioning = true;

        yield return new WaitForSeconds(1.2f);

        GameStateManager.Instance.ReturnToCombat();
    }
    // exit delay.
    IEnumerator ExitDelay()
    {
        yield return new WaitForSeconds(1.2f);

        GameStateManager.Instance.ExitFunction();
    }
    // try to use decision
    bool TryUseDecision()
    {
        if (Player.Instance == null) return false;
        // checks if player made a decision
        if (Player.Instance.madeDecision)
        {
            Debug.Log("Decision already made.");
            return false;
        }
        // made decision true
        Player.Instance.madeDecision = true;
        return true;
    }
    // rest sequence
    IEnumerator RestSequence(int healAmount)
    {
        isTransitioning = true;

        yield return DecisionPopupUI.Instance.ShowPopup(
            "Recovered " + healAmount + " HP!"
        );

        ApplyRewards();
    }
    // searches for item
    IEnumerator SearchSequence(bool foundPotion)
    {
        isTransitioning = true;
        // found potion (updates popup)
        if (foundPotion)
        {
            yield return DecisionPopupUI.Instance.ShowPopup(
                "You found a Health Potion!"
            );
        }
        else // fail
        {
            yield return DecisionPopupUI.Instance.ShowPopup(
                "Nothing was found..."
            );
        }
        // apply reward
        ApplyRewards();
    }
    // enemy sequence
    IEnumerator EnemySequence()
    {
        isTransitioning = true;
        // updates popup
        yield return DecisionPopupUI.Instance.ShowPopup(
            "Weak enemy found..."
        );

        ApplySmallRewards();
    }
}