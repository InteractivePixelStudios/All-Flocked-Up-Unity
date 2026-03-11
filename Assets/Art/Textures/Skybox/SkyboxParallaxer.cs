using System;
using UnityEngine;

public class SkyboxParallaxer : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Material dayMaterial;
    [SerializeField] private Material nightMaterial;

    void Update()
    {
        RotateSkybox();
    }

    void RotateSkybox()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }

    public void ChangeTimeOfDay(bool isNight)
    {
        if(isNight)
        {
            RenderSettings.skybox = nightMaterial;
        }
        else
        {
            RenderSettings.skybox = dayMaterial;
        }
    }
}
