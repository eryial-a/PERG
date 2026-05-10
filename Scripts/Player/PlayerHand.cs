using System.Collections.Generic;

public class PlayerHand
{
    public List<Card> cards = new List<Card>();
    private const int MAX_HAND_SIZE = 9;
    // draws first 9 cards from created deck
    public void DrawStartingHand(Deck deck)
    {
        while (cards.Count < MAX_HAND_SIZE)
        {
            Card card = deck.Draw();
            if (card != null)
                cards.Add(card);
        }
    }
    // remove cards
    public void RemoveCards(List<Card> playedCards)
    {
        foreach (Card card in playedCards)
        {
            cards.Remove(card);
        }
    }
    // refill hand
    public void RefillHand(Deck deck)
    {
        while (cards.Count < MAX_HAND_SIZE)
        {
            Card card = deck.Draw();
            if (card == null)
                break;

            cards.Add(card);
        }
    }

}