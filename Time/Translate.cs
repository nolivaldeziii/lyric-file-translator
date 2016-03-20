﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* <<---HIDE THIS
 * Namespace:  DevCircuit
 * **************************************************************
 * Coded by
 *      Name: Noli Valdez III
 *     Email: devcircuit23@gmail.com
 *       URL:
 * **************************************************************
 *   Classes:  
 *             TranslateFromLRC : Translate
 *                  constructors:
 *                  private methods:
 *                  public methods:         
 *                  properties:
 *                  exceptions:
 *                      TranslateFailException
 *             Time
 *                  constructors:
 *                  private methods:
 *                  public methods:
 *                  properties:
 *                  exceptions:
 *                  
 *             Settings
 *                  private methods:
 *                  public methods:
 *                  properties:
 *                  exceptions:
 *                  
 *             Translate (abstract)
 *                  public methods(static):
 *                      int CalculateLineNumbers(string)
 *                      int CalculateLineNumbers(string[])
 *                      string[] ConvertToStringArray(string)
 *                  Abstracts:
 *                      string[] Append()
 *                      string[] Forward()
 *                      string[] Replace()
 *                      void GetLyric(string)
 *                      void GetLyric(string[])
 *                      void GetTranslation(string)
 *                      void GetTranslation(string[])
 *                      string[] ToLRC()
 *                      void CheckSum()
 *                      
 *             ITimeConvertible (interface)
 *             IConvertible (interface)
 *             
 *   Version:  1.0
 *   Website:  
 *   
 * ************************************************************
 * Project Started: November 15, 2012
 *     Modified on: 23 March 2014
 *     
 * ************************************************************
 * Thanks to:
 *  None so far
 *  
 * ************************************************************
 * Changelog:
 *  1.0.1 Changed Append to not add additionanl one second to translation
 *  
 * ************************************************************/
namespace DevCircuit
{
    public enum Format { lrc, srt, ass, unknown }
    public interface IConvertible
    {
        //string[] ToASS();
        //string[] ToSRT();
        string[] ToLRC();
    }
    public abstract class Translate
    {
        public abstract string[] Append();
        //public abstract string[] ReverseAppend();
        public abstract string[] Forward();
        //public abstract string[] ReverseForward();
        public abstract string[] Replace();

        public abstract void GetLyric(string x);
        public abstract void GetLyric(string[] x);
        public abstract void GetTranslation(string x);
        public abstract void GetTranslation(string[] x);

        public abstract void CheckSum();

        public static int CalulateLineNumbers(string[] x)
        {
            return x.Length;
        }
        public static int CalulateLineNumbers(string x)
        {
            return ConvertToStringArray(x).Length;
        }
        public static string[] ConvertToStringArray(string x)
        {
            List<string> tmp = new List<string>();
            string stor = "";
            for (int j = 0; j < x.Length; j++)
            {
                if (x[j] != '\n' /*|| x[j] != '\r'*/)
                {
                    stor += x[j];
                }
                else
                {
                    tmp.Add(stor);
                    stor = "";
                }
            }
            return tmp.ToArray();
        }
    }

    public class TranslateFromLRC:Translate , IConvertible
    {

        #region Declarations
        string[] Lyrics;
        string[] Translations;
        public string[] Result;
        int lyrcLines, translationLines;
        Time MyTime = new Time();
        public Settings MySettings = new Settings();
        #endregion

        //public TranslateFromLRC() { Lyrics = ""; Translations = ""; }
        public TranslateFromLRC(string lyrics, string translation)
        {
            Translations = ConvertToStringArray(translation);
            Lyrics = ConvertToStringArray(lyrics);
            CheckSum();
        }

