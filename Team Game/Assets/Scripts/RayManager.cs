using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayManager : MonoBehaviour
{
    public Camera mainCamera;  // カメラを参照
    public BulletManager bulletManager;  // BulletManagerを参照

    // ターゲット位置を取得するメソッド
    public Vector3 GetTargetPosition()
    {
        // レイを飛ばす
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        Vector3 targetPosition;

        // レイキャストを行う
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // レイがヒットした位置を取得
            targetPosition = hit.point;
        }
        else
        {
            // ヒットしなかった場合は、カメラの前方をターゲットとする
            targetPosition = ray.origin + ray.direction * 20f;
        }

        return targetPosition;
    }

    /*//発射元オブジェクト
    public GameObject Emitter_object;

    //判定処理をしたくないレイヤーを無視するためのレイヤーマスク
    public LayerMask mask = -1;

    // Update is called once per frame
    void FixedUpdate()
    {

        //発射位置と方向
        Vector3 emitpos = Emitter_object.transform.position;
        Vector3 emitdir = Emitter_object.transform.forward;

        //新しいレイを作成する（発射位置と方向をあらわす情報）
        Ray ray = new Ray(emitpos, emitdir);

        //あたったものの情報が入るRaycastHitを準備
        RaycastHit hit;

        //物理コリジョン判定をさせるためにレイをレイキャストしてあたったかどうか返す
        bool isHit = Physics.Raycast(ray, out hit, 100.0f, mask);

        //あたっているなら
        if (isHit)
        {
            //あたったゲームオブジェクトを取得
            GameObject hitobject = hit.collider.gameObject;

            //マテリアルカラー変更
            //hitobject.GetComponent().material.color = Color.green;

            //あたったオブジェクト削除
            //Destroy(hitobject);

            //レイがあたった座標
            Vector3 position = hit.point;

            //レイがあたった当たり判定オブジェクトの面の法線
            Vector3 normal = hit.normal;

            //レイの方向ベクトル
            Vector3 direction = ray.direction;

            //反射ベクトル（反射方向を示すベクトル）
            Vector3 reflect_direction = 2 * normal * Vector3.Dot(normal, -direction) + direction;

            //レイと反射ベクトルのなす角度(ラジアン）
            float rad = Mathf.Acos(Vector3.Dot(-ray.direction, reflect_direction) / ray.direction.magnitude * reflect_direction.magnitude);

            //ラジアンを度に変換
            float deg = rad * Mathf.Rad2Deg;

            //反射角度が90度以上だったら・・・
            //if(deg>90)
            //{  
            //}

            ////////////// デバッグ用 ////////////////

            //（デバッグ用）角度を表示
            Debug.Log(deg);

            //（デバッグ用）新しい反射用レイを作成する
            Ray reflect_ray = new Ray(position, reflect_direction);

            //（デバッグ用）レイを画面に表示する
            Debug.DrawLine(reflect_ray.origin, reflect_ray.origin + reflect_ray.direction * 100, Color.blue, 0);

        }

        ////////////// デバッグ用 ////////////////

        //（デバッグ用）発射レイを表示
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 100, Color.red, 0);

    }*/
}
