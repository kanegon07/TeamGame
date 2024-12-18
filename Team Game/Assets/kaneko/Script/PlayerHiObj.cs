using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiObj : MonoBehaviour
{

    CameraPlayer player;//ここでのプレイヤー
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



    //当たったときだけ
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            player.JumpingFlg = false;//ジャンプしていない判定
            

        }

        if (collision.gameObject.CompareTag("Spring")|| collision.gameObject.CompareTag("Ground"))
        {
            player.FlyFlg = false;//飛んでるフラグをオフにする
        }
    }


    //当たっている間ずっと
    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            player.PlayerStaminaRec();//スタミナを回復
        }
       

        if (collision.gameObject.CompareTag("Spring") || collision.gameObject.CompareTag("Ground"))
        {
            player.FlyFlg = false;
            player.PlayerStaminaRec();//スタミナを回復(念のためここにも記述している。)
        }
       


        //FallFlgの管理
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
