/*
 * Bosen Qu
 * May 28, 2018
 * This class stores all the game information from a file
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Provide methods to store all the information from a file
    /// </summary>
    class SavedGameInfo
    {
        //variable for players
        public string PlayerName { get; set; }
        public int PlayerNumOfHands { get; set; }
        public int PlayerMoney { get; set; }
        public int PlayerCurrentBet { get; set; }
        public List<Hand> PlayerHands { get; set; }
        //variable for dealers
        public string DealerName { get; set; }
        public int DealerNumOfHands { get; set; }
        public bool DelaerShowFullHands { get; set; }
        public List<Hand> DealerHands { get; set; }
        //variable for deck
        public List<Card> NewCards { get; set; }
        public Stack<Card> UsedCards { get; set; }

        /// <summary>
        /// Initialize all the lists and stacks inside the class
        /// </summary>
        public SavedGameInfo()
        {
            PlayerHands = new List<Hand>();
            DealerHands = new List<Hand>();
            NewCards = new List<Card>();
            UsedCards = new Stack<Card>();
        }
    }
}
