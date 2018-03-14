using RayCalInfo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using CalculateModelClasses;

namespace DirectEfieldTest
{
    
    
    /// <summary>
    ///这是 ReflectEfieldCalTest 的测试类，旨在
    ///包含所有 ReflectEfieldCalTest 单元测试
    ///</summary>
    [TestClass()]
    public class ReflectEfieldCalTest
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
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest1()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, -3), new Point(6, -2, 3), new Point(6, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1,0,0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 4.1; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0, 0),
                new Plural(-0.0847023107852173, -0.146708705798483), new Plural(-0.0847023107852173, -0.146708705798483)); ; // TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re-expected.X.Re)<0.0000001&&Math.Abs(actual.X.Im-expected.X.Im)<0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest2()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 / Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1,Math.Sqrt(3),0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 4.1; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0623877164578374,0.108058694673174),
                new Plural(0.0360195648910585,0.0623877164578365), new Plural(0.0968928150328006, 0.167823278525182));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest3()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 * Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 4.1; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0122584145400459,0.0212321968036003),
                new Plural(-0.00707739893453353,-0.0122584145400457), new Plural(0.142717699857073,0.247194307291812));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest4()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 6.2; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0, 0),
                new Plural(-0.106732700038679,-0.184866459295999), new Plural(-0.106732600038679,-0.184866459295999));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest5()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(10, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(0,1, 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 6.2; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0737128450442626,0.127674392787113),
                new Plural(0,0), new Plural(0.135750547921789,0.235126846155848));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest6()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 * Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara =6.2; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.0304958700190565,0.0528203962940216),
                new Plural(-0.0176067987646741,-0.0304958700190561), new Plural(0.161801754519918,0.28024885958228));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest7()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 / Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 80.4; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.167173715239667,0.289553368485151),
                new Plural(0.0965177894950518,0.167173715239665), new Plural(0.205899086049201,0.356627678269207));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest8()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(10, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 80.4; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.182066642851485,0.315348675782264),
                new Plural(0,0), new Plural(0.213346339815971,0.36952670017011));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest9()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 * Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 80.4; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.137839140789951,0.238744395119831),
                new Plural(-0.0795814650399449,-0.137839140789949), new Plural(0.223473914142364,0.387068173460853));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest10()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 10000000; // TODO: 初始化为适当的值
            double Epara = 0; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0,0),
                new Plural(-0.250019286421755,-0.432940708809809), new Plural(-0.250019286421755,-0.432940708809809));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest11()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 / Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 10000000; // TODO: 初始化为适当的值
            double Epara = 0; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.216525636623664,0.374928007347336),
                new Plural(0.125011134591127,0.216464785968714), new Plural(0.250016703089389,0.432950353731651));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest12()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(10, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 10000000; // TODO: 初始化为适当的值
            double Epara = 0; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(0.2500272723013,0.432910889925977),
                new Plural(0,0), new Plural(0.25001363855995,0.432961794520133));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest13()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 / Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(-1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 1/80.4; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(-0.232861639196329,-0.365069112622252),
                new Plural(-0.134442730073938,-0.210772750445275), new Plural(0.240433653861357,-0.43839669032828));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest14()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(10, 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 1/80.4; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(-0.26082694568263,-0.426578602845795),
                new Plural(0,0), new Plural(0.429829743600304,-0.25543373214299));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ReflectEfield 的测试
        ///</summary>
        [TestMethod()]
        public void ReflectEfieldTest15()
        {
            EField e = new EField(new Plural(0, 0), new Plural(1, 0), new Plural(1, 0)); // TODO: 初始化为适当的值
            RayInfo rayIn = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face face = new SpaceFace(new Point(6, -2, 3), new Point(6, -2, -3), new Point(6 + 4 * Math.Sqrt(3), 2, 0));
            RayInfo rayOut = new RayInfo(new Point(0, 0, 0), new Point(1, Math.Sqrt(3), 0)); // TODO: 初始化为适当的值
            SpectVector l = face.GetNormalVector(); // TODO: 初始化为适当的值
            double Conduct = 0; // TODO: 初始化为适当的值
            double Epara = 1/80.4; // TODO: 初始化为适当的值
            double s1 = 1; // TODO: 初始化为适当的值
            double s2 = 1; // TODO: 初始化为适当的值
            double f = 1000; // TODO: 初始化为适当的值
            EField expected = new EField(new Plural(-0.221914313088277,-0.37182527838564),
                new Plural(0.128122288398547,0.214673424567456), new Plural(0.49998683666538,-0.00362810713003811));// TODO: 初始化为适当的值
            EField actual = new EField();
            actual = ReflectEfieldCal.ReflectEfield(e, rayIn, rayOut, l, Conduct, Epara, s1, s2, f);
            Assert.IsTrue(Math.Abs(actual.X.Re - expected.X.Re) < 0.0000001 && Math.Abs(actual.X.Im - expected.X.Im) < 0.0000001
                && Math.Abs(actual.Y.Re - expected.Y.Re) < 0.0000001 && Math.Abs(actual.Y.Im - expected.Y.Im) < 0.0000001
                && Math.Abs(actual.Z.Re - expected.Z.Re) < 0.0000001 && Math.Abs(actual.Z.Im - expected.Z.Im) < 0.0000001);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

    }
}
