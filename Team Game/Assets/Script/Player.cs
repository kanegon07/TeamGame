using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.CompilerServices;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3;//�ړ����x
    [SerializeField] public float jumpPower = 3;//�W�����v�̋���

    private CharacterController _characterController;//�L���b�V��
    private Transform _transform;//�L���b�V��
    private Vector3 _PlayerMove;//�ړ����x���
    private InputAction _move;//InputAction�� Move �̃L���b�V��
    private InputAction _jump;//InputActin �� Jump �̃L���b�V��

    // Start is called before the first frame update
    void Start()
    {
        //���ׂ������邽�߁B
        _characterController = GetComponent<CharacterController>();
        _transform = transform;

        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();

        _move = input.currentActionMap.FindAction("Move");
        _jump = input.currentActionMap.FindAction("Jump");

    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�����
        var moveValue = _move.ReadValue<Vector2>();
        _PlayerMove.x = moveValue.x * moveSpeed;
        _PlayerMove.z = moveValue.y * moveSpeed;

        //�ړ������Ɍ���
        _transform.LookAt(_transform.position + new Vector3(_PlayerMove.x, 0, _PlayerMove.z));

        if (_characterController.isGrounded)
        {
            if (_jump.WasPressedThisFrame())
            {
                _PlayerMove.y = jumpPower;
            }
        }
        else
        {

            //�d�͂ɂ�����
            _PlayerMove.y += Physics.gravity.y * Time.deltaTime;
        }


            //�I�u�W�F�N�g�𓮂���
            _characterController.Move(_PlayerMove * Time.deltaTime);

    }
}
