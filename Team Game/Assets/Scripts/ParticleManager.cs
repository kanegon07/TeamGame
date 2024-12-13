using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // パーティクル名で検索を高速化するための辞書
    private Dictionary<string, ParticleSystem> particleDictionary;

    // インスペクターで設定可能なパーティクルのリスト
    [SerializeField] private List<ParticleSystem> particles;

    //辞書を初期化
    private void Awake()
    {
        InitializeDictionary();
    }

    // 重複するパーティクル名がないか確認
    private void InitializeDictionary()
    {
        particleDictionary = new Dictionary<string, ParticleSystem>();

        foreach (var particle in particles)
        {
            if (!particleDictionary.ContainsKey(particle.name))
            {
                // パーティクル名をキーとして辞書に登録
                particleDictionary.Add(particle.name, particle);
            }
            else
            {
                Debug.LogWarning($"パーティクル名が重複しています: {particle.name}. 最初のインスタンスのみ使用されます。");
            }
        }
    }

    // 指定したパーティクルを取得するメソッド
    private ParticleSystem GetParticle(string effectName)
    {
        if (particleDictionary.TryGetValue(effectName, out var particle))
        {
            return particle;
        }
        else
        {
            Debug.LogWarning($"指定されたエフェクト {effectName} が見つかりません！");
            return null;
        }
    }

    // 指定したパーティクルを再生
    public void PlayEffect(string effectName)
    {
        var particle = GetParticle(effectName);
        if (particle != null)
        {
            particle.Play();
        }
    }

    // 指定したパーティクルを停止
    public void StopEffect(string effectName)
    {
        var particle = GetParticle(effectName);
        if (particle != null)
        {
            particle.Stop();
        }
    }

    // 指定したパーティクルが再生中かどうかを判定
    public bool IsEffectPlaying(string effectName)
    {
        var particle = GetParticle(effectName);
        return particle != null && particle.isPlaying;
    }
}
