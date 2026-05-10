using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HandEvaluatorRandomnessTest : MonoBehaviour
{
    public int testIterations = 1000;

    void Start()
    {
        RunStressTest();
    }

    void RunStressTest()
    {
        Deck deck = new Deck();

        for (int i = 0; i < testIterations; i++)
        {
            deck.InitializeDeck();
            deck.Shuffle();

            // Draw 9 cards (player hand)
            List<Card> playerHand = new List<Card>();

            for (int j = 0; j < 9; j++)
                playerHand.Add(deck.Draw());

            var bestPlay = FindBestHand(playerHand);

            Debug.Log("Player Hand: " + string.Join(", ", playerHand.Select(c => c.ToString())));

            Debug.Log("Best Play: " + bestPlay.Item2 + " -> " +
                string.Join(", ", bestPlay.Item1.Select(c => c.ToString())));
        }

        Debug.Log("Stress Test Completed");
    }

    (List<Card>, PokerHandType) FindBestHand(List<Card> cards)
    {
        PokerHandType bestType = PokerHandType.HighCard;
        List<Card> bestCards = new List<Card>();

        // Check combinations from 2 to 5 cards
        for (int size = 2; size <= 5; size++)
        {
            var combinations = GenerateCombinations(cards, size);

            foreach (var combo in combinations)
            {
                PokerHandType result = HandEvaluator.Evaluate(combo);

                if (bestCards.Count == 0 || result > bestType)
                {
                    bestType = result;
                    bestCards = combo;
                }
            }
        }

        return (bestCards, bestType);
    }

    List<List<Card>> GenerateCombinations(List<Card> cards, int combinationSize)
    {
        List<List<Card>> result = new List<List<Card>>();
        GenerateRecursive(cards, combinationSize, 0, new List<Card>(), result);
        return result;
    }

    void GenerateRecursive(List<Card> cards, int size, int start,
                           List<Card> current, List<List<Card>> result)
    {
        if (current.Count == size)
        {
            result.Add(new List<Card>(current));
            return;
        }

        for (int i = start; i < cards.Count; i++)
        {
            current.Add(cards[i]);
            GenerateRecursive(cards, size, i + 1, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }
}