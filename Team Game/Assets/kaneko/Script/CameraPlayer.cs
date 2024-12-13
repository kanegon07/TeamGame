//テスト

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]

public class CameraPlayer : MonoBehaviour
{
    //--------------------------------プレイヤー関連--------------------------------------------
    private CharacterController _characterController;//キャラクターコントローラーのキャッシュ
    private InputAction _jump;//InputSystemのJumpのキャッシュ
    private InputAction _move;//InputSystemのmoveのキャッシュ
    private Vector3 _moveVelocity;//キャラの移動情報
    private Vector3 moveInput;//最終的なキャラの移動情報
    private bool isMovingFlg; // 移動フラグ


    public float moveSpeed;//移動の速さ
    public float jumpPower;//ジャンプの大きさ

    private Vector3 _originalCenter; // キャラクターコントローラーの元の center
    private float _originalHeight; // キャラクターコントローラーの元の height
    private CapsuleCollider _capsuleCollider; // キャッシュするカプセルコライダー

    [Header("判定を消す対象")]
    public PlayerHiObj _playerHit;
    public GameObject _playerHead;

    [Header("描画を消す対象")]
    public GameObject playerLeftFoot; // プレイヤーの左足オブジェクト
    public GameObject playerRightFoot; // プレイヤーの右足オブジェクト

    //public float gravityModifier;//重力 ※今回もキャラは慣性を無視するので使ってないです。
    private float BoundPower = 0;
    public bool BoundFlg = false;
    private int BoundTime = 0;

    public Transform MomongaHead;//モモンガの頭

    //---------------------------------新規実装---------------------------------------------------
    public bool JumpingFlg = false;//ジャンプ中かどうか
    public bool FlyFlg = false;//滑空状態にあるかどうか
    private float FlyGravity = 0.0f;
    private int FlyCount = 0;


    //--------------------------------カメラ関連---------------------------------------------------
    //public Transform camTrans;//カメラは誰なのか

    //FPSCamera Camera;//FPSカメラ

    public float mouseSensitivity;//カメラの感度
    public bool invertX;//X軸反転する場合はチェックをつける
    public bool invertY;//Y軸反転する場合はチェックをつける*/

    //--------------------------------パーティクル関連---------------------------------------------
    private ParticleManager _particleManager;

    //キノコのジャンプ
    public void UpPlayer(float y,int time)
    {
        BoundTime = time;
        if (BoundFlg == false)
        {
            BoundFlg = true;
            BoundPower = y;
            _moveVelocity.y = BoundPower;
            JumpingFlg = true;
        }

        //moveInput.y += y;
        //_moveVelocity.z += y;


    }
    // Start is called before the first frame update
    void Start()
    {
        //-------------------InputSystemの導入や、キャッシュ-------------------------------
        _characterController = GetComponent<CharacterController>();
        _capsuleCollider = GetComponent<CapsuleCollider>(); // カプセルコライダーを取得

        // CharacterController の元の設定を保存
        _originalCenter = _characterController.center;
        _originalHeight = _characterController.height;

        var input = GetComponent<PlayerInput>();
        input.currentActionMap.Enable();
        _jump = input.currentActionMap.FindAction("Jump");
        _move = input.currentActionMap.FindAction("Move");
        _particleManager = GetComponent<ParticleManager>(); // ParticleManagerをキャッシュ
    }

    // Update is called once per frame
    void Update()
    {
        //頭は常に非表示
        HideObjectRenderer(_playerHead);

        //Debug.Log(BoundTime);

        if (BoundFlg==true)
        {
            BoundTime--;
            if (BoundTime <= 0)
            {
                //Debug.Log("ストップ！");
                BoundTime = 0;
                BoundPower = 0.0f;
                BoundFlg = false;
            }
        }

        if (BoundFlg==false)
        {
            BoundPower = 0.0f;
            BoundTime = 0;
        }


        //滑空
        Fly();

        //--------------------------キャラの移動-------------------------------------------
        var moveValue = _move.ReadValue<Vector2>();

        // 移動入力に基づいて移動フラグを設定
        isMovingFlg = moveValue.x != 0 || moveValue.y != 0;

        _moveVelocity.x = moveValue.x * moveSpeed;
        _moveVelocity.z = moveValue.y * moveSpeed;

        Vector3 verMove = transform.forward * _moveVelocity.z;
        Vector3 horiMove = transform.right * _moveVelocity.x;
        moveInput = horiMove + verMove;
        moveInput.Normalize();

        moveInput = moveInput * moveSpeed;

        //-----------------地面にいるときはジャンプができる----------------------------
        if (_characterController.isGrounded)
        {
            
            //JumpingFlg = false;
            if (_jump.WasPerformedThisFrame())
            {
                _moveVelocity.y = jumpPower;
                JumpingFlg = true;
            }
        }
        else
        {

            if (BoundFlg == false)
            {
                //重力
                _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
            }
            
            if(FlyFlg==true)
            {
                _moveVelocity.y = 0.0f;
                FlyGravity += -0.8f * Time.deltaTime;
            }

            //if(BoundFlg==true)
            //{
            //    _moveVelocity.y = 0.0f;
            //}
        }

        //Debug.Log(JumpingFlg);
        if (FlyFlg == true)
        {
            moveInput.y = moveInput.y + FlyGravity;
        }
        else
        {
            moveInput.y = moveInput.y + _moveVelocity.y + BoundPower;//moveInputにY軸の情報も追加する
        }
        _characterController.Move(moveInput * Time.deltaTime);//ここで最終的なキャラの移動情報を渡す

        //-----------------滑空時のパーティクル制御-----------------
        if (FlyFlg)
        {
            if (!_particleManager.IsEffectPlaying("RadialLines")) // 再生されていない場合は再生
            {
                _particleManager.PlayEffect("RadialLines");
            }
        }
        else
        {
            if (_particleManager.IsEffectPlaying("RadialLines")) // 再生中の場合は停止
            {
                _particleManager.StopEffect("RadialLines");
            }
        }

        //

        //transform.rotation = Quaternion.Euler(Camera.yRotation,0f,Camera.transform.rotation.eulerAngles.z);

        //-------------------------------------カメラ関連-----------------------------------------



        /*//カメラの回転制御
        Vector2 mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), 0.0f) * mouseSensitivity;

        if (invertX)
        {
            mouseInput.x = -mouseInput.x;
        }

        if (invertY)
        {
            mouseInput.y = -mouseInput.y;
        }


        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);
        */

        //camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));

        //モモンガの頭部
        //MomongaHead.rotation = Quaternion.Euler(MomongaHead.rotation.eulerAngles + new Vector3(mouseInput.y, 0f, 0f));
        //camTrans.rotation = Quaternion.Euler(camTrans.rotation.eulerAngles + new Vector3(-mouseInput.y, 0f, 0f));
    }

