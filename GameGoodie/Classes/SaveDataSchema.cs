using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace DodgeGame.Classes
{
    class SaveDataSchema
    {
        //Model to create a json file

        public int _BaddieStartTop { get; set; }
        public int _BaddieStartLeft { get; set; }
        public int _IdBaddie { get; set; }
        public int _GoodieStartTop { get; set; }
        public int _GoodieStartLeft { get; set; }
      

        //constructor
        public SaveDataSchema(int BaddieStartTop,int BaddieStartLeft, int IdBaddie, int GoodieStartTop, int GoodieStartLeft)
        {
            //Baddie---------------------------------
            this._BaddieStartTop = BaddieStartTop;
            this._BaddieStartLeft = BaddieStartLeft;
            this._IdBaddie = IdBaddie;
           //Goddie----------------------------------
            this._GoodieStartTop = GoodieStartTop;
            this._GoodieStartLeft = GoodieStartLeft;
         
        }

        public SaveDataSchema()
        {
            
        }

    }
}
