using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BreakFinder
{
    public class Break
    {
        //*************************************************************ELEMENTY
        public string channel { get; set; }
        public DateTime date { get; set; }
        public string startTimeString { get; set; }
        public TimeSpan startTime { get; set; }
        public TimeSpan endTime { get; set; }
        public TimeSpan duration { get; set; }
        public string description { get; set; }
        public string breakCode { get; set; }
        public long ID1 { get; set; }
        public long ID2 { get; set; }
        public TimeSpan diff { get; set; }
        public Break() { }

        //*************************************************************DANE NIELSENA
        public static List<Break> DataTableToListNielsen(DataTable dt)
        {
            int checker = 0;
            string pattern = "dd-MM-yyyy";
            List<Break> result = new List<Break>();
            try
            {
                long iteratorID1 = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Break tmpBreak = new Break();
                    tmpBreak.ID1 = iteratorID1;
                    iteratorID1++;
                    tmpBreak.channel = ChannelNielsenToAMOAD(dr["Channel"].ToString());
                    DateTime tmpDate;
                    if(DateTime.TryParseExact(dr["Date"].ToString(), pattern, null, DateTimeStyles.None, out tmpDate))
                        tmpBreak.date = tmpDate;
                    else
                        tmpBreak.date = DateTime.MinValue;
                    tmpBreak.description = dr["Programme"].ToString();
                    tmpBreak.startTimeString = dr["Start Time"].ToString();
                    //tmpBreak.startTime = TimeSpan.Parse(dr["Start time"].ToString());
                    TimeSpan tmpSpan;
                    if (TimeSpan.TryParseExact(dr["Start Time"].ToString(), "c", CultureInfo.CurrentCulture, out tmpSpan))
                        tmpBreak.startTime = tmpSpan;
                    else
                    {
                        tmpBreak.startTime = TimeSpan.Parse(tmpBreak.startTimeString.Split(' ')[1]);
                        tmpBreak.date = tmpBreak.date.AddDays(1); //********************************PRZERABIAMY DATĘ NA DZIEŃ KOLEJNY W CELU PORÓWNANIA!
                    }
                    if (TimeSpan.TryParseExact(dr["End Time"].ToString(), "c", CultureInfo.CurrentCulture, out tmpSpan))
                        tmpBreak.endTime = tmpSpan;
                    else
                    {
                        tmpBreak.endTime = TimeSpan.Parse(dr["End Time"].ToString().Split(' ')[1]);
                    }
                    if (tmpBreak.startTime != TimeSpan.MinValue)
                    {
                        tmpBreak.date += tmpBreak.startTime;
                        if (tmpBreak.endTime != TimeSpan.MinValue)
                        {
                            tmpBreak.duration = tmpBreak.endTime - tmpBreak.startTime;                            
                        }
                    }
                    result.Add(tmpBreak);
                    checker++;
                }
            }
            catch (Exception err1)
            {
                MessageBox.Show("Problem w linii: " + checker + " -> " + err1.ToString());
            }
            return result;
        }

        //*************************************************************UWSPÓLNIANIE NAZW STACJI
        private static string ChannelNielsenToAMOAD(string channel)
        {
            string result = channel;
            switch (channel)
            {
                case "TVN7 [RTL7]":
                    result = "TVN7";
                    break;
                case "TVP INFO [TVP3]":
                    result = "TVP3";
                    break;
                case "TVN24":
                    result = "TVN 24";
                    break;
                default:
                    break;
            }
            return result;
        }

        //*************************************************************DANE AMOAD
        public static List<Break> DataTableToListAMOAD(DataTable dt)
        {
            int checker = 0;
            string pattern = "yyyy.MM.dd";
            List<Break> result = new List<Break>();
            try
            {
                long iteratorID2 = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Break tmpBreak = new Break();
                    tmpBreak.ID2 = iteratorID2;
                    iteratorID2++;
                    tmpBreak.channel = dr["Channel"].ToString();
                    DateTime tmpDate;
                    if (DateTime.TryParseExact(dr["Date"].ToString(), pattern, null, DateTimeStyles.None, out tmpDate))
                        tmpBreak.date = tmpDate;
                    else
                        tmpBreak.date = DateTime.MinValue;
                    tmpBreak.description = dr["Program"].ToString();
                    tmpBreak.startTimeString = dr["Time"].ToString();
                    //tmpBreak.startTime = TimeSpan.Parse(dr["Time"].ToString());
                    TimeSpan tmpSpan;
                    if (TimeSpan.TryParseExact(dr["Time"].ToString(),"c", CultureInfo.CurrentCulture, out tmpSpan))
	                {
                        tmpBreak.startTime = tmpSpan;                        		 
	                }
                    else
                    {
                        //tmpBreak.startTime = TimeSpan.MinValue;
                        int tmpIntTime = Int32.Parse(dr["Time"].ToString().Replace(":", "")) ;
                        if (tmpIntTime >= 240000) // godzina powyżej północy
                        {
                            tmpIntTime -= 240000;
                            string backToString = tmpIntTime.ToString().PadLeft(6, '0');
                            TimeSpan tmpSkipSpan = new TimeSpan(Int32.Parse(backToString.Substring(0,2)), Int32.Parse(backToString.Substring(2,2)), Int32.Parse(backToString.Substring(4,2)));
                            tmpBreak.startTime = tmpSkipSpan;
                            tmpBreak.date = tmpBreak.date.AddDays(1); //********************************PRZERABIAMY DATĘ NA DZIEŃ KOLEJNY W CELU PORÓWNANIA!                            
                        }
                       
                    }
                    //tmpBreak.startTime = 
                    tmpBreak.breakCode = dr["Break Code"].ToString();
                    if (TimeSpan.TryParse("0:00:" + dr[@"Copy Length"].ToString(), out tmpSpan))
                        tmpBreak.duration = tmpSpan;
                    else
                        tmpBreak.duration = TimeSpan.MinValue;

                    if (tmpBreak.startTime != TimeSpan.MinValue)
                    {
                        tmpBreak.date += tmpBreak.startTime;
                        if (tmpBreak.duration != TimeSpan.MinValue)
                        {
                            tmpBreak.endTime = tmpBreak.startTime + tmpBreak.duration;                            
                        }                        
                    }
                    result.Add(tmpBreak);
                    checker++;
                }
            }
            catch (Exception err1)
            {
                MessageBox.Show("Problem w linii: " + checker + " -> " + err1.ToString());
            }
            return result;
        }

        //*************************************************************NORMALIZUJ OPISY
        public string NormalizeDescription()
        {
            return description.ToUpper();
        }

        //*************************************************************ToString()
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(channel);
            sb.Append("\n ");
            sb.Append(date);
            sb.Append("\n ");
            sb.Append(duration);
            sb.Append("\n ");
            sb.Append(description);
            return sb.ToString();
        }
    }
}
