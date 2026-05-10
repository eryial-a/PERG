using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> cards = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    // debug units
    public int DeckCount => cards.Count;
    public int DiscardCount => discardPile.Count;
    // create deck
    public void InitializeDeck()
    {
        cards.Clear();
        foreach (Suit suit in System.Enum.GetValues(typeof(Suit)))
        {
            for (int i = 1; i <= 13; i++)
            {
                cards.Add(new Card(suit, i));
            }
        }
    }
    // shuffles deck
    public void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card temp = cards[i];
            int randomIndex = Random.Range(i, cards.Count);
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }
    // draw card
    public Card Draw()
    {
        if (cards.Count == 0)
        {
            ReshuffleDiscardIntoDeck();
            // no cards left to draw
            if (cards.Count == 0)
            {
                Debug.LogWarning("No cards left to draw!");
                return null;
            }
        }
        // returns drawn card
        Card card = cards[0];
        cards.RemoveAt(0);
        // debugs
        Debug.Log("Deck: " + cards.Count + " | Discard: " + discardPile.Count);

        return card;
    }
    // adds discarded and played cards to discard pile.
    public void AddToDiscard(Card card)
    {
        if (card != null)
            discardPile.Add(card);
    }
    // reshuffle discarded pile as deck
    void ReshuffleDiscardIntoDeck()
    {
        if (discardPile.Count == 0)
            return;

        Debug.Log("Reshuffling discard into deck...");
        // reshuffles discarded pile as new deck
        cards.AddRange(discardPile);
        discardPile.Clear();
        Shuffle();
        // debug
        PrintDeckOrder(); 
        Debug.Log("Deck: " + DeckCount + " | Discard: " + DiscardCount);
    }
    // prints order of deck (debug)
    public void PrintDeckOrder()
    {
        Debug.Log("---- DECK ORDER ----");

        for (int i = 0; i < cards.Count; i++)
        {
            Debug.Log(i + ": " + cards[i]);
        }

        Debug.Log("--------------------");
    }
    // prints discard pile order (debug)
    public void PrintDiscardOrder()
    {
        Debug.Log("---- DISCARD PILE ----");

        for (int i = 0; i < discardPile.Count; i++)
        {
            Debug.Log(i + ": " + discardPile[i]);
        }

        Debug.Log("----------------------");
    }
    // prints list of cards in deck
    public List<Card> GetDeckSnapshot()
    {
        return new List<Card>(cards);
    }
    // prints list of cards in discard pile
    public List<Card> GetDiscardSnapshot()
    {
        return new List<Card>(discardPile);
    }
}