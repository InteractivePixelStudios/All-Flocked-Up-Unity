using Unity.VisualScripting;
using UnityEngine;
using System;

public class S_DayNightCycle : MonoBehaviour
{
    
    [Header("Time Settings")]
    [Tooltip("Length of a full day in seconds")]
    public float dayLengthInSeconds = 120f;
    
    
    [Tooltip("0 = start of day, 0.5 = start of night")]
    [Range(0f, 1f)]
    public float timeOfDay = 0f;
    
    [Header("Sun Settings")]
    public Light sunLight; //make sure to assign a directional light

    public float sunBaseRotation = -90f; //offset so 0.25 is noon

    //Events
    public event Action OnDayStart;
    public event Action OnNightStart;
    
    public bool isDay = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        timeOfDay += Time.deltaTime /  dayLengthInSeconds;
        if (timeOfDay >= 1f)
            timeOfDay -= 1f; //wrap back to zero

        float sunAngle = timeOfDay * 360f;
        if (sunLight) //doing a unity
        {
            sunLight.transform.rotation = Quaternion.Euler(sunAngle + sunBaseRotation, 170f, 0f);
        }
        
        //check for transition
        CheckDayNightTransition();

    }

    private void CheckDayNightTransition()
    {
        
        //example: Day = 0.25 -> 0.75
        bool shouldBeDay = timeOfDay is >= 0.25f and <= 0.75f; //note: this is identical to saying "bool shouldBeDay = (timeOfDay >= 0.25f && timeOfDay <= 0.75f);" This is just pattern matching which is a more modern use of C# syntax

        if (shouldBeDay && !isDay)
        {
            isDay = true;
            Debug.Log("Day Starting");
            OnDayStart?.Invoke();
        }else if (!shouldBeDay && isDay)
        {
            isDay = false;
            Debug.Log("Night Starting");
            OnNightStart?.Invoke();
        }
        
        
    }
    
}
