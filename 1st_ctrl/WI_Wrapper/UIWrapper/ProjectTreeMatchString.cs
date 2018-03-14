using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
namespace WF1
{
    class ProjectTreeMatchString
    {
        public static ArrayList MatchStr(string res, string str, int n)
        {
            MatchCollection mc;
            Regex w = new Regex(str);
            string temp = null;
            ArrayList arr = new ArrayList();
            //在res查找与str匹配的所有项，存到MatchCollection中
            mc = w.Matches(res);
            for (int i = 0; i < mc.Count; i++)
            {
                //将匹配的字符串转存到String数组txt中
                //txt[i] = mc[i].Value;
                //记录匹配的字符位置
                //matchPosition[i] = mc[i].Index;
                int j;
                for (j = 0; res[mc[i].Index + n + 1 + j] != '\r'; j++)
                {
                    temp = temp + res[mc[i].Index + n + 1 + j];          
                }
                arr.Add(temp);
                temp = null;
            }
            return arr;
        }
    }
}
