using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gun : MonoBehaviourPun
{
    public float offset;
    public float startTimeBtwShots;
    private float timeBtwShots;

    public GameObject bulletPrefab;
    public Transform shotPoint;
    public AudioSource audioSource;

    void Update()
    {
        if (photonView.IsMine)
        {
            HandleShooting();

            // ���������� ��������� ���� ��� ������ ��������� � �������� ����
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float rotZ = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        }
    }

    void HandleShooting()
    {
        if (timeBtwShots <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Vector3 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector3)Input.mousePosition;

                // ��������� ��������� ��������� ����� ������� Gun
                Vector3 localShotPoint = shotPoint.localPosition;

                // ���������� ��������� ��������� GUN
                Quaternion originalRotation = transform.rotation;

                // ��������� �������� �� ���� ��� ��������
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(inputPosition);
                Vector3 shootDirection = mousePosition - transform.position;

                // ��������� GUN � �������� �������
                float rotZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

                // ��������� ���
                photonView.RPC("ShootBullet", RpcTarget.AllViaServer, transform.TransformPoint(localShotPoint), Quaternion.LookRotation(Vector3.forward, shootDirection.normalized));

                // ���������� GUN � ��������� ���������
                transform.rotation = originalRotation;
            }
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
            timeBtwShots = startTimeBtwShots;
        }
    }


    Vector3 GetShootDirection()
    {
        // ���������� �������� ������� ��� ���� ��� ��������
        Vector3 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector3)Input.mousePosition;

        // ���� ��� ������ �� ������, ���������� �������� �������
        Vector3 difference = inputPosition - new Vector3(Screen.width / 2, Screen.height / 2);

        if (difference.x <= 0)
        {
            // ���� ��� ���� �� ������, ��������� Vector3.zero, ��� �� �������
            return Vector3.zero;
        }

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rotZ + offset) * Vector3.right;
    }

    [PunRPC]
    void ShootBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation);
        PhotonView bulletPhotonView = bullet.GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            audioSource.Play();
        }

        if (bulletPhotonView != null)
        {
            bulletPhotonView.TransferOwnership(photonView.ViewID);
        }
    }
}
