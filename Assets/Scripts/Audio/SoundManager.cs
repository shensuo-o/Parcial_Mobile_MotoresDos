using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioClip pays;
    public AudioClip colletPayment;
    public AudioClip enter;
    public AudioClip foodReady;
    public AudioClip askFood;
    public AudioClip win;
    public AudioClip lose;

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
