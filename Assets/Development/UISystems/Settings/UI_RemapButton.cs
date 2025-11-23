using TMPro;
using UnityEngine;
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
    }

    public void RebindButton()
    {
        controlsRef.CheckForRebindPressed(actionText.text.ToString());
    }
}

