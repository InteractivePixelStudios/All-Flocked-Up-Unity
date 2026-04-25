using System.Threading.Tasks;
using UnityEngine;

public class PlayerPeckComponent : MonoBehaviour
{
    [SerializeField] private bool isPecking;
    [SerializeField] private PlayerGroundMovement player;

    private void Start()
    {
        player = GetComponent<PlayerGroundMovement>();
    }

    public async void Peck()
    {
        if(player.GetIsFlying()) return; 
            isPecking = true;
        await Task.Delay(200);
             isPecking = false;
        
    }

    public bool GetIsPecking()
    {
        return isPecking;
    }

}
