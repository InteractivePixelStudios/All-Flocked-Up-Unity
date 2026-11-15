using NUnit.Framework;
using UnityEngine;

public class NestBase : MonoBehaviour
{
    [Header("BaseClassRefs")]
    private GameObject playerRef;
    [SerializeField] private float activeRadius;
    [SerializeField] private ParticleSystem spawnedVFX;
    public bool isActiveNest;
    private UI_CanvasController canvasController;
    [SerializeField] Material baseMaterial;
    [SerializeField] Material distanceMaterial;



    public void InteractWithNest()
    {
        if (!isActiveNest)
        {
            SpawnVFX();
        }
        else OpenNestMenu();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;

        }

    }

    private void SpawnVFX()
    {
        spawnedVFX.Play();
        if (spawnedVFX.isPlaying)
        {
            spawnedVFX.Stop();
        }
        isActiveNest = true;
    }

    private void OpenNestMenu()
    {
        if (isActiveNest)
        {
            canvasController = FindAnyObjectByType<UI_CanvasController>();
            canvasController.OpenNestMenu();
        }
    }

}
