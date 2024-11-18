using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    public static float TOP = 130f / 27f, BOTTOM = -130f / 27f, LEFT = -235f / 27f, RIGHT = 235f / 27f;

    //private MazeGeneration mg;
    private float tileSize;
    private int height, width;
    private List<Vector2Int> groundTiles;

    public GameObject pacmanPrefab;
    public GameObject blinkyPrefab;
    public GameObject pinkyPrefab;
    public GameObject inkyPrefab;
    public GameObject clydePrefab;

    MazeGeneration mazeGeneration;

    // Start is called before the first frame update
    void Start()
    {
        mazeGeneration = FindObjectOfType<MazeGeneration>();
        tileSize = mazeGeneration.GetTileSize();

        int[] dimensions = mazeGeneration.GetDimensions();
        height = dimensions[0];
        width = dimensions[1];

        groundTiles = mazeGeneration.GetGroundTiles();
        ShuffleList(groundTiles);

        Setup();
    }

    private void Setup()
    {
        groundTiles = mazeGeneration.GetGroundTiles();
        ShuffleList(groundTiles);

        // pacman
        Vector2Int pacmanTile = groundTiles[0];
        GameObject pacman = Instantiate(pacmanPrefab, tilePosToWorldPos(pacmanTile), Quaternion.identity);
        SpriteRenderer pacmanSR = pacman.GetComponent<SpriteRenderer>();
        pacmanSR.sortingLayerName = "Pacman";
        pacmanSR.sortingOrder = 2;
        pacman.GetComponent<Pacman>().Initialize(pacmanTile);


        // ghosts
        int i = 1;
        float range = 8;

        Vector2Int blinkyTile = getRandTileOutOfRange(ref i, pacmanTile, range);
        GameObject blinky = InstantiateGhost(blinkyPrefab, blinkyTile);
        try
        {
            blinky.GetComponent<Blinky>().Initialize(blinkyTile);
        }
        catch
        {
            //
        }

        Vector2Int pinkyTile = getRandTileOutOfRange(ref i, pacmanTile, range);
        GameObject pinky = InstantiateGhost(pinkyPrefab, pinkyTile);
        try
        {
            pinky.GetComponent<Pinky>().Initialize(pinkyTile, 4);
        }
        catch
        {
            //
        }

        Vector2Int inkyTile = getRandTileOutOfRange(ref i, pacmanTile, range);
        GameObject inky = InstantiateGhost(inkyPrefab, inkyTile);
        try
        {
            inky.GetComponent<Inky>().Initialize(inkyTile, 2);
        }
        catch
        {
            //
        }

        Vector2Int clydeTile = getRandTileOutOfRange(ref i, pacmanTile, range);
        GameObject clyde = InstantiateGhost(clydePrefab, clydeTile);
        try
        {
            clyde.GetComponent<Clyde>().Initialize(clydeTile);
        }
        catch
        {
            //
        }
    }

    private Vector2Int getRandTileOutOfRange(ref int currInd, Vector2Int pacmanTile, float range)
    {
        Vector2Int newTile = groundTiles[currInd];
        currInd++;

        while (Vector2.Distance(pacmanTile, newTile) < range)
        {
            newTile = groundTiles[currInd];
            currInd++;
        }

        return newTile;
    }

    private Vector3 tilePosToWorldPos(Vector2Int tilePos)
    {
        return new Vector3(LEFT + tilePos.x * tileSize, BOTTOM + tilePos.y * tileSize, 0);
    }

    private GameObject InstantiateGhost(GameObject ghostPrefab, Vector2Int ghostPos)
    {
        Vector3 worldPos = tilePosToWorldPos(ghostPos);
        GameObject ghost = Instantiate(ghostPrefab, worldPos, Quaternion.identity);

        SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
        ghostSR.sortingLayerName = "Ghosts";
        ghostSR.sortingOrder = 3;

        return ghost;
    }

    public void Reset()
    {
        Pacman pacman = FindObjectOfType<Pacman>();
        Destroy(pacman.gameObject);

        Blinky blinky = FindObjectOfType<Blinky>();
        Destroy(blinky.gameObject);

        Pinky pinky = FindObjectOfType<Pinky>();
        Destroy(pinky.gameObject);

        Inky inky = FindObjectOfType<Inky>();
        Destroy(inky.gameObject);

        Clyde clyde = FindObjectOfType<Clyde>();
        Destroy(clyde.gameObject);

        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    //
    // HELPER METHODS
    //
    public static void ShuffleList(List<Vector2Int> list)
    {
        int n = list.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            Vector2Int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
