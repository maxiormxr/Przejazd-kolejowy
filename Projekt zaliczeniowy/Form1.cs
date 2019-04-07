using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Collections;

namespace Projekt_zaliczeniowy
{
    public partial class Form1 : Form
    {
        //zmienne
        int[] k = new int[1];
        
        //Zmienna losowa
        Random rnd = new Random();
        
        //Flagi
        bool IsTrainIs = false;
        bool Randomize = true;
        bool IsPeopleIs = false;

        //Listy obiektów//tablice
        int[,] Ekran = new int[1932, 1492];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ładowanie interfacu i potrzebnych danych
            textBox1.Visible = false;
            spawn_car.Visible = false;
            k[0] = 0;
            DoubleBuffered = true; //ustawiamy podwójne buforowanie grafiki, by ekran nie mrugał

            //wypełnienie mapy sceny zerami
            for (int i = 0; i < 1932; i++)
            {
                for (int j = 0; j < 1492; j++)
                {
                    Ekran[i, j] = 0;
                }
            }
        }

        private void spawn_train_Click(object sender, EventArgs e)
        {
            //uruchomienie wątku odpowiedzialnego za pociąg
            new Thread(SpawningTrain).Start();
        }

        private void spawn_car_Click(object sender, EventArgs e)
        {
            new Thread(SpawningCars).Start(); // uruchamiamy wątek spownujący samochód na scenę
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lock (k)
            { k[0] = 0; }
            for (int i = 0; i <= 3; i++) //spawnuj falę samochodów
            {
                new Thread(SpawningCars).Start(); // uruchamianie wątku spawnującego samochod
            }
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) //zmiana spawnowania z automatycznego na ręczne
        {
            if (checkBox1.Checked == true) // jeśli ręczne
            {
                Randomize = false; //droga wczytywana z text boxa nie ze zmiennej
                textBox1.Visible = true; //GUI
                spawn_car.Visible = true;
                random_wave.Visible = false;
            }
            else //jeśli automatyczne
            {
                Randomize = true; //droga wczytywana ze zmiennej nie z text boxa
                textBox1.Visible = false; //GUI
                spawn_car.Visible = false;
                random_wave.Visible = true;
            }
        }

