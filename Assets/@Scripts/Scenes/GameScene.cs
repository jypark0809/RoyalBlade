using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    PlayerController _player;
    UI_GameScene _ui;
    SpawningPool _spawningPool;
    Vector3 _spawnPos = new Vector3 (0, 5f, 0);

    protected override void Init()
    {
        base.Init();

        // 리소스 로딩 (Addressable)
        Managers.Resource.LoadAllAsync<UnityEngine.Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        });
    }

    void StartLoaded()
    {
        SceneType = Define.Scene.GameScene;

        Managers.UI.ShowPopupUI<UI_SelectWeapon>();

        // 플레이어 생성
        _player = Managers.Object.Spawn<PlayerController>(_spawnPos, "Player");

        // UI 생성
        _ui = Managers.UI.ShowSceneUI<UI_GameScene>();
        _ui.OnPlayerJump += _player.Jump;
        _ui.OnPlayerGuard += _player.Guard;
        _ui.OnPlayerAttack += _player.Attack;

        Camera.main.GetComponent<CameraController>().PlayerTransform = _player.gameObject.transform;

        // 몬스터 생성
        if (_spawningPool == null)
            _spawningPool = gameObject.AddComponent<SpawningPool>();
        _spawningPool.StartSpawn();

        // Bgm
        Managers.Sound.Play(Define.Sound.Bgm, "Bgm_Game", volume: 0.6f);
    }

    public override void Clear()
    {

    }

    private void OnDisable()
    {
        _ui.OnPlayerJump -= _player.Jump;
        _ui.OnPlayerGuard -= _player.Guard;
        _ui.OnPlayerAttack -= _player.Attack;
    }
}
