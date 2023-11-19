using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Runtime.CompilerServices;

public class Player : MonoBehaviour
{
    private float X, Y;
    public float speed;
    
    Animator anim;
    PhotonView view;

    public Joystick joystick;
    private Rigidbody2D rb;

    void Start()
    {
        view = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        joystick = GameObject.FindGameObjectWithTag("Joystick").GetComponent<Joystick>();

        if (view.Owner.IsLocal)
        {
            Camera.main.GetComponent<CameraFollow>().player = gameObject.transform;
        }
    }

    void Update()
    {
        X = joystick.Horizontal * speed;
        Y = joystick.Vertical * speed;

        if (view.IsMine)
        {
            //for PC
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
            transform.position += input.normalized * speed * Time.deltaTime;
            //for mobile device
            rb.velocity = new Vector2(X, Y);

            /*if (input == Vector3.zero)
            {
                anim.SetBool("isRunnig", false);
            }
            else
            {
                anim.SetBool("isRunnig", true);
            }*/
        }
    }
}
