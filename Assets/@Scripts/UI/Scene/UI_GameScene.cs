using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    Button _jumpButton;
    Button _guardButton;
    Button _AttackButton;
    private Vector2 _touchPos;
    private Vector2 _jumpButtonOriginalPos;
    private Vector2 _attackButtonOriginalPos;
    [SerializeField] Image[] _hearts;


    #region Enum
    enum GameObjects
    {
        HpSlider,
        JumpBackPanel,
        AttackBackPanel,
    }

    enum Buttons
    {
        JumpButton,
        GuardButton,
        AttackButton,
    }

    enum Images
    {
        SpecialSkillImage,
        WeaponSkillImage,
    }

    enum Texts
    {
        ComboText,
        MonsterHpText,
    }
    #endregion

    #region Action
    // public event Action<Vector2> OnMoveDirChanged;
    public Action OnPlayerJump;
    public Action OnPlayerGuard;
    public Action OnPlayerAttack;
    #endregion

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        #region Bind
        BindObject(typeof(GameObjects));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        _jumpButton = GetButton((int)Buttons.JumpButton);
        _guardButton = GetButton((int)Buttons.GuardButton);
        _AttackButton = GetButton((int)Buttons.AttackButton);

        _jumpButton.gameObject.BindEvent(OnJumpButtonClicked);
        _jumpButton.gameObject.BindEvent(OnJumpButtonPointerDown, type: Define.UIEvent.PointerDown);
        _jumpButton.gameObject.BindEvent(OnJumpButtonPointerUp, type: Define.UIEvent.PointerUp);
        _jumpButton.gameObject.BindEvent(dragAction: OnSpecialSkillDrag, type: Define.UIEvent.Drag);
        _jumpButton.gameObject.BindEvent(dragAction: OnSpecialSkillEndDrag, type: Define.UIEvent.EndDrag);
        _AttackButton.gameObject.BindEvent(OnAttackButtonClicked);
        _AttackButton.gameObject.BindEvent(OnAttackButtonPointerDown, type: Define.UIEvent.PointerDown);
        _AttackButton.gameObject.BindEvent(OnAttackButtonPointerUp, type: Define.UIEvent.PointerUp);
        _AttackButton.gameObject.BindEvent(dragAction: OnWeaponSkillDrag, type: Define.UIEvent.Drag);
        _AttackButton.gameObject.BindEvent(dragAction: OnWeaponSkillEndDrag, type: Define.UIEvent.EndDrag);
        _guardButton.gameObject.BindEvent(OnGuardButtonPointerDown, type: Define.UIEvent.PointerDown);
        #endregion

        _jumpButtonRectTransform = _jumpButton.gameObject.GetComponent<RectTransform>();
        _attackButtonRectTransform = _AttackButton.gameObject.GetComponent<RectTransform>();
        _jumpButtonOriginalPos = _jumpButton.transform.localPosition;
        _attackButtonOriginalPos = _AttackButton.transform.localPosition;

        GetObject((int)GameObjects.JumpBackPanel).SetActive(false);
        GetObject((int)GameObjects.AttackBackPanel).SetActive(false);

        Managers.Game.OnComboCountChanged += SetComboText;
        Managers.Game.OnJumpCountChanged += FillSpecialSkillGuage;
        Managers.Game.OnAttackCountChanged += FillWeaponSkillGuage;

        return true;
    }

    #region Combo
    Coroutine _CoTextAnimation;
    public void SetComboText(int combo)
    {
        GetText((int)Texts.ComboText).alpha = 1;

        if (combo == 0)
        {
            GetText((int)Texts.ComboText).text = "";
            return;
        }

        GetText((int)Texts.ComboText).text = $"{combo.ToString()} COMBO";
        if (_CoTextAnimation != null)
            StopCoroutine(_CoTextAnimation);
        _CoTextAnimation = StartCoroutine(CoTextAnimation());
    }

    IEnumerator CoTextAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        while (GetText((int)Texts.ComboText).alpha > 0)
        {
            GetText((int)Texts.ComboText).alpha -= Time.deltaTime;
            yield return null;
        }
    }
    #endregion

    public void SetPlayerHp(int hp)
    {
        foreach(Image img in _hearts)
            img.gameObject.SetActive(false);

        for (int i = 0; i < hp; i++)
            _hearts[i].gameObject.SetActive(true);
    }

    public void SetHpSlider(int hp, int maxHp)
    {
        Slider slider = GetObject((int)GameObjects.HpSlider).GetComponent<Slider>();
        slider.value = (float)hp / maxHp;

        GetText((int)Texts.MonsterHpText).text = $"{hp} / {maxHp}";
    }

    public void FillSpecialSkillGuage()
    {
        if (Managers.Game.JumpCount == 0)
            GetObject((int)GameObjects.JumpBackPanel).SetActive(false);

        if (Managers.Game.JumpCount >= Define.MAX_JUMP_COUNT)
            GetObject((int)GameObjects.JumpBackPanel).SetActive(true);

        GetImage((int)Images.SpecialSkillImage).fillAmount = (float)Managers.Game.JumpCount/Define.MAX_JUMP_COUNT;
    }

    public void FillWeaponSkillGuage()
    {
        if (Managers.Game.AttackCount >= Define.MAX_ATTACK_COUNT)
            GetObject((int)GameObjects.AttackBackPanel).SetActive(true);

        GetImage((int)Images.WeaponSkillImage).fillAmount = (float)Managers.Game.AttackCount / Define.MAX_ATTACK_COUNT;
    }

    #region Jump
    RectTransform _jumpButtonRectTransform;

    void OnJumpButtonClicked()
    {
        OnPlayerJump?.Invoke();
    }

    void OnJumpButtonPointerDown()
    {
        _touchPos = Input.mousePosition;
        GetImage((int)Images.SpecialSkillImage).color = new Color(0.5f, 0.5f, 0.5f);
    }

    void OnJumpButtonPointerUp()
    {
        GetImage((int)Images.SpecialSkillImage).color = new Color(1f, 1f, 1f);
    }

    void OnSpecialSkillDrag(BaseEventData baseEventData)
    {
        if (Managers.Game.JumpCount < Define.MAX_JUMP_COUNT)
            return;

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;

        float dist = (dragPos.y - _touchPos.y);
        // Debug.Log(dragPos);

        // 아래로 드래그
        if (dragPos.y < _touchPos.y)
            return;

        Vector2 newPos;
        if (dist > 200)
            newPos = new Vector2(_jumpButtonOriginalPos.x, _jumpButtonOriginalPos.y + 200);
        else
            newPos = new Vector2(_jumpButtonOriginalPos.x, _jumpButtonOriginalPos.y + dist);

        _jumpButtonRectTransform.localPosition = newPos;

    }

    void OnSpecialSkillEndDrag(BaseEventData baseEventData)
    {
        if (Managers.Game.JumpCount < Define.MAX_JUMP_COUNT)
            return;

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (pointerEventData.position.y >= 400)
        {
            Managers.Game.Player.SpecialSkill();
        }
            

        _jumpButtonRectTransform.localPosition = _jumpButtonOriginalPos;
    }
    #endregion

    #region Attack
    RectTransform _attackButtonRectTransform;

    void OnAttackButtonClicked()
    {
        OnPlayerAttack?.Invoke();
    }

    void OnAttackButtonPointerDown()
    {
        _touchPos = Input.mousePosition;
        GetImage((int)Images.WeaponSkillImage).color = new Color(0.5f, 0.5f, 0.5f);
    }

    void OnAttackButtonPointerUp()
    {
        GetImage((int)Images.WeaponSkillImage).color = new Color(1f, 1f, 1f);
    }

    void OnWeaponSkillDrag(BaseEventData baseEventData)
    {
        if (Managers.Game.AttackCount < Define.MAX_ATTACK_COUNT)
            return;

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector2 dragPos = pointerEventData.position;

        float dist = (dragPos.y - _touchPos.y);
        Debug.Log(dragPos);

        // 아래로 드래그
        if (dragPos.y < _touchPos.y)
            return;

        Vector2 newPos;
        if (dist > 200)
            newPos = new Vector2(_attackButtonOriginalPos.x, _attackButtonOriginalPos.y + 200);
        else
            newPos = new Vector2(_attackButtonOriginalPos.x, _attackButtonOriginalPos.y + dist);

        _attackButtonRectTransform.localPosition = newPos;

    }

    void OnWeaponSkillEndDrag(BaseEventData baseEventData)
    {
        if (Managers.Game.AttackCount < Define.MAX_ATTACK_COUNT)
            return;

        PointerEventData pointerEventData = baseEventData as PointerEventData;
        if (pointerEventData.position.y >= 400)
            Debug.Log("WeaponSkill");

        _attackButtonRectTransform.localPosition = _attackButtonOriginalPos;
    }
    #endregion

    void OnGuardButtonPointerDown()
    {
        _touchPos = Input.mousePosition;
        OnPlayerGuard?.Invoke();
    }
}
