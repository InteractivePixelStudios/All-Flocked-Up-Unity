using UnityEngine;
using System.Collections.Generic;

public class StickBuilder : MonoBehaviour
{
    [SerializeField] private int stickCount = 3;
    [SerializeField] private List<StickObject> sticks = new();
    [SerializeField] private float timer = 30f;
    public NestBase nestBaseRef;
    private Transform nestLocation;
    [SerializeField] private float spawnRadius;
    private bool timerComplete = false;
    [SerializeField] private Vector3 offset = new Vector3(0,1,0);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        nestLocation = nestBaseRef.transform;
        timer = 30f;
        stickCount = 3;
        spawnRadius = 10f;
        SpawnSticks();
    }

    // Update is called once per frame
    void Update()
    {
        if (!timerComplete)
        {
            StickTimer();

        }
        else return;
    }

    private void StickTimer()
    {
        timer-=Time.deltaTime;
        if (timer < 0)
        {
            timerComplete = true;
        }
        else return;
    }


    public void SpawnSticks()
    {
        var i = 0;
        foreach(var stick in sticks)
        {
            var newStick = Instantiate(stick, nestBaseRef.transform.position + offset , Quaternion.identity);
            newStick.stickBuilder = this;
            i++;
        }
    }

    public void CollectStick()
    {
        if (sticks.Count <=1)
        {
            BuildNest();            
            Debug.Log("SticksCollected");
        }
        else
        {
            sticks.Remove(sticks[0]);
        }


    }

    private void BuildNest()
    {
        nestBaseRef.isActiveNest = true;
        timerComplete = true;
        Destroy(this.gameObject);
    }

    private void ResetSticks()
    {

    }
}
