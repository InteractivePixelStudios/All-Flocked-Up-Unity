using UnityEngine;
using UnityEngine.Rendering;

public class CompassController : MonoBehaviour
{
    private Transform player;
 
    [SerializeField] private RectTransform compassDial; //North pointing compass
    //[SerializeField] private RectTransform questMarker; //quest pointing compass
    
    //[SerializeField] private Transform questTarget;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj)
            player = playerObj.transform;
        else
            Debug.LogError("No object tagged Player found!");

    }

   
    
    

    // Update is called once per frame
    void Update()
    {
        
        //north compass
     float playerYaw = player.transform.eulerAngles.y;   
     compassDial.localEulerAngles = new Vector3(0,0, playerYaw);
     Debug.Log(compassDial.localEulerAngles);
     
     //Quest marker direction
     //Vector3 toTarget = questTarget.position - player.position
     //toTarget.y = 0 //ignoring height
     //float angle = Vector3.SignedAngle(player.forward, toTarget, Vector3.up);
     //questMarker.localEulerAngles = new Vector3(0,0, -angle);
     
    }
    
    public void SetQuestTarget(Transform newTarget)
    {
    //    questTarget = newTarget;
    }
}
