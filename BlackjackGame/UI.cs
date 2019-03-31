/*
 * Felix Liu
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using _2_Assignment_Blackjack.GUI;

namespace _2_Assignment_Blackjack
{
    /// <summary>
    /// Enumeration that defines type/state of the UI.
    /// </summary>
    public enum UIState { Menu, Play, Win, Loss, GameOver }

    
    /// <summary>
    /// Controls and provides methods for graphics drawing and the user interface.
    /// </summary>
    static class UI
    {
        /// <summary>
        /// Enumeration that defines widgets used in the game's play area or settings. Values correspond to indices of the "widgets" array in class UI.
        /// </summary>
        private enum UIWidgets
        {
            Background, StartContainer, Start, SaveContainer, LoadContainer, Load, StakesMin, /*StakesMinInc, StakesMinDec,*/ StakesMax, /*StakesMaxInc, StakesMaxDec,*/ StartMoney, /*StartMoneyInc, StartMoneyDec,*/ Save, DialogBox, Dealer, Player, CurrentMoney,
            CurrentBet, SetBetContainer, SetBet, ResetBet, BetFive, BetTen, Bet25, Bet100, BetMax, StandContainer, Stand, HitContainer, Hit, DoubleDownContainer, DoubleDown, SplitContainer, Split, RestartContainer, Restart, 
            NumOfWidgets // Integer value of this enumeration defines num of widgets
        }
        private const int NUM_OF_MENU_WIDGETS = 9;
        private const int NUM_OF_GAME_WIDGETS = 21;
        private static UIWidget[] widgets;
        private static UIState state;
        public static UIState State
        {
            get { return state; }
        }

        public static void SetState(UIState uiState)
        {
            state = uiState;
            switch (uiState)
            {
                case UIState.Menu:
                    InitializeMenu();
                    break;
                case UIState.Play:
                    break;
                case UIState.Win:
                    break;
                case UIState.Loss:
                    break;
                case UIState.GameOver:
                    break;
                default:
                    break;
            }
        }

        private static void InitializeMenu()
        {
            widgets = new UIWidget[(int)UIWidgets.NumOfWidgets];
            widgets[(int)UIWidgets.Background] = new UIContainerBox(Blackjack.DisplaySize, new Point(0, 0), Brushes.Green, true);
            widgets[(int)UIWidgets.Start] = new UITextBox(new Point(100, Blackjack.DisplaySize.Height / 2 - 50), 
                "Start", new Font("Trebuchet MS", 50), Color.White, true);
            widgets[(int)UIWidgets.StartContainer] = new UIContainerBox(new Size(170, 90), widgets[(int)UIWidgets.Start].Rectangle.Location, Brushes.Black, true);
            widgets[(int)UIWidgets.StartMoney] = new UITextBox(new Point(widgets[(int)UIWidgets.StartContainer].Rectangle.Right + 50, widgets[(int)UIWidgets.StartContainer].Rectangle.Y),
                "Starting Money: " + Blackjack.StartingMoney, new Font("Trebuchet MS", 24), Color.White, true);
            //widgets[(int)UIWidgets.StartMoneyDec] = new UIButton(new Point(widgets[(int)UIWidgets.StartMoney].Rectangle.Left, widgets[(int)UIWidgets.StartMoney].Rectangle.Bottom + 40),
            //    "- 1", new Font("Trebuchet MS", 24), Color.Black, Color.LightGray, Color.White, Color.Black, true);
            //widgets[(int)UIWidgets.StartMoneyInc] = new UIButton(new Point(widgets[(int)UIWidgets.StartMoneyDec].Rectangle.Right + 100, widgets[(int)UIWidgets.StartMoneyDec].Rectangle.Y),
            //    "+ 1", new Font("Trebuchet MS", 24), Color.Black, Color.LightGray, Color.White, Color.Black, true);

            widgets[(int)UIWidgets.StakesMin] = new UITextBox(new Point(widgets[(int)UIWidgets.StartContainer].Rectangle.Right + 50, widgets[(int)UIWidgets.StartMoney].Rectangle.Bottom + 50),
                "Minimum Stakes: " + Blackjack.StakesMinimum, new Font("Trebuchet MS", 24), Color.White, true);
            //widgets[(int)UIWidgets.StakesMinDec] = new UIButton(new Point(widgets[(int)UIWidgets.StakesMin].Rectangle.Left, widgets[(int)UIWidgets.StakesMin].Rectangle.Bottom + 40),
            //    "- 1", new Font("Trebuchet MS", 24), Color.Black, Color.LightGray, Color.White, Color.Black, true);
            //widgets[(int)UIWidgets.StakesMinInc] = new UIButton(new Point(widgets[(int)UIWidgets.StakesMinDec].Rectangle.Right + 100, widgets[(int)UIWidgets.StakesMinDec].Rectangle.Y),
            //    "+ 1", new Font("Trebuchet MS", 24), Color.Black, Color.LightGray, Color.White, Color.Black, true);

            widgets[(int)UIWidgets.StakesMax] = new UITextBox(new Point(widgets[(int)UIWidgets.StartContainer].Rectangle.Right + 50, widgets[(int)UIWidgets.StakesMin].Rectangle.Bottom + 50),
               "Maximum Stakes: " + Blackjack.StakesMaximum, new Font("Trebuchet MS", 24), Color.White, true);
            
            //widgets[(int)UIWidgets.StakesMaxDec] = new UIButton(new Point(widgets[(int)UIWidgets.StakesMax].Rectangle.Left, widgets[(int)UIWidgets.StakesMax].Rectangle.Bottom + 40),
             //   "- 1", new Font("Trebuchet MS", 24), Color.Black, Color.LightGray, Color.White, Color.Black, true);
            //widgets[(int)UIWidgets.StakesMaxInc] = new UIButton(new Point(widgets[(int)UIWidgets.StakesMaxDec].Rectangle.Right + 100, widgets[(int)UIWidgets.StakesMaxDec].Rectangle.Y),
             //   "+ 1", new Font("Trebuchet MS", 24), Color.Black, Color.LightGray, Color.White, Color.Black, true);
            //widgets[(int)UIWidgets.StartMoney]
            //widgets[(int)UIWidgets.StakesMin]
            //widgets[(int)UIWidgets.StakesMax]
            //widgets[(int)UIWidgets.Load]
            //Blackjack.CreateMenuControls();
        }

        public static void Draw(PaintEventArgs e)
        {
            foreach (UIWidget widget in widgets)
            {
                if (widget != null)
                {
                    if (state == UIState.Menu)
                    {
                        
                    }
                    widget.Paint(e);
                }
            }
        }

        public static void Click(MouseEventArgs e)
        {
            foreach (UIWidget widget in widgets)
            {
                if (widget != null)
                {
                    widget.CheckClick(e);
                }
            }
            ProcessClicks();
        }

        public static void Hover(MouseEventArgs e)
        {
            foreach (UIWidget widget in widgets)
            {
                if (widget != null)
                {
                    widget.CheckHover(e);
                }
            }
            Refresh();
        }

        private static void ProcessClicks()
        {
            if (state == UIState.Menu)
            {
                if (widgets[(int)UIWidgets.Start].IsClicked == true)
                {
                    state = UIState.Play;
                    Blackjack.CreatePlayer(null, Blackjack.StartingMoney);
                    Blackjack.CreateDealer(null);

                    //InitializeGameArea();
                }
            }
            Refresh();
        }

        public static void Refresh()
        {
            Blackjack.ActiveForm.Refresh();
        }
    }
}
