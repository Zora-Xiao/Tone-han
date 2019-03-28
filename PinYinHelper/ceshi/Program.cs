using PinYinHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ceshi
{
    class Program
    {
        static void Main(string[] args)
        {
            // ReadTxt();
            while (true)
            {
                string pinyin = Console.ReadLine();
                foreach (var item in pinyin)
                {
                    //string[] fonts = FontsMap.GetFonts(pinyin);
                    string[] pinyis = FontsMap.GetOnlyPinYin(item);
                    foreach (var items in pinyis)
                    {
                        Console.WriteLine(items);
                    }
                    Console.WriteLine(pinyis.Length.ToString());
                    Console.WriteLine();
                }
                
            }
            Console.ReadKey();
        }

        public static void ReadTxt()
        {
            Dictionary<string, string> Phrase = new Dictionary<string, string>();
            Dictionary<string, string> Phrase2 = new Dictionary<string, string>();
            string pathUrl = @"D:\RayData\ToolKit\Data\Font\Phrase.txt";
            string pathUrl2 = @"E:\DownLoad\数学词汇大全【官方推荐】.txt";
            string contents = File.ReadAllText(pathUrl);
            string contents2 = File.ReadAllText(pathUrl2);
            contents = contents.Replace("\r\n", " ");
            contents2 = contents2.Replace("\r\n", " ");
            string[] fonts = contents.Split(' ');
            string[] fonts2 = contents2.Split(' ');
            for (int i = 0; i < fonts.Length - 1; i = i + 2)
            {
                Phrase.Add(fonts[i] + " " + fonts[i + 1], fonts[i]);
            }
            for (int i = 0; i < fonts2.Length - 1; i = i + 2)
            {
                Phrase2.Add(fonts2[i] + " " + fonts2[i + 1], fonts2[i]);
            }
            foreach (string key in Phrase.Keys)
            {
                if (!Phrase2.ContainsKey(key))
                    Phrase2.Add(key, Phrase[key]);
            }
            using (StreamWriter writer = new StreamWriter(@"D:\RayData\ToolKit\Data\Font\Phrase - 副本.txt", true))
            {
                foreach (var item in Phrase2)
                {
                    writer.WriteLine(item.Key);
                }
            }
        }

       
        private static bool changed; private static int indexs = 1; private static int falge = 1,InputIndex=1;
        private static List<string> SelectFont = new List<string>();//选中的汉字
        private static List<string> Fonts = new List<string>();//返回全部的汉字
        private static List<string> Cfont = new List<string>();//记录拼音输入
        private static List<string> Sfont = new List<string>();//输入框中的汉字
        private static List<string> PinYinChanged = new List<string>();//记录拼音
        private static int PinyinCount = 0;//拼音的长度
        private static string RemberPinyin = "";//纪录需要添加的词库拼音
        private static int fontCount = 0;//字的长度
        private static Dictionary<string, string> Phrase1 = new Dictionary<string, string>();//词库
        public static void PinYinHelper()
        {
            string InputSrtValue = Console.ReadLine();
            if (InputSrtValue == "") { return; }
            int falge = 0;//是否显示输入框
            int asciicode1 = 0;
            string strPinYin = InputSrtValue;//输入内容
            if (strPinYin != "" && strPinYin != null) { byte[] array = new byte[1]; array = System.Text.Encoding.ASCII.GetBytes(strPinYin); asciicode1 = (short)(array[0]); }////把str的每个字符转换成ascii码
            string fontContent = "";
            string strFont = "";
            string PinYins = "";
            int index = 1;//汉字的下标
            int count = 1;//页码
            int fontIndex = 0;//判断获取第几个汉字
            string fonstr = "";//选中文字
            string fonstrFlage = "";//纪录选中文字
            List<string> fonts = new List<string>();
            if (true)
            {
                string reg = "[a-z]";
                Regex rx = new Regex(reg);
                if (!rx.IsMatch(strPinYin) && strPinYin != "" && strPinYin != null)//判断不为字母；
                {
                    if (Cfont.Count > 0) { falge = 1; }
                    if (Sfont.Count > 0)
                    {
                        if (asciicode1 == 10)
                        {
                            foreach (var item in Cfont)
                            {
                                SelectFont.Add(item);
                            }
                            strFont = "";
                            Cfont.Clear();
                            Fonts.Clear();
                            Sfont.Clear();
                        }
                        else
                        {
                            string regNum = "[1-5]";
                            Regex rxNum = new Regex(regNum);
                            if (rxNum.IsMatch(strPinYin))
                            {
                                int indexSelect = Convert.ToInt32(strPinYin);
                                if (indexSelect <= Sfont.Count)
                                {//判断数据是否在数字中
                                    fonstr = Sfont[indexSelect - 1].Trim();//选中文字
                                    if (fonstr.Length != PinYinChanged.Count())
                                    {//判断选择文字长度是否和拼音的长度一直
                                        Cfont.Clear();
                                        SelectFont.Add(fonstr);
                                        Cfont.Add(fonstr);
                                        PinYinChanged.RemoveRange(0, fonstr.Length);
                                        foreach (var item in PinYinChanged)
                                        {
                                            Cfont.Add(item.ToString());
                                        }
                                    }
                                    else
                                    {
                                        SelectFont.Add(fonstr);
                                        strFont = "";
                                        Cfont.Clear();
                                        Fonts.Clear();
                                        Sfont.Clear();
                                        falge = 0;
                                    }
                                    //判断是否需要记录新词库
                                    if (PinyinCount != fonstr.Length)
                                    {
                                        if (falge == 0)
                                        {
                                            int longs = SelectFont.Count - PinyinCount;//截取记录的汉字的长度
                                            string remberfont = "";//记录汉字
                                            for (int i = longs; i < PinyinCount; i++)
                                            {
                                                remberfont += SelectFont[i];
                                            }
                                            string dickey = "";
                                            if (RemberPinyin != "" || remberfont != "") { dickey = RemberPinyin + "," + remberfont; }
                                            if (!Phrase1.ContainsKey(dickey))//判断字典中是否存在此词组
                                            {
                                                //using (StreamWriter writer = new StreamWriter(this.Scene.WorkingDirectory + pathUrl01, true))
                                                //{
                                                //    writer.WriteLine(RemberPinyin + " " + remberfont);
                                                //}
                                            }
                                            PinyinCount = 0;
                                            RemberPinyin = "";
                                            ////ReadTxt(1);
                                        }
                                    }
                                }
                            }
                            else if (strPinYin == " ")
                            {
                                fonstr = Sfont[0].Trim();//选中文字
                                SelectFont.Add(fonstr);
                                strFont = "";
                                Cfont.Clear();
                                Fonts.Clear();
                                Sfont.Clear();
                            }
                            else { changed = true; return; }
                        }
                    }
                    else
                    {
                        if (asciicode1 == 10) { return; }
                        SelectFont.Add(strPinYin);
                    }
                }//if 第二个
                else
                {

                    if (asciicode1 == 10) { return; }
                    if (strPinYin != "" && strPinYin != null)
                    {
                        Cfont.Add(strPinYin);
                        falge = 1;
                    }
                    foreach (var item in Cfont)
                    {
                        if (rx.IsMatch(item))
                        {
                            PinYins += item;
                        }
                    }
                    if (Cfont.Count > 0) { falge = 1; }
                    fonts = FontsMap.GetFonts(PinYins).ToList();//获取汉字
                    PinYinChanged = FontsMap.GetPinYins(PinYins).ToList();//记录拼音
                    PinyinCount = PinYinChanged.Count();//记录拼音个数
                    if (fonts.Count() > 0)
                    {
                        Sfont.Clear();
                        count = fonts.Count();
                        count = (count / 5) + ((count % 5) == 0 ? 0 : 1);//分成几组
                        if (InputIndex != 1) { index = (InputIndex - 1) * 5; }
                        if (fonts.Count < 5)
                        {
                            for (int i = 0; i < fonts.Count; i++)
                            {
                                strFont += (i + 1).ToString() + fonts[index - 1 + i] + " ";
                                Sfont.Add(fonts[index - 1 + i]);
                            }
                        }
                        else
                        {
                            if (InputIndex == count)
                            {
                                int s = fonts.Count - index;
                                for (int i = 0; i < s; i++)
                                {
                                    strFont += (i + 1).ToString() + fonts[index + i] + " ";
                                    Sfont.Add(fonts[index + i]);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    strFont += (i + 1).ToString() + fonts[index - 1 + i] + " ";
                                    Sfont.Add(fonts[index - 1 + i]);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                strFont = "";
                if (strPinYin != "" && strPinYin != null)
                {
                    if (asciicode1 == 10) { return; }
                    SelectFont.Add(strPinYin);
                }
            }
            foreach (var item in SelectFont)
            {
                fontContent += item;
            }
        }
    }
}
