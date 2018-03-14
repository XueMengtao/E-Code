using CalculateModelClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace GetPointsOfContainOneDiffractPathTest
{


    /// <summary>
    ///这是 PathTest 的测试类，旨在
    ///包含所有 PathTest 单元测试
    ///</summary>
    [TestClass()]
    public class PathTest
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
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        // Tx-D-Rx的测试

        //在棱外部
        public void GetPointsOfContainOneDiffractPathTest0()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(1, 0, 0), new Point(-1, 0, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.Rx;
            node3.Position = new Point(-4, 0, 0);
            List<Node> nodes = new List<Node>() { node1, node2, node3 };
            Path_Accessor target = new Path_Accessor(nodes); // TODO: 初始化为适当的值
            int indexOfEdgeOfThisPath = 1; // TODO: 初始化为适当的值
            List<Point> expected = new List<Point>(); // TODO: 初始化为适当的值
            expected.Add(new Point(-2, 0, 0));
            List<Point> actual;
            actual = target.GetPointsOfContainOneDiffractPath(indexOfEdgeOfThisPath);
            Assert.IsTrue(actual.Count == 0);
        }


        //在棱的上端点
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest1()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(1, 0, 0), new Point(-1, 0, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.Rx;
            node3.Position = new Point(-2, 0, -1);
            List<Node> nodes = new List<Node>() { node1, node2, node3 };
            Path_Accessor target = new Path_Accessor(nodes); // TODO: 初始化为适当的值
            int indexOfEdgeOfThisPath = 1; // TODO: 初始化为适当的值
            List<Point> expected = new List<Point>(); // TODO: 初始化为适当的值
            expected.Add(new Point(-1, 0, 0));
            List<Point> actual;
            actual = target.GetPointsOfContainOneDiffractPath(indexOfEdgeOfThisPath);
            Assert.IsTrue(expected[0].equal(actual[0]));
        }

        //在下端点
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest2()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(1, 0, 0), new Point(-1, 0, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.Rx;
            node3.Position = new Point(2, -1, 0);
            List<Node> nodes = new List<Node>() { node1, node2, node3 };
            Path_Accessor target = new Path_Accessor(nodes); // TODO: 初始化为适当的值
            int indexOfEdgeOfThisPath = 1; // TODO: 初始化为适当的值
            List<Point> expected = new List<Point>(); // TODO: 初始化为适当的值
            expected.Add(new Point(1, 0, 0));
            List<Point> actual;
            actual = target.GetPointsOfContainOneDiffractPath(indexOfEdgeOfThisPath);
            Assert.IsTrue(expected[0].equal(actual[0]));

        }

        //在棱上
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest3()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(1, 0, 0), new Point(-1, 0, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.Rx;
            node3.Position = new Point(0, 1, 3);
            List<Node> nodes = new List<Node>() { node1, node2, node3 };
            Path_Accessor target = new Path_Accessor(nodes); // TODO: 初始化为适当的值
            int indexOfEdgeOfThisPath = 1; // TODO: 初始化为适当的值
            List<Point> expected = new List<Point>(); // TODO: 初始化为适当的值
            expected.Add(new Point(0, 0, 0));
            List<Point> actual;
            actual = target.GetPointsOfContainOneDiffractPath(indexOfEdgeOfThisPath);
            Assert.IsTrue(expected[0].equal(actual[0]));
        }









        //Tx-R-D-Rx的绕射测试

        //三角形的内部棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest4()
        {

            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, 3, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(0, 0, 0));
            expected.Add(new Point(0, 1, 1));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            Assert.IsTrue(expected[0].equal(actual[0]) && (expected[1].equal(actual[1])));
        }

        //三角形的端点棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest5()
        {

            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(4, 3, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(1, 0, 0));
            expected.Add(new Point(2, 1, 1));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            Assert.IsTrue(expected[0].equal(actual[0]) && (expected[1].equal(actual[1])));
        }

        //三角形的内部棱的端点
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest6()
        {

            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(-2, 3, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-0.5, 0, 0));
            expected.Add(new Point(-1, 1, 1));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            Assert.IsTrue(expected[0].equal(actual[0]) && (expected[1].equal(actual[1])));
        }

        //三角形的内部棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest7()
        {

            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(-4, 3, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 0, 0));
            expected.Add(new Point(-2, 1, 1));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            Assert.IsTrue(actual.Count == 0);

        }

        //三角形的外部棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest8()
        {

            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(8, 3, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(2, 0, 0));
            expected.Add(new Point(4, 1, 1));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            Assert.IsTrue(actual.Count == 0);
        }

        //三角形的外部棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest9()
        {

            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, -1, 1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(12, 3, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(3, 0, 0));
            expected.Add(new Point(6, 1, 1));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            Assert.IsTrue(actual.Count == 0);
        }






        //Tx-D-R-Rx的反向绕射测试

        //三角形的内部棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest10()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 3, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-180, 0), new Point(0, 1, 0));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, -1, 1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(0, 1, 1));
            expected.Add(new Point(0, 0, 0));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            Assert.IsTrue(expected[0].equal(actual[0]) && (expected[1].equal(actual[1])));
        }

        //三角形端点棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest11()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(4, 3, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, -1, 1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(2, 1, 1));
            expected.Add(new Point(1, 0, 0));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            Assert.IsTrue(expected[0].equal(actual[0]) && (expected[1].equal(actual[1])));
        }

        //三角形的内部棱的端点
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest12()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-2, 3, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, -1, 1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 1));
            expected.Add(new Point(-0.5, 0, 0));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            Assert.IsTrue(expected[0].equal(actual[0]) && (expected[1].equal(actual[1])));
        }

        //三角形的内部棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest13()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-4, 3, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, -1, 1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-2, 1, 1));
            expected.Add(new Point(-1, 0, 0));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            Assert.IsTrue(actual.Count == 0);
        }


        //三角形的外部棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest14()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(8, 3, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, -1, 1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(4, 1, 1));
            expected.Add(new Point(2, 0, 0));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            Assert.IsTrue(actual.Count == 0);
        }


        //三角形的外部棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest15()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(12, 3, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(5, 1, 1), new Point(-1, 1, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 0, 0), new Point(-1, 0, 0), new Point(0, 1, 0));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.Rx;
            node4.Position = new Point(0, -1, 1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(6, 1, 1));
            expected.Add(new Point(3, 0, 0));
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            Assert.IsTrue(actual.Count == 0);
        }






        //Tx-R-R-D-Rx
        //面1端点面2端点棱内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest16()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(0.5, -0.5, 1.5), new Point(1.5, -1.5, 2.5));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(3, -1, 3);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 0));
            expected.Add(new Point(1, 1, 1));
            expected.Add(new Point(1, -1, 2));
            Assert.IsTrue((expected[0].equal(actual[0])) && (expected[1].equal(actual[1])) && (expected[2].equal(actual[2])));
        }


        //面1内部面2内部棱的端点
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest17()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(0.5, -0.5, 1.5), new Point(1, -1, 2));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(3, -1, 3);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 0));
            expected.Add(new Point(1, 1, 1));
            expected.Add(new Point(1, -1, 2));
            Assert.IsTrue((expected[0].equal(actual[0])) && (expected[1].equal(actual[1])) && (expected[2].equal(actual[2])));
        }


        //
        //面1外部面2内部棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest18()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(0.5, -0.5, 6), new Point(1.5, -1.5, 7));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(3, -1, 9);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            Assert.IsTrue(actual.Count == 0);
        }

        //面1内部面2内部棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest19()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(0, 0, 0), new Point(0.5, -0.5, 1.5));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(3, -1, 9);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            Assert.IsTrue(actual.Count == 0);
        }








        //Tx-D-R-R-Rx

        //面1端点面2端点棱内部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest20()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(3, -1, 3);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(0.5, -0.5, 1.5), new Point(1.5, -1.5, 2.5));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(-1, -1, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(1, -1, 2));
            expected.Add(new Point(1, 1, 1));
            expected.Add(new Point(-1, 1, 0));
            Assert.IsTrue((expected[0].equal(actual[0])) && (expected[1].equal(actual[1])) && (expected[2].equal(actual[2])));
        }

        //
        //面1内部面2内部棱的端点

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest21()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(3, -1, 3);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(0.5, -0.5, 1.5), new Point(1, -1, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(-1, -1, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(1, -1, 2));
            expected.Add(new Point(1, 1, 1));
            expected.Add(new Point(-1, 1, 0));
            Assert.IsTrue((expected[0].equal(actual[0])) && (expected[1].equal(actual[1])) && (expected[2].equal(actual[2])));
        }

        //面1外部面2内部棱的内部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest22()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(3, -1, 9);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(0.5, -0.5, 6), new Point(1.5, -1.5, 7));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(-1, -1, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(1, -1, 6.5));
            expected.Add(new Point(1, 1, 4));
            expected.Add(new Point(-1, 1, 1.5));
            Assert.IsTrue(actual.Count == 0);
        }

        //面1内部面2内部棱的外部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest23()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(3, -1, 3);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(0, 0, 0), new Point(0.5, -0.5, 1.5));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 1, 1), new Point(1.5, 0.5, 4), new Point(0.5, 1.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1), new Point(-1, 1, 0));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(-1, -1, -1);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(1, -1, 2));
            expected.Add(new Point(1, 1, 1));
            expected.Add(new Point(-1, 1, 0));
            Assert.IsTrue(actual.Count == 0);
        }









        //Tx-R-D-R-Rx
        //面1端点棱内部面2端点

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest24()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 0), new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(1.5, 0.5, 1), new Point(0.5, 1.5, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, -1, 0), new Point(1.5, -0.5, -0.5), new Point(0.5, -1.5, -0.5));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(0, -1, -0.5);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 0));
            expected.Add(new Point(1, 1, 1));
            expected.Add(new Point(1, -1, 0));
            Assert.IsTrue((expected[0].equal(actual[0])) && (expected[1].equal(actual[1])) && (expected[2].equal(actual[2])));
        }

        //面1边缘棱内部面2外部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest25()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 0), new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(1.5, 0.5, 3), new Point(0.5, 1.5, 3));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, -1, 0), new Point(1.5, -0.5, -0.5), new Point(0.5, -1.5, -0.5));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(0, -1, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            List<Point> expected = new List<Point>();
            Assert.IsTrue(actual.Count == 0);
        }


        //面1端点棱外部面2端点

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest26()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(-1, -1, -1);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 0), new Point(-1.5, 0.5, 1), new Point(-0.5, 1.5, 1));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(0, 0, 0), new Point(0.5, 0.5, 1));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, -1, 0), new Point(1.5, -0.5, -0.5), new Point(0.5, -1.5, -0.5));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.Rx;
            node5.Position = new Point(0, -1, 0.5);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            List<Point> expected = new List<Point>();
            Assert.IsTrue(actual.Count == 0);
        }








        // Tx-R-R-R-D-Rx
        //面1端点面2端点面3端点棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest27()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 0, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1, 5, 5), new Point(-1, 4.5, 6), new Point(-1, 5.5, 6));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.DiffractionNode;
            node5.DiffractionEdge = new AdjacentEdge(new Point(1, 6, 6), new Point(-1, 6, 6));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(1, 5, 5);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(4);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 1));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 5, 5));
            expected.Add(new Point(0, 6, 6));
            Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) && expected[2].equal(actual[2]) && expected[3].equal(actual[3]));
        }

        //三个面的端点棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest28()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 0, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1, 5, 5), new Point(-1, 4.5, 6), new Point(-1, 5.5, 6));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.DiffractionNode;
            node5.DiffractionEdge = new AdjacentEdge(new Point(2, 6, 6), new Point(1, 6, 6));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(1, 5, 5);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(4);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 1));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 5, 5));
            expected.Add(new Point(0, 6, 6));
            Assert.IsTrue(actual.Count == 0);
        }

        //面3的外部棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest29()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 0, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(-1, 5, 5.5), new Point(-1, 4.5, 6), new Point(-1, 5.5, 6));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.DiffractionNode;
            node5.DiffractionEdge = new AdjacentEdge(new Point(1, 6, 6), new Point(-1, 6, 6));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(1, 5, 5);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(4);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 1));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 5, 5));
            expected.Add(new Point(0, 6, 6));
            Assert.IsTrue(actual.Count == 0);
        }









        //Tx-D-R-R-R-Rx
        //三个面的端点棱的内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest30()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(1, 5, 5)
