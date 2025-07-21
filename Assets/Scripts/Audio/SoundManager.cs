using UnityEngine;

namespace Audio
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioSource sfxSource;

        public AudioClip pays;
        public AudioClip collectPayment;
        public AudioClip enter;
        public AudioClip foodReady;
        public AudioClip askFood;
        public AudioClip win;
        public AudioClip lose;
        public AudioClip angry;

        public void PlaySfx(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
