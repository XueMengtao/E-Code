using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectLib
{
    public class FileNameInfo
    {
        private string fileName;
        private string projectName;
        private string resultType;
        private string transmitterNumber;
        private string txAmount;
        private string receiverNumber;
        private string midPointFrequency;
        private string minFrequency;
        private string maxFrequency;
        private string fileType;
        public FileNameInfo(string fileName)
        {
            this.fileName = fileName;
            string[] array = fileName.Split('_', '.');
            if (array.Length == 9 && !array.Contains("situation"))
            {
                this.projectName = array[0];
                this.resultType = array[1];
                this.transmitterNumber = array[2];
                this.txAmount = array[3];
                this.midPointFrequency = array[4];
                this.minFrequency = array[5];
                this.maxFrequency = array[6];
                this.receiverNumber = array[7];
                this.fileType = array[8];
            }
            else if (array.Length == 9 && array.Contains("situation") && (array.Contains("erm") || (array.Contains("power"))))
            {
                this.projectName = array[0];
                this.resultType = array[1] + array[2];
                this.transmitterNumber = array[3];
                this.txAmount = array[4];
                this.midPointFrequency = array[5];
                this.minFrequency = array[6];
                this.maxFrequency = array[7];
                this.fileType = array[8];
            }
            else if (array.Length == 7 && array.Contains("situation") && array.Contains("power") && array.Contains("total"))//situationpowertotal
            {
                this.projectName = array[0];
                this.resultType = array[1] + array[2] + array[3];
                this.transmitterNumber = array[4];
                this.txAmount = array[5];
                this.fileType = array[6];
            }
            else if (array.Length == 7 && !array.Contains("situation") && array.Contains("power") && array.Contains("total"))//powertotal
            {
                this.projectName = array[0];
                this.resultType = array[1] + array[4];
                this.transmitterNumber = array[2];
                this.txAmount = array[3];
                this.receiverNumber = array[5];
                this.fileType = array[6];
            }
            else if (array.Length == 7 && array.Contains("situation") && array.Contains("erms") && array.Contains("max")&&array.Contains("value"))
            {
                this.projectName = array[0];
                this.resultType = array[1] + array[2] + array[3] + array[4];
                this.midPointFrequency = array[5];
                this.fileType = array[6];
            }
            else if (array.Length == 6 && array.Contains("situation"))
            {
                this.projectName = array[0];
                this.resultType = array[1]+array[2]+array[3]+array[4];
                this.fileType = array[5];
            }
            else if (array.Length == 5 && !array.Contains("situation"))
            {
                this.projectName = array[0];
                this.resultType = array[1];
                this.midPointFrequency = array[2];
                this.receiverNumber = array[3];
                this.fileType = array[4];
            }
            else if (array.Length == 5 && array.Contains("situation")&&array.Contains("erms"))
            {
                this.projectName = array[0];
                this.resultType = array[1] + array[2];
                this.midPointFrequency = array[3];
                this.fileType = array[4];
            }
            else if (array.Length == 4 && array.Contains("power"))
            {
                this.projectName = array[0];
                this.resultType = array[1];
                this.receiverNumber = array[2];
                this.fileType = array[3];
            }
            else if (array.Length == 4 && array.Contains("erms"))
            {
                this.projectName = array[0];
                this.resultType = array[1];
                this.midPointFrequency=array[2];
                this.fileType=array[3];
            }
            else if (array.Length == 4 && array.Contains("psum"))
            {
                this.projectName = array[0];
                this.resultType = array[1]+array[2];
                this.fileType = array[3];
            }
            else if (array.Length == 3 && array.Contains("psum"))
            {
                this.projectName = array[0];
                this.resultType = array[1];
                this.fileType = array[2];
            }

            //else
            //    throw new WrongFileException(String.Format("工程文件夹下存在错误的文件{0}", this.fileName));
                
        }
        public string FileName
        {
            get
            {
                return fileName;
            }
        }
        public string ProjectName
        {
            get
            {
                return projectName;
            }
        }
        public string ResultType
        {
            get
            {
                return resultType;
            }
        }
        public string TransmitterNumber
        {
            get
            {
                return transmitterNumber;
            }

        }
        public string TxAmount
        {
            get
            {
                return txAmount;
            }

        }
        public string ReceiverNumber
        {
            get
            {
                return receiverNumber;
            }

        }
        public string MidPointFrequency
        {
            get
            {
                return midPointFrequency;
            }

        }
        public string MaxFrequency
        {
            get
            {
                return maxFrequency;
            }

        }
        public string MinFrequency
        {
            get
            {
                return minFrequency;
            }

        }
        public string FileType
        {
            get
            {
                return fileType;
            }
        }
    }
}
