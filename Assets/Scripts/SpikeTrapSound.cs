using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapSound : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void PlaySound()
    {
        audio.Play();
    }
}
