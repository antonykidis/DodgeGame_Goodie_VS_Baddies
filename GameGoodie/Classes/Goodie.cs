using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // for Canvas
using Windows.UI.Xaml.Media.Imaging; //for BitMapImage

namespace DodgeGame.Classes
{
    class Goodie:Creature
    {
        //This is a Goodie start coordinates
        int _startTop = 500;
        int _startLeft = 300;
        int _size = 50;

        //Constructor1 Passing path to base's srs
        public Goodie(Canvas _playground) : base("ms-appx:///Assets/Goodie.png")
        {        
            _BaseCanvasplayground = _playground;                  //Pass the Playground Canvas object to Base canvas object
            _Baseimg = new Image();                               //Create a new image within a base class
            _Baseimg.Source = new BitmapImage(new Uri(_BaseSrc)); //Extracting the path from the base class, and put it into new image's source
           
            //Apply the size to our new Goodie
            _Baseimg.Height = _size;
            _Baseimg.Width = _size;

            //Set the x,y position of the goodie
            Canvas.SetLeft(_Baseimg, _startLeft);
            Canvas.SetTop(_Baseimg, _startTop);

            //Adding Goodie to The Canvas
            _BaseCanvasplayground.Children.Add(_Baseimg);
        }

        //Constructor2
        public Goodie(Canvas _playground, int startTop, int startLeft) : base("ms-appx:///Assets/Goodie.png")
        {
            _BaseCanvasplayground = _playground;                  //Pass the Playground Canvas object to Base canvas object
            _Baseimg = new Image();                               //Create a new image within a base class
            _Baseimg.Source = new BitmapImage(new Uri(_BaseSrc)); //Extracting the path from the base class, and put it into new image's source

            //Apply the size to our new Goodie
            _Baseimg.Height = _size;
            _Baseimg.Width = _size;

            //Set the x,y position of the goodie
            Canvas.SetLeft(_Baseimg, startLeft);
            Canvas.SetTop(_Baseimg, startTop);

            //Adding Goodie to The Canvas
            _BaseCanvasplayground.Children.Add(_Baseimg);
        }


        //Kill Goodie
        protected void KillGoodie()
        {
            _BaseCanvasplayground.Children.Remove(_Baseimg);
        }

        protected double GetGoodiePosition()
        {
            Double positionTop = Canvas.GetTop(this.BaseImage);
             return positionTop;
        }

        public void SetGoodiePosition(int startTop, int startLeft)
        {
            _startTop = startTop;
            _startLeft = startLeft;

        }
    }
}
