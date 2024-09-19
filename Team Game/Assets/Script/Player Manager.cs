using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerManager : MonoBehaviour
{

    private Rigidbody rb;
    private float moveSpeed = 10f;


    // Start is called before the first frame update
    void Start()
    {
        //これでRigidbodyが使えるようになる。
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   //Rigidbodyを使ったUpdate処理はここに書く
        
        //前に進む
        //if(Input.GetKey(KeyCode.W))
        //{
        //    rb.AddForce(transform.forward * moveSpeed);
        //}


        ////後ろに進む
        //if (Input.GetKey(KeyCode.S))
        //{
        //    rb.AddForce(-transform.forward * moveSpeed);
        //}

        ////右に進む
        //if (Input.GetKey(KeyCode.D))
        //{
        //    rb.AddForce(transform.right * moveSpeed);
        //}

    }
}
