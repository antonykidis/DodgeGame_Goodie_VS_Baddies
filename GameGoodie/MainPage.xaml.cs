using GameGoodie.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace GameGoodie
{
   
    public sealed partial class MainPage : Page
    {
        GameDriver _GameDriver; //Public GameDriver Object

        public MainPage()
        {
            this.InitializeComponent();

            //Initialize GameDriver and pass OUR canvas to it
            _GameDriver = new GameDriver(CanvasPlayingArea);

            //Register the KeyDown Event with a method
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            //Manipulate the Goodie movements in this Method
            //Decide what was pressed and behave accordingly.
            switch (args.VirtualKey)
            {
                    //Invoke corresponding Method
                    //In GameDriver class
                case VirtualKey.Up: //Using Microsoft.System (Don't forget)
                    _GameDriver.MoveGoodyUp();
                    break;
                case VirtualKey.Down:
                    _GameDriver.MoveGoodyDown();
                    break;
                case VirtualKey.Left:
                    _GameDriver.MoveGoodyLeft();
                    break;
                case VirtualKey.Right:
                    _GameDriver.MoveGoodyRight();
                    break;
            }

        }


        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            //Invoke NewGame() 
            //when pressing the Start new game button
            _GameDriver.NewGame();
        }

        private void PauseGameButton_Click(object sender, RoutedEventArgs e)
        {
            //Stop The movement of all the baddies on the screen
            //Stop the Timer(My guess)
        }
        
        //will not be implemented
        private void Page_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

    }
}
