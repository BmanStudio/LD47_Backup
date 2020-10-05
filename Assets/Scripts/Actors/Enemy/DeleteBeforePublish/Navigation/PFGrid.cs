using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Actors.Enemy.Navigation
{
    public class PFGrid : MonoBehaviour
    {
        public Transform StartPosition;
        public LayerMask ObstacleLayerMask;
        public Vector2 GridWorldSize;
        public float NodeRadius;
        public float Distance;

        private PFNode[,] _grid;
        public List<PFNode> FinalPath;

        private float _nodeDiameter;
        private int _gridSizeX, _gridSizeY;

        private void Start()
        {
            _nodeDiameter = NodeRadius * 2;
            _gridSizeX = Mathf.RoundToInt(GridWorldSize.x / _nodeDiameter);
            _gridSizeY = Mathf.RoundToInt(GridWorldSize.y / _nodeDiameter);
            CreateGrid();
        }

        void CreateGrid()
        {
            _grid = new PFNode[_gridSizeX,_gridSizeY];
            Vector3 bottomLeft = transform.position - Vector3.right * GridWorldSize.x / 2 -
                                 Vector3.forward * GridWorldSize.y / 2;
            
            // Iterate through the grid
            for (int y = 0; y < _gridSizeY; y++) // todo check for bug
            {
                for (int x = 0; x < _gridSizeX; x++)
                {
                    Vector3 worldPoint = bottomLeft + Vector3.right * (x * _nodeDiameter + NodeRadius) +
                                         Vector3.forward * (y * _nodeDiameter + NodeRadius);

                    bool obstacle = Physics.CheckSphere(worldPoint, NodeRadius, ObstacleLayerMask); // todo check for bug
                    _grid[x,y] = new PFNode(obstacle, worldPoint, x, y);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(GridWorldSize.x, 1, GridWorldSize.y));

            if (_grid != null)
            {
                foreach (var node in _grid)
                {
                    if (node.IsObstacle)
                    {
                        Gizmos.color = Color.white;
                    }
                    else
                    {
                        Gizmos.color = Color.yellow;
                    }

                    if (FinalPath != null)
                    {
                        Gizmos.color = Color.red;
                    }
                    
                    Gizmos.DrawCube(node.Position, Vector3.one * (_nodeDiameter - Distance));
                }
            }
        }

        public PFNode GetNodeFromWorldPosition(Vector3 worldPosition)
        {
            float xPoint = ((worldPosition.x + GridWorldSize.x / 2) / GridWorldSize.x);
            float yPoint = ((worldPosition.y + GridWorldSize.y / 2) / GridWorldSize.y);

            xPoint = Mathf.Clamp01(xPoint);
            yPoint = Mathf.Clamp01(yPoint);

            int x = Mathf.RoundToInt((_gridSizeX - 1) * xPoint);
            int y = Mathf.RoundToInt((_gridSizeY - 1) * yPoint);

            return _grid[x, y];
        }

        public List<PFNode> GetNeighboringNodes(PFNode node)
        {
            List<PFNode>neighboringNodes = new List<PFNode>();
            int xCheck;
            int yCheck;
            
            // Right side
            xCheck = node.GridX + 1;
            yCheck = node.GridY;
            if (xCheck >= 0 && xCheck < _gridSizeX)
            {
                if (yCheck >= 0 && yCheck < _gridSizeY)
                {
                    neighboringNodes.Add(_grid[xCheck, yCheck]);
                }
            }
            
            // Left side
            xCheck = node.GridX - 1;
            yCheck = node.GridY;
            if (xCheck >= 0 && xCheck < _gridSizeX)
            {
                if (yCheck >= 0 && yCheck < _gridSizeY)
                {
                    neighboringNodes.Add(_grid[xCheck, yCheck]);
                }
            }
            
            // Top side
            xCheck = node.GridX;
            yCheck = node.GridY + 1;
            if (xCheck >= 0 && xCheck < _gridSizeX)
            {
                if (yCheck >= 0 && yCheck < _gridSizeY)
                {
                    neighboringNodes.Add(_grid[xCheck, yCheck]);
                }
            }
            
            // Bottom side
            xCheck = node.GridX;
            yCheck = node.GridY - 1;
            if (xCheck >= 0 && xCheck < _gridSizeX)
            {
                if (yCheck >= 0 && yCheck < _gridSizeY)
                {
                    neighboringNodes.Add(_grid[xCheck, yCheck]);
                }
            }

            return neighboringNodes;
        }
    }
}
