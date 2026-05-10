using System.Buffers.Text;
using UnityEngine;
using System.Collections.Generic;
// values are subject for change
public class DamageCalculator
{
    // strength now added for calculation
    public static float CalculateDamage(PokerHandType handType, List<Card> usedCards, float strength)
    {
        int baseDamage = 0;

        switch(handType)
        {
            case PokerHandType.HighCard:
                baseDamage = 15;
                break;

            case PokerHandType.Pair:
                baseDamage = 30;
                break;

            case PokerHandType.TwoPair:
                baseDamage = 60;
                break;

            case PokerHandType.ThreeKind:
                baseDamage = 90;
                break;

            case PokerHandType.Straight:
                baseDamage = 120;
                break;

            case PokerHandType.Flush:
                baseDamage = 150;
                break;

            case PokerHandType.FullHouse:
                baseDamage = 200;
                break;

            case PokerHandType.FourKind:
                baseDamage = 300;
                break;

            case PokerHandType.StraightFlush:
                baseDamage = 500;
                break;

            case PokerHandType.RoyalFlush:
                baseDamage = 1000;
                break;
        }

        // Bonus from special cards
        float faceCardBonus = 0f;

        foreach (Card c in usedCards)
        {
            switch (c.Rank)
            {
                case 11: // Jack
                    faceCardBonus += 0.05f;
                    break;

                case 12: // Queen
                    faceCardBonus += 0.10f;
                    break;

                case 13: // King
                    faceCardBonus += 0.25f;
                    break;

                case 14: // Ace
                case 1:
                    faceCardBonus += 0.30f;
                    break;
            }
        }
        // Final multiplier
        float multiplier = 1f + faceCardBonus;

        return baseDamage * multiplier * strength;
    }
}
