using System;
// enum suits
public enum Suit
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}
// card class
public class Card
{
    public Suit Suit { get; private set; }
    public int Rank { get; private set; }
    // creates card (suit + rank)
    public Card(Suit suit, int rank)
    {
        this.Suit = suit;
        this.Rank = rank;
    }
    // return string value of card
    public override string ToString()
    {
        return $"{Rank} of {Suit}";
    }
}