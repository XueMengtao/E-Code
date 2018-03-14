using RayCalInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CalculateModelClasses;

namespace DirectEfieldTest
{
    
    
    /// <summary>
    ///这是 DiffractEfiledCalTest 的测试类，旨在
    ///包含所有 DiffractEfiledCalTest 单元测试
    ///</summary>
    [TestClass()]
    public class DiffractEfiledCalTest
    {


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
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest1()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(-1, -1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0556213074619009, -0.306408583568637), new Plural(-0.0556213074619009, 0.306408583568637), new Plural(0.265391841563493, - 0.134776741125432)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest2()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(-1, 1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.118904205160571, -0.205230457800243), new Plural(0.118904205160571, -0.205230457800243),
                new Plural(0.0185754946326771, 0.588790130218008)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest3()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(-0.103022931940275, - 0.0280030238843371), new Plural(0, 0), 
                new Plural(0.103022931940275,0.0280030238843371)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest4()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1, 1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.113381375385091, -0.319990762442036), new Plural(-0.113381375385091, 0.319990762442036),
                new Plural(-0.120214209223646, 0.326749074964341)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest5()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(0, 1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, -1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.103022931940275, 0.0280030238843371), new Plural(0, 0),
                new Plural(0.103022931940275, 0.0280030238843371)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest6()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(1/Math.Sqrt(2), 0), new Plural(1, 0), new Plural(1/Math.Sqrt(2), 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 1), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 1, -1); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.166975560281137, -0.46209581030037), new Plural(-0.118069550967215, 0.326751081021284),
                new Plural(-0.118069550967215, 0.326751081021284)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest7()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(1/Math.Sqrt(2), 0), new Plural(1, 0), new Plural(1/Math.Sqrt(2), 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 1), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(Math.Sqrt(2)/2, Math.Sqrt(2)/2, -1); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0410495728503019, -0.10367673448101), new Plural(-0.135595898900246, 0.315870738160379),
                new Plural(-0.06685434828619, 0.150043818928805)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest8()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(1/Math.Sqrt(2), 0), new Plural(1, 0), new Plural(1/Math.Sqrt(2), 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 1), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 0, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(-Math.Sqrt(2)/2, -Math.Sqrt(2)/2, -1); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(-0.0928766295368952, -0.229962605130961), new Plural(-0.158037944306427, 0.219740939107947),
                new Plural(0.177423396663146, 0.00722780935989699)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest9()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(-1, -1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0762769421881891, -0.272243710594031), new Plural(-0.0762769421881891, 0.272243710594031),
                new Plural(0.1078718861388886, -0.385010747792854)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetDiffractionEField 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionEFieldTest10()
        {
            EField RayEFieldAtDiffractionPoint = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo ray = new RayInfo(new Point(-1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, -1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace1.Material = new TerMaterial("", 0, 2, 0, 0);
            Face diffractionFace2 = new Triangle(new Point(1, 1, 0), new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            diffractionFace2.Material = new TerMaterial("", 0, 2, 0, 0);
            Point diffractionPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point viewPoint = new Point(-1, -1, 0); // TODO: 初始化为适当的值
            double frequence = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0251959882308153, -0.35642556335775), new Plural(-0.0251959882308153, 0.35642556335775),
                new Plural(0.205069242257395, -0.233019898107341)); // TODO: 初始化为适当的值
            EField actual;
            actual = DiffractEfiledCal.GetDiffractionEField(RayEFieldAtDiffractionPoint, ray, diffractionFace1, diffractionFace2, diffractionPoint, viewPoint, frequence);
            Assert.IsTrue(Math.Abs(expected.X.Re - actual.X.Re) < 0.000001 && Math.Abs(expected.X.Im - actual.X.Im) < 0.000001
                && Math.Abs(expected.Y.Re - actual.Y.Re) < 0.000001 && Math.Abs(expected.Y.Im - actual.Y.Im) < 0.000001
                && Math.Abs(expected.Z.Re - actual.Z.Re) < 0.000001 && Math.Abs(expected.Z.Im - actual.Z.Im) < 0.000001);
            //Assert.AreEqual(expected, actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
