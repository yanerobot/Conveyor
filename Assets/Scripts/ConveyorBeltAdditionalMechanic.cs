using UnityEngine;

public class ConveyorBeltAdditionalMechanic : MonoBehaviour
{
    [SerializeField] GameObject cutPrefabGFX;
    [SerializeField] EffectHandler effects;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent(out MovingPart part))
        {
            part.ChangeGFX(cutPrefabGFX);
            effects.gameObject.SetActive(true);
            effects.PlayEffects();
        }
    }
}
