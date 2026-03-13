using System.Threading.Tasks;
using UnityEngine;

public class TrinketScript : MonoBehaviour
{
    [SerializeField] private GameObject playerRef;
    public int value;
    [SerializeField] private bool isKeychain;
    [SerializeField] private bool isPresto;
    [SerializeField] private bool isTrinket;
    private int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
        }
    }

    void SetIndex()
    {
        if (isKeychain)
        {
            index = 1;
        }
        else if (isTrinket)
        {
            index = 2;
        }
        else if (isPresto)
        {
            index = 3;
        }
        else index = 0;
    }

    public void CollectTrinket(int amt)
    {
        SetIndex();
        playerRef.GetComponent<PlayerWingventory>().AddTrinketToInv(amt, index);
        //Q_Collect collect;
        //TryGetComponent<Q_Collect>(out collect);
        //if(collect != null)
        //{
        //    collect.CompleteObjective();
        //}
        //await Task.Delay(500);
        Destroy(this.gameObject);
    }
}
