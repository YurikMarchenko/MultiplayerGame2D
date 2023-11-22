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

            // Зчитування положення миші для Windows
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float rotZ = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
            lastAimDirection = transform.right;
        }
    }

    public void HandleShooting()
    {
        if (timeBtwShots > 0)
        {
            timeBtwShots -= Time.deltaTime;
        }

        Vector3 shootDirection = Vector3.zero;


        if ((Input.GetKeyDown(KeyCode.Space) || shooting) && timeBtwShots <= 0 && PhotonNetwork.InRoom)
        {
            Vector3 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector3)Input.mousePosition;

            // Отримання локальних координат точки відносно Gun
            Vector3 localShotPoint = shotPoint.localPosition;

            // Збереження поточного обертання GUN
            Quaternion originalRotation = transform.rotation;

            // Отримання напрямку від миші
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(inputPosition);
            shootDirection = mousePosition - transform.position;

            // Обертання GUN в напрямку стрільби
            float rotZ = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

            // Створення кулі
            photonView.RPC("ShootBullet", RpcTarget.AllViaServer, transform.TransformPoint(localShotPoint), Quaternion.LookRotation(Vector3.forward, shootDirection.normalized));

            // Встановлюємо затримку перед наступним пострілом
            timeBtwShots = startTimeBtwShots;
            // Скидаємо флаг shooting
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
