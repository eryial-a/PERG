using System.Collections.Generic;
using System.Linq;
// Poker Hand
public enum PokerHandType
{
    HighCard,
    Pair,
    TwoPair,
    ThreeKind,
    Straight,
    Flush,
    FullHouse,
    FourKind,
    StraightFlush,
    RoyalFlush
}

// Hand evaluator class
public class HandEvaluator
{
    public static PokerHandType Evaluate(List<Card> cards)
    {
        int cardCount = cards.Count;
        // get functions
        var rankCounts = GetRankCounts(cards);
        var suitCounts = GetSuitCounts(cards);
        // boolean values for flush and straight function
        bool flush = suitCounts.ContainsValue(cardCount);
        bool straight = IsStraight(cards);
        // handles hands of 5
        if (cardCount == 5)
        {
            if (straight && flush && IsRoyal(cards)) return PokerHandType.RoyalFlush;
            if (straight && flush) return PokerHandType.StraightFlush;
            if (rankCounts.ContainsValue(4)) return PokerHandType.FourKind;
            if (rankCounts.ContainsValue(3) && rankCounts.ContainsValue(2)) return PokerHandType.FullHouse;
            if (flush) return PokerHandType.Flush;
            if (straight) return PokerHandType.Straight;
        }
        // hands 4 or less
        if (cardCount >= 4)
        {
            if (rankCounts.ContainsValue(4)) return PokerHandType.FourKind;
            if (rankCounts.ContainsValue(3)) return PokerHandType.ThreeKind;
            if (CountPairs(rankCounts) == 2) return PokerHandType.TwoPair;
        }
        // handes 3 or less
        if (cardCount >= 3)
        {
            if (rankCounts.ContainsValue(3)) return PokerHandType.ThreeKind;
        }
        // hands 1 or more
        int pairs = CountPairs(rankCounts);
        if (pairs >= 1)
        {
            if (pairs == 2) return PokerHandType.TwoPair;
            return PokerHandType.Pair;
        }
        // high card
        return PokerHandType.HighCard;
    }
    // keeps track of rank for evaluation
    static Dictionary<int,int> GetRankCounts(List<Card> cards)
    {
        Dictionary<int,int> counts = new Dictionary<int,int>();
        foreach (Card c in cards)
        {
            if (!counts.ContainsKey(c.Rank)) counts[c.Rank] = 0;
            counts[c.Rank]++;
        }
        return counts;
    }
    // keeps track of suit for evalutation
    static Dictionary<Suit,int> GetSuitCounts(List<Card> cards)
    {
        Dictionary<Suit,int> counts = new Dictionary<Suit,int>(); 
        foreach (Card c in cards)
        {
            if (!counts.ContainsKey(c.Suit)) counts[c.Suit] = 0;
            counts[c.Suit]++;
        }
        return counts; // example: hearts -> 3, spades -> 2 clubs -> 2 diamonds -> 2
    }
    // counts amount of pairs.
    static int CountPairs(Dictionary<int,int> rankCounts)
    {
        int pairs = 0;
        foreach (var value in rankCounts.Values)
        {
            if (value == 2) pairs++;
        }
        return pairs; // example: 3 -> 3, 6 -> 1, 7 -> 1 , 10 -> 1 , 11 -> 1 , 12 -> 1 , 13 -> 1
    }
    // is straight NOW works with ace.
    static bool IsStraight(List<Card> cards)
    {
        var ranks = cards.Select(c => c.Rank).Distinct().OrderBy(r => r).ToList();

        if (ranks.Count != cards.Count)
            return false;

        // Normal straight
        if (ranks.Last() - ranks.First() == cards.Count - 1)
            return true;

        // Ace-high straight (10 J Q K A)
        if (ranks.SequenceEqual(new List<int> {1,10,11,12,13}))
            return true;

        // Ace-low straight (A 2 3 4 5)
        if (ranks.SequenceEqual(new List<int> {1,2,3,4,5}))
            return true;

        // King-low straight (K A 2 3 4)
        if (ranks.SequenceEqual(new List<int> {1,2,3,4,13}))
            return true;

        // Queen-low straight (Q K A 2 3)
        if (ranks.SequenceEqual(new List<int> {1,2,3,12,13}))
            return true;

        // Jack-low straight (J Q K A 2)
        if (ranks.SequenceEqual(new List<int> {1,2,11,12,13}))
            return true;

        return false;
    }
    // checks for royality, both are in here incase of joker
    static bool IsRoyal(List<Card> cards)
    {
        var ranks = cards.Select(c => c.Rank).OrderBy(r => r).ToList();

        return ranks.SequenceEqual(new List<int> {10,11,12,13,14}) ||
               ranks.SequenceEqual(new List<int> {1,10,11,12,13});
    }
    // helpers
  static bool IsBetterHand(PokerHandType newType, List<Card> newCards,
                         PokerHandType oldType, List<Card> oldCards)
    {
        int newRank = (int)newType;
        int oldRank = (int)oldType;

        if (newRank > oldRank)
            return true;
        // high card
        if (newRank == oldRank)
        {
            // empty hand loses
            if (oldCards.Count == 0)
                return true;
            // prefer fewer cards for same hand type
            if (newCards.Count < oldCards.Count)
                return true;
            // prefers higher ranked cards
            if (newCards.Count == oldCards.Count)
            {
                int newHigh = newCards.Max(c => c.Rank);
                int oldHigh = oldCards.Max(c => c.Rank);
                // return higher rank
                if (newHigh > oldHigh)
                    return true;
            }
        }

        return false;
    }
    // Grabs all subsets possible from card list
    static List<List<Card>> GetAllSubsets(List<Card> cards, int min, int max)
    {
        List<List<Card>> result = new List<List<Card>>();
        // accounts for cards count and adds total
        int count = cards.Count;
        int total = 1 << count;
        // loops to add cards to subset
        for (int mask = 1; mask < total; mask++)
        {
            List<Card> subset = new List<Card>();

            for (int i = 0; i < count; i++)
            {
                if ((mask & (1 << i)) != 0)
                    subset.Add(cards[i]);
            }
            // when greater than minimum abd less then max it will add to subset
            if (subset.Count >= min && subset.Count <= max)
                result.Add(subset);
        }

        return result;
    }
    // will evaluate best hand out of the result
    public static PokerHandResult EvaluateBest(List<Card> cards)
    {
        PokerHandType bestType = PokerHandType.HighCard;
        List<Card> bestCards = new List<Card>();
        // grabs combos starting from 1 to 5
        var combos = GetAllSubsets(cards, 1, 5);
        // Sorts through all combos
        foreach (var combo in combos)
        {
            PokerHandType type = Evaluate(combo);
            // chooses best hand type (To later prevent extra selected cards from being used)
            if (IsBetterHand(type, combo, bestType, bestCards))
            {
                bestType = type;
                bestCards = new List<Card>(combo);
            }
        }
        // gives result of combo
        return new PokerHandResult(bestType, bestCards);
    }
    
}
