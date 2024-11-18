using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacman : MonoBehaviour
{
    private Dictionary<string, int> rotations = new Dictionary<string, int>
    {
        { "r", 0 },
        { "u", 90 },
        { "l", 180 },
        { "d", 270 }
    };

    private Dictionary<string, Vector2Int> directions = new Dictionary<string, Vector2Int>
    {
        { "u", new Vector2Int( 0,   1)},
        { "d", new Vector2Int( 0,  -1)},
        { "l", new Vector2Int(-1,   0)},
        { "r", new Vector2Int( 1,   0)}
    };

    public static int WALL = 0, GROUND = 1;
    private int[,] tiles;
    private float tileSize;

    private string currOrientation = "r";
    private Vector2Int currPos;

    private float timeBetweenMoves = 1f / 11f;
    private float prevTime;

    private Lives lives;

    public Sprite[] eatingFrames;
    private int frameInd = 0;
    private SpriteRenderer sr;
    private float prevFrameTime;
    private float timeBetweenFrames = 1f / 10f;

    // Start is called before the first frame update
    void Start()
    {
        MazeGeneration mg = FindObjectOfType<MazeGeneration>();
        tiles = mg.GetTiles();
        tileSize = mg.GetTileSize();

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = eatingFrames[frameInd];

        lives = FindObjectOfType<Lives>();

        prevTime = prevFrameTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public void Initialize(Vector2Int startPos)
    {
        currPos = startPos;
    }

    void FixedUpdate()
    {
        string newOrientation = currOrientation;
        //Vector2Int currPosRounded = new Vector2Int(Mathf.RoundToInt(currPos.x), Mathf.RoundToInt(currPos.y));

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            newOrientation = "u";
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            newOrientation = "d";
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            newOrientation = "l";
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            newOrientation = "r";
        }

        Vector2 direction = directions[newOrientation];
        if (currOrientation != newOrientation)
        {
            Vector2 nextTilePos = currPos + direction;
            if (tiles[(int)nextTilePos.x, (int)nextTilePos.y] != WALL)
            {
                Rotate(newOrientation);
            }
        }

        float currentTime = Time.time;
        if (currentTime - prevTime > timeBetweenMoves)
        {
            Move();
            prevTime = currentTime;
        }

        Animate();
    }

    private void Rotate(string newOrientation)
    {
        transform.rotation = Quaternion.Euler(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            rotations[newOrientation]
        );

        currOrientation = newOrientation;
    }

    private void Move()
    {
        Vector2Int direction = directions[currOrientation];
        Vector2Int nextPos = new Vector2Int(currPos.x + direction.x, currPos.y + direction.y);
        if (tiles[(int)nextPos.x, (int)nextPos.y] == WALL)
        {
            return;
        }

        transform.position = new Vector3(
            transform.position.x + direction.x * tileSize,
            transform.position.y + direction.y * tileSize,
            0
        );

        currPos = nextPos;
    }

    private void Animate()
    {
        float currentTime = Time.time;
        if (currentTime - prevFrameTime >= timeBetweenFrames)
        {
            frameInd = (frameInd + 1) % eatingFrames.Length;
            sr.sprite = eatingFrames[frameInd];
            prevFrameTime = currentTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Blinky>() != null || collision.GetComponent<Pinky>() != null ||
            collision.GetComponent<Inky>() != null || collision.GetComponent<Clyde>() != null)
        {
            lives.LoseLife();
        }
    }

    //
    // GETTER
    //
    public Vector2Int GetCurrPos()
    {
        return currPos;
    }

    public string GetCurrOrientation()
    {
        return currOrientation;
    }
}
