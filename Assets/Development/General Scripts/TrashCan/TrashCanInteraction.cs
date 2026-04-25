using UnityEngine;
using System.Collections.Generic;

enum CanState {Full, InUse, Empty}

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
    void Start()
    {
        TryGetComponent<Q_SearchTrash>(out questComp);
        TryGetComponent<InteractionPrompt>(out prompt);
    }


    public void InteractWithTrashCan()
    {
            SearchCan();
            Debug.Log("interacted");   
    }

    private void SearchCan()
    {
        looted = true;
        ToggleParticles(looted);
        GiveReward();
        if (questComp != null)
        {
            questComp.SearchTrash();
        } 
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
        for(int i = itemsRemain; i>0;i--)
        {
            int rand = Random.Range(0, consumableList.Count-1);
            var spawned = Instantiate(consumableList[rand],transform.position,transform.rotation);
            spawned.GetComponent<Rigidbody>().AddForce(Vector3.up * shootForce + Vector3.forward, ForceMode.Impulse) ;
        }
    }

    
}
