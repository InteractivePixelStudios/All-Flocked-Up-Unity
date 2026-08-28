using UnityEngine;

public class HAS_SpawnLoc : MonoBehaviour
{
    [SerializeField] private string HAS_ID;

    public string GetHASID()
    {
        return HAS_ID;
    }
}
