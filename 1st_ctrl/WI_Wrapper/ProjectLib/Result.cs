using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectLib
{
    public enum DataType { Scientific, FloatAccountToTwoDecimalPlaces, FloatAccountToThreeDecimalPlaces };
    public enum Unit { M, DB, s, dBm, Deg, Null };
    public class Result
    {
        public DataType dataType;
        public Unit unit;
        private string resultName;//属性名，如magnitude,phase,power等
        private string resultValue;//属性值
        public string ResultName
        {
            get { return this.resultName; }
            set { this.resultName = value; }
        }
        public string ResultValue
        {
            get { return this.resultValue; }
            set { resultValue = value; }
        }
        public Result(string resultName, string resultValue)
        {
            this.resultName = resultName;
            this.resultValue = resultValue;
            switch (resultName)
            {
                case "pathLoss":
                case "pathGain":
                    this.unit = Unit.DB;
                    this.dataType = DataType.FloatAccountToTwoDecimalPlaces;
                    break;
                case "X":
                case "Y":
                    this.unit = Unit.M;
                    this.dataType = DataType.Scientific;
                    break;
                case "Z":
                    this.unit = Unit.M;
                    this.dataType = DataType.FloatAccountToThreeDecimalPlaces;
                    break;
                case "distance":
                    this.unit = Unit.M;
                    this.dataType = DataType.FloatAccountToTwoDecimalPlaces;
                    break;
                case "power":
                    this.unit = Unit.dBm;
                    this.dataType = DataType.FloatAccountToTwoDecimalPlaces;
                    break;
                case "powerPhase":
                    this.unit = Unit.Deg;
                    this.dataType = DataType.FloatAccountToTwoDecimalPlaces;
                    break;
                case "delaySpread":
                    this.unit = Unit.s;
                    this.dataType = DataType.Scientific;
                    break;
                default:
                    this.unit = Unit.Null;
                    this.dataType = DataType.Scientific;
                    break;
            }
        }
    }
}
