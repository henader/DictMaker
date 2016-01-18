using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictMaker.ScheduleElements
{
    class ScheduleElementTVN7BP
    {
        public string Data { get; set; }
        public string IDBLOK { get; set; }
        public string Kod { get; set; }
        public string Godz { get; set; }
        public string Blok_reklamowy { get; set; }
        public string powt { get; set; }
        public string Cena { get; set; }

        public ScheduleElementTVN7BP() { }

        public sealed class MyClassMap : CsvClassMap<ScheduleElementTVN7BP>
        {
            public MyClassMap()
            {
                Map(m => m.Data).Name("Data");
                Map(m => m.IDBLOK).Name("IDBLOK");
                Map(m => m.Kod).Name("Kod");
                Map(m => m.Godz).Name("Godz");
                Map(m => m.Blok_reklamowy).Name("Blok reklamowy");
                Map(m => m.powt).Name("powt.");
                Map(m => m.Cena).Name("Cena");
            }
        }
    }
}
