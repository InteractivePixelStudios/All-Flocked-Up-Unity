using System.Threading.Tasks;
using UnityEngine;

public class PlayerPeckComponent : MonoBehaviour
{
    [SerializeField] private bool isPecking;

    public async void Peck()
    {

            isPecking = true;
        await Task.Delay(200);
             isPecking = false;
        
    }

    public bool GetIsPecking()
    {
        return isPecking;
    }

}
