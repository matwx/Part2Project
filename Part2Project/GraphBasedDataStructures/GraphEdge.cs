using System;

namespace Part2Project.GraphBasedDataStructures
{
    class GraphEdge : IComparable
    {
        public GraphEdge(GraphNode node1, GraphNode node2, double w)
        {
            V1 = node1;
            V2 = node2;

            Weight = w;
        }

        public int CompareTo(object obj)
        {
            GraphEdge otherEdge = obj as GraphEdge;
            if (otherEdge == null)
            {
                throw new ArgumentException("Object is not a GraphEdge. You can only compare a GraphEdge to another GraphEdge.");
            }
            else
            {
                return Weight.CompareTo(otherEdge.Weight);
            }
        }

        #region Properties

        public double Weight { get; private set; } //R
        public GraphNode V1 { get; private set; } //R
        public GraphNode V2 { get; private set; } //R

        #endregion
    }
}
