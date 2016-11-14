using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaliko.ImageLibrary.ColorSpace;

namespace Part2Project.GraphBasedDataStructures
{
    class GraphNode
    {
        private int x, y, rank, componentSize;
        private double internalDifference;
        private GraphNode parent;

        double totalL, totalA, totalB;

        public GraphNode(int xCoord, int yCoord, CIELab labColor)
        {
            x = xCoord;
            y = yCoord;

            rank = 0;
            componentSize = 1;
            internalDifference = 0;
            parent = this;

            totalL = labColor.L;
            totalA = labColor.A;
            totalB = labColor.B;
        }

        #region Properties

        public int X
        {
            get { return x; }
        } //R
        public int Y
        {
            get { return y; }
        } //R
        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        } //RW
        public int ComponentSize
        {
            get { return componentSize; }
            set { componentSize = value; }
        } //RW
        public double InternalDifference
        {
            get { return internalDifference; }
            set { internalDifference = value; }
        } //RW
        public GraphNode Parent
        {
            get { return parent; }
            set { parent = value; }
        } //RW
        public double TotalL
        {
            get { return totalL; }
            set { totalL = value; }
        } //RW
        public double TotalA
        {
            get { return totalA; }
            set { totalA = value; }
        } //RW
        public double TotalB
        {
            get { return totalB; }
            set { totalB = value; }
        } //RW

        #endregion
    }
}
