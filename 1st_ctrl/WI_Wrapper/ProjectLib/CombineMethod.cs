using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectLib
{
    public static class CombineMethod
    {
        /// <summary>
        ///  对某一频点有贡献的文件中的实部和虚部分别加权合成
        /// </summary>
        /// <param name="singleMidpointResultFilesWithWeight"></param>某一中点频率下的各个文件以及它的权重的
        /// <returns></returns>
        static public string CombineMag(Dictionary<ResultFile, double> singleMidpointResultFilesWithWeight)
        {
            if (singleMidpointResultFilesWithWeight != null && singleMidpointResultFilesWithWeight.Count > 0)
            {
                List<ResultFile> resultFiles = new List<ResultFile>(singleMidpointResultFilesWithWeight.Keys);
                double totalReal = 0;
                double totalImag = 0;
                string row3 = null;
                foreach (KeyValuePair<ResultFile, double> resultFileWithWeight in singleMidpointResultFilesWithWeight)
                {
                    totalReal = totalReal + Double.Parse(resultFileWithWeight.Key.FileContent.DataRows[0].Results[1].ResultValue) * resultFileWithWeight.Value;
                    totalImag = totalImag + Double.Parse(resultFileWithWeight.Key.FileContent.DataRows[0].Results[2].ResultValue) * resultFileWithWeight.Value;
                }
                string totalRealString = String.Format("{0:0.00000E+00}", totalReal);    //该行的实部总和          
                string totalImagString = String.Format("{0:0.00000E+00}", totalImag);    //该行的虚部总和
                double totalMag = Math.Sqrt(Math.Pow(totalReal, 2) + Math.Pow(totalImag, 2));
                string totalMagString = String.Format("{0:0.00000E+00}", totalMag);
                row3 = "     " + resultFiles[0].FileContent.DataRows[0].ReceiverNumber + "  " + resultFiles[0].FileContent.DataRows[0].PositionX + "     "
                    + resultFiles[0].FileContent.DataRows[0].PositionY + "     " + resultFiles[0].FileContent.DataRows[0].PositionZ + "   " +
                    resultFiles[0].FileContent.DataRows[0].Distance + "     " + totalMagString + "  " + totalRealString + "  " + totalImagString;
                string result = resultFiles[0].FileContent.ReceiverTypeRow + "\r\n" + resultFiles[0].FileContent.DefineRow + "\r\n" + row3 + "\r\n";
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleMidpointResultFilesWithWeight"></param>
        /// <returns></returns>
        static public string CombinePhase(Dictionary<ResultFile, double> singleMidpointResultFilesWithWeight)
        {
            if (singleMidpointResultFilesWithWeight != null && singleMidpointResultFilesWithWeight.Count > 0)
            {
                List<ResultFile> resultFiles = new List<ResultFile>(singleMidpointResultFilesWithWeight.Keys);
                double totalReal = 0;
                double totalImag = 0;
                foreach (KeyValuePair<ResultFile, double> resultFileWithWeight in singleMidpointResultFilesWithWeight)
                {
                    totalReal = totalReal + Double.Parse(resultFileWithWeight.Key.FileContent.DataRows[0].Results[1].ResultValue) * resultFileWithWeight.Value;
                    totalImag = totalImag + Double.Parse(resultFileWithWeight.Key.FileContent.DataRows[0].Results[2].ResultValue) * resultFileWithWeight.Value;
                }
                string totalRealString = String.Format("{0:0.00000E+00}", totalReal);    //该行的实部总和          
                string totalImagString = String.Format("{0:0.00000E+00}", totalImag);    //该行的虚部总和
                double degree = Math.Atan2(totalImag, totalReal) * (180 / Math.PI);
                string degreeString = String.Format("{0:f2}", degree);
                string row3 = "     " + resultFiles[0].FileContent.DataRows[0].ReceiverNumber + "  " + resultFiles[0].FileContent.DataRows[0].PositionX + "     "
                    + resultFiles[0].FileContent.DataRows[0].PositionY + "       " + resultFiles[0].FileContent.DataRows[0].PositionZ + "   " +
                    resultFiles[0].FileContent.DataRows[0].Distance + "    " + degreeString + "   " + totalRealString + "   " + totalImagString;
                string result = resultFiles[0].FileContent.ReceiverTypeRow + "\r\n" + resultFiles[0].FileContent.DefineRow + "\r\n" + row3 + "\r\n";
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleMidPointAndSameRxTypeFile"></param>
        /// <returns></returns>
        static public string CombineEtrms(List<ResultFile> singleMidPointAndSameRxTypeFile)
        {
            if (singleMidPointAndSameRxTypeFile != null && singleMidPointAndSameRxTypeFile.Count > 0)
            {
                double sumReal = 0;
                double sumImag = 0;
                foreach (ResultFile resultFile in singleMidPointAndSameRxTypeFile)
                {
                    sumReal = sumReal + Double.Parse(resultFile.FileContent.DataRows[0].Results[1].ResultValue);
                    sumImag = sumImag + Double.Parse(resultFile.FileContent.DataRows[0].Results[2].ResultValue);
                }
                double erms = Math.Sqrt(Math.Pow(sumReal, 2) + Math.Pow(sumImag, 2)) / Math.Sqrt(2);
                string ermsString = string.Format("{0:0.00000E+00}", erms);
                string row3 = "     " + singleMidPointAndSameRxTypeFile[0].FileContent.DataRows[0].ReceiverNumber + "  " + singleMidPointAndSameRxTypeFile[0].FileContent.DataRows[0].PositionX + "     "
                     + singleMidPointAndSameRxTypeFile[0].FileContent.DataRows[0].PositionY + "     " + singleMidPointAndSameRxTypeFile[0].FileContent.DataRows[0].PositionZ + "   " +
                     singleMidPointAndSameRxTypeFile[0].FileContent.DataRows[0].Distance + "         " + ermsString;
                string row2 = singleMidPointAndSameRxTypeFile[0].FileContent.DefineRow.Remove(63);
                row2 = row2.Insert(63, "        Erms");
                string result = singleMidPointAndSameRxTypeFile[0].FileContent.ReceiverTypeRow + "\r\n" + row2 + "\r\n" + row3 + "\r\n";
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleRxTypeFile"></param>
        /// <returns></returns>
        static public string CombinePower(List<ResultFile> singleRxTypeFile)
        {
            if (singleRxTypeFile != null && singleRxTypeFile.Count > 0)
            {
                double totalPower = 0;
                double totalPhase = 0;
                foreach (ResultFile resultFile in singleRxTypeFile)
                {
                    totalPower = totalPower + Double.Parse(resultFile.FileContent.DataRows[0].Results[0].ResultValue);
                    totalPhase = totalPhase + Double.Parse(resultFile.FileContent.DataRows[0].Results[1].ResultValue);
                }
                string totalPowerString = String.Format("{0:0.00000E+00}", totalPower);
                string totalPhaseString = String.Format("{0:0.00000E+00}", totalPhase);
                string row3 = "     " + singleRxTypeFile[0].FileContent.DataRows[0].ReceiverNumber + "  " + singleRxTypeFile[0].FileContent.DataRows[0].PositionX + "  "
                     + singleRxTypeFile[0].FileContent.DataRows[0].PositionY + "     " + singleRxTypeFile[0].FileContent.DataRows[0].PositionZ + "       " +
                     singleRxTypeFile[0].FileContent.DataRows[0].Distance + "    " + totalPowerString + "    " + totalPhaseString;
                string result = singleRxTypeFile[0].FileContent.ReceiverTypeRow + "\r\n" + singleRxTypeFile[0].FileContent.DefineRow + "\r\n" + row3 + "\r\n";
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="singleMidpointResultFilesWithWeight"></param>
        /// <returns></returns>
        static public string CombineSituationErms(Dictionary<ResultFile, double> singleMidpointResultFilesWithWeight)
        {
            if (singleMidpointResultFilesWithWeight != null && singleMidpointResultFilesWithWeight.Count > 0)
            {
                List<ResultFile> resultFiles = new List<ResultFile>(singleMidpointResultFilesWithWeight.Keys);
                double sum = 0;
                List<string> rows = new List<string>();
                for (int i = 0; i < resultFiles[0].FileContent.DataRows.Count; i++)
                {
                    foreach (KeyValuePair<ResultFile, double> resultFileWithWeight in singleMidpointResultFilesWithWeight)
                    {
                        sum = sum + Math.Pow(Double.Parse(resultFileWithWeight.Key.FileContent.DataRows[i].Results[0].ResultValue), 2) * Math.Pow(resultFileWithWeight.Value, 2);
                    }
                    double erms = Math.Sqrt(sum) / Math.Sqrt(2);
                    string ermsString = String.Format("{0:0.00000E+00}", erms);
                    rows.Add("     " + resultFiles[0].FileContent.DataRows[i].ReceiverNumber + "  " + resultFiles[0].FileContent.DataRows[i].PositionX + "  "
                    + resultFiles[0].FileContent.DataRows[i].PositionY + "    " + resultFiles[0].FileContent.DataRows[i].PositionZ + "     " +
                    resultFiles[0].FileContent.DataRows[i].Distance + "    " + ermsString);
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(resultFiles[0].FileContent.ReceiverTypeRow);
                sb.AppendLine(resultFiles[0].FileContent.DefineRow);
                foreach (string dataRow in rows)
                {
                    sb.AppendLine(dataRow);
                }
                string result = sb.ToString();
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultTypes"></param>
        /// <returns></returns>
        static public string CombineSituationPower(List<ResultFile> resultTypes)
        {
            if (resultTypes != null && resultTypes.Count > 0)
            {
                List<string> rows = new List<string>();
                Plural sum = new Plural();
                double totalPow;
                double totalPhase;
                for (int i = 0; i < resultTypes[0].FileContent.DataRows.Count; i++)
                {
                    foreach (ResultFile resultFile in resultTypes)
                    {
                        Plural temp = new Plural().Converse(Math.Pow(10, (Double.Parse(resultTypes[0].FileContent.DataRows[i].Results[0].ResultValue) - 30) / 10), Double.Parse(resultTypes[0].FileContent.DataRows[i].Results[1].ResultValue));
                        sum.Re += temp.Re;
                        sum.Im += temp.Im;
                    }
                    totalPow = Math.Sqrt(Math.Pow(sum.Re, 2) + Math.Pow(sum.Im, 2));
                    if (totalPow.Equals(0.0))
                    {
                        totalPow = -100.0;
                        totalPhase = 0.0;
                    }
                    else
                    {
                        totalPow = Math.Log10(totalPow) * 10 + 30;
                        totalPhase = Math.Atan(sum.Im / sum.Re);
                        totalPhase = totalPhase < 0 ? totalPhase + 180 : totalPhase;
                    }
                    string totalPowString = String.Format("{0:0.00000E+00}", totalPow);
                    string totalPhaseString = totalPhase.ToString("f2");
                    rows.Add("     " + resultTypes[0].FileContent.DataRows[i].ReceiverNumber + "  " + resultTypes[0].FileContent.DataRows[i].PositionX + "  "
                    + resultTypes[0].FileContent.DataRows[i].PositionY + "     " + resultTypes[0].FileContent.DataRows[i].PositionZ + "       " +
                    resultTypes[0].FileContent.DataRows[i].Distance + "    " + totalPowString + "    " + totalPhaseString);
                }
                StringBuilder sb = new StringBuilder();
                int pos = resultTypes[0].FileContent.DefineRow.IndexOf("Power");
                resultTypes[0].FileContent.DefineRow = resultTypes[0].FileContent.DefineRow.Remove(pos);
                resultTypes[0].FileContent.DefineRow = resultTypes[0].FileContent.DefineRow.Insert(pos, "Total power(dBm) Phase (Deg.)");
                sb.AppendLine(resultTypes[0].FileContent.ReceiverTypeRow);
                sb.AppendLine(resultTypes[0].FileContent.DefineRow);
                foreach (string dataRow in rows)
                {
                    sb.AppendLine(dataRow);
                }
                string result = sb.ToString();
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
