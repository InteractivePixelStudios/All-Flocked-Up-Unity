using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.ProBuilder.MeshOperations;
using static UnityEditor.Rendering.MaterialUpgrader;

public class UI_QuestTracker : MonoBehaviour
{
    [SerializeField] GameObject questTrackerCanvas;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    private LocalizedString objName;
    private LocalizedString objDesc;
    int objIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrackCurrentQuest();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTracker(string questID, LocalizedString objName, LocalizedString objDesc, int index)
    {
        objName = new LocalizedString
        {
            TableReference = "AFU_Quest",
            TableEntryReference = questID+"_ObjName_"+index
        };

        objDesc = new LocalizedString
        {
            TableReference = "AFU_Quest",
            TableEntryReference = questID + "_ObjDesc_" + index
        };
        objName.StringChanged += name => nameText.text = name;
        objDesc.StringChanged += desc => descText.text = desc;

            
    }

    public void TrackCurrentQuest()
    {
        questTrackerCanvas.SetActive(true);
    }

    public void RemoveTracker()
    {
        Destroy(questTrackerCanvas);
    }
}
