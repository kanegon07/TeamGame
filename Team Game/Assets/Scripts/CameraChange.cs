using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraChange : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject subCamera;
    public CharacterController characterController; // プレイヤーのCharacterController
    private InputAction _move; // InputSystemのmoveアクション

    [SerializeField] private Vector3 mainCameraPosition = new Vector3(0, 0.8f, 0.3f);
    [SerializeField] private Vector3 subCameraPosition = new Vector3(0, 0.3f, 0.3f);
    private bool isTransitioning = false; // カメラの切り替え中かどうか
    private bool isMovingFlag = false; // 移動しているかのフラグ
    private bool isJumpingFlag = false; // ジャンプしているかのフラグ
    private float transitionSpeed = 5.0f; // カメラ切り替え速度

    void Start()
    {
        // 初期状態でmainCameraがアクティブ、subCameraは非アクティブにする
        SetInitialCameraState();

        // PlayerInputの取得
        var input = GetComponent<PlayerInput>();
        var actionMap = input.currentActionMap;

        // Moveアクションの設定
        _move = actionMap.FindAction("Move");

        // mainCameraとsubCameraの位置を取得
        mainCameraPosition = mainCamera.transform.localPosition;
        subCameraPosition = subCamera.transform.localPosition;
    }

    void Update()
    {
        // 移動キーが押されているかどうかをチェック
        var moveValue = _move.ReadValue<Vector2>();
        isMovingFlag = moveValue.x != 0 || moveValue.y != 0; // 移動入力があれば移動フラグをtrue

        // ジャンプ中かどうかをフラグで管理
        isJumpingFlag = !characterController.isGrounded;

        // ジャンプしていない場合のみ移動フラグをチェックしてカメラを切り替え
        if (!isJumpingFlag)
        {
            if (isMovingFlag && !isTransitioning && mainCamera.activeSelf)
            {
                // メインカメラからサブカメラへ切り替え
                StartCoroutine(SwitchToCamera(subCamera, subCameraPosition));
            }
            else if (!isMovingFlag && !isTransitioning && subCamera.activeSelf)
            {
                // サブカメラからメインカメラへ切り替え
                StartCoroutine(SwitchToCamera(mainCamera, mainCameraPosition));
            }
        }
        else
        {
            // ジャンプ中は常にメインカメラを維持
            if (!mainCamera.activeSelf)
            {
                StartCoroutine(SwitchToCamera(mainCamera, mainCameraPosition));
            }
        }
    }

    private void SetInitialCameraState()
    {
        // メインカメラを初期位置に設定してアクティブにする
        mainCamera.transform.localPosition = mainCameraPosition;
        subCamera.transform.localPosition = subCameraPosition;
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
    }

    private IEnumerator SwitchToCamera(GameObject targetCamera, Vector3 targetPosition)
    {
        if (isTransitioning) yield break; // 既にカメラ切り替え中なら何もしない

        isTransitioning = true;
        GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

        // 現在のカメラの位置を取得してスムーズに補間
        Vector3 startPosition = activeCamera.transform.localPosition;
        float t = 0.0f;

        // 現在位置と目標位置が同じでないときにのみ補間を実行
        while (t < 1.0f)
        {
            t += Time.deltaTime * transitionSpeed;
            activeCamera.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        // 最終的に位置と状態を正確に設定
        activeCamera.SetActive(false);
        targetCamera.transform.localPosition = targetPosition;
        targetCamera.SetActive(true);

        isTransitioning = false;
    }
}