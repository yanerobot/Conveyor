using UnityEngine;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystems;

    public void PlayEffects()
    {
        foreach (var ps in particleSystems)
        {
            ps.Play();
        }
    }
}
