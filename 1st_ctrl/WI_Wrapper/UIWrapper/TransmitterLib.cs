using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace WF1
{
    public  class TransmitterLib
    {

        //get all the TransmitterName in Lib
        public string[] GetTransmitterName()
        {
           
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from transmitter in dbconnection.Transmitter
                    select transmitter.Name;
                if (querry.Count() == 0)
                    return null;
                else
                {
                    ArrayList nameArray = new ArrayList();
                    foreach (string name in querry)
                    {
                        nameArray.Add(name);
                       
                    }
                    return (string[])nameArray.ToArray(typeof(string));
                    
                }
            }

        }  

        
        //select the transmitter by name
        public Transmitter GetTransmitter(string name)
        {
            
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                   from trans in dbconnection.Transmitter
                   where trans.Name==name
                   select trans;
                if (querry.Count() != 1)
                    return null;
                else
                {
                    return querry.First();
                }

            }
          
        }

        
        //Add new transmitter
        public void AddTransimitter(Transmitter transmitter)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                dbconnection.Transmitter.InsertOnSubmit(transmitter);

                try
                {
                    dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                
                
            }
        }

        //delete the tansmitter by name
        public void DeleteTransmitter(string name)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from transmitter in dbconnection.Transmitter
                    where transmitter.Name == name
                    select transmitter;
                if (querry.Count() > 0)
                {
                   
                    dbconnection.Transmitter.DeleteOnSubmit(querry.First());
                    try
                    {
                        dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    throw (new DBException("the transmitter does not exist"));
                }
            }
        }

        //update the tansmitter
        public void UpdateTransmitter(Transmitter transmitter)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from trans in dbconnection.Transmitter
                    where trans.Name == transmitter.Name
                    select trans;
                if (querry.Count() > 0)
                {
                    dbconnection.Transmitter.DeleteOnSubmit(querry.First());
                    dbconnection.Transmitter.InsertOnSubmit(transmitter);
                    try
                    {
                        dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                   
                }

            }
        }
       


        public string[] GetAntennaName()
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from antenna in dbconnection.Antenna
                    select antenna.Name;
                if (querry.Count() == 0)
                    return null;
                else
                {
                    ArrayList nameArray = new ArrayList();
                    foreach (string name in querry)
                    {
                        nameArray.Add(name);

                    }
                    return (string[])nameArray.ToArray(typeof(string));

                }
            }
        }


        public Antenna GetAntenna(string name)
        {

            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                   from antenna in dbconnection.Antenna
                   where antenna.Name==name
                   select antenna;
                if (querry.Count() != 1)
                    return null;
                else
                {
                    return querry.First();
                }

            }

        }

        public void AddAntenna(Antenna antenna)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                dbconnection.Antenna.InsertOnSubmit(antenna);

                try
                {
                    dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }


            }
        }

        public void DeleteAntenna(string name)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from antenna in dbconnection.Antenna
                    where antenna.Name == name
                    select antenna;
                if (querry.Count() > 0)
                {
                    dbconnection.Antenna.DeleteOnSubmit(querry.First());
                    try
                    {
                        dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    catch(SqlException ex)
                    {
                        throw ex;
                    }
                }
               
                
                     else
                {
                    
                    throw (new DBException("the antenna does not exist"));
                }
                }
            }

        public void UpdateAntenna(Antenna antenna)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from ante in dbconnection.Antenna
                    where ante.Name == antenna.Name
                    select ante;
                if (querry.Count() > 0)
                {
                    dbconnection.Antenna.DeleteOnSubmit(querry.First());
                    dbconnection.Antenna.InsertOnSubmit(antenna);
                    try
                    {
                        dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                }

            }
        }
        


        public string[] GetWaveFormName()
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from waveform in dbconnection.WaveForm
                    select waveform.Name;
                if (querry.Count() == 0)
                    return null;
                else
                {
                    ArrayList nameArray = new ArrayList();
                    foreach (string name in querry)
                    {
                        nameArray.Add(name);

                    }
                    return (string[])nameArray.ToArray(typeof(string));

                }
            }
        }


        public WaveForm GetWaveForm(string name)
        {

            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                   from waveform in dbconnection.WaveForm
                   where waveform.Name==name
                   select waveform;
                if (querry.Count() != 1)
                    return null;
                else
                {
                    return querry.First();
                }

            }

        }

        public void AddWaveForm(WaveForm waveForm)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                dbconnection.WaveForm.InsertOnSubmit(waveForm);

                try
                {
                    dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                }
                catch(SqlException ex)
                {
                    throw ex;
                }


            }
        }

        public void DeleteWaveForm(string name)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from waveform in dbconnection.WaveForm
                    where waveform.Name == name
                    select waveform;
                if (querry.Count() > 0)
                {
                    dbconnection.WaveForm.DeleteOnSubmit(querry.First());
                    try
                    {
                        dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    
                    throw (new DBException("the waveform does not exist"));
                }
            }
        }

        public void UpdateWaveForm(WaveForm waveform)
        {
            using (DataClassesDataContext dbconnection = new DataClassesDataContext())
            {
                var querry =
                    from wave in dbconnection.WaveForm
                    where wave.Name == waveform.Name
                    select wave;
                if (querry.Count() > 0)
                {
                    dbconnection.WaveForm.DeleteOnSubmit(querry.First());
                    dbconnection.WaveForm.InsertOnSubmit(waveform);
                    try
                    {
                        dbconnection.SubmitChanges(System.Data.Linq.ConflictMode.FailOnFirstConflict);
                    }
                    catch (SqlException ex)
                    {
                        throw ex;
                    }

                }

            }
        }


    }
}
