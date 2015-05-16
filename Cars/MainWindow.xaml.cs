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
           
            PildytiLentele(Gridas, new string[] {"Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas"},
               Masyvas);

            int skaicuok = 0;
            for (int i = 0; i < Eil.Length; i++)
            {
                
                if (KuroTipas[i] == "dyzelinas")
                    skaicuok++;
                    PildytiLentele(Gridas1, new string[] {"Numeris", "Valstybes Numeris", "Kilometrazas", "Masinos Marke", "Metai", "Kuro Tipas"},
               Masyvas);
            }
           
           
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
    }
}

