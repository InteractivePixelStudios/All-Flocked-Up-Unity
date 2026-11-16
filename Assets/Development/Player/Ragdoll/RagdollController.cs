using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.Threading.Tasks;
using Steamworks;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<Rigidbody> bones = new();
    PlayerFlightMovement flight;
    PlayerGroundMovement ground;
    [SerializeField] float relVelMagnitudeThresh;
    [SerializeField] string[] collisionLayers = { "Ground", "Prop&Building", "Perchable" };
    void Start()
    {
        animator = GetComponent<Animator>();
        flight = GetComponent<PlayerFlightMovement>();
        ground = GetComponent<PlayerGroundMovement>();
        Rigidbody[] rbArray = this.GetComponentsInChildren<Rigidbody>();
        foreach(var bone in rbArray)
        {
            bones.Add(bone);           
        }
        bones.RemoveAt(0);
        foreach( var bone in bones)
        {
            bone.isKinematic = true;
            bone.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }
    [ContextMenu("ToggleOn")]
    public async void ToggleRagdollOn()
    {
        ground.enabled = false;
        foreach (var bone in bones)
        {
            bone.isKinematic = false;
            bone.constraints = RigidbodyConstraints.None;
        }        
        animator.enabled = false;
        await Task.Delay(3000);
        ToggleRagdollOff();
        
    }
    [ContextMenu("ToggleOff")]
    public void ToggleRagdollOff()
    {
        ground.enabled = true;
        foreach (var bone in bones)
        {
            bone.isKinematic = true;
            bone.constraints = RigidbodyConstraints.FreezePositionY;
        }
        animator.enabled = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > relVelMagnitudeThresh && collision.gameObject.layer == LayerMask.GetMask(collisionLayers))
        {
                ToggleRagdollOn();        
        }
    }
}
