using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

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
    }
    [ContextMenu("ToggleOn")]
    public void ToggleRagdollOn()
    {
        foreach(var bone in bones)
        {
            bone.isKinematic = false;
        }
        animator.enabled = false;
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
}
