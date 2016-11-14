using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Part2Project.GraphBasedDataStructures
{
    class GraphEdge : IComparable
    {
        private GraphNode v1, v2; // The two vertices that this edge connects
        private double weight;

        public GraphEdge(GraphNode node1, GraphNode node2, double w)
        {
            v1 = node1;
            v2 = node2;

            weight = w;
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
                return this.weight.CompareTo(otherEdge.weight);
            }
        }

        #region Properties

        public double Weight
        {
            get { return weight; }
        } //R
        public GraphNode V1
        {
            get { return v1; }
        } //R
        public GraphNode V2
        {
            get { return v2; }
        } //R

        #endregion
    }
}
