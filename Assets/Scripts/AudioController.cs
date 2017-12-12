using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public Sprite onStateSprite;
    public Sprite offStateSprite;

    Image image;

    void Awake()
    {
        image = GetComponent<Image>();

        if (PlayerPrefs.GetInt("Mute", 0) == 1)
        {
            AudioListener.volume = 0;
            image.sprite = offStateSprite;
        }
        else
        {
            AudioListener.volume = 1;
            image.sprite = onStateSprite;
        }
    }

    public void ToggleSound()
    {
        if (PlayerPrefs.GetInt("Mute", 0) == 0)
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("Mute", 1);
            image.sprite = offStateSprite;
        }
        else
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("Mute", 0);
            image.sprite = onStateSprite;
        }

        PlayerPrefs.Save();
    }
}
