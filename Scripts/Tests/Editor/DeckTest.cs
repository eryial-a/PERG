using NUnit.Framework;
// deck test
public class DeckTests
{
    [Test]
    public void Deck_Has52Cards_AfterInit()
    {
        Deck deck = new Deck();
        deck.InitializeDeck();

        Assert.AreEqual(52, deck.GetDeckSnapshot().Count);
    }

    [Test]
    public void Deck_Shuffle_Changes_Order()
    {
        Deck deck = new Deck();
        deck.InitializeDeck();

        var original = deck.GetDeckSnapshot();
        deck.Shuffle();
        var shuffled = deck.GetDeckSnapshot();

        bool changed = false;

        for (int i = 0; i < original.Count; i++)
        {
            if (original[i].Rank != shuffled[i].Rank ||
                original[i].Suit != shuffled[i].Suit)
            {
                changed = true;
                break;
            }
        }

        Assert.IsTrue(changed);
    }
    
    [Test]
    public void Deck_Reshuffles_From_Discard()
    {
        Deck deck = new Deck();
        deck.InitializeDeck();

        for (int i = 0; i < 52; i++)
        {
            Card c = deck.Draw();
            deck.AddToDiscard(c);
        }

        Card newCard = deck.Draw();

        Assert.IsNotNull(newCard);
        Assert.AreEqual(51, deck.GetDeckSnapshot().Count);
    }
}