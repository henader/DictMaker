using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreakFinder
{
    class AriannaBreak
    {
        //*************************************************************ELEMENTY
        public string date { get; set; }
        public string startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public string channelID { get; set; }
        public string fieldTypology { get; set; }
        public long ID1 { get; set; }
        public long ID2 { get; set; }

        private string ZeroFormatHelper(string source)
        {
            if (source.Length <= 1)
                return "0" + source;
            else
                return source;
        }
        public AriannaBreak(Break sourceBreak)
        {            
            if ((sourceBreak.startTime <= TimeSpan.Parse("02:00:00"))&&(sourceBreak.date > DateTime.MinValue))
            {
                this.date = ZeroFormatHelper(sourceBreak.date.AddDays(-1).Day.ToString()) + @"/" + ZeroFormatHelper(sourceBreak.date.AddDays(-1).Month.ToString()) + @"/" + sourceBreak.date.AddDays(-1).Year.ToString();
                int outputHelper = sourceBreak.startTime.Hours * 10000 + sourceBreak.startTime.Minutes * 100 + sourceBreak.startTime.Seconds;
                outputHelper += 240000;
                string backToString = outputHelper.ToString().PadLeft(6, '0');
                this.startTime = backToString.Substring(0,2) + ":" + backToString.Substring(2,2) + ":" + backToString.Substring(4,2);
            }
            else
            {
                this.date = ZeroFormatHelper(sourceBreak.date.Day.ToString()) + @"/" + ZeroFormatHelper(sourceBreak.date.Month.ToString()) + @"/" + sourceBreak.date.Year.ToString();
                this.startTime = sourceBreak.startTime.ToString();
            }
            this.endTime = sourceBreak.endTime;
            switch (sourceBreak.channel)
            {
                case "TVP1":
                    this.channelID = "1101";
                    break;
                case "TVP2":
                    this.channelID = "1102";
                    break;
                case "TVP3":
                    this.channelID = "30008";
                    break;
                case "TV4":
                    this.channelID = "20050";
                    break;
                case "Polsat":
                    this.channelID = "1103";
                    break;
                case "TVN":
                    this.channelID = "2018";
                    break;
                case "TVN7":
                    this.channelID = "2024";
                    break;
                case "TVN 24":
                    this.channelID = "12146";
                    break;
                default:
                    this.channelID = "-99999";
                    break;
            }
            this.fieldTypology = "F";
            this.ID1 = sourceBreak.ID1;
            this.ID2 = sourceBreak.ID2;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(date);
            sb.Append(",");
            sb.Append(startTime);
            sb.Append(",");
            sb.Append(endTime);
            sb.Append(",");
            sb.Append(channelID);
            sb.Append(",");
            sb.Append(fieldTypology);
            return sb.ToString();
        }
    }
}
