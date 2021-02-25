using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Provider;
using Windows.Storage.Search;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls; //for Canvas object
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame.Classes
{
    public  class GameDriver
    {
        #region MembersOfGameDriver
        // Class members
        Goodie _goodie;
        List<Baddie> _baddies = new List<Baddie>();            //Holds the amount of baddies
        Canvas _playgroundCanvas;                              //Holds the actual Canvas(MainPage) with the Player and Enemies
        private DispatcherTimer _tmr = new DispatcherTimer();  //The timer that will move the Enemy
        private DispatcherTimer _tmrExplosion = new DispatcherTimer(); //Timer That removes Explosion Images after 800 milliseconds
        Random _rnd = new Random();                             //Will randomize the Baddies on the screen

        private AppBarButton _btnStartNewGame;                 //Buttons that will be binded to MainPage's buttons through GameDrive constructor
        private AppBarButton _btnResumeGame;                   //btn Resume
        private AppBarButton _btnPause;                        //btn Pause
        private AppBarButton _btnSaveFile;                     //btn Save
        private AppBarButton _btnLoadFile;                     //btn Load
        private List<SaveDataSchema> _savedDataList;           //Hold the saved data As a simplified object of type SaveDataSchema

        private bool IsGameLoad = false;
        private bool IsSaved;
        private bool IsGameLose;
        private bool IsGameWin;
        private bool IsGameRunning;
        public bool isGameisLoaded { get; set; }
        public bool IsloadedMorethanOnce;

        private int loadedTimes = 0;
        private int _goodieTop;                                  //Holds CanvasGetTop(needed for Load game)
        private int _goodieLeft;                                 //Holds CanvasGetLeft(needed for load game)
        private int _baddieTopExplosionCoordinates;              //Holds Coordinates for Exoplosion image
        private int _baddieLeftExplosionCoordinates;             //Holds Coordinates for Exoplosion image
        List<Image> _ExplosionImgList = new List<Image>();       //Holds the List of ExplosionImages
       
        public DispatcherTimer TimerOfTheGame
        {
            get { return _tmr; }
        }                 //timer ptoperties to get from MainPage.cs
        private MusicManager _musicManager = new MusicManager(); //Music manager(We will manage our music through this object)
        Image _explosionImage = new Image();
        ContentDialog _contentDialogLose = new ContentDialog();      //Holds the custom content dialog for LOSE game
        ContentDialog _contentDialogWin = new ContentDialog();     //Holds the custom content dialog for WIN game

        #endregion Members

        //GameDriver Constructor
        //By Passing 3 Buttons as a parameters to this GameBoard constructor,
        //we ensure that THIS class will be connected with the MainPage's Physical buttons.
        public GameDriver(Canvas playgound, AppBarButton pauseGameBtn, AppBarButton resumeGameBtn, AppBarButton startNewGameBtn, AppBarButton saveFileBtn, AppBarButton loadFileBtn, bool isGameisLoaded, ContentDialog contentDialogLose, ContentDialog contentDialogWin)
        {  
            //Game timer
            _tmr.Interval = new TimeSpan(0, 0, 0, 0, 150); //Set 150 milliseconds
            _tmr.Tick += OnTickHandler;

            //Explosion timer
            _tmrExplosion.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            _tmrExplosion.Tick += OnTickHandlerExplosion;

            //Here we simply Bind the Main Page's  control to this class so we can manipulate them from here.
            this._playgroundCanvas = playgound;                 
            this._btnPause = pauseGameBtn;
            this._btnResumeGame = resumeGameBtn;
            this._btnStartNewGame = startNewGameBtn;
            this._btnSaveFile = saveFileBtn;
            this._btnLoadFile = loadFileBtn;
            this.isGameisLoaded = isGameisLoaded;
            this._contentDialogLose = contentDialogLose; //Passes the custom content dialog window in game over
            this._contentDialogWin = contentDialogWin; //Passes the custom content dialog for Win
        }

        private void OnTickHandlerExplosion(object sender, object e)
        {
            foreach (var item in _ExplosionImgList)
            {
                //Set the x,y position of the Explosion
                Canvas.SetLeft(item, _baddieLeftExplosionCoordinates);
                Canvas.SetTop(item, _baddieTopExplosionCoordinates);
                _playgroundCanvas.Children.Remove(item);
            }
          
            //RemoveExplosionFromCanvas();
        }

        //Create a new Game.....................................
        public async void NewGame()
        {
            _musicManager.StopBgMusic();                   //Stop the music if its already playing(otherwise the music will overlap with the old one)
           await _musicManager.PlayBackgroundMusicSound(); //Play BGSound
            CreateBoard();
        }

        //MAIN ANIMATION METHOD =>>>>>> Enemy Starts Mooving - STARTS ON Pressing the Start Button
        //Fires EACH 150 miliseconds 
        private void OnTickHandler(object sender, object e) {

            ChaseAfterGoodie(_baddies); //baddies must be assigned before passing to the loop
            BaddieCollision();          //Checks Baddie-baddie collision
            GoodieCollision();          //Checks Baddie-Goodie collision
            CheckWin();                 //Checks Win Condition

           
        }

        #region CustomMethods
        //custom methods.........................................
        internal void ResumeGame()
        {
              //Resume game
             _musicManager.ResumePlayingBGMusic(); //Unpause the music
            _tmr.Start();
        }
        internal void StopTimer()
        {
            _musicManager.PauseBgMusic();
            _tmr.Stop(); //stop Music
        }

        public void CreateBaddies()
        {
            Random rnd = new Random();
            int _Startleftrandom = 0;
            int _StartTopRandom = 0;
            int _BaddieID = 0;       //Each Baddie has an ID
            //create baddies in a random maner, at the upper part of the screen.
            for (int i = 0; i < 10; i++)
            {
                _Startleftrandom = rnd.Next(-100, 1400); //Random left
                _StartTopRandom = rnd.Next(1, 150); //Random Top
                _baddies.Add(new Baddie(_playgroundCanvas, _Startleftrandom, _StartTopRandom, _BaddieID));
                _BaddieID++; //icrement the id for each new baddie.
            }
        } //Baddies NUST be created BEFORE EVERYTHING
        public void CreateBaddies2()
        {
            int GoodieTop=0;
            int GoodieLeft=0;
            for (int i = 0; i < _savedDataList.Count; i++)
            {
                _baddies.Add(
                new Baddie(_playgroundCanvas, _savedDataList[i]._BaddieStartLeft, _savedDataList[i]._BaddieStartTop, _savedDataList[i]._IdBaddie));
                GoodieTop = _savedDataList[i]._GoodieStartTop;
                GoodieLeft = _savedDataList[i]._GoodieStartLeft; 
            }
            //_goodie.SetGoodiePosition(GoodieTop, GoodieLeft);
            _goodieTop = GoodieTop;
            _goodieLeft = GoodieLeft;
           
        } //Baddies Created on Game LOAD

        private void ChaseAfterGoodie(List<Baddie> baddies)
        {
            for (int i = 0; i < baddies.Count; i++)
            {
                int randBaddie = _rnd.Next(30);
                if (_goodie.GetTop() < _baddies[i].GetTop()) //check top
                {
                    if (randBaddie != 0)
                    {
                        _baddies[i].MoveUp();
                    }
                }
                else if (_goodie.GetTop() > _baddies[i].GetTop())
                {
                    if (randBaddie != 0)
                    {
                        _baddies[i].MoveDown();
                    }
                }
                if (_goodie.GetLeft() > baddies[i].GetLeft())
                {
                    if (randBaddie != 0)
                    {
                        _baddies[i].MoveRight();
                    }
                }
                else if (_goodie.GetLeft() < _baddies[i].GetLeft())
                {
                    if (randBaddie != 0)
                    {
                        _baddies[i].MoveLeft();
                    }
                }
            }
        }

        internal async void LoadFile()
        {
           
            List<SaveDataSchema> _data = new List<SaveDataSchema>();
            Windows.Storage.StorageFolder storageFolder =  Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("myconfig.json");

            //Reading Saved Data into String
            string json_str = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
            //Deserialize json and put it into a List
            _data = JsonConvert.DeserializeObject<List<SaveDataSchema>>(json_str);
            _savedDataList = _data; //passing data to a global variable. (This list will hold all of the Deserialized Baddies)

            //Clear board BEFORE PROCEEDING
            ClearBoard();
            IsGameLoad = true;
            CreateBoard();
            SetButtonsOnLoad();
            _musicManager.PauseBgMusic();
            await   _musicManager.PlayBackgroundMusicSound();

            isGameisLoaded = true;//-----------------------Set this flag to false to enable New keyEvent in Mainpage after load(otherwise goodie will freeze)
           
            loadedTimes++;
            if (loadedTimes > 1)
            {
                IsloadedMorethanOnce = true;
            }
        }

        internal async void SaveFile()
        {
            //SaveData class it is a DataScheme-DataModel
            List<SaveDataSchema> _simpleBaddiesDataList = new List<SaveDataSchema>(); //List to hold each Baddie X,Y, and ID
           
            #region Explanation Of Schema Object
            //About the trick......................................................
            //_simpleBaddiesDataList will hold the simplified version of _baddies list.

            //Yes we could pass _baddies List straight to JsonConvert.SerializeObject() method, but this will
            //cause a memory leak (Stack overflow) I guess because the JsonConvert.SerializeObject() method, cannot parse
            //compex collections such as (_baddies) list.        I had to find a solution....

            //That is why I came with an Idea of creating a  SIMPLIFIED REPRESENTATION of _baddies.
            // Which is a List<SaveDataSchema> _data, that can be Easily parsed by JsonConvert.SerializeObject() method
            //Without causing a memory leak.
            #endregion  Explanation Of Schema Object

            foreach (var CurrentBaddie in _baddies) //Loop through the "Complicated" _baddies List
            {
                //Map each _baddies' coordinates to new _simpleBaddiesDataList
                //Each new Object will hold the coordinates of _beddies
                //THIS WILL CREATE A LIST OF SIMPLE OBJECTS that contains each baddie's coordinates.
                _simpleBaddiesDataList.Add(new SaveDataSchema((int)CurrentBaddie.GetTop(),
                                                               (int)CurrentBaddie.GetLeft(),
                                                                     CurrentBaddie.GetId(),
                                                               (int)this._goodie.GetTop(),
                                                               (int)this._goodie.GetLeft()));
                try
                {
                    //Finally we can Pass a List of Simple Objects to JsonConvert.SerializeObject() method.
                    // Serialize _data to JSON strinng.
                    string json = JsonConvert.SerializeObject(_simpleBaddiesDataList.ToArray());
                    // write string to a file
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myconfig.json");
                    await FileIO.WriteTextAsync(file, json);
                    IsSaved = true;

                }
                catch (Exception e)
                {
                    //If file Exists simply delete the previous myconfig.json------------DELETE FILE
                    Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                    Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("myconfig.json");

                    string name = "myconfig.json";
                    StorageFile manifestFile = await storageFolder.GetFileAsync(name);
                    await manifestFile.DeleteAsync();
                    IsSaved = false;

                    //Than immediately save a new one------------------------------------SAVE FILE
                    // Serialize _data to JSON string
                    string json = JsonConvert.SerializeObject(_simpleBaddiesDataList.ToArray());
                    // write string to a file
                    var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("myconfig.json");
                    await FileIO.WriteTextAsync(file, json);
                    IsSaved = true;

                }
            }
        } 


        private async void GoodieCollision()
        {
            double Xtop = _goodie.GetTop();
            double YLeft = _goodie.GetLeft();
            double size = 50;

            for (int i = 0; i < _baddies.Count; i++)
            {
                if ((_goodie.GetTop() > _baddies[i].GetTop() - size && _goodie.GetTop() < _baddies[i].GetTop() + size)
                    && _goodie.GetLeft() > _baddies[i].GetLeft() - size && _goodie.GetLeft() < _baddies[i].GetLeft() + size)
                {
                    _tmr.Stop();
                  await _musicManager.PlayGoodieIsDeadSound();
                    GameOverLoose();
                }
            }
        }
        private async void BaddieCollision()
        {
            try
            {
                for (int i = 0; i < _baddies.Count; i++)
                {
                    //loop through the baddies array
                    for (int j = 0; j < _baddies.Count; j++)  //don't  check i against j
                    {
                        //Differenciate each baddie to other Baddie
                        if (i != j && _baddies[i].GetTop() > _baddies[j].GetTop() - 50
                                   && _baddies[i].GetTop() < _baddies[j].GetTop() + 50
                                   && _baddies[i].GetLeft() > _baddies[j].GetLeft() - 50
                                   && _baddies[i].GetLeft() < _baddies[j].GetLeft() + 50)
                        {

                            _baddieTopExplosionCoordinates = (int)_baddies[i].GetTop();   //Passing coordinates of killed Baddie_Top
                            _baddieLeftExplosionCoordinates = (int)_baddies[i].GetLeft(); //Passing coordinates of killed Baddie_Left

                            //If true Kill The Beddie
                            _baddies[i].Destroy();                //Destroys graphical representation
                            _baddies.RemoveAt(i);                 //Remove the current killed Baddie at the specified index of List<Baddie>
                            await _musicManager.PlayCollisionSound();

                            DisplayExplosionOnCollision();       //Show Explosion Image in these coordinates
                        }
                    }
                }
            }
            catch (Exception)
            {
                //Sometimes the above piece of code gives exception regarding Index out of the scope
                //try catch was the easy fix. But i still wonder why it happens. It needs to be explored.
                //I think it is due to incorrect randomization of the baddies(they get invalid coordinates, or the List of baddies populated to slow)
                //ANYWAYS IF above code "fucked-up" we just try to start From scratch. Preventing System crash.
                NewGame();
            }  
        }

        private void CreateBoard()
        {
            _tmr.Stop();                      //Be sure Timer is stopped before continue.
            _tmrExplosion.Stop();
            ClearBoard();                     //Clears board just in case it is not clear.

            //Fresh Start.....................................................................
            _goodie = new Goodie(_playgroundCanvas);  //create new Goodie

            if (IsGameLoad == true)                                               //If game is loaded Create Baddies based on saved data.
            {
                CreateBaddies2();                                                 //Create baddies based on saved data.
                _goodie.Destroy();                                                //Destroy previous goodie.
                _goodie = new Goodie(_playgroundCanvas, _goodieTop, _goodieLeft); //Start goddie based on saved data.
                _tmr.Start();                                                     //Start the timer again.
                IsGameRunning = true;
                IsGameLoad = false;//----------------Important--------------------//Turn OFF the flag. Otherwise you will start new game from the load!!!!
                _tmrExplosion.Start();                                            //Start explosion timer
            }
            else
            {    
                CreateBaddies();             //Create 10 Baddies (important to initialize them before starting timer)
                _tmr.Start();                //START THE TIMER (Invoke OnTickHandler()Method each 150 milliseconds)
                IsGameRunning = true;
                _tmrExplosion.Start();       //test explosion timer
            }

        }
        private void ClearBoard()
        {
            if (IsGameWin == true || IsGameLose == true)
            {
                //do not set the buttons
                //if You Lose or Win We will set them in Lose or Win methods.
            }
            else
            {   //If this is a FIRST TIME GAME, set thew buttons
                //otherwise set the buttons
                SetPlayButtonState(); //Set the default button state before clearing the board
            }

            if (_goodie != null)
            {
                _goodie.Destroy(); //Destroy  goodie
            }
           
            for (int i = 0; i < _baddies.Count; i++)  //You don't really need this because we cleared the _baddies List above: Line 178
            {
                if (_baddies[i] != null)
                {
                    _baddies[i].Destroy();
                }
            }
            _baddies.Clear();       //clear baddies List
            IsGameRunning = false; //Reset the flag
            IsGameLose = false;
            IsGameWin = false;
            loadedTimes = 0;
            IsloadedMorethanOnce = false;
           
        }

        private void SetButtonsOnWin()
        {
            //Here we manipulate the Physical buttons located in MainPage.xaml
            _btnPause.Visibility = Visibility.Collapsed;
            _btnResumeGame.Visibility = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility = Visibility.Collapsed;
            _btnLoadFile.Visibility = Visibility.Visible;
        }         //Button on Win Game
        private void SetButtonsOnLooseGame()
        {
            //Here we manipulate the Physical buttons located in MainPage.xaml
            _btnPause.Visibility = Visibility.Collapsed;
            _btnResumeGame.Visibility = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility = Visibility.Collapsed;
            _btnLoadFile.Visibility = Visibility.Visible;
        }   //Buttons On Lose Game
        private void SetPlayButtonState()
        {        
            //Here we manipulate the Physical buttons located in MainPage.xaml
  
            _btnPause.Visibility        = Visibility.Visible;
            _btnResumeGame.Visibility   = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility     = Visibility.Visible;
            _btnLoadFile.Visibility     = Visibility.Visible;
           
        }      //Button on Play State
        private void SetButtonsOnLoad()
        {
            _btnPause.Visibility        = Visibility.Visible;
            _btnResumeGame.Visibility   = Visibility.Collapsed;
            _btnStartNewGame.Visibility = Visibility.Visible;
            _btnSaveFile.Visibility     = Visibility.Visible;
            _btnLoadFile.Visibility     = Visibility.Visible;

        }        //Buttons on Load state


        private void CheckWin()
        {
            //Getting the total amount of alive baddies
            int _AmountOfAliveBaddies = 0;
            for (int i = 0; i < _baddies.Count; i++)
            {
                if (_baddies[i].Isalive)
                {
                    _AmountOfAliveBaddies++;
                }
            }

            if (_AmountOfAliveBaddies == 1)
            {
                //If amount of baddies is less than 2
                //Game over
                GameOverWin(); //Call predefined method
            }

        }
        private async void GameOverLoose()
        {
            IsGameLose = true;                        //Set the flag for buttons to be set as OnLoose-state
            SetButtonsOnLooseGame();
            _musicManager.PauseBgMusic();             //Stop current music
           await _musicManager.PlayGameOverSound();   //Gameover music Invoke
            _tmrExplosion.Stop();
            _tmr.Stop();                              //Important to STOP TIMER before awiat MessageBox!!
            IsGameRunning = false;
            ClearBoard(); //clear the board before async message!! (clears the board/sets the buttons)
            GameOverMessage();
          
           
        }
        private async void GameOverWin()
        {
            IsGameWin = true;
            SetButtonsOnWin();
            _musicManager.PauseBgMusic();          //Stop Current music

            await _musicManager.PlayYouWinSound(); //Play win music.
            _tmrExplosion.Stop();
            _tmr.Stop();    //Important to stop timer before MessageDialog()method. It will cause infinitie loop
            IsGameRunning = false;
            YouWinMessage();
            ClearBoard(); //clear the board before async message!! (clears the board, cleaers the baddies List .Sets the buttons)
            
           
        }
        

        //Movements of Player (Goodie)
        public void MoveGoodieUp()
        {
            _goodie.MoveUp();
        }
        public void MoveGoodieDown()
        {
            _goodie.MoveDown();
        }
        public void MoveGoodieLeft()
        {
            _goodie.MoveLeft();
        }
        public void MoveGoodieRight()
        {
            _goodie.MoveRight();
        }

        private void DisplayExplosionOnCollision()
        {
            Image ExplosionImage = new Image();
            Uri uri = new Uri("ms-appx:///Assets/Exlosion.gif");
            ExplosionImage.Source = new BitmapImage(uri);
            //Apply the size to our new Goodie
            ExplosionImage.Height = 70;
            ExplosionImage.Width = 70;

            //Set the x,y position of the Explosion
            Canvas.SetLeft(ExplosionImage, _baddieLeftExplosionCoordinates);
            Canvas.SetTop(ExplosionImage, _baddieTopExplosionCoordinates);

            //Adding Explosion to The Canvas
            _explosionImage = ExplosionImage;
            _playgroundCanvas.Children.Add(ExplosionImage);

            //Add each newly created Explosion image to a list of Images (See the above members: List<Image> _ExplosionImgList = new List<Image>(); ) 
            //Each image will contain its Unique coordinates
            //Later on we will loop throuhgt _ExplosionImgList of images every 800 milliseconds to
            //clear them out of the screen.
            _ExplosionImgList.Add(ExplosionImage);
        }

        private async void GameOverMessage()
        {

           await _contentDialogLose.ShowAsync();    
           
        }
        private async void YouWinMessage()
        {

            await _contentDialogWin.ShowAsync();

        }

        #endregion CustomMethods
    }
}
