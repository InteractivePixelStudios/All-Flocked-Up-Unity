using System;
using UnityEngine;

public class Pooper : MonoBehaviour
{
    [SerializeField] private PoopSystem poopSystem;
    [SerializeField] private PoopFunction poopFunction;
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask poopableLayer;
    [SerializeField] private float maxRange = 20f; //Adjust as needed for gameplay

    //[SeralizeField] private PoopArcRenderer arcRenderer; option for visualizing the arc, not yet implemented

    private bool isAiming = false; // Track if the player is currently aiming


    //We should probably use the new input system for better control, but for not using old input system
    private void Update()
    {
        HandleAimInput();

        if (isAiming)
        {
            //arcRenderer.ShowArc();

            if(Input.GetMouseButtonDown(0)) //Left click to poop
            {
                TryPooping();
            }
            else
            {
                //arcRenderer.HideArc();
            }
        }
    }

    private void HandleAimInput()
    {
        if (Input.GetMouseButtonDown(1)) //Right click
        {
            isAiming = true;
            //show UI reticle or similar aiming UI - temp code added for Arc Renderer above
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;
        }
    }

    private void TryPooping()
    {
        if (poopSystem.TryPoop())
        {
            Vector3 target = GetTarget();
            poopFunction.FirePoop(target);
        }
    }

    private Vector3 GetTarget()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxRange, poopableLayer))
        {
            return hit.point;
        }

        return cam.transform.position + cam.transform.forward * maxRange;
    }

}
