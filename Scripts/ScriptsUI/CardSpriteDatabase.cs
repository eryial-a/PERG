using UnityEngine;

public class CardSpriteDatabase : MonoBehaviour
{
    public Sprite[] diamonds;
    public Sprite[] hearts;
    public Sprite[] spades;
    public Sprite[] clubs;
    // gets sprite
    public Sprite GetSprite(Card card)
    {
        int index = card.Rank - 1;
        // assign card sprite baed on index
        switch (card.Suit)
        {
            case Suit.Diamonds: return diamonds[index];
            case Suit.Hearts: return hearts[index];
            case Suit.Spades: return spades[index];
            case Suit.Clubs: return clubs[index];
        }

        return null;
    }
}