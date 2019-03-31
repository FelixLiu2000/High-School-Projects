/*
 * Felix Liu
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Main form that works with the user interface and manages the Blackjack program events and gameplay.
    /// </summary>
    public partial class Blackjack : Form
    {
        public static Player player;
        public static Dealer dealer;
        private Deck deck;

        // Program display dimensions
        public const int CLIENT_WIDTH = 1300;
        public const int CLIENT_HEIGHT = 800;
        private static Size displaySize;
        public static Size DisplaySize
        {
            get { return displaySize; }
        }

        public const int MAX_HAND_VALUE = 21;
        private static int stakesMinimum = 5;
        public static int StakesMinimum
        {
            get { return stakesMinimum; }
        }
        private static int stakesMaximum = 250;
        public static int StakesMaximum
        {
            get { return stakesMaximum; }
        }
        private static int startingMoney = 1000;
        public static int StartingMoney
        {
            get { return startingMoney; }
        }
        private static int numOfDecks = 4;
        public static int NumOfDecks
        {
            get { return numOfDecks; }
        }

        public Blackjack()
        {
            InitializeComponent();
            
            // Set, lock and save program size
            ClientSize = new Size(CLIENT_WIDTH, CLIENT_HEIGHT);
            MaximizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            displaySize = ClientSize;

            UI.SetState(UIState.Menu);
        }

        public static bool CreateDealer(string name)
        {
            if (dealer == null)
            {
                if (name != "")
                    dealer = new Dealer(name);
                else
                    dealer = new Dealer();

                return true;
            }
            else
                return false;
        }

        public static bool CreatePlayer(string name, int money)
        {
            if (player == null)
            {
                if (name != "")
                    player = new Player(name, money);
                else
                    player = new Player(money);

                return true;
            }
            else
                return false;
        }

        public static int SetStakesMinimum(int value)
        {
            if (value < 0)
            {
                stakesMinimum = 1;
                return stakesMinimum;
            }

            else
            {
                if (value <= stakesMaximum)
                {
                    stakesMinimum = value;
                    return stakesMinimum;
                }
            }
            // Default value for failed set
            return -1;
        }

        public static int SetStakesMaximum(int value)
        {
            if (value < 0)
            {
                stakesMaximum = 1;
                return stakesMaximum;
            }

            else
            {
                if (value >= stakesMinimum)
                {
                    stakesMaximum = value;
                    return stakesMaximum;
                }
            }
            // Default value for failed set
            return -1;
        }

        private void CreateNewDeck()
        {
            deck = new Deck(numOfDecks);
        }

        private void Deal(Dealer dealer)
        {
            dealer.ReceiveCard(deck.Deal(false), 0);
            dealer.ReceiveCard(deck.Deal(true), 0);
            if (dealer.RetrieveHand(0).CheckBlackjack() == true)
            {
                PlayerLoss(player);
            }
        }

        private void Deal(Player player)
        {
            player.ReceiveCard(deck.Deal(true), 0);
            player.ReceiveCard(deck.Deal(true), 0);
        }

        private void Hit(Dealer dealer)
        {
            while (dealer.DealerLimitExceeded == false)
            {
                dealer.ReceiveCard(deck.Deal(true), 0);
            }
        }

        private bool Hit(Player player, int hand)
        {
            if (player.CheckBusted(hand) == false)
            {
                player.ReceiveCard(deck.Deal(true), 0);
                return true;
            }
            else
                return false;
        }


        private void PlayerWin(Player player)
        {
            // Reset hands
            ResetHands();

            // Pay the player the amount they bet
            player.UpdateMoney(player.CurrentBet);

            // Reset UI

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        private void PlayerLoss(Player player)
        {
            // Reset hands
            ResetHands();

            // Subtract player's current money by their bet amount
            if (player.UpdateMoney(-(player.CurrentBet)) == -1)
                // End the game
                GameOver();

            // Reset UI
        }

        /// <summary>
        /// Ends the game and prompts user to restart.
        /// </summary>
        private void GameOver()
        {

        }

        /// <summary>
        /// Resets the player hands, places used cards in the deck's used card list, and scrambles the deck.
        /// </summary>
        private void ResetHands()
        {
            Hand[] usedHands = player.DisposeHands();
            foreach (Hand hand in usedHands)
            {
                foreach (Card card in hand.Cards)
                {
                    deck.Dispose(card);
                }
            }
            usedHands = dealer.DisposeHands();
            foreach (Hand hand in usedHands)
            {
                foreach (Card card in hand.Cards)
                {
                    deck.Dispose(card);
                }
            }
            deck.Scramble();
        }

        // Hit button method
        private void HitButton(int hand)
        {
            // If a bet has been set, the player is allowed to hit
            if (player.CurrentBet > 0)
            {
                if (Hit(player, hand) == true)
                {
                    if (player.CheckBusted(hand) == true)
                    {
                        PlayerLoss(player);
                    }
                }
                else
                    PlayerLoss(player);
            }
            else
            {
                // Prompt user to bet first
            }

        }

        private void BetButton(string amount)
        {
            int bet = 0;
            if (int.TryParse(amount, out bet) == true)
            {
                if (player.Bet(bet) == -1)
                {
                    // Prompt user to change bet
                }
            }
            else
            {
                // Prompt user to change bet
            }
        }

        /// <summary>
        /// Button that finishes player actions and allows dealer to hit
        /// </summary>
        private void StandButton()
        {
            if (player.Bettable == false)
            {
                Hit(dealer);
            }
        }

        //public static void CreateMenuControls()
        //{
        //}

        private void Blackjack_MouseClick(object sender, MouseEventArgs e)
        {
            UI.Click(e);
        }

        private void Blackjack_MouseMove(object sender, MouseEventArgs e)
        {
            UI.Hover(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            UI.Draw(e);
        }
    }
}
