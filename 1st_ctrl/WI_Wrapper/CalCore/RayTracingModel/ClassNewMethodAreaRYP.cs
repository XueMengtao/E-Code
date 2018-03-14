using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using CalculateModelClasses;
using TxRxFileProceed;

namespace RayCalInfo
{
    public class ClassNewMethodAreaRYP
    {
        string TerPath = ConfigurationManager.AppSettings["terpath"];
        string SetupPath = ConfigurationManager.AppSettings["setupath"];
        string TxPath = ConfigurationManager.AppSettings["txpath"];
        string RxPath = ConfigurationManager.AppSettings["rxpath"];
        public List<Path> MultiplyMethod(Node tx,Terrain ter, ReceiveArea reArea,List<FrequencyBand> TxFrequencyBand)
        {
            List<Path> pathInfo = new List<Path>();
            int cellAngle = 1;
            double divideAngle = 0.5;
            //       if (tx.Position.x > reArea.OriginPoint.x && tx.Position.x < reArea.OriginPoint.x + reArea.rxLength && tx.Position.y > reArea.OriginPoint.y && tx.Position.y < reArea.OriginPoint.y + reArea.rxwidth)//若发射机在接收区域内或者正上方
            if (RayPath.IsInTheArea(tx.Position, reArea))//若发射机在接收区域内
            {
                divideAngle = 1;
            }
    //        RayInfo.divideAngle = divideAngle;
            ClassNewRay[,] initArray = new ClassNewRay[360 / cellAngle + 1, 180 / cellAngle + 1];
            ClassNewRay[,] temp = NewInit(tx, cellAngle, ter, reArea,TxFrequencyBand );
            for (int i = 0; i < 360 / cellAngle; i++)
            {
                for (int j = 0; j < 180 / cellAngle; j++)
                {
                    initArray[i, j] = temp[i, j];
                }
            }
            for (int i = 0; i < 360 / cellAngle; i++)
            {
                int temp1 = 180 / cellAngle;
                initArray[i, temp1] = initArray[i, 0];
            }
            for (int i = 0; i < 180 / cellAngle; i++)
            {
                int temp1 = 360 / cellAngle;
                initArray[temp1, i] = initArray[0, i];
            }
            initArray[360 / cellAngle, 180 / cellAngle] = initArray[0, 0];
            for (int i = 0; i < 360 / cellAngle; i++)
            {
                for (int j = 0; j < 180 / cellAngle; j++)
                {
                    if (initArray[i, j].Flag == true || initArray[i + 1, j].Flag == true || initArray[i, j + 1].Flag == true || initArray[i + 1, j + 1].Flag == true)
                    {
                        for (double m = i * cellAngle; m < (i + 1) * cellAngle; m += divideAngle)
                        {
                            for (double n = j * cellAngle; n < (j + 1) * cellAngle; n += divideAngle)
                            {
                                if (m != i * cellAngle || n != j * cellAngle)
                                {
                                    SpectVector vector = new SpectVector(Math.Sin(Math.PI / 180 * n) * Math.Cos(Math.PI / 180 * m), Math.Sin(Math.Sin(Math.PI / 180 * n)) * Math.Sin(Math.PI / 180 * m), Math.Cos(Math.PI / 180 * n));
                                    List<RayInfo> list = new List<RayInfo>();
                                    list.Add(new RayInfo(tx.Position, vector));
                                    List<Path> pathtemp = GetFanalPath(tx, ter, reArea, TxFrequencyBand,list);
                                    for (int a = 0; a < pathtemp.Count; a++)
                                    {
                                        pathInfo.Add(pathtemp[a]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < 360 / cellAngle; i++)
            {
                for (int j = 0; j < 180 / cellAngle; j++)
                {
                    if (initArray[i, j].Flag == true)
                        pathInfo.AddRange(initArray[i, j].Path);
                }
            }
         Console.WriteLine("算法完成：" + DateTime.Now);
            return pathInfo;
        }
        private ClassNewRay[,] NewInit(Node tx, int cellAngle,Terrain ter, ReceiveArea reArea, List<FrequencyBand> TxFrequencyBand)
        {
            double radius = 1;
            Point target;
            SpectVector vector;
            int cellTheta = 180 / cellAngle;
            int cellPhi = 360 / cellAngle;
            ClassNewRay[,] initNewRay = new ClassNewRay[cellPhi, cellTheta];
            for (int i = 0; i < cellPhi; i++)
            {
                for (int j = 0; j < cellTheta; j++)
                {
                    target = new Point(radius * Math.Sin(Math.PI / 180 * j * cellAngle) * Math.Cos(Math.PI / 180 * i * cellAngle), radius * Math.Sin(Math.PI / 180 * j * cellAngle) * Math.Sin(Math.PI / 180 * i * cellAngle), radius * Math.Cos(Math.PI / 180 * j * cellAngle));
                    vector = new SpectVector(target.X, target.Y, target.Z);
                    List<RayInfo> list1 = new List<RayInfo>();
                    list1.Add(new RayInfo(tx.Position, vector));
                    List<Path> pathtemp = GetFanalPath(tx, ter, reArea, TxFrequencyBand,list1);
                    initNewRay[i, j] = new ClassNewRay(tx.Position, vector, Ismeat(pathtemp), pathtemp);
                }
            }
            return initNewRay;

        }
        private List<CalculateModelClasses.Path> GetFanalPath(Node tx, Terrain ter, ReceiveArea reArea, List<FrequencyBand> TxFrequencyBand, List<RayInfo> list = null)
        {
            //Stopwatch watch = new Stopwatch();

            return AreaMethod(tx, ter, reArea,TxFrequencyBand, list);

        }
        public List<Path> AreaMethod(Node tx,Terrain ter, ReceiveArea reArea, List<FrequencyBand> txFrequencyBand, List<RayInfo> list = null)
        {

         //   RayPath.GetTransPathArea(ter, tx, reArea,txFrequencyBand, list);
            List<CalculateModelClasses.Path> temp = new List<CalculateModelClasses.Path>();
        //    if (tx.ChildNodes.Count != 0)
        //    {
        //        temp = RayPath.GetLegalPathsArea(tx);
         //   }
            tx.ChildNodes.Clear();
            return temp;
        }

        private bool Ismeat(List<CalculateModelClasses.Path> temp)
        {
            if (temp.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
