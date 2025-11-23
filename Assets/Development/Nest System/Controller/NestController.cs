using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class NestController : MonoBehaviour
{
    [SerializeField] private LargeNest mainNest;
    [SerializeField] private Dictionary<SmallNest, bool> smallNests = new();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetSmallNestObjects();
    }


    public void ActivateMainNest()
    {
        if (!mainNest.isActiveNest)
        {

        }
    }

    private void GetSmallNestObjects()
    {
        SmallNest[] nestList=Object.FindObjectsByType<SmallNest>(FindObjectsSortMode.None) as SmallNest[];
        foreach(var nest in nestList)
        {
            smallNests.Add(nest, nest.isActiveNest);
        }
    }

    public void ActiveSmallNestObject()
    {

    }
}
