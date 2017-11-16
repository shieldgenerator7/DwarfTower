using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LevelManager : NetworkBehaviour
{
    public GameObject grassPrefab;
    public GameObject waterPrefab;
    public GameObject roadPrefab;
    public GameObject bridgePrefab;
    public GameObject treePrefab;
    public GameObject borderPrefab;
    [SyncVar]
    public int tileHeight = 100;//how many tiles across
    [SyncVar]
    public int tileWidth = 100;//how many tiles from top to bottom
    //SyncListStruct<NetworkTileMap> networkTileMap = new SyncListStruct<NetworkTileMap>();
    public GameObject[,] tileMap;//the map of tiles

    //struct NetworkTileMap
    //{
    //    int x, y;//where it is in the grid
    //    string type;//what type of tile it is
    //}

    private static LevelManager instance;

    // Use this for initialization
    void Start()
    {
        instance = this;

        if (isServer)
        {
            //Initialization stuff
            generateLevel(tileWidth, tileHeight);
        }
        else
        {
            tileMap = new GameObject[tileWidth, tileHeight];
            receiveLevel();
        }
    }

    private void OnConnectedToServer()
    {
        if (!isServer)
        {
            tileMap = new GameObject[tileWidth, tileHeight];
            receiveLevel();
        }
    }

    /// <summary>
    /// Returns the tile in the map in the given world position
    /// </summary>
    /// <param name="pos">The position in world space</param>
    /// <returns></returns>
    public static LevelTile getTile(Vector2 pos)
    {
        Vector2 gridCoors = vectorToGrid(pos);
        return instance.tileMap[(int)gridCoors.x, (int)gridCoors.y].GetComponent<LevelTile>();
    }
    public static Vector2 vectorToGrid(Vector2 pos)
    {
        return new Vector2(
            (int)pos.x + instance.tileWidth / 2,
            (int)pos.y + instance.tileHeight / 2);
    }
    public static int getDisplaySortingOrder(Vector2 pos)
    {
        return (int)((instance.tileHeight / 2 - pos.y) * 100);
    }
    public static Vector2 randomPosition()
    {
        return new Vector2(
            Random.Range(-instance.tileWidth / 2, instance.tileWidth / 2) * 0.9f,
            Random.Range(-instance.tileHeight / 2, instance.tileHeight / 2) * 0.9f
            );
    }
    public static bool inBounds(Vector2 pos)
    {
        return pos.x > -instance.tileWidth / 2 * 0.99f
            && pos.x < instance.tileWidth / 2 * 0.99f
            && pos.y > -instance.tileHeight / 2 * 0.99f
            && pos.y < instance.tileHeight / 2 * 0.99f;
    }

    private void generateLevel(int width, int height)
    {
        GameObject[,] tiles = new GameObject[width, height];
        generateFill(grassPrefab, tiles, width, height);
        generateRiver(waterPrefab, tiles, width, height, height / 4);
        generateRiver(waterPrefab, tiles, width, height, height / 2);
        generateRiver(waterPrefab, tiles, width, height, height / 2);
        generateRiver(waterPrefab, tiles, width, height, 3 * height / 4);
        generateRoad(roadPrefab, bridgePrefab, tiles, width, height, width / 2);
        tileMap = new GameObject[width, height];
        for (int xi = 0; xi < width; xi++)
        {
            for (int yi = 0; yi < height; yi++)
            {
                GameObject go = GameObject.Instantiate(tiles[xi, yi]);
                go.transform.position = new Vector2(xi - width / 2, yi - height / 2);
                tileMap[xi, yi] = go;
                NetworkServer.Spawn(go);
            }
        }
        generateForest(treePrefab, new Vector2(90, 90), new Vector2(0, 0));
        generateBorder(borderPrefab, width, height);
    }

    private void generateFill(GameObject prefab, GameObject[,] prefabMap, int width, int height)
    {
        for (int xi = 0; xi < width; xi++)
        {
            for (int yi = 0; yi < height; yi++)
            {
                prefabMap[xi, yi] = prefab;
            }
        }
    }

    private void generateRiver(GameObject prefab, GameObject[,] prefabMap, int width, int height, int startY)
    {
        int currentY = startY;
        int prevY = currentY;
        for (int xi = 0; xi < width; xi++)
        {
            if (Random.Range(0, 2) > 0)
            {
                currentY += Random.Range(-2, 2);
            }
            for (int yi = prevY; yi != currentY; yi += (int)Mathf.Sign(currentY - prevY))
            {
                if (yi >= 0 && yi < height)
                {
                    prefabMap[xi, yi] = prefab;
                }
            }
            if (currentY >= 0 && currentY < height)
            {
                prefabMap[xi, currentY] = prefab;
            }
            prevY = currentY;
        }
    }

    private void generateRoad(GameObject prefab, GameObject bridgePrefab, GameObject[,] prefabMap, int width, int height, int startX)
    {
        int currentX = startX;
        int prevX = currentX;
        for (int yi = 0; yi < height; yi++)
        {
            if (Random.Range(0, 2) > 0)
            {
                currentX += Random.Range(-2, 2);
            }
            for (int xi = prevX; xi != currentX; xi += (int)Mathf.Sign(currentX - prevX))
            {
                if (xi >= 0 && xi < width)
                {
                    if (prefabMap[xi, yi].name.Contains("water"))
                    {
                        prefabMap[xi, yi] = bridgePrefab;
                    }
                    else
                    {
                        prefabMap[xi, yi] = prefab;
                    }
                }
            }
            if (currentX >= 0 && currentX < width)
            {
                if (prefabMap[currentX, yi].name.Contains("water"))
                {
                    prefabMap[currentX, yi] = bridgePrefab;
                }
                else
                {
                    prefabMap[currentX, yi] = prefab;
                }
            }
            prevX = currentX;
        }
    }

    private void generateForest(GameObject prefab, Vector2 size, Vector2 pos)
    {
        for (int count = 0; count < size.x * size.y / 3; count++)
        {
            float randomX = Random.Range(-size.x / 2, size.x / 2) + pos.x;
            float randomY = Random.Range(-size.y / 2, size.y / 2) + pos.y;
            Vector2 randomPos = new Vector2(randomX, randomY);
            GameObject tile = getTile(randomPos).gameObject;
            if (!tile.name.Contains("water") && !tile.name.Contains("road"))
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.transform.position = randomPos;
                go.GetComponent<SpriteRenderer>().sortingOrder = getDisplaySortingOrder(randomPos);
                NetworkServer.Spawn(go);
            }
        }
    }

    private void generateBorder(GameObject prefab, int width, int height)
    {
        int extraWidth = width + 2;
        int extraHeight = height + 2;
        for (int xi = 0; xi < extraWidth; xi++)
        {
            for (int yi = 0; yi < extraHeight; yi++)
            {
                //Only spawn on the border
                if (xi == 0 || yi == 0 || xi == extraWidth - 1 || yi == extraHeight - 1)
                {
                    GameObject go = GameObject.Instantiate(prefab);
                    go.transform.position = new Vector2(xi - extraWidth / 2, yi - extraHeight / 2);
                    NetworkServer.Spawn(go);
                }
            }
        }
    }

    private void receiveLevel()
    {
        tileMap = new GameObject[tileWidth, tileHeight];
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("MapTile"))
        {
            Vector2 gridCoor = vectorToGrid(go.transform.position);
            tileMap[(int)gridCoor.x, (int)gridCoor.y] = go;
        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("MapObject"))
        {
            go.GetComponent<SpriteRenderer>().sortingOrder = getDisplaySortingOrder(go.transform.position);
        }
    }

    //private void generateLevel(SyncListStruct<NetworkTileMap> ntileMap)
    //{

    //}
}
