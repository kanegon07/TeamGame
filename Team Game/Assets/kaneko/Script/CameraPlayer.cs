//テスト

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    public bool WallHitFlg = false;//壁にはりつけるよ
    public bool StickWall = false;//壁に張り付いてるよ
    private int StickWallCount = 0;
    private bool WallHitMouseButtonFlg = false;//壁貼りつき準備完了の時の右クリックを制御するためのフラグ
    private bool StickWallMouseButtonFlg = false;//壁貼りつきの時の右クリックを制御するためのフラグ
    private bool UnStickWall = false;//クリックじゃなく、途中で張り付きが解除された場合のフラグ

    private InputAction _StickWall;//壁に貼りついてる時のプレイヤー操作
    public float Player_Stamina = 0.0f;//壁貼りつき時のプレイヤーのスタミナ
    private const float StamiMax = 100.0f;//壁貼りつき時のプレイヤーのスタミナのMAX値
    private const float StaminaUp = 1.0f;//スタミナの回復の値
    private const float StaminaDown = 0.5f;//スタミナの消費の値

    public bool FallFlg = false;//落下しているかどうか
    public float RingSpeedUpForward = 0.0f;//リングでスピードアップする速さ
    public float RingSpeedUpY = 0.0f;//リングで上昇するY座標の大きさ
    private const float _Speed = 5.0f;//プレイヤーの移動速度の固定値
    public bool RingFlg = false;
    private int RingCount = 0;


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

        _StickWall = input.currentActionMap.FindAction("StickWall");

        _particleManager = GetComponent<ParticleManager>(); // ParticleManagerをキャッシュ
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Player_Stamina);
        //Debug.Log(FallFlg);
        Debug.Log(RingCount);
        //頭は常に非表示
        HideObjectRenderer(_playerHead);


        if(RingFlg==true)
        {
            RingCount++;
            if(RingCount>=30)
            {
                RingCount = 30;
                RingFlg = false;
            }
        }
        if (RingFlg == false)
        {
            RingCount = 0;
            moveSpeed = _Speed;
        }

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

       

        //壁に貼りつく
        Stickwall();

        

        //--------------------------キャラの移動-------------------------------------------
       

        if(Input.GetKey(KeyCode.T))
        {
            //transform.forward += UpSpeed;
            //nowForward = transform.forward;
            
           
        }
       
        

        //貼りついてる時
        if (StickWall == true)
        {
           
            //移動処理
            var WallmoveValue = _StickWall.ReadValue<Vector2>();

            //スタミナのための処理
            float vexX = _moveVelocity.x;
            float vexY = _moveVelocity.y;

            //移動処理(通常の処理とは違い、縦と横にしか動かない)
            _moveVelocity.x = WallmoveValue.x * moveSpeed;
            _moveVelocity.y = WallmoveValue.y * moveSpeed;

            //スタミナのための処理
            if(vexX!=_moveVelocity.x)
            {
                PlayerStaminaDec(0.2F);//スタミナをここで消費
            }

            if (vexY!= _moveVelocity.y)
            {
                PlayerStaminaDec(0.2F);//スタミナをここで消費
            }

            //移動処理
            Vector3 WallverMove = transform.up * _moveVelocity.y;
            Vector3 WallhoriMove = transform.right * _moveVelocity.x;
            moveInput = WallhoriMove + WallverMove;
            moveInput.Normalize();

            moveInput = moveInput * moveSpeed;


        }
        else
        {
          
            
            
            //-------------------移動処理-----------------------------

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
        }


        //-----------------地面にいるときはジャンプができる----------------------------
        if (_characterController.isGrounded)
        {
            

            WallHitFlg = false;//壁に当たってるよのフラグ
                //JumpingFlg = false;
            if (_jump.WasPerformedThisFrame())
            {
                _moveVelocity.y = jumpPower;
                JumpingFlg = true;//オフにするにはPlayerHitObj.csのほうで操作する

                
            }
        }
        else
        {
            if(FallFlg==true)
            {
                if (_jump.WasPerformedThisFrame())
                {
                    FlyFlg = true;
                }
            }

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

            if(StickWall == true)
            {
                _moveVelocity.y = 0.0f;
            }
            

            //if(BoundFlg==true)
            //{
            //    _moveVelocity.y = 0.0f;
            //}
        }

        //Debug.Log(JumpingFlg);

        


        if(StickWall==true)
        {
            moveInput.y = moveInput.y + 0;
        }
        else if (FlyFlg == true)
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
			PlayerStaminaDec(0.05F);

			FallFlg = false;
            FlyCount++;

            // 飛行カウントが5に達っするか地面に触れると飛行を停止
            if (FlyCount >= 5)
            {
                if (_jump.WasPerformedThisFrame() || _characterController.isGrounded || Player_Stamina <= 0F)
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



    void Stickwall()
    {
        //壁に貼りつける準備
        if (WallHitFlg == true)
        {

            //壁に貼りつくための右クリックtrue(可能)、貼りつき解除する右クリックfalse(不可能)の時
            if (WallHitMouseButtonFlg == true && StickWallMouseButtonFlg == false)
            {
                if (Input.GetMouseButtonDown(1))//右クリック
                {
                    WallHitMouseButtonFlg = false;//壁に貼りつくための右クリックがfalse(不可能)
                    StickWall = true;//貼りつけ！

                }
            }

        }

        //壁に貼りついてるよ
        if (StickWall == true)
        {
            //途中で範囲から外れた場合
            if(WallHitFlg==false)
            {
                UnStickWall = true;
                
            }

            _moveVelocity.y = 0.0f;//重力リセット
            FlyFlg = false;//滑空中の場合も解除する
            JumpingFlg = false;//ジャンプしてるかどうかもリセットする
            //WallHitFlg = false;
            StickWallCount++;//一応次のボタンが押せるようになるカウント

            if (StickWallCount >= 5)//カウントが5経ったら
            {
                StickWallMouseButtonFlg = true;//貼りつき解除するためのフラグがtrue
            }

            //貼りつき解除するため右クリックのフラグがtrue(可能)、貼りつくための右クリックのフラグがfalse(不可能)の時
            if (StickWallMouseButtonFlg == true && WallHitMouseButtonFlg == false)
            {
                if (Input.GetMouseButtonDown(1))//右クリック
                {
                    StickWallMouseButtonFlg = false;//壁から離れるための右クリックがfalse(不可能)
                    StickWall = false;//貼りついてないよ!

                }
            }


        }
        else if (StickWall == false)//貼りついてない時(貼りつき準備はケースバイケース)
        {
           
            WallHitMouseButtonFlg = true;//貼りつきするための右クリックのフラグがtrue(可能)
            StickWallCount = 0;//貼りつき解除のための右クリックのクールタイムをリセット
            UnStickWall = false;//任意では無い時に張り付きが解除された場合のフラグもfalse(そうではない状態)にする
            //WallHitFlg = true;//再度貼り付けるように、貼り付け準備フラグをtrue(可能)にする


        }


        //途中で貼り付きが解除された場合
        if(UnStickWall==true)
        {

            StickWallMouseButtonFlg = false;//壁から離れるための右クリックがfalse(不可能)


            StickWall = false;//貼りつき解除

        }

        //スタミナが切れた時の処理
        if (Player_Stamina <= 0)
        {
            Player_Stamina = 0;
            UnStickWall = true;

        }

    }

    //壁貼りつき時のスタミナが減っていく関数
    public void PlayerStaminaDec()
    {
        Player_Stamina -= StaminaDown;
        if(Player_Stamina<=0)
        {
            Player_Stamina = 0;
        }
    }

    public void PlayerStaminaDec(float _stamina)
    {
        Player_Stamina -= _stamina;
        if (Player_Stamina <= 0)
        {
            Player_Stamina = 0;
        }
    }


    //壁貼りつき時のスタミナが増えていく関数
    public void PlayerStaminaRec()
    {
        Player_Stamina += StaminaUp;
        if(Player_Stamina>=StamiMax)
        {
            Player_Stamina = StamiMax;
        }
    }

    public void PlayerStaminaRec(float _stamina)
    {
        Player_Stamina+= _stamina;
        if (Player_Stamina >= StamiMax)
        {
            Player_Stamina = StamiMax;
        }
    }


    
    public void RingSpeedUp(float ForwardSpeed,float SpeedY)
    {
        if (RingFlg == true)
        {
            moveSpeed = ForwardSpeed;
            _moveVelocity.y = SpeedY;
        }
        //else
        //{
        //    moveSpeed = _Speed;
        //}
    }
}