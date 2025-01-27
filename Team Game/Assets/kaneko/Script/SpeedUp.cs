using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//スクリプトの使い方
//Unity側のForward_speed、Yspeedに値をセットする
//Forward_speedが前面に加速するスピード
//Yspeedがその時に上昇する高さ。0にはしないこと。

public class SpeedUp : MonoBehaviour
{

    CameraPlayer player;//ここでのプレイヤー
    public float Forward_speed = 0.0f;
    public float Y_speed = 0.0f;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.RingFlg = true;
            player.FlyFlg = false;

            player.RingSpeedUp(Forward_speed, Y_speed);//ここでどうするかを指定
            
        }
    }

   
}
