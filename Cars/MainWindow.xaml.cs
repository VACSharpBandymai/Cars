using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Windows.Threading;

namespace Cars
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int[] Nr;
        string[] ValstybNr;
        int[] Kilometrazas;
        string[] Modelis;
        int[] Metai;
        string[] KuroTipas;
        string[] Eil;
        double[] Kaina;
        string[][] Masyvas;
        DispatcherTimer T = new DispatcherTimer();

        //Nauji masyvai uzsakymui
        string[] ValstybesNR = new string[0];
        string[] Marke = new string[0];
        int[] PagaminimoMetai = new int[0];
        int[] Rida = new int[0];
        string[] UzsakymoData = new string[0];
        int[] UzsakymoPabaiga = new int[0];
        string[][] Masyvas1 = new string[0][];
        string[] Failui;
        string[] EiluteUzsakymo;


        public MainWindow()
        {
            InitializeComponent();
            T.Interval = new TimeSpan(0, 0, 1);
            T.Tick += T_Tick;
            T.Start();


            Eil = File.ReadAllLines("Cars.txt");
            Masyvas = new string[Eil.Length][];
            Nr = new int[Eil.Length];
            ValstybNr = new string[Eil.Length];
            Kilometrazas = new int[Eil.Length];
            Modelis = new string[Eil.Length];
            Metai = new int[Eil.Length];
            KuroTipas = new string[Eil.Length];
            Kaina = new double[Eil.Length];

            for (int i = 0; i < Eil.Length; i++)
            {
                Masyvas[i] = new string[7];
                String Eilute = Eil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < Dalys.Length; j++) Masyvas[i][j] = Dalys[j];
                Nr[i] = int.Parse(Dalys[0]);
                ValstybNr[i] = Dalys[1];
                Kilometrazas[i] = int.Parse(Dalys[2]);
                Modelis[i] = Dalys[3];
                Metai[i] = int.Parse(Dalys[4]);
                KuroTipas[i] = Dalys[5];
                Kaina[i] = double.Parse(Dalys[6]);
            }

            PildytiLentele(GridasDuomenys, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina"}, Masyvas);

            string[][] Dyzelinas = new string[0][];
            string[][] Benzinas = new string[0][];
            string[][] Dujos = new string[0][];
            //Filtrui[0] = new string[6];
            //for(int i = 0; i < Eil.Length; i++) Filtrui[i] = new string[6];
            for (int i = 0; i < Eil.Length; i++)
            {
                if (KuroTipas[i].Contains("Dyzelinas"))
                {
                    Dyzelinas = Dyzelinas.Concat(new string[][] { Masyvas[i] }).ToArray();
                }
                if (KuroTipas[i].Contains("Benzinas"))
                {
                    Benzinas = Benzinas.Concat(new string[][] { Masyvas[i] }).ToArray();
                }
                if (KuroTipas[i].Contains("Dujos"))
                {
                    Dujos = Dujos.Concat(new string[][] { Masyvas[i] }).ToArray();
                }
            }
            PildytiLentele(GridasDyzelio, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, Dyzelinas);
            PildytiLentele(GridasBenzino, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, Benzinas);
            PildytiLentele(GridasDujos, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, Dujos);

            string[][] MazasKm = new string[0][];
            string[][] DidelisKm = new string[0][];
            for (int i = 0; i < Eil.Length; i++)
            {
                if (Kilometrazas[i] <= 200000)
                {
                    MazasKm = MazasKm.Concat(new string[][] { Masyvas[i] }).ToArray();
                }
                else DidelisKm = DidelisKm.Concat(new string[][] { Masyvas[i] }).ToArray();
            }
            PildytiLentele(GridasMazuKm, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, MazasKm);
            PildytiLentele(GridasDideliuKm, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, DidelisKm);
            
            boxas();
            NuskaitytiUzsakymus();
        }

        void T_Tick(object sender, EventArgs e)
        {
            Laikas.Content = DateTime.Now.ToLongTimeString();
            Laikas1.Content = DateTime.Now.ToLongTimeString();
            Laikas2.Content = DateTime.Now.ToLongTimeString();
        }

        void PildytiLentele(DataGrid Lentele, string[] Stulpeliai, string[][] Eilute)
        {
            DataTable Lenta = new DataTable();
            //foreach (string Stulpelis in Stulpeliai) Lenta.Columns.Add(Stulpelis);
            //foreach (string[] Eile in Eilute) Lenta.Rows.Add(Eile);
            for (int i = 0; i < Stulpeliai.Length; i++) Lenta.Columns.Add(Stulpeliai[i]);
            for (int i = 0; i < Eilute.Length; i++) Lenta.Rows.Add(Eilute[i]);

            Lentele.DataContext = Lenta.DefaultView;
        }

        void Paieska()
        {
            string[][] PaieskaiRida = new string[0][];
            string[][] PaieskaiMetai = new string[0][];
            string[][] PaieskaiKaina = new string[0][];
            
                {
                    if (cbx.SelectedItem.ToString().Contains("Rida"))
                    {
                        for (int i = 0; i < Eil.Length; i++)
                            if (Kilometrazas[i] >= int.Parse(Nuo.Text) && Kilometrazas[i] <= int.Parse(Iki.Text))
                            {
                                PaieskaiRida = PaieskaiRida.Concat(new string[][] { Masyvas[i] }).ToArray();
                            }
                        PildytiLentele(GridasPaieska, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, PaieskaiRida);
                    }
                    else if (cbx.SelectedItem.ToString().Contains("Metai"))
                    {
                        for (int i = 0; i < Eil.Length; i++)
                            if (Metai[i] >= int.Parse(Nuo.Text) && Metai[i] <= int.Parse(Iki.Text))
                            {
                                PaieskaiMetai = PaieskaiMetai.Concat(new string[][] { Masyvas[i] }).ToArray();
                            }
                        PildytiLentele(GridasPaieska, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, PaieskaiMetai);
                    }
                    else if (cbx.SelectedItem.ToString().Contains("Kaina"))
                    {
                        for (int i = 0; i < Eil.Length; i++)
                            if (Kaina[i] >= double.Parse(Nuo.Text) && Kaina[i] <= double.Parse(Iki.Text))
                            {
                                PaieskaiKaina = PaieskaiKaina.Concat(new string[][] { Masyvas[i] }).ToArray();
                            }
                        PildytiLentele(GridasPaieska, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas", "Kaina" }, PaieskaiKaina);
                    }
                }
            }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Paieska();
            
        }

        void boxas()
        {
            cbx.Items.Add("Rida");
            cbx.Items.Add("Metai");
            cbx.Items.Add("Kaina");   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TrintiIrasa(GridasDuomenys);
        }

        void TrintiIrasa(DataGrid P)
        {
            if (P.SelectedIndex > -1)
            {
                DataTable L = new DataTable();
                L.Merge((P.DataContext as DataView).ToTable());
                L.Rows.RemoveAt(P.SelectedIndex);
                P.DataContext = L.DefaultView;
            }
            else MessageBox.Show("Pasirinkite kuria eilute norite trinti");
        }

        void Uzsakymui()
        {
            try
            {
                if (!String.IsNullOrEmpty(Valst.Text) || !String.IsNullOrEmpty(MarkeText.Text) || !String.IsNullOrEmpty(PagMetai.Text) || !String.IsNullOrEmpty(RidaText.Text)
                        || !String.IsNullOrEmpty(UzsakymoDataText.Text) || !String.IsNullOrEmpty(KadaAtlikti.Text))
                {
                    ValstybesNR = ValstybesNR.Concat(new string[] { Valst.Text }).ToArray();
                    Marke = Marke.Concat(new string[] { MarkeText.Text }).ToArray();
                    PagaminimoMetai = PagaminimoMetai.Concat(new int[] { int.Parse(PagMetai.Text) }).ToArray();
                    Rida = Rida.Concat(new int[] { int.Parse(RidaText.Text) }).ToArray();
                    UzsakymoData = UzsakymoData.Concat(new string[] { UzsakymoDataText.Text.ToString() }).ToArray();
                    UzsakymoPabaiga = UzsakymoPabaiga.Concat(new int[] { int.Parse(KadaAtlikti.Text) }).ToArray();

                    Masyvas1 = Masyvas1.Concat(new string[][] { new string[] { Valst.Text, MarkeText.Text, PagMetai.Text, RidaText.Text, 
                UzsakymoDataText.Text, KadaAtlikti.Text} }).ToArray();

                    PildytiLentele(GridasUzsakymas, new string[] { "Valstyves Numeris", "Marke", "Pagaminimo Metai", "Rida", "Uzsakymo Data", "Uzsakymo Pab.Terminas" },
                    Masyvas1);

                    Failui = new string[Masyvas1.Length]; // Cia is eil, pakeiciau i Masyvas1
                    for (int i = 0; i < Failui.Length; i++) { 
                    Failui[i] = ValstybesNR[i] + "|" + Marke[i] + "|" + PagaminimoMetai[i] + "|" + Rida[i] + "|" + UzsakymoData[i] + "|" + UzsakymoPabaiga[i];
                    //for (int j = 0; j < Failui.Length; j++ ) // nezinau ar cia Failui.Lenght ar EiluteUzsakymo? manau failui, nes jis cia irasinejamas i faila.
                    //    Masyvas1[i][j] = Failui[i]; // manau neteisingai priskiriu.. man reikia priskirti, kad galeciau funkcijoj NuskaitytiUzsakymus() pildyti lentele.
                    }//nebent gal galima iskart tam pildyme duoti Failui pildyti, bet netinka, nes funkcija aprasyta kaip dvimacio masyvo
                    
                    // As nelabai supratau ko nori Tu su tuo ciklu pasiekt.
                    // I faila rasoma tada, kai jau visa masyva turi suformuota. 
                    // To Masyvas1 [i][j] net nesupratau kam tau reikia?
                    // O nepridedavo todel, kad Masyvas[i] dydis buvo 6, o sukai cikla 10 kartu.
                    // Ir jis turedavo nulust, bet catch sugaudydavo, ir programa neluzdavo ir nerodydavo tau kur negerai.
                    File.WriteAllLines("Pirkimai.txt", Failui);
                }
                else MessageBox.Show("Neuzpildete visu laukeliu!");
            }
            catch { MessageBox.Show("KLAIDA"); };
        }

        void NuskaitytiUzsakymus()
        {
            EiluteUzsakymo = File.ReadAllLines("Pirkimai.txt");
            for (int i = 0; i < EiluteUzsakymo.Length; i++) File.ReadAllLines("Pirkimai.txt");
            PildytiLentele(GridasUzsakymas, new string[] { "Valstyves Numeris", "Marke", "Pagaminimo Metai", "Rida", "Uzsakymo Data", "Uzsakymo Pab.Terminas" },
                    Masyvas1);
        }
        private void PildymoMygtukas_Click(object sender, RoutedEventArgs e)
        {
            Uzsakymui();
            
        }

        private void Valst_MouseMove(object sender, MouseEventArgs e)
        {
            Valst.Text = "";
            MarkeText.Text = "";
            PagMetai.Text = "";
            RidaText.Text = "";
            UzsakymoDataText.Text = "";
            KadaAtlikti.Text = "";
        }
    }
}

