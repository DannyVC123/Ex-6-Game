using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lives : MonoBehaviour
{
    public GameObject[] lives;
    private int numLives = 3;

    private Spawner spawner;

    private AudioPlayer audioSource;
    private bool playingAudio = false;

    public AudioClip deathSound;

    private float prevTime = 0;
    private float soundLength;

    public GameObject textPrefab;
    float tileSize;

    // Start is called before the first frame update
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        audioSource = FindObjectOfType<AudioPlayer>();

        MazeGeneration mg = FindObjectOfType<MazeGeneration>();
        tileSize = mg.GetTileSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (playingAudio && Time.time - prevTime >= soundLength)
        {
            audioSource.Loop();
            playingAudio = false;

            if (numLives > 0)
            {
                spawner.Reset();
            }
        }
    }

    public void LoseLife()
    {
        numLives--;
        Destroy(lives[numLives]);
        PlayDeathSound();

        if (numLives == 0)
        {
            Vector3 pos = new Vector3(0, -tileSize, 0);
            GameObject gameOver = Instantiate(textPrefab, pos, Quaternion.identity);

            audioSource.enabled = false;

            MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }

            // Additional actions like displaying Game Over UI can go here
            Debug.Log("Game Over! All scripts are disabled.");
        }
        else
        {
            Pacman pacman = FindObjectOfType<Pacman>();
            pacman.enabled = false;

            Blinky blinky = FindObjectOfType<Blinky>();
            blinky.enabled = false;

            Pinky pinky = FindObjectOfType<Pinky>();
            pinky.enabled = false;

            Inky inky = FindObjectOfType<Inky>();
            inky.enabled = false;

            Clyde clyde = FindObjectOfType<Clyde>();
            clyde.enabled = false;
        }
    }

    private void PlayDeathSound()
    {
        soundLength = audioSource.PlayAudio(deathSound);
        playingAudio = true;
        prevTime = Time.time;
    }
}
