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
    private InputAction _look;
    private PlayerInput _input;


    Vector3 cameraForward;
    [SerializeField] private GameObject Camera;
    float inputHorizontal;
    float inputVertical;

    float groundtime;
    bool isgrounded;
    private void Awake()
    {
        _input = this.GetComponent<PlayerInput>();
    }

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
        _look = input.currentActionMap.FindAction("Look");

    }


    private void OnEnable()
    {
        //_input
    }
    // Update is called once per frame
    void Update()
    {
        //�ړ�����
        var moveValue = _move.ReadValue<Vector2>();
        _PlayerMove.x = moveValue.x * moveSpeed;
        _PlayerMove.z = moveValue.y * moveSpeed;



        //�ړ������Ɍ���
        //_transform.LookAt(_transform.position + new Vector3(_PlayerMove.x, 0, _PlayerMove.z));


        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

        cameraForward = Camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        _PlayerMove = cameraForward * inputVertical + Camera.transform.right * inputHorizontal * moveSpeed;

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
