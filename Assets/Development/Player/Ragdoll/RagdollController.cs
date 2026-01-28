using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class RagdollController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private List<Rigidbody> bones = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        Rigidbody[] rbArray = this.GetComponentsInChildren<Rigidbody>();
        foreach(var bone in rbArray)
        {
            bones.Add(bone);
            
        }
        bones.RemoveAt(0);
        foreach( var bone in bones)
        {
            bone.isKinematic = true;
        }
    }
    [ContextMenu("ToggleOn")]
    public async void ToggleRagdollOn()
    {
        foreach(var bone in bones)
        {
            bone.isKinematic = false;
        }
        animator.enabled = false;
        await Task.Delay(3000);
        ToggleRagdollOff();
    }
    [ContextMenu("ToggleOff")]
    public void ToggleRagdollOff()
    {
        foreach (var bone in bones)
        {
            bone.isKinematic = true;
        }
        animator.enabled = true;
    }

    public void OnCollisionEnter(Collision collision)
    {

        if (collision.relativeVelocity.magnitude > 12)
        {
                ToggleRagdollOn();
            
        }
    }
}
