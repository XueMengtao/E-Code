using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WF1
{
    class TerrainOperation
    {
        public static bool DeleteTerrain(string nodeNamesStr)
        {
            bool b = false;
            try
            {
                //从setup文件中删除有关地形的相应的段
                MainWindow.setupStr = MainWindow.setupStr.Remove(MainWindow.setupStr.IndexOf("begin_<globals>"), MainWindow.setupStr.IndexOf("end_<globals>") - MainWindow.setupStr.IndexOf("begin_<globals>") + 15);
                MainWindow.setupStr = MainWindow.setupStr.Remove(MainWindow.setupStr.IndexOf("begin_<studyarea>"), MainWindow.setupStr.IndexOf("end_<studyarea>") - MainWindow.setupStr.IndexOf("begin_<studyarea>") + "end_<studyarea>".Length + 2);
                MainWindow.setupStr = MainWindow.setupStr.Remove(MainWindow.setupStr.IndexOf("begin_<feature>"), MainWindow.setupStr.IndexOf("end_<feature>") - MainWindow.setupStr.IndexOf("begin_<feature>") + "end_<feature>".Length + 2);
                //从工程树中删除原来的地形结点
                MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[0].Nodes.Remove(MainWindow.staticTreeView.Nodes[0].Nodes[0].Nodes[0].Nodes[0]);
                //在.info文件中找出地形的名字，经纬度的大小字符串
                string iniTerrain = StringFound.FoundBackStr("<terrain>", nodeNamesStr, false);
                string iniLongitude = StringFound.FoundBackStr("<longitude>", nodeNamesStr, false);
                string iniLatitude = StringFound.FoundBackStr("<latitude>", nodeNamesStr, false);
                //从info文件中删除原来添加的地形的相关信息，然后将删除后的字符串写到info文件中
                nodeNamesStr = nodeNamesStr.Remove(nodeNamesStr.IndexOf("<terrain>"), 12 + iniTerrain.Length);
                nodeNamesStr = nodeNamesStr.Remove(nodeNamesStr.IndexOf("<longitude>"), 14 + iniLongitude.Length);
                nodeNamesStr = nodeNamesStr.Remove(nodeNamesStr.IndexOf("<latitude>"), 13 + iniLatitude.Length);
                FileOperation.WriteFile(nodeNamesStr, MainWindow.nodeInfoFullPath, false);
                //将原来添加的地形从工程文件中删除
                File.Delete(MainWindow.projectRealPath + "\\" + iniTerrain);
                File.Delete(MainWindow.projectRealPath + "\\" + iniTerrain + ".terinfo");
                b = true;
            }
            catch (Exception e)
            {
                //如果捕获到异常，则表示删除失败。
                LogFileManager.ObjLog.fatal(e.Message, e);
                b = false;
            }
            return b;
        }
    }
}
