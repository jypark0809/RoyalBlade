using System.Collections;
using UnityEngine;
// using Transform = UnityEngine.Transform;

public class ObjectManager
{
    public PlayerController Player { get; set; }
    public MonsterController Monsters { get; set; }

    public T Spawn<T>(Vector3 position, string prefabName) where T : MonoBehaviour
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName);
            PlayerController pc = go.GetOrAddComponent<PlayerController>();
            go.transform.position = position;

            Player = pc;
            Managers.Game.Player = pc;

            return pc as T;
        }
        else if (type == typeof(MonsterController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            MonsterController mc = go.GetOrAddComponent<MonsterController>();
            go.transform.position = position;

            Monsters = mc;
            go.name = prefabName;

            return mc as T;
        }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {
        if (obj.IsValid() == false)
        {
            return;
        }

        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            // TODO
            // �÷��̾� ���
        }
        else if (type == typeof(MonsterController))
        {
            Managers.Resource.Destroy(obj.gameObject);
        }
    }

    public void ShowDamageText(Vector2 pos, int damage, bool isCritical = false)
    {
        string prefabName;
        if (isCritical)
            prefabName = "CriticalDamageText";
        else
            prefabName = "DamageText";

        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        DamageText damageText = go.GetOrAddComponent<DamageText>();
        damageText.SetInfo(pos, damage);
    }

    public void ShowDestroyEffect(Vector2 pos)
    {
        GameObject go = Managers.Resource.Instantiate("Explosion", pooling: true);
        BaseEffect effect = go.GetOrAddComponent<BaseEffect>();
        effect.transform.position = pos;
    }
}
