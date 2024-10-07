using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.CompilerServices;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class Player : MonoBehaviour
{
    [SerializeField] public float moveSpeed = 3;//移動速度
    [SerializeField] public float jumpPower = 3;//ジャンプの強さ

    private CharacterController _characterController;//キャッシュ
    private Transform _transform;//キャッシュ
    private Vector3 _PlayerMove;//移動速度情報
    private InputAction _move;//InputActionの Move のキャッシュ
    private InputAction _jump;//InputActin の Jump のキャッシュ
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
        //負荷を下げるため。
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
        //移動処理
        var moveValue = _move.ReadValue<Vector2>();
        _PlayerMove.x = moveValue.x * moveSpeed;
        _PlayerMove.z = moveValue.y * moveSpeed;



        //移動方向に向く
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

            //重力による加速
            _PlayerMove.y += Physics.gravity.y * Time.deltaTime;
        }


        //オブジェクトを動かす
        _characterController.Move(_PlayerMove * Time.deltaTime);

    }
}
