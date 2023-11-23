using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Heal : MonoBehaviour
{
    public int countHeal;
    public LayerMask whatIsSolid;
    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player"))
            {
                hitInfo.collider.GetComponent<Player>().GiveHeal(countHeal);
                Destroy(gameObject);
            }
            if (hitInfo.collider.CompareTag("Bullet"))
            {
                Destroy(gameObject);
            }
        }
    }
}
