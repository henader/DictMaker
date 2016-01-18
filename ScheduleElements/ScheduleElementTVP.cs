using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictMaker.ScheduleElements
{
    class ScheduleElementTVP
    {
        public string Id_bloku { get; set; }
        public string Data { get; set; }
        public string Dzien_tyg { get; set; }
        public string Kanal_TV { get; set; }
        public string Godzina { get; set; }
        public string Minuta { get; set; }
        public string Usytuowanie_bloku { get; set; }
        public string Nazwa_programu { get; set; }
        public string Cena_do_30 { get; set; }

        public ScheduleElementTVP() { }

        public sealed class MyClassMap : CsvClassMap<ScheduleElementTVP>
        {
            public MyClassMap()
            {
                Map(m => m.Id_bloku).Name("Id bloku");
                Map(m => m.Data).Name("Data");
                Map(m => m.Dzien_tyg).Name("Dzień tyg.");
                Map(m => m.Kanal_TV).Name("Kanał TV");
                Map(m => m.Godzina).Name("Godzina");
                Map(m => m.Minuta).Name("Minuta");
                Map(m => m.Usytuowanie_bloku).Name("Usytuowanie bloku");
                Map(m => m.Nazwa_programu).Name("Nazwa programu");
                Map(m => m.Cena_do_30).Name("Cena do 30'' [zł.]");
            }
        }
    }
}
