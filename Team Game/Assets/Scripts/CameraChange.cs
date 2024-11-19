using System.Collections;
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
}