using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;
    private bool playingAudio = false;

    public AudioClip eatingSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Loop();
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    public float PlayAudio(AudioClip audioClip)
    {
        audioSource.Stop();
        audioSource.loop = false;

        audioSource.clip = audioClip;
        audioSource.volume = 1f;
        float soundLength = audioClip.length;
        audioSource.Play();

        return soundLength;
    }

    public void Loop()
    {
        audioSource.clip = eatingSound;
        audioSource.volume = 0.75f;
        audioSource.Play();
        audioSource.loop = true;
    }
}
