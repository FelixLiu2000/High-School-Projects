using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Generic class that provides attributes and methods for any Blackjack participant.
    /// </summary>
    public abstract class Person
    {
        public string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public int NumOfHands
        {
            get
            {
                return hands.Length;
            }
        }

        protected Hand[] hands;

        public Hand RetrieveHand(int handNumber)
        {
            if (hands[handNumber] != null)
            {
                Hand hand = hands[handNumber];
                return hand;
            }
            else
                return null;
        }

        public Person(string personName)
        {
            name = personName;
            hands = new Hand[1];
        }

        public bool HasBlackjack(int handNumber)
        {
            if (handNumber >= 0 && handNumber < hands.Length)
            {
                return hands[handNumber].CheckBlackjack();
            }
            else
                return false;
        }

        /// <summary>
        /// Returns a copy of the person's hands and disposes the originals.
        /// </summary>
        /// <returns>Returns the person's array of hands to save the cards into the deck's used card list.</returns>
        public Hand[] DisposeHands()
        {
            Hand[] usedHands = hands;
            // Dispose used hands and create new hands array
            hands = new Hand[hands.Length];
            // Return used hands
            return usedHands;
        }

        public abstract void ReceiveCard(Card card, int handNumber);
    }
}
