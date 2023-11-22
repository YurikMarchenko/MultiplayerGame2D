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

    public void Start()
    {
        if (AudioListener.volume == 1)
        {
            buttonAudio.GetComponent<Image>().sprite = audioOn;
        }
        else
        {
            buttonAudio.GetComponent<Image>().sprite = audioOff;
        }
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
    }
    public void PlaySound()
    {
        audioSource.PlayOneShot(clip);
    }
}
