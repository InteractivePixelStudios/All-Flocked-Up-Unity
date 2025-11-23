using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class ShopManager : MonoBehaviour
{
    [SerializeField] protected List<ShopItem> shopItems = new();
    [SerializeField] protected Dictionary<GameObject, int> itemDictionary = new();
    [SerializeField] protected List<Transform> slots = new();
    [SerializeField] protected float timerLength;
    [SerializeField] protected float resetTimer;
    [SerializeField] protected bool resetFlag;
    [SerializeField] protected GameObject edgarPrefab;
    [SerializeField] protected Transform edgarTransform;

    protected virtual void Start()
    {

        GetSlots();
        resetTimer = timerLength;
        foreach (var item in shopItems)
        {
            itemDictionary = shopItems.ToDictionary(item => item.gameObject, item => item.cost);
        }
        SpawnEdgar();

    }

    // Update is called once per frame
    void Update()
    {
        ShopResetTimer();
    }

    protected void GetSlots()
    {
        Transform shopRoot = this.transform;
        foreach(Transform child in shopRoot)
        {
            slots.Add(child);
        }
    }



    protected void ShopResetTimer()
    {
        if (resetTimer > 0)
        {
            resetTimer -= Time.deltaTime;
            resetFlag = false;
        }
        else { resetTimer = timerLength; resetFlag = true; }
    }

    protected void SpawnEdgar()
    {
        var edgar = Instantiate(edgarPrefab);
        edgar.transform.position = edgarTransform.position;
    }



}
