using DodgeGame.Classes;
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

namespace DodgeGame
{
    public sealed partial class MainPage : Page
    {
        GameDriver _GameDriver; //Public GameDriver Object
        private bool _IsGameLoaded { get; }
        private bool _IsPaused;

        MusicManager _musicManager = new MusicManager();
    

        //Constructor-MainPage
        public MainPage()
        {
            this.InitializeComponent();
            _GameDriver = new GameDriver(CanvasPlayingArea,btnPauseGame,btnResumeGame,btnStartNewgame, btnSaveFile, btnLoadFile, _IsGameLoaded, GaneOverContentDialog, YouWinContentDialog, PauseContentDialog); //Initialize GameDriver and pass OUR Canvas, and BUTTONS to it so we can use them in GameDriver.cs
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;   //Register the KeyDown Event with a CoreWindow_KeyDown Method
            SetButtonsToNewGameState(); //Start button state   
        }

        //What key was pressed
        //This method fires evetytime you press button
        private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
                switch (args.VirtualKey)
                {
                    //Invoke corresponding Method In GameDriver class
                    case VirtualKey.Up:                                //UP Allowed Only if Game is running
                    if (_GameDriver.IsGameRunning) {
                        _GameDriver.MoveGoodieUp();
                    }  
                        break;

                    case VirtualKey.Down:                             //Down Allowed Only if Game is running
                    if (_GameDriver.IsGameRunning==true) {
                        _GameDriver.MoveGoodieDown();
                    }

                        break;
                    case VirtualKey.Left:                             //Left Allowed Only if Game is running
                    if (_GameDriver.IsGameRunning ==true){
                        _GameDriver.MoveGoodieLeft();
                    }
                        break;
                    case VirtualKey.Right:                           // Right Allowed Only if Game is running
                    if (_GameDriver.IsGameRunning ==true) {
                        _GameDriver.MoveGoodieRight();
                    }
                        break;
                    case VirtualKey.Space:                           //Space Bar Allowed only if game is running
                    if (_GameDriver.IsGameRunning ==true) {
                        _GameDriver.PutGoodieAtRandomPlace(); //forgotten feature implementation
                    }
                        break;
                    case VirtualKey.P:                               //Pause Toggle On/off logic  implemented in  _GameDriver.PauseGame()method (GameDriver.cs)
                    _GameDriver.PauseGame(); //New Feature
                        break;
                }
            
         
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (_IsGameLoaded)
            {
                FreezeGoodie(); //reset the eventhandler just in case
                UnFreezeGoodie();
            }
            else if(_IsPaused)
            {
                UnFreezeGoodie();
            }
           
            _GameDriver.NewGame();   //Invoke NewGame() when pressing the Start new game button
            FreezeGoodie();          //---------Unregister, and register the KeyDown, to pevent unnecessary  Goodie Speed
            UnFreezeGoodie();
            SetButtonsToPlayState(); //Set buttons to play state
        }

        private void PauseGameButton_Click(object sender, RoutedEventArgs e)
        {
            _IsPaused = true;
            _GameDriver.StopTimer();                         //Stop The movement of all the baddies on the scree
            FreezeGoodie();
            SetButtonsToPauseState();
        }

        //will not be implemented
        private void Page_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        private void ResumeGame(object sender, RoutedEventArgs e)
        {
             UnFreezeGoodie(); //Register the KeyDown Event before continue
            _GameDriver.ResumeGame();                              //register keyDown event before proceed
            btnResumeGame.Visibility = Visibility.Collapsed; //hide the Resume button
            btnPauseGame.Visibility = Visibility.Visible;    //Make Pause Button visible to be able to stop the game
        }

        private void SetButtonsToNewGameState()
        {
            btnResumeGame.Visibility = Visibility.Collapsed; //No Resume
            btnPauseGame.Visibility = Visibility.Collapsed;  //No pause
            btnLoadFile.Visibility = Visibility.Visible;     //Yes load button
            btnSaveFile.Visibility = Visibility.Collapsed;   //No Save
        } //Relevant only to a new game state (first time started)
        private void SetButtonsToPlayState()    //While Playing(not veryfirst time)
        {
                btnStartNewgame.Visibility = Visibility.Collapsed;  //No new Game(while Playing)
                btnResumeGame.Visibility = Visibility.Collapsed;    //No Resume
                btnPauseGame.Visibility = Visibility.Visible;       //Yes Pause 
                btnLoadFile.Visibility = Visibility.Visible;        //yes Load

        }
        private void SetButtonsToPauseState()
        {
            btnStartNewgame.Visibility = Visibility.Visible; // Yes New Game
            btnResumeGame.Visibility = Visibility.Visible;   // Yes Resume
            btnPauseGame.Visibility = Visibility.Collapsed;  // NO Pause
            btnSaveFile.Visibility = Visibility.Visible;     // Yes Save
            btnLoadFile.Visibility = Visibility.Visible;     // Yes Load
        }

        //Register Unregister KeyDown event
        private void FreezeGoodie()
        {
            //Unregister the CoreWindow_KeyDown event while goodie on Pause
            //This will prevent goodie to move
            Window.Current.CoreWindow.KeyDown -= CoreWindow_KeyDown;
        }
        private void UnFreezeGoodie()
        {
            //Register the CoreWindow_KeyDown event while goodie on Pause
            //This will prevent goodie to move
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
        }

        //Save + load methods
        private void btnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            _GameDriver.SaveFile();
        }
        private void btnLoadFile_Click(object sender, RoutedEventArgs e)
        {
            _GameDriver.LoadFile();

                //IMPORTANT
                //Everytime you register CoreWindow_KeyDown event
                //First remove it, then register it again.
                //I do this to be sure a multiplied CoreWindow_KeyDown event IS NOT APPLIEED  2 times
                //because it will increase Goodie's speed. (We don't want to increase goodie's speed)
                FreezeGoodie(); 
                UnFreezeGoodie(); 
        }

        private async void ShowIntro()
        {
          await  NewGameContentDialogWindow.ShowAsync();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ShowIntro();
           await _musicManager.PlayIntroSound();
            
        }
    }
}
