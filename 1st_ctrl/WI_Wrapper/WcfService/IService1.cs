using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using TranmitterLib;
using System.Data.SqlClient;
namespace WcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string[] iGetAllWaveFormNames();//返回所有的波形名称，在天线或者辐射源的按钮中使用
        [OperationContract]
        [FaultContract(typeof(WcfException))]
        string[] iGetAllWaveForm(string type); //返回选中的波形类型所有的波形名称，在波形窗口中使用
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         WaveForm iGetWaveForm(string name);//返回选中名称的波形的所有信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iAddWaveForm(WaveForm waveform); //向数据库中增加波形信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iDelWaveForm(string name); //删除数据库中的波形
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iUpdateWaveForm(WaveForm waveform); //更新波形信息
        
        //天线操作
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string[] iGetAllAntennaNames(); //返回所有的天线名称，在辐射源的按钮中使用
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string[] iGetAllAntenna(string type); //返回选中的天线类型所有的波天线名称，在天线窗口中使用
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         Antenna iGetAntenna(string name);//返回选中名称的天线的所有信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iAddAntenna(Antenna antenna) ;//向数据库中增加天线信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iDelAntenna(string name); //删除数据库中的天线
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iUpdateAntenna(Antenna antenna); //更新天线信息
        
        //辐射源操作
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string[] iGetAllTransmitter(); //返回所有的辐射源名称
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         Transmitter iGetTransmitter(string name);//返回选中名称的辐射源的所有信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iAddTransmitter(Transmitter transmitter); //向数据库中增加辐射源信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iDelTransmitter(string name); //删除数据库中的辐射源
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iUpdateTransmitter(Transmitter transmitter); //更新辐射源信息
        //辐射源操作
        [OperationContract]
        [FaultContract(typeof(WcfException))]
        string[] iGetAllReceiver(); //返回所有的辐射源名称
        [OperationContract]
        [FaultContract(typeof(WcfException))]
        Receiver iGetReceiver(string name);//返回选中名称的辐射源的所有信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void iAddReceiver(Receiver transmitter); //向数据库中增加辐射源信息
        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void iDelReceiver(string name); //删除数据库中的辐射源
        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void iUpdateReceiver(Receiver transmitter); //更新辐射源信息

        //地形操作
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string[] iGetTerNames();
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         TerInfo iGetTer(string name);
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iAddTerInfo(TerInfo terinfo);
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void iDelTerInfo(string name);


        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void CreatTables(string path);
        
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string GetData(string path);
        
        [OperationContract]
        [FaultContract(typeof(WcfException))]
         void PutData(string path, string name, string putStr);

        [OperationContract]
        [FaultContract(typeof(WcfException))]
         TaskInfo iGetMinTaskID();

        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string[] iGetFilePath(string[] filenames);

        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void iSetTaskState(int TaskID, short state);

        [OperationContract]
        [FaultContract(typeof(WcfException))]
        ProjectInfo[] iGetProjectInfo();

        [OperationContract]
        [FaultContract(typeof(WcfException))]
         string iGetTaskResultDir(int TaskID);

        [OperationContract]
        [FaultContract(typeof(WcfException))]
        string[] iGetResultFileNames(string path);

        [OperationContract]
        [FaultContract(typeof(WcfException))]
        string[] iGetResultDirNames(string path);

        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void iDelProject(string proname);  

        [OperationContract]
        [FaultContract(typeof(WcfException))]
        void HeartBeat(Guid guid,string ip);                //节点心跳信息

        
    }
}
