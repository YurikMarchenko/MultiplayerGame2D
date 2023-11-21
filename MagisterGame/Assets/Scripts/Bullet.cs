using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifetime;
    public float distance;
    public int damage;
    public LayerMask whatIsSolid;

    private void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, distance, whatIsSolid);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.CompareTag("Player") || hitInfo.collider.CompareTag("Bullet"))
            {
                hitInfo.collider.GetComponent<Player>().TakeDamage(damage);
            }
            if (hitInfo.collider.CompareTag("background"))
            {
                Destroy(gameObject);
            }

            Destroy(gameObject);         
        }
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
