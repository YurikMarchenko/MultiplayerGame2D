using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.Windows;

public class Player : MonoBehaviourPun, IPunObservable
{
    private float X, Y;
    public float speed;
    public int maxHealth;
    private int currentHealth; 
    private float posX;

    Animator anim;
    PhotonView view;
    public Image healthBar;

    public Text textName;
    public Joystick joystick;
    private Rigidbody2D rb;
    public RectTransform canvas;
    public AudioSource getHitSound;


    void Start()
    {
        posX = transform.position.x;
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();

        //звуки
        getHitSound = GetComponent<AudioSource>();

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
                Vector3 input = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal") + X, UnityEngine.Input.GetAxisRaw("Vertical") + Y, 0);
                transform.position += input.normalized * speed * Time.deltaTime;
                // Перевіряємо, в яку сторону рухається гравець
                if (input.x > 0)
                {
                    // Гравець рухається вправо, спрайт повертаємо в початковий стан
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    
                }
                else if (input.x < 0)
                {
                    // Гравець рухається вліво, спрайт повертаємо в інший бік
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
                    // Гравець рухається вправо, спрайт повертаємо в початковий стан
                    transform.rotation = Quaternion.Euler(0, 0, 0);

                }
                else if (delX < 0)
                {
                    // Гравець рухається вліво, спрайт повертаємо в інший бік
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