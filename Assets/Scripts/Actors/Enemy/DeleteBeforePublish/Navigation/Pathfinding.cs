using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors.Enemy.Navigation;
using UnityEngine;

[RequireComponent(typeof(PFGrid))]
public class Pathfinding : MonoBehaviour
{
    private PFGrid _grid;
    [SerializeField] private Transform _startPosition = null;
    [SerializeField] private Transform _targetPosition = null;
    
    void Awake()
    {
        _grid = GetComponent<PFGrid>();
    }

    private void Update()
    {
        FindPath(_startPosition.position, _targetPosition.position);
    }

    private void FindPath(Vector3 startPosition, Vector3 targetPosition)
    {
        PFNode startNode = _grid.GetNodeFromWorldPosition(startPosition);
        PFNode targetNode = _grid.GetNodeFromWorldPosition(targetPosition);
        
        // The list for all the checked nodes, and get deleted each time
        List<PFNode> openList = new List<PFNode>();
        
        // The hashset of the relevant nodes for the path
        HashSet<PFNode> closedList = new HashSet<PFNode>();
        
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PFNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].FCost < currentNode.FCost || openList[i].FCost == currentNode.FCost
                    && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                GetFinalPath(startNode, targetNode);
            }

            foreach (PFNode neighborNode in _grid.GetNeighboringNodes(currentNode))
            {
                if (neighborNode.IsObstacle || closedList.Contains(neighborNode))
                {
                    continue;
                }

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode);

                if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhattenDistance(neighborNode, targetNode);
                    neighborNode.Parent = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
    }

    private int GetManhattenDistance(PFNode nodeA, PFNode nodeB)
    {
        int ix = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int iy = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        return ix + iy;
    }

    private void GetFinalPath(PFNode startNode, PFNode endNode)
    {
        List<PFNode> finalPath = new List<PFNode>();
        PFNode currentNode = endNode;

        while (currentNode != startNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        
        Debug.Log("found path!");
        finalPath.Reverse();
        _grid.FinalPath = finalPath;
    }
}
