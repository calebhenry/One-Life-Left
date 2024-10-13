using System.Collections.Generic;
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
    private Tilemap WalkableTiles;
    private List<Vector3> potentialPathways = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        WalkableTiles = GameObject.Find("BaseLayer").GetComponent<Tilemap>();
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
            Vector3 closestColliderPoint = player.GetComponent<BoxCollider2D>().bounds.center;
            Vector3 closestTilePoint = WalkableTiles.WorldToCell(transform.position) - transform.position;
            potentialPathways.Add(closestTilePoint);
            potentialPathways.Add(new Vector3(closestTilePoint.x + 1, closestTilePoint.y, 0));
            potentialPathways.Add(new Vector3(closestTilePoint.x + 1, closestTilePoint.y + 1, 0));
            potentialPathways.Add(new Vector3(closestTilePoint.x, closestTilePoint.y + 1, 0));


            closestTilePoint = ClosestTilePoint(potentialPathways);
            // raycast calculations to go here
            CalculatePathways(closestTilePoint, 0);

            RaycastHit2D[] hits = new RaycastHit2D[10];
            Physics2D.Raycast(transform.position, closestColliderPoint - transform.position, contactFilter, hits);
            if (hits[1].collider?.gameObject?.tag == "Player")
            {
                Debug.DrawRay(transform.position, closestColliderPoint - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, closestColliderPoint - transform.position, Color.red);
            }
        }
        else
        {
            currDestination = home;
        }

        Vector2 playerDirection = (currDestination - gameObject.transform.position);
        rb.velocity = playerDirection.normalized;
        potentialPathways.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerInSight = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            playerInSight = false;
        }
    }
    private void CalculatePathways(Vector3 startingVector, int tileWidth)
    {
        if (tileWidth > 3)
        {
            return;
        }
        List<Vector3> pathways = new List<Vector3>();
        
        pathways.Add(new Vector3(startingVector.x - 1, startingVector.y, 0));
        pathways.Add(new Vector3(startingVector.x + 1, startingVector.y, 0));
        pathways.Add(new Vector3(startingVector.x, startingVector.y + 1, 0));
        pathways.Add(new Vector3(startingVector.x, startingVector.y - 1, 0));
        //pathways.ForEach((pathway) => { Debug.DrawRay(transform.position, pathway, Color.blue); });

        pathways.ForEach(path => { Debug.DrawRay(transform.position, path, Color.blue); 
                                   if(path != startingVector) CalculatePathways(path, tileWidth+1); });
    }
    private Vector3 ClosestTilePoint(List<Vector3> vectors)
    {
        Vector3 closest = vectors.OrderBy(x => x.sqrMagnitude).First();

        Debug.DrawRay(transform.position, closest, Color.green);
        return closest;
    }
}
