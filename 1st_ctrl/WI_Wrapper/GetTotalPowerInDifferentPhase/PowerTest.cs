using RayCalInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CalculateModelClasses;
using System.Collections.Generic;
using System.IO;
using UanFileProceed;
namespace GetTotalPowerInDifferentPhase
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
            StreamReader sr = new StreamReader(@"C:\Users\wangnan\Desktop\EMC_Project\FinalCode6\1st_ctrl\WI_Wrapper\TestForCal\bin\Debug\HalfWaveDipole.uan");
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
        ///GetTotalPowerInDifferentPhase 的测试
        ///</summary>
        [TestMethod()]
        public void GetTotalPowerInDifferentPhaseTest()
        {
            
            
            Node node1 = new Node();
            node1.Position = new Point(0,0,0);
            node1.Frequence = 100;
            Node node2 = new Node();
            node2.Frequence = 100;
            node2.Position = new Point(1,0,-1);
            Node node3 = new Node();
            node3.Position = new Point(2,0,0);
            node3.Frequence = 100;
            node3.TotalE = new EField(new Plural(1, 1), new Plural(1, 1), new Plural(1, 1));
            List<Node> lists = new List<Node> { node1,node2,node3};
            node1.TxNum = 1; node3.RxNum = 1;
            CalculateModelClasses.Path path1 = new CalculateModelClasses.Path(lists);
            Node node4 = new Node();
            node4.Position = new Point(2, 0, 0);
            node3.Frequence = 100;
            node4.TotalE = new EField(new Plural(2,2),new Plural(2,2),new Plural(2,2));
            List<Node> lists2 = new List<Node> { node1,node4};
            node4.RxNum = 1;
            CalculateModelClasses.Path path2 = new CalculateModelClasses.Path(lists2);
            List<CalculateModelClasses.Path> areaPaths = new List<CalculateModelClasses.Path> { path1,path2}; // TODO: 初始化为适当的值
            //areaPaths.Add(path1);
            //areaPaths.Add(path2);
            double[] expected = new double[2] { 0.00036444, 45.0 }; // TODO: 初始化为适当的值
            double[] actual;
            actual = Power.GetTotalPowerInDifferentPhase(areaPaths);
            Assert.IsTrue((Math.Abs(expected[0]-actual[0])<=0.000001)&&(Math.Abs(expected[1]-(actual[1]))<=0.000001));
            
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
        ///GetTotalPowerInDifferentPhase 的测试
        ///</summary>
        [TestMethod()]
        public void GetTotalPowerInDifferentPhaseTest1()
        {


            Node node1 = new Node();
            node1.Position = new Point(0, 0, 0);
            node1.Frequence = 100;
            Node node2 = new Node();
            node2.Frequence = 100;
            node2.Position = new Point(0,1,-1);
            Node node3 = new Node();
            node3.Position = new Point(0, 3, 0);
            node3.Frequence = 100;
            node3.TotalE = new EField(new Plural(1, 1), new Plural(1, 1), new Plural(1, 1));
            List<Node> lists = new List<Node> { node1, node2, node3 };
            node1.TxNum = 1; node3.RxNum = 1;
            CalculateModelClasses.Path path1 = new CalculateModelClasses.Path(lists);
            Node node4 = new Node();
            node4.Position = new Point(0, 3, 0);
            node4.Frequence = 100;
            node4.TotalE = new EField(new Plural(2, 2), new Plural(2, 2), new Plural(2, 2));
            Node node5 = new Node();
            node5.Position = new Point(0, 0, 0);
            node5.Frequence = 100;
            List<Node> lists2 = new List<Node> { node5, node4 };
            node4.RxNum = 1;
            CalculateModelClasses.Path path2 = new CalculateModelClasses.Path(lists2);
            List<CalculateModelClasses.Path> areaPaths = new List<CalculateModelClasses.Path> { path1, path2 }; // TODO: 初始化为适当的值
            //areaPaths.Add(path1);
            //areaPaths.Add(path2);
            double[] expected = new double[2] { 0.0692341, -135.0 }; // TODO: 初始化为适当的值
            double[] actual;
            actual = Power.GetTotalPowerInDifferentPhase(areaPaths);
            Assert.IsTrue((Math.Abs( expected[0] - (actual[0])) <= 0.0001) && (Math.Abs(expected[1]-(actual[1]))<= 0.0001));

        }
    }
}
