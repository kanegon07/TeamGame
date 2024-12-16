using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHit : MonoBehaviour
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



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("WallHit"))
        {
            player.WallHitFlg = true;
        }
    }


}
