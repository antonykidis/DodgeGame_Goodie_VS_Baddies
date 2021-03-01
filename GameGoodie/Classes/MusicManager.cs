
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DodgeGame.Classes
{
    public class MusicManager
    {
        // members to manipulate the sounds for below methods
        //For example the StopBackgroundMusic() method
        private MediaElement _collisionSound = new MediaElement();
        private MediaElement _gamneOverSound = new MediaElement();
        private MediaElement _introSound = new MediaElement();
        private MediaElement _backgroundSound = new MediaElement();
        private MediaElement _saveGameSound = new MediaElement();
        private MediaElement _loadGameSound = new MediaElement();
        private MediaElement _youWinSound = new MediaElement();
        private MediaElement _ouchSound = new MediaElement();


        public MusicManager()
        {

        }

        //music methods mapped to music members.............................................................
        public async Task<MediaElement> PlayCollisionSound()
        {
            var CollisionSoundElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("CollisionBaddie.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            CollisionSoundElement.SetSource(stream, "");
            CollisionSoundElement.Play();
            return CollisionSoundElement;
        }
        public async Task<MediaElement> PlayGameOverSound()
        {
            var GameOverElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("GameOver.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            GameOverElement.SetSource(stream, "");
            GameOverElement.Play();
            return GameOverElement;
        }
        public async Task<MediaElement> PlayIntroSound()
        {
            var IntroMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("IntroMusic.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            IntroMusicElement.SetSource(stream, "");
            IntroMusicElement.Play();
            return IntroMusicElement;
        }
        public async Task<MediaElement> PlayBackgroundMusicSound()
        {
            var BgMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Mario.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            BgMusicElement.SetSource(stream, "");
            BgMusicElement.Play();
            BgMusicElement.Volume = 0.5;   //Play it half volume
            _backgroundSound = BgMusicElement;
            return BgMusicElement;


        }
        public async Task<MediaElement> PlaySaveGameSound()
        {
            var SaveGameElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Mario.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            SaveGameElement.SetSource(stream, "");
            SaveGameElement.Play();
            SaveGameElement.Volume = 0.5;   //Play it half volume
            return SaveGameElement;


        }
        public async Task<MediaElement> PlayLoadGameSound()
        {
            var LoadMusicElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("Mario.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            LoadMusicElement.SetSource(stream, "");
            LoadMusicElement.Play();
            LoadMusicElement.Volume = 0.5;   //Play it half volume
            return LoadMusicElement;


        }
        public async Task<MediaElement> PlayYouWinSound()
        {
            var YouWinElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("YouWin.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            YouWinElement.SetSource(stream, "");
            YouWinElement.Play();
            YouWinElement.Volume = 0.5;   //Play it half volume
            return YouWinElement;


        }
        public async Task<MediaElement> PlayGoodieIsDeadSound()
        {
            var GoodieIsDead = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("DeadGoodie.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            GoodieIsDead.SetSource(stream, "");
            GoodieIsDead.Play();
            return GoodieIsDead;
        }
        public async Task<MediaElement> PlaGoodieOuchSound()
        {
            var OuchMusciElement = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Music");
            var file = await folder.GetFileAsync("ai-ai-2.mp3");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            OuchMusciElement.SetSource(stream, "");
            OuchMusciElement.Play();
            OuchMusciElement.Volume = 0.5;   //Play it half volume
            _ouchSound = OuchMusciElement;
            return OuchMusciElement;


        }


        //Public methods to be called from GameDriver.cs.....................................................
        public void StopBgMusic()
        {
            //Stops the background music
            _backgroundSound.Stop();

        }     //Pause the music :-)

        public void PauseBgMusic()
        {
            //Stops the background music
            _backgroundSound.Pause();
           
        }     //Pause the music :-)
        public  MediaElement ResumePlayingBGMusic()  //Unpause the game. The music will start from the paused state :-))
        {
            var BgMusicElement = new MediaElement();
            BgMusicElement = _backgroundSound;
             BgMusicElement.Play();
            return BgMusicElement;
        }

     

    }
}
