using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private List<ParticleSystem> particles;

    public void PlayEffect(string effectName)
    {
        var particle = particles.Find(p => p.name == effectName);
        if (particle != null)
        {
            particle.Play();
        }
        else
        {
            Debug.LogWarning($"Effect {effectName} not found!");
        }
    }

    public void StopEffect(string effectName)
    {
        var particle = particles.Find(p => p.name == effectName);
        if (particle != null)
        {
            particle.Stop();
        }
    }
}
