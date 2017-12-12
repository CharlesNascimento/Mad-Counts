using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip correctClip;

    [SerializeField]
    private AudioClip wrongClip;

    [SerializeField]
    private AudioClip missedClip;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayCorrectSound()
    {
        audioSource.PlayOneShot(correctClip);
    }

    public void PlayWrongSound()
    {
        audioSource.PlayOneShot(wrongClip);
    }

    public void PlayMissedSound()
    {
        audioSource.PlayOneShot(missedClip);
    }
}
