using UnityEngine;

public class PaintLocation : MonoBehaviour
{
    [SerializeField]bool readyToPaint;
    [SerializeField]PaintInteraction interaction;

    public bool GetReadyToPaint()
    {
        return readyToPaint;
    }

    private void Start()
    {
        interaction = GetComponentInChildren<PaintInteraction>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            readyToPaint = true;
            interaction.SetPlayerRef(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            readyToPaint = false;
            interaction.SetPlayerRef(null);
        }
    }
}
