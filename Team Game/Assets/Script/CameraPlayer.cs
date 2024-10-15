using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(PlayerInput))]

public class CameraPlayer : MonoBehaviour
{

    //[SerializeField] public float jumpPower = 200;

    //�L�����N�^�[�R���g���[���[�̃L���b�V��
    private CharacterController _characterController;
    private InputAction _jump;
    private InputAction _move;
    private Transform _transform;
    private Vector3 _moveVelocity;


    public float moveSpeed;
    public float gravityModifier;//�d��
    public float jumpPower;//�W�����v�̑傫��
    public CharacterController charaCon;

    private Vector3 moveInput;

    public Transform camTrans;
    public float mouseSensitivity;
    public bool invertX;
    public bool invertY;

    //�ǉ�
    private bool canJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;


        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();
        _jump = input.currentActionMap.FindAction("Jump");
        _move = input.currentActionMap.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        //moveInput.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //moveInput.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //--------------------------------------------------------------------------------------------------------------------------------------------

        //Vector3 verMove = transform.forward * Input.GetAxis("Vertical");
        //Vector3 horiMove = transform.right * Input.GetAxis("Horizontal");
        //moveInput = horiMove + verMove;
        //moveInput.Normalize();

        //moveInput = moveInput * moveSpeed;





        //moveInput.y += Physics.gravity.y * gravityModifier * Time.deltaTime;


        //if(charaCon.isGrounded)
        //{
        //    moveInput.y = Physics.gravity.y * gravityModifier * Time.deltaTime;
        //}

        ////�W�����v
        ////�n�ʂɒ����Ă���0.25�o������
        //canJump = Physics.OverlapSphere(groundCheckPoint.position, 0.25f, whatIsGround).Length > 0;

        //if(Input.GetKeyDown(KeyCode.Space)&&canJump)
        //{
        //    moveInput.y = jumpPower;
        //}


        //--------------------------------------------------------------------------------------------------------------------------------------------


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




        //--------------------------------------------------------------------------------------------------------------------------------------------

        //����g���Ă�
        //charaCon.Move(moveInput * Time.deltaTime);


        //-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*

        var moveValue = _move.ReadValue<Vector2>();
        _moveVelocity.x = moveValue.x * moveSpeed;
        _moveVelocity.z = moveValue.y * moveSpeed;


        Vector3 verMove = transform.forward * _moveVelocity.z;
        Vector3 horiMove = transform.right * _moveVelocity.x;
        moveInput = horiMove + verMove;
        moveInput.Normalize();

        




        if (_characterController.isGrounded)
        {
            if(_jump.WasPerformedThisFrame())
            {
                _moveVelocity.y = jumpPower;
            }
        }
        else
        {
            _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }


        moveInput.y = moveInput.y + _moveVelocity.y;
        _characterController.Move(moveInput * Time.deltaTime);
       














       

        //�J�����̉�]����
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
    }
}
