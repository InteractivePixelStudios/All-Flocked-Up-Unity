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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            bone.constraints = RigidbodyConstraints.FreezePositionZ;
        }
    }
    [ContextMenu("ToggleOn")]
    public async void ToggleRagdollOn()
    {
        ground.enabled = false;
        flight.enabled = false;
        if (ground.GetIsFlying())
        {
            flight.CallReturnToWalk();
        }
        foreach(var bone in bones)
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
        flight.enabled = true;
        foreach (var bone in bones)
        {
            bone.isKinematic = true;
            bone.constraints = RigidbodyConstraints.FreezePositionZ;
        }
        animator.enabled = true;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude > 7)
        {
                ToggleRagdollOn();
            
        }
    }
}
