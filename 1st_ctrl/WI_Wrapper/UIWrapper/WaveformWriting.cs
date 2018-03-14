using System;
using System.Text;
using System.Collections.Generic;

using System.IO;
using System.Collections;
using System.Text.RegularExpressions;


namespace WF1
{
    class WaveformWriting
    {
        string sourceString;
        //初始化字段sourcestring
        public WaveformWriting(string str)
        {
            sourceString = str;
        }
        //用正则表达式类找出匹配字符串的位置
        int[] IndexFun(string matchString)
        {
            int[] matchPosition = new int[25];
            MatchCollection mc;
            Regex r = new Regex(matchString);
            mc = r.Matches(sourceString);
            for (int i = 0; i < mc.Count; i++)
                matchPosition[i] = mc[i].Index;
            return matchPosition;
        }


        //找出关键字(关键字不能超过30个）后面的波形名字
        public string[] waveformNames(string matchString)
        {

            int[] matchSite = new int[30];
            matchSite = IndexFun(matchString);

            string[] wName = new string[30];
            //StringBuilder s = new StringBuilder();

            for (int i = 0; i < Array.IndexOf(matchSite, 0); i++)
            {
                //char[] ch = new char[30];
                StringBuilder sb = new StringBuilder();
                int j;
                for (j = 0; sourceString[matchSite[i] + j + matchString.Length + 1] != '\r'; j++)
                {
                    //temp=temp+ sourceString[matchSite[i] + j + matchString.Length+1];
                    sb = sb.Append(sourceString[matchSite[i] + j + matchString.Length + 1]);
                }
                wName[i] = sb.ToString();
            }
            return wName;
        }


