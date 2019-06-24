using System;
namespace CardSystem
{
    public abstract class CardDenomination
    {
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }

    public abstract class CardRank : CardDenomination { }

    public abstract class CardSuit : CardDenomination { }

    // Traditional suits Spades, Hearts, Diamonds, Clubs will have one of these colors associated with them.
    public enum CardColor { Red, Black, Wild }

    public abstract class Card
    {
        CardRank Rank;
        CardSuit Suit;
    }
}
