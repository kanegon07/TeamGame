using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class EffectManager : MonoBehaviour
{
    // �G�t�F�N�g�Ǘ��p
    private Dictionary<string, EffekseerEffectAsset> _loadedEffects;

    //�G�t�F�N�g��ǔ������邩
    private Dictionary<EffekseerHandle, Transform> _trackingEffects;

    // Start is called before the first frame update
    void Start()
    {
        InitializeEffects();
    }

    // �K�v�ȃG�t�F�N�g�̃��[�h
    private void InitializeEffects()
    {
        //�G�t�F�N�g����������������
        _loadedEffects = new Dictionary<string, EffekseerEffectAsset>();
        _trackingEffects = new Dictionary<EffekseerHandle, Transform>();

        // �G�t�F�N�g�������ɓo�^
        AddEffect("Undine", "Undine"); // �e�X�g�p�G�t�F�N�g
        AddEffect("Sylph", "Sylph"); //�����n�_�p�G�t�F�N�g
    }

    // �G�t�F�N�g�������ɒǉ�
    private void AddEffect(string key, string resourcePath)
    {
        EffekseerEffectAsset effect = Resources.Load<EffekseerEffectAsset>(resourcePath);
        if (effect != null)
        {
            _loadedEffects.Add(key, effect);
        }
        else
        {
            Debug.LogWarning($"�G�t�F�N�g {resourcePath} �̃��[�h�Ɏ��s���܂����B");
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

        if (_loadedEffects.TryGetValue(key, out EffekseerEffectAsset effect))
        {
            EffekseerHandle handle = EffekseerSystem.PlayEffect(effect, position);
            handle.SetRotation(rotation);
            handle.SetScale(scale);

            // �ǔ��Ώۂ�����ꍇ�͎����ɒǉ�
            if (followTarget != null)
            {
                _trackingEffects.Add(handle, followTarget);
            }
        }
        else
        {
            Debug.LogWarning($"�G�t�F�N�g�L�[ {key} ��������܂���B");
        }
    }

    //�e�X�g�G�t�F�N�g
    public void PlayTestEffect(Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        PlayEffect("Undine", position, rotation, followTarget, scale);
    }

    //�����n�_�G�t�F�N�g
    public void PlayFallingPointEffect(Vector3 position, Quaternion rotation, Transform followTarget = null, Vector3 scale = default)
    {
        PlayEffect("Sylph", position, rotation, followTarget, scale);
    }

    /*
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
    */

    // Update is called once per frame
    // �ǔ�����G�t�F�N�g�̈ʒu���X�V
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
                handlesToRemove.Add(kvp.Key); // ���݂��Ȃ��ꍇ�폜���X�g�ɒǉ�
            }
        }

        // �ǔ����I������G�t�F�N�g����������폜
        foreach (var handle in handlesToRemove)
        {
            _trackingEffects.Remove(handle);
        }
    }
}
