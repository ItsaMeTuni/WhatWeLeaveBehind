using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSfxController : MonoBehaviour
{
    public AudioSource audioSource;

    public List<AudioClip> clips = new List<AudioClip>();

    [Range(-3, 3)]
    public float pitchMin;
    [Range(-3, 3)]
    public float pitchMax;

    [Range(0, 1)]
    public float volumeMin;
    [Range(0, 1)]
    public float volumeMax;

    public float intervalMin;
    public float intervalMax;

    float nextPlayTime;

    public bool playing = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextPlayTime && playing)
        {
            PlayPiece();
        }
    }


    public void Play()
    {
        playing = true;
    }

    public void Stop()
    {
        playing = false;
    }

    public void PlayPiece()
    {
        Debug.Log("Play piece");
        AudioClip audio = clips[Random.Range(0, clips.Count)];

        //nextPlayTime = Time.time + audio.length + Random.Range(intervalMin, intervalMax);
        nextPlayTime = Time.time + Random.Range(intervalMin, intervalMax);

        audioSource.clip = audio;
        audioSource.volume = Random.Range(volumeMin, volumeMax);
        audioSource.pitch = Random.Range(pitchMin, pitchMax);

        audioSource.Play();
    }
}