    // 移動キーが押されているかどうかを返すプロパティ
    public bool IsMoving()
    {
        return isMovingFlg;
    }

    void Fly()
    {
        // ジャンプ中かどうかを確認
        if (JumpingFlg == true)
        {
            // ジャンプ開始時に飛行フラグをセット
            if (_jump.WasPerformedThisFrame() && !FlyFlg)
            {
                FlyFlg = true;
                FlyCount = 0; // FlyCountをリセット

                DisableCharacterControllerCollision(); // 滑空開始時に判定を無効化
                HideFeetDuringFly();
            }
        }

        // 飛行中の処理
        if (FlyFlg == true)
        {
            FlyCount++;

            // 飛行カウントが5に達っするか地面に触れると飛行を停止
            if (FlyCount >= 5)
            {
                if (_jump.WasPerformedThisFrame() || _characterController.isGrounded)
                {
                    FlyFlg = false;
                }
            }
        }

        // 飛行が終了した場合のリセット処理
        if (FlyFlg == false)
        {
            moveInput.y = 0.0f;  // Y軸の移動をリセット
            FlyGravity = 0;      // 飛行重力をリセット
            FlyCount = 0;        // カウントをリセット
            EnableCharacterControllerCollision(); // 滑空終了時に判定を有効化
            ShowFeet();
        }
    }

    private void DisableCharacterControllerCollision()
    {
        // 判定をゼロに設定
        _characterController.center = Vector3.zero;
        _characterController.height = 0.0f;

        // カプセルコライダーを無効化
        if (_capsuleCollider != null)
        {
            _capsuleCollider.enabled = false;
        }
        // PlayerHitObjのコライダーを無効化
        if (_playerHit != null)
        {
            Collider playerCollider = _playerHit.GetComponent<Collider>();

            playerCollider.enabled = false;
        }
        // PlayerHeadのコライダーを有効化
        if (_playerHead != null)
        {
            Collider playerHeadCollider = _playerHead.GetComponent<Collider>();

            playerHeadCollider.enabled = true;
        }
    }

    private void EnableCharacterControllerCollision()
    {
        // 元の設定に戻す
        _characterController.center = _originalCenter;
        _characterController.height = _originalHeight;

        // カプセルコライダーを有効化
        if (_capsuleCollider != null)
        {
            _capsuleCollider.enabled = true;
        }
        // PlayerHitObjのコライダーを有効化
        if (_playerHit != null)
        {
            Collider playerCollider = _playerHit.GetComponent<Collider>();

            playerCollider.enabled = true;
        }
        // PlayerHeadのコライダーを無効化
        if (_playerHead != null)
        {
            Collider playerCollider = _playerHead.GetComponent<Collider>();

            playerCollider.enabled = false;
        }
    }

    // 滑空中に足を非表示にする
    private void HideFeetDuringFly()
    {
        HideObjectRenderer(playerLeftFoot);
        HideObjectRenderer(playerRightFoot);
    }

    // 足を表示する
    private void ShowFeet()
    {
        ShowObjectRenderer(playerLeftFoot);
        ShowObjectRenderer(playerRightFoot);
    }

    // オブジェクトのRendererを非表示にする
    private void HideObjectRenderer(GameObject obj)
    {
        if (obj != null)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.enabled = false; // オブジェクトを非表示
            }
        }
    }

    // オブジェクトのRendererを表示する
    private void ShowObjectRenderer(GameObject obj)
    {
        if (obj != null)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.enabled = true; // オブジェクトを表示
            }
        }
    }
}