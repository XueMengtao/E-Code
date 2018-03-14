using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;

namespace ProjectLib
{
    public class ResultFile
    {
        private string filePath;
        private FileNameInfo fileNameInfo;
        private FileContent fileContent;

        public string FilePath
        {
            get{ return filePath;}
        }
        public FileNameInfo FileNameInfo
        {
            get{ return fileNameInfo;}
        }
        public FileContent FileContent
        {
            get{ return fileContent;}
        }

        public ResultFile()
        {
        }

        public ResultFile(string filePath)
        {
            this.filePath = filePath;
            try
            {
                this.fileNameInfo = new FileNameInfo(Path.GetFileName(filePath));
                switch (fileNameInfo.ResultType)
                {
                    case "exmag":
                    case "eymag":
                    case "ezmag":
                    case "etxmag":
                    case "etymag":
                    case "etzmag":
                        this.fileContent = new MagnitudeFileContent(this.filePath);
                        break;
                    case "exphs":
                    case "eyphs":
                    case "ezphs":
                        this.fileContent = new PhaseFileContent(this.filePath);
                        break;
                    case "pg":
                        this.fileContent = new PathGainFileContent(this.filePath);
                        break;
                    case "pl":
                        this.fileContent = new PathLossFileContent(this.filePath);
                        break;
                    case "spread":
                        this.fileContent = new SpreadFileContent(this.filePath);
                        break;
                    case "powertotal":
                    case "power":
                        this.fileContent = new PowerFileContent(this.filePath);
                        break;
                    case "situationerm":
                        this.fileContent = new SituationErmContent(this.filePath);
                        break;
                    case "situationpowertotal":
                        this.fileContent = new SituationPowerContent(this.filePath);
                        break;
                    default:
                        break;
                }
            }
            //catch (WrongFileException ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    LogFileManager.ObjLog.debug(ex.Message);
            //    File.Delete(this.filePath);
            //}
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                LogFileManager.ObjLog.debug(ex.Message);
                File.Delete(this.filePath);
            }
            catch (LackOfFileContentException e)
            {
                LogFileManager.ObjLog.debug(e.Message);
                Console.WriteLine(e.Message);
                File.Delete(this.filePath);
            }
        }
    }   
}
