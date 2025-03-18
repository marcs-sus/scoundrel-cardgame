using Godot;
using System;

/// <summary>
/// Auto-loaded global enums for the game
/// </summary>
namespace ScoundrelGame
{
    public partial class Enums : Node
    {
        // Enums or the card Suits
        public enum Suit
        {
            Hearts,
            Diamonds,
            Clubs,
            Spades
        }

        // Enums or the card Ranks
        public enum Rank
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Five = 5,
            Six = 6,
            Seven = 7,
            Eight = 8,
            Nine = 9,
            Ten = 10,
            Jack = 11,
            Queen = 12,
            King = 13,
            Ace = 14
        }

        // Enums for the card types, according Scoundrel game
        public enum CardType
        {
            Monster,
            Weapon,
            Health
        }
    }
}
