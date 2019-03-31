/*
 * Felix Liu
 * May 28, 2018
 * This class sets and process the hand information for both dealer and player
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Provide methods and attributes for the current set of Cards possessed by a Player or Dealer.
    /// </summary>
    public class Hand
    {
        private bool isBusted;
        private int handValue;

        private List<Card> cards = new List<Card>();

        public bool IsBusted
        {
            get
            {
                return isBusted;
            }
        }

        public int Value
        {
            get
            {
                return handValue;
            }
        }

        public List<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        /// <summary>
        /// Calculate the number of cards in the hand
        /// </summary>
        /// <returns>returns the size of the cards</returns>
        public int Size
        {
            get
            {
                return cards.Count;
            }
        }

        /// <summary>
        /// Calculate the sum of the cards value
        /// </summary>
        /// <returns>the sum of the cards value</returns>
        private int GetHandValue()
        {
            int value = 0;
            foreach (Card card in cards)
                value += card.Value;
            handValue = value;
            return value;
        }

        /// <summary>
        /// Determine whether person's hand has reached a given limit, for instance if the player has busted. Automatically resolves ace values.
        /// </summary>
        /// <returns>true if the person has not reached a given limit, false if otherwise</returns>
        public bool LimitExceeded(int limit)
        {
            // Current hand value has exceeded limit
            if (GetHandValue() > limit)
            {
                // Resolve ace values
                for (int i = 0, n = cards.Count; i < n; i++)
                if (cards[i].Value == 11)
                {
                    cards[i].Value = 1;
                    // New resolved hand value has not exceeded limit
                    if (GetHandValue() <= limit)
                        return false;
                }
                // New resolved hand value has still exceeded limit
                return true;
            }
            // Current limit has not exceeded limit
            else
                return false;
        }

        /// <summary>
        /// Add a new card to the player's hand
        /// </summary>
        /// <param name="newCard">The new card player received</param>
        /// <returns>The new card just added to the hand</returns>
        public Card Receive(Card newCard)
        {
            cards.Add(newCard);
            if (LimitExceeded(Blackjack.MAX_HAND_VALUE) == true) 
                isBusted = true;
            return newCard;
        }

        /// <summary>
        /// Remove a card from the hand
        /// </summary>
        /// <param name="index">The index of the card wanted to be removed</param>
        /// <returns>The card just removed</returns>
        public Card Dispose(int index)
        {
            Card returnValue = cards[index];
            cards.RemoveAt(index);
            return returnValue;
        }

        /// <summary>
        /// Sets the show face property of a card
        /// </summary>
        /// <param name="index">The index of the card to be shown/hidden</param>
        /// <returns></returns>
        public Card SetFace(int index, bool state)
        {
            cards[index].ShowFace = state;
            return cards[index];
        }

        public bool CheckBlackjack()
        {
            if (cards.Count() >= 2)
            {
                // If first two cards are equal to 21 (A 10 card and Ace)
                if (cards[0].Value + cards[1].Value == Blackjack.MAX_HAND_VALUE)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }
    }
}
