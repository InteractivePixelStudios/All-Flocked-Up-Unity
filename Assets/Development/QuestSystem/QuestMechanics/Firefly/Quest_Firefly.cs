using UnityEngine;

public class Quest_Firefly : MonoBehaviour, IQuestMechanic
{
    ParticleSystem particles;
    QuestLog questLog;
    [SerializeField]string objectiveID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        questLog = FindAnyObjectByType<QuestLog>();
        particles = GetComponent<ParticleSystem>();
        StopParticles();
    }

    public void StartParticles()
    {
        particles.Play();
    }

    public void StopParticles()
    {
        particles.Stop();
        questLog.UpdateQuestObjective(objectiveID, 1);
    }

    public void GetQuestLog()
    {
        questLog = FindAnyObjectByType<QuestLog>();
    }

    public string GetObjectiveID() => objectiveID;

}
