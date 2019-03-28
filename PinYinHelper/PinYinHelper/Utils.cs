using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PinYinHelper
{
    public class Utils
    {

        private static string[] initialmeta = { "b", "p", "m", "f", "d", "t", "l", "k", "j", "q", "x", "z", "c", "s", "zh", "ch", "sh", "h", "y", "w" };
        private static string[] finalmeta = { "ai", "ao", "an", "ou", "ei", "en", "er", "a", "o", "e" };
        private static string[] finalmetaFull = {"i" + "u", "v", "ia", "ua", "ie", "ve", "uo", "uai", "uei", "iao", "iou",
            "ian", "uan", "van", "in", "uen", "vn", "ang", "iang", "uang", "eng", "ing", "ueng", "ong", "iong"};

        private static string[] voicemark_a = { "ā", "á", "ǎ", "à" };
        private static string[] voicemark_o = { "ō", "ó", "ǒ", "ò" };
        private static string[] voicemark_e = { "ē", "é", "ě", "è" };
        private static string[] voicemark_i = { "ī", "í", "ǐ", "ì" };
        private static string[] voicemark_u = { "ū", "ú", "ǔ", "ù" };
        private static string[] voicemark_v = { "ǖ", "ǘ", "ǚ", "ǜ", "ü" };

        public static string splitPinyin(string str)
        {
            StringBuilder sb = new StringBuilder(str);
            String after = str.Substring(1, str.Length - 1);
            int tempVoicemeta;
            if (startWithFinal(str) != 0)
            {
                tempVoicemeta = startWithFinal(str);
            }
            else
            {
                tempVoicemeta = containsInitial(after);
            }
            if (tempVoicemeta != 0)
            {
                sb.Insert(tempVoicemeta, " ");
            }
           
            return sb.ToString();
        }

        private static int containsInitial(string str)
        {
            string metaStr = formatMeta(str);
            foreach (var word in metaStr.Substring(1))
            {
                foreach (var item in initialmeta)
                {
                    if (word.ToString().Contains(item))
                        return metaStr.Substring(1).IndexOf(item) + 2;
                }
            }
            //foreach (var item in initialmeta)
            //{
            //    if (metaStr.ToString().Contains(item))
            //        return metaStr.Substring(1).IndexOf(item) + 2;
            //}
            if (metaStr.Contains("r"))
            {
                if (metaStr.EndsWith("er"))
                {
                    return metaStr.Length - 1;
                }
                if (metaStr.Contains("er") && !metaStr.StartsWith("er"))
                {
                    // er en 传入r en ，r属于word 1
                    //返回r的后一个字母
                    return metaStr.IndexOf("r") + 2;
                }
                else
                {
                    // he ren传入e ren，r属于word 2.
                    return metaStr.IndexOf("r") + 1;
                }
            }
            if (str.Contains("gn"))
            {
                return str.IndexOf("gn") + 2;
            }
            if (str.Contains("g"))
            {
                if (str.Contains("gg"))
                {
                    return str.IndexOf("gg") + 2;
                }
                else
                {
                    String before = str.Substring(0, str.IndexOf("g"));
                    String after = str.Substring(str.IndexOf("g") + 1);
                    
                    if (after.Length > 0)
                    {
                        //如果g之后不能独立成拼音 g属于后面
                        if (equalsFinal(after))
                        {
                            return str.IndexOf("g") + 1;
                        }
                        ////如果g之后能独立成拼音 g属于前面  新增20190328
                        //if (finalmeta.Contains(after))
                        //{
                        //    return str.IndexOf("g") + 2;
                        //}//新增20190328
                    }

                    //如果 before 不以n结尾 避免了"ng"，可以g分割
                    if (!before.EndsWith("n"))
                    {
                        return str.IndexOf("g") + 1;
                    }
                    //如果没有 ng  且 before的长度大于1
                    if (!str.Contains("ng"))
                    {
                        if (!before.StartsWith("i") && before.Length > 1)
                        {
                            return str.IndexOf("g") + 1;
                        }
                        if (before.StartsWith("i") && before.Length > 2)
                        {
                            return str.IndexOf("g") + 1;
                        }
                    }
                }
            }
            if (str.Contains("n"))
            {
                if (str.Contains("nn"))
                {
                    return str.IndexOf("nn") + 2;
                }
                else if (str.IndexOf("n") != str.Length - 1)
                {
                    String before = str.Substring(0, str.IndexOf("n"));
                    String after = str.Substring(str.IndexOf("n") + 1);
                    //如果n之后不能独立成拼音 n属于后面
                    if (after.Length > 0)
                    {
                        if (equalsFullFinal(after))
                        {
                            return str.IndexOf("n") + 1;
                        }
                    }

                    if (!before.StartsWith("i") && before.Length > 1)
                    {
                        return str.IndexOf("n") + 1;
                    }
                    if (before.StartsWith("i") && before.Length > 2)
                    {
                        return str.IndexOf("n") + 1;
                    }
                }
            }
            String middle = str.Substring(0, str.Length - 1);
            if (!middle.Contains("n") && !middle.Contains("g"))
            {
                return containsFinal(str);
            }
            return 0;
        }

        public static int startWithFinal(string str)
        {
            String meta = formatMeta(str);
            foreach (var item in finalmeta)
            {
                if (meta.Contains(item) && meta.StartsWith(item))
                {
                    return item.Length;
                }
            }
            return 0;
        }

        private static int containsFinal(string str)
        {
            String meta = formatMeta(str);
            foreach (var item in finalmeta)
            {
                if (meta.Contains(item) && meta.EndsWith(item))
                {
                    return meta.IndexOf(item) + 1;
                }
            }
            return 0;
        }
        private static bool equalsFinal(string str)
        {
            String meta = formatMeta(str);
            foreach (var item in finalmeta)
            {
                if (meta.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        private static bool equalsFullFinal(string str)
        {
            String meta = formatMeta(str);
            foreach (var item in finalmetaFull)
            {
                if (meta.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }
        private static string formatMeta(string str)
        {
            foreach (var item in voicemark_a)
            {
                while (str.Contains(item))
                {
                    str = str.Substring(0, str.IndexOf(item)) + "a" + str.Substring(str.IndexOf(item) + 1);
                }
            }
            foreach (var item in voicemark_o)
            {
                while (str.Contains(item))
                {
                    str = str.Substring(0, str.IndexOf(item)) + "o" + str.Substring(str.IndexOf(item) + 1);
                }
            }
            foreach (var item in voicemark_e)
            {
                while (str.Contains(item))
                {
                    str = str.Substring(0, str.IndexOf(item)) + "e" + str.Substring(str.IndexOf(item) + 1);
                }
            }
            foreach (var item in voicemark_i)
            {
                while (str.Contains(item))
                {
                    str = str.Substring(0, str.IndexOf(item)) + "i" + str.Substring(str.IndexOf(item) + 1);
                }
            }
            foreach (var item in voicemark_u)
            {
                while (str.Contains(item))
                {
                    str = str.Substring(0, str.IndexOf(item)) + "u" + str.Substring(str.IndexOf(item) + 1);
                }
            }
            foreach (var item in voicemark_v)
            {
                while (str.Contains(item))
                {
                    str = str.Substring(0, str.IndexOf(item)) + "v" + str.Substring(str.IndexOf(item) + 1);
                }
            }
            return str;
        }
    }
}
