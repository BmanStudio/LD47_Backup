using UnityEngine;

namespace Assets.Scripts.Actors.Enemy.Navigation
{
    public class PFNode
    {
        public int GridX;
        public int GridY;

        public bool IsObstacle;
        public Vector3 Position;

        public PFNode Parent;

        public int gCost;
        public int hCost;
    
        public int FCost => gCost + hCost;

        public PFNode(bool pIsObstacle, Vector3 pos, int pGridX, int pGridY)
        {
            IsObstacle = pIsObstacle; // Node is an navigation obstacle
            Position = pos; // The world position of the node
            GridX = pGridX; // X position in the grid (node array)
            GridY = pGridY; // Y position in the grid (node array)
        }
    }
}
