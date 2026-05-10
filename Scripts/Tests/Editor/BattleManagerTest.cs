using System.Collections.Generic;
using UnityEngine;

public class BattleManagerTest : MonoBehaviour
{
    public HandManager handManager;
    public BattleManager battleManager;
    //test runs
    public int testRuns = 5;

    void Start()
    {
        if (handManager == null || battleManager == null)
        {
            Debug.LogError("HandManager or BattleManager not assigned in the Inspector!");
            return;
        }

        RunTest();
    }

    void RunTest()
    {
        Debug.Log("=-= HAND & BATTLE TEST START =-=");

        for (int i = 0; i < testRuns; i++)
        {
            // refill player hand
            handManager.RefillHand();
            // get current hand
            List<Card> playerHand = handManager.GetCurrentHand();
            // random hand size
            int handSize = Random.Range(2, Mathf.Min(6, playerHand.Count + 1));

            List<Card> selectedCards = new List<Card>();
            List<int> usedIndexes = new List<int>();
            // randomly selects cards from hand
            while (selectedCards.Count < handSize)
            {
                int index = Random.Range(0, playerHand.Count);
                if (!usedIndexes.Contains(index))
                {
                    selectedCards.Add(playerHand[index]);
                    usedIndexes.Add(index);
                }
            }
            // plays selected cards
            battleManager.PlayHand(selectedCards);
            // log selected cards
            string handStr = "";
            foreach (var card in selectedCards)
            {
                handStr += card.Rank + " of " + card.Suit + ", ";
            }

            Debug.Log($"Test {i + 1}: Played {selectedCards.Count}-card hand: {handStr}");
        }

        Debug.Log("=-= HAND & BATTLE TEST COMPLETE =-=");
    }
}