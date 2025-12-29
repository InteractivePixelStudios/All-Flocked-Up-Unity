using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerPerchSystem : MonoBehaviour
{
    [SerializeField] private LayerMask perchLayer;
    [SerializeField] private I_Perchable currentPerchPoint;
    [SerializeField] private IconToggle icon;
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
                    isReady = true;
                    var check = hit.collider.CompareTag("HideSpot");
                    if (check)
                    {
                      var tree =currentPerchPoint as PerchableObject_Tree;
                        tree.isHiding = true;
                    }
                    break;
                case PerchableObject_Bush:
                    isReady = true;
                    break;
                case PerchableObject_General:
                    isReady = true;
                    break;
            }

        }
        else {  isReady = false; }
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
            
        }
    }


}
