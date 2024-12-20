using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushRoomAnimation : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            //Bool�^�̃p�����[�^�[�ł���blRot��True�ɂ���
            anim.SetBool("blRot", true);
        }

        if (Input.GetKey(KeyCode.R))
        {
            //Bool�^�̃p�����[�^�[�ł���blRot��True�ɂ���
            anim.SetBool("blRot", false);
        }
    }
}
