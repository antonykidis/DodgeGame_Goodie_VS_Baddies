using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls; // for Canvas
using Windows.UI.Xaml.Media.Imaging; //for BitMapImage

namespace GameGoodie.Classes
{
    class Goodie:Creature
    {
        //This is a Goodie start coordinates
        int _startTop = 500;
        int _startLeft = 300;
        int _size = 50;

        /// <summary>
        /// 1.This Constructor will Create a new image of type Goodie
        /// 2.Then the new Goodie image will be added to a Canvas Object
        /// 3.The Canvas object named "CanvasPlayingArea"located in Mainpage.xaml
        /// </summary>
        /// <param name="_playground"></param>

        //Passing Our canvas to consrructor      //Passing path to base's srs
        public Goodie(Canvas _playground) : base("ms-appx:///Assets/Goodie.jpg")
        {        
            _BaseCanvasplayground = _playground;         //Pass the Playground Canvas object to Base canvas object
            _Baseimg = new Image();                      //Create a new image within a base class
            _Baseimg.Source = new BitmapImage(new Uri(_BaseSrc)); //Extracting the path from the base class, and put it into new image's source
           
            //Apply the size to our new Goodie
            _Baseimg.Height = _size;
            _Baseimg.Width = _size;

           //Position the newly created image on the screen
           //Using Canvas SetLeft/SetTop() methods.
            Canvas.SetLeft(_Baseimg, _startLeft);
            Canvas.SetTop(_Baseimg, _startTop);


            //Adding a newly created image to our CanvasPlayingArea object (SeeMainPage.xaml)
            //Adding it  as a Canvas children object.
            _BaseCanvasplayground.Children.Add(_Baseimg);
        }

      
    }
}
