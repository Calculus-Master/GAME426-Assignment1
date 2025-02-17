using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class MovementController : MonoBehaviour
{
    [Tooltip("The desired movement speed of character.")]
    public float moveSpeed = 20.0f;

    [Tooltip("Game Object that holds the Pathfinder data.")]
    public Pathfinder pathfinder;

    [Tooltip("Tile radius of the sword's area of effect attack.")]
    public int areaEffectDist = 2;

    [Tooltip("Recharge timer for the sword's area of effect attack.")]
    public float rechargeTime = 5.0F;

    private Animator animController;
    private Camera   cam;

    // start and end points of movement
    private Vector3 _startPos;
    private Vector3 _targetPos;
    // linear interpolation parameter t
    private float t = 0.0f;

    private List<Pathfinder.PathNode> _movementPath;
    private bool _isMoving;
    private bool _canMagicAttack;

    private List<PathNodeTracker> _enemies;

    // co-routine method variable (pointer)
    private IEnumerator coroutine;

    private IEnumerator MoveToNext()
    {
        // direction vector (with magnitude)
        Vector3 moveDir;

        while (t < 1.0f)
        {
            moveDir = _targetPos - _startPos;

            // find next interpolation step, enforcing constant
            // a move rate to the target based on desired speed
            t += Time.deltaTime * moveSpeed / moveDir.magnitude;

            // transform between start position and end position
            transform.position = Vector3.Lerp(_startPos, _targetPos, t);
            
            // pause execution for 1 tick of game loop
            yield return null;
        }

        // finished with co-routine
        this._isMoving = false;
        yield return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        // get the animation controller on startup
        this.animController = GetComponent<Animator>();

        // find and save reference to scene camera
        this.cam = (Camera) FindObjectOfType (typeof (Camera));

        this._movementPath = new List<Pathfinder.PathNode>();
        this._canMagicAttack = true;
        
        // Store a list of references to all enemies
        this._enemies = new List<PathNodeTracker>(FindObjectsOfType<PathNodeTracker>());
    }

    // Update is called once per frame
    void Update()
    {
        // left mouse button (on map): Move to location
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (this.coroutine != null)
                StopCoroutine(this.coroutine);

            Ray ray = this.cam.ScreenPointToRay (Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 start = this.transform.position;
                Vector3 end = hit.point;

                // Reset old path colors if there was one
                if (this._movementPath.Count > 0)
                    foreach (Pathfinder.PathNode n in this._movementPath)
                        n.Deactivate();
                
                this._movementPath = this.pathfinder.FindPath(start, end);

                if (this._movementPath.Count > 0)
                {
                    // Remove start node
                    this._movementPath.RemoveAt(0);
                    
                    // Color path green
                    foreach (Pathfinder.PathNode n in this._movementPath)
                        n.Activate(Pathfinder.PathNode.PATH_COLOR);
                    
                    // Move
                    this._isMoving = false;
                    this.Advance();
                }
            }
        }
        else if(!this._isMoving && this.NeedsToMove()) // Continue movement path if the coroutine finishes
            this.Advance();

        // right mouse button or spacebar: Magical sword attack
        if (this._canMagicAttack && (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Space)))
        {
            animController.SetTrigger("Attack");

            List<Pathfinder.PathNode> targetTiles =
                this.pathfinder.FindAoE(this.transform.position, this.areaEffectDist);
            
            Debug.Log("Dijkstra's Algorithm: Found " + targetTiles.Count + " tiles in range.");

            foreach (Pathfinder.PathNode n in targetTiles)
            {
                n.Activate(Pathfinder.PathNode.AOE_COLOR);
                
                foreach (PathNodeTracker e in this._enemies)
                    if (e.GetCurrentGridTile().transform.position == n.transform.position)
                    {
                        e.gameObject.SetActive(false);
                        Debug.Log("Killed Enemy at " + e.transform.position);
                    }
            }

            StartCoroutine(ResetAoETileColors(targetTiles));

            StartCoroutine(MagicAttackCooldown());
        }
    }

    private IEnumerator MagicAttackCooldown()
    {
        yield return new WaitForSeconds(this.rechargeTime);
        
        this._canMagicAttack = true;
        
        yield return null;
    }

    private IEnumerator ResetAoETileColors(List<Pathfinder.PathNode> tiles)
    {
        yield return new WaitForSeconds(1.5F);
        
        foreach (Pathfinder.PathNode n in tiles.Where(n => n.GetColor() == Pathfinder.PathNode.AOE_COLOR)) 
            n.Deactivate();

        yield return null;
    }

    // Move to next spot along path (assumes a path does still exist)
    private void Advance()
    {
        this._isMoving = true;
        
        this._startPos = this.transform.position;
        this._targetPos = this._movementPath[0].transform.position;

        this._movementPath[0].Deactivate();
        this._movementPath.RemoveAt(0);

        this._targetPos.y = this._startPos.y;
        this.t = 0F;

        this.coroutine = MoveToNext();
        StartCoroutine(this.coroutine);
    }

    private bool NeedsToMove()
    {
        return this._movementPath.Count > 0;
    }
}
