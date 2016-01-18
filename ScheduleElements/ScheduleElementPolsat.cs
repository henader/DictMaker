using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictMaker.ScheduleElements
{
    class ScheduleElementPolsat
    {
        //*************************************************************ELEMENTY
        public string data { get; set; }
        public string typ_bloku { get; set; }
        public string pasmo { get; set; }
        public string godzina { get; set; }
        public string godz { get; set; }
        public string min { get; set; }
        public string cena { get; set; }
        public string program { get; set; }


        public ScheduleElementPolsat() { }
        public sealed class MyClassMap : CsvClassMap<ScheduleElementPolsat>
        {
            public MyClassMap()
            {
                Map(m => m.data).Name("data");
                Map(m => m.typ_bloku).Name("typ bloku");
                Map(m => m.pasmo).Name("pasmo");
                Map(m => m.pasmo).Name("pasmo");
                Map(m => m.godzina).Name("godzina");
                Map(m => m.godz).Name("godz");
                Map(m => m.min).Name("min");
                Map(m => m.cena).Name("cena");
                Map(m => m.program).Name("program");
            }
        }
    }
  
}
