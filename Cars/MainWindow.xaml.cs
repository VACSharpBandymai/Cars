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
        string[][] Masyvas;

        public MainWindow()
        {
            InitializeComponent();

            Eil = File.ReadAllLines("Cars.txt");
            Masyvas = new string[Eil.Length][];
            Nr = new int[Eil.Length];
            ValstybNr = new string[Eil.Length];
            Kilometrazas = new int[Eil.Length];
            Modelis = new string[Eil.Length];
            Metai = new int[Eil.Length];
            KuroTipas = new string[Eil.Length];

            for (int i = 0; i < Eil.Length; i++)
            {
                Masyvas[i] = new string[6];
                String Eilute = Eil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < Dalys.Length; j++) Masyvas[i][j] = Dalys[j];
                Nr[i] = int.Parse(Dalys[0]);
                ValstybNr[i] = Dalys[1];
                Kilometrazas[i] = int.Parse(Dalys[2]);
                Modelis[i] = Dalys[3];
                Metai[i] = int.Parse(Dalys[4]);
                KuroTipas[i] = Dalys[5];
            }

            PildytiLentele(GridasDuomenys, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, Masyvas);

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
                  PildytiLentele(GridasDyzelio, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, Dyzelinas);
                  PildytiLentele(GridasBenzino, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, Benzinas);
                  PildytiLentele(GridasDujos, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, Dujos);

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
                  PildytiLentele(GridasMazuKm, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, MazasKm);
                  PildytiLentele(GridasDideliuKm, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, DidelisKm); 
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
            string[][] Paieskai = new string[0][];

            for (int i = 0; i < Eil.Length; i++)
            {
                if (int.Parse(Nuo.Text) <= Kilometrazas[i] && int.Parse(Iki.Text) >= Kilometrazas[i])
                {
                    Paieskai = Paieskai.Concat(new string[][] { Masyvas[i] }).ToArray();
                }
            }

            PildytiLentele(GridasPaieska, new string[] { "Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas" }, Paieskai);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Paieska();
        }
    }
}

