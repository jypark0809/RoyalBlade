using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    Vector2 _spawnPosition;
    Coroutine _coUpdateSpawningPool;
    GameManager _game;

    public void StartSpawn()
    {
        _game = Managers.Game;
        _game.OnWaveIndexChanged += HandleOnWaveIndexChanged;
        _spawnPosition = _game.Player.transform.position + new Vector3(0, 20f, 0);
        Managers.Object.Spawn<MonsterController>(_spawnPosition, "MonsterController");
    }

    void HandleOnWaveIndexChanged()
    {
        if (_coUpdateSpawningPool == null)
            _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    IEnumerator CoUpdateSpawningPool()
    {
        yield return new WaitForSeconds(2f);

        // Next Wave UI
        (Managers.UI.SceneUI as UI_GameScene).SetWaveText();

        // Spawn Monster
        _spawnPosition = _game.Player.transform.position + new Vector3(0, 20f, 0);
        Managers.Object.Spawn<MonsterController>(_spawnPosition, "MonsterController");

        _coUpdateSpawningPool = null;
    }

    void OnDisable()
    {
        if (_game != null)
            _game.OnWaveIndexChanged -= HandleOnWaveIndexChanged;
    }
}
