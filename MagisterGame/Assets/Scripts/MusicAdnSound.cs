using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sprite audioOn;
    public Sprite audioOff;
    public GameObject buttonAudio;
    public GameObject exitButton;

    public AudioClip clip;
    public AudioSource audioSource;

    private void Start()
    {
        LoadAudioState();
    }

    private void LoadAudioState()
    {

        float savedVolume = PlayerPrefs.GetFloat("AudioVolume", 1f);
        AudioListener.volume = savedVolume;


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

        PlayerPrefs.SetFloat("AudioVolume", AudioListener.volume);
        PlayerPrefs.Save();
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

        SaveAudioState();
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
