using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Runtime.CompilerServices;
using System.IO.IsolatedStorage;

public class Player : MonoBehaviour
{
    private float X, Y;
    public float speed;
    public int health;
    
    Animator anim;
    PhotonView view;

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
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
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
        health -= damage;
    }
}
