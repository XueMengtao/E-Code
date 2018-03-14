using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PointClassMethodTest
{
    
    
    /// <summary>
    ///这是 PointTest 的测试类，旨在
    ///包含所有 PointTest 单元测试
    ///</summary>
    [TestClass()]
    public class PointTest
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
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest1()
        {
            Point target = new Point(0, 0, 0); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 0), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest2()
        {
            Point target = new Point(0, 0, 0); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 0), new Point(-1, 0, 4)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest3()
        {
            Point target = new Point(0, 0, 1); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 0), new Point(-1, 0, -2)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest4()
        {
            Point target = new Point(2, 0, 1); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 1), new Point(-1, 0, 1)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest5()
        {
            Point target = new Point(2, 0, 1); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 0), new Point(0, 0, 3)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest6()
        {
            Point target = new Point(2, 0, 1); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 0), new Point(0, 3, 3)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest7()
        {
            Point target = new Point(1, 0, 1); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, 0), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIfPointInLineInXY 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIfPointInLineInXYTest8()
        {
            Point target = new Point(1, 0, 1); // TODO: 初始化为适当的值
            LineSegment line = new LineSegment(new Point(1, 0, -1), new Point(-1, 1, 4)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIfPointInLineInXY(line);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest1()
        {
            Point target = new Point(0, -1, 0); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest2()
        {
            Point target = new Point(0, 1, 0); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest3()
        {
            Point target = new Point(0, -1, 3); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest4()
        {
            Point target = new Point(1, 1, 0); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest5()
        {
            Point target = new Point(1, 1, 4); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest6()
        {
            Point target = new Point(0, -1, 0); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest7()
        {
            Point target = new Point(0, -1, 0); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(1, 0, 1), new Point(1, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///JudgeIsConcaveOrConvexToViewPoint 的测试
        ///</summary>
        [TestMethod()]
        public void JudgeIsConcaveOrConvexToViewPointTest8()
        {
            Point target = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Face diffractionFace1 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Face diffractionFace2 = new Triangle(new Point(0, 0, 1), new Point(0, 0, -1), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            bool actual;
            actual = target.JudgeIsConcaveOrConvexToViewPoint(diffractionFace1, diffractionFace2);
            Assert.IsTrue(!actual);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest1()
        {
            Point target = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, 1, 0); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest2()
        {
            Point target = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, -1, 0); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest3()
        {
            Point target = new Point(1, 0, 5); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, 1, -5); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest4()
        {
            Point target = new Point(1, 0, 5); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, 1, 4); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest5()
        {
            Point target = new Point(1, 0, 1); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, 2, -2); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest6()
        {
            Point target = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, 0, -1); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(-1, -1, -1), new Point(1, 1, 1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest7()
        {
            Point target = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point otherPoint = new Point(1, 2, 3); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetDiffractionPostion 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPostionTest8()
        {
            Point target = new Point(0, 0, 2); // TODO: 初始化为适当的值
            Point otherPoint = new Point(0, 1, 0); // TODO: 初始化为适当的值
            AdjacentEdge diffractionEdge = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPostion(otherPoint, diffractionEdge);
            Assert.IsTrue(actual == null);
        }
    }
}
