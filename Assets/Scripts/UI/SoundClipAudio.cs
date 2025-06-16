using UnityEngine;

public class SoundClipAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioStart;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound() 
    {
        GetComponent<AudioSource>().PlayOneShot(_audioStart);
    }
}
