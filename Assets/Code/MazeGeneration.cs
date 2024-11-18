using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class MazeGeneration : MonoBehaviour
{
    public static float TOP = 130f / 27f, BOTTOM = -130f / 27f, LEFT = -235f / 27f, RIGHT = 235f / 27f;

    public static int WALL = 0, GROUND = 1, PELLET = 2;
    public GameObject tilePrefab;

    public Sprite mazeLine;
    public Sprite mazeCurve;
    public Sprite ground;
    public Sprite pellet;
    private Dictionary<string, int> rotations = new Dictionary<string, int>
    {
        { "7", 0 },
        { "F", 90 },
        { "L", 180 },
        { "J", 270 },
        // normal vector direction
        { "l", 0 },
        { "d", 90 },
        { "r", 180 },
        { "u", 270 }
    };

    private int height, width;
    private int[,] tiles;
    private float tileSize;


    private List<Vector2Int> groundTiles;

    void Awake()
    {
        float realTileSize = 40;
        height = 24; // (int)(1080 / realTileSize);
        width = (int)(1920 / realTileSize);

        GenerateMaze(height, width);

        tileSize = 10f / 27f;
        DrawMaze();
    }

    // Start is called before the first frame update
    void Start()
    {
        //
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateMaze(int height, int width)
    {
        tiles = new int[width, height];
        groundTiles = new List<Vector2Int>();

        Stack<Vector2Int> dfs = new Stack<Vector2Int>();
        dfs.Push(new Vector2Int(1, 1));
        tiles[1, 1] = PELLET;

        int[][] directions = new int[][]
        {
            new int[] { 0, 3 },
            new int[] { 3, 0 },
            new int[] { 0, -3 },
            new int[] { -3, 0 }
        };

        while (dfs.Count > 0)
        {
            Vector2Int currTile = dfs.Peek();
            ShuffleArray(directions);

            int newX = 0, newY = 0;
            int[] selectedDirec = null;

            foreach (int[] d in directions)
            {
                newX = currTile.x + d[0];
                newY = currTile.y + d[1];

                if (newX <= 0 || newX >= width - 1 || newY <= 0 || newY >= height - 1 || tiles[newX, newY] == PELLET)
                {
                    continue;
                }

                selectedDirec = d;
                break;
            }

            if (selectedDirec == null)
            {
                dfs.Pop();
                continue;
            }

            Vector2Int newGround = new Vector2Int(newX, newY);
            tiles[newX, newY] = PELLET;
            groundTiles.Add(newGround);

            Vector2Int newGround2, newGround3;
            if (selectedDirec[0] != 0)
            {
                float dist = newX - currTile.x;
                newGround2 = new Vector2Int(Mathf.RoundToInt(currTile.x + dist * 1f / 3f), currTile.y);
                newGround3 = new Vector2Int(Mathf.RoundToInt(currTile.x + dist * 2f / 3f), currTile.y);
            }
            else
            {
                float dist = newY - currTile.y;
                newGround2 = new Vector2Int(currTile.x, Mathf.RoundToInt(currTile.y + dist * 1f / 3f));
                newGround3 = new Vector2Int(currTile.x, Mathf.RoundToInt(currTile.y + dist * 2f / 3f));
            }

            tiles[newGround2.x, newGround2.y] = PELLET;
            tiles[newGround3.x, newGround3.y] = PELLET;
            groundTiles.Add(newGround2);
            groundTiles.Add(newGround3);

            dfs.Push(newGround);
        }

        DeleteWalls(10);
    }

    private void DeleteWalls(int numWalls)
    {
        int count = 0;
        while (count < numWalls)
        {
            int randX = Random.Range(1, width - 1);
            int randY = Random.Range(1, height - 1);
            Vector2Int randPos = new Vector2Int(randX, randY);

            if (tiles[randPos.x, randPos.y] == WALL)
            {
                bool vert = DeleteVerticalWall(randPos), horiz = false;
                if (!vert)
                {
                    horiz = DeleteHorizontalWall(randPos);
                }
                if (!horiz)
                {
                    continue;
                }

                count++;
            }
        }
    }

    private bool DeleteVerticalWall(Vector2Int wallPos)
    {
        // top
        try {
            if (
                (tiles[wallPos.x, wallPos.y + 1] == PELLET || wallPos.y + 1 == height - 1) &&
                tiles[wallPos.x, wallPos.y]     == WALL &&
                tiles[wallPos.x, wallPos.y - 1] == WALL &&
                (tiles[wallPos.x, wallPos.y - 2] == PELLET || wallPos.y - 2 == 0)
                )
            {
                tiles[wallPos.x, wallPos.y] = tiles[wallPos.x, wallPos.y - 1] = PELLET;
                return true;
            }
        }
        catch
        {
            return false;
        }

        // bottom
        try
        {
            if (
                (tiles[wallPos.x, wallPos.y + 2] == PELLET || wallPos.y + 1 == height - 1) &&
                tiles[wallPos.x, wallPos.y + 1] == WALL &&
                tiles[wallPos.x, wallPos.y] == WALL &&
                (tiles[wallPos.x, wallPos.y - 1] == PELLET || wallPos.y - 1 == 0)
                )
            {
                tiles[wallPos.x, wallPos.y + 1] = tiles[wallPos.x, wallPos.y] = PELLET;
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    private bool DeleteHorizontalWall(Vector2Int wallPos)
    {
        // right
        try
        {
            if (
                (tiles[wallPos.x + 1, wallPos.y] == PELLET || wallPos.x + 1 == width - 1) &&
                tiles[wallPos.x, wallPos.y] == WALL &&
                tiles[wallPos.x - 1, wallPos.y] == WALL &&
                (tiles[wallPos.x - 2, wallPos.y] == PELLET || wallPos.x - 2 == 0)
                )
            {
                tiles[wallPos.x, wallPos.y] = tiles[wallPos.x - 1, wallPos.y] = PELLET;
                return true;
            }
        }
        catch
        {
            return false;
        }

        // left
        try
        {
            if (
                (tiles[wallPos.x + 2, wallPos.y] == PELLET || wallPos.x + 2 == width - 1) &&
                tiles[wallPos.x + 1, wallPos.y] == WALL &&
                tiles[wallPos.x, wallPos.y] == WALL &&
                (tiles[wallPos.x - 1, wallPos.y] == PELLET || wallPos.x - 1 == 0)
                )
            {
                tiles[wallPos.x + 1, wallPos.y] = tiles[wallPos.x, wallPos.y] = PELLET;
                return true;
            }
        }
        catch
        {
            return false;
        }

        return false;
    }

    public void DrawMaze()
    {
        CreateTile(0, height - 1, WALL, mazeCurve, "F");
        CreateTile(width - 1, height - 1, WALL, mazeCurve, "7");
        CreateTile(0, 0, WALL, mazeCurve, "L");
        CreateTile(width - 1, 0, WALL, mazeCurve, "J");

        for (int x = 1; x < width - 1; x++)
        {
            CreateTile(x, height - 1, WALL, mazeLine, "d");
            CreateTile(x, 0, WALL, mazeLine, "u");
        }
        for (int y = 1; y < height - 1; y++)
        {
            CreateTile(0, y, WALL, mazeLine, "l");
            CreateTile(width - 1, y, WALL, mazeLine, "r");
        }

        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (tiles[x, y] == PELLET)
                {
                    CreateTile(x, y, PELLET, pellet, "u");
                    continue;
                }

                Sprite sp = pellet;
                string orientation = "u";

                bool left = tiles[x - 1, y] == WALL, right = tiles[x + 1, y] == WALL,
                     up = tiles[x, y + 1] == WALL, down = tiles[x, y - 1] == WALL;

                // horizontal
                if (left && right && (!up ^ !down))
                {
                    orientation = !up ? "u" : "d";
                    sp = mazeLine;
                }

                // vertical
                if (up && down && (!left ^ !right))
                {
                    orientation = !left ? "l" : "r";
                    sp = mazeLine;
                }

                // inner radius 7 F L J
                if (left && down && tiles[x - 1, y - 1] == PELLET)
                {
                    orientation = "7";
                    sp = mazeCurve;
                }
                if (right && down && tiles[x + 1, y - 1] == PELLET)
                {
                    orientation = "F";
                    sp = mazeCurve;
                }
                if (right && up && tiles[x + 1, y + 1] == PELLET)
                {
                    orientation = "L";
                    sp = mazeCurve;
                }
                if (left && up && tiles[x - 1, y + 1] == PELLET)
                {
                    orientation = "J";
                    sp = mazeCurve;
                }

                // outer radius 7 F L J
                if (!up && down && left && !right)
                {
                    orientation = "7";
                    sp = mazeCurve;
                }
                if (!up && down && !left && right)
                {
                    orientation = "F";
                    sp = mazeCurve;
                }
                if (up && !down && !left && right)
                {
                    orientation = "L";
                    sp = mazeCurve;
                }
                if (up && !down && left && !right)
                {
                    orientation = "J";
                    sp = mazeCurve;
                }

                CreateTile(x, y, WALL, sp, orientation);
            }
        }
    }

    private GameObject CreateTile(int x, int y, int tileType, Sprite sprite, string orientation)
    {
        Vector3 pos = new Vector3(LEFT + x * tileSize, BOTTOM + y * tileSize, 0);
        GameObject mazeTile = Instantiate(tilePrefab, pos, Quaternion.identity);
        mazeTile.name = $"Tile_{x}_{y}";

        SpriteRenderer sr = mazeTile.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        mazeTile.transform.rotation = Quaternion.Euler(
            mazeTile.transform.rotation.eulerAngles.x,
            mazeTile.transform.rotation.eulerAngles.y,
            rotations[orientation]
        );

        sr.sortingLayerName = "Maze";
        sr.sortingOrder = 1;

        mazeTile.GetComponent<Tile>().Initialize(tileType);

        return mazeTile;
    }

    //
    // HELPER METHODS
    //
    public static void ShuffleArray(int[][] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int[] temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }

    //
    // GETTER
    //
    public int[,] GetTiles()
    {
        return tiles;
    }

    public List<Vector2Int> GetGroundTiles()
    {
        return groundTiles;
    }

    public float GetTileSize()
    {
        return tileSize;
    }

    public int[] GetDimensions()
    {
        return new int[] { height, width };
    }
}
