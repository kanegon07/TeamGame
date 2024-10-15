using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerInput))]

public class CameraPlayer_Copy : MonoBehaviour
{

   // [SerializeField] private float jumpPower = 500;

    //キャラクターコントローラーのキャッシュ
    private CharacterController _characterController;
    private InputAction _jump;
    private InputAction _move;

    public float moveSpeed;
    public float gravityModifier;//重力
    public float jumpPower;//ジャンプの大きさ
    public CharacterController charaCon;

    private Vector3 moveInput;

    public Transform camTrans;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    //追加
    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();

        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();
        _jump = input.currentActionMap.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Vector3 verMove = transform.forward * Input.GetAxis("Vertical");
        Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");
        moveInput = horiMove + verMove;
        moveInput.Normalize();

        moveInput = moveInput * moveSpeed;

        //追加
        moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;


        if(charaCon.isGrounded)
        {
            moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        }

        //ジャンプ
        //地面に着いていて0.25経ったら
        canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, whatIsGround).Length > 0;

        if(Input.GetKeyDown(KeyCode.Space)&&canJump)
        {
            moveInput.y = jumpPower;
        }

        



        //if(_characterController.isGrounded)
        //{
        //    if(_jump.WasPressedThisFrame())
        //    {
        //        //moveInput.y = jumpPower;
        //    }
        //}
        //else
        //{
        //    moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;
        //}

        //charaCon.Move(moveInput * Time.deltaTime);

        charaCon.Move(moveInput * Time.deltaTime);


        //charaCon.Move(moveInput);

        //カメラの回転制御
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        if(invertX)
        {
            mouseInput.x = -mouseInput.x;
        }

        if(invertY)
        {
            mouseInput.y = -mouseInput.y;
        }


        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //charaCon.Move(moveInput);
    }
}
