using NUnit.Framework.Constraints;
using System;
using UnityEngine;

public class PerchableObject_Bush : MonoBehaviour, I_Perchable
{
    public GameObject playerRef;
    [SerializeField] private bool isPerching;
    Vector3 offset = new Vector3(0, 1, 0);

    void Update()
    {

        if (isPerching)
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
    }

    public void StopPerch()
    {
        isPerching = false;
    }

    public void UpdatePerch()
    {
        playerRef.transform.position = transform.position+offset;
    }

    public void MovePosition()
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
            }
        }
    }
}
