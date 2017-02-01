using Kaliko.ImageLibrary.ColorSpace;

namespace Part2Project.GraphBasedDataStructures
{
    class GraphNode
    {
        public GraphNode(int xCoord, int yCoord, CIELab labColor)
        {
            X = xCoord;
            Y = yCoord;

            Rank = 0;
            ComponentSize = 1;
            InternalDifference = 0;
            Parent = this;

            TotalL = labColor.L;
            TotalA = labColor.A;
            TotalB = labColor.B;
        }

        #region Properties

        public int X { get; private set; } //R
        public int Y { get; private set; } //R
        public int Rank { get; set; } //RW
        public int ComponentSize { get; set; } //RW
        public double InternalDifference { get; set; } //RW
        public GraphNode Parent { get; set; } //RW
        public double TotalL { get; set; } //RW
        public double TotalA { get; set; } //RW
        public double TotalB { get; set; } //RW

        #endregion
    }
}
