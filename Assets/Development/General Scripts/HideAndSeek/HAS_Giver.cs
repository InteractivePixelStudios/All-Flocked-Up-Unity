using UnityEngine;

public class HAS_Giver : MonoBehaviour
{
    HAS_Controller controller;
    [SerializeField] HAS_Info infoToGive;
    bool hasGiven;
    bool canRepeat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
