using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    DungeonManager dungMan;

    private void Awake()
    {
        // create floors
        dungMan = FindObjectOfType<DungeonManager>();

        GameObject goFloor = Instantiate(dungMan.floorPrefab, transform.position, Quaternion.identity);

        goFloor.name = dungMan.floorPrefab.name;
        goFloor.transform.SetParent(dungMan.transform);

        if (transform.position.x > dungMan.maxX)
        {
            dungMan.maxX = transform.position.x;
        }
        if (transform.position.x < dungMan.minX)
        {
            dungMan.minX = transform.position.x;
        }
        if (transform.position.y > dungMan.maxY)
        {
            dungMan.maxY = transform.position.y;
        }
        if (transform.position.y < dungMan.minY)
        {
            dungMan.minY = transform.position.y;
        }
    }

    void Start()
    {
        // create walls
        LayerMask envMask = LayerMask.GetMask("Wall", "Floor");

        /*
         * x
         * [-1,0,1]
         * y
         * [1]
         * [0]
         * [-1]
         */
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2 targetPos = new Vector2(transform.position.x + x, transform.position.y + y);
                Collider2D hit = Physics2D.OverlapBox(targetPos, Vector2.one * 0.8f, 0, envMask);
                if (!hit)
                {
                    // add a wall
                    GameObject goWall = Instantiate(dungMan.wallPrefab, targetPos, Quaternion.identity);

                    goWall.name = dungMan.wallPrefab.name;
                    goWall.transform.SetParent(dungMan.transform);
                }
            }
        }

        // destroy the spawner as the tiles have already been placed in the environment (object)
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
