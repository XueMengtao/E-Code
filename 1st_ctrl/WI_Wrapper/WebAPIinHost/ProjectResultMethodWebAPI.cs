using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace WebAPIinHost
{
    //使用了Newtonsoft.Json开源类库.Net4.0版本，下载地址"http://json.codeplex.com/"
    public class ProjectResultMethodWebAPI
    {
        //实际使用时ip地址改为localhost，必须加"http://"，否则报错
        const string uriString = "http://localhost:3260/api/";
        /// <summary>
        /// 获取数据库任务表中所有的信息
        /// </summary>
        /// <returns></returns>
        private List<TaskInfo> GetAllTaskInfoes()
        {
            Uri uriAddress = new Uri(uriString + "/TaskInfoes");
            //不要使用HttpWebRequest构造函数。 使用WebRequest.Create方法初始化新HttpWebRequest对象。 如果方案的统一资源标识符 (URI) 是http://或https://，Create返回HttpWebRequest对象。
            //参考"https://msdn.microsoft.com/zh-cn/library/system.net.httpwebrequest(v=vs.110).aspx"
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);
            request.Method = "GET";
            request.ContentType = "application/json";
            //同样不要创建HttpWebResponse的实例
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet)))
                    {
                        string msg = sr.ReadToEnd();
                        List<TaskInfo> tis = JsonConvert.DeserializeObject<List<TaskInfo>>(msg);
                        return tis;
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据库工程表中所有的信息
        /// </summary>
        /// <returns></returns>
        private List<ProjectInfo> GetAllProjectInfoes()
        {
            Uri uriAddress = new Uri(uriString + "/Projects");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);
            request.Method = "GET";
            request.ContentType = "application/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet)))
                    {
                        string msg = sr.ReadToEnd();
                        List<ProjectInfo> pis = JsonConvert.DeserializeObject<List<ProjectInfo>>(msg);
                        return pis;
                    }
                }
            }
        }

        /// <summary>
        /// 根据工程ID得到特定工程对象
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ProjectInfo GetSpecificProjectInfo(int projectID)
        {
            Uri uriAddress = new Uri(uriString + "/Projects/" + projectID.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);
            request.Method = "GET";
            request.ContentType = "application/json";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader sr = new StreamReader(responseStream, Encoding.GetEncoding(response.CharacterSet)))
                    {
                        string msg = sr.ReadToEnd();
                        ProjectInfo pi = (ProjectInfo)JsonConvert.DeserializeObject(msg, typeof(ProjectInfo));
                        return pi;
                    }
                }

            }
        }

        /// <summary>
        /// 筛选出工程状态为1的所有工程ID和其对应路径
        /// </summary>
        /// <param name="projectState"></param>
        /// <returns></returns>
        public List<ProjectResult> GetProjectIdsByProjectState(int projectState)
        {
            List<ProjectResult> prs = new List<ProjectResult>();
            List<ProjectInfo> pis = GetAllProjectInfoes();
            foreach (var pi in pis)
            {
                if (pi.ProjectState == 1)
                {
                    prs.Add(new ProjectResult(pi.ProjectID, pi.Name, pi.ResultDirectory));
                }
            }
            return prs;
        }

        /// <summary>
        /// 设置工程的状态
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="projectState"></param>
        public void SetProjectState(int projectID, int projectState)
        {
            //参考"http://www.cnblogs.com/tangge/p/4998007.html"和"http://stackoverflow.com/questions/5140674/how-to-make-a-http-put-request"
            //修改对象的工程状态
            ProjectInfo pi = GetSpecificProjectInfo(projectID);
            pi.ProjectState = projectState;
            //序列化成json格式
            string jsonProjectInfo = JsonConvert.SerializeObject(pi);
            byte[] byteArray = Encoding.UTF8.GetBytes(jsonProjectInfo);
            //创建HttpWebRequest对象
            Uri uriAddress = new Uri(uriString + "/Projects/" + projectID.ToString());
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriAddress);
            //初始化HttpWebRequest对象
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            //附加到要PUT给服务器的数据到HttpWebRequest对象
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);             
            }
            //读取服务器返回信息，用于调试
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {               
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string msg = sr.ReadToEnd();
                    Console.WriteLine(msg);
                    Console.ReadKey();
                }
            }
        }



    }
}
