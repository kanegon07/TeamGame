using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mushroom : MonoBehaviour
{

    CameraPlayer player;
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


    //“–‚½‚Á‚½‚Æ‚«
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("“–‚½‚Á‚Ä‚é‚æ");
            player.UpPlayer(10.0f,15);
            
        }

       
    }
}
