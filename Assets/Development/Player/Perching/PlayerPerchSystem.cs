using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerPerchSystem : MonoBehaviour
{
    [SerializeField] private LayerMask perchLayer;
    [SerializeField] private I_Perchable currentPerchPoint;
    [SerializeField] private UI_PerchPrompt prompt;
    [SerializeField] private UI_PerchPrompt promptPrefab;
    [SerializeField] private bool isReady;
    [SerializeField] private float checkDistance = 5f;


    void Update()
    {
        if (isReady && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithPerch(currentPerchPoint);
            Debug.Log("InteractWithPerch");
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, checkDistance, perchLayer))
        {
            hit.collider.TryGetComponent<I_Perchable>(out currentPerchPoint);
            switch (currentPerchPoint)
            {
                case PerchableObject_Tree:
                    ShowPrompt("Tree");
                    isReady = true;
                    var check = hit.collider.CompareTag("HideSpot");
                    if (check)
                    {
                      var tree =currentPerchPoint as PerchableObject_Tree;
                        tree.isHiding = true;
                    }
                    break;
                case PerchableObject_Bush:
                    ShowPrompt("Bush");
                    isReady = true;
                    break;
                case PerchableObject_General:
                    ShowPrompt(hit.collider.name);
                    isReady = true;
                    break;
            }

        }
        else { HidePrompt(); isReady = false; }
    }

    private void InteractWithPerch(I_Perchable currentPerchPoint)
    {
        try
        {
            currentPerchPoint.StartPerch();
        }
        catch
        {
            if(PerchableObject_Tree.Equals(currentPerchPoint, true))
            {
                currentPerchPoint.StartPerch();
            }
            
        }
        finally
        {
            HidePrompt();
        }
    }

    private void ShowPrompt(string obj)
    {
        if (prompt == null)
        {
            prompt = Instantiate<UI_PerchPrompt>(promptPrefab);
            prompt.promptText.SetText(obj);
        }
    }

    private void HidePrompt()
    {
        if(prompt != null)
        {
            Destroy(prompt.gameObject);
        }
    }


}
