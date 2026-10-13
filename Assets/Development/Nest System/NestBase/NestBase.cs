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


    protected void ShowNest()
    {
        gameObject.GetComponent<MeshRenderer>().material = baseMaterial;
    }

    protected void HideNest()
    {
        gameObject.GetComponent<MeshRenderer>().material = distanceMaterial;
    }


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
            //ShowNest();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //HideNest();
        }
    }

    private void SpawnVFX()
    {
        spawnedVFX.Play();
        if (spawnedVFX.isPlaying)
        {
            spawnedVFX.Stop();
        }
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
