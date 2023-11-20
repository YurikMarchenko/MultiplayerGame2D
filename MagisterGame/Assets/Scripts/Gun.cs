using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviourPun
{
    public float offset;
    public float startTimeBtwShots;
    private float timeBtwShots;

    public GameObject bulletPrefab;
    public Transform shotPoint;

    void Start()
    {
        // Приховати та заблокувати курсор
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            HandleShooting();
        }
    }

    void HandleShooting()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if(timeBtwShots <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                photonView.RPC("ShootBullet", RpcTarget.AllViaServer, shotPoint.position, transform.rotation);
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
            timeBtwShots = startTimeBtwShots;
        }
        
    }

    [PunRPC]
    void ShootBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        PhotonView bulletPhotonView = bullet.GetComponent<PhotonView>();

        if (bulletPhotonView != null)
        {
            bulletPhotonView.TransferOwnership(photonView.ViewID);
        }
    }
}
