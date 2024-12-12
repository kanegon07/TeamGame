using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class EffectManager : MonoBehaviour
{
    // エフェクト管理用
    private Dictionary<string, EffekseerEffectAsset> _loadedEffects;

    //エフェクトを追尾させるか
    private Dictionary<EffekseerHandle, Transform> _trackingEffects;

    // Start is called before the first frame update
    void Start()
    {
        InitializeEffects();
    }

    // 必要なエフェクトのロード
    private void InitializeEffects()
    {
        //エフェクト辞書を初期化する
        _loadedEffects = new Dictionary<string, EffekseerEffectAsset>();
        _trackingEffects = new Dictionary<EffekseerHandle, Transform>();

        // エフェクトを辞書に登録
        AddEffect("Undine", "Undine"); // テスト用エフェクト
        AddEffect("Sylph", "Sylph"); //落下地点用エフェクト
    }

    // エフェクトを辞書に追加
    private void AddEffect(string key, string resourcePath)
    {
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(resourcePath);
        if (effect != null)
        {
            _loadedEffects.Add(key, effect);
        }
        else
        {
            Debug.LogWarning($"エフェクト {resourcePath} のロードに失敗しました。");
        }
    }

    // エフェクトを再生
    public void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        //エフェクトのサイズを指定しない場合はエフェクトのデフォルトサイズを適用
        if (scale == default)
        {
            scale = Vector3.one;
        }

        if (_loadedEffects.TryGetValue(key, out EffekseerEffectAsset effect))
        {
            EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, position);
            handle.SetRotation(rotation);
            handle.SetScale(scale);

            // 追尾対象がある場合は辞書に追加
            if (followTarget != null)
            {
                _trackingEffects.Add(handle, followTarget);
            }
        }
        else
        {
            Debug.LogWarning($"エフェクトキー {key} が見つかりません。");
        }
    }

    //テストエフェクト
    public void PlayTestEffect(Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        PlayEffect("Undine", position, rotation, followTarget, scale);
    }

    //落下地点エフェクト
    public void PlayFallingPointEffect(Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        PlayEffect("Sylph", position, rotation, followTarget, scale);
    }

    /*
    // プレイヤーの位置と回転にエフェクトのTransformを追従させるコルーチン
    private IEnumerator FollowPlayer(Transform effectTransform, Transform playerTransform)
    {
        while (true)
        {
            effectTransform.position = playerTransform.position; // エフェクトの位置をプレイヤーの位置に設定
            effectTransform.rotation = playerTransform.rotation; // エフェクトの回転をプレイヤーの回転に設定
            yield return null; // 次のフレームまで待機
        }
    }
    */

    // Update is called once per frame
    // 追尾するエフェクトの位置を更新
    void Update()
    {
        List<EffekseerHandle> handlesToRemove = new List<EffekseerHandle>();
        foreach (var kvp in _trackingEffects)
        {
            if (kvp.Key.exists)
            {
                kvp.Key.SetLocation(kvp.Value.position);
            }
            else
            {
                handlesToRemove.Add(kvp.Key); // 存在しない場合削除リストに追加
            }
        }

        // 追尾が終わったエフェクトを辞書から削除
        foreach (var handle in handlesToRemove)
        {
            _trackingEffects.Remove(handle);
        }
    }
}
