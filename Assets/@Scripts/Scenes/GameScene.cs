using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    PlayerController _player;
    UI_GameScene ui;
    Vector3 _spawnPos = new Vector3 (0, 5f, 0);

    protected override void Init()
    {
        base.Init();

        // ���ҽ� �ε� (Addressable)
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
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

        // �÷��̾� ����
        _player = Managers.Object.Spawn<PlayerController>(_spawnPos, "Player");

        // UI ����
        ui = Managers.UI.ShowSceneUI<UI_GameScene>();
        ui.OnPlayerJump += _player.Jump;
        ui.OnPlayerGuard += _player.Guard;
        ui.OnPlayerAttack += _player.Attack;

        Camera.main.GetComponent<CameraController>().PlayerTransform = _player.gameObject.transform;
    }

    public override void Clear()
    {

    }

    private void OnDestroy()
    {
        ui.OnPlayerJump -= _player.Jump;
        ui.OnPlayerGuard -= _player.Guard;
        ui.OnPlayerAttack -= _player.Attack;
    }
}
