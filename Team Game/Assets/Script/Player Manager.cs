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
        //�����Rigidbody���g����悤�ɂȂ�B
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   //Rigidbody���g����Update�����͂����ɏ���
        
        //�O�ɐi��
        //if(Input.GetKey(KeyCode.W))
        //{
        //    rb.AddForce(transform.forward * moveSpeed);
        //}


        ////���ɐi��
        //if (Input.GetKey(KeyCode.S))
        //{
        //    rb.AddForce(-transform.forward * moveSpeed);
        //}

        ////�E�ɐi��
        //if (Input.GetKey(KeyCode.D))
        //{
        //    rb.AddForce(transform.right * moveSpeed);
        //}

    }
}
