using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNodeTracker : MonoBehaviour
{
    private Pathfinder _pathfinder;
    private Pathfinder.PathNode _currentGridTile;
    
    void Start()
    {
        this._pathfinder = FindObjectOfType<Pathfinder>();
    }

    void Update()
    {
        this._currentGridTile = this._pathfinder.GetPathNode(this.transform.position);
    }
    
    public Pathfinder.PathNode GetCurrentGridTile()
    {
        return this._currentGridTile;
    }
}
