using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // �p�[�e�B�N�����Ō��������������邽�߂̎���
    private Dictionary<string, ParticleSystem> particleDictionary;

    // �C���X�y�N�^�[�Őݒ�\�ȃp�[�e�B�N���̃��X�g
    [SerializeField] private List<ParticleSystem> particles;

    //������������
    private void Awake()
    {
        InitializeDictionary();
    }

    // �d������p�[�e�B�N�������Ȃ����m�F
    private void InitializeDictionary()
    {
        particleDictionary = new Dictionary<string, ParticleSystem>();

        foreach (var particle in particles)
        {
            if (!particleDictionary.ContainsKey(particle.name))
            {
                // �p�[�e�B�N�������L�[�Ƃ��Ď����ɓo�^
                particleDictionary.Add(particle.name, particle);
            }
            else
            {
                Debug.LogWarning($"�p�[�e�B�N�������d�����Ă��܂�: {particle.name}. �ŏ��̃C���X�^���X�̂ݎg�p����܂��B");
            }
        }
    }

    // �w�肵���p�[�e�B�N�����擾���郁�\�b�h
    private ParticleSystem GetParticle(string effectName)
    {
        if (particleDictionary.TryGetValue(effectName, out var particle))
        {
            return particle;
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽ�G�t�F�N�g {effectName} ��������܂���I");
            return null;
        }
    }

    // �w�肵���p�[�e�B�N�����Đ�
    public void PlayEffect(string effectName)
    {
        var particle = GetParticle(effectName);
        if (particle != null)
        {
            particle.Play();
        }
    }

    // �w�肵���p�[�e�B�N�����~
    public void StopEffect(string effectName)
    {
        var particle = GetParticle(effectName);
        if (particle != null)
        {
            particle.Stop();
        }
    }

    // �w�肵���p�[�e�B�N�����Đ������ǂ����𔻒�
    public bool IsEffectPlaying(string effectName)
    {
        var particle = GetParticle(effectName);
        return particle != null && particle.isPlaying;
    }
}
