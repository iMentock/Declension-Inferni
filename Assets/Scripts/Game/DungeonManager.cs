﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum DungeonType { Caverns, Rooms, Winding }

public class DungeonManager : MonoBehaviour
{
    public GameObject[] randomDecorations, randomEnemies, roundedEdges;
    public GameObject floorPrefab, wallPrefab, tilePrefab, exitPrefab;
    [Range(50, 5000)] public int totalFloorCount;
    [Range(0, 100)] public int decorationSpawnPercent;
    [Range(0, 100)] public int enemySpawnPercent;
    [Range(5, 10)] public int roomMaxWidth;
    [Range(5, 10)] public int roomMaxHeight;
    public bool useRoundedEdges;
    public DungeonType dungeonType;
    [Range(0, 100)] public int windingHallPercent;

    [HideInInspector] public float minX, maxX, minY, maxY;

    private List<Vector3> floorList = new List<Vector3>();

    LayerMask floorMask, wallMask;

    Vector2 hitSize;

    private void Start()
    {
        hitSize = Vector2.one * 0.8f;
        floorMask = LayerMask.GetMask("Floor");
        wallMask = LayerMask.GetMask("Wall");

        switch (dungeonType)
        {
            case DungeonType.Caverns: RandomWalker(); break;
            case DungeonType.Rooms: RoomWalker(); break;
            case DungeonType.Winding: WindingWalker(); break;
        }

    }

