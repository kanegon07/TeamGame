using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//�X�N���v�g�̎g����
//Unity����Forward_speed�AYspeed�ɒl���Z�b�g����
//Forward_speed���O�ʂɉ�������X�s�[�h
//Yspeed�����̎��ɏ㏸���鍂���B0�ɂ͂��Ȃ����ƁB

public class SpeedUp : MonoBehaviour
{

    CameraPlayer player;//�����ł̃v���C���[
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

            player.RingSpeedUp(Forward_speed, Y_speed);//�����łǂ����邩���w��
            
        }
    }

   
}
