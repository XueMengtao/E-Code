using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.IO;

namespace TranmitterLib
{
    [Serializable]  //测试能否序列化
    public class TerInfo
    {
        public string name;
        public double originX;
        public double originY;
        public string path;
        public double Zmin;
        public double Zmax;
        public double Vertex1X;
        public double Vertex1Y;
        public double Vertex1Z;
        public double Vertex2X;
        public double Vertex2Y;
        public double Vertex2Z;
        public double Vertex3X;
        public double Vertex3Y;
        public double Vertex3Z;
        public double Vertex4X;
        public double Vertex4Y;
        public double Vertex4Z;
        public TerInfo(string name, double originX, double originY, string path,double Zmin,double Zmax,double Vertex1X,double Vertex1Y,double Vertex1Z,
                        double Vertex2X,double Vertex2Y,double Vertex2Z,double Vertex3X,double Vertex3Y,double Vertex3Z,double Vertex4X,double Vertex4Y,double Vertex4Z)   //经测试，方法不能传递给客户端
        {
            this.name = name;
            this.originX = originX;
            this.originY = originY;
            this.path = path;
            this.Zmin = Zmin;
            this.Zmax = Zmax;
            this.Vertex1X = Vertex1X;
            this.Vertex1Y = Vertex1Y;
            this.Vertex1Z = Vertex1Z;
            this.Vertex2X = Vertex2X;
            this.Vertex2Y = Vertex2Y;
            this.Vertex2Z = Vertex2Z;
            this.Vertex3X = Vertex3X;
            this.Vertex3Y = Vertex3Y;
            this.Vertex3Z = Vertex3Z;
            this.Vertex4X = Vertex4X;
            this.Vertex4Y = Vertex4Y;
            this.Vertex4Z = Vertex4Z;
        }
    }
    public class TerLib
    {
        private string cnStr;
        public TerLib(string cnStr)
        {
            this.cnStr = cnStr;
        }
        public string[] GetTerNames()
        {
            


            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList terNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAllTerNames", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    terNames.Add(myDataReader["Name"].ToString().Trim());
                }

                return (string[])terNames.ToArray(typeof(string));


            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                return null;
                throw ex;
            }
            finally
            {
                cn.Close();

            }
        }
      
        public TerInfo GetTerInfo(string terName)
        {
            TerInfo terInfo = null;
            

            SqlConnection cn = new SqlConnection(cnStr);
            

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getTerInfo", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter("@Name", terName);
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    terInfo = new TerInfo(myDataReader.GetString(0), myDataReader.GetDouble(1), myDataReader.GetDouble(2), myDataReader.GetString(3),myDataReader.GetDouble(4),myDataReader.GetDouble(5),
                                          myDataReader.GetDouble(6),myDataReader.GetDouble(7),myDataReader.GetDouble(8),myDataReader.GetDouble(9),myDataReader.GetDouble(10),myDataReader.GetDouble(11),
                                          myDataReader.GetDouble(12),myDataReader.GetDouble(13),myDataReader.GetDouble(14),myDataReader.GetDouble(15),myDataReader.GetDouble(16),myDataReader.GetDouble(17));
                }

                return terInfo;


            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                return null;
                throw ex;
            }
            finally
            {
                cn.Close();

            }

        }
        public void AddTerInfo(TerInfo terInfo)
        {
            //string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;
           
            SqlConnection cn = new SqlConnection(cnStr);


            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("addTerInfo", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param1 = new SqlParameter("@Name", terInfo.name);
                sqlCommand.Parameters.Add(param1);

                SqlParameter param2 = new SqlParameter("@OriginX", terInfo.originX);
                sqlCommand.Parameters.Add(param2);

                SqlParameter param3 = new SqlParameter("@OriginY", terInfo.originY);
                sqlCommand.Parameters.Add(param3);

                SqlParameter param4 = new SqlParameter("@Path", terInfo.path);
                sqlCommand.Parameters.Add(param4);

                SqlParameter param5 = new SqlParameter("@Zmin", terInfo.Zmin);
                sqlCommand.Parameters.Add(param5);

                SqlParameter param6 = new SqlParameter("@Zmax", terInfo.Zmax);
                sqlCommand.Parameters.Add(param6);

                SqlParameter param7 = new SqlParameter("@Vertex1X", terInfo.Vertex1X);
                sqlCommand.Parameters.Add(param7);

                SqlParameter param8 = new SqlParameter("@Vertex1Y", terInfo.Vertex1Y);
                sqlCommand.Parameters.Add(param8);

                SqlParameter param9 = new SqlParameter("@Vertex1Z", terInfo.Vertex1Z);
                sqlCommand.Parameters.Add(param9);

                SqlParameter param10 = new SqlParameter("@Vertex2X", terInfo.Vertex2X);
                sqlCommand.Parameters.Add(param10);

                SqlParameter param11 = new SqlParameter("@Vertex2Y", terInfo.Vertex2Y);
                sqlCommand.Parameters.Add(param11);

                SqlParameter param12 = new SqlParameter("@Vertex2Z", terInfo.Vertex2Z);
                sqlCommand.Parameters.Add(param12);

                SqlParameter param13 = new SqlParameter("@Vertex3X", terInfo.Vertex3X);
                sqlCommand.Parameters.Add(param13);

                SqlParameter param14 = new SqlParameter("@Vertex3Y", terInfo.Vertex3Y);
                sqlCommand.Parameters.Add(param14);

                SqlParameter param15 = new SqlParameter("@Vertex3Z", terInfo.Vertex3Z);
                sqlCommand.Parameters.Add(param15);

                SqlParameter param16 = new SqlParameter("@Vertex4X", terInfo.Vertex4X);
                sqlCommand.Parameters.Add(param16);

                SqlParameter param17 = new SqlParameter("@Vertex4Y", terInfo.Vertex4Y);
                sqlCommand.Parameters.Add(param17);

                SqlParameter param18 = new SqlParameter("@Vertex4Z", terInfo.Vertex4Z);
                sqlCommand.Parameters.Add(param18);

                

                sqlCommand.ExecuteNonQuery();
            }

            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;

            }
            finally
            {
                cn.Close();
            }
               
        }
        public void DeleteTerInfo(string terName)
        {
            

            SqlConnection cn = new SqlConnection(cnStr);


            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("deleteTerInfo", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter("@Name", terName);
                sqlCommand.Parameters.Add(param);

                sqlCommand.ExecuteNonQuery();
                string terPathName="d:\\projects\\ter\\"+terName;
                if (File.Exists(terPathName))
                {
                    File.Delete(terPathName);
                }

            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
               
            }
            finally
            {
                cn.Close();

            }
        }

    }
}
