using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiObj : MonoBehaviour
{

    CameraPlayer player;//Ç±Ç±Ç≈ÇÃÉvÉåÉCÉÑÅ[
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



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            player.JumpingFlg = false;

        }

        if (collision.gameObject.CompareTag("Spring")|| collision.gameObject.CompareTag("Ground"))
        {
            player.FlyFlg = false;
        }
    }

}
