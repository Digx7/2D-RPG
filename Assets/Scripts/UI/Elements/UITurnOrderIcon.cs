using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(ImageColorHelper))]
public class UITurnOrderIcon : MonoBehaviour
{
    public Vector3Channel RequestFocusLocationChannel;
    public BooleanChannel requestCanConfirmAbilities;
    
    private Image m_image;
    private CombatUnit m_combatUnit;
    private bool hasGone = false;
    private bool isTurn = false;
    private bool isHighLighted = false;
    private ImageColorHelper m_imageColorHelper;

    private void Awake()
    {
        m_image = GetComponent<Image>();
        m_imageColorHelper = GetComponent<ImageColorHelper>();
    }

    public void Render(Sprite sprite)
    {
        m_image.sprite = sprite;
    }



    public void SetCombatUnit(CombatUnit combatUnit)
    {
        m_combatUnit = combatUnit;
        Render(m_combatUnit.TurnOrderIcon);
    }

    public void RefreshColor()
    {
        if(isHighLighted)
        {
            m_imageColorHelper.SetColorIndex(3);
        }
        else if(isTurn)
        {
            m_imageColorHelper.SetColorIndex(2);
        }
        else if(hasGone)
        {
            m_imageColorHelper.SetColorIndex(1);
        }
        else
        {
            m_imageColorHelper.SetColorIndex(0);
        }
    }

    public void SetHasGone(bool value)
    {
        hasGone = value;
        RefreshColor();
    }

    public void SetIsTurn(bool value)
    {
        isTurn = value;
        RefreshColor();
    }

    public void SetIsHighLighted(bool value)
    {
        isHighLighted = value;
        RefreshColor();
    }

    public void OnPointerEnter()
    {
        requestCanConfirmAbilities.Raise(false);
        SetIsHighLighted(true);
    }

    public void OnPointerExit()
    {
        requestCanConfirmAbilities.Raise(true);
        SetIsHighLighted(false);
    }

    public void OnClick()
    {
        if(m_combatUnit != null)
        {
            Vector3 unitPosition = m_combatUnit.transform.position;
            RequestFocusLocationChannel.Raise(unitPosition);
        }
    }
}
