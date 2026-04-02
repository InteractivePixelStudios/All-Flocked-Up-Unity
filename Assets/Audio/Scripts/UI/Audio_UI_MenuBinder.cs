using UnityEngine;
using UnityEngine.UI;

/*
---Notes---
- This script is just a temporary "dirty" solution, once I make the Editor version this will not be a thing - IPM
*/

public class Audio_UI_MenuBinder : MonoBehaviour
{
    private void Start()
    {
        var audioWizard = AudioWizard.Instance;

        Button[] buttons = GetComponentsInChildren<Button>(true);

        foreach (var button in buttons)
        {
            var listener = button.GetComponent<Audio_UI_Listener>();
            if (listener == null)
                listener = button.gameObject.AddComponent<Audio_UI_Listener>();

            button.onClick.RemoveListener(listener.OnButtonClick);
            button.onClick.AddListener(listener.OnButtonClick);
        }
    }
}
