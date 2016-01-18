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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Data;
using System.Reflection;
using Microsoft.Win32;
using DictMaker.ScheduleElements;
using CsvHelper;

namespace BreakFinder
{
    public partial class MainWindow : Window
    {
        const string succesfulLoad = "Załadowano ";
        const string errorLoad = "Błąd w pliku ";

        private int columnFinder = 2; //szukamy w dół gdzie aczynają się dane. Robimy to sprawdzając jak długo ta kolumna jest pusta
        private const int lostCause = 1024; //Kiedy przestać szukać
        List<Break> breakListAMOAD = new List<Break>();
        List<Break> breakListNielsen = new List<Break>();
        List<Break> matchedList = new List<Break>();
        List<AriannaBreak> ariannaList = new List<AriannaBreak>();
        List<ScheduleElement> scheduleElementList = new List<ScheduleElement>();
        List<ScheduleElementPuls> scheduleElementListPuls = new List<ScheduleElementPuls>();
        List<ScheduleElementPolsat> scheduleElementListPolsat = new List<ScheduleElementPolsat>();
        List<ScheduleElementTV4> scheduleElementListTV4 = new List<ScheduleElementTV4>();
        List<ScheduleElementTVNBP> scheduleElementListTVNBP = new List<ScheduleElementTVNBP>();
        List<ScheduleElementTVN7BP> scheduleElementListTVN7BP = new List<ScheduleElementTVN7BP>();
        List<ScheduleElementTVP> scheduleElementListTVP = new List<ScheduleElementTVP>();

        OpenFileDialog openFileDialog = new OpenFileDialog();
        public MainWindow()
        {
            InitializeComponent();
            openFileDialog.DefaultExt = "*.xlsx";
            openFileDialog.Filter = "CSV (*.csv)|*.csv|Dokumenty Excela (*.xlsx)|*.xlsx|Dokumenty Excela 2003 (*.xls)|*.xls";

        }

        //*************************************************************GUZIK AMOAD
        private void buttonLoadAMOAD_Click(object sender, RoutedEventArgs e)
        {
            columnFinder = 32;
            breakListAMOAD = new List<Break>();
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    breakListAMOAD = Break.DataTableToListAMOAD(loadDataFromExcel(openFileDialog.FileName));
                    //dataGridSchedules.ItemsSource = breakListAMOAD;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Nie udało się załadować kampanii AMOAD!\n" + err.ToString());
            }
            
        }

