using UnityEngine;

public class HealGiver : MonoBehaviour
{
    public AudioSource audioSource;
    private int previousHealth;
    private bool soundPlayed;

    void Start()
    {
        Player player = GetComponentInParent<Player>();

        if (player != null && player.photonView.IsMine)
        {
            previousHealth = player.currentHealth;
            soundPlayed = false;
        }
    }

    void Update()
    {
        Player player = GetComponentInParent<Player>();

        if (player != null && player.photonView.IsMine)
        {
            int currentHealth = player.currentHealth;

            if (currentHealth > previousHealth && !soundPlayed)
            {
                audioSource.Play();
                soundPlayed = true;
            }

            previousHealth = currentHealth;
        }
    }
}
