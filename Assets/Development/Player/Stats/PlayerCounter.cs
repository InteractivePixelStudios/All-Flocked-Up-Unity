using UnityEngine;
using System.Collections.Generic;

public class PlayerCounter :  Counters
{
    public int local_timesPooped;
    public int local_flaps;
    public int local_deaths;
    private Dictionary<string, int> countersToSend = new();

    private void Update()
    {
      GetStats();
    }
    void GetStats()
    {
        local_deaths = deaths;
        local_flaps = flaps;
        local_timesPooped = timesPooped;
        Debug.Log("GetStats");
    }

    void UpdateDictionary()
    {
        countersToSend.Clear();
        countersToSend.Add("Deaths",local_deaths);
        countersToSend.Add("Flaps", local_flaps);
        countersToSend.Add("Times Pooped", local_timesPooped);
        Debug.Log("UpdateDict");

    }
    
    public Dictionary<string,int> SendStats()
    {
        Debug.Log("SendDict");
        UpdateDictionary();
        return countersToSend;
    }
}
