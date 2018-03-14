using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using TranmitterLib;

namespace ProjectLib
{
    /// <summary>
    /// 工程类
    /// </summary>
    public class Project
    {
        private string projectPath;   //工程路径
        private string projectName;   //工程名
        private List<ResultFile> resultFiles = null;

        public string ProjectPath
        {
            get { return this.projectPath; }
        }
        public string ProjectName
        {
            get { return projectName; }
        }
        public List<ResultFile> ResultFiles 
        {
            get
            {
                if (this.resultFiles == null)
                { 
                    this.resultFiles= GetResultFiles(this.projectPath);
                    return this.resultFiles;
                }
                else
                    return this.resultFiles;
            }
        }

        public Project()
        {

        }
        public Project(string projectPath)
        {
            this.projectPath = projectPath;
            this.projectName = Directory.GetParent(projectPath).FullName.Substring(
                Directory.GetParent(projectPath).FullName.LastIndexOf("\\") + 1);
            //eg.从路径C:\Users\Administrator\Desktop\测试项目\hwy317\studyarea中取出hwy317
        }

        /// <summary>
        /// 通过路径得到路径下的所有结果文件
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        private List<ResultFile> GetResultFiles(string projectPath)
        {
            List<ResultFile> resultFiles = new List<ResultFile>();
            try
            {
                string[] files = Directory.GetFiles(projectPath);
                foreach (string f in files)
                {
                    ResultFile file = new ResultFile(f);
                    resultFiles.Add(file);
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("工程所在目录不存在");
                LogFileManager.ObjLog.debug(ex.Message);
                throw new DirectoryNotFoundException("can't find the directory of project",ex);
            }
            if (resultFiles.Count  == 0)
                throw new FileNotFoundException("The project doesn't have result files");
            return resultFiles;
        }
        //*************************************以上部分完成的是project的初始化过程***********************



        
        static Dictionary<string, string> filePathsAndTotalResults = new Dictionary<string, string>();//Dictionary存放合成后结果和对应的文件路径
        /// <summary>
        /// 合并的总方法
        /// </summary>
        public void CombineResult()
        {
            //*************************此过程完成的是把结果文件分类的过程****************************************
            Dictionary<string, List<ResultFile>> differentTypesFiles = new Dictionary<string, List<ResultFile>>();
            differentTypesFiles.Add("exmag", new List<ResultFile>());
            differentTypesFiles.Add("eymag", new List<ResultFile>());
            differentTypesFiles.Add("ezmag", new List<ResultFile>());
            differentTypesFiles.Add("exphs", new List<ResultFile>());
            differentTypesFiles.Add("eyphs", new List<ResultFile>());
            differentTypesFiles.Add("ezphs", new List<ResultFile>());
            differentTypesFiles.Add("powertotal", new List<ResultFile>());
            differentTypesFiles.Add("situationerm", new List<ResultFile>());
            differentTypesFiles.Add("situationpowertotal", new List<ResultFile>());
            differentTypesFiles.Add("power", new List<ResultFile>());
            differentTypesFiles.Add("pl", new List<ResultFile>());
            differentTypesFiles.Add("pg", new List<ResultFile>());
            differentTypesFiles.Add("paths", new List<ResultFile>());
            differentTypesFiles.Add("spread", new List<ResultFile>());
            foreach (ResultFile resultFile in ResultFiles)//把结果文件中的各个文件分到各种类型的数组中
            {
                if (differentTypesFiles.Keys.Contains(resultFile.FileNameInfo.ResultType))
                    differentTypesFiles[resultFile.FileNameInfo.ResultType].Add(resultFile);
            }
            foreach (KeyValuePair<string, List<ResultFile>> kvp in differentTypesFiles)
            {
                if (kvp.Key != "situationerm" && kvp.Key != "situationpowertotal" && kvp.Value.Count == 0)
                {
                    throw new LackOfSomeTypeFileException(String.Format("The Project lacks of files :{0}", kvp.Key));
                }
            }
            //****************可以确保每一种类型的文件都不为空，如果缺失某种文件，则抛出异常，停止合成过程

            string tempFilePath = Path.Combine(this.projectPath, "temp");
            //创建temp文件夹是为了暂时存放场强合成第一步的结果文件（即etxamg,etymag,etzmag）
            if (!File.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }

            //合成和输出是分开的过程
            this.CombineMagnitudeOrPhase(differentTypesFiles["exmag"]);
            this.CombineMagnitudeOrPhase(differentTypesFiles["eymag"]);
            this.CombineMagnitudeOrPhase(differentTypesFiles["ezmag"]);
            this.OutPutResult(filePathsAndTotalResults);
            //将temp文件夹中的文件移动到结果文件根目录下（即studyarea下）
            string[] fileList = Directory.GetFiles(tempFilePath);
            foreach (string file in fileList)
            {
                string totalMagnitudeFilePath = Path.Combine(this.projectPath, Path.GetFileName(file));
                if (!File.Exists(totalMagnitudeFilePath))
                {
                    File.Copy(file, totalMagnitudeFilePath);
                }
            }
            filePathsAndTotalResults.Clear();
            this.CombineEtrms();//场强合成的第二步
            this.CombineMagnitudeOrPhase(differentTypesFiles["exphs"]);
            this.CombineMagnitudeOrPhase(differentTypesFiles["eyphs"]);
            this.CombineMagnitudeOrPhase(differentTypesFiles["ezphs"]);
            this.CombinePower(differentTypesFiles["powertotal"]);
            this.CombineSituationErms(differentTypesFiles["situationerm"]);
            this.CombineSituationPower(differentTypesFiles["situationpowertotal"]);
            //合并完结果后输出结果
            this.OutPutResult(filePathsAndTotalResults);
            Directory.Delete(this.projectPath + "\\temp", true);
        }
        


        //为方便单元测试，以下方法都改为internal
        /// <summary>
        /// 把接收机的种类给取出来
        /// </summary>
        /// <returns></returns>
        internal List<string> GetRxType(List<ResultFile> resultTypes)
        {
            List<string> rxType = new List<string>();
            foreach (ResultFile resultType in resultTypes)
            {
                if (rxType == null)
                    rxType.Add(resultType.FileNameInfo.ReceiverNumber);
                if (rxType.IndexOf(resultType.FileNameInfo.ReceiverNumber) == -1)
                    rxType.Add(resultType.FileNameInfo.ReceiverNumber);
            }
            return rxType;
        }


        /// <summary>
        /// 把同一种接收机的文件给取出来
        /// </summary>
        /// <returns></returns>
        internal List<List<ResultFile>> SearchSameRxTypeFile(List<ResultFile> resultTypes, List<string> rxTypes)
        {
            List<List<ResultFile>> sameRxTypeFile = new List<List<ResultFile>>();

            foreach (string rxType in rxTypes)
            {
                List<ResultFile> singleRxTypeFile = new List<ResultFile>();
                foreach (ResultFile resultType in resultTypes)
                {
                    if (resultType.FileNameInfo.ReceiverNumber == rxType)
                        singleRxTypeFile.Add(resultType);
                }
                sameRxTypeFile.Add(singleRxTypeFile);
            }
            return sameRxTypeFile;
        }


        /// <summary>
        /// 取出同一种接收机所包含的各个中点频率
        /// </summary>
        /// <param name="sameRxTypeFile"></param>
        /// <returns></returns>
        internal List<string> GetMidPointFrequency(List<ResultFile> singleRxTypeFile)
        {
            List<string> midPointFrequency = new List<string>();
            foreach (ResultFile file in singleRxTypeFile)
            {
                if (midPointFrequency == null)
                    midPointFrequency.Add(file.FileNameInfo.MidPointFrequency);
                if (midPointFrequency.IndexOf(file.FileNameInfo.MidPointFrequency) == -1)
                    midPointFrequency.Add(file.FileNameInfo.MidPointFrequency);
            }
            return midPointFrequency;
        }


        /// <summary>
        /// 多频段求权重函数
        /// </summary>
        internal List<Dictionary<ResultFile, double>> GetWeight(List<ResultFile> singleRxTypeFile, List<string> midPointFrequencies)
        {
            List<Dictionary<ResultFile, double>> resultFilesWithWeights = new List<Dictionary<ResultFile, double>>();
            foreach (string midPointFrequency in midPointFrequencies)
            {
                Dictionary<ResultFile, double> singleMidPointResultFilesWithWeight = new Dictionary<ResultFile, double>();
                ResultFile baseFile = new ResultFile();
                foreach (ResultFile resultFile in singleRxTypeFile)
                {
                    if (resultFile.FileNameInfo.MidPointFrequency == midPointFrequency)
                    {
                        baseFile = resultFile;
                        break;
                    }
                }
                foreach (ResultFile resultFile in singleRxTypeFile)
                {
                    if (resultFile.FileNameInfo.MidPointFrequency == midPointFrequency)
                        singleMidPointResultFilesWithWeight.Add(resultFile, 1);
                    else if (((int.Parse(resultFile.FileNameInfo.MinFrequency) >= int.Parse(baseFile.FileNameInfo.MinFrequency))
                        && (int.Parse(resultFile.FileNameInfo.MinFrequency) <= int.Parse(baseFile.FileNameInfo.MaxFrequency)))
                        || ((int.Parse(resultFile.FileNameInfo.MaxFrequency) >= int.Parse(baseFile.FileNameInfo.MinFrequency))
                        && (int.Parse(resultFile.FileNameInfo.MaxFrequency) <= int.Parse(baseFile.FileNameInfo.MaxFrequency))))
                    {
                        double leftside = Math.Max(int.Parse(resultFile.FileNameInfo.MinFrequency), int.Parse(baseFile.FileNameInfo.MinFrequency));
                        double rightside = Math.Min(int.Parse(resultFile.FileNameInfo.MaxFrequency), int.Parse(baseFile.FileNameInfo.MaxFrequency));
                        double weight = (rightside - leftside) / (int.Parse(baseFile.FileNameInfo.MaxFrequency)
                            - int.Parse(baseFile.FileNameInfo.MinFrequency));
                        singleMidPointResultFilesWithWeight.Add(resultFile, weight);
                    }
                }
                resultFilesWithWeights.Add(singleMidPointResultFilesWithWeight);
            }
            return resultFilesWithWeights;
        }


        /// <summary>
        /// 从相同接收机的一组文件中取出中点频率相同的一组文件
        /// </summary>
        /// <param name="singleRxTypeFile"></param>
        /// <param name="midPointFrequencies"></param>
        /// <returns></returns>
        internal List<List<ResultFile>> SearchSameMidPointAndSameRxTypeFile(List<ResultFile> singleRxTypeFile, List<string> midPointFrequencies)
        {
            List<List<ResultFile>> sameMidPointAndSameRxTypeFile = new List<List<ResultFile>>();
            foreach (string midPointFrequency in midPointFrequencies)
            {
                List<ResultFile> singleMidPointAndSameRxTypeFile = new List<ResultFile>();
                foreach (ResultFile resultFile in singleRxTypeFile)
                {
                    if (resultFile.FileNameInfo.MidPointFrequency == midPointFrequency)
                        singleMidPointAndSameRxTypeFile.Add(resultFile);
                }
                sameMidPointAndSameRxTypeFile.Add(singleMidPointAndSameRxTypeFile);
            }
            return sameMidPointAndSameRxTypeFile;
        }


        /// <summary>
        /// 合并场强幅度或相位的方法
        /// </summary>
        /// <param name="resultType"></param>
        internal void CombineMagnitudeOrPhase(List<ResultFile> resultTypes)
        {
            string resultType = null;   //结果文件类型,比如:etxmag,etyphs
            string totalResult = null;  //合成后的结果
            string filePath = null;     //合成结果的文件路径
            string typeSymbol = null;   //标识合成的是场强还是相位
            switch (resultTypes[0].FileNameInfo.ResultType) //根据输入的参数判断合成的是哪种类型的文件
            {
                case "exmag":
                    resultType = "etxmag";
                    typeSymbol = "magnitude";
                    break;
                case "eymag":
                    resultType = "etymag";
                    typeSymbol = "magnitude";
                    break;
                case "ezmag":
                    resultType = "etzmag";
                    typeSymbol = "magnitude";
                    break;
                case "exphs":
                    resultType = "etxphs";
                    typeSymbol = "phase";
                    break;
                case "eyphs":
                    resultType = "etyphs";
                    typeSymbol = "phase";
                    break;
                case "ezphs":
                    resultType = "etzphs";
                    typeSymbol = "phase";
                    break;
            }
            List<List<ResultFile>> sameRxTypeFile = new List<List<ResultFile>>();
            List<string> midPointFrequencies = new List<string>();
            List<Dictionary<ResultFile, double>> resultFilesWithWeights = new List<Dictionary<ResultFile, double>>();
            List<string> rxtypes = GetRxType(resultTypes);
            if (resultTypes != null && resultTypes.Count > 0)
            {
                sameRxTypeFile = SearchSameRxTypeFile(resultTypes, GetRxType(resultTypes));
                foreach (List<ResultFile> singleRxTypeFile in sameRxTypeFile)
                {
                    midPointFrequencies = GetMidPointFrequency(singleRxTypeFile);
                    resultFilesWithWeights = GetWeight(singleRxTypeFile, midPointFrequencies);
                    for (int i = 0; i < midPointFrequencies.Count; i++)
                    {
                        if(typeSymbol=="magnitude")
                        {
                            totalResult = CombineMethod.CombineMag(resultFilesWithWeights[i]);
                            filePath = this.projectPath + "\\" + "temp" + "\\" + this.projectName + "_" +resultType
                                + "_" + midPointFrequencies[i] + "_" + singleRxTypeFile[0].FileNameInfo.ReceiverNumber + ".p2m";
                                filePathsAndTotalResults.Add(filePath, totalResult);
                         }
                        if(typeSymbol=="phase")
                        {
                            totalResult = CombineMethod.CombinePhase(resultFilesWithWeights[i]);
                            filePath = this.projectPath + "\\" + this.projectName + "_" + resultType + "_" +
                                midPointFrequencies[i] + "_" + singleRxTypeFile[0].FileNameInfo.ReceiverNumber + ".p2m";
                                filePathsAndTotalResults.Add(filePath, totalResult);
                         }
                    }
                }
            }
        }


        /// <summary>
        /// 合并etrms 
        /// </summary>
        internal void CombineEtrms()
        {
            string totalResult = null;
            string filePath = null;
            //******************取出temp文件夹中的所有文件
            List<ResultFile> resultFiles = new List<ResultFile>();
            string[] files = Directory.GetFiles(this.projectPath + "\\" + "temp");
            foreach (string f in files)
            {
                ResultFile file = new ResultFile(f);
                resultFiles.Add(file);
            }
            //*********************************************
            List<string> midPointFrequencies = new List<string>();
            List<List<ResultFile>> sameRxTypeFile = new List<List<ResultFile>>();
            List<List<ResultFile>> sameMidPointAndSameRxtypeFile = new List<List<ResultFile>>();
            if (resultFiles != null && resultFiles.Count > 0)
            {
                sameRxTypeFile = SearchSameRxTypeFile(resultFiles, GetRxType(resultFiles));
                foreach (List<ResultFile> singleRxTypeFile in sameRxTypeFile)
                {
                    midPointFrequencies = GetMidPointFrequency(singleRxTypeFile);
                    sameMidPointAndSameRxtypeFile = SearchSameMidPointAndSameRxTypeFile(singleRxTypeFile, midPointFrequencies);
                    for (int i = 0; i < midPointFrequencies.Count; i++)
                    {
                        totalResult = CombineMethod.CombineEtrms(sameMidPointAndSameRxtypeFile[i]);
                        filePath = this.projectPath + "\\" + this.projectName + "_etrms_" + midPointFrequencies[i] +
                            "_" + singleRxTypeFile[0].FileNameInfo.ReceiverNumber + ".p2m";
                        filePathsAndTotalResults.Add(filePath, totalResult);
                    }
                }
            }
        }


        /// </summary>
        /// 合并功率
        /// </summary>
        internal void CombinePower(List<ResultFile> resultTypes)
        {
            if (resultTypes != null && resultTypes.Count > 0)
            {
                List<List<ResultFile>> sameRxTypeFile = new List<List<ResultFile>>();
                sameRxTypeFile = SearchSameRxTypeFile(resultTypes, GetRxType(resultTypes));
                for (int i = 0; i < sameRxTypeFile.Count; i++)
                {
                    string totalResult = CombineMethod.CombinePower(sameRxTypeFile[i]);
                    string filePath = this.projectPath + "\\" + this.projectName + "_power_" + GetRxType(resultTypes)[i] + ".p2m";
                    filePathsAndTotalResults.Add(filePath, totalResult);
                }
            }
        }


        /// </summary>
        /// 合并态势显示的erm的方法
        /// </summary>
        internal void CombineSituationErms(List<ResultFile> resultTypes)
        {
            if (resultTypes != null && resultTypes.Count > 0)
            {
                string totalResult = null;
                List<string> midPointFrequencies = GetMidPointFrequency(resultTypes);
                List<Dictionary<ResultFile, double>> resultFilesWithWeights = GetWeight(resultTypes, midPointFrequencies);
                for (int i = 0; i < midPointFrequencies.Count; i++)
                {
                    totalResult = CombineMethod.CombineSituationErms(resultFilesWithWeights[i]);
                    using (StreamWriter sw = File.CreateText(this.projectPath + "\\" + this.projectName + "_erms_" + midPointFrequencies[i] + ".p2m"))
                        sw.Write(totalResult);
                    //********************添加生成bmp文件,有异常则不生成，跳至下一处
                    do
                    {
                        StreamReader streamReader = new StreamReader(this.projectPath + "\\" + this.projectName + "_erms_" + midPointFrequencies[i] + ".p2m");
                        List<PointValue> pointValueList = new List<PointValue>();
                        List<PointValue> pointNormalizeList = new List<PointValue>();
                        streamReader.ReadLine();
                        streamReader.ReadLine();//第1、2行不读
                        string lineData = streamReader.ReadLine();
                        double tempMax = -80;
                        double tempMin = 0;
                        while (lineData != null)
                        {
                            PointValue pointValue = new PointValue();
                            lineData = lineData.Trim();
                            string[] tmp = System.Text.RegularExpressions.Regex.Split(lineData, @"\s+");
                            bool isSuccess = Double.TryParse(tmp[1], out pointValue.x);
                            if (!isSuccess)
                                break;
                            isSuccess = Double.TryParse(tmp[2], out pointValue.y);
                            if (!isSuccess)
                                break;
                            isSuccess = Double.TryParse(tmp[3], out pointValue.z);
                            if (!isSuccess)
                                break;
                            isSuccess = Double.TryParse(tmp[5], out pointValue.value);
                            if (!isSuccess)
                                break;
                            pointValueList.Add(pointValue);
                            double tempValue = pointValue.value;
                            if (tempValue < 0.00000001)
                                tempValue = -80;
                            else
                                tempValue = 10 * Math.Log10(tempValue);
                            pointValue.value = tempValue;
                            pointNormalizeList.Add(pointValue);
                            if (pointValue.value > tempMax)
                                tempMax = pointValue.value;
                            if (pointValue.value < tempMin)
                                tempMin = pointValue.value;
                            lineData = streamReader.ReadLine();
                        }
                        streamReader.Close();
                        for (int itemp = 0; itemp < pointValueList.Count; itemp++)
                        {
                            pointNormalizeList[itemp].value = (pointValueList[itemp].value - tempMin) / (tempMax - tempMin);//以对数形式归一化
                        }
                        //生成bmp
                        BmpResultUti.CreateBMP(pointValueList, this.projectPath + "\\" + this.projectName + "_situation_erms_" + midPointFrequencies[i] + ".bmp", tempMax, tempMin);
                        using (StreamWriter sw = File.CreateText(this.projectPath + "\\" + this.projectName + "_situation_erms_max_value_" + midPointFrequencies[i] + ".p2m"))//将最大值写入文件
                        {
                            sw.WriteLine(tempMax.ToString());
                            sw.WriteLine(tempMin.ToString());
                        }
                    } while (false);
                }
                //******************************************
            }
        }


        /// <summary>
        /// 合并态势功率的方法
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="projectPath"></param>
        /// <param name="projectName"></param>
        internal void CombineSituationPower(List<ResultFile> resultTypes)
        {
            string totalResult = null;
            if (resultTypes != null && resultTypes.Count > 0)
            {
                totalResult = CombineMethod.CombineSituationPower(resultTypes);
                using (StreamWriter sw = File.CreateText(this.projectPath + "\\" + this.projectName + "_psum.p2m"))
                    sw.Write(totalResult);
                //********************添加生成bmp文件,有异常则不生成，跳至下一处
                do
                {
                    StreamReader streamReader = new StreamReader(this.projectPath + "\\" + this.projectName + "_psum.p2m");
                    List<PointValue> pointNormalizeList = new List<PointValue>();
                    List<PointValue> pointValueList = new List<PointValue>();
                    streamReader.ReadLine();
                    streamReader.ReadLine();//第1、2行不读
                    string lineData = streamReader.ReadLine();
                    double tempMax = -100;
                    double tempMin = 0;
                    while (lineData != null)
                    {
                        PointValue pointValue = new PointValue();
                        lineData = lineData.Trim();
                        //按空格拆分
                        string[] tmp = System.Text.RegularExpressions.Regex.Split(lineData, @"\s+");
                        bool isSuccess = Double.TryParse(tmp[1], out pointValue.x);
                        if (!isSuccess)
                            break;
                        isSuccess = Double.TryParse(tmp[2], out pointValue.y);
                        if (!isSuccess)
                            break;
                        isSuccess = Double.TryParse(tmp[3], out pointValue.z);
                        if (!isSuccess)
                            break;
                        isSuccess = Double.TryParse(tmp[5], out pointValue.value);
                        if (!isSuccess)
                            break;
                        pointValueList.Add(pointValue);
                        pointNormalizeList.Add(pointValue);
                        if (pointValue.value > tempMax)
                            tempMax = pointValue.value;
                        if (pointValue.value < tempMin)
                            tempMin = pointValue.value;
                        lineData = streamReader.ReadLine();
                    }
                    streamReader.Close();
                    for (int itemp = 0; itemp < pointValueList.Count; itemp++)
                    {
                        pointNormalizeList[itemp].value = (pointValueList[itemp].value - tempMin) / (tempMax - tempMin);//以对数形式归一化
                    }
                    BmpResultUti.CreateBMP(pointValueList, this.projectPath + "\\" + this.projectName + "_situation_psum.bmp", tempMax, tempMin);
                    using (StreamWriter sw = File.CreateText(this.projectPath + "\\" + this.projectName + "_situation_psum_max_value" + ".p2m"))//将功率最大值最小值写入文件
                    {
                        sw.WriteLine(tempMax.ToString());
                        sw.WriteLine(tempMin.ToString());
                    }
                } while (false);
            }
        }


        /// <summary>
        /// 把合成的结果输出出来
        /// </summary>
        /// <param name="totalResultAndFilePaths"></param>
        internal void OutPutResult(Dictionary<string, string> totalResultAndFilePaths)
        {
            foreach (KeyValuePair<string, string> totalResultAndFilePath in totalResultAndFilePaths)
            {
                if (!File.Exists(totalResultAndFilePath.Key))
                    using (StreamWriter sw = File.CreateText(totalResultAndFilePath.Key))
                        sw.Write(totalResultAndFilePath.Value);
            }
        }
    }
}
