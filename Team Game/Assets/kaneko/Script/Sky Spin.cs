using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySpin : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // transform���擾
        Transform myTransform = this.transform;

        // ���[���h���W��ŁA���݂̉�]�ʂ։��Z����
        myTransform.Rotate(0.0f, 0.01f, 0.0f, Space.World);
    }
}
