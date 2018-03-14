using RayCalInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CalculateModelClasses;
using System.IO;
using UanFileProceed;

namespace DirectEfieldTest
{
    
    
    /// <summary>
    ///这是 DirectEfieldCalTest 的测试类，旨在
    ///包含所有 DirectEfieldCalTest 单元测试
    ///</summary>
    [TestClass()]
    public class DirectEfieldCalTest
    {


        private TestContext testContextInstance;
        private string uanstr;

        //构造函数
        public DirectEfieldCalTest()
        {
            
            FileStream fs = new FileStream("../../../DirectEfieldTest/bin/Debug/short_monopole.uan", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string str = sr.ReadToEnd();
            if (sr.EndOfStream)
            {
                //sr.Close();
                //fs.Close();
                using (sr)
                { }
                using (fs)
                { }
            }
            uanstr = str;
            
        }

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
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest1()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 0;
            expected.X.Im = 0;
            expected.Y.Re = -3.8729833462074;
            expected.Y.Im = -6.70820393249938;
            expected.Z.Re = 6.66493056834915;
            expected.Z.Im = 11.5439983732997;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest2()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(0, 1, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 3.8729833462074;
            expected.X.Im = 6.70820393249938;
            expected.Y.Re = 0;
            expected.Y.Im = 0;
            expected.Z.Re = 6.66493056834915;
            expected.Z.Im = 11.5439983732997;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest3()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(-1, 0, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 0;
            expected.X.Im = 0;
            expected.Y.Re = 3.8729833462074;
            expected.Y.Im = 6.70820393249938;
            expected.Z.Re = 6.66493056834915;
            expected.Z.Im = 11.5439983732997;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest4()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(0, -1, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = -3.8729833462074;
            expected.X.Im = -6.70820393249938;
            expected.Y.Re = 0;
            expected.Y.Im = 0;
            expected.Z.Re = 6.66493056834915;
            expected.Z.Im = 11.5439983732997;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest5()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, 1, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 0.867524790988055;
            expected.X.Im = -3.77457292114235;
            expected.Y.Re = -0.867524790988055;
            expected.Y.Im = 3.77457292114236;
            expected.Z.Re = 2.11128514574735;
            expected.Z.Im = -9.18613487791125;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest6()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(-1, 1, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 0.867524790988055;
            expected.X.Im = -3.77457292114235;
            expected.Y.Re = 0.867524790988055;
            expected.Y.Im = -3.77457292114236;
            expected.Z.Re = 2.11128514574735;
            expected.Z.Im = -9.18613487791125;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest7()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(-1, -1, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = -0.867524790988055;
            expected.X.Im = 3.77457292114235;
            expected.Y.Re = 0.867524790988055;
            expected.Y.Im = -3.77457292114236;
            expected.Z.Re = 2.11128514574735;
            expected.Z.Im = -9.18613487791125;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest8()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, -1, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = -0.867524790988055;
            expected.X.Im = 3.77457292114235;
            expected.Y.Re = -0.867524790988055;
            expected.Y.Im = 3.77457292114236;
            expected.Z.Re = 2.11128514574735;
            expected.Z.Im = -9.18613487791125;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest9()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, Math.Sqrt(3) / 3, 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = -1.95443401965821;
            expected.X.Im = -2.72583705727299;
            expected.Y.Re = 3.38517902208908;
            expected.Y.Im = 4.72128827635085;
            expected.Z.Re = -6.72668373552254;
            expected.Z.Im = -9.38166426413794;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest10()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, Math.Sqrt(3), 0); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 1.67705098312488;
            expected.X.Im = -2.90473750965554;
            expected.Y.Re = -0.968245836551877;
            expected.Y.Im = 1.67705098312483;
            expected.Z.Re = 3.33246528417467;
            expected.Z.Im = -5.77199918664981;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest11()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, 0, 1); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = -1.05564268913923;
            expected.X.Im = 4.59306794482337;
            expected.Y.Re = -1.22686532511019;
            expected.Y.Im = 5.33805221724575;
            expected.Z.Re = 1.05564268913923;
            expected.Z.Im = -4.59306794482337;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest12()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, 0, Math.Sqrt(3) / 3); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 2.9127396218559;
            expected.X.Im = 4.06237996247662;
            expected.Y.Re = 3.90886803931641;
            expected.Y.Im = 5.45167411454598;
            expected.Z.Re = -5.04501301427337;
            expected.Z.Im = -7.03624849465925;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest13()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, 0, Math.Sqrt(3)); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = -1.44299978225551;
            expected.X.Im = 2.4993489381773;
            expected.Y.Re = -1.93649167310375;
            expected.Y.Im = 3.35410196624966;
            expected.Z.Re = 0.833116312725791;
            expected.Z.Im = -1.44299978225547;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest14()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(0, 0, 1); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 0;
            expected.X.Im = 0;
            expected.Y.Re = -3.8729833462074;
            expected.Y.Im = -6.70820393249938;
            expected.Z.Re = 0;
            expected.Z.Im = 0;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///EfieldCal 的测试
        ///</summary>
        [TestMethod()]
        public void EfieldCalTest15()
        {
            string uan = uanstr; // TODO: 初始化为适当的值
            ReadUan.GetGainPara(uan);
            double power1 = 1; // TODO: 初始化为适当的值
            double frequency = 1000; // TODO: 初始化为适当的值
            Point originPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point targetPoint = new Point(1, 1, Math.Sqrt(2)); // TODO: 初始化为适当的值
            Point rotateAngle = null; // TODO: 初始化为适当的值
            EField expected = new EField(); // TODO: 初始化为适当的值
            expected.X.Re = 0.191101863744437;
            expected.X.Im = -0.330998137426465;
            expected.Y.Re = -2.54751092378142;
            expected.Y.Im = 4.41241835282609;
            expected.Z.Re = 1.66623282560157;
            expected.Z.Im = -2.88599991118093;
            EField actual;
            actual = DirectEfieldCal.EfieldCal(uan, power1, frequency, originPoint, targetPoint, rotateAngle);
            Assert.IsTrue((Math.Abs(expected.X.Re - actual.X.Re) < 0.00001) && (Math.Abs(expected.X.Im - actual.X.Im) < 0.00001) &&
                (Math.Abs(expected.Y.Re - actual.Y.Re) < 0.00001) && (Math.Abs(expected.Y.Im - actual.Y.Im) < 0.00001) &&
                (Math.Abs(expected.Z.Re - actual.Z.Re) < 0.00001) && (Math.Abs(expected.Z.Im - actual.Z.Im) < 0.00001));
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

    }
}
