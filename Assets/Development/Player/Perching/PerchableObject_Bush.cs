using NUnit.Framework.Constraints;
using System;
using UnityEngine;

public class PerchableObject_Bush : MonoBehaviour, I_Perchable
{
    public GameObject playerRef;
    [SerializeField] private bool isPerching;
    Vector3 offset = new Vector3(0, 1, 0);
    [SerializeField] IconToggle icon;
    bool jumpCheck => playerRef.GetComponent<PlayerGroundMovement>().GetIsFlying();

    void Update()
    {

        if (isPerching)
        {

            if (jumpCheck)
            {
                StopPerch();
                playerRef.GetComponent<Rigidbody>().linearVelocity = new Vector3(1, 1, 0);
            }
            UpdatePerch();
        }
        else return;
    }

    public void StartPerch()
    {
        isPerching = true;
        playerRef.transform.position = transform.position - offset;
        playerRef.GetComponentInChildren<IconToggle>().HideIcon();
    }

    public void StopPerch()
    {
        isPerching = false;
    }

    public void UpdatePerch()
    {
        playerRef.transform.position = transform.position+offset;
    }

    public void MovePosition(float x)
    {
        //not needed for bush... maybe could think of something later
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(playerRef == null)
            {
                playerRef = other.gameObject;
                playerRef.GetComponentInChildren<IconToggle>().ShowIcon();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerRef != null)
            {
                playerRef = null;
                playerRef.GetComponentInChildren<IconToggle>().HideIcon();
            }
        }
    }
}
