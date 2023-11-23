using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    private float X, Y;
    public float speed;
    public int maxHealth;
    public int currentHealth;
    private float posX;

    Animator anim;
    PhotonView view;
    public Image healthBar;

    public Text textName;
    private Rigidbody2D rb;
    public RectTransform canvas;
    public AudioSource getHitSound;

    private Canvas gameCanvas;
    private Canvas deathCanvas;

    void Start()
    {
        posX = transform.position.x;
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();

        gameCanvas = GameObject.FindGameObjectWithTag("gameCanvas").GetComponent<Canvas>();
        deathCanvas = GameObject.FindGameObjectWithTag("deathCanvas").GetComponent<Canvas>();

        // �����
        getHitSound = GetComponent<AudioSource>();

        textName.text = view.Owner.NickName;
        
        if (view.IsMine)
        {
            deathCanvas.gameObject.SetActive(false);
        }

        if (view.Owner.IsLocal)
        {
            Camera.main.GetComponent<CameraFollow>().player = gameObject.transform;
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (view.IsMine)
        {
            healthBar.fillAmount = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0 && view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            gameCanvas.gameObject.SetActive(false);
            deathCanvas.gameObject.SetActive(true);
        }
        else
        {
            X = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
            Y = Input.GetAxisRaw("Vertical") * speed * Time.deltaTime;

            if (view.IsMine)
            {
                Vector3 input = new Vector3(X, Y, 0);
                transform.position += input.normalized * speed * Time.deltaTime;

                // ����������, � ��� ������� �������� �������
                if (input.x > 0)
                {
                    // ������� �������� ������, ������ ��������� � ���������� ����
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (input.x < 0)
                {
                    // ������� �������� ����, ������ ��������� � ����� ��
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
            else
            {
                var newposX = transform.position.x;
                var delX = newposX - posX;
                posX = newposX;

                if (delX > 0)
                {
                    // ������� �������� ������, ������ ��������� � ���������� ����
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (delX < 0)
                {
                    // ������� �������� ����, ������ ��������� � ����� ��
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
        canvas.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            getHitSound.Play();
            photonView.RPC("UpdateHealthAfterDamage", RpcTarget.AllBuffered, damage);
        }
    }

    public void GiveHeal(int healCount)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateHealthAfterHeal", RpcTarget.AllBuffered, healCount);
        }
    }

    [PunRPC]
    void UpdateHealthAfterDamage(int damage)
    {
        currentHealth -= damage;
    }

    [PunRPC]
    void UpdateHealthAfterHeal(int healCount)
    {
        if (currentHealth <= 75)
        {
            currentHealth += healCount;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ³������� ����� ��� ������'� ����� ������
            stream.SendNext(currentHealth);
        }
        else
        {
            // ��������� ����� ��� ������'� ����� ������
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}
