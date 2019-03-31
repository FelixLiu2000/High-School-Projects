/*
 * Bosen Qu
 * May 28, 2018
 * This class creates a deck of card for the game
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Provides methods for managing a set of Card decks currently in use.
    /// </summary>
    class Deck
    {
        //this is a standard deck that are used as a reference
        private List<Card> standardDeck = new List<Card>();
        //create variables to store the cards' information
        private int numberOfDecks;
        private List<Card> newCards = new List<Card>();
        private Stack<Card> usedCards = new Stack<Card>();
        //create a random number generator
        private Random NumberGenerator = new Random();

        /// <summary>
        /// Put all cards into new cards and shuffle them
        /// </summary>
        /// <param name="numberOfDecks">The number of decks users want to use</param>
        public Deck(int numberOfDecks)
        {
            //make sample card
            for(int i = 0; i < 4; i++)
                for(int j = 1; j <= 13; j++)                
                    standardDeck.Add(NewCard(i, j));
            this.numberOfDecks = numberOfDecks;
            //put all cards into new cards
            for (int i = 0; i < numberOfDecks; i++)
                foreach (Card card in standardDeck)
                    newCards.Add(card);
            //shuffle
            Scramble();
        }

        /// <summary>
        /// An alternative constructer that loads all the cards in
        /// </summary>
        /// <param name="newCards">the unused cards from the deck</param>
        /// <param name="usedCards">the cards that are already disposed</param>
        /// <param name="numberOfDecks">the number of decks contained in the deck</param>
        public Deck(List<Card> newCards, Stack<Card> usedCards, int numberOfDecks)
        {
            this.newCards = newCards;
            this.usedCards = usedCards;
            this.numberOfDecks = numberOfDecks;
        }
        /// <summary>
        /// Scramble the cards in the used and new cards lists.
        /// </summary>
        public void Scramble()
        {
            //combine both used cards and new cards
            for (int i = 0, n = usedCards.Count; i < n; i++)
                newCards.Add(usedCards.Pop());
            //shuffle all the cards
            for (int i = 0, n = newCards.Count; i < n; i++)
            {
                int index = NumberGenerator.Next(0, newCards.Count);
                Card temp = newCards[index];
                newCards[index] = newCards[n - 1];
                newCards[n - 1] = temp;
            }
        }
        /// <summary>
        /// Draw a card from new cards
        /// </summary>
        /// <returns></returns>
        public Card Deal(bool showFace)
        {
            //check if the new cards deck is empty
            if (newCards.Count > 0)
            {
                //if not, draw a card from the beginning of the deck
                Card dealCard = newCards[0];              
                newCards.RemoveAt(0);
                dealCard.ShowFace = showFace;
                return dealCard;
            }
            else
                return null;
        }
        /// <summary>
        /// Put a specific card into used cards 
        /// </summary>
        /// <param name="card">The card user wants to dispose</param>
        /// <returns>The card that is disposed</returns>
        public Card Dispose(Card card)
        {
            usedCards.Push(card);
            return card;
        }
        /// <summary>
        /// Takes the toppest card from the used card deck
        /// </summary>
        /// <returns>The card that is taken</returns>
        public Card PopUsedCard()
        {
            if (usedCards.Count > 0)
                return usedCards.Pop();
            else
                return null;        
        }
        /// <summary>
        /// This function sets a new card
        /// </summary>
        /// <param name="suit">the suit of the card</param>
        /// <param name="face">the face value of the card</param>
        /// <returns>the new card that is created</returns>
        private Card NewCard(int suit, int face)
        {
            Card newCard = new Card();
            //initialize the card
            newCard.Face = face;
            newCard.Suit = suit;
            newCard.ShowFace = true;         
            //set the value
            if (face == 1)
                newCard.Value = 11;
            else if (face > 1 && face < 11)
                newCard.Value = face;
            else if (face >= 11 && face <= 13)
                newCard.Value = 10;
            //return the card
            return newCard;
        }
    }
}
