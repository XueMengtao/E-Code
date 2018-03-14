using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace LineClassMethodTest
{


    /// <summary>
    ///这是 AdjacentEdgeTest 的测试类，旨在
    ///包含所有 AdjacentEdgeTest 单元测试
    ///</summary>
    [TestClass()]
    public class AdjacentEdgeTest
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
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest1()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 0); // TODO: 初始化为适当的值
            Point target1 = new Point(-1, -1, 0); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest2()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 0); // TODO: 初始化为适当的值
            Point target1 = new Point(-1, 1, 0); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest3()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 1); // TODO: 初始化为适当的值
            Point target1 = new Point(0, 1, -1); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest4()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 1); // TODO: 初始化为适当的值
            Point target1 = new Point(Math.Sqrt(2)/2,Math.Sqrt(2)/2, -1); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest5()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 1); // TODO: 初始化为适当的值
            Point target1 = new Point(1, 0, 1); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 1); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest6()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 0); // TODO: 初始化为适当的值
            Point target1 = new Point(1, 0, 2); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 1); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetDiffractionPoint 的测试
        ///</summary>
        [TestMethod()]
        public void GetDiffractionPointTest7()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point origin = new Point(-1, 0, 0); // TODO: 初始化为适当的值
            Point target1 = new Point(1, 0, 3); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetDiffractionPoint(origin, target1);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///getVectorFromEdge2EdgeWithAngle 的测试
        ///</summary>
        [TestMethod()]
        public void getVectorFromEdge2EdgeWithAngleTest1()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point beginPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            double angle = 90; // TODO: 初始化为适当的值
            AdjacentEdge targetEdge = new AdjacentEdge(new Point(1, 0, 1), new Point(1, 0, -1)); // TODO: 初始化为适当的值
            RayInfo expected = new RayInfo(new Point(1, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo actual;
            List<RayInfo> list = new List<RayInfo>();
            list = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            actual = list[0];
            //actual = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            Assert.IsTrue(Math.Abs(expected.Origin.X - actual.Origin.X) < 0.000001 && Math.Abs(expected.Origin.Y - actual.Origin.Y) < 0.000001 &&
                Math.Abs(expected.Origin.Z - actual.Origin.Z) < 0.000001 && Math.Abs(expected.RayVector.a - actual.RayVector.a) < 0.000001 &&
                Math.Abs(expected.RayVector.b - actual.RayVector.b) < 0.000001 && Math.Abs(expected.RayVector.c - actual.RayVector.c) < 0.000001);
        }

        /// <summary>
        ///getVectorFromEdge2EdgeWithAngle 的测试
        ///</summary>
        //[TestMethod()]
        //public void getVectorFromEdge2EdgeWithAngleTest2()
        //{
        //    AdjacentEdge target = new AdjacentEdge(new Point(0, 1, 1), new Point(0, -1, 1)); // TODO: 初始化为适当的值
        //    Point beginPoint = new Point(0, 1, 1); // TODO: 初始化为适当的值
        //    double angle = 135; // TODO: 初始化为适当的值
        //    AdjacentEdge targetEdge = new AdjacentEdge(new Point(0, -2, 1), new Point(0, -2, -2)); // TODO: 初始化为适当的值
        //    RayInfo expected1 = new RayInfo(new Point(0, -2, 2), new SpectVector(0, -1, 1));// TODO: 初始化为适当的值
        //    RayInfo expected2 = new RayInfo(new Point(0, -2, 0), new SpectVector(0, -1, -1));
        //    RayInfo actual;
        //    List<RayInfo> list = new List<RayInfo>();
        //    list = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            
        //    //actual = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
        //    //Assert.IsTrue(Math.Abs(expected.Origin.X - actual.Origin.X) < 0.000001 && Math.Abs(expected.Origin.Y - actual.Origin.Y) < 0.000001 &&
        //    //    Math.Abs(expected.Origin.Z - actual.Origin.Z) < 0.000001 && Math.Abs(expected.RayVector.a - actual.RayVector.a) < 0.000001 &&
        //    //    Math.Abs(expected.RayVector.b - actual.RayVector.b) < 0.000001 && Math.Abs(expected.RayVector.c - actual.RayVector.c) < 0.000001);
        //}

        /// <summary>
        ///getVectorFromEdge2EdgeWithAngle 的测试
        ///</summary>
        [TestMethod()]
        public void getVectorFromEdge2EdgeWithAngleTest3()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point beginPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            double angle = 135; // TODO: 初始化为适当的值
            AdjacentEdge targetEdge = new AdjacentEdge(new Point(-1, 0, 1), new Point(-1, 0, -1)); // TODO: 初始化为适当的值
            RayInfo expected = new RayInfo(new Point(-1, 0, 1), new SpectVector(-1, 0, 1)); // TODO: 初始化为适当的值
            RayInfo actual;
            List<RayInfo> list = new List<RayInfo>();
            list = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            actual = list[0];
            Assert.IsTrue(Math.Abs(expected.Origin.X - actual.Origin.X) < 0.000001 && Math.Abs(expected.Origin.Y - actual.Origin.Y) < 0.000001 &&
                Math.Abs(expected.Origin.Z - actual.Origin.Z) < 0.000001 && Math.Abs(expected.RayVector.a - actual.RayVector.a) < 0.000001 &&
                Math.Abs(expected.RayVector.b - actual.RayVector.b) < 0.000001 && Math.Abs(expected.RayVector.c - actual.RayVector.c) < 0.000001);
        }

        /// <summary>
        ///getVectorFromEdge2EdgeWithAngle 的测试
        ///</summary>
        [TestMethod()]
        public void getVectorFromEdge2EdgeWithAngleTest4()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point beginPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            double angle = 90; // TODO: 初始化为适当的值
            AdjacentEdge targetEdge = new AdjacentEdge(new Point(1, 1, 0), new Point(1, -1,0)); // TODO: 初始化为适当的值
            RayInfo expected = new RayInfo(new Point(1, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            RayInfo actual;
            List<RayInfo> list = new List<RayInfo>();
            list = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            actual = list[0];
            //actual = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            Assert.IsTrue(Math.Abs(expected.Origin.X - actual.Origin.X) < 0.000001 && Math.Abs(expected.Origin.Y - actual.Origin.Y) < 0.000001 &&
                Math.Abs(expected.Origin.Z - actual.Origin.Z) < 0.000001 && Math.Abs(expected.RayVector.a - actual.RayVector.a) < 0.000001 &&
                Math.Abs(expected.RayVector.b - actual.RayVector.b) < 0.000001 && Math.Abs(expected.RayVector.c - actual.RayVector.c) < 0.000001);
        }

        /// <summary>
        ///getVectorFromEdge2EdgeWithAngle 的测试
        ///</summary>
        [TestMethod()]
        public void getVectorFromEdge2EdgeWithAngleTest5()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point beginPoint = new Point(0, 0, 1); // TODO: 初始化为适当的值
            double angle = 45; // TODO: 初始化为适当的值
            AdjacentEdge targetEdge = new AdjacentEdge(new Point(0, -1, 1), new Point(0, -1, -1)); // TODO: 初始化为适当的值
            RayInfo expected = new RayInfo(new Point(0, -1, 0), new SpectVector(0, -1, -1)); // TODO: 初始化为适当的值
            RayInfo actual;
            List<RayInfo> list = new List<RayInfo>();
            list = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            actual = list[0];
        
            //actual = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            Assert.IsTrue(Math.Abs(expected.Origin.X - actual.Origin.X) < 0.000001 && Math.Abs(expected.Origin.Y - actual.Origin.Y) < 0.000001 &&
                Math.Abs(expected.Origin.Z - actual.Origin.Z) < 0.000001 && Math.Abs(expected.RayVector.a - actual.RayVector.a) < 0.000001 &&
                Math.Abs(expected.RayVector.b - actual.RayVector.b) < 0.000001 && Math.Abs(expected.RayVector.c - actual.RayVector.c) < 0.000001);
        }

        /// <summary>
        ///getVectorFromEdge2EdgeWithAngle 的测试
        ///</summary>
        [TestMethod()]
        public void getVectorFromEdge2EdgeWithAngleTest6()
        {
            AdjacentEdge target = new AdjacentEdge(new Point(0, 0, 1), new Point(0, 0, -1)); // TODO: 初始化为适当的值
            Point beginPoint = new Point(0, 0, -1); // TODO: 初始化为适当的值
            double angle = 60; // TODO: 初始化为适当的值
            AdjacentEdge targetEdge = new AdjacentEdge(new Point(1, 1, 0), new Point(1, -1, 0)); // TODO: 初始化为适当的值
            //RayInfo actual;
            List<RayInfo> list = new List<RayInfo>();
            list = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            
            //actual = target.getVectorFromEdge2EdgeWithAngle(beginPoint, angle, targetEdge);
            Assert.IsTrue(list.Count == 0);
        }
    }
}
