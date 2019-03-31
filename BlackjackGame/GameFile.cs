/*
 * Bosen Qu
 * May 28, 2018
 * This class writes and loads the game information to a text file
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Provides methods for storing and loading the game information
    /// </summary>
    class GameFile
    {
        const string FILE_NAME = "information.txt";
        /// <summary>
        /// Create a delimiter to seperate each section of information
        /// </summary>
        /// <returns>string with 15 digit numbers</returns>
        private string CreateDelimiter()
        {
            string returnValue = null;
            Random NumberGenerator = new Random();
            for (int i = 0, n = 15; i < n; i++)
                returnValue += NumberGenerator.Next(0, 10);
            return returnValue;
        }
        /// <summary>
        /// Check if a given text contains delimiter
        /// </summary>
        /// <param name="text">a given piece of text</param>
        /// <param name="delimiter">the delimiter that needs to be checked</param>
        /// <returns>true if the text contains delimiter, false if the text does not contain delimiter</returns>
        private bool CheckDelimiter(string text, string delimiter)
        {
            if (text.Contains(delimiter))
                return true;
            return false;
        }
        /// <summary>
        /// Load the saved file
        /// </summary>
        /// <returns>all the information last game had</returns>
        public SavedGameInfo Load()
        {
            SavedGameInfo returnValue = new SavedGameInfo();
            try
            {
                if (File.Exists(FILE_NAME))
                {
                    using (StreamReader sr = new StreamReader(FILE_NAME))
                    {
                        //read delimiter
                        string delimiter = sr.ReadLine();
                        //read player info
                        returnValue.PlayerName = sr.ReadLine();
                        returnValue.PlayerNumOfHands = int.Parse(sr.ReadLine());
                        returnValue.PlayerMoney = int.Parse(sr.ReadLine());
                        returnValue.PlayerCurrentBet = int.Parse(sr.ReadLine());
                        //read dealer info
                        returnValue.DealerName = sr.ReadLine();
                        returnValue.DealerNumOfHands = int.Parse(sr.ReadLine());
                        returnValue.DelaerShowFullHands = bool.Parse(sr.ReadLine());
                        //read both player hands, dealer hands, and new cards                   
                        for (int i = 0; i < 5; i++)
                        {
                            Hand currentHand = new Hand();
                            while (true)
                            {
                                string line = sr.ReadLine();
                                if (line == delimiter)
                                    break;
                                Card currentCard = new Card();
                                currentCard.Value = int.Parse(line);
                                currentCard.Suit = int.Parse(sr.ReadLine());
                                currentCard.Face = int.Parse(sr.ReadLine());
                                currentCard.ShowFace = bool.Parse(sr.ReadLine());
                                currentHand.Cards.Add(currentCard);
                            }
                            if (i < 2)
                                returnValue.PlayerHands.Add(currentHand);
                            else if (i < 4)
                                returnValue.DealerHands.Add(currentHand);
                            else
                                returnValue.NewCards = currentHand.Cards;
                        }
                        //read the used cards
                        while (true)
                        {
                            string line = sr.ReadLine();
                            if (line == delimiter)
                                break;
                            Card currentCard = new Card();
                            currentCard.Value = int.Parse(line);
                            currentCard.Suit = int.Parse(sr.ReadLine());
                            currentCard.Face = int.Parse(sr.ReadLine());
                            currentCard.ShowFace = bool.Parse(sr.ReadLine());
                            returnValue.UsedCards.Push(currentCard);
                        }
                    }
                    return returnValue;
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
            return null;
        }
        /// <summary>
        /// Write the current information to a file
        /// </summary>
        /// <param name="player">the current player</param>
        /// <param name="dealer">the current dealer</param>
        /// <param name="deck">the current deck</param>
        public void Save(Player player, Dealer dealer, Deck deck)
        {
            try
            {
                //store info
                string playerInfo = PlayerInformation(player);
                string dealerInfo = DealerInformation(dealer);
                string newCardInfo = GetNewCardsInfo(deck);
                string usedCardInfo = GetUsedCardsInfo(deck);
                //no need to use list
                string[] playerHands = new string[2];
                string[] dealerHands = new string[2];
                for (int i = 0; i < 2; i++)
                    playerHands[i] = HandsInformation(player, i);
                for (int i = 0; i < 2; i++)
                    dealerHands[i] = HandsInformation(dealer, i);
                //create delimiter
                string delimiter = null;
                do
                {
                    delimiter = CreateDelimiter();
                } while (CheckDelimiter(playerInfo, delimiter) == true || CheckDelimiter(dealerInfo, delimiter) == true);
                //writefile
                using (StreamWriter sw = new StreamWriter(FILE_NAME))
                {
                    sw.WriteLine(delimiter);
                    sw.WriteLine(playerInfo);
                    sw.WriteLine(dealerInfo);
                    foreach (string hand in playerHands)
                    {
                        sw.Write(hand);
                        sw.WriteLine(delimiter);
                    }
                    foreach (string hand in dealerHands)
                    {
                        sw.Write(hand);
                        sw.WriteLine(delimiter);
                    }
                    sw.Write(newCardInfo);
                    sw.WriteLine(delimiter);
                    sw.Write(usedCardInfo);
                    sw.Write(delimiter);
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
        /// <summary>
        /// Puts the player's information on a string
        /// </summary>
        /// <param name="player">the current player</param>
        /// <returns>all the player's information stored in a string</returns>
        private string PlayerInformation(Player player)
        {
            string returnValue = null;
            returnValue = player.name;
            returnValue += "\r\n" + player.NumOfHands;
            returnValue += "\r\n" + player.Money;
            returnValue += "\r\n" + player.CurrentBet;
            return returnValue;
        }
        /// <summary>
        /// Puts the dealer's information on a string
        /// </summary>
        /// <param name="dealer">the current dealer</param>
        /// <returns>all the dealer's information stored in a string</returns>
        private string DealerInformation(Dealer dealer)
        {
            string returnValue = null;
            returnValue = dealer.name;
            returnValue += "\r\n" + dealer.NumOfHands;
            returnValue += "\r\n" + dealer.ShowFullHand;
            return returnValue;
        }
        /// <summary>
        /// Reads either a player or dealer's hand information
        /// </summary>
        /// <param name="person">the target that needs to be checked</param>
        /// <param name="handNumber">the index of their hand, either 0 or 1</param>
        /// <returns></returns>
        private string HandsInformation(Person person, int handNumber)
        {
            string returnValue = null;
            if (person.RetrieveHand(handNumber) == null)
                return null;
            Hand hand = person.RetrieveHand(handNumber);
            for (int i = 0, n = hand.Cards.Count; i < n; i++)
            {
                returnValue += hand.Cards[i].Value + "\r\n";
                returnValue += hand.Cards[i].Suit + "\r\n";
                returnValue += hand.Cards[i].Face + "\r\n";
                returnValue += hand.Cards[i].ShowFace + "\r\n";
            }
            return returnValue;
        }
        /// <summary>
        /// Put all the unused cards' information on a string
        /// </summary>
        /// <param name="deck">the current deck</param>
        /// <returns>the unused cards' information stored in a string</returns>
        private string GetNewCardsInfo(Deck deck)
        {
            string returnValue = null;
            while (true)
            {
                Card currentCard = deck.Deal(false);
                if (currentCard == null)
                    break;
                returnValue += currentCard.Value + "\r\n";
                returnValue += currentCard.Suit + "\r\n";
                returnValue += currentCard.Face + "\r\n";
                returnValue += currentCard.ShowFace + "\r\n";
            }
            return returnValue;
        }
        /// <summary>
        /// Uses recursion to reversly put the used cards' information on a string
        /// </summary>
        /// <param name="deck">the current deck</param>
        /// <returns>the used cards' information reversly stored in a string</returns>
        private string GetUsedCardsInfo(Deck deck)
        {
            Card currentCard = deck.PopUsedCard();
            if (currentCard == null)
                return null;
            string cardInfo = currentCard.Value + "\r\n";
            cardInfo += currentCard.Suit + "\r\n";
            cardInfo += currentCard.Face + "\r\n";
            cardInfo += currentCard.ShowFace + "\r\n";
            return GetUsedCardsInfo(deck) + cardInfo;
        }
    }
}
