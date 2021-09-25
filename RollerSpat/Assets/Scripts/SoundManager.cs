using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip startSound;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    public void MakeSound()
    {
        playerAudio.PlayOneShot(startSound, 1.0f);
    }
}
