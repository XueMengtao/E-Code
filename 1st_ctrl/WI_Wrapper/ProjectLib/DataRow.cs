using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ProjectLib
{
    public class DataRow
    {
        private string receiverNumber;//接收机编号   
        private string positionX;     //接收机的坐标
        private string positionY;
        private string positionZ;
        private string distance;      //接收机距离辐射源的距离
        //存放具体结果的数组，例如，对于场强文件，存放magnitude,real,imag.对于功率文件，存放power,phase
        private List<Result> results = new List<Result>();
        public string ReceiverNumber
        {
            get{ return this.receiverNumber;}
        }
        public string PositionX
        {
            get{ return this.positionX;}
        }
        public string PositionY
        {
            get{ return this.positionY;}
        }
        public string PositionZ
        {
            get
            { return this.positionZ;}
        }
        public string Distance
        {
            get{ return this.distance;}
        }
        public List<Result> Results
        {
            get{ return this.results;}
            set{ this.results = value;}
        }
        public DataRow(string dataRow)
        {
           try
           {
                string[] attribute = dataRow.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                //正则表达式写法：string[] attribute = Regex.Split(dataRow.Trim(), @"\s+");
                this.receiverNumber = attribute[0];
                this.positionX = attribute[1];
                this.positionY = attribute[2];
                this.positionZ = attribute[3];
                this.distance = attribute[4];
           }
          catch (IndexOutOfRangeException ex)
          {
              throw ex;
           }
        }
    }
}
