using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame.Classes
{
    class Baddie:Creature
    {

        int _startTop = 300;
        int _startLeft = 200;
        int _size = 50;
        int _id = 0;
      
        
        public Baddie(Canvas playground, int startLeft, int startTop, int id) : base("ms-appx:///Assets/Baddie.png")
        {
            //local members
            _startLeft = startLeft;
            _startTop = startTop;
            _id = id;

            //Base members
            _BaseCanvasplayground = playground;
            _Baseimg = new Image();
            _Baseimg.Source = new BitmapImage(new Uri(_BaseSrc));
            _Baseimg.Height = _size;
            _Baseimg.Width = _size;

            //Set The Baddie coordinates left-Top
            Canvas.SetLeft(_Baseimg, _startLeft);
            Canvas.SetTop(_Baseimg, _startTop);

            //Put the Baddie into the Canvas as a child
            _BaseCanvasplayground.Children.Add(_Baseimg);

        }

        public int GetId()
        {
            return this._id;
        }

        public void killBaddie()
        {
            _BaseCanvasplayground.Children.Remove(_Baseimg); //Renoves ONLY a graphical representation(the object will be removed in GameDriver class)
        }
    }
}
