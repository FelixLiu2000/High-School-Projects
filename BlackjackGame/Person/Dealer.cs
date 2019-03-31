using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Person that represents the casino and plays against the player. Inherits the generic Person class.
    /// </summary>
    public class Dealer : Person
    {
        private const int DEALER_HIT_LIMIT = 17;
        private bool showFullHand;
        public bool ShowFullHand
        {
            get
            {
                return showFullHand;
            }
            set
            {
                showFullHand = value;
                // Flip the second card to show/hide its face (opposite of ShowFullHand)
                hands[1].SetFace(1, !value);
            }
        }

        public Dealer(string name) : base(name) { }

        public Dealer() : base("Computer") { }

        public bool DealerLimitExceeded
        {
            get
            {
                return hands[0].LimitExceeded(DEALER_HIT_LIMIT);
            }
        }

        public override void ReceiveCard(Card card, int handNumber)
        {
            // If there the current card will become the second card of the dealer's hand
            if (hands[0].Size == 1)
            {
                card.ShowFace = false;
            }
            hands[0].Receive(card);
        }
    }
}
