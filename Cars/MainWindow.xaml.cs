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
                String Eilute = Eil[i];
                String[] Dalys = Eilute.Split(new String[] { "|" }, System.StringSplitOptions.RemoveEmptyEntries);
                for (int j = 0; j < 6; j++) Masyvas[i][j] = Dalys[j];
                Nr[i] = int.Parse(Dalys[0]);
                ValstybNr[i]= Dalys[1];
                Kilometrazas[i] = int.Parse(Dalys[2]);
                Modelis[i] = Dalys[3];
                Metai[i] = int.Parse(Dalys[4]);
                KuroTipas[i] = Dalys[5];
            }

            DataTable G = new DataTable();
            G.Columns.Add("Numeris");
            G.Columns.Add("ValstybesNr");
            G.Columns.Add("Kilometrazas");
            G.Columns.Add("MasinosMarke");
            G.Columns.Add("Metai");
            G.Columns.Add("KuroTipas");

            for (int i = 0; i < Eil.Length; i++)
            {
                for (int j = 0; i < 6; j++ )
                    G.Rows.Add(Nr[i], ValstybNr[i], Kilometrazas[i], Modelis[i], Metai[i], KuroTipas[i]);
            }

                Gridas.DataContext = G.DefaultView;


        }
    }
}
