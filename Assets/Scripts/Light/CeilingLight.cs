using System.Collections;
using UnityEngine;

public class CeilingLight : MonoBehaviour
{
    private Animator animator;
    private AudioSource audio;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        StartCoroutine(RandomLightFlicker());
    }

    IEnumerator RandomLightFlicker()
    {
        while (true)
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            yield return new WaitForSeconds(Random.Range(40f, 60f));
        }
    }

    public void PlayAudioSource()
    {
        audio.Play();
    }
    
}
