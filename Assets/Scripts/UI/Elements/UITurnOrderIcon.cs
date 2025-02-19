using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UITurnOrderIcon : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Render(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