        public override string[] Append()
        {
            List<string> output = new List<string>();
            //string[] LyricsHolder = ConvertToStringArray(Lyrics + "\n");
            //string[] TranslationHolder = ConvertToStringArray(Translations + "\n");
            string[] LyricsHolder = Lyrics;
            string[] TranslationHolder = Translations;

            string time = "";
            string lrc = "";

            for (int h = 0; h < LyricsHolder.Length; h++)
            {
                if (LyricsHolder[h] != "\r" && LyricsHolder[h] != "\n" && LyricsHolder[h] != "" && LyricsHolder[h] != null)
                {
                    for (int i = 0; i < LyricsHolder[h].Length; i++)
                    {
                        if (i < 10)
                        {
                            time += LyricsHolder[h][i];
                        }
                        else
                        {
                            lrc += LyricsHolder[h][i];
                        }
                    }
                    if (lrc != "" && lrc != "\r" && lrc != "\n" && lrc != null)
                    {
                        lrc = lrc.Trim('\r');
                        try
                        {
                            MyTime.ReadLRC(time);
                            output.Add(string.Format("{0}{1} [{2}]", time, lrc, TranslationHolder[h].Trim('\r')));
                            //output.Add(string.Format("{0}[{1}]", MyTime.ToLrcTimeFormat(), TranslationHolder[h].Trim('\r')));
                        }
                        catch (DevCircuit.Time.TimeEmptyException)
                        {
                            output.Add("\n");
                        }

                    }
                    else
                    {
                        output.Add(string.Format("{0}\r", time));
                    }

                    lrc = "";
                    time = "";
                }
                else
                {
                    output.Add("\r");
                }

            }

            Result = output.ToArray();

            return Result;
        }
        public override string[] Replace()
        {
            List<string> output = new List<string>();
            //string[] LyricsHolder = ConvertToStringArray(Lyrics + "\n");
            //string[] TranslationHolder = ConvertToStringArray(Translations + "\n");
            string[] LyricsHolder = Lyrics;
            string[] TranslationHolder = Translations;

            string time = "";
            string lrc = "";

            for (int h = 0; h < LyricsHolder.Length; h++)
            {
                if (LyricsHolder[h] != "\r" && LyricsHolder[h] != "\n" && LyricsHolder[h] != "" && LyricsHolder[h] != null)
                {
                    for (int i = 0; i < LyricsHolder[h].Length; i++)
                    {
                        if (i < 10)
                        {
                            time += LyricsHolder[h][i];
                        }
                        else
                        {
                            lrc += LyricsHolder[h][i];
                        }
                    }
                    if (lrc != "" && lrc != "\r" && lrc != "\n" && lrc != null)
                    {
                        lrc = lrc.Trim('\r');
                        try
                        {
                            MyTime.ReadLRC(time);
                            output.Add(string.Format("{0}{1}", time, TranslationHolder[h].Trim('\r')));
                        }
                        catch (DevCircuit.Time.TimeEmptyException)
                        {
                            output.Add("\n");
                        }

                    }
                    else
                    {
                        output.Add(string.Format("{0}\r", time));
                    }

                    lrc = "";
                    time = "";
                }
                else
                {
                    output.Add("\r");
                }

            }
            Result = output.ToArray();

            return Result;
        }
        public override string[] Forward()
        {
            List<string> output = new List<string>();
            string[] LyricsHolder = Lyrics;
            string[] TranslationHolder = Translations;

            string time = "";
            string lrc = "";

            for (int h = 0; h < LyricsHolder.Length; h++)
            {
                if (LyricsHolder[h] != "\r" && LyricsHolder[h] != "\n" && LyricsHolder[h] != "" && LyricsHolder[h] != null)
                {
                    for (int i = 0; i < LyricsHolder[h].Length; i++)
                    {
                        if (i < 10)
                        {
                            time += LyricsHolder[h][i];
                        }
                        else
                        {
                            lrc += LyricsHolder[h][i];
                        }
                    }
                    if (lrc != "" && lrc != "\r" && lrc != "\n" && lrc != null)
                    {
                        lrc = lrc.Trim('\r');
                        try
                        {
                            MyTime.ReadLRC(time);
                            //MyTime.AddSec(); // pwede palang magkasunod, tanggalin na muna to
                            if (string.Compare(lrc.Trim(),TranslationHolder[h].Trim('\r').Trim(),true) != 0)
                            {
                                output.Add(string.Format("{0}{1}", time, lrc));
                                if (TranslationHolder[h].Trim('\r') != " ") // ignore if translation is just a white space
                                {
                                    output.Add(string.Format("{0}[{1}]", MyTime.ToLRC(), TranslationHolder[h].Trim('\r')));
                                }
                            }
                            else
                            {
                                //if lrc is same with translation, it means lyrics is originally in enlgish,
                                // therefore no need to translate
                                output.Add(string.Format("{0}{1}", time, lrc));
                            }
                        }
                        catch (DevCircuit.Time.TimeEmptyException)
                        {
                            output.Add("\n");
                        }
                        
                    }
                    else
                    {
                        output.Add(string.Format("{0}\r", time));
                    }

                    lrc = "";
                    time = "";
                }
                else
                {
                    output.Add("\r");
                }

            }
            MyTime.AddSec(6);
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "."));
            MyTime.AddSec(1);
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "  ♫ ♫ end ♫ ♫  "));
            MyTime.AddSec(8);
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "konoakuta(at)gmail(dot)com"));
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "."));
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "."));
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "."));
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), "."));
            MyTime.AddSec();
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), " "));
            MyTime.AddSec();
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), " "));
            MyTime.AddSec();
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), " "));
            MyTime.AddSec();
            output.Add(string.Format("{0}{1}", MyTime.ToLRC(), " "));
            output.Add(" "); output.Add(" ");
            output.Add(" "); output.Add(" ");
       
            Result = output.ToArray();

            return Result;
        }

        public override void GetLyric(string x)
        {
            Lyrics = ConvertToStringArray(x);
        }
        public override void GetLyric(string[] x)
        {
            Lyrics = x;
        }
        public override void GetTranslation(string x)
        {
            Translations = ConvertToStringArray(x);
        }
        public override void GetTranslation(string[] x)
        {
            Translations = x;
        }

        #region TEMPORARY DISABLED
        //private static string[] ConvertToStringArray(string x)
        //{
        //    List<string> tmp = new List<string>();
        //    string stor = "";
        //    for (int j = 0; j < x.Length; j++)
        //    {
        //        if (x[j] != '\n' /*|| x[j] != '\r'*/)
        //        {
        //            stor += x[j];
        //        }
        //        else
        //        {
        //            tmp.Add(stor);
        //            stor = "";
        //        }
        //    }
        //    return tmp.ToArray();
        //}
        //public static int CalulateLineNumbers(string[] x)
        //{
        //    return x.Length;
        //}
        //public static int CalulateLineNumbers(string x)
        //{
        //    return ConvertToStringArray(x).Length;
        //}
        #endregion
        public int GetLyricLine { get { return lyrcLines; } }
        public int GetTranslationLine { get { return translationLines; } }

        public override void CheckSum()
        {
            string[] Lrc = Lyrics;
            string[] Trans = Translations;

            lyrcLines = Lrc.Length;
            translationLines = Trans.Length;

            if (Lrc.Length != Trans.Length)
            {
                if(!MySettings.IgnoreLineNumberCheck)
                    throw new TranslateFailException("[chksm]Line numbers does not match!");
            }

            for (int i = 0; i < Lrc.Length; i++)
            {
                if (Lrc[i][0] == '[' && (Trans[i][0] == '\r' || Trans[i][0] == '\n' || Trans[i] == ""))
                {
                    if(!MySettings.IgnoreEmptryTranslation)
                        throw new TranslateFailException(string.Format("[chksm]Missmatch: Lrc [line:{0}] has time while translation is empty", i + 1));
                }

                if ((Trans[i] != "" && Trans[i][0] != '\r' && Trans[i][0] != '\n') && (Lrc[i][0] == '\r' || Lrc[i][0] == '\n' || Lrc[i] == ""))
                {
                    if(!MySettings.IgnoreEmprtyLyrics)
                        throw new TranslateFailException(string.Format("[chksm]Missmatch: translation is empty for a lyric line: {0}", i + 1));
                }

            }
        }

        public string[] ToLRC()
        {
            throw new NotImplementedException("Cannot convert LRC to LRC");
        }

        public class TranslateFailException : Exception
        {
            public TranslateFailException() : base() { }
            public TranslateFailException(string Message) : base(Message) { }
        }
    }

    public interface ITimeConvertibe
    {
        string ToLRC();
        string ToASS();
        string ToSRT();

        void ReadLRC();
        void ReadASS();
        void ReadSRT();
    }
    public class Time
    {
        int minute, endMinute;
        double second, endSecond;
        public Time() { }
        public Time(int minutE, double sec)
        {
            minute = minutE;
            second = sec;
        }

        #region Time Mutators
        public void AddMinute(int minutes)
        {
            if (minutes + minute > 99)
            {
                throw new TimeException("[admn!]try to reduce the minutes");
            }
            minute += minutes;
        }
        public void AddMinute()
        {
            if (minute == 99)
            {
                throw new TimeException("[admn]minute is more than 99");
            }
            minute++;
        }
        /// <summary>
        /// Do not use this to add more than 59 secs, it will cause an erro
        /// use add minute instead
        /// </summary>
        /// <param name="seconds"></param>
        public void AddSec(double seconds)
        {
            if (seconds > 59)
            {
                throw new TimeException("[adsc]Do not use this to add more than 59 secs, it will cause an error");
                // use add minute instead
            }
            second += seconds;
            if (second >= 60.00)
            {
                minute++;
                second = second % 60.00;
            }
        }
        /// <summary>
        /// adds one second to the time
        /// </summary>
        public void AddSec()
        {
            second++;
            if (second >= 60.00)
            {
                minute++;
                second = second % 60.00;
            }
        }
        public void AddEndSec(double seconds)
        {
            if (seconds > 59)
            {
                throw new TimeException("[+sec]Do not use this to add more than 59 secs, it will cause an error");
                // use add minute instead
            }
            endSecond += seconds;
            if (second >= 60.00)
            {
                endMinute++;
                endSecond = endSecond % 60.00;
            }
        }
        #endregion

        public Format DetectTime(string source)
        {
            return Format.unknown;
        }
        /// <summary>
        /// Format must be "[mm:ss.ss]"
        /// </summary>
        /// <param name="source">[mm:ss.ss]</param>
        public void ReadLRC(string source)
        {
            if (source == "")
            {
                throw new TimeEmptyException("[rd]Time is empty");
            }
            if (source[0] != '[' || source[3] != ':' || source[6] != '.' || source[9] != ']')
            {
                throw new TimeFormatException("[rd]Time is not in correct format");
            }
            string tmp = source[1].ToString() + source[2].ToString();

            minute = Convert.ToInt32(tmp);
            tmp = source[4].ToString() + source[5].ToString() + source[6].ToString()
                + source[7].ToString() + source[8].ToString();
            second = Convert.ToDouble(tmp);

        }

        /// <summary>
        /// returns the string in lrc time format
        /// </summary>
        /// <returns>[mm:ss.ss]</returns>
        public string ToLRC()
        {
            string time;
            string m = "", s = "";

            if (minute < 10)
            {
                m = "0" + minute;
            }
            if (second < 10)
            {
                s = "0" + string.Format("{0:f2}", second);
            }
            else
            {
                s = string.Format("{0:f2}", second);
            }

            time = "[" + m + ":" + s + "]";

            return time;
        }

        #region Properties
        public int Minute 
        { 
            get { return minute; } 
            set 
            {
                if (value > 59)
                {
                    throw new TimeException("[minPROP]cannot set minute > 59");
                }
                minute = value; 
            } 
        }
        public double Second 
        { 
            get { return second; } 
            set 
            {
                if (value > 59)
                {
                    throw new TimeException("[secPROP]cannot set seconds > 59");
                }
                second = value; 
            } 
        }
        public int EndMinute { get { return endMinute; } set { endMinute = value; } }
        public double EndSecond { get { return endSecond; } set { endSecond = value; } }
        #endregion
        #region Exceptions
        public class TimeEmptyException : TimeException
        {
            public TimeEmptyException() : base() { }
            public TimeEmptyException(string Message) : base(Message) { }
        }
        public class TimeException : Exception
        {
            public TimeException() : base() { }
            public TimeException(string Message) : base(Message) { }
        }
        public class TimeFormatException : TimeException
        {
            public TimeFormatException() : base() { }
            public TimeFormatException(string Message) : base(Message) { }
        }
#endregion
    }

    public class Settings
    {
        bool IgnoreEmptyTranslation_ = false;
        bool IgnoreLineNumberCheck_ = false;
        bool IgnoreEmptyLyrics_ = false;
        bool WrapTranslation_ = true;
        char[] Wrapper_ = new char[2];

        public Settings()
        {
            Wrapper_[0] = '[';
            Wrapper_[1] = ']';

        }

        public bool IgnoreEmprtyLyrics
        {
            get { return IgnoreEmptyLyrics_; }
            set { IgnoreEmptyLyrics_ = value; }
        }
        public bool IgnoreEmptryTranslation 
        { get { return IgnoreEmptyTranslation_; } set { IgnoreEmptyTranslation_ = value; } }
        public bool IgnoreLineNumberCheck 
        { get { return IgnoreLineNumberCheck_; } set { IgnoreLineNumberCheck_ = value; } }
        public bool WrapTranslation 
        { get { return WrapTranslation_; } set { WrapTranslation_ = value; } }
        public char[] Wrapper 
        { get { return Wrapper_; } set { Wrapper_ = value; } }

        public void ReadSettings() 
        {
            if (!System.IO.File.Exists(".\\settings.ini"))
            {
                throw new SettingNotFoundException("Settings Not Found");
            }
            else
            {
                System.IO.TextReader ReadSettings = new System.IO.StreamReader(".\\settings.ini");
                ReadSettings.ReadLine();
                IgnoreEmptyLyrics_ = Convert.ToBoolean(ReadSettings.ReadLine());
                IgnoreEmptyTranslation_ = Convert.ToBoolean(ReadSettings.ReadLine());
                IgnoreLineNumberCheck_ = Convert.ToBoolean(ReadSettings.ReadLine());
                WrapTranslation_ = Convert.ToBoolean(ReadSettings.ReadLine());
                Wrapper_[0] = Convert.ToChar(ReadSettings.Read()); Wrapper_[1] = Convert.ToChar(ReadSettings.Read());
            }
        }
        public void WriteSettings() 
        {
            System.IO.TextWriter WriteSettings = new System.IO.StreamWriter(".\\settings.ini");

            WriteSettings.WriteLine("Settings: with great powers comes with great responsibility");
            WriteSettings.WriteLine(string.Format("{0}", IgnoreEmptyTranslation_));
            WriteSettings.WriteLine(string.Format("{0}", IgnoreLineNumberCheck_));
            WriteSettings.WriteLine(string.Format("{0}", IgnoreEmptyLyrics_));
            WriteSettings.WriteLine(string.Format("{0}", WrapTranslation_));
            WriteSettings.WriteLine(string.Format("{0}", Wrapper_.ToString()));

            WriteSettings.Close(); WriteSettings.Dispose();
        }

        public class SettingsException : Exception
        {
            public SettingsException() : base() { }
            public SettingsException(string Message) : base(Message) { }
        }
        public class SettingNotFoundException : SettingsException
        {
            public SettingNotFoundException() : base() { }
            public SettingNotFoundException(string Message) : base(Message) { }
        }
    }
}
