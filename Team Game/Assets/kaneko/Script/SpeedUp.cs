using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


            player.RingSpeedUp(Forward_speed, Y_speed);//ここでどうするかを指定
            Debug.Log("true");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.RingFlg = false;
        }
    }
}
