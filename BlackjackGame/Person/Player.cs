using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Person that represents a human player playing against the Dealer/casino. Inherits the generic Person class.
    /// </summary>
    public class Player : Person
    {
        private bool hittable = true;
        public bool Hittable
        {
            get { return hittable; }
        }

        public bool Bettable
        {
            get
            {
                if (currentBet > 0)
                    return false;
                else
                    return true;
            }
        }

        private int money;
        public int Money
        {
            get { return money; }
            private set
            {
                if (value < 0)
                {
                    money = 0;
                }
                else
                    money = value;
            }
        }

        private int currentBet;
        public int CurrentBet
        {
            get { return currentBet; }
        }

        public Player(string name, int money)
            : base(name)
        {

        }

        public Player(int money)
            : base("Player")
        {

        }

        public override void ReceiveCard(Card card, int handNumber)
        {
            if (handNumber < hands.Length && handNumber >= 0 && hands.Length > 0)
            {
                hands[handNumber].Receive(card);
            }
        }

        /// <summary>
        /// Checks if a given hand is busted.
        /// </summary>
        /// <param name="hand">Specifies index of hand number, 0 or 1. </param>
        /// <returns>Returns true if hand is busted, false if not. Returns true when the hand does not exist.</returns>
        public bool CheckBusted(int hand)
        {
            if (hands[hand] != null || (hand != 0 && hand != 1))
            {
                return hands[hand].IsBusted;
            }
            else
                return true;
        }

        /// <summary>
        /// Adds to or subtracts the player's current money by a specified amount.
        /// </summary>
        /// <param name="amount">Positive or negative value that is added to the player's current money.</param>
        /// <returns></returns>
        public int UpdateMoney(int amount)
        {
            if (money < 0)
            {
                return -1;
            }
            else
            {
                Money += amount;

                return Money;
            }
        }

        public int Bet(int bet)
        {
            // If no bet has been made yet
            if (Bettable == true)
            {
                // If the player's bet is within the stake limits and less than their current money
                if (bet >= Blackjack.StakesMinimum && bet <= Blackjack.StakesMaximum && bet <= money)
                {
                    currentBet = bet;
                    return currentBet;
                }
            }
            // Default value for failed bet
            return -1;
        }

        public int DoubleDown()
        {
            if (currentBet > 0)
            {
                currentBet *= 2;
                hittable = false;
                return currentBet;
            }
            else
                return -1;
        }
    }
}
