using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Pathfinder : MonoBehaviour
{
    public class PathNode
    {
        public static readonly Color PATH_COLOR = Color.green;
        public static readonly Color AOE_COLOR = Color.cyan;
        
        public Transform transform;
        public PathNode parent;
        
        public float gCost;
        public float hCost;
        public float fCost;

        private readonly Color _default;

        public PathNode(Transform transform)
        {
            this.transform = transform;
            this._default = this.transform.GetComponent<MeshRenderer>().material.color;
            this.Reset();
        }

        public void CalcF()
        {
            this.fCost = this.gCost + this.hCost;
        }

        public void Reset()
        {
            this.parent = null;
            this.gCost = Mathf.Infinity;
            this.hCost = 0F;
            this.CalcF();
        }

        public void Activate(Color color)
        {
            this.transform.GetComponent<MeshRenderer>().material.color = color;
        }

        public void Deactivate()
        {
            this.transform.GetComponent<MeshRenderer>().material.color = this._default;
        }

        public Color GetColor()
        {
            return this.transform.GetComponent<MeshRenderer>().material.color;
        }
    }
    
    // Upper right and bottom left tiles of the labyrinth
    public Transform upperRightTile;
    public Transform bottomLeftTile;
    
    private PathNode[,] _labyrinth;
    
    private void Start()
    {
        // Dynamically load all walkable floor tiles and create a 2D array grid data structure
        int xWidth = (int)((this.upperRightTile.position.x - this.bottomLeftTile.position.x) / 4 + 1);
        int zWidth = (int)((this.upperRightTile.position.z - this.bottomLeftTile.position.z) / 4 + 1);
        this._labyrinth = new PathNode[xWidth, zWidth];
        
        for (int i = 0; i < this._labyrinth.GetLength(0); i++)
            for (int j = 0; j < this._labyrinth.GetLength(1); j++)
                this._labyrinth[i, j] = null;
        
        foreach (Transform child in GetComponentsInChildren<Transform>())
            if (child.gameObject.name.Contains("_Floor"))
            {
                (int x, int z) = GetIndex(child);
                this._labyrinth[x, z] = new PathNode(child);
            }
    }

    public List<PathNode> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        List<PathNode> path = new List<PathNode>();
        
        // Convert input Vector3 into PathNodes
        PathNode start = GetPathNode(startPos);
        PathNode end = GetPathNode(targetPos);

        if (start is null || end is null) return new List<PathNode>();
        
        // Reset any PathNode data to start fresh
        for (int i = 0; i < this._labyrinth.GetLength(0); i++)
            for (int j = 0; j < this._labyrinth.GetLength(1); j++)
                if (this._labyrinth[i, j] is not null) this._labyrinth[i, j].Reset();
        
        // Setup A* variables
        start.gCost = 0;
        start.hCost = GetEuclideanDistance(start.transform, end.transform);
        start.CalcF();

        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        PathNode current = start;
        closedList.Add(current);

        // A*
        while (current != end)
        {
            foreach (PathNode n in this.GetAdjacentNodes(current))
            {
                if (closedList.Contains(n))
                    continue;
                else if (openList.Contains(n))
                {
                    float g = current.gCost + GetEuclideanDistance(current.transform, n.transform);
                    if (g < n.gCost)
                    {
                        n.parent = current;
                        n.gCost = g;
                        n.CalcF();
                    }
                }
                else
                {
                    n.parent = current;
                    n.hCost = GetEuclideanDistance(n.transform, end.transform);
                    n.gCost = current.gCost + GetEuclideanDistance(current.transform, n.transform);
                    n.CalcF();
                    openList.Add(n);
                }
            }

            if (openList.Count == 0) break;

            current = GetLowestFCostNode(openList);
            openList.Remove(current);
            closedList.Add(current);
        }
        
        if (end.parent == null) return new List<PathNode>();
        
        // Calculate path from links
        current = end;
        while (current is not null)
        {
            path.Insert(0, current);
            current = current.parent;
        }
        
        return path;
    }

    public List<PathNode> FindAoE(Vector3 startPos, int maxSteps)
    {
        PathNode source = GetPathNode(startPos);
        if (source == null) return new List<PathNode>();
        
        HashSet<PathNode> visited = new HashSet<PathNode>();
        List<PathNode> queue = new List<PathNode>();
        
        // Reset any PathNode data to start fresh and add them to the queue
        for (int i = 0; i < this._labyrinth.GetLength(0); i++)
            for (int j = 0; j < this._labyrinth.GetLength(1); j++)
                if (this._labyrinth[i, j] is not null)
                {
                    PathNode n = this._labyrinth[i, j];
                    n.Reset();
                    
                    if(n == source)
                    {
                        n.gCost = 0;
                        n.CalcF();
                    }
                    
                    queue.Add(n);
                }
        
        // Dijkstra
        while (queue.Count > 0)
        {
            // Find min distance node
            PathNode min = GetLowestFCostNode(queue);

            queue.Remove(min);
            visited.Add(min);

            foreach (PathNode v in this.GetAdjacentNodes(min))
            {
                float w = GetEuclideanDistance(min.transform, v.transform);
                if (v.gCost > min.gCost + w)
                {
                    v.gCost = min.gCost + w;
                    v.CalcF();
                    v.parent = min;
                }
            }
        }
        
        // Prune a list of nodes that are within the step distance
        return visited.Where(n => n.gCost <= maxSteps).ToList();
    }
    
    private static PathNode GetLowestFCostNode(List<PathNode> list) 
    {
        PathNode min = list[0];
        foreach (PathNode n in list)
            if (n.fCost < min.fCost)
                min = n;
        return min;
    }

    private List<PathNode> GetAdjacentNodes(PathNode node)
    {
        List<PathNode> tiles = new();

        (int x, int z) = GetIndex(node.transform);

        AddTile(x + 1, z);
        AddTile(x, z + 1);
        AddTile(x - 1, z);
        AddTile(x, z - 1);
        // AddTile(x + 1, z + 1);
        // AddTile(x - 1, z - 1);
        // AddTile(x - 1, z + 1);
        // AddTile(x + 1, z - 1);

        return tiles;

        void AddTile(int xc, int zc)
        {
            if (xc < 0 || xc >= this._labyrinth.GetLength(0) || zc < 0 || zc >= this._labyrinth.GetLength(1))
                return;
            
            PathNode p = this._labyrinth[xc, zc];
            if (p != null) tiles.Add(p);
        }
    }

    private static float GetEuclideanDistance(Transform a, Transform b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.position.x - b.position.x, 2) + Mathf.Pow(a.position.z - b.position.z, 2));
    }

    private PathNode GetPathNode(Vector3 pos)
    {
        pos += new Vector3(0, 1, 0);
        if (!Physics.Raycast(pos, Vector3.down, out RaycastHit hit, 5)) 
            return null;
        
        (int x, int z) = GetIndex(hit.transform);
        return this._labyrinth[x, z];
    }

    private static (int, int) GetIndex(Transform tile)
    {
        return (((int)tile.position.x - 6) / 4, ((int)tile.position.z - 6) / 4);
    }
}
