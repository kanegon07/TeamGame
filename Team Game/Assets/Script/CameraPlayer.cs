using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
//[RequireComponent (typeof(PlayerInput))]

public class CameraPlayer : MonoBehaviour
{
    //--------------------------------�v���C���[�֘A--------------------------------------------
    private CharacterController _characterController;//�L�����N�^�[�R���g���[���[�̃L���b�V��
    private InputAction _jump;//InputSystem��Jump�̃L���b�V��
    private InputAction _move;//InputSystem��move�̃L���b�V��
    private Transform _transform;//Transorm�̃L���b�V��
    private Vector3 _moveVelocity;//�L�����̈ړ����
    private Vector3 moveInput;//�ŏI�I�ȃL�����̈ړ����


    public float moveSpeed;//�ړ��̑���
    public float jumpPower;//�W�����v�̑傫��
  //public float gravityModifier;//�d�� �������L�����͊����𖳎�����̂Ŏg���ĂȂ��ł��B

   
    //--------------------------------�J�����֘A---------------------------------------------------
    public Transform camTrans;//�J�����͒N�Ȃ̂�
    public float mouseSensitivity;//�J�����̊��x
    public bool invertX;//X�����]����ꍇ�̓`�F�b�N�����
    public bool invertY;//Y�����]����ꍇ�̓`�F�b�N�����

    

    // Start is called before the first frame update
    void Start()
    {
        //-------------------InputSystem�̓�����A�L���b�V��-------------------------------
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
       
        //--------------------------�L�����̈ړ�-------------------------------------------
        var moveValue = _move.ReadValue<Vector2>();
        _moveVelocity.x = moveValue.x * moveSpeed;
        _moveVelocity.z = moveValue.y * moveSpeed;

        Vector3 verMove = transform.forward * _moveVelocity.z;
        Vector3 horiMove = transform.right * _moveVelocity.x;
        moveInput = horiMove + verMove;
        moveInput.Normalize();

        moveInput = moveInput * moveSpeed;



        //-----------------�n�ʂɂ���Ƃ��̓W�����v���ł���----------------------------
        if (_characterController.isGrounded)
        {
            if(_jump.WasPerformedThisFrame())
            {
                _moveVelocity.y = jumpPower;
            }
        }
        else
        {
            //�d��
            _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }


        moveInput.y = moveInput.y + _moveVelocity.y;//moveInput��Y���̏���ǉ�����
        _characterController.Move(moveInput * Time.deltaTime);//�����ōŏI�I�ȃL�����̈ړ�����n��
       



    //-------------------------------------�J�����֘A-----------------------------------------

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