        public void SpawningTrain()
        {
            //Spawnowanie pociągu

            if (IsTrainIs == false) //jeżeli nie istnieje żaden pociąg
            {
                IsTrainIs = true; //zmieniamy że istnieje
                //Thread.Sleep(1);

                lock (Ekran) // blokada mapy sceny i wypełnienie trasy pociągu '2'
                {

                    for (int i = 0; i < 1932; i++)
                    {
                        for (int j = 250; j < 340; j++)
                        {

                            if (Ekran[i, j] != 1) //pomijamy miejsca gdzie na torach jest auto, aby zdążyło zjechać
                            {
                                Ekran[i, j] = 2; 
                            }
                        }
                    }
                }

                bool TrainFlag = false; //Flaga ruszenia pociągu

                while (TrainFlag == false) //Czekamy aż tory będą puste
                {
                    TrainFlag = true; //ustawiamy tory na wolne
                    for (int i = 0; i < 1932; i++)
                    {
                        for (int j = 250; j < 340; j++)
                        {

                            if (Ekran[i, j] != 1) //sprawdzamy czy jest auto na torach
                            {
                                lock (Ekran)
                                { Ekran[i, j] = 2; } //miejsca gdzie już nie ma samochodów wypełniamy '2'
                            }
                            else
                            {
                                TrainFlag = false; //gdy znajdzie się chociaż jeden piksel gdzie jeszcze jest auto na torach ustawiamy tory na zajęte
                            }
                        }
                    }
                }
                //Po wyjściu z pętli tory na pewno są puste

                int Losowanie;
                Losowanie = rnd.Next(1, 5000) % 2; //losujemy strone z której pojedzie pociąg
                Action action;
                Train Train1 = new Train(Losowanie); //tworzymy pociąg 
                Point Punkt = new Point(); //oraz punkt w którym zostanie umieszczony
                if (Losowanie == 1)
                { Punkt.X = -100; } //pociąg jedzie z lewej współrzędna X
                else { Punkt.X = 1900; } //pociąg jedzie z prawej
                Punkt.Y = 258; //współrzędna Y

                Train1.Location = Punkt; //ustawiamy pociąg w odpowiedniej lokalizacji
                Train1.Height = 70; //nadajemy mu wysokośc
                Train1.Width = 400; //i szerokość
                Train1.BackColor = Color.Yellow; //oraz kolor
                action = () => { this.Controls.Add(Train1); }; //dodajemy go do formatki za pomocą delegata
                this.Invoke(action);

                IsTrainIs = true;

                if (Losowanie == 1) //jeżeli pociąg jedzie z lewej - przesuwamy go w prawo
                {
                    Point Punkt1 = Train1.Location; //tworzymy nowy punkt
                    Action action1; //i delegata
                    while (Punkt1.X < 1900) // dopóki pociąg nie wyjedzie za mapę
                    {
                        Punkt1.X += 8; //przesuwamy go w każdym kroku o 8px w prawo

                        action1 = () => { Train1.Location = Punkt1; Train1.Update(); }; //i updatujemy jego położenie na formatce
                        this.Invoke(action1);

                        Thread.Sleep(80); //usypiamy wątek by dać szanse innym na dostęp do procesora
                    }
                    IsTrainIs = false;// jeżeli dojechał do końca mapy ustawiamy flage na brak pociągu
                }
                else if (Losowanie == 0) //jeżeli z prawej - w lewo
                {
                    Point Punkt1 = Train1.Location;//tworzymy nowy punkt
                    Action action1;//i delegata
                    while (Punkt1.X > -200)// dopóki pociąg nie wyjedzie za mapę
                    {
                        Punkt1.X -= 8;//przesuwamy go w każdym kroku o 8px w lewo
                        action1 = () => { Train1.Location = Punkt1; Train1.Update(); }; //i updatujemy jego położenie na formatce
                        this.Invoke(action1);
                        this.Invoke(action1);

                        Thread.Sleep(80); //usypiamy wątek by dać szanse innym na dostęp do procesora
                    }
                    IsTrainIs = false; // jeżeli dojechał do końca mapy ustawiamy flage na brak pociągu
                }

                for (int i = 0; i < 1932; i++) //mapę sceny w miejscu gdzie przebiegała trasa pociągu wypełniamy zerami
                {
                    for (int j = 250; j < 340; j++)
                    {
                        lock (Ekran)
                        { Ekran[i, j] = 0; }

                    }
                }

                if (IsTrainIs == false) // jeżeli pociąg dojechał do końca mapy - usuwamy go z formatki
                {
                    action = () => { this.Controls.Remove(Train1); };
                    this.Invoke(action);
                }
            }
        }     

