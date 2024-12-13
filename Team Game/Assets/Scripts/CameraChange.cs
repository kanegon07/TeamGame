using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private GameObject subCamera;
    [SerializeField] private CharacterController characterController; // プレイヤーのCharacterController
    [SerializeField] private CameraPlayer player; // CameraPlayerスクリプトへの参照

    private InputAction _move; // InputSystemのmoveアクション

    // カメラの位置（ローカル座標）
    [SerializeField] private Vector3 mainCameraPosition = new (0, 0.8f, 0.3f);
    [SerializeField] private Vector3 subCameraPosition = new (0, 0.3f, 0.3f);

    private bool isTransitioning = false; // カメラの切り替え中かどうか
    private bool isMoving = false; // プレイヤーが移動しているかどうか
    private bool isJumping = false; // プレイヤーがジャンプしているかどうか
    private const float cameraTransitionSpeed = 5.0f; // カメラ切り替え速度

    private void Start()
    {
        // カメラの初期化
        SetInitialCameraState();

        // PlayerInputの取得
        var input = GetComponent<PlayerInput>();
        _move = input.currentActionMap.FindAction("Move");
    }

    private void Update()
    {
        UpdateMovementState(); // プレイヤーの移動状態を更新
        HandleCameraSwitch(); // プレイヤーの状態に応じてカメラを切り替える
    }

    /// <summary>
    /// カメラの初期状態を設定するメソッド。
    /// メインカメラをアクティブにして、位置を設定する。
    /// </summary>
    private void SetInitialCameraState()
    {
        // メインカメラを初期位置に設定してアクティブにする
        mainCamera.transform.localPosition = mainCameraPosition;
        subCamera.transform.localPosition = subCameraPosition;
        mainCamera.SetActive(true);
        subCamera.SetActive(false);
    }

    /// <summary>
    /// プレイヤーの移動状態（移動中かどうか、ジャンプ中かどうか）を更新する。
    /// </summary>
    private void UpdateMovementState()
    {
        var moveInput = _move.ReadValue<Vector2>(); // プレイヤーの移動入力を取得
        isMoving = moveInput.x != 0 || moveInput.y != 0; // 入力がある場合は移動中とみなす
        isJumping = !characterController.isGrounded; // 地面に接していない場合はジャンプ中
    }

    /// <summary>
    /// プレイヤーの状態に基づいてカメラを切り替える。
    /// </summary>
    private void HandleCameraSwitch()
    {
        if (player.FlyFlg) // プレイヤーが滑空している場合
        {
            // 滑空中はサブカメラに切り替え
            if (!subCamera.activeSelf) StartCoroutine(SwitchCameraSmoothly(subCamera, subCameraPosition));
        }
        else if (isJumping) // ジャンプ中は常にメインカメラ
        {
            if (!mainCamera.activeSelf) StartCoroutine(SwitchCameraSmoothly(mainCamera, mainCameraPosition));
        }
        else if (!isMoving && !isTransitioning && subCamera.activeSelf)
        {
            // 停止中はメインカメラに戻す
            StartCoroutine(SwitchCameraSmoothly(mainCamera, mainCameraPosition));
        }
    }

    /// <summary>
    /// カメラをスムーズに切り替えるコルーチン。
    /// </summary>
    private IEnumerator SwitchCameraSmoothly(GameObject targetCamera, Vector3 targetPosition)
    {
        if (isTransitioning) yield break; // 切り替え中ならスキップ

        isTransitioning = true;
        GameObject activeCamera = mainCamera.activeSelf ? mainCamera : subCamera;

        // 現在のカメラ位置を取得
        Vector3 startPosition = activeCamera.transform.localPosition;
        float t = 0.0f;

        // 補間を使用してカメラ位置をスムーズに移動
        while (t < 1.0f)
        {
            t += Time.deltaTime * cameraTransitionSpeed; // 時間経過に応じて補間値を増加
            activeCamera.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, t); // 線形補間で位置を移動
            yield return null; // 次のフレームまで待機
        }

        // 最終的な位置とアクティブ状態を設定
        activeCamera.SetActive(false);
        targetCamera.transform.localPosition = targetPosition;
        targetCamera.SetActive(true);

        isTransitioning = false; // 切り替え完了
    }
}