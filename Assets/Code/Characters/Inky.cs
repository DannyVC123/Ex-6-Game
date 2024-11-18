using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Inky : MonoBehaviour
{
    public static int WALL = 0, GROUND = 1;
    private int[,] tiles;
    private float tileSize;

    private int height, width;

    string[] orientations = new string[] { "u", "d", "l", "r" };
    private Dictionary<string, Vector2Int> directions = new Dictionary<string, Vector2Int>
    {
        { "u", new Vector2Int( 0,   1)},
        { "d", new Vector2Int( 0,  -1)},
        { "l", new Vector2Int(-1,   0)},
        { "r", new Vector2Int( 1,   0)}
    };

    private Vector2Int currPos;
    private Vector2Int targetTile;
    private Vector2Int cornerTile;

    private float timeBetweenMoves = 1f / 6f;
    private float prevTime;

    public static bool SCATTER = false, CHASE = true;
    private float timePerChase = 10f, timePerScatter = 5f;
    private bool currMode;
    private float prevTimeMode;

    private Pacman pacman;
    private Blinky blinky;
    private int targetOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        MazeGeneration mg = FindObjectOfType<MazeGeneration>();
        tiles = mg.GetTiles();
        tileSize = mg.GetTileSize();

        int[] dimensions = mg.GetDimensions();
        height = dimensions[0];
        width = dimensions[1];

        pacman = FindObjectOfType<Pacman>();
        blinky = FindObjectOfType<Blinky>();

        currMode = SCATTER;
        cornerTile = new Vector2Int(width - 2, 1);
        targetTile = cornerTile;

        prevTime = prevTimeMode = Time.time;
    }

    public void Initialize(Vector2Int startPos, int offset)
    {
        currPos = startPos;
        targetOffset = offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        CalculateTargetTile();

        float currentTime = Time.time;
        if (currentTime - prevTime > timeBetweenMoves)
        {
            Move();
            prevTime = currentTime;
        }

        float timeBetween = currMode == SCATTER ? timePerScatter : timePerChase;
        if (currentTime - prevTimeMode >= timeBetween)
        {
            currMode = !currMode;
            prevTimeMode = currentTime;
        }
    }

    private void CalculateTargetTile()
    {
        if (currMode == SCATTER)
        {
            targetTile = cornerTile;
            return;
        }

        Vector2Int pacmanPos = pacman.GetCurrPos(), blinkyPos = blinky.GetCurrPos();

        Vector2Int pacmanPosOffset = pacmanPos + directions[pacman.GetCurrOrientation()] * targetOffset;
        targetTile = blinkyPos + (pacmanPosOffset - blinkyPos) * 2;

        if (!inBounds(targetTile) || tiles[targetTile.x, targetTile.y] == WALL)
        {
            int numSteps = Mathf.RoundToInt(Vector2.Distance(blinkyPos, targetTile));
            float stepSize = 1f / numSteps;
            for (int i = numSteps - 1; i >= 0; i--)
            {
                Vector2Int potentialTarget = Vector2Int.RoundToInt(Vector2.Lerp(blinkyPos, targetTile, i * stepSize));
                if (inBounds(potentialTarget) && tiles[potentialTarget.x, potentialTarget.y] != WALL)
                {
                    targetTile = potentialTarget;
                    break;
                }

            }
        }
    }

    private bool inBounds(Vector2Int pos)
    {
        return 1 <= pos.x && pos.x < width - 1 && 1 <= pos.y && pos.y < height - 1;
    }

    private void Move()
    {
        Vector2Int newPos = GetNextPos();

        Vector2Int direction = newPos - currPos;
        transform.position = new Vector3(
            transform.position.x + direction.x * tileSize,
            transform.position.y + direction.y * tileSize,
            0
        );
        currPos = newPos;
    }

    public Vector2Int GetNextPos()
    {
        Queue<Vector2Int> bfs = new Queue<Vector2Int>();
        bfs.Enqueue(targetTile);

        bool[,] visited = new bool[width, height];
        visited[targetTile.x, targetTile.y] = true;

        while (bfs.Count > 0)
        {
            Vector2Int currTile = bfs.Dequeue();

            ShuffleArray(orientations);
            foreach (string orientation in orientations)
            {
                Vector2Int direction = directions[orientation];
                Vector2Int newPos = currTile + direction;

                if (tiles[newPos.x, newPos.y] == WALL || visited[newPos.x, newPos.y])
                {
                    continue;
                }

                // If the target position is reached
                if (Vector2.SqrMagnitude(newPos - currPos) == 0)
                {
                    return currTile;
                }

                bfs.Enqueue(newPos);
                visited[newPos.x, newPos.y] = true;
            }
        }

        // Default return in case no path is found
        return currPos;
    }

    //
    // HELPER METHODS
    //
    public static void ShuffleArray(string[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
