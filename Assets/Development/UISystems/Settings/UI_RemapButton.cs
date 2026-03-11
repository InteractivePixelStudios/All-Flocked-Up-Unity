using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_RemapButton : MonoBehaviour
{
    public UI_ControlsMenu controlsRef;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI actionText;
    public Button remapButton;

    private void Start()
    {
        remapButton.onClick.AddListener(RebindButton);
        EventSystem.current.SetSelectedGameObject(remapButton.gameObject);
    }

    public void RebindButton()
    {
        controlsRef.CheckForRebindPressed(actionText.text.ToString());
    }
}

