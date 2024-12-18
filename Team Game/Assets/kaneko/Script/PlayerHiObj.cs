using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiObj : MonoBehaviour
{

    CameraPlayer player;//�����ł̃v���C���[
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObj = GameObject.Find("Player_0");
        player = playerObj.GetComponent<CameraPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //���������Ƃ�����
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            player.JumpingFlg = false;//�W�����v���Ă��Ȃ�����
            

        }

        if (collision.gameObject.CompareTag("Spring")|| collision.gameObject.CompareTag("Ground"))
        {
            player.FlyFlg = false;//���ł�t���O���I�t�ɂ���
        }
    }


    //�������Ă���Ԃ�����
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            player.PlayerStaminaRec();//�X�^�~�i����
        }
       

        if (collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("Ground"))
        {
            player.FlyFlg = false;
            player.PlayerStaminaRec();//�X�^�~�i����(�O�̂��߂����ɂ��L�q���Ă���B)
        }
       


        //FallFlg�̊Ǘ�
        if (collision.gameObject.CompareTag("Ground")||collision.gameObject.CompareTag("Spring")||player.StickWall==true)
        {
            player.FallFlg = false;
        }
        else
        {
            player.FallFlg = true;
        }
    }
}
