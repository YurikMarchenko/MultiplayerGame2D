using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sprite audioOn;
    public Sprite audioOff;
    public GameObject buttonAudio;

    public AudioClip clip;
    public AudioSource audioSource;

    private void Start()
    {
        LoadAudioState(); // ƒобавлено дл€ загрузки состо€ни€ звука при запуске
    }

    private void LoadAudioState()
    {
        // «агружаем значение звука из PlayerPrefs, если оно существует
        float savedVolume = PlayerPrefs.GetFloat("AudioVolume", 1f);
        AudioListener.volume = savedVolume;

        // ”станавливаем соответствующий спрайт кнопки в зависимости от значени€ звука
        if (AudioListener.volume == 1)
        {
            buttonAudio.GetComponent<Image>().sprite = audioOn;
        }
        else
        {
            buttonAudio.GetComponent<Image>().sprite = audioOff;
        }
    }

    private void SaveAudioState()
    {
        // —охран€ем текущее значение звука в PlayerPrefs
        PlayerPrefs.SetFloat("AudioVolume", AudioListener.volume);
        PlayerPrefs.Save(); // —охран€ем изменени€
    }

    public void OnOffAudio()
    {
        if (AudioListener.volume == 1)
        {
            AudioListener.volume = 0;
            buttonAudio.GetComponent<Image>().sprite = audioOff;
        }
        else
        {
            AudioListener.volume = 1;
            buttonAudio.GetComponent<Image>().sprite = audioOn;
        }

        SaveAudioState(); // —охран€ем состо€ние звука после его изменени€
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(clip);
    }
}
