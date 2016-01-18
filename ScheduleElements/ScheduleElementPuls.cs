using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DictMaker.ScheduleElements
{
    class ScheduleElementPuls
    {
        //*************************************************************ELEMENTY
        public string Data { get; set; }
        public string IDBLOK { get; set; }
        public string Godz { get; set; }
        public string Program { get; set; }
        public string Cena { get; set; }
        
     
        public ScheduleElementPuls() { }
    }
}
