using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace GameGoodie.Classes
{
    class Creature //Base Class
    {
        //local members
        protected string _BaseSrc;
        protected Canvas _BaseCanvasplayground;
        protected Image _Baseimg;
        private int _Basestep = 10;

        //Constructor
        //Will recieve the Path to the actual Goodie,or Baddie image
        //Goodie is a Creature
        //Badie Is A Creature
        public Creature(string src)
        {
            _BaseSrc = src;
        }

        //implementing the basic movement of the goodie on the Canvas element
        //Each Creature can move to 4 sides
        public void MoveUp()
        {
            if (Canvas.GetTop(_Baseimg) <= _Basestep)
                return;
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg));
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg) - _Basestep);
        }
        public void MoveDown()
        {
            if (Canvas.GetTop(_Baseimg) + _Basestep + _Baseimg.Height > _BaseCanvasplayground.ActualHeight)
                return;
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg));
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg) + _Basestep);
        }
        public void MoveLeft()
        {
            if (Canvas.GetLeft(_Baseimg) <= _Basestep)
                return;
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg) - _Basestep);
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg));
        }
        public void MoveRight()
        {
            if (Canvas.GetLeft(_Baseimg) + _Basestep + _Baseimg.Width > _BaseCanvasplayground.ActualWidth)
                return;
            Canvas.SetLeft(_Baseimg, Canvas.GetLeft(_Baseimg) + _Basestep);
            Canvas.SetTop(_Baseimg, Canvas.GetTop(_Baseimg));
        }
    }

}
