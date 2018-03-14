using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ProjectLib
{
    public class FileContent
    {
        protected string receiverTypeRow;  
        //接收机类型行，形如：# Receiver Set:rx
        protected string defineRow;        
        //属性定义行，形如：#   Rx#   X(m)   Y(m)  Z(m)  Distance  Magnitude
        protected List<DataRow> dataRows = new List<DataRow>();
        //数据行，对于态势有多行，形如：1  1.88464E+03  1.70349E+03   5198.050   244.90  4.64662E-03
        public string ReceiverTypeRow
        {
            get{return this.receiverTypeRow;}
        }
        public string DefineRow
        {
            get{return this.defineRow;}
            set{this.defineRow = value;}
        }
        public List<DataRow> DataRows
        {
            get { return this.dataRows; }
        }
        public FileContent()
        {

        }
        public FileContent(string filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
           if (rows.Count <3)
               throw new LackOfFileContentException(String.Format("number of rows in the file{0} is not complete",filePath));
           else
           {
                this.receiverTypeRow = rows[0];          //对于每一个结果文件，第一行都为接收机类型行
                this.defineRow = rows[1];                
                //第二行都为属性定义行
                try
                {
                    for (int i = 2; i < rows.Count; i++)     //对于数据行，要把每一行的各个数据都拆分到具体的属性
                    {
                        DataRow dataRow = new DataRow(rows[i]);
                        this.dataRows.Add(dataRow);
                    }
                }
               catch(IndexOutOfRangeException ex)
                   {
                       throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                   }
           }
        }
    }

    public class MagnitudeFileContent : FileContent
    {
        private Result magnitude;  //场强文件特有的三个属性：幅度、实部、虚部
        private Result real;
        private Result imag;
        public Result Magnitude
        {
            get { return this.magnitude; }
        }
        public Result Real
        {
            get { return this.real; }
        }
        public Result Imag
        {
            get { return this.imag; }
        }
        public MagnitudeFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.magnitude = new Result("magnitude", attribute[5]);
                    this.real = new Result("real", attribute[6]);
                    this.imag = new Result("imag", attribute[7]);
                    this.dataRows[i - 2].Results.Add(this.magnitude);
                    this.dataRows[i - 2].Results.Add(this.real);
                    this.dataRows[i - 2].Results.Add(this.imag);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete",filePath), ex);
                }
            }
        }
    }
    public class totalMagnitudeFileContent : FileContent
    {
        private Result totalMagnitude;
        public Result Magnitude
        {
            get { return this.totalMagnitude; }
        }

        public totalMagnitudeFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.totalMagnitude = new Result("magnitude", attribute[5]);
                    this.dataRows[i - 2].Results.Add(this.totalMagnitude);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class PhaseFileContent : FileContent
    {
        private Result phase;
        private Result real;
        private Result imag;
        public Result Phase
        {
            get { return this.phase; }
        }
        public Result Real
        {
            get { return this.real; }
        }
        public Result Imag
        {
            get { return this.imag; }
        }
        public PhaseFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.phase = new Result("phase", attribute[5]);
                    this.real = new Result("real", attribute[6]);
                    this.imag = new Result("imag", attribute[7]);
                    this.dataRows[i - 2].Results.Add(this.phase);
                    this.dataRows[i - 2].Results.Add(this.real);
                    this.dataRows[i - 2].Results.Add(this.imag);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class PathGainFileContent : FileContent
    {
        private Result pathGain;
        public Result PathGain
        {
            get
            {
                return this.pathGain;
            }
        }
        public PathGainFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.pathGain = new Result("pathGain", attribute[5]);
                    this.dataRows[i - 2].Results.Add(this.pathGain);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class PathLossFileContent : FileContent
    {
        private Result pathLoss;
        public Result PathLoss
        {
            get
            {
                return this.pathLoss;
            }
        }
        public PathLossFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.pathLoss = new Result("pathLoss", attribute[5]);
                    this.dataRows[i - 2].Results.Add(this.pathLoss);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class PowerFileContent : FileContent
    {
        private Result power;
        private Result powerPhase;
        public Result Power
        {
            get
            {
                return this.power;
            }
        }
        public Result PowerPhase
        {
            get
            {
                return this.powerPhase;
            }
        }
        public PowerFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.power = new Result("power", attribute[5]);
                    this.powerPhase = new Result("powerPhase", attribute[6]);
                    this.dataRows[i - 2].Results.Add(this.power);
                    this.dataRows[i - 2].Results.Add(this.powerPhase);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class SpreadFileContent : FileContent
    {
        private Result delaySpread;
        public Result DelaySpread
        {
            get
            {
                return this.delaySpread;
            }
        }
        public SpreadFileContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string dataRow = rows[i].Trim();
                    string[] attribute = Regex.Split(dataRow, @"\s+");
                    this.delaySpread = new Result("Erms", attribute[5]);
                    this.dataRows[i - 2].Results.Add(this.delaySpread);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class SituationErmContent : FileContent
    {
        public SituationErmContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string[] attribute = Regex.Split(rows[i].Trim(), @"\s+");
                    Result erms = new Result("erms", attribute[5]);
                    this.dataRows[i - 2].Results.Add(erms);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
    public class SituationPowerContent : FileContent
    {
        public SituationPowerContent(string filePath)
            : base(filePath)
        {
            List<string> rows = new List<string>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    rows.Add(sr.ReadLine());
                }
            }
            for (int i = 2; i < rows.Count; i++)
            {
                try
                {
                    string[] attribute = Regex.Split(rows[i].Trim(), @"\s+");
                    Result power = new Result("power", attribute[5]);
                    Result powerPhase = new Result("powerPhase", attribute[6]);
                    this.dataRows[i - 2].Results.Add(power);
                    this.dataRows[i - 2].Results.Add(powerPhase);
                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new IndexOutOfRangeException(String.Format("The data of {0} is not complete", filePath), ex);
                }
            }
        }
    }
}
