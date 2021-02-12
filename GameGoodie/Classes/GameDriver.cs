using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; //for Canvas object

namespace GameGoodie.Classes
{
    class GameDriver
    {
        Goodie _goodie;
        Baddie[] _baddies = new Baddie[10]; //will hold 10 bad motherfuckers.
        Canvas _playground;

        //Constructor will pass a Canvas object
        public GameDriver(Canvas playgound)
        {
            _playground = playgound;
        }
        public void NewGame()
        {
            //create new Goodie
            _goodie = new Goodie(_playground);

            //Create 10 Baddies
           CreateBaddies();

        }

        public void CreateBaddies()
        {
            Random rnd = new Random();
            int _Startleftrandom=0;
            int _StartTopRandom =0;
           // double CurrentCanvasWidht =_playground.ActualWidth;

            //create buddies in a random maner, at the upper part of the screen.
            for (int i = 0; i < _baddies.Length; i++)
            {
                _Startleftrandom = rnd.Next(1, 800); //Random left
                _StartTopRandom = rnd.Next(1, 150); //Random Top
                _baddies[i] = new Baddie(_playground, _Startleftrandom, _StartTopRandom);
            }
        }

        public void MoveGoodyUp()
        {
            _goodie.MoveUp();
        }
        public void MoveGoodyDown()
        {
            _goodie.MoveDown();
        }
        public void MoveGoodyLeft()
        {
            _goodie.MoveLeft();
        }
        public void MoveGoodyRight()
        {
            _goodie.MoveRight();
        }
        public void DestroyGoody()
        {
        }
    }
}
