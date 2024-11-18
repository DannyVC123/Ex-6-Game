using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public static int WALL = 0, GROUND = 1, PELLET = 2;

    private int tileType;

    private SpriteRenderer sr;
    public Sprite mazeLine;
    public Sprite mazeCurve;
    public Sprite ground;
    public Sprite pellet;

    private ScoreKeeper scoreKeeper;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }

    public void Initialize(int tile)
    {
        tileType = tile;
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tileType != PELLET || collision.GetComponent<Pacman>() == null)
        {
            return;
        }

        sr.sprite = ground;
        tileType = GROUND;

        scoreKeeper.AddScore(1);
    }
}
