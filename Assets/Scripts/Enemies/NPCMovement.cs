using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCMovement : MonoBehaviour
{
    private GameObject player;
    public bool playerInSight;
    public ContactFilter2D contactFilter;
    private Vector3 home;
    private Vector3 currDestination;
    private Rigidbody2D rb;
    private bool isRunning = false;
    //private Tilemap WalkableTiles;
    //private List<Vector2> branchingPaths = new List<Vector2> {  new Vector2(1f, 0), new Vector2(1f, 1f),
    //                                                            new Vector2(0, 1f), new Vector2(1f, -1f),
    //                                                            new Vector2(0, -1f), new Vector2(-1f, -1f),
    //                                                            new Vector2(-1f, 0), new Vector2(-1f, 1f) 
    //                                                         };
    //private IDictionary<Vector2, int> dp = new Dictionary<Vector2, int>();
    //private static Vector2 offset = new Vector2(.1f, .1f);
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        //WalkableTiles = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
        // Set home position as the position where enemy was instantiated on the map
        home = gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // If player is in sightline move towards them, else return to home resting space
        if (playerInSight)
        {
            currDestination = player.transform.position;
            // Find closest points to tile and player
            //Vector3 closestColliderPoint = player.GetComponent<BoxCollider2D>().bounds.center;
            //Vector3 closestTilePoint = WalkableTiles.WorldToCell(transform.position) - transform.position;
            ////Debug.DrawRay(transform.position, closestTilePoint, Color.blue);
            //List<Vector3> potentialPathways = new List<Vector3>();
            //potentialPathways.Add(closestTilePoint);
            //potentialPathways.Add(new Vector3(closestTilePoint.x + 1, closestTilePoint.y, 0));
            //potentialPathways.Add(new Vector3(closestTilePoint.x + 1, closestTilePoint.y + 1, 0));
            //potentialPathways.Add(new Vector3(closestTilePoint.x, closestTilePoint.y + 1, 0));


            //closestTilePoint = ClosestTilePoint(potentialPathways);
            //// raycast calculations to go here
            //CalculatePathways(closestTilePoint, 0);

            //RaycastHit2D[] hits = new RaycastHit2D[10];
            //Physics2D.Raycast(transform.position, closestColliderPoint - transform.position, contactFilter, hits);
            //if (hits[1].collider?.gameObject?.tag == "Player")
            //{
            //    Debug.DrawRay(transform.position, closestColliderPoint - transform.position, Color.green);
            //}
            //else
            //{
            //    Debug.DrawRay(transform.position, closestColliderPoint - transform.position, Color.red);
            //}
        }
        else
        {
            currDestination = home;
        }
        
        Vector2 playerDirection = (currDestination - gameObject.transform.position);

        if (currDestination != home)
        {
            if (Mathf.Abs(Vector2.Distance(player.transform.position, transform.position)) > 1 && !isRunning)
            {
                StopAllCoroutines();
                rb.velocity = playerDirection.normalized;
            }
            else
            {
                StartCoroutine(NPCRun());
            }
        }
        else
        {
            if (Mathf.Abs(Vector2.Distance(home, transform.position)) > .1)
                rb.velocity = playerDirection.normalized;    
            else
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
    }
    IEnumerator NPCRun()
    {
        isRunning = true;
        rb.velocity = (currDestination - gameObject.transform.position).normalized * -.8f;
        yield return new WaitForSeconds(1);
        isRunning = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            playerInSight = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            playerInSight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            rb.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
            playerInSight = false;
        }
    }
    //// FIXME: To make this better, make one statically shared object that has all the paths, then the enemies slowly map everything and no longer have the need to calculate besides finding
    //private void CalculatePathways(Vector2 startingVector, int tileWidth)
    //{
    //    Vector2 closestColliderPoint = player.GetComponent<BoxCollider2D>().bounds.center;
    //    Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
    //    if (tileWidth >= 4)
    //    {
    //        return;
    //    }
    //    if (Vector2.Distance(startingVector + playerPos, closestColliderPoint) <= 1)
    //    {
    //        Debug.DrawRay(startingVector + playerPos, (startingVector + playerPos - closestColliderPoint) * -1, Color.green);
    //        return;
    //    }
    //    branchingPaths.ForEach(path =>
    //    {
    //        RaycastHit2D[] hits = new RaycastHit2D[10];
    //        Physics2D.Raycast(startingVector + playerPos, path, contactFilter, hits, 1f);
    //        if (hits[0].collider?.gameObject?.tag != "Obstacles")
    //        {
    //            CalculatePathways(startingVector + path, tileWidth + 1);
    //            Debug.DrawRay(startingVector + playerPos, path, Color.blue);
    //        }

    //    });
    //}
    //private Vector3 ClosestTilePoint(List<Vector3> vectors)
    //{
    //    Vector3 closest = vectors.OrderBy(x => x.sqrMagnitude).First();

    //    Debug.DrawRay(transform.position, closest, Color.green);
    //    return closest;
    //}
}
