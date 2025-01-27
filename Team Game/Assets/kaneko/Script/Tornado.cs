using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float MoveX;
    public float MoveZ;
    public int speedX;
    public int speedZ;
    private int _speedX;
    private int _speedZ;
    private Vector3 startPos;
    public Vector3 MyPos;
    public float LongX;
    public float LongZ;

    public bool XRight;
    public bool XLeft;
    public bool ZRight;
    public bool ZLeft;
    // Start is called before the first frame update
    void Start()
    {
        _speedX = speedX * -1;
        _speedZ = speedZ * -1;
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        

        transform.Translate(Vector3.right * MoveX * Time.deltaTime);
        transform.Translate(Vector3.forward * MoveZ * Time.deltaTime);

        MyPos = transform.position;

        if(XRight)
        {
            if (MyPos.x - startPos.x >= LongX)
            {

                MoveX = _speedX;
            }
            else if (MyPos.x - startPos.x <= 0)
            {
                MoveX = speedX;
            }
        }

        if(XLeft)
        {
            if (MyPos.x - startPos.x <= -LongX)
            {

                MoveX = speedX;
            }
            else if (MyPos.x - startPos.x >= 0)
            {
                MoveX = _speedX;
            }
        }
       

        if(ZRight)
        {
            if (MyPos.z - startPos.z >= LongZ)
            {

                MoveZ = _speedZ;
            }
            else if (MyPos.z - startPos.z <= 0)
            {
                MoveZ = speedZ;
            }
        }

        if(ZLeft)
        {
            if (MyPos.z - startPos.z <= -LongZ)
            {

                MoveZ = speedZ;
            }
            else if (MyPos.z - startPos.z >= 0)
            {
                MoveZ = _speedZ;
            }
        }
        

    }
}
