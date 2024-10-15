using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class EffectManager : MonoBehaviour
{
    // �G�t�F�N�g�Ǘ��p
    private Dictionary<string, EffekseerEffectAsset> _effects;

    private EffekseerHandle _landingEffectHandle;
    private EffekseerHandle _testEffectHandle;

    //�G�t�F�N�g��ǔ������邩
    private Dictionary<EffekseerHandle, Transform> _followingEffects;

    // Start is called before the first frame update
    void Start()
    {
        //�G�t�F�N�g����������������
        _effects = new Dictionary<string, EffekseerEffectAsset>();
        _followingEffects = new Dictionary<EffekseerHandle, Transform>();

        // �K�v�ȃG�t�F�N�g�����[�h���Ď����ɒǉ�����
        AddEffect("Undine", "Undine"); //�e�X�g�p
    }

    // �G�t�F�N�g�������ɒǉ�
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

    // �G�t�F�N�g���Đ�
    public void PlayEffect(string key, Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        //�G�t�F�N�g�̃T�C�Y���w�肵�Ȃ��ꍇ�̓G�t�F�N�g�̃f�t�H���g�T�C�Y��K�p
        if (scale == default)
        {
            scale = Vector3.one;
        }

        if (_effects.TryGetValue(key, out EffekseerEffectAsset effect))
        {
            EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, position);
            handle.SetRotation(rotation);
            handle.SetScale(scale);

            // �ǔ��Ώۂ�����ꍇ�͎����ɒǉ�
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

    // �v���C���[�̈ʒu�Ɖ�]�ɃG�t�F�N�g��Transform��Ǐ]������R���[�`��
    private IEnumerator FollowPlayer(Transform effectTransform, Transform playerTransform)
    {
        while (true)
        {
            effectTransform.position = playerTransform.position; // �G�t�F�N�g�̈ʒu���v���C���[�̈ʒu�ɐݒ�
            effectTransform.rotation = playerTransform.rotation; // �G�t�F�N�g�̉�]���v���C���[�̉�]�ɐݒ�
            yield return null; // ���̃t���[���܂őҋ@
        }
    }

    // Update is called once per frame
    void Update()
    {
        // �ǔ�����G�t�F�N�g�̈ʒu���X�V
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

        // �ǔ����I������G�t�F�N�g����������폜
        foreach (var handle in handlesToRemove)
        {
            _followingEffects.Remove(handle);
        }
    }
}
