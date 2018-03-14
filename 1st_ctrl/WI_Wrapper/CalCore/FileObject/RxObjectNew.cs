using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileObject
{
    class RxObjectNew
    {
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private int num;

        public int Num
        {
            get { return num; }
            set { num = value; }
        }
        private string active;

        public string Active
        {
            get { return active; }
            set { active = value; }
        }
        private bool vertical_line;

        public bool Vertical_line
        {
            get { return vertical_line; }
            set { vertical_line = value; }
        }
        private double cubesize, CVxLength, CVyLength, CVzLength;

        public double CVzLength1
        {
            get { return CVzLength; }
            set { CVzLength = value; }
        }

        public double CVyLength1
        {
            get { return CVyLength; }
            set { CVyLength = value; }
        }

        public double CVxLength1
        {
            get { return CVxLength; }
            set { CVxLength = value; }
        }

        public double Cubesize
        {
            get { return cubesize; }
            set { cubesize = value; }
        }
        private double side1, side2, spacing;

        public double Spacing
        {
            get { return spacing; }
            set { spacing = value; }
        }

        public double Side2
        {
            get { return side2; }
            set { side2 = value; }
        }

        public double Side1
        {
            get { return side1; }
            set { side1 = value; }
        }
        private string AutoPatternScale;

        public string AutoPatternScale1
        {
            get { return AutoPatternScale; }
            set { AutoPatternScale = value; }
        }
        private bool ShowDescription, CVsVisible;

        public bool CVsVisible1
        {
            get { return CVsVisible; }
            set { CVsVisible = value; }
        }

        public bool ShowDescription1
        {
            get { return ShowDescription; }
            set { ShowDescription = value; }
        }
        private int CVsThickness;

        public int CVsThickness1
        {
            get { return CVsThickness; }
            set { CVsThickness = value; }
        }
        private Location Lc;

        public Location Lc1
        {
            get { return Lc; }
            set { Lc = value; }
        }
        private double? rotation;

        public double? Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
        private Antenna at;

        public Antenna At
        {
            get { return at; }
            set { at = value; }
        }
        private Sbr sb;

        public Sbr Sb
        {
            get { return sb; }
            set { sb = value; }
        }
        private double noiseFigure;

        public double NoiseFigure
        {
            get { return noiseFigure; }
            set { noiseFigure = value; }
        }
        private bool pattern_show_arrow, pattern_show_as_sphere, generate_p2p;

        public bool Generate_p2p
        {
            get { return generate_p2p; }
            set { generate_p2p = value; }
        }

        public bool Pattern_show_as_sphere
        {
            get { return pattern_show_as_sphere; }
            set { pattern_show_as_sphere = value; }
        }

        public bool Pattern_show_arrow
        {
            get { return pattern_show_arrow; }
            set { pattern_show_arrow = value; }
        }
      
    }
}