    private void Update()
    {
        if (Application.isEditor && Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void RandomWalker()
    {
        Vector3 curPos = Vector3.zero;

        // set floor tile at current position
        floorList.Add(curPos);

        while (floorList.Count < totalFloorCount)
        {
            curPos += RandomDirection();

            if (!InFloorList(curPos))
            {
                floorList.Add(curPos);
            }
        }

        // wait on floors and walls to finish (start in each tile spawner)
        StartCoroutine(DelayProgress());
    }

    void RoomWalker()
    {
        Vector3 curPos = Vector3.zero;

        // set floor tile at current position
        floorList.Add(curPos);

        while (floorList.Count < totalFloorCount)
        {
            curPos = TakeAHike(curPos);
            RandomRoom(curPos);
        }

        // wait on floors and walls to finish (start in each tile spawner)
        StartCoroutine(DelayProgress());
    }

    void WindingWalker()
    {
        Vector3 curPos = Vector3.zero;

        // set floor tile at current position
        floorList.Add(curPos);

        while (floorList.Count < totalFloorCount)
        {
            curPos = TakeAHike(curPos);
            int roll = Random.Range(0, 100);
            if (roll > windingHallPercent)
            {
                RandomRoom(curPos);
            }
        }

        // wait on floors and walls to finish (start in each tile spawner)
        StartCoroutine(DelayProgress());
    }

    Vector3 TakeAHike(Vector3 myPos)
    {
        Vector3 walkDir = RandomDirection();
        int walkLength = Random.Range(9, 18);

        for (int i = 0; i < walkLength; i++)
        {
            if (!InFloorList(myPos + walkDir))
            {
                floorList.Add(myPos + walkDir);
            }
            myPos += walkDir;
        }

        return myPos;
    }

    void RandomRoom(Vector3 myPos)
    {
        // random room at end of long walk
        int width = Random.Range(1, roomMaxWidth);
        int height = Random.Range(1, roomMaxHeight);

        for (int w = -width; w <= width; w++)
        {
            for (int h = -height; h <= height; h++)
            {
                Vector3 offset = new Vector3(w, h, 0);
                if (!InFloorList(myPos + offset))
                {
                    floorList.Add(myPos + offset);
                }
            }
        }
    }

    bool InFloorList(Vector3 myPos)
    {
        for (int i = 0; i < floorList.Count; i++)
        {
            // make sure is unique
            if (Vector3.Equals(myPos, floorList[i]))
            {
                return true;
            }
        }
        return false;
    }

    Vector3 RandomDirection()
    {
        switch (Random.Range(1, 5))
        {
            case 1: return Vector3.up;
            case 2: return Vector3.right;
            case 3: return Vector3.down;
            case 4: return Vector3.left;
        }
        return Vector3.zero;
    }

    IEnumerator DelayProgress()
    {
        // set floor tile at new position
        for (int i = 0; i < floorList.Count; i++)
        {
            GameObject goTile = Instantiate(tilePrefab, floorList[i], Quaternion.identity) as GameObject;
            goTile.name = tilePrefab.name;
            goTile.transform.SetParent(transform);
        }

        // wait till all floors are placed
        while (FindObjectsOfType<TileSpawner>().Length > 0)
        {
            yield return null;
        }

        ExitDoorway();

        for (int x = (int)minX - 2; x <= (int)maxX + 2; x++)
        {
            for (int y = (int)minY - 2; y <= (int)maxY + 2; y++)
            {
                Collider2D hitFloor = Physics2D.OverlapBox(new Vector2(x, y), hitSize, 0, floorMask);

                if (hitFloor)
                {
                    // make sure it's not the exit doorway
                    if (!Vector2.Equals(hitFloor.transform.position, floorList[floorList.Count - 1]))
                    {
                        Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x, y + 1), hitSize, 0, wallMask);
                        Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1, y), hitSize, 0, wallMask);
                        Collider2D hitBottom = Physics2D.OverlapBox(new Vector2(x, y - 1), hitSize, 0, wallMask);
                        Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1, y), hitSize, 0, wallMask);
                        RandomDecorations(hitFloor);
                        RandomEnemies(hitFloor, hitTop, hitRight, hitBottom, hitLeft);
                    }
                }

                RoundedEdges(x, y);
            }
        }
    }

    void RoundedEdges(int x, int y)
    {
        if (useRoundedEdges)
        {
            Collider2D hitWall = Physics2D.OverlapBox(new Vector2(x, y), hitSize, 0, wallMask);

            if (hitWall)
            {
                Collider2D hitTop = Physics2D.OverlapBox(new Vector2(x, y + 1), hitSize, 0, wallMask);
                Collider2D hitRight = Physics2D.OverlapBox(new Vector2(x + 1, y), hitSize, 0, wallMask);
                Collider2D hitBottom = Physics2D.OverlapBox(new Vector2(x, y - 1), hitSize, 0, wallMask);
                Collider2D hitLeft = Physics2D.OverlapBox(new Vector2(x - 1, y), hitSize, 0, wallMask);

                int bitVal = 0;

                if (!hitTop) { bitVal += 1; }
                if (!hitRight) { bitVal += 2; }
                if (!hitBottom) { bitVal += 4; }
                if (!hitLeft) { bitVal += 8; }

                if (bitVal > 0)
                {
                    GameObject goEdge = Instantiate(roundedEdges[bitVal], new Vector2(x, y), Quaternion.identity) as GameObject;
                    goEdge.name = roundedEdges[bitVal].name;
                    goEdge.transform.SetParent(hitWall.transform);
                }
            }
        }
    }

    void RandomEnemies(Collider2D hitFloor, Collider2D hitTop, Collider2D hitRight, Collider2D hitBottom, Collider2D hitLeft)
    {
        if (!hitTop && !hitRight && !hitBottom && !hitLeft)
        {
            int roll = Random.Range(1, 101);
            if (roll <= enemySpawnPercent)
            {
                int enemyIndex = Random.Range(0, randomEnemies.Length);
                GameObject goEnemy = Instantiate(randomEnemies[enemyIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
                goEnemy.name = randomEnemies[enemyIndex].name;
                goEnemy.transform.SetParent(hitFloor.transform);
            }
        }
    }

    void RandomDecorations(Collider2D hitFloor)
    {
        int roll = Random.Range(1, 101);
        if (roll <= decorationSpawnPercent)
        {
            int decorIndex = Random.Range(0, randomDecorations.Length);
            GameObject goItem = Instantiate(randomDecorations[decorIndex], hitFloor.transform.position, Quaternion.identity) as GameObject;
            goItem.name = randomDecorations[decorIndex].name;
            goItem.transform.SetParent(hitFloor.transform);
        }
    }

    void ExitDoorway()
    {
        // set as last space
        Vector3 doorPos = floorList[floorList.Count - 1];
        GameObject goDoor = Instantiate(exitPrefab, doorPos, Quaternion.identity) as GameObject;
        goDoor.name = exitPrefab.name;
        goDoor.transform.SetParent(transform);
    }
}
