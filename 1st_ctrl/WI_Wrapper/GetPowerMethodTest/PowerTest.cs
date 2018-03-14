using RayCalInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using CalculateModelClasses;
using TxRxFileProceed;
using System.IO;
using UanFileProceed;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using FileObject;

using System.Runtime.InteropServices;
using System.Security;
using LogFileManager;
using System.Collections;
using SetupFileProceed;



namespace GetPowerMethodTest
{


    /// <summary>
    ///这是 PowerTest 的测试类，旨在
    ///包含所有 PowerTest 单元测试
    ///</summary>
    [TestClass()]
    public class PowerTest
    {

        public PowerTest()
        {
            StreamReader sr = new StreamReader(@"C:\Users\wangnan\Desktop\新建文件夹\FinalCode6\1st_ctrl\WI_Wrapper\TestForCal\bin\Debug\");
            string s2 = sr.ReadToEnd();
            ReadUan.GetGainPara(s2);
        }





        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///GetPower 的测试
        ///</summary>
        [TestMethod()]
        public void GetPowerTest()
        {
            //TextReader textreader = new TextReader(ReadUan.GetUan(RxFileProceed.RxReader(".\\.\\project\\ln2016102401\\ln2016102401.rx"), SetupFileProceed.GetSetupFile.GetSetup(".\\.\\project\\ln2016102401\\ln2016102401.setup")));
            //ReadUan.GetUan(RxFileProceed.RxReader(".\\.\\project\\ln2016102401\\ln2016102401.rx"), SetupFileProceed.GetSetupFile.GetSetup(".\\.\\project\\ln2016102401\\ln2016102401.setup")));
            //TextReader textreader = StreamReader;
            string Rx_uan = ""; // TODO: 初始化为适当的值
            Node finalNode = new Node();
             // TODO: 初始化为适当的值.
            finalNode.NodeStyle = NodeStyle.Rx;
            finalNode.Position =new Point(-274.107,784.107,4294.69);
            finalNode.RayTracingDistance = 100;
            finalNode.FrontDiffractionNode = null;
            finalNode.Frequence = 90;
            finalNode.FrequenceWidth = 1;
            Plural plural4 = new Plural(5,5);
            Plural plural5 = new Plural(6,6);
            Plural plural6 = new Plural(7,7);
            finalNode.TotalE = new EField(plural4,plural5,plural6);

            Node parentNode = new Node(); // TODO: 初始化为适当的值
            parentNode.Position = new Point(0,0,0);
            parentNode.Power = new Plural(1000,0);
            parentNode.RayTracingDistance=0;
            parentNode.NodeStyle = NodeStyle.Tx;
            parentNode.ChildNodes.Add(finalNode);
            parentNode.LayNum = 1;
            Plural plural1 = new Plural(1,1);
            Plural plural2 = new Plural(2,2);
            Plural plural3 = new Plural(3,3);
            parentNode.Frequence = 100;
            parentNode.FrequenceWidth = 1;
            parentNode.TotalE = new EField(plural1,plural2,plural3);
            parentNode.FrontDiffractionNode = null;
           
           
           double actual;
            actual = Power.GetPower(finalNode, parentNode);
            Plural expected = null; // TODO: 初始化为适当的值
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
