using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Player : MonoBehaviourPun, IPunObservable
{
    private float X, Y;
    public float speed;
    public int maxHealth;
    private int currentHealth;

    Animator anim;
    PhotonView view;
    public Image healthBar;

    public Text textName;
    public Joystick joystick;
    private Rigidbody2D rb;

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();

        textName.text = view.Owner.NickName;
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
        }
        else
        {
            X = joystick.Horizontal * speed * Time.deltaTime;
            Y = joystick.Vertical * speed * Time.deltaTime;

            if (view.IsMine)
            {
                Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal") + X, Input.GetAxisRaw("Vertical") + Y, 0);
                transform.position += input.normalized * speed * Time.deltaTime;
            }
        }
    }

    public void Flip()
    {
        textName.transform.Rotate(0f, 180f, 0f);
    }

    public void TakeDamage(int damage)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("UpdateHealth", RpcTarget.AllBuffered, damage);
        }
    }

    [PunRPC]
    void UpdateHealth(int damage)
    {
        currentHealth -= damage;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Відправка даних про здоров'я через мережу
            stream.SendNext(currentHealth);
        }
        else
        {
            // Отримання даних про здоров'я через мережу
            currentHealth = (int)stream.ReceiveNext();
        }
    }
}