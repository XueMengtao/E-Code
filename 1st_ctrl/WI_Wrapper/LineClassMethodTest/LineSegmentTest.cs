using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace LineClassMethodTest
{
    
    
    /// <summary>
    ///这是 LineSegmentTest 的测试类，旨在
    ///包含所有 LineSegmentTest 单元测试
    ///</summary>
    [TestClass()]
    public class LineSegmentTest
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
        ///JudgeIfPointInLineRange 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineRangeTest1()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(2, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineRange(viewPoint);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInLineRange 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineRangeTest2()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(2, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1, 0, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineRange(viewPoint);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInLineRange 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineRangeTest3()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(2, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(3, 0, 0); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineRange(viewPoint);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInLineRange 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineRangeTest4()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(2, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1, 0, 1); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineRange(viewPoint);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInLineRange 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineRangeTest5()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(2, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(3, -9, 4); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineRange(viewPoint);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInLineRange 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineRangeTest6()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(2, 2, 2)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1, 1, 1); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineRange(viewPoint);
            Assert.IsTrue(actual);
        }



        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest1()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(0, 0, -1), new Point(0, 0, -2)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest2()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(1, 0, 1), new Point(2, 0, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest3()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(1, 0, 1), new Point(-1, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest4()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest5()
        {
            LineSegment target = new LineSegment(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(0, 0, 1), new Point(-1, -1, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest6()
        {
            LineSegment target = new LineSegment(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(0, 0, -1), new Point(-1, -1, -1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest7()
        {
            LineSegment target = new LineSegment(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(0, 0, 0), new Point(1, 1, 4)); // TODO: 初始化为适当的值
            Point expected = new Point(0.5, 0.5, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest8()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 10), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(0, 0, 0), new Point(1, 1, 9)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherLineInXYTest9()
        {
            LineSegment target = new LineSegment(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            LineSegment otherLine = new LineSegment(new Point(-1, 1, 0), new Point(-1, -1, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherLineInXY(otherLine);
            Assert.IsTrue(actual == null);
        }
    }
}
