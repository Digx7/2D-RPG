using UnityEngine;
using TMPro;

public class BuildNumberHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private void Start()
    {
        textMeshProUGUI.text = Application.version;
    }
}