        public void SpawningCars()
        {
            Point SpwnCarPoint = new Point(); //stwórz punkt spownowania samochodu
            Action action2; //oraz delegata
            int kierunek = 0; //zmienna okreslającą kierunek jazdy (do góry/na dół)
            
                
                try //jeśli w text boxie nie będzie liczby wyświetli msg boxa z informacją
                {
                lock (k) //zablokuj zmienna od numeru drogi
                {
                    if (Randomize == false) // wpisywanie reczne
                    {
                        lock (textBox1)
                        {
                            k[0] = Convert.ToInt32(textBox1.Text); // załaduj z text boxa nr drogi
                        }
                    }
                    else //automatyczne
                    {
                        lock (k)
                        { k[0] = k[0] + 1; } // załaduj kolejną drogę
                    }

                    //Określanie współrzędnych startowych w zależności od wczytanej drogi
                    
                    if (k[0] == 1)//droga 1
                    {
                        SpwnCarPoint.X = 715;
                        SpwnCarPoint.Y = 10;
                    }
                    else if (k[0] == 2)//droga 2
                    {
                        SpwnCarPoint.X = 808;
                        SpwnCarPoint.Y = 10;
                    }
                    
                    else if (k[0] == 3)//droga 3
                    {
                        SpwnCarPoint.X = 935;
                        SpwnCarPoint.Y = 940;
                    }
                    else if (k[0] == 4)//droga 4
                    {
                        SpwnCarPoint.X = 1028;
                        SpwnCarPoint.Y = 940;
                    }
                    else if (k[0] > 4 || k[0] < 0)//błędne wpisanie drogi - automatycznie na drogę 4
                    {
                        MessageBox.Show("Podałeś zły parametr, samochód zostanie wygenerowany na drodze 4 - jeżeli to możliwe"); //msg box
                        SpwnCarPoint.X = 1735; 
                        SpwnCarPoint.Y = 940;
                    }
                    //LOSOWANIE PUNKTU
                    //
                    //  170,10 -1
                    //  880,10 -2
                    // 1435,10 -3
                    // 1528,10 -4
                    //
                    // 260,940 -5
                    // 1637,940 -6
                    // 1735,940 -7
                    //

                    if (k[0] > 2) //określenie kierunku jazdy dla dróg z numerem mniejszym od 4 w dół
                    {
                        kierunek = -1;
                    }
                    else // dla większych od 4 w górę
                    {
                        kierunek = 1;
                    }

                }


                    int u = SpwnCarPoint.X + 22, p = SpwnCarPoint.Y + 50; //punkt potrzeebny do sprawdzenia czy jest miejsce na spawn nowego auta na daną drogę w przypadku górnej drogi
                    int u1 = SpwnCarPoint.X + 22, p1 = SpwnCarPoint.Y; //oraz dolnej

                if ((Ekran[u, p] == 0 && kierunek == 1) || (Ekran[u1, p1] == 0 && kierunek == -1)) //sprawdzenie czy można spawnować
                {
                    if ((Ekran[u, p-10] == 0 && kierunek == 1) || (Ekran[u1, p1+20] == 0 && kierunek == -1))
                    {
                        Car Car1 = new Car(kierunek); //stworzenie samochodu
                        Car1.Width = 45; //nadanie rozmiarów
                        Car1.Height = 50;
                        Car1.Location = SpwnCarPoint; //punktu początkowego
                        Car1.BackColor = Color.Blue; //koloru

                        action2 = () => { this.Controls.Add(Car1); };//dodanie auta na scene
                        lock (action2)
                        {

                            this.Invoke(action2);
                        }
                        lock (Ekran) //wypełnienie miejsca zajmowanego przez samochód '1'
                        {
                            for (int i = SpwnCarPoint.X; i <= (SpwnCarPoint.X + 45); i++)
                                for (int j = SpwnCarPoint.Y; j <= SpwnCarPoint.Y + 50; j++)
                                {
                                    Ekran[i, j] = 1;
                                }
                        }
                        Thread.Sleep(500); //usypiamy wątek by inni mieli 'swój' czas

                        Point Punkt1 = new Point(); //stworzenie punktu do którego się poruszamy
                        Punkt1 = Car1.Location; // i przypisanie mu współrzędnych auta
                        int x, y, y1; //zmienne pomocnicze przy ustalaniu odpowiednich współrzędnych
                        bool flag = false; //sprawdzanie czy auto mieści się na mapie
                        if (kierunek == -1) //jeżeli jedzie od dołu do góry
                        {
                            while (Car1.Location.Y > -50) //porusza się dopóki nie wyjedzie poza górną krawędź obrazu
                            {
                                Punkt1 = Car1.Location; //przypisanie aktualnych współrzędnych samochodu
                                x = Punkt1.X + 22; //określenie punktu kontrolnego w kierunku poruszania auta
                                y = Punkt1.Y - 16;
                                y1 = Punkt1.Y - 5;

                                if (y > 0) //jeżeli auto mieści się na ekranie
                                {
                                    if (Ekran[x, y] == 0 && Ekran[x, y1] == 0) // i nic przed nim nie ma
                                    {
                                        flag = true; //zezwól na poruszanie
                                    }
                                    else
                                    {
                                        flag = false; //w przeciwnym wypadku nie zezwalaj
                                    }
                                }

                                if (flag == true || (y < 0 && y >= -80)) //gdy auto ma zezwolenie na poruszanie, bądź prawie już zjechało z mapy

                                {
                                    Punkt1.Y -= 5; //przesuń punkt docelowy o 5 pks w górę
                                    action2 = () => { Car1.Location = Punkt1; Car1.Update(); }; // i updatuj formatkę z nowym punktem
                                    lock (action2) { this.Invoke(action2); }

                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++) //wypełnij przestrzeń którą auto zwolniło zerami
                                    {
                                        for (int j = Punkt1.Y + 50; j <= Punkt1.Y + 58; j++)
                                        {
                                            lock (Ekran)
                                            { Ekran[i, j] = 0; }
                                        }
                                    }
                                    if (y > 0) //jeżeli auto całe znajduje się na mapie
                                    {
                                        for (int i = Punkt1.X; i <= Punkt1.X + 45; i++)
                                        {
                                            for (int j = Punkt1.Y; j <= Punkt1.Y + 8; j++)
                                            {
                                                lock (Ekran) //wypełnij obszar w który się przesunęło jedynkami
                                                {
                                                    Ekran[i, j] = 1;
                                                }
                                            }
                                        }
                                    }
                                    else //jeżeli tylko fragment auta znajduje się w granicach ekranu
                                    {
                                        for (int i = Punkt1.X; i <= Punkt1.X + 45; i++)
                                        {
                                            for (int j = 0; j <= Punkt1.Y + 8; j++)
                                            {
                                                lock (Ekran)
                                                {
                                                    Ekran[i, j] = 1; //tylko ten fagment wypełnij jedynkami
                                                }
                                            }
                                        }
                                    }

                                    Thread.Sleep(rnd.Next(100, 300));//daj czas innym wątkom na pracę
                                }//jeżeli auto znajduje się na torach - zwolnij tory - by pociąg mógł przejechać
                                else if ((Ekran[x, y] != 1 && Ekran[x, y1] != 1) && ((y < 0 && y >= -80) || Ekran[Punkt1.X - 5, Punkt1.Y] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 10] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 20] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 40] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 60] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 50] == 2))
                                {
                                    Punkt1.Y -= 5; //przesuń pojazd o 5px
                                    action2 = () => { Car1.Location = Punkt1; Car1.Update(); }; //updatuj
                                    lock (action2) { this.Invoke(action2); }

                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++) //wypełnij zerami gdzie go nie ma
                                    {
                                        for (int j = Punkt1.Y + 50; j <= Punkt1.Y + 58; j++)
                                        {
                                            lock (Ekran)
                                            { Ekran[i, j] = 0; }
                                        }
                                    }

                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++) //i jedynkami gdzie się przesunął
                                    {
                                        for (int j = Punkt1.Y; j <= Punkt1.Y + 8; j++)
                                        {
                                            lock (Ekran)
                                            {
                                                Ekran[i, j] = 1;
                                            }
                                        }
                                    }
                                    Thread.Sleep(rnd.Next(100, 300)); //idź spać
                                }
                                else//jeżeli nie możesz nigdzie jechać
                                {
                                    Thread.Sleep(rnd.Next(100, 300)); // odpocznij
                                }

                            }
                        }

                        else if (kierunek == 1) //analogicznie do wersji wyżej - tylko inny kierunek
                        {
                            while (Car1.Location.Y < 1000)
                            {
                                Punkt1 = Car1.Location;
                                x = Punkt1.X + 22;
                                y = Punkt1.Y + 16 + 50;
                                y1 = Punkt1.Y + 50 + 5;

                                if (Ekran[x, y] == 0 && Ekran[x, y1] == 0)
                                {
                                    Punkt1.Y += 5;

                                    action2 = () => { Car1.Location = Punkt1; Car1.Update(); };
                                    lock (action2)
                                    {
                                        try
                                        {
                                            this.Invoke(action2);
                                        }
                                        catch (System.ObjectDisposedException)
                                        {

                                        }

                                    }


                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++)
                                    {
                                        for (int j = Punkt1.Y + 42; j <= Punkt1.Y + 50; j++)
                                        {
                                            lock (Ekran)
                                            { Ekran[i, j] = 1; }


                                        }
                                    }

                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++)
                                    {
                                        for (int j = Punkt1.Y - 8; j <= Punkt1.Y; j++)
                                        {
                                            lock (Ekran)
                                            {
                                                Ekran[i, j] = 0;
                                            }
                                        }
                                    }

                                    Thread.Sleep(rnd.Next(100, 300));
                                }
                                else if ((Ekran[x, y] != 1 && Ekran[x, y - 10] != 1 && Ekran[x, y1] != 1) && (Ekran[Punkt1.X - 5, Punkt1.Y] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 10] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 20] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 40] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 60] == 2 || Ekran[Punkt1.X - 5, Punkt1.Y + 50] == 2))
                                {
                                    Punkt1.Y += 5;

                                    action2 = () => { Car1.Location = Punkt1; Car1.Update(); };
                                    lock (action2)
                                    {
                                        this.Invoke(action2);

                                    }


                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++)
                                    {
                                        for (int j = Punkt1.Y + 42; j <= Punkt1.Y + 50; j++)
                                        {
                                            lock (Ekran)
                                            { Ekran[i, j] = 1; }


                                        }
                                    }

                                    for (int i = Punkt1.X; i <= Punkt1.X + 45; i++)
                                    {
                                        for (int j = Punkt1.Y - 8; j <= Punkt1.Y; j++)
                                        {
                                            lock (Ekran)
                                            {
                                                Ekran[i, j] = 0;
                                            }
                                        }
                                    }

                                    Thread.Sleep(rnd.Next(100, 300));
                                }
                                else
                                {
                                    Thread.Sleep(rnd.Next(100, 300));
                                }



                            }
                        }

                        if (Car1.Location.Y > 0)
                        {
                            for (int i = Car1.Location.X; i <= Car1.Location.X + 45; i++)
                                for (int j = Car1.Location.Y; j <= Car1.Location.Y + 50; j++)
                                    lock (Ekran)
                                    {
                                        Ekran[i, j] = 0;
                                    }
                        }
                        else
                        {
                            for (int i = Car1.Location.X; i <= Car1.Location.X + 45; i++)
                                for (int j = 0; j <= Car1.Location.Y + 50; j++)
                                    lock (Ekran)
                                    {
                                        Ekran[i, j] = 0;
                                    }
                        }

                        action2 = () => { this.Controls.Remove(Car1); };
                        lock (action2)
                        {
                            this.Invoke(action2);
                        }

                    }
                    else
                    {
                        if (Randomize == true)
                        { }
                        else { MessageBox.Show("NIE ŚPIESZ SIĘ!!!"); }
                    }
                }
                }
                catch (System.FormatException)
                {
                    if (Randomize == false) { MessageBox.Show("Podaj prawidłową liczbę"); }
                }
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new Thread(SpawningPeople).Start();
        }

        private void SpawningPeople()
        {
            //Spawnowanie ludzia
            
            if (IsPeopleIs == false) //jeżeli nie istnieje żaden ludź
            {
                IsPeopleIs = true; //zmieniamy że istnieje
                //Thread.Sleep(1);

                lock (Ekran) // blokada mapy sceny i wypełnienie trasy ludzia '2'
                {

                    for (int i = 600; i < 1200; i++)
                    {
                        for (int j = 670; j < 800; j++)
                        {

                            if (Ekran[i, j] != 1) //pomijamy miejsca gdzie na pasach jest auto, aby zdążyło zjechać
                            {
                                Ekran[i, j] = 2;
                            }
                        }
                    }
                }

                bool PeopleFlag = false; //Flaga ruszenia ludzia

                while (PeopleFlag == false) //Czekamy aż pasy będą puste
                {
                    PeopleFlag = true; //ustawiamy pasy na wolne
                    for (int i = 600; i < 1200; i++)
                    {
                        for (int j = 670; j < 800; j++)
                        {

                            if (Ekran[i, j] != 1) //sprawdzamy czy jest auto na pasach
                            {
                                lock (Ekran)
                                { Ekran[i, j] = 2; } //miejsca gdzie już nie ma samochodów wypełniamy '2'
                            }
                            else
                            {
                                PeopleFlag = false; //gdy znajdzie się chociaż jeden piksel gdzie jeszcze jest auto na pasach ustawiamy pasy na zajęte
                            }
                        }
                    }
                }
                //Po wyjściu z pętli pasy na pewno są puste

                int Losowanie;
                Losowanie = rnd.Next(1, 5000) % 2; //losujemy strone z której pojdzie ludź
                Action action;
                Train People1 = new Train(Losowanie); //tworzymy ludzia
                Point Punkt = new Point(); //oraz punkt w którym zostanie umieszczony
                if (Losowanie == 1)
                { Punkt.X = 600; } //ludź idzie z lewej współrzędna X
                else { Punkt.X = 1200; } //ludź idzie z prawej
                Punkt.Y = 715; //wspóżędna Y

                People1.Location = Punkt; //ustawiamy ludzia w odpowiedniej lokalizacji
                People1.Height = 30; //nadajemy mu wysokośc
                People1.Width = 30; //i szerokość
                People1.BackColor = Color.Green; //oraz kolor
                action = () => { this.Controls.Add(People1); }; //dodajemy go do formatki za pomocą delegata
                this.Invoke(action);

                IsPeopleIs = true;

                if (Losowanie == 1) //jeżeli ludź idzie z lewej - przesuwamy go w prawo
                {
                    Point Punkt1 = People1.Location; //tworzymy nowy punkt
                    Action action1; //i delegata
                    while (Punkt1.X < 1200) // dopóki ludź nie zejdzie z pasów, żeby go przypadkiem coś nie rozjechało na miazgę
                    {
                        Punkt1.X += 3; //przesuwamy go w każdym kroku o 3pks w prawo

                        action1 = () => { People1.Location = Punkt1; People1.Update(); }; //i updatujemy jego położenie na formatce
                        this.Invoke(action1);

                        Thread.Sleep(80); //usypiamy wątek by dać szanse innym na dostęp do procesora
                    }
                    IsPeopleIs = false;// jeżeli doszedł do końca pasów - ustawiamy flagę na brak ludzia (heheszki)
                }
                else if (Losowanie == 0) //jeżeli z prawej - w lewo
                {
                    Point Punkt1 = People1.Location;//tworzymy nowy punkt
                    Action action1;//i delegata
                    while (Punkt1.X > 600)// dopóki ludź nie zejdzie z pasów
                    {
                        Punkt1.X -= 3;//przesuwamy go w każdym kroku o 3pks w lewo
                        action1 = () => { People1.Location = Punkt1; People1.Update(); }; //i updatujemy jego położenie na formatce
                        this.Invoke(action1);
                        this.Invoke(action1);

                        Thread.Sleep(80); //usypiamy wątek by dać szanse innym na dostęp do procesora
                    }
                    IsPeopleIs = false; // jeżeli doszedł do końca pasów ustawiamy flagę na brak ludzia
                }

                for (int i = 600; i < 1200; i++) //mapę sceny w miejscu gdzie przebiegała trasa ludzia wypełniamy zerami
                {
                    for (int j = 670; j < 800; j++)
                    {
                        lock (Ekran)
                        { Ekran[i, j] = 0; }

                    }
                }

                if (IsPeopleIs == false) // jeżeli pociąg dojechał do końca mapy - usuwamy go z formatki
                {
                   action = () => { this.Controls.Remove(People1); };  //naprawic
                    this.Invoke(action);
                }
            }
        }

    }
}
