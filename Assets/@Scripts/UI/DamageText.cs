using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    TextMeshPro _damageText;
    Coroutine _coTextAnimation;

    public void SetInfo(Vector2 pos, int damage, Transform parent = null)
    {
        _damageText = GetComponent<TextMeshPro>();
        transform.position = pos;

        _damageText.text = $"{damage}";
        _damageText.alpha = 1;
        if (parent != null)
        {
            //transform.parent = parent;
            transform.position = parent.GetComponent<Collider2D>().bounds.center;

            //transform.localPosition = Vector3.zero;
            GetComponent<MeshRenderer>().sortingOrder = 300;
        }

        if (_coTextAnimation == null)
            _coTextAnimation = StartCoroutine(CoTextAnimation());
    }

    float _tick = 0;
    IEnumerator CoTextAnimation()
    {
        while (_tick < 1.0f)
        {
            transform.position += Vector3.up * Time.deltaTime;
            _damageText.alpha -= Time.deltaTime;
            _tick += Time.deltaTime;
            yield return null;
        }

        Managers.Resource.Destroy(gameObject);
    }
}
