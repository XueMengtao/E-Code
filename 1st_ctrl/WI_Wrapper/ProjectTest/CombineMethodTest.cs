using ProjectLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ProjectTest
{
    
    
    /// <summary>
    ///这是 CombineMethodTest 的测试类，旨在
    ///包含所有 CombineMethodTest 单元测试
    ///</summary>
    [TestClass()]
    public class CombineMethodTest
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
        ///CombineEtrms 的测试
        ///</summary>
        [TestMethod()]
        public void CombineEtrmsTest()
        {
            Project project = new Project(@"..\..\..\ProjectTest\测试项目\测试combineMethod.combineEtrms");
            List<List<ResultFile>> sameRxTypeFile = project.SearchSameRxTypeFile(project.ResultFiles, project.GetRxType(project.ResultFiles));
            List<ResultFile> singleRxTypeFile = sameRxTypeFile[0];
            List<string> midPointFrequencies = project.GetMidPointFrequency(singleRxTypeFile);
            List<List<ResultFile>> sameMidPointAndSameRxtypeFile = project.SearchSameMidPointAndSameRxTypeFile(singleRxTypeFile, midPointFrequencies);
            List<ResultFile> singleMidPointAndSameRxTypeFile = sameMidPointAndSameRxtypeFile[0];
            string actual = CombineMethod.CombineEtrms(singleMidPointAndSameRxTypeFile);
            string expected = "1.25195E-03";
            StringAssert.Contains(actual, expected);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///CombineMag 的测试
        ///</summary>
        [TestMethod()]
        public void CombineMagTest()
        {
            Project project = new Project(@"..\..\..\ProjectTest\测试项目\测试combineMethod.combineMag");
            List<string> midPointFrequencies = project.GetMidPointFrequency(project.ResultFiles); // TODO: 初始化为适当的值
            List<Dictionary<ResultFile, double>> resultFilesWithWeights = project.GetWeight(project.ResultFiles, midPointFrequencies);
            List<string> actual = new List<string>();
            foreach (Dictionary<ResultFile, double> singleMidpointResultFilesWithWeight in resultFilesWithWeights)
            {
                actual.Add(CombineMethod.CombineMag(singleMidpointResultFilesWithWeight));
            }
            string expected = "7.53574E-01  6.01320E-01  4.54190E-01"; // TODO: 初始化为适当的值
            StringAssert.Contains(actual[0], expected);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///CombinePhase 的测试
        ///</summary>
        [TestMethod()]
        public void CombinePhaseTest()
        {
            Project project = new Project(@"..\..\..\ProjectTest\测试项目\测试combineMethod.combinePhase");
            List<string> midPointFrequencies = project.GetMidPointFrequency(project.ResultFiles); // TODO: 初始化为适当的值
            List<Dictionary<ResultFile, double>> resultFilesWithWeights = project.GetWeight(project.ResultFiles, midPointFrequencies);
            List<string> actual = new List<string>();
            foreach (Dictionary<ResultFile, double> singleMidpointResultFilesWithWeight in resultFilesWithWeights)
            {
                actual.Add(CombineMethod.CombinePhase(singleMidpointResultFilesWithWeight));
            }
            string expected = "37.06   6.01320E-01   4.54190E-01"; // TODO: 初始化为适当的值
            StringAssert.Contains(actual[0], expected);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///CombinePower 的测试
        ///</summary>
        [TestMethod()]
        public void CombinePowerTest()
        {
            Project project = new Project(@"..\..\..\ProjectTest\测试项目\测试combineMethod.combinePower");
            List<List<ResultFile>> sameRxTypeFiles = project.SearchSameRxTypeFile(project.ResultFiles, project.GetRxType(project.ResultFiles));
            List<ResultFile> singleRxTypeFile = sameRxTypeFiles[0];
            string expected = "-1.41980E+02    7.16500E+01";
            string actual = CombineMethod.CombinePower(singleRxTypeFile);
            StringAssert.Contains(actual, expected);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }

        /// <summary>
        ///CombineSituationErms 的测试
        ///</summary>
        [TestMethod()]
        public void CombineSituationErmsTest()
        {
            Project project = new Project(@"..\..\..\ProjectTest\测试项目\测试combineMethod.CombineSituationErms");
            List<string> midPointFrequencies = project.GetMidPointFrequency(project.ResultFiles);
            List<Dictionary<ResultFile, double>> resultFilesWithWeights = project.GetWeight(project.ResultFiles, midPointFrequencies);
            Dictionary<ResultFile, double> singleMidpointResultFilesWithWeight = resultFilesWithWeights[0];
            string expected = "1.94932E-02"; // TODO: 初始化为适当的值
            string actual;
            actual = CombineMethod.CombineSituationErms(singleMidpointResultFilesWithWeight);
            StringAssert.Contains(actual, expected);
            //Assert.Inconclusive("验证此测试方法的正确性。");
        }


    }
}
