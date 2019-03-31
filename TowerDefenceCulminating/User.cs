/*
 * Felix Liu
 * January 21st, 2017
 * 
 * Contains methods and properties about the game's current user. Stores values such as money, lives, and waves, and provides methods to
 * modify, get, or compare these values.
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BosenQuFelixLiuFinalCulminatingProject.GUI;

namespace BosenQuFelixLiuFinalCulminatingProject
{
    /// <summary>
    /// Provides methods and properties to store, get, or compare user statistics or data.
    /// </summary>
    class User
    {
        // Properties and fields

        // Constants for the user's starting money and lives on game start.
        const int USER_START_MONEY = 150;
        const int USER_START_LIVES = 10;
        // Integer properties for the user's money, lives, and number of waves survived. These properties have private
        // setters to allow modification only within the object or using specialized methods.
        public int Money { get; private set; }
        public int Lives { get; private set; }
        public int WavesSurvived { get; private set; }
        // Private boolean that stores whether or not the user is alive.
        private bool isAlive = true;

        // Constructor for user class
        public User()
        {
            // Waves survived starts at 0, set the money and lives to the default starting value.
            WavesSurvived = 0;
            Money = USER_START_MONEY;
            Lives = USER_START_LIVES;
        }

        // Method with no parameters or return values that decreases the user's lives by 1
        public void LivesLoss()
        {
            // Reduce lives by 1
            Lives--; ;
            // If the user's lives are 0 or less, set the user as dead
            if (Lives <= 0)
                isAlive = false;
        }

        // Method with no parameters or return values that increases the user's money by a given amount, integer parameter accomplishes this
        public void MoneyGain(int moneyGain)
        {
            // Add parameter to money if the value is not 0
            if (moneyGain != 0)
                Money += moneyGain;
        }

        // Method with no parameters or return values that decreases the user's money by the integer parameter
        public void MoneyLoss(int moneyLoss)
        {
            // Decrease money by parameter if the value is not 0
            if (moneyLoss != 0)
                Money -= moneyLoss;
        }

        // Method with no return values, increases the number of waves the user survived
        public void NextWave()
        {
            WavesSurvived++;
        }

        // Method returning a bool of whether or not an item is purchasable, needs the integer price value as a parameter
        public bool Buyable(int price)
        {
            // Return value is whether or not the money is greater or equal to the price (true if true etc.)
            return (Money >= price) ? true : false;
        }

        // Method returns whether or not the user is alive, boolean return value
        public bool Alive()
        {
            // If the isAlive is true, return true etc.
            return (isAlive) ? true : false;
        }
    }
}
