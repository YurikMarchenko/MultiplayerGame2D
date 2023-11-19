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
        X = joystick.Horizontal * speed * Time.deltaTime;
        Y = joystick.Vertical * speed * Time.deltaTime;

        if (view.IsMine)
        {        
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal") + X, Input.GetAxisRaw("Vertical") + Y, 0);
            transform.position += input.normalized * speed * Time.deltaTime;
        }
    }
}