        //*************************************************************GUZIK NIELSEN
        private void buttonLoadNielsen_Click(object sender, RoutedEventArgs e)
        {
            columnFinder = 6;
            breakListNielsen = new List<Break>();
            try
            {
                if (openFileDialog.ShowDialog() == true)
                {
                    breakListNielsen = Break.DataTableToListNielsen(loadDataFromExcel(openFileDialog.FileName));
                    dataGridNielsen.ItemsSource = breakListNielsen;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("Nie udało się załadować kampanii Nielsena!\n" + err.ToString());
            }
            //dataGridNielsen.ItemsSource = Break.DataTableToListNielsen(loadDataFromExcel(@".\ramówka do briefu 9.xlsx"));
            
        }


        //*************************************************************UNIWERSALNE ŁADOWANIE LIST SPOTÓW
        private List<T> UniversalListLoad<T>()
        {
            List<T> res = new List<T>();
            bool? result = openFileDialog.ShowDialog();
            try
            {
                if (result == true)
                {
                    var csvReader = new CsvReader(new StreamReader(openFileDialog.FileName));
                    csvReader.Configuration.Delimiter = "\t";
                    Type checkType = typeof(T);
                    if (checkType == typeof(ScheduleElementTV4))
                    {
                        csvReader.Configuration.RegisterClassMap<ScheduleElementTV4.MyClassMap>();
                    }
                    if (checkType == typeof(ScheduleElementPolsat))
                    {
                        csvReader.Configuration.RegisterClassMap<ScheduleElementPolsat.MyClassMap>();
                    }
                    if (checkType == typeof(ScheduleElementTV4))
                    {
                        csvReader.Configuration.RegisterClassMap<ScheduleElementTV4.MyClassMap>();
                    }
                    if (checkType == typeof(ScheduleElementTVP))
                    {
                        csvReader.Configuration.RegisterClassMap<ScheduleElementTVP.MyClassMap>();
                    }
                    if (checkType == typeof(ScheduleElementTVN7BP))
                    {
                        csvReader.Configuration.RegisterClassMap<ScheduleElementTVN7BP.MyClassMap>();
                    }
                    while (csvReader.Read())
                    {
                        res.Add(csvReader.GetRecord<T>());
                    }
                }
                else
                {
                    MessageBox.Show("Proszę wybrać poprawny plik");
                }
            }
            catch (Exception errLoad)
            {
                MessageBox.Show("Nie udało się załadować pliku z powodu\n" + errLoad.ToString());
                //throw;
            }

            return res;
        }

        //*************************************************************REBUILD LIST
        private List<ScheduleElement> RebuildList()
        {
            List<ScheduleElement> masterList = new List<ScheduleElement>();
            foreach (ScheduleElementTVP set in scheduleElementListTVP)
            {
                masterList.Add(new ScheduleElement(set));
            }
            foreach (ScheduleElementPolsat sep in scheduleElementListPolsat)
            {
                masterList.Add(new ScheduleElement(sep));
            }
            foreach (ScheduleElementTV4 set4 in scheduleElementListTV4)
            {
                masterList.Add(new ScheduleElement(set4));
            }
            foreach (ScheduleElementTVNBP set in scheduleElementListTVNBP)
            {
                masterList.Add(new ScheduleElement(set));
            }
            foreach (ScheduleElementTVN7BP set7 in scheduleElementListTVN7BP)
            {
                masterList.Add(new ScheduleElement(set7));
            }
            foreach (ScheduleElementPuls sep in scheduleElementListPuls)
            {
                masterList.Add(new ScheduleElement(sep));
            }


            return masterList;
        }

        //*************************************************************GUZIK PULS
        private void buttonLoadPuls_Click(object sender, RoutedEventArgs e)
        {
            scheduleElementListPuls = UniversalListLoad<ScheduleElementPuls>();
            if (scheduleElementListPuls.Count > 0)
            {
                scheduleElementList = RebuildList();
                dataGridSchedules.ItemsSource = scheduleElementList;
                textBlockPuls.Background = new SolidColorBrush(Colors.Green);
                textBlockPuls.Text = succesfulLoad + openFileDialog.SafeFileName;
            }
            else
            {
                textBlockPuls.Background = new SolidColorBrush(Colors.Red);
                textBlockPuls.Text = errorLoad + openFileDialog.SafeFileName;
            }
            
        }

        //*************************************************************GUZIK POLSAT
        private void buttonLoadPolsat_Click(object sender, RoutedEventArgs e)
        {
            scheduleElementListPolsat = UniversalListLoad<ScheduleElementPolsat>();
            if (scheduleElementListPolsat.Count > 0)
            {
                scheduleElementList = RebuildList();
                dataGridSchedules.ItemsSource = scheduleElementList;
                textBlockPolsat.Background = new SolidColorBrush(Colors.Green);
                textBlockPolsat.Text = succesfulLoad + openFileDialog.SafeFileName;
            }
            else
            {
                textBlockPolsat.Background = new SolidColorBrush(Colors.Red);
                textBlockPolsat.Text = errorLoad + openFileDialog.SafeFileName;
            }
            
        }

        //*************************************************************GUZIK TV4
        private void buttonLoadTV4_Click(object sender, RoutedEventArgs e)
        {
            scheduleElementListTV4 = UniversalListLoad<ScheduleElementTV4>();
            if (scheduleElementListTV4.Count > 0)
            {
                scheduleElementList = RebuildList();
                dataGridSchedules.ItemsSource = scheduleElementList;
                textBlockTV4.Background = new SolidColorBrush(Colors.Green);
                textBlockTV4.Text = succesfulLoad + openFileDialog.SafeFileName;
            }
            else
            {
                textBlockTV4.Background = new SolidColorBrush(Colors.Red);
                textBlockTV4.Text = errorLoad + openFileDialog.SafeFileName;
            }
            
        }

        //*************************************************************GUZIK TVN
        private void buttonLoadTVNBP_Click(object sender, RoutedEventArgs e)
        {
            scheduleElementListTVNBP = UniversalListLoad<ScheduleElementTVNBP>();
            if (scheduleElementListTVNBP.Count > 0)
            {
                scheduleElementList = RebuildList();
                dataGridSchedules.ItemsSource = scheduleElementList;
                textBlockTVNBP.Background = new SolidColorBrush(Colors.Green);
                textBlockTVNBP.Text = succesfulLoad + openFileDialog.SafeFileName;
            }
            else
            {
                textBlockTVNBP.Background = new SolidColorBrush(Colors.Red);
                textBlockTVNBP.Text = errorLoad + openFileDialog.SafeFileName;
            }
            
        }

        //*************************************************************GUZIK TVN7
        private void buttonLoadTVN7BP_Click(object sender, RoutedEventArgs e)
        {
            scheduleElementListTVN7BP = UniversalListLoad<ScheduleElementTVN7BP>();
            if (scheduleElementListTVN7BP.Count > 0)
            {
                scheduleElementList = RebuildList();
                dataGridSchedules.ItemsSource = scheduleElementList;
                textBlockTVN7BP.Background = new SolidColorBrush(Colors.Green);
                textBlockTVN7BP.Text = succesfulLoad + openFileDialog.SafeFileName;
            }
            else
            {
                textBlockTVN7BP.Background = new SolidColorBrush(Colors.Red);
                textBlockTVN7BP.Text = errorLoad + openFileDialog.SafeFileName;
            }
            
        }

        //*************************************************************GUZIK TVP
        private void buttonLoadTVP_Click(object sender, RoutedEventArgs e)
        {
            scheduleElementListTVP = UniversalListLoad<ScheduleElementTVP>();
            if (scheduleElementListTVP.Count > 0)
            {
                scheduleElementList = RebuildList();
                dataGridSchedules.ItemsSource = scheduleElementList;
                textBlockTVP.Background = new SolidColorBrush(Colors.Green);
                textBlockTVP.Text = succesfulLoad + openFileDialog.SafeFileName;
            }
            else
            {
                textBlockTVP.Background = new SolidColorBrush(Colors.Red);
                textBlockTVP.Text = errorLoad + openFileDialog.SafeFileName;
            }
        }

        private void LoadScheduleList(ScheduleElement schedElement)
        {

        }

        //*************************************************************ŁADOWANIE DANYCH Z EXCELA
        private DataTable loadDataFromExcel(string path)
        {            
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                ExcelWorksheet ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                bool hasHeader = true;
                int headerFinderRow = 1;
                while ((ws.Cells[headerFinderRow,columnFinder].Text.Equals(""))&&(lostCause >= headerFinderRow))
                {
                    headerFinderRow++;
                }
                int headerFinderColumn = 1;
                while ((ws.Cells[headerFinderRow, headerFinderColumn].Text.Equals("")) && (lostCause >= headerFinderColumn))
                {
                    headerFinderColumn++;
                }
                for (int i = headerFinderColumn; i <= ws.Dimension.End.Column; i++)
                {
                    tbl.Columns.Add(hasHeader ? ws.Cells[headerFinderRow, i].Value.ToString() : string.Format("Column {0}", ws.Cells[headerFinderRow, i].Start.Column));                    
                }
                int startRow = hasHeader ? 1:0;
                for (int rowNum = headerFinderRow + startRow ; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    ExcelRange wsRow = ws.Cells[rowNum, ws.Dimension.Start.Column, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.NewRow();

                    foreach (ExcelRangeBase cell in wsRow)                    
                    {
                        if ((cell.Text == "01") || (cell.Text == "12"))
                        {
                            row[cell.Start.Column - ws.Dimension.Start.Column] = cell.Value.ToString();
                            //row[cell.Start.Column - ws.Dimension.Start.Column] = cell.Text;
                        }
                        else
                        {
                            row[cell.Start.Column - ws.Dimension.Start.Column] = cell.Text;
                        }
                    }
                    tbl.Rows.Add(row.ItemArray);
                }
                return tbl;                
            }            
        }

        //*************************************************************STYLE KOLUMN NIELSENA
        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyType == typeof(DateTime))
            {
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyy HH:mm:ss";
            }
        }

        //*************************************************************DOPASUJ PRZERWĘ
        public void buttonCalculateResult_Click(object sender, RoutedEventArgs e)
        {
            if (breakListAMOAD.Count == 0)
            {
                buttonLoadAMOAD_Click(this,e);                
            }
            if (breakListNielsen.Count == 0)
            {
                buttonLoadNielsen_Click(this,e);
            }
            if (dataGridSchedules.SelectedItem == null)
            {
                dataGridSchedules.SelectedIndex = 0;                
            }
            Break selectedBreak = (Break)dataGridSchedules.SelectedItem;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(selectedBreak.ToString());
            sb.AppendLine(NextNielsen(selectedBreak).ToString());
            //MessageBox.Show(sb.ToString());
            matchedList = new List<Break>();
            foreach (Break currentBreak in breakListAMOAD)
            {
                Break matchedBreak = NextNielsen(currentBreak);
                matchedBreak.ID2 = currentBreak.ID2;
                matchedBreak.diff = matchedBreak.startTime - currentBreak.startTime;
                matchedList.Add(matchedBreak);                
            }
            dataGridResult.ItemsSource = matchedList;
            ariannaList = new List<AriannaBreak>();
            foreach (Break br in matchedList)
            {
                AriannaBreak arb = new AriannaBreak(br);
                ariannaList.Add(arb);
            }
            //dataGridArianna.ItemsSource = ariannaList;
        }

        //*************************************************************NASTĘPNY BREAK Z NIELSENA
        public Break NextNielsen(Break refBreak)
        {
            Break result;
            IEnumerable<Break> tmpNielsen = breakListNielsen.Where<BreakFinder.Break>(br => ((br.date >= refBreak.date.AddMinutes(-10)) && (br.date <= refBreak.date.AddMinutes(60)) && (br.channel == refBreak.channel)));
            long tmpMin = long.MaxValue;
            int iterator = 0;
            bool gettingBetter = true;
            //MessageBox.Show(tmpNielsen.Count().ToString());
            while ((gettingBetter)&&(iterator < tmpNielsen.Count()))
            {
                if (Math.Abs(tmpNielsen.ElementAt(iterator).date.Ticks - refBreak.date.Ticks) < tmpMin)
                {
                    tmpMin = Math.Abs((tmpNielsen.ElementAt(iterator).date.Ticks - refBreak.date.Ticks));
                    iterator++;
                }
                else
                {
                    gettingBetter = false;
                }
            }
            if (!tmpNielsen.Equals(null)&&(tmpNielsen.Count()>0))
            {
                result = tmpNielsen.ElementAt(iterator - 1);                
            }
            else
            {
                result = new Break();
                result.channel = refBreak.channel;         
            }
            return result;
        }

        //*************************************************************ZAPISANIE WYNIKU DO ARIANNOWEGO FORMATU
        private void buttonCalculateAriannaOutput_Click(object sender, RoutedEventArgs e)
        {
        
            SaveFileDialog saveFD = new SaveFileDialog();
            saveFD.FileName = "ArainnaOutput.csv";
            saveFD.DefaultExt = ".csv";
            saveFD.Filter = "Pliki CSV (.csv)|*.csv";
            Nullable<bool> result = saveFD.ShowDialog();
            if (result == true)
            {
                string filename = saveFD.FileName;
                StreamWriter sw = new StreamWriter(filename);
                sw.WriteLine("DATE,STARTTIME,ENDTIME,CHANNELID,FIELDTYPOLOGY");
                foreach (AriannaBreak arbreak in ariannaList)
                {
                    sw.WriteLine(arbreak.ToString());                                       
                }
                sw.Close();
            }
            else
            {
                MessageBox.Show("Wybierz poprawną nazwę pliku!");
            }

        }

        private void dataGridResult_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dg = (DataGrid)sender;
            Break selectedBreakAMOAD = (Break)dg.SelectedItem;
            try
            {
                dataGridSchedules.SelectedItems.Clear();
                dataGridNielsen.SelectedItems.Clear();
                //dataGridArianna.SelectedItems.Clear();

                dataGridSchedules.SelectedItem = breakListAMOAD.Find(br => (br.ID2 == selectedBreakAMOAD.ID2));
                dataGridNielsen.SelectedItem = breakListNielsen.Find(br => (br.ID1 == selectedBreakAMOAD.ID1));
                //dataGridArianna.SelectedItem = ariannaList.Find(br => (br.ID1 == selectedBreakAMOAD.ID1));

                dataGridSchedules.ScrollIntoView(dataGridSchedules.SelectedItem);
                dataGridNielsen.ScrollIntoView(dataGridNielsen.SelectedItem);
                //dataGridArianna.ScrollIntoView(dataGridArianna.SelectedItem);

            }
            catch (Exception err1)
            {
                MessageBox.Show(err1.ToString());
                //throw;
            }
            
            
        }

       
    }
}
