/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraChange : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject subCamera;
    public CharacterController characterController; // �v���C���[��CharacterController
    private InputAction _move; // InputSystem��move�A�N�V����

    [SerializeField] private Vector3 mainCameraPosition = new Vector3(0, 0.8f, 0.3f);
    [SerializeField] private Vector3 subCameraPosition = new Vector3(0, 0.3f, 0.3f);
    private bool isTransitioning = false; // �J�����̐؂�ւ������ǂ���
    private bool isMovingFlag = false; // �ړ����Ă��邩�̃t���O
    private bool isJumpingFlag = false; // �W�����v���Ă��邩�̃t���O
    private float transitionSpeed = 5.0f; // �J�����؂�ւ����x

    void Start()
    {
        // ������Ԃ�mainCamera���A�N�e�B�u�AsubCamera�͔�A�N�e�B�u�ɂ���
        SetInitialCameraState();

        // PlayerInput�̎擾
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;

        // Move�A�N�V�����̐ݒ�
        _move = actionMap.FindAction("Move");

        // mainCamera��subCamera�̈ʒu���擾
        mainCameraPosition = mainCamera.transform.localPosition;
        subCameraPosition = subCamera.transform.localPosition;
    }

    void Update()
    {
        // �ړ��L�[��������Ă��邩�ǂ������`�F�b�N
        var moveValue = _move.ReadValue<Vector2>();
        isMovingFlag = moveValue.x != 0 || moveValue.y != 0; // �ړ����͂�����Έړ��t���O��true

        // �W�����v�����ǂ������t���O�ŊǗ�
        isJumpingFlag = !characterController.isGrounded;

        // �W�����v���Ă��Ȃ��ꍇ�݈̂ړ��t���O���`�F�b�N���ăJ������؂�ւ�
        if (!isJumpingFlag)
        {
            if (isMovingFlag && !isTransitioning && mainCamera.activeSelf)
            {
                // ���C���J��������T�u�J�����֐؂�ւ�
                StartCoroutine(SwitchToCamera(subCamera, subCameraPosition));
            }
            else if (!isMovingFlag && !isTransitioning && subCamera.activeSelf)
            {
                // �T�u�J�������烁�C���J�����֐؂�ւ�
                StartCoroutine(SwitchToCamera(mainCamera, mainCameraPosition));
            }
        }
        else
        {
            // �W�����v���͏�Ƀ��C���J�������ێ�
            if (!mainCamera.activeSelf)
            {
                StartCoroutine(SwitchToCamera(mainCamera, mainCameraPosition));
            }
        }
    }

    private void SetInitialCameraState()
    {
        // ���C���J�����������ʒu�ɐݒ肵�ăA�N�e�B�u�ɂ���
        mainCamera.transform.localPosition = mainCameraPosition;
        subCamera.transform.localPosition = subCameraPosition;
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
    }

    private IEnumerator SwitchToCamera(GameObject targetCamera, Vector3 targetPosition)
    {
        if (isTransitioning) yield break; // ���ɃJ�����؂�ւ����Ȃ牽�����Ȃ�

        isTransitioning = true;
        GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

        // ���݂̃J�����̈ʒu���擾���ăX���[�Y�ɕ��
        Vector3 startPosition = activeCamera.transform.localPosition;
        float t = 0.0f;

        // ���݈ʒu�ƖڕW�ʒu�������łȂ��Ƃ��ɂ̂ݕ�Ԃ����s
        while (t < 1.0f)
        {
            t += Time.deltaTime * transitionSpeed;
            activeCamera.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // �ŏI�I�Ɉʒu�Ə�Ԃ𐳊m�ɐݒ�
        activeCamera.SetActive(false);
        targetCamera.transform.localPosition = targetPosition;
        targetCamera.SetActive(true);

        isTransitioning = false;
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject subCamera;
    [SerializeField] private CharacterController characterController; // �v���C���[��CharacterController
    [SerializeField] private CameraPlayer player; // CameraPlayer�X�N���v�g�ւ̎Q��

    private InputAction _move; // InputSystem��move�A�N�V����

    // �J�����̈ʒu�i���[�J�����W�j
    [SerializeField] private Vector3 mainCameraPosition = new (0, 0.8f, 0.3f);
    [SerializeField] private Vector3 subCameraPosition = new (0, 0.3f, 0.3f);

    private bool isTransitioning = false; // �J�����̐؂�ւ������ǂ���
    private bool isMoving = false; // �v���C���[���ړ����Ă��邩�ǂ���
    private bool isJumping = false; // �v���C���[���W�����v���Ă��邩�ǂ���
    private const float cameraTransitionSpeed = 5.0f; // �J�����؂�ւ����x

    private void Start()
    {
        // �J�����̏�����
        SetInitialCameraState();

        // PlayerInput�̎擾
        var input = GetComponent<PlayerInput>();
        _move = input.currentActionMap.FindAction("Move");
    }

    private void Update()
    {
        UpdateMovementState(); // �v���C���[�̈ړ���Ԃ��X�V
        HandleCameraSwitch(); // �v���C���[�̏�Ԃɉ����ăJ������؂�ւ���
    }

    /// <summary>
    /// �J�����̏�����Ԃ�ݒ肷�郁�\�b�h�B
    /// ���C���J�������A�N�e�B�u�ɂ��āA�ʒu��ݒ肷��B
    /// </summary>
    private void SetInitialCameraState()
    {
        // ���C���J�����������ʒu�ɐݒ肵�ăA�N�e�B�u�ɂ���
        mainCamera.transform.localPosition = mainCameraPosition;
        subCamera.transform.localPosition = subCameraPosition;
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
    }

    /// <summary>
    /// �v���C���[�̈ړ���ԁi�ړ������ǂ����A�W�����v�����ǂ����j���X�V����B
    /// </summary>
    private void UpdateMovementState()
    {
        var moveInput = _move.ReadValue<Vector2>(); // �v���C���[�̈ړ����͂��擾
        isMoving = moveInput.x != 0 || moveInput.y != 0; // ���͂�����ꍇ�͈ړ����Ƃ݂Ȃ�
        isJumping = !characterController.isGrounded; // �n�ʂɐڂ��Ă��Ȃ��ꍇ�̓W�����v��
    }

    /// <summary>
    /// �v���C���[�̏�ԂɊ�Â��ăJ������؂�ւ���B
    /// </summary>
    private void HandleCameraSwitch()
    {
        /*if (isJumping)
        {
            // �W�����v���͏�Ƀ��C���J����
            if (!mainCamera.activeSelf) StartCoroutine(SwitchCameraSmoothly(mainCamera, mainCameraPosition));
        }
        else if (isMoving && !isTransitioning && mainCamera.activeSelf)
        {
            // �ړ����̓T�u�J�����֐؂�ւ�
            StartCoroutine(SwitchCameraSmoothly(subCamera, subCameraPosition));
        }
        else if (!isMoving && !isTransitioning && subCamera.activeSelf)
        {
            // ��~���̓��C���J�����ɖ߂�
            StartCoroutine(SwitchCameraSmoothly(mainCamera, mainCameraPosition));
        }*/

        if (player.FlyFlg) // �v���C���[�����󂵂Ă���ꍇ
        {
            // ���󒆂̓T�u�J�����ɐ؂�ւ�
            if (!subCamera.activeSelf) StartCoroutine(SwitchCameraSmoothly(subCamera, subCameraPosition));
        }
        else if (isJumping) // �W�����v���͏�Ƀ��C���J����
        {
            if (!mainCamera.activeSelf) StartCoroutine(SwitchCameraSmoothly(mainCamera, mainCameraPosition));
        }
        else if (!isMoving && !isTransitioning && subCamera.activeSelf)
        {
            // ��~���̓��C���J�����ɖ߂�
            StartCoroutine(SwitchCameraSmoothly(mainCamera, mainCameraPosition));
        }
    }

    /// <summary>
    /// �J�������X���[�Y�ɐ؂�ւ���R���[�`���B
    /// </summary>
    private IEnumerator SwitchCameraSmoothly(GameObject targetCamera, Vector3 targetPosition)
    {
        if (isTransitioning) yield break; // �؂�ւ����Ȃ�X�L�b�v

        isTransitioning = true;
        GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

        // ���݂̃J�����ʒu���擾
        Vector3 startPosition = activeCamera.transform.localPosition;
        float t = 0.0f;

        // ��Ԃ��g�p���ăJ�����ʒu���X���[�Y�Ɉړ�
        while (t < 1.0f)
        {
            t += Time.deltaTime * cameraTransitionSpeed; // ���Ԍo�߂ɉ����ĕ�Ԓl�𑝉�
            activeCamera.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t); // ���`��Ԃňʒu���ړ�
            yield return null; // ���̃t���[���܂őҋ@
        }

        // �ŏI�I�Ȉʒu�ƃA�N�e�B�u��Ԃ�ݒ�
        activeCamera.SetActive(false);
        targetCamera.transform.localPosition = targetPosition;
        targetCamera.SetActive(true);

        isTransitioning = false; // �؂�ւ�����
    }
}