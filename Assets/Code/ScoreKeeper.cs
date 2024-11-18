using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public static float TOP = 130f / 27f, BOTTOM = -130f / 27f, LEFT = -235f / 27f, RIGHT = 235f / 27f;
    private float tileSize;

    public GameObject digitPrefab;
    public Sprite[] digitSprites;

    public int score = 0;

    private AudioPlayer audioSource;
    private bool playingAudio = false;

    public AudioClip scoreSound;

    private float prevTime = 0;
    private float soundLength;

    public GameObject textPrefab;
    public Sprite youWinSprite;

    // Start is called before the first frame update
    void Start()
    {
        tileSize = FindObjectOfType<MazeGeneration>().GetTileSize();
        audioSource = FindObjectOfType<AudioPlayer>();
        RenderScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (playingAudio && Time.time - prevTime >= soundLength)
        {
            audioSource.Loop();
            playingAudio = false;
        }
    }

    public void AddScore(int points)
    {
        score += points;

        if (score % 100 == 0)
        {
            soundLength = audioSource.PlayAudio(scoreSound);
            playingAudio = true;
            prevTime = Time.time;
        }

        if (score == 300)
        {
            Debug.Log("here");
            Vector3 pos = new Vector3(0, TOP, 0);
            GameObject youWin = Instantiate(textPrefab, pos, Quaternion.identity);
            youWin.GetComponent<SpriteRenderer>().sprite = youWinSprite;
        }

        RenderScore();
    }

    public void RenderScore()
    {
        Digit[] digits = FindObjectsOfType<Digit>();
        for (int i = digits.Length - 1; i >= 0; i--)
        {
            digits[i].Destroy();
        }

        Vector3 startPos = new Vector3(LEFT + tileSize, TOP - tileSize, 0);
        Vector3 offset = new Vector3(tileSize, 0, 0);

        string num = score.ToString();
        Debug.Log(num);
        for (int i = 0; i < num.Length; i++)
        {
            int digit = num[i] - '0';
            Debug.Log(digit);

            GameObject digitObj = Instantiate(digitPrefab, startPos + offset * i, Quaternion.identity);
            SpriteRenderer sr = digitObj.GetComponent<SpriteRenderer>();

            sr.sprite = digitSprites[digit];
            sr.sortingLayerName = "Score";
            sr.sortingOrder = 4;
        }
    }
}
