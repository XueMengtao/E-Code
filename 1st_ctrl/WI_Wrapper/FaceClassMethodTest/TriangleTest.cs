using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FaceClassMethodTest
{
    
    
    /// <summary>
    ///这是 TriangleTest 的测试类，旨在
    ///包含所有 TriangleTest 单元测试
    ///</summary>
    [TestClass()]
    public class TriangleTest
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
        ///JudgeIfPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInFaceTest1()
        {
            Triangle target = new Triangle(new Point(1, 0, 0), new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInFace(viewPoint);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInFaceTest2()
        {
            Triangle target = new Triangle(new Point(1, 0, 0), new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1, 0, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInFace(viewPoint);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInFaceTest3()
        {
            Triangle target = new Triangle(new Point(1, 0, 0), new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 0.5, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInFace(viewPoint);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInFaceTest4()
        {
            Triangle target = new Triangle(new Point(1, 0, 0), new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0.1, 0.5, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInFace(viewPoint);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInFaceTest5()
        {
            Triangle target = new Triangle(new Point(1, 0, 0), new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0.2, 0.5, 1); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInFace(viewPoint);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInFaceTest6()
        {
            Triangle target = new Triangle(new Point(1, 0, 0), new Point(0, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1.1, 0, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInFace(viewPoint);
            Assert.IsTrue(!actual);
        }



        /// <summary>
        ///GetTriangleAreaInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetTriangleAreaInXYTest1()
        {
            Triangle target = new Triangle(new Point(0, 0, 0), new Point(1, 0, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            double expected = 0.5; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetTriangleAreaInXY();
            Assert.IsTrue(Math.Abs(expected - actual) < 0.000001);
        }

        /// <summary>
        ///GetTriangleAreaInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetTriangleAreaInXYTest2()
        {
            Triangle target = new Triangle(new Point(0, 0, 0), new Point(0, 0, 1), new Point(1, 0, 1)); // TODO: 初始化为适当的值
            double expected = 0; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetTriangleAreaInXY();
            Assert.IsTrue(Math.Abs(expected - actual) < 0.000001);
        }

        /// <summary>
        ///GetTriangleAreaInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetTriangleAreaInXYTest3()
        {
            Triangle target = new Triangle(new Point(0, 0, 1), new Point(1, 0, 2), new Point(0, 1, 3)); // TODO: 初始化为适当的值
            double expected = 0.5; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetTriangleAreaInXY();
            Assert.IsTrue(Math.Abs(expected - actual) < 0.000001);
        }

        /// <summary>
        ///GetTriangleAreaInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetTriangleAreaInXYTest4()
        {
            Triangle target = new Triangle(new Point(0, 0, -3), new Point(1, 0, -1), new Point(0, 1, -2)); // TODO: 初始化为适当的值
            double expected = 0.5; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetTriangleAreaInXY();
            Assert.IsTrue(Math.Abs(expected - actual) < 0.000001);
        }
    }
}
