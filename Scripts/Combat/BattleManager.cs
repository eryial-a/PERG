using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // enemy
    public Enemy enemy;
    // visually displays move made by player instance
    public AttackPopupUI attackPopup;
    // stores last cards actually used in hand
    public List<Card> lastUsedCards = new List<Card>();
    // does turn
    public void PlayHand(List<Card> selectedCards)
    {
        // result
        PokerHandResult result = HandEvaluator.EvaluateBest(selectedCards);
        PokerHandType handType = result.handType;
        float effectiveStrength = Player.Instance.strength;

        if (Player.Instance.strengthBuffActive)
        {
            effectiveStrength *= 2;
            Player.Instance.strengthBuffActive = false; // consumes buff
        }
        // last used cards
        lastUsedCards = result.usedCards;
        // damage uses only cards actually used
        float damage = DamageCalculator.CalculateDamage(
            handType,
            lastUsedCards,
            effectiveStrength
        );
        // damage dealt to enemy
        if (enemy != null)
        {
            enemy.OncomingDamage(damage);
        }
        else
        {
            Debug.LogError("No enemy assigned to BattleManager!");
        }
        // visual display
        attackPopup.Show(handType.ToString(), damage);

        Debug.Log("Played " + handType + " using " + lastUsedCards.Count + " cards for " + damage + " damage");
        lastUsedCards = result.usedCards;

        // ACE heals apon usage
        foreach (Card c in lastUsedCards)
        {
            if (c.Rank == 1) // depending on your Ace setup
            {
                Player.Instance.Heal(Mathf.RoundToInt(Player.Instance.maxHealth * 0.02f));
                Debug.Log("Ace played: healed 2% max health");
            }
        }
    }
    public void SetEnemy(Enemy newEnemy)
    {
        enemy = newEnemy;

        FindFirstObjectByType<TurnManager>()?.UpdateBoards();
    }
}