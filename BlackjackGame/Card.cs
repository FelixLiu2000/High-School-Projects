using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Provides methods and attributes for a Blackjack playing card.
    /// </summary>
    public class Card
    {
        public int Value { get; set; }
        public int Face { get; set; }
        public bool ShowFace { get; set; }
        public int Suit { get; set; }

        public const int DIAMONDS = 0, CLUBS = 1, HEARTS = 2, SPADE = 3;
    }
}
