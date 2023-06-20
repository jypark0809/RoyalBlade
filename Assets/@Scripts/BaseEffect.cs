using System.Collections;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{
    ParticleSystem _particleSystem;
    Coroutine _coEffectEnd;

    void OnEnable()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        if (_coEffectEnd != null)
            StopCoroutine(_coEffectEnd);
        _coEffectEnd = StartCoroutine(CoEffectEnd(_particleSystem));
    }

    IEnumerator CoEffectEnd(ParticleSystem effect)
    {
        yield return new WaitForSeconds(_particleSystem.main.duration);
        Managers.Resource.Destroy(effect.gameObject);
    }
}
