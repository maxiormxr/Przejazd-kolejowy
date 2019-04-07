using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt_zaliczeniowy
{
    class Car : System.Windows.Forms.PictureBox
    {
        int kierunek;

        public Car(int _kierunek) : base()
        {
            kierunek = _kierunek;
        }

        public int GetKierunek()
        {
            return kierunek;
        }

    }
}
