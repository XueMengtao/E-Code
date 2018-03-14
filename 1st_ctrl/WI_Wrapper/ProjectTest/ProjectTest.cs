using ProjectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace ProjectTest
{
    
    
    /// <summary>
    ///这是 ProjectTest 的测试类，旨在
    ///包含所有 ProjectTest 单元测试
    ///</summary>
    [TestClass()]
    public class ProjectTest
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
        ///Project 构造函数 的测试(指定的目录不存在)
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest()
        {
            try
            {
                string projectPath = @"..\..\..\ProjectTest\不存在的文件夹"; // TODO: 初始化为适当的值
                Project target = new Project(projectPath);
                //Assert.IsNull(target.ResultFiles);
                //Assert.AreEqual(target.ResultFiles.Count, 0);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }

        /// <summary>
        ///Project 构造函数 的测试(工程路径下没有任何文件)
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest1()
        {
            try
            {
                string projectPath = @"..\..\..\ProjectTest\测试项目\没有结果文件的工程"; // TODO: 初始化为适当的值
                Project target = new Project(projectPath);
                Assert.IsNull(target.ResultFiles);
                Assert.AreEqual(target.ResultFiles.Count, 0);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }


        /// <summary>
        ///Project 构造函数 的测试 : 文件内容出现bug的工程文件(exmag文件内容不全)
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest2()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\文件内容出现bug的工程文件（exmag文件内容不全）"; // TODO: 初始化为适当的值
            Project target = new Project(projectPath);
            Assert.IsNotNull(target.ResultFiles);
            Assert.AreEqual(target.ResultFiles.Count, 459);
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }




        /// <summary>
        ///Project 构造函数 的测试 : 存在一些文件名异常的文件
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest3()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\存在一些文件名异常的文件"; 
            Project target = new Project(projectPath);
            Assert.IsNotNull(target.ResultFiles);
            Assert.AreEqual(target.ResultFiles.Count, 299);
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }



        /// <summary>
        ///Project 构造函数 的测试 : 缺少某种文件的工程文件（没有exphs类型的文件）
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest4()
        {
            try
            {
                string projectPath = @"..\..\..\ProjectTest\测试项目\缺少某种文件的工程文件（没有exphs类型的文件）";
                Project target = new Project(projectPath);
                target.CombineResult();
            }
            catch (LackOfSomeTypeFileException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("合成过程结束");
            }
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }


        /// <summary>
        ///Project 构造函数 的测试 : 文件内容出现bug的工程文件（有些文件没有数据）
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest5()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\文件内容出现bug的工程文件（有些文件没有数据）";
            Project target = new Project(projectPath);
            Assert.IsNotNull(target.ResultFiles);
            Assert.AreEqual(target.ResultFiles.Count, 292);
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }


        /// <summary>
        ///Project 构造函数 的测试 : 多出某种类型的文件（ewmag）
        ///</summary>
        [TestMethod()]
        public void ProjectConstructorTest6()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\多出某种类型的文件（ewmag）";
            Project target = new Project(projectPath);
            Assert.IsNotNull(target.ResultFiles);
            Assert.AreEqual(target.ResultFiles.Count, 296);
            //Assert.Inconclusive("TODO: 实现用来验证目标的代码");
        }



        /// <summary>
        ///GetRxType 的测试
        ///</summary>
        [TestMethod()]
        public void GetRxTypeTest()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\测试GetRxtype";
            Project target = new Project(projectPath); // TODO: 初始化为适当的值
            List<ResultFile> resultTypes = target.ResultFiles; // TODO: 初始化为适当的值
            List<string> expected = new List<string> { "r001", "r004", "r003" }; // TODO: 初始化为适当的值
            List<string> actual;
            actual = target.GetRxType(resultTypes);
            for (int i = 0; i < expected.Count; i++)
            { Assert.AreEqual(expected[i], actual[i]); }
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }



        /// <summary>
        ///SearchSameRxTypeFile 的测试
        ///</summary>
        [TestMethod()]
        public void SearchSameRxTypeFileTest()
        {
            Project target = new Project(@"..\..\..\ProjectTest\测试项目\测试SearchSameRxTypeFile"); 
            List<ResultFile> resultTypes = target.ResultFiles; 
            List<string> rxTypes = target.GetRxType(resultTypes); 
            List<List<ResultFile>> actual;
            actual = target.SearchSameRxTypeFile(resultTypes, rxTypes);
            Assert.AreEqual(3, actual.Count);
            Assert.AreEqual(142, actual[0].Count);
            Assert.AreEqual(71, actual[1].Count);
            Assert.AreEqual(71, actual[2].Count);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }


        /// <summary>
        ///GetMidPointFrequency 的测试
        ///</summary>
        [TestMethod()]
        public void GetMidPointFrequencyTest()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\测试GetMidPointFrequency";
            Project target = new Project(projectPath);
            List<ResultFile> resultTypes = target.ResultFiles;
            List<string> expected = new List<string> { "0990", "0993", "0996", "0999", "1002", "1005", "1008" };
            List<string> actual = target.GetMidPointFrequency(resultTypes);
            for (int i = 0; i < expected.Count; i++)
            { Assert.AreEqual(expected[i], actual[i]); }
        }


        /// <summary>
        ///GetWeight 的测试
        ///</summary>
        [TestMethod()]
        public void GetWeightTest()
        {
            Project target = new Project(@"..\..\..\ProjectTest\测试项目\测试GetWeight"); 
            List<ResultFile> singleRxTypeFile = target.ResultFiles; 
            List<string> midPointFrequencies = target.GetMidPointFrequency(singleRxTypeFile); 
            List<Dictionary<ResultFile, double>> actual;
            actual = target.GetWeight(singleRxTypeFile, midPointFrequencies);
            double[] expected = { 1, 0.85, 0.7, 0.55, 0.4, 0.25, 0.1 };
            List<double> temp = new List<double>(actual[0].Values);
            for (int i = 0; i < temp.Count; i++)
            { Assert.AreEqual(expected[i], temp[i]); }
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }




        /// <summary>
        ///SearchSameMidPointAndSameRxTypeFile 的测试
        ///</summary>
        [TestMethod()]
        public void SearchSameMidPointAndSameRxTypeFileTest()
        {
            Project target = new Project(@"..\..\..\ProjectTest\测试项目\测试SearchSameMidPointAndSameRxTypeFile");
            List<List<ResultFile>> sameRxTypeFiles = target.SearchSameRxTypeFile(target.ResultFiles, target.GetRxType(target.ResultFiles)); 
            foreach (List<ResultFile> singleRxTypeFile in sameRxTypeFiles)
            {
                List<string> midPointFrequencies = target.GetMidPointFrequency(singleRxTypeFile);
                List<List<ResultFile>> actual = target.SearchSameMidPointAndSameRxTypeFile(singleRxTypeFile, midPointFrequencies);
                for (int i = 0; i < midPointFrequencies.Count; i++)
                {
                    Assert.AreEqual(3, actual[i].Count);
                }
                Assert.AreEqual(7, actual.Count);
            }
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }


        /// <summary>
        ///OutPutResult 的测试
        ///</summary>
        [TestMethod()]
        public void OutPutResultTest()
        {
            Project target = new Project(); 
            Dictionary<string, string> totalResultAndFilePaths =
                new Dictionary<string, string> { { @"..\..\..\ProjectTest\测试项目\测试OutPutResult\test.txt", "HelloWord" } }; 
            target.OutPutResult(totalResultAndFilePaths);
            StreamReader sr = new StreamReader(@"..\..\..\ProjectTest\测试项目\测试OutPutResult\test.txt");
            string expected = "HelloWord";
            Assert.AreEqual(expected, sr.ReadToEnd());
            //Assert.Inconclusive("无法验证不返回值的方法。");
        }



        /// <summary>
        ///ProjectName 的测试
        ///</summary>
        [TestMethod()]
        public void ProjectNameTest()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\测试ProjectName\hwy317\studyarea"; 
            Project target = new Project(projectPath); 
            string actual;
            actual = target.ProjectName;
            Assert.AreEqual("hwy317", actual);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///ProjectPath 的测试
        ///</summary>
        [TestMethod()]
        public void ProjectPathTest()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\测试ProjectPath\hwy317\studyarea"; // TODO: 初始化为适当的值
            Project target = new Project(projectPath); // TODO: 初始化为适当的值
            string actual;
            actual = target.ProjectPath;
            Assert.AreEqual(@"..\..\..\ProjectTest\测试项目\测试ProjectPath\hwy317\studyarea", actual);
            // Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///GetResultFiles 的测试
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Project.dll")]
        public void GetResultFilesTest()
        {
            string projectPath = @"..\..\..\ProjectTest\测试项目\测试GetResultFiles\hwy317\studyarea";
            Project target = new Project(projectPath); 
            Assert.IsNotNull(target.ResultFiles);
            Assert.AreEqual(target.ResultFiles.Count,300);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }


        /// <summary>
        ///CombineResult 的测试
        ///</summary>
        [TestMethod()]
        public void CombineResultTest1()
        {
            try
            {
                Project target = new Project(@"..\..\..\ProjectTest\测试项目\总测试\hwy317\studyarea");
                target.CombineResult();
                Project actual = new Project(@"..\..\..\ProjectTest\测试项目\总测试\hwy317\studyarea");
                Assert.AreEqual(474, actual.ResultFiles.Count);
                //Assert.Inconclusive("无法验证不返回值的方法。");
            }
            catch (LackOfSomeTypeFileException ex)
            {
                Console.WriteLine(ex.Message);
            }               
        }

        /// <summary>
        ///CombineResult 的测试
        ///</summary>
        [TestMethod()]
        public void CombineResultTest2()
        {
            Project target = new Project(@"..\..\..\ProjectTest\测试项目\总测试2\hwy787\studyarea");
            target.CombineResult();
            Project actual = new Project(@"..\..\..\ProjectTest\测试项目\总测试2\hwy787\studyarea");
            Assert.AreEqual(295, actual.ResultFiles.Count);
            //Assert.Inconclusive("无法验证不返回值的方法。");
        }
    }
}
