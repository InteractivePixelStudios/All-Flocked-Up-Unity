using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerchableObject_Tree : MonoBehaviour, I_Perchable
{
    public GameObject playerRef;
    PlayerPerchSystem perchComp;
    public bool isPerching;
    public bool isHiding;
    bool jumpCheck => playerRef.GetComponent<PlayerGroundMovement>().GetIsFlying();
    [SerializeField] private GameObject[] topHideSpots;
    [SerializeField] private GameObject[] branchPerchSpots;
    int currentIndex;
    [SerializeField] private Vector3 playerOffset  = new Vector3(0,0,0);
    [SerializeField] private SphereCollider perchSphere;
    [SerializeField] private SphereCollider hideColliders;
    [SerializeField] private bool isPromptShown;
    [SerializeField] IconToggle icon;

  

    void Update()
    {

        if (playerRef != null && isPerching)
        {
            if (jumpCheck)
            {
                Debug.Log("Called" + jumpCheck);
                StopPerch();
                playerRef.GetComponent<Rigidbody>().linearVelocity = new Vector3(1, 1, 0);
            }
            UpdatePerch();
        }
        else return;
    }

    public void SetPlayerRef(GameObject player)
    {
        playerRef = player;
        perchComp = playerRef.GetComponent<PlayerPerchSystem>();
                    playerRef.GetComponentInChildren<IconToggle>().ShowIcon();
    }

    public void StartPerch()
    {
        isPerching = true;
        currentIndex = 0;
        playerRef.GetComponentInChildren<IconToggle>().HideIcon();
        if (isHiding || branchPerchSpots.Length ==0)
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

    public void MovePosition(float x)
    {

        if (x>0)
        {
            currentIndex++;
        }
        if (x<0)
        {
            currentIndex--;
            if(currentIndex < 0)
            {
                currentIndex = 0;
            }
        }
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
                if (icon != null) { icon.ShowIcon(); return; } 
                else
                { 
                    icon = playerRef.GetComponentInChildren<IconToggle>();
                    icon.ShowIcon();
                }

            }

        }
    }



    void OnTriggerExit(Collider perchSphere)
    {
        if (perchSphere.gameObject.CompareTag("Player"))
        {
            playerRef.GetComponentInChildren<IconToggle>().HideIcon();
            if (playerRef != null)
            {
                playerRef = null;
                perchComp = null;
                

            }
        }
    }
}
