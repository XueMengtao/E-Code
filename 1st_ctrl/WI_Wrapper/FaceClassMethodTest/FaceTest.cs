using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FaceClassMethodTest
{
    
    
    /// <summary>
    ///这是 FaceTest 的测试类，旨在
    ///包含所有 FaceTest 单元测试
    ///</summary>
    [TestClass()]
    public class FaceTest
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


        internal virtual Face CreateFace()
        {
            // TODO: 实例化相应的具体类。
            Face target = null;
            return target;
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest1()
        {
            Face target = new Triangle(new Point(1, 0, 1), new Point(1, -1, -1), new Point(1, 1, -1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point expected = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest2()
        {
            Face target = new Triangle(new Point(1, 0, 1), new Point(1, -1, 0), new Point(1, 1, 0)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point expected = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest3()
        {
            Face target = new Triangle(new Point(1, 0, 1), new Point(1, -1, 0.1), new Point(1, 1, 0.1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest4()
        {
            Face target = new Triangle(new Point(-1, 0, 1), new Point(-1, -1, -1), new Point(-1, 1, -1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest5()
        {
            Face target = new Triangle(new Point(0, 1, 0), new Point(1, 1, 1), new Point(1, 1, -1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest6()
        {
            Face target = new Triangle(new Point(0, 0, 0), new Point(1, 0, 1), new Point(1, 0, -1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(actual == null);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest7()
        {
            Face target = new Triangle(new Point(0, -1, 0), new Point(2, 1, 1), new Point(2, 1, -1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 0, 0)); // TODO: 初始化为适当的值
            Point expected = new Point(1, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetCrossPointWithRay 的测试
        ///</summary>
        [TestMethod()]
        public void GetCrossPointWithRayTest8()
        {
            Face target = new Triangle(new Point(1, 0, 0), new Point(1, 0, 2), new Point(1, 2, 1)); // TODO: 初始化为适当的值
            RayInfo oneRay = new RayInfo(new Point(0, 0, 0), new SpectVector(1, 1, 1)); // TODO: 初始化为适当的值
            Point expected = new Point(1, 1, 1); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetCrossPointWithRay(oneRay);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }


        /// <summary>
        ///GetProjectionPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void GetProjectionPointInFaceTest1()
        {
            Face target = new SpaceFace(new Point(1, 1, 0), new Point(1, -1, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetProjectionPointInFace(viewPoint);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetProjectionPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void GetProjectionPointInFaceTest2()
        {
            Face target = new SpaceFace(new Point(1, 1, 0), new Point(1, -1, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 0, 1); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetProjectionPointInFace(viewPoint);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetProjectionPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void GetProjectionPointInFaceTest3()
        {
            Face target = new SpaceFace(new Point(1, 1, 0), new Point(1, -1, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(0, 0, -1); // TODO: 初始化为适当的值
            Point expected = new Point(0, 0, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetProjectionPointInFace(viewPoint);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetProjectionPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void GetProjectionPointInFaceTest4()
        {
            Face target = new SpaceFace(new Point(1, 1, 0), new Point(1, -1, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(1, 2, 3); // TODO: 初始化为适当的值
            Point expected = new Point(1, 2, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetProjectionPointInFace(viewPoint);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }

        /// <summary>
        ///GetProjectionPointInFace 的测试
        ///</summary>
        [TestMethod()]
        public void GetProjectionPointInFaceTest5()
        {
            Face target = new SpaceFace(new Point(1, 1, 0), new Point(1, -1, 0), new Point(0, 0, 0)); // TODO: 初始化为适当的值
            Point viewPoint = new Point(10, 10, -10); // TODO: 初始化为适当的值
            Point expected = new Point(10, 10, 0); // TODO: 初始化为适当的值
            Point actual;
            actual = target.GetProjectionPointInFace(viewPoint);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.000001 && Math.Abs(expected.Y - actual.Y) < 0.000001 &&
                Math.Abs(expected.Z - actual.Z) < 0.000001);
        }
    }
}
