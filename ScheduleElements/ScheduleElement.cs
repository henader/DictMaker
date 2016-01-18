using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictMaker.ScheduleElements
{
    class ScheduleElement
    {
        //*************************************************************ELEMENTY
        public string Stacja { get; set; }
        public DateTime Data { get; set; }
        //public string startTimeString { get; set; }
        public TimeSpan Godzina { get; set; }
        public string Opis { get; set; }
        public int Cena { get; set; }
        public string IDBloku { get; set; }
        public string Kod { get; set; }
        public long ID1 { get; set; }
        public long ID2 { get; set; }

        //*************************************************************KONSTRUKTORY
        public ScheduleElement() { }

        //*************************************************************POLSAT
        public ScheduleElement(ScheduleElementPolsat sep)
        {
            Stacja = "Polsat";
            Data = DateTime.Parse(sep.data.Split('|')[1]);
            Godzina = TimeSpan.Parse(sep.godzina);
            Opis = sep.program;
            Cena = Int32.Parse(sep.cena.Replace(" ", ""));
        }

        //*************************************************************PULS
        public ScheduleElement(ScheduleElementPuls sep)
        {
            Stacja = "Puls";
            Data = DateTime.Parse(sep.Data);
            Godzina = TimeSpan.Parse(sep.Godz);
            Opis = sep.Program;
            IDBloku = sep.IDBLOK;
            Cena = Int32.Parse(sep.Cena.Replace(" ", "").Replace("zł", ""));
        }

        //*************************************************************TVN
        public ScheduleElement(ScheduleElementTVNBP set)
        {
            Stacja = "TVN";
            Data = DateTime.Parse(set.Data);
            Godzina = TimeSpan.Parse(set.Godz);
            Opis = set.Nazwa;
            Kod = set.Kod;
            IDBloku = set.IDBLOK;
            Cena = Int32.Parse(set.Cena.Replace(" ", ""));
        }

        //*************************************************************TVN 7
        public ScheduleElement(ScheduleElementTVN7BP set7)
        {
            Stacja = "TVN 7";
            Data = DateTime.Parse(set7.Data);
            Godzina = TimeSpan.Parse(set7.Godz);
            Opis = set7.Blok_reklamowy;
            Kod = set7.Kod;
            IDBloku = set7.IDBLOK;
            Cena = Int32.Parse(set7.Cena.Replace(" ", ""));
        }

        //*************************************************************TV 4
        public ScheduleElement(ScheduleElementTV4 set4)
        {
            Stacja = "TV 4";
            Data = DateTime.Parse(set4.data);
            Godzina = TimeSpan.Parse(set4.godzina);
            Opis = set4.program;
            //breakCode = set4.Kod;
            //blockID = set4.IDBLOK;
            Cena = Int32.Parse(set4.cena.Replace(" ", ""));
        }

        //*************************************************************TVP
        public ScheduleElement(ScheduleElementTVP set)
        {
            Stacja = set.Kanal_TV;
            Data = DateTime.Parse(set.Data);
            Godzina = new TimeSpan(Int32.Parse(set.Godzina), Int32.Parse(set.Minuta), 0);
            Opis = set.Nazwa_programu;
            IDBloku = set.Id_bloku;
            Cena = Int32.Parse(set.Cena_do_30.Replace(" ", ""));
        }


        //*************************************************************ToString()
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Stacja);
            sb.Append("\n ");
            sb.Append(Data);
            sb.Append("\n ");
            sb.Append(Godzina);
            sb.Append("\n ");
            sb.Append(Opis);
            sb.Append("\n ");
            sb.Append(Cena);
            sb.Append("\n ");
            sb.Append(IDBloku);
            return sb.ToString();
        }
    }


}
