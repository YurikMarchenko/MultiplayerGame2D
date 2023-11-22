using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviourPun
{
    public float offset;
    private float X, Y;
    public float startTimeBtwShots;
    private float timeBtwShots;
    private bool shooting = false;

    public GameObject bulletPrefab;
    public Transform shotPoint;
    public AudioSource audioSource;
    public Button shootButton;
    public Joystick joystick;

    private Vector3 lastAimDirection;
    private void Start()
    {
        shootButton = GameObject.FindGameObjectWithTag("ShootButton").GetComponent<Button>();
        shootButton.onClick.AddListener(isShooting);
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();
    }

    void isShooting()
    {
        shooting = true;
    }

    void Update()
    {
        X = joystick.Horizontal * Time.deltaTime;
        Y = joystick.Vertical * Time.deltaTime;

        if (photonView.IsMine)
        {
            HandleShooting();

            // ��������� ��������� ��������� �� Android (�������������� ��� ������ �� Android!!!)
            if (X != 0 || Y != 0)
            {
                // ������������ ��������� ��� ���������� �������� ���� ������
                Vector3 inputDirection = new Vector3(X, Y, 0f).normalized;
                float rotZ = Mathf.Atan2(Y, X) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
                lastAimDirection = inputDirection.normalized;
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, lastAimDirection);
            }
        }
    }

    public void HandleShooting()
    {
        if (timeBtwShots > 0)
        {
            timeBtwShots -= Time.deltaTime;
        }

        Vector3 shootDirection = Vector3.zero;

        //��������� ��������� ��������� �� Android ��� ������� � �������� ���� ������ (�������������� ��� ������ �� Android!!!)
        shootDirection = (X != 0 || Y != 0) ? new Vector3(X, Y, 0f).normalized : lastAimDirection;

        if ((Input.GetKeyDown(KeyCode.Space) || shooting) && timeBtwShots <= 0 && PhotonNetwork.InRoom)
        {
            // ��������� ��������� ��������� ����� ������� Gun
            Vector3 localShotPoint = shotPoint.localPosition;

            // ���������� ��������� ��������� GUN
            Quaternion originalRotation = transform.rotation;

            // ��������� GUN � �������� �������
            float rotZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

            // ��������� ���
            photonView.RPC("ShootBullet", RpcTarget.AllViaServer, transform.TransformPoint(localShotPoint), Quaternion.LookRotation(Vector3.forward, shootDirection.normalized));

            // ���������� GUN � ��������� ���������
            transform.rotation = originalRotation;

            // ������������ �������� ����� ��������� ��������
            timeBtwShots = startTimeBtwShots;
            // �������� ����� shooting
            if (shooting)
            {
                shooting = false;
            }
        }
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
