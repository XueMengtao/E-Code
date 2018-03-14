using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RayClassMethodTest
{
    
    
    /// <summary>
    ///这是 RayInfoTest 的测试类，旨在
    ///包含所有 RayInfoTest 单元测试
    ///</summary>
    [TestClass()]
    public class RayInfoTest
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
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest1()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(0, 0, 1), new Point(1, 0, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest2()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(0, 0, 0), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest3()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(0, -1, 0), new Point(0, 1, 0)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest4()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(0, 1, 0), new Point(0, 2, 0)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest5()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(2, 0, 1), new Point(2, 0, -1)); // TODO: 初始化为适当的值
            Point expected = new Point(2, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest6()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 1, 1)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(-1, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithOtherRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithOtherRayTest7()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(0, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithOtherRay(otherRay);
            Assert.IsTrue(actual == null);
        }

        ///// <summary>
        /////GetDistanceOfNonUniplanarRays 的测试
        /////</summary>
        //[TestMethod()]
        //public void GetDistanceOfNonUniplanarRaysTest1()
        //{
        //    RayInfo target = new RayInfo(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
        //    RayInfo otherRay = new RayInfo(new Point(1, 0, 1), new Point(1, 0, -1)); // TODO: 初始化为适当的值
        //    double expected = -1; // TODO: 初始化为适当的值
        //    double actual;
        //    actual = target.GetDistanceOfNonUniplanarRays(otherRay);
        //    Assert.IsTrue(Math.Abs(actual - expected) < 0.001);
        //}

        /// <summary>
        ///GetDistanceOfNonUniplanarRays 的测试
        ///</summary>
        [TestMethod()]
        public void GetDistanceOfNonUniplanarRaysTest2()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(1, 0, 0), new Point(-1, 0, 0)); // TODO: 初始化为适当的值
            double expected = 0; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetDistanceOfNonUniplanarRays(otherRay);
            Assert.IsTrue(Math.Abs(actual - expected) < 0.000001);
        }

        /// <summary>
        ///GetDistanceOfNonUniplanarRays 的测试
        ///</summary>
        [TestMethod()]
        public void GetDistanceOfNonUniplanarRaysTest3()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(1, 1, 0), new Point(1, -1, 0)); // TODO: 初始化为适当的值
            double expected = 1; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetDistanceOfNonUniplanarRays(otherRay);
            Assert.IsTrue(Math.Abs(actual - expected) < 0.000001);
        }

        /// <summary>
        ///GetDistanceOfNonUniplanarRays 的测试
        ///</summary>
        [TestMethod()]
        public void GetDistanceOfNonUniplanarRaysTest4()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(1, 1, 0), new Point(2, 1, 0)); // TODO: 初始化为适当的值
            double expected = 1; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetDistanceOfNonUniplanarRays(otherRay);
            Assert.IsTrue(Math.Abs(actual - expected) < 0.000001);
        }

        /// <summary>
        ///GetDistanceOfNonUniplanarRays 的测试
        ///</summary>
        [TestMethod()]
        public void GetDistanceOfNonUniplanarRaysTest5()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            RayInfo otherRay = new RayInfo(new Point(1, 1, 1), new Point(-1, 1, -1)); // TODO: 初始化为适当的值
            double expected = 1; // TODO: 初始化为适当的值
            double actual;
            actual = target.GetDistanceOfNonUniplanarRays(otherRay);
            Assert.IsTrue(Math.Abs(actual - expected) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest1()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(0, 0, -1), new Point(0, 0, -2)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(actual == null); 
        }

        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest2()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(1, 0, 1), new Point(2, 0, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(actual == null);
        }


        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest3()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(0, 0, 1)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest4()
        {
            RayInfo target = new RayInfo(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(0, 0, 1), new Point(-1, -1, 1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest5()
        {
            RayInfo target = new RayInfo(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(0, 0, -1), new Point(-1, -1, -1)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest6()
        {
            RayInfo target = new RayInfo(new Point(1, 0, 1), new Point(0, 1, 1)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(0, 0, 0), new Point(1, 1, 4)); // TODO: 初始化为适当的值
            Point expected = new Point(0.5, 0.5, 0); // TOD   O: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        
        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest7()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(-1, 1, 0), new Point(-1, -1, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWtihLineInXYPlane 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWtihLineInXYPlaneTest8()
        {
            RayInfo target = new RayInfo(new Point(1, 0, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            LineSegment oneLine = new LineSegment(new Point(-1, 1, 1), new Point(-1, -1, 1)); // TODO: 初始化为适当的值
            Point expected = new Point(-1, 0, 0); // TOD   O: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWtihLineInXYPlane(oneLine);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///getDropFoot 的测试
        ///</summary>
        [TestMethod()]
        public void getDropFootTest1()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Point p = new Point(1, 0, 1); // TODO: 初始化为适当的值
            Point expected = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.getDropFoot(p);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///getDropFoot 的测试
        ///</summary>
        [TestMethod()]
        public void getDropFootTest2()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Point p = new Point(2, 1, 0); // TODO: 初始化为适当的值
            Point expected = new Point(2, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.getDropFoot(p);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///getDropFoot 的测试
        ///</summary>
        [TestMethod()]
        public void getDropFootTest3()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Point p = new Point(-1, -2, 0); // TODO: 初始化为适当的值
            Point expected = new Point(-1, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.getDropFoot(p);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///getDropFoot 的测试
        ///</summary>
        [TestMethod()]
        public void getDropFootTest4()
        {
            RayInfo target = new RayInfo(new Point(0, 0, 0), new Point(1, 0, 0)); // TODO: 初始化为适当的值
            Point p = new Point(0, 0, 1); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.getDropFoot(p);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }
    }
}