        //判断波形的名字是否重复
        public bool judge(string textWaveName, string[] waveNames)
        {
            bool b = false;
            foreach (string s in waveNames)
            {
                //for(int i=0;i<textWaveName.Length;i++)
                //    if(textWaveName[i]==
                if (string.Equals(s, textWaveName))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }
        /*第一个string数组总共有8个元素
         每个元素都包含两部分，一是标识字符串，另一是内容和"\r\n"*/
        public string InsertAntenna8(string[] contents,string waveNameOfAntenna, string constStr1, string constStr2)
        {
            int insertSite = 0;
            bool firstAntenna = false;
            string antennaCount = null;
            //插入的是第一个天线
            if (sourceString.LastIndexOf(constStr1) == -1)
            {
                insertSite = sourceString.LastIndexOf(constStr2) + constStr2.Length + 2;
                firstAntenna = true;
                //当是第一次添加天线时，在traninfo文件中写入antenna 0
                antennaCount = "0";
            }
                //插入的不是第一个天线
            else
            {
                //插入的起始位置
                insertSite = sourceString.LastIndexOf(constStr1) + constStr1.Length + 2;
                //插入的不是第一个天线，算出天线的编号
                int antennaSite = sourceString.LastIndexOf("\nantenna") + 9;//10是\nantenna的长度
                while (sourceString[antennaSite] != '\r')
                    antennaCount = antennaCount + sourceString[antennaSite++];
                int b = int.Parse(antennaCount) + 1;
                antennaCount = string.Format("{0:##}", b);
            }
            //用户没有在combox中选择波形名称，则插入默认的正弦波
            //if (waveNameOfAntenna==null)
            //{
            //    string[] sinStr = { "Sinusoid", "Sinusoid", "1000000000", "1000000", "0" };
            //    sourceString = InsertWaveform5(sinStr, SetupContent.waveFormStr2, SetupContent.waveFormStr3,true);
                
            //}
            //else
            //{
               string[] waveNames = new string[30];
               waveNames = waveformNames(SetupContent.waveFormStr1);
                //插入setup文件中的波形
                   sourceString = sourceString.Insert(insertSite, contents[0]);
                   insertSite = insertSite + contents[0].Length ;
                   //是第一次添加天线，天线的编号
                   if (firstAntenna)
                   {
                       sourceString = sourceString.Insert(insertSite, "antenna " + "0" + "\r\n");
                       insertSite = insertSite + 11;
                   }
                       //不是第一次添加天线，天线的编号
                   else
                   {
                       sourceString = sourceString.Insert(insertSite, "antenna " + antennaCount + "\r\n");
                       insertSite = insertSite + 10 + antennaCount.Length;
                   }
                   sourceString = sourceString.Insert(insertSite, contents[1]);
                   insertSite = insertSite + contents[1].Length ;
                   //找到最大的waveform编号
                    //根据选中的波形名称去获得对应的波形编号
                   string waveStr = null;
                   int waveNameSite = sourceString.IndexOf(SetupContent.waveFormStr1 + " " + waveNameOfAntenna);
                   if (waveNameSite != -1)
                   {
                       //由begin_<Waveform>去获取waveform这一行字符串。第二次循环才获得waveform这一行字符串
                       for (int j = 0; j < 2; j++)
                       {
                           waveStr = null;
                           while (sourceString[waveNameSite +(SetupContent.waveFormStr1 + " " + waveNameOfAntenna).Length + 2] != '\r')
                           {
                               waveStr = waveStr + sourceString[waveNameSite + (SetupContent.waveFormStr1 + " " + waveNameOfAntenna).Length + 2];
                               waveNameSite++;
                           }
                           //跳过\r\n这两个字符
                           waveNameSite =waveNameSite+2;
                       }
                   }
                   string antennaName=StringFound.FoundBackStr(SetupContent.antennaStr1, contents[0], true); 
                   //将发射机需要的信息存放到.transinfo文件中
                   FileOperation.WriteFile(contents[0] + "antenna " + antennaCount + "\r\n" + waveStr + "\r\n" +SetupContent.antennaStr3+" " + antennaName +"\r\n", MainWindow.transInfoFullPath, true);
                   
                 //将新建的天线和其对应的波形保存在.match文件中
                   string antennaNameTemp=StringFound.FoundBackStr(SetupContent.antennaIndeStr,contents[0],true);
                   //FileOperation.WriteLineFile(SetupContent.antennaIndeStr +" "+ antennaNameTemp + "#" + waveStr + "#" + waveNameOfAntenna, MainWindow.relationOfAntAndWavePath,true);
                   FileOperation.WriteLineFile(SetupContent.antennaIndeStr + " " + antennaNameTemp  + "#" + waveNameOfAntenna, MainWindow.relationOfAntAndWavePath, true);

                   sourceString = sourceString.Insert(insertSite, waveStr+"\r\n");
                   insertSite = insertSite + waveStr.Length+2;

                   //contents[2]到contents[7]都是按相同的规律插入
                   for (int j = 2; j < 8; j++)
                   {
                       sourceString = sourceString.Insert(insertSite, contents[j]);
                       insertSite = insertSite + contents[j].Length;
                   }
            //}
            return sourceString;
        }
        //插入相应的波形数据，第一个参数是插入的内容，第二个参数是插入块的结束标志<end_waveform>
        //第三个参数是插入块开始的标志字符串
        public string InsertWaveform5(string[] contents, string constString1, string constString2,bool bbool)
        {
            sourceString = WriteConst.Write(sourceString);

            int site = 0;
            bool sign = false;
            string waveCount = null;
            //if-else语句先确定插入的位置
            if (sourceString.LastIndexOf(constString1) == -1)
            {
                //前面没有波形时确定插入波形块的起始位置
                site = sourceString.LastIndexOf(constString2) + constString2.Length + 1;
                sign = true;
            }
            else
            {
                //这时第一个插入的波形时，插入的起始位置为site
                site = sourceString.LastIndexOf(constString1) + constString1.Length + 1;
                //为获得waveform后面的编号，先确定查找的起始位置
                int waveSite = sourceString.LastIndexOf("\nwaveform") + 10;//10是\nwaveform的长度
                while (sourceString[waveSite] != '\r')
                    waveCount = waveCount + sourceString[waveSite++];
                int b = int.Parse(waveCount) + 1;
                waveCount = string.Format("{0:##}", b);
               }

            string temp = sourceString.Insert(site + 1, "begin_<Waveform> " + contents[0] + "\r\n");
            int count = site + 20 + contents[0].Length;//20是begin_<Waveform> 和控制字符的个数
            temp = temp.Insert(count, contents[1] + "\r\n");
            count = count + contents[1].Length + 2;
            //控制Waveform的编号
            if (sign)
            {
                temp = temp.Insert(count, "waveform " + "0" + "\r\n");
                count = count + 12;
            }
            else
            {
                temp = temp.Insert(count, "waveform " + waveCount + "\r\n");
                count = count + 11 + waveCount.Length;
            }
            double value = Double.Parse(contents[2]) * 1000000;
            //格式化字符串
            contents[2] = string.Format("{0:0.000}", value);
            temp = temp.Insert(count, "CarrierFrequency " + contents[2] + "\r\n");


            count = count + contents[2].Length + 19;

            if (contents[1] == "Sinusoid")
            {
                value = Double.Parse(contents[3]) * 1000000;
                contents[3] = string.Format("{0:0.000}", value);
                temp = temp.Insert(count, "bandwidth " + contents[3] + "\r\n");
                count = count + contents[3].Length + 12;

            }
            else
            {
                temp = temp.Insert(count, "PulseWidth " + contents[3] + "\r\n");
                count = count + contents[3].Length + 13;
            }


            value = Double.Parse(contents[4]);
            contents[4] = string.Format("{0:0.000}", value);
            if (bbool)
            {
                temp = temp.Insert(count, "phase " + contents[4] + "\r\n");
            }
            else
            {
                temp = temp.Insert(count, "Phase " + contents[4] + "\r\n");

            }

            count = count + contents[4].Length + 8;
            temp = temp.Insert(count, "end_<Waveform>\r\n");
            return temp;
        }
        //插入5+1型波形的相应数据
        public string InsertWaveform6(string[] contents, string constString1, string constString2)
        {
            string temp6 = this.InsertWaveform5(contents, constString1, constString2,false);
            int site6 = temp6.LastIndexOf(constString1) ;
            temp6 = temp6.Insert(site6, "Rolloff " + contents[5]+"\r\n");
            return temp6;
        }
        //插入7+1型波形的相应数据
        public string InsertWaveform8(string[] contents, string constString1, string constString2)
        {
            string temp8 = this.InsertWaveform6(contents, constString1, constString2);
            int site8 = temp8.LastIndexOf(constString1);
            temp8 = temp8.Insert(site8, "FrequencyVariation "+ contents[6]+ "\r\n");
            site8 = temp8.LastIndexOf(constString1);
            temp8 = temp8.Insert(site8, "StartFrequency " + contents[7] + "\r\n");
            site8 = temp8.LastIndexOf(constString1);
            temp8 = temp8.Insert(site8, "StopFrequency " + contents[8] + "\r\n");
            site8 = temp8.LastIndexOf("Phase");
            temp8 = temp8.Insert(site8, "Dispersive" + "\r\n");
            return temp8;
        }
        //插入3+1型波形的相应数据
        public string InsertWaveform4(string[] contents, string constString1, string constString2)
        {
            string temp4 = this.InsertWaveform5(contents, constString1, constString2,false);
            int site4 = temp4.LastIndexOf("CarrierFrequency ");
            temp4 = temp4.Remove(site4, 24);
            return temp4;
        }
        public static int GetCountOfMatchStr(string matchStr,string sourceStr)
        {
            MatchCollection mc;
            Regex r = new Regex(matchStr);
            mc = r.Matches(sourceStr);
            //for (int i = 0; i < mc.Count; i++)
            //    matchPosition[i] = mc[i].Index;
            return mc.Count+1 ;
        }
    }
}



