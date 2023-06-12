using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public Queue<MonsterController> Monsters { get; } = new Queue<MonsterController>();

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
            go.name = prefabName;
            Monsters.Enqueue(mc);

            return mc as T;
        }

        return null;
    }
}
