using UnityEngine;
public class HAS_Giver : MonoBehaviour
{
    HAS_Controller controller;
    [SerializeField] HAS_Info infoToGive;
    bool hasGiven;
    bool canRepeat;
    void Start()
    {
     controller = FindAnyObjectByType<HAS_Controller>();   
    }

    public void GiveInfo()
    {
        if (controller != null && !hasGiven)
        {
            controller.InitGameInfo(infoToGive);
            hasGiven = true;
        }
    }
}
