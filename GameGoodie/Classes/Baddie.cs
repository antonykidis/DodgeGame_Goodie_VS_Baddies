using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace GameGoodie.Classes
{
    class Baddie:Creature
    {

        int _startTop = 300;
        int _startLeft = 200;
        int _size = 50;
        
        /// <summary>
        /// 1.This Constructor will created Baddie object
        /// 2.Will set its coordinates based on StartLeft/StartTop
        /// 3.will Grab the baddie image source and apply it to the baddie object.
        /// 4.Will Set the baddies coordinates.
        /// 5.Finally a Baddie object will be added to the playground canvas object as its child.
        /// </summary>
        /// <param name="playground"></param>
        /// <param name="startLeft"></param>
        /// <param name="startTop"></param>
        public Baddie(Canvas playground, int startLeft, int startTop) : base("ms-appx:///Assets/Baddie.png")
        {
            _startLeft = startLeft;
            _startTop = startTop;

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
    }
}
