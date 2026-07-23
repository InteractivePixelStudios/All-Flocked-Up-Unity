using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

enum CanState { Full, InUse, Empty }

public class TrashCanInteraction : MonoBehaviour
{
    [SerializeField] private GameObject trashCanObject;
    [SerializeField] private bool looted;
    [SerializeField] private ParticleSystem trashParticles;
    private Q_SearchTrash questComp;
    private InteractionPrompt prompt;
    [SerializeField] List<ConsumableBase> consumableList = new();
    [SerializeField] private int itemsRemain;
    [SerializeField] private float shootForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("Events")] // Added by IPM on 07/22/2026
    [SerializeField] private UnityEvent OnSearchCanEvent; // Event to trigger when the trash can is searched - for Audio in this case.


    void Start()
    {
        TryGetComponent<Q_SearchTrash>(out questComp);
        TryGetComponent<InteractionPrompt>(out prompt);
    }


    public void InteractWithTrashCan()
    {
        SearchCan();
        OnSearchCanEvent?.Invoke();
    }

    private void SearchCan()
    {
        if (questComp != null)
        {
            questComp.SearchTrash();
            Debug.Log("CompActive");
        }
        looted = true;
        ToggleParticles(looted);
        GiveReward();
    }


    private void ResetCan()
    {
        if (looted)
        {
            looted = false;
            ToggleParticles(looted);
        }
    }

    private void ToggleParticles(bool used)
    {
        if (used)
        {
            trashParticles.Stop();
        }
        else trashParticles.Play();
    }


    private void GiveReward()
    {
        for (int i = itemsRemain; i > 0; i--)
        {
            int rand = Random.Range(0, consumableList.Count - 1);
            var spawned = Instantiate(consumableList[rand], transform.position, transform.rotation);
            spawned.GetComponent<Rigidbody>().AddForce(Vector3.up * shootForce + Vector3.forward, ForceMode.Impulse);
        }
    }


}
