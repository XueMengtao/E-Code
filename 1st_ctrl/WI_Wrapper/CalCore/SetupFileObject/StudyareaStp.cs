using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SetupFileObject
{
    class StudyareaStp
    {
        public string name;
        public int StudyAreaNumber;
        public string active;
        public int autoboundary;
        Model mo;
        Boundary boun;
        void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            name = str.Substring(pos, linelength).Substring(18);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            StudyAreaNumber = Convert.ToInt32(str.Substring(pos, linelength).Substring(16));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            active = str.Substring(pos, linelength);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            autoboundary = Convert.ToInt32(str.Substring(pos, linelength).Substring(13));
            int start = str.IndexOf("begin_<model>");
            linelength = Tool.readline(str, start);
            mo = new Model(str.Substring(start + linelength + 2, str.IndexOf("end_<model>") - start - linelength - 2));
            start = str.IndexOf("begin_<boundary>");
            linelength = Tool.readline(str, start);
            int end = str.IndexOf("end_<boundary>");
            boun = new Boundary(str.Substring(start + linelength + 2, str.IndexOf("end_<boundary>") - start - linelength - 2));
        }
        public StudyareaStp(string str)
        {
            init(str );
        }
    }
    class Model 
    {
        string modelType;
        string raytracingmode;
        void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            modelType = str.Substring(pos, linelength);
            pos += modelType.Length + 2;
            linelength = Tool.readline(str, pos);
            raytracingmode = str.Substring(pos, linelength).Substring(15);
        }
        public Model(string str)
        {
            init(str);
        }
    }
    class Reference 
    {
        public string coordinate;
        public double longitude;
        double latitude;
        bool visible;
        string sealevel;
       void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            coordinate = str.Substring(pos, linelength);
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            longitude = Convert.ToDouble(str.Substring(pos, linelength).Substring(10));
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            latitude = Convert.ToDouble(str.Substring(pos, linelength).Substring(9));
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            visible = Convert.ToBoolean(str.Substring(pos, linelength).Substring(8) == "yes");
            pos += str.Substring(pos, linelength).Length + 2;
            linelength = Tool.readline(str, pos);
            sealevel = str.Substring(pos, linelength);
        }
        public Reference(string str)
        {
            init(str);
        }
    }
    class Boundary 
    {
        public double zmin;
        public double zmax;
        public Reference Rf;
        public int nVertices;
        public double[,] vertices;
        public string temp;
        void init(string str)
        {
            int pos = 0;
            int linelength = Tool.readline(str, pos);
            int length = str.IndexOf("end_<reference>");
            Rf = new Reference(str.Substring(pos + linelength + 2, length - linelength - 2));
            pos = str.IndexOf("zmin");
            linelength = Tool.readline(str, pos);
            zmin = Convert.ToDouble(str.Substring(pos, linelength).Substring(5));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            zmax = Convert.ToDouble(str.Substring(pos, linelength).Substring(5));
            pos += linelength + 2;
            linelength = Tool.readline(str, pos);
            nVertices = Convert.ToInt16(str.Substring(pos, linelength).Substring(10));
            pos += linelength + 2;
            length = str.Length;
            vertices = new double[4, 3];
            // length = str.IndexOf("end_<boundary>");
            temp = str.Substring(pos, length - pos);
            pos = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 3; j++)
                {
                    length = SpaceFinding.read(temp, pos);
                    vertices[i, j] = Convert.ToDouble(temp.Substring(pos, length));
                    pos += length + 1;
                }
        }
        public Boundary(string str)
        {
            init(str);
        }
    }
}
