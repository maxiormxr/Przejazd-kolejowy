using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_zaliczeniowy
{
    class Train : System.Windows.Forms.PictureBox
    {
        int trasa;

        public Train(int _trasa) : base()
        {
            trasa = _trasa;
        }
        
        public int GetTrasa()
        {
            return trasa;
        }

        ~Train()
        {

        }
    }
}
