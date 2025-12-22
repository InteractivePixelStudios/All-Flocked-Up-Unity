using Unity.VisualScripting;
using UnityEngine;

public class PerchableObject_Tree : MonoBehaviour, I_Perchable
{
    public GameObject playerRef;
    [SerializeField] private bool isPerching;
    public bool isHiding;
    [SerializeField] private GameObject[] topHideSpots;
    [SerializeField] private GameObject[] branchPerchSpots;
    int currentIndex;
    [SerializeField] private Vector3 playerOffset  = new Vector3(0,0,0);
    [SerializeField] private SphereCollider perchSphere;
    [SerializeField] private SphereCollider hideColliders;
    [SerializeField] private UI_PerchPrompt currentPrompt;
    [SerializeField] private UI_PerchPrompt promptPrefab;
    [SerializeField] private bool isPromptShown;

    void Update()
    {
        if (isPerching)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopPerch();
                playerRef.GetComponent<Rigidbody>().linearVelocity = new Vector3(1, 1, 0);
            }
            UpdatePerch();
        }
        else return;
    }

    public void StartPerch()
    {
        isPerching = true;
        if (isHiding)
        {
            playerRef.transform.position = topHideSpots[currentIndex].transform.position + playerOffset;
        }
        else playerRef.transform.position = branchPerchSpots[currentIndex].transform.position + playerOffset;
    }

    public void StopPerch()
    {
        isPerching = false;
        ToggleMeshCollidersOn();
    }

    public void UpdatePerch()
    {
        if (isHiding)
        {
            playerRef.transform.position = topHideSpots[currentIndex].transform.position + playerOffset;
        }else playerRef.transform.position = branchPerchSpots[currentIndex].transform.position+ playerOffset;
    }

    public void MovePosition()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            currentIndex++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentIndex--;
            if(currentIndex < 0)
            {
                currentIndex = 0;
            }
        }
    }

    protected void ShowPrompt()
    {
        if (currentPrompt == null)
        {
            currentPrompt = Instantiate<UI_PerchPrompt>(promptPrefab);
            isPromptShown = true;
        }
        else return;
    }

    protected void HidePrompt()
    {
        if (currentPrompt != null)
        {
            Destroy(currentPrompt);
            isPromptShown = false;
        }
        else return;
    }

    protected void ToggleMeshCollidersOn()
    {

        hideColliders.GetComponent<SphereCollider>().isTrigger = true;
        
    }

    protected void ToggleMeshCollidersOff()
    {

        hideColliders.GetComponent<SphereCollider>().isTrigger = false;
        
    }

   void OnTriggerEnter(Collider perchSphere)
    {
        if (perchSphere.gameObject.CompareTag("Player"))
        {
            if (playerRef == null)
            {

                playerRef = perchSphere.gameObject;
            }
        }
    }

    void OnTriggerStay(Collider perchSphere)
    {
        if (perchSphere.gameObject.CompareTag("Player"))
        {
            if (playerRef == null)
            {

                playerRef = perchSphere.gameObject;
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerRef != null)
            {
                playerRef = null;

            }
        }
    }
}
