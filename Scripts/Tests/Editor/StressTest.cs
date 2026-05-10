using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandEvaluatorStressTest : MonoBehaviour
{
    public int testIterations = 100; // Default amount of test, change the value in the triangle test not here
    // Starts test + royal flush test
    void Start()
    {
        // uncomment to manually insert a royal flush of hearts
        //TestRoyalFlush();   
        RunStressTest();
    }

    void RunStressTest()
    {
        Deck deck = new Deck();

        // Dictionary to count each hand type
        Dictionary<PokerHandType, int> handCounts = new Dictionary<PokerHandType, int>();

        // Initialize counts
        foreach (PokerHandType type in System.Enum.GetValues(typeof(PokerHandType)))
        {
            handCounts[type] = 0;
        }

        for (int i = 0; i < testIterations; i++)
        {
            deck.InitializeDeck();
            deck.Shuffle();

            // Draw 9 cards (player hand)
            List<Card> playerHand = new List<Card>();

            for (int j = 0; j < 9; j++)
                playerHand.Add(deck.Draw());

            var bestPlay = FindBestHand(playerHand);

            // Count the best hand type
            handCounts[bestPlay.Item2]++;
        }

        // Print results
        Debug.Log("===== Stress Test Results =====");

        foreach (var entry in handCounts)
        {
            Debug.Log(entry.Key + ": " + entry.Value);
        }

        Debug.Log("Total Hands Tested: " + testIterations);
    }
    // searches for best hand
    (List<Card>, PokerHandType) FindBestHand(List<Card> cards)
    {
        PokerHandType bestType = PokerHandType.HighCard;
        List<Card> bestCards = new List<Card>();
        // for loop that evaluates hands, 2 -> 5
        for (int size = 2; size <= 5; size++)
        {
            var combinations = GenerateCombinations(cards, size);
            // goes through all combinations
            foreach (var combo in combinations)
            {
                PokerHandType result = HandEvaluator.Evaluate(combo);
                // if new best found replace best results
                if (bestCards.Count == 0 || result > bestType)
                {
                    bestType = result;
                    bestCards = combo;
                }
            }
        }
        // returns best combination / hand
        return (bestCards, bestType);
    }
    // creats every possible group of cards for specific size
    List<List<Card>> GenerateCombinations(List<Card> cards, int combinationSize)
    {
        List<List<Card>> result = new List<List<Card>>();
        GenerateRecursive(cards, combinationSize, 0, new List<Card>(), result);
        return result;
    }
    // builds hands
    void GenerateRecursive(List<Card> cards, int size, int start,
                           List<Card> current, List<List<Card>> result)
    {
        // stops once hand is complete
        if (current.Count == size)
        {
            result.Add(new List<Card>(current));
            return;
        }
        // adds each remaining card
        for (int i = start; i < cards.Count; i++)
        {
            current.Add(cards[i]); // adds card to saved list
            GenerateRecursive(cards, size, i + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }
    // test royal flush (Manual insert)
    void TestRoyalFlush()
    {
        List<Card> royal = new List<Card>()
        {
            new Card(Suit.Hearts, 10),
            new Card(Suit.Hearts, 11),
            new Card(Suit.Hearts, 12),
            new Card(Suit.Hearts, 13),
            new Card(Suit.Hearts, 1)
        };

        PokerHandType result = HandEvaluator.Evaluate(royal);
        // print
        Debug.Log("===== MANUAL ROYAL FLUSH TEST =====");
        Debug.Log("Hand: " + string.Join(", ", royal.Select(c => c.ToString())));
        Debug.Log("Result: " + result);
    }

}