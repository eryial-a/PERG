using System.Collections.Generic;

public class PokerHandResult
{
    public PokerHandType handType;
    public List<Card> usedCards;

    public PokerHandResult(PokerHandType type, List<Card> cards)
    {
        handType = type;
        usedCards = cards;
    }
}