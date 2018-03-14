using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace TranmitterLib
{
    [Serializable] //经测试方法不能传递,而且不能使用消息协定
    public class Transmitter
    {
        public string Name;
        public double RotateX;
        public double RotateY;
        public double RotateZ;
        public double Power;
        public string AntennaName;
        

        public Transmitter(string Name, double RotateX, double RotateY, double RotateZ, double Power, string AntennaName)  //经测试，方法不能传递给客户端
        {
            this.Name = Name;
            this.RotateX = RotateX;
            this.RotateY = RotateY;
            this.RotateZ = RotateZ;
            this.Power = Power;
            this.AntennaName = AntennaName;
           

        }
    }
    [Serializable] //经测试方法不能传递,而且不能使用消息协定
    public class Receiver
    {
        public string Name;
        public double RotateX;
        public double RotateY;
        public double RotateZ;
        public string AntennaName;


        public Receiver(string Name, double RotateX, double RotateY, double RotateZ, string AntennaName)  //经测试，方法不能传递给客户端
        {
            this.Name = Name;
            this.RotateX = RotateX;
            this.RotateY = RotateY;
            this.RotateZ = RotateZ;
            this.AntennaName = AntennaName;
        }
    }
    [Serializable]
    public class Antenna
    {
        public string Name;
        public string Type;
        public double MaxGain;
        public double RecieveThrehold;
        public double TransmissionLoss;
        public double VSWR;
        public double Temperature;
        public string Polarization=null;
        public double? Radius=null;
        public double? BlockageRadius=null;
        public string ApertureDistribution=null;
        public double? EdgeTeper=null;
        public double? Length=null;
        public double? Pitch=null;
        public Antenna()
        {
        }

        public Antenna(string Name, string Type, double MaxGain,
                        double RecieveThrehold, double TransmissionLoss, double VSWR, double Temperature, string Polarization = null,
                        double? Radius = null, double? BlockageRadius = null, string ApertureDistribution = null, double? EdgeTeper = null, double? Length = null, double? Pitch = null
                        )
        {
            this.Name = Name;
            this.Type = Type;
            this.MaxGain = MaxGain;
            this.Polarization = Polarization;
            this.RecieveThrehold = RecieveThrehold;
            this.TransmissionLoss = TransmissionLoss;
            this.VSWR = VSWR;
            this.Temperature = Temperature;
            this.Radius = Radius;
            this.BlockageRadius = BlockageRadius;
            this.ApertureDistribution = ApertureDistribution;
            this.EdgeTeper = EdgeTeper;
            this.Length = Length;
            this.Pitch = Pitch;

        }
    }
    [Serializable]
    public class WaveForm
    {
        public string Name;
        public string Type;
        public double Frequency;
        public double BandWidth;
        public double? Phase=null;
        public double? StartFrequency=null;
        public double? EndFrequency=null;
        public double? RollOffFactor=null;
        public string FreChangeRate = null;
        public WaveForm()
        {
        }
        
        
        public WaveForm(string Name, string Type, double Frequency, double BandWidth, double? Phase = null,
                         double? StartFrequency = null, double? EndFrequency = null, double? RollOffFactor = null, string FreChangeRate = null
                        )
        {
            this.Name = Name;
            this.Type = Type;
            this.Frequency = Frequency;
            this.BandWidth = BandWidth;
            this.Phase = Phase;
            this.StartFrequency = StartFrequency;
            this.EndFrequency = EndFrequency;
            this.RollOffFactor = RollOffFactor;
            this.FreChangeRate = FreChangeRate;


        }
    }
   public class TransmitterLib
    {
      // private string cnStr = "Data Source=192.168.0.127;Initial Catalog=ParallelTask;User ID=sa;Password=sa";
       private string cnStr;
       public TransmitterLib(string cnStr)
       {
           this.cnStr = cnStr;
       }

        //get all the TransmitterName in Lib
        public string[] GetTransmitterNames()
        {
           

            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList transmitterNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAllTransmitters", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    transmitterNames.Add(myDataReader["Name"].ToString().Trim());
                }

                return (string[])transmitterNames.ToArray(typeof(string));


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
        //get all the TransmitterName in Lib
        public string[] GetReceiverNames()
        {
            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList receiverNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAllReceivers", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    receiverNames.Add(myDataReader["Name"].ToString().Trim());
                }

                return (string[])receiverNames.ToArray(typeof(string));


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


        //select the transmitter by name
        public Transmitter GetTransmitter(string name)
        {

            //string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;

            SqlConnection cn = new SqlConnection(cnStr);
            Transmitter transmitter = null;

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getTransmitter", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Name";
                param.Value = name;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (myDataReader.Read())
                {
                    double rotateX = myDataReader.GetDouble(1);
                    double rotateY = myDataReader.GetDouble(2);
                    double rotateZ = myDataReader.GetDouble(3);
                    double power = myDataReader.GetDouble(4);
                    string antennaName = myDataReader.GetString(5);
          

                    transmitter = new Transmitter(name, rotateX, rotateY, rotateZ, power, antennaName);
                }

                return transmitter;


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
        //select the receiver by name
        public Receiver GetReceiver(string name)
        {

            //string cnStr = ConfigurationManager.ConnectionStrings["sqlProviderParallelTask"].ConnectionString;

            SqlConnection cn = new SqlConnection(cnStr);
            Receiver receiver = null;

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getReceiver", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Name";
                param.Value = name;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (myDataReader.Read())
                {
                    double rotateX = myDataReader.GetDouble(1);
                    double rotateY = myDataReader.GetDouble(2);
                    double rotateZ = myDataReader.GetDouble(3);
                    string antennaName = myDataReader.GetString(4);


                    receiver = new Receiver(name, rotateX, rotateY, rotateZ, antennaName);
                }

                return receiver;


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


        // Add new transmitter
        public void AddTransmitter(Transmitter transmitter)
        {

            try
            {
                

                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("addTransmitter", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ArrayList param = new ArrayList();
                param.Add(new SqlParameter("@Name", transmitter.Name));
                param.Add(new SqlParameter("@RotateX", transmitter.RotateX));
                param.Add(new SqlParameter("@RotateY", transmitter.RotateY));
                param.Add(new SqlParameter("@RotateZ", transmitter.RotateZ));
                param.Add(new SqlParameter("@Power", transmitter.Power));
                param.Add(new SqlParameter("@AntennaName", transmitter.AntennaName));
               


                sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }

        }
        // Add new transmitter
        public void AddReceiver(Receiver receiver)
        {
            try
            {
                SqlConnection cn = new SqlConnection(cnStr);
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("addReceiver", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ArrayList param = new ArrayList();
                param.Add(new SqlParameter("@Name", receiver.Name));
                param.Add(new SqlParameter("@RotateX", receiver.RotateX));
                param.Add(new SqlParameter("@RotateY", receiver.RotateY));
                param.Add(new SqlParameter("@RotateZ", receiver.RotateZ));
                param.Add(new SqlParameter("@AntennaName", receiver.AntennaName));

                sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }

        }

        // delete the tansmitter by name
        public void DeleteTransmitter(string name)
        {
            try
            {
                

                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("deleteTransmitter", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;


                SqlParameter param = new SqlParameter("@Name", name);
                sqlCommand.Parameters.Add(param);
                sqlCommand.ExecuteNonQuery();



            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }
        public void DeleteReceiver(string name)
        {
            try
            {
                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("deleteReceiver", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@Name", name);
                sqlCommand.Parameters.Add(param);
                sqlCommand.ExecuteNonQuery();

            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }

        //  update the tansmitter
        public void UpdateTransmitter(Transmitter transmitter)
        {

            if (transmitter == null)
                return;
            else
            {
                try
                {
                   

                    SqlConnection cn = new SqlConnection(cnStr);

                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("updateTransmitter", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    ArrayList param = new ArrayList();
                    param.Add(new SqlParameter("@Name", transmitter.Name));
                    param.Add(new SqlParameter("@RotateX", transmitter.RotateX));
                    param.Add(new SqlParameter("@RotateY", transmitter.RotateY));
                    param.Add(new SqlParameter("@RotateZ", transmitter.RotateZ));
                    param.Add(new SqlParameter("@Power", transmitter.Power));
                    param.Add(new SqlParameter("@AntennaName", transmitter.AntennaName));
                   


                    sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    throw ex;
                }
            }

        }
        public void UpdateReceiver(Receiver receiver)
        {

            if (receiver == null)
                return;
            else
            {
                try
                {


                    SqlConnection cn = new SqlConnection(cnStr);

                    cn.Open();
                    SqlCommand sqlCommand = new SqlCommand("updateReceiver", cn);
                    sqlCommand.CommandType = CommandType.StoredProcedure;

                    ArrayList param = new ArrayList();
                    param.Add(new SqlParameter("@Name", receiver.Name));
                    param.Add(new SqlParameter("@RotateX", receiver.RotateX));
                    param.Add(new SqlParameter("@RotateY", receiver.RotateY));
                    param.Add(new SqlParameter("@RotateZ", receiver.RotateZ));
                    param.Add(new SqlParameter("@AntennaName", receiver.AntennaName));

                    sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));

                    sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    LogFileManager.ObjLog.error(ex.Message);
                    throw ex;
                }
            }

        }



        public string[] GetAntennaNames()
        {
            

            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList antennaNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAllAntennas", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    antennaNames.Add(myDataReader["Name"].ToString().Trim());
                }

                return (string[])antennaNames.ToArray(typeof(string));


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
        public string[] GetAntennaNames(string type)
        {
            
            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList antennaNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAntennaByType", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;


                //add params
                SqlParameter param = new SqlParameter("@Type", type);
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    antennaNames.Add(myDataReader["Name"].ToString().Trim());
                }

                return (string[])antennaNames.ToArray(typeof(string));


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


        public Antenna GetAntenna(string name)
        {
            

            SqlConnection cn = new SqlConnection(cnStr);
            Antenna antenna = null;

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAntenna", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@Name";
                param.Value = name;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader();
                if (myDataReader.Read())
                {

                    string type = myDataReader.GetString(1);
                    double maxGain = myDataReader.GetDouble(2);
                    double receiverThreshold = myDataReader.GetDouble(4);
                    double transmissionLoss = myDataReader.GetDouble(5);
                    double vswr = myDataReader.GetDouble(6);
                    double temperature = myDataReader.GetDouble(7);
                    string polarization = null;
                    if (!myDataReader.IsDBNull(3))
                    {
                        polarization = myDataReader.GetString(3);
                    }
                    double? radius = null;
                    if (!myDataReader.IsDBNull(8))
                    {
                        radius = myDataReader.GetDouble(8);
                    }
                    double? blockageRadius = null;
                    if (!myDataReader.IsDBNull(9))
                    {
                        blockageRadius = myDataReader.GetDouble(9);
                    }
                    string apertureDistribution = null;
                    if (!myDataReader.IsDBNull(10))
                    {
                        apertureDistribution = myDataReader.GetString(10);
                    }
                    double? edgeTeper = null;
                    if (!myDataReader.IsDBNull(11))
                    {
                        edgeTeper = myDataReader.GetDouble(11);
                    }
                    double? length = null;
                    if (!myDataReader.IsDBNull(12))
                    {
                        length = myDataReader.GetDouble(12);
                    }
                    double? pitch = null;
                    if (!myDataReader.IsDBNull(13))
                    {
                        pitch = myDataReader.GetDouble(13);
                    }

                    antenna = new Antenna(name, type, maxGain, receiverThreshold, transmissionLoss, vswr, temperature,
                                          polarization, radius, blockageRadius, apertureDistribution, edgeTeper, length, pitch);
                }

                return antenna;


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

        public void AddAntenna(Antenna antenna)
        {
            try
            {
               
                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("addAntenna", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ArrayList param = new ArrayList();
                param.Add(new SqlParameter("@Name", antenna.Name));
                param.Add(new SqlParameter("@Type", antenna.Type));
                param.Add(new SqlParameter("@MaxGain", antenna.MaxGain));
                param.Add(new SqlParameter("@Polarization", antenna.Polarization));
                param.Add(new SqlParameter("@ReceiverThreshold", antenna.RecieveThrehold));
                param.Add(new SqlParameter("@TransmisionLoss", antenna.TransmissionLoss));
                param.Add(new SqlParameter("@VSWR", antenna.VSWR));
                param.Add(new SqlParameter("@Temperature", antenna.Temperature));
                param.Add(new SqlParameter("@Radius", antenna.Radius));
                param.Add(new SqlParameter("@BlockageRadius", antenna.BlockageRadius));
                param.Add(new SqlParameter("@ApetureDistribution", antenna.ApertureDistribution));
                param.Add(new SqlParameter("@EdgeTeper", antenna.EdgeTeper));
                param.Add(new SqlParameter("@Length", antenna.Length));
                param.Add(new SqlParameter("@Pitch", antenna.Pitch));




                sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }

        }

        public void DeleteAntenna(string name)
        {
            try
            {
                
                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("deleteAntenna", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;


                SqlParameter param = new SqlParameter("@Name",name);
                sqlCommand.Parameters.Add(param);
                sqlCommand.ExecuteNonQuery();



            }
            catch (SqlException ex)
            {
                //删除失败，有辐射源正在引用
                LogFileManager.ObjLog.error(ex.Message);
                //throw ex;
            }

        }

        public void UpdateAntenna(Antenna antenna)
        {
            try
            {
               

                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("updateAntenna", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ArrayList param = new ArrayList();
                param.Add(new SqlParameter("@Name", antenna.Name));
                param.Add(new SqlParameter("@Type", antenna.Type));
                param.Add(new SqlParameter("@MaxGain", antenna.MaxGain));
                param.Add(new SqlParameter("@Polarization", antenna.Polarization));
                param.Add(new SqlParameter("@ReceiverThreshold", antenna.RecieveThrehold));
                param.Add(new SqlParameter("@TransmisionLoss", antenna.TransmissionLoss));
                param.Add(new SqlParameter("@VSWR", antenna.VSWR));
                param.Add(new SqlParameter("@Temperature", antenna.Temperature));
                param.Add(new SqlParameter("@Radius", antenna.Radius));
                param.Add(new SqlParameter("@BlockageRadius", antenna.BlockageRadius));
                param.Add(new SqlParameter("@ApetureDistribution", antenna.ApertureDistribution));
                param.Add(new SqlParameter("@EdgeTeper", antenna.EdgeTeper));
                param.Add(new SqlParameter("@Length", antenna.Length));
                param.Add(new SqlParameter("@Pitch", antenna.Pitch));





                sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }



        public string[] GetWaveFormNames()
        {

           

            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList waveFormNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getAllWaveForms", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    waveFormNames.Add(myDataReader["Name"].ToString().Trim());
                }

                return (string[])waveFormNames.ToArray(typeof(string));


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
        public string[] GetWaveFormNames(string type)
        {

           

            SqlConnection cn = new SqlConnection(cnStr);
            ArrayList waveFormNames = new ArrayList();

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getWaveFormByType", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter("@Type", type);
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                while (myDataReader.Read())
                {
                    waveFormNames.Add(myDataReader["Name"].ToString().Trim());
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
            return (string[])waveFormNames.ToArray(typeof(string));
        }


        public WaveForm GetWaveForm(string name)
        {
            
            SqlConnection cn = new SqlConnection(cnStr);
            WaveForm waveform = null;

            try
            {
                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("getWaveForm", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //add params
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@WaveFormName";
                param.Value = name;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param);

                SqlDataReader myDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                if (myDataReader.Read())
                {
                    string type = myDataReader.GetString(1);

                    double frequency = myDataReader.GetDouble(2);
                    double bandwidth = myDataReader.GetDouble(3);
                    double? phase = null;
                    if (!myDataReader.IsDBNull(4))
                    {
                        phase = myDataReader.GetDouble(4);
                    }
                    double? startFre = null;
                    if (!myDataReader.IsDBNull(5))
                    {
                        startFre = myDataReader.GetDouble(5);
                    }
                    double? endFre = null;
                    if (!myDataReader.IsDBNull(6))
                    {
                        endFre = myDataReader.GetDouble(6);
                    }
                    double? rolloffFactor = null;
                    if (!myDataReader.IsDBNull(7))
                    {
                        rolloffFactor = myDataReader.GetDouble(7);
                    }
                    string frechangeRate = null;
                    if (!myDataReader.IsDBNull(8))
                    {
                        frechangeRate = myDataReader.GetString(8);
                    }
                    waveform = new WaveForm(name, type, frequency, bandwidth, phase, startFre, endFre, rolloffFactor, frechangeRate);

                }

                return waveform;


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

        public void AddWaveForm(WaveForm waveForm)
        {

            try
            {

               

                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("addWaveForm", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ArrayList param = new ArrayList();
                param.Add(new SqlParameter("@Name", waveForm.Name));
                param.Add(new SqlParameter("@Type", waveForm.Type));
                param.Add(new SqlParameter("@Frequency", waveForm.Frequency));
                param.Add(new SqlParameter("@BandWidth", waveForm.BandWidth));
                param.Add(new SqlParameter("@Phase", waveForm.Phase));
                param.Add(new SqlParameter("@StartFrequency", waveForm.StartFrequency));
                param.Add(new SqlParameter("@EndFrequency", waveForm.EndFrequency));
                param.Add(new SqlParameter("@RollOffFactor", waveForm.RollOffFactor));
                param.Add(new SqlParameter("@FreChangeRate", waveForm.FreChangeRate));
                


                sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }

        }

        public void DeleteWaveForm(string name)
        {
            try
            {

                
                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("deleteWaveForm", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;


                SqlParameter param = new SqlParameter();

                param.ParameterName = "@WaveFormName";
                param.Value = name;
                param.SqlDbType = SqlDbType.NVarChar;
                param.Direction = ParameterDirection.Input;
                sqlCommand.Parameters.Add(param);
                sqlCommand.ExecuteNonQuery();



            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }

        public void UpdateWaveForm(WaveForm waveForm)
        {
            try
            {

                
                SqlConnection cn = new SqlConnection(cnStr);

                cn.Open();
                SqlCommand sqlCommand = new SqlCommand("updateWaveForm", cn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ArrayList param = new ArrayList();
                param.Add(new SqlParameter("@Name", waveForm.Name));
                param.Add(new SqlParameter("@Type", waveForm.Type));
                param.Add(new SqlParameter("@Frequency", waveForm.Frequency));
                param.Add(new SqlParameter("@BandWidth", waveForm.BandWidth));
                param.Add(new SqlParameter("@Phase", waveForm.Phase));
                param.Add(new SqlParameter("@StartFrequency", waveForm.StartFrequency));
                param.Add(new SqlParameter("@EndFrequency", waveForm.EndFrequency));
                param.Add(new SqlParameter("@RollOffFactor", waveForm.RollOffFactor));
                param.Add(new SqlParameter("@FreChangeRate", waveForm.FreChangeRate));



                sqlCommand.Parameters.AddRange((SqlParameter[])param.ToArray(typeof(SqlParameter)));


                sqlCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                LogFileManager.ObjLog.error(ex.Message);
                throw ex;
            }
        }


    }
}

