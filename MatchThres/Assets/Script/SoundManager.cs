using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    AudioSource clip;
    public AudioClip continueSound;
    public AudioClip StartSound;
    // Start is called before the first frame update
    void Start()
    {
        clip = GetComponent<AudioSource>();
    }

    public void makeSound()
    {
        clip.PlayOneShot(StartSound, 1.5f);
    }

    public void ContinueSound()
    {
        clip.PlayOneShot(continueSound, 1.5f);
    }    
}
