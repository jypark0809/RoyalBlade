using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    TextMeshPro _damageText;
    Coroutine _coTextAnimation;

    public void SetInfo(Vector2 pos, int damage)
    {
        _damageText = GetComponent<TextMeshPro>();
        transform.position = pos;
        _damageText.text = $"{damage}";

        if (_coTextAnimation != null)
            StopCoroutine(_coTextAnimation);
        _coTextAnimation = StartCoroutine(CoTextAnimation());
    }

    float _tick = 0;
    IEnumerator CoTextAnimation()
    {
        _damageText.alpha = 1;

        while (_tick < 1.0f)
        {
            transform.position += Vector3.up * Time.deltaTime;
            _damageText.alpha -= Time.deltaTime;
            _tick += Time.deltaTime;
            yield return null;
        }

        _tick = 0;
        _coTextAnimation = null;
        Managers.Resource.Destroy(gameObject);
    }
}