;
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(1, 6, 6), new Point(-1, 6, 6));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(-1, 5, 5), new Point(-1, 4.5, 6), new Point(-1, 5.5, 6));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 0, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(0, 6, 6));
            expected.Add(new Point(-1, 5, 5));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 1, 1));
            Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) && expected[2].equal(actual[2]) && expected[3].equal(actual[3]));
        }

        //三个面都是端点棱的外部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest31()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(1, 5, 5)
;
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(2, 6, 6), new Point(1, 6, 6));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(-1, 5, 5), new Point(-1, 4.5, 6), new Point(-1, 5.5, 6));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 0, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(0, 6, 6));
            expected.Add(new Point(-1, 5, 5));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 1, 1));
            Assert.IsTrue(actual.Count == 0);
        }

        //面三在外部，其余在端点或内部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest32()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(1, 5, 5)
;
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.DiffractionNode;
            node2.DiffractionEdge = new AdjacentEdge(new Point(1, 6, 6), new Point(-1, 6, 6));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(-1, 5, 5.5), new Point(-1, 4.5, 6), new Point(-1, 5.5, 6));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 0, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(1);
            List<Point> expected = new List<Point>();
            Assert.IsTrue(actual.Count == 0);
        }






        //Tx-R-R-D-R-Rx
        //三个端点和棱内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest33()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 0, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(1, 4, 4), new Point(-1, 4, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 5, 3), new Point(-1, 4.5, 4), new Point(-1, 5.5, 4));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 6, 2);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 1));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(0, 4, 4));
            expected.Add(new Point(-1, 5, 3));
            Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) && expected[2].equal(actual[2]) && expected[3].equal(actual[3]));
        }

        //三个端点和棱外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest34()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 0, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(1, 4, 4), new Point(2, 4, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 5, 3), new Point(-1, 4.5, 4), new Point(-1, 5.5, 4));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 6, 2);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 1, 1));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(0, 4, 4));
            expected.Add(new Point(-1, 5, 3));
            Assert.IsTrue(actual.Count == 0);
        }


        //面三外部和棱内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest35()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 0, 0);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.ReflectionNode;
            node3.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.DiffractionNode;
            node4.DiffractionEdge = new AdjacentEdge(new Point(1, 4, 4), new Point(2, 4, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 5, 3.5), new Point(-1, 4.5, 4), new Point(-1, 5.5, 4));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 6, 2);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(3);
            List<Point> expected = new List<Point>();
            Assert.IsTrue(actual.Count == 0);
        }






        //Tx-R-D-R-R-Rx
        //三个端点和棱内部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest36()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 6, 2);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 5, 3), new Point(-1, 4.5, 4), new Point(-1, 5.5, 4));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(1, 4, 4), new Point(-1, 4, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 0, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 5, 3));
            expected.Add(new Point(0, 4, 4));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 1, 1));

            Assert.IsTrue(expected[0].equal(actual[0]) && expected[1].equal(actual[1]) && expected[2].equal(actual[2]) && expected[3].equal(actual[3]));
        }

        //三个端点棱的外部
        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest37()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 6, 2);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 5, 3), new Point(-1, 4.5, 4), new Point(-1, 5.5, 4));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(1, 4, 4), new Point(2, 4, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 0, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            List<Point> expected = new List<Point>();
            Assert.IsTrue(actual.Count == 0);
        }


        // 面三的外部棱的内部

        /// <summary>
        ///GetPointsOfContainOneDiffractPath 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("CalculateModelClasses.dll")]
        public void GetPointsOfContainOneDiffractPathTest38()
        {
            Node node1 = new Node();
            node1.NodeStyle = NodeStyle.Tx;
            node1.Position = new Point(0, 6, 2);
            Node node2 = new Node();
            node2.NodeStyle = NodeStyle.ReflectionNode;
            node2.ReflectionFace = new Triangle(new Point(-1, 5, 3.5), new Point(-1, 4.5, 4), new Point(-1, 5.5, 4));
            Node node3 = new Node();
            node3.NodeStyle = NodeStyle.DiffractionNode;
            node3.DiffractionEdge = new AdjacentEdge(new Point(1, 4, 4), new Point(-1, 4, 4));
            Node node4 = new Node();
            node4.NodeStyle = NodeStyle.ReflectionNode;
            node4.ReflectionFace = new Triangle(new Point(1, 3, 3), new Point(1, 2.5, 4), new Point(1, 3.5, 4));
            Node node5 = new Node();
            node5.NodeStyle = NodeStyle.ReflectionNode;
            node5.ReflectionFace = new Triangle(new Point(-1, 1, 1), new Point(-1, 0.5, 2), new Point(-1, 1.5, 2));
            Node node6 = new Node();
            node6.NodeStyle = NodeStyle.Rx;
            node6.Position = new Point(0, 0, 0);
            List<Node> nodes = new List<Node> { node1, node2, node3, node4, node5, node6 };
            Path_Accessor target = new Path_Accessor(nodes);
            List<Point> actual = new List<Point>();
            actual = target.GetPointsOfContainOneDiffractPath(2);
            List<Point> expected = new List<Point>();
            expected.Add(new Point(-1, 5, 3));
            expected.Add(new Point(0, 4, 4));
            expected.Add(new Point(1, 3, 3));
            expected.Add(new Point(-1, 1, 1));
            Assert.IsTrue(actual.Count == 0);
        }
    }
}
