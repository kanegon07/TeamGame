using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class EffectManager : MonoBehaviour
{
    // エフェクト管理用
    private Dictionary<string, EffekseerEffectAsset> _effects;

    private EffekseerHandle _landingEffectHandle;
    private EffekseerHandle _testEffectHandle;

    //エフェクトを追尾させるか
    private Dictionary<EffekseerHandle, Transform> _followingEffects;

    // Start is called before the first frame update
    void Start()
    {
        //エフェクト辞書を初期化する
        _effects = new Dictionary<string, EffekseerEffectAsset>();
        _followingEffects = new Dictionary<EffekseerHandle, Transform>();

        // 必要なエフェクトをロードして辞書に追加する
        AddEffect("Undine", "Undine"); //テスト用
    }

    // エフェクトを辞書に追加
    private void AddEffect(string key, string resourcePath)
    {
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(resourcePath);
        if (effect != null)
        {
            _effects.Add(key, effect);
        }
        else
        {
            Debug.LogWarning($"Effect {resourcePath} could not be loaded.");
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

        if (_effects.TryGetValue(key, out EffekseerEffectAsset effect))
        {
            EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, position);
            handle.SetRotation(rotation);
            handle.SetScale(scale);

            // 追尾対象がある場合は辞書に追加
            if (followTarget != null)
            {
                _followingEffects.Add(handle, followTarget);
            }
        }
        else
        {
            Debug.LogWarning($"Effect with key {key} not found.");
        }
    }

    public void PlayTestEffect(Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        PlayEffect("Undine", position, rotation, followTarget, scale);
    }

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

    // Update is called once per frame
    void Update()
    {
        // 追尾するエフェクトの位置を更新
        List<EffekseerHandle> handlesToRemove = new List<EffekseerHandle>();
        foreach (var kvp in _followingEffects)
        {
            if (kvp.Key.exists)
            {
                kvp.Key.SetLocation(kvp.Value.position);
            }
            else
            {
                handlesToRemove.Add(kvp.Key);
            }
        }

        // 追尾が終わったエフェクトを辞書から削除
        foreach (var handle in handlesToRemove)
        {
            _followingEffects.Remove(handle);
        }
    }
}
