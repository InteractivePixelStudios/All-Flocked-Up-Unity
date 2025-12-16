using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;

public class AmbienceLogic_Bird : MonoBehaviour
{
    private List<GameObject> treesInArea = new List<GameObject>();
    [SerializeField] EventReference birdChirpEvent; // Assign in Inspector
    [SerializeField] int numberOfBirds = 11; // This is hard coded, I don't like it but it is needed for now.
    [SerializeField] GameObject FMODVisualizerObject; // For testing purposes only.
    private bool isBirdChirping = false;

    public bool freezeScript = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (freezeScript) // TEMPORARY - For testing purposes.
            return;

        if (treesInArea.Count < 1 || isBirdChirping) // No trees in area or bird is already chirping - either way, we're doing nothing.
            return;

        int randomNumber = Random.Range(0, treesInArea.Count); // Random index based on number of trees in area.
        GameObject selectedTree = treesInArea[randomNumber]; // Keep track of which tree was selected.
        Debug.DrawLine(transform.position, selectedTree.transform.position, Color.red, 999f); // Temp for visualization
        MoveVisualizerToTree(selectedTree); // Move visualizer to selected tree position
        Debug.Log("Selected tree for bird chirp: " + selectedTree.name);

        int randomChirpClip = Random.Range(0, numberOfBirds); // Random bird chirp clip.
        Debug.Log("Selected bird chirp clip index: " + randomChirpClip);
        StartCoroutine(PlayBirdChirp(selectedTree, randomChirpClip)); // Play the bird chirp at the
    }

    private void OnTriggerEnter(Collider other) // One time trigger.
    {
        if (other.CompareTag("Tree"))
        {
            if (!treesInArea.Contains(other.gameObject))
            {
                treesInArea.Add(other.gameObject);
                //Debug.Log("Tree entered ambience area. Total trees: " + treesInArea.Count);
                //Debug.DrawLine(transform.position, other.transform.position, Color.green, 999f); // Temp for visualization
            }
        }
    }

    private IEnumerator PlayBirdChirp(GameObject tree, int chirpIndex)
    {
        EventInstance inst = RuntimeManager.CreateInstance(birdChirpEvent);
        Debug.Log("Playing bird chirp at tree: " + tree.name + " with clip index: " + chirpIndex);

        inst.set3DAttributes(RuntimeUtils.To3DAttributes(tree.transform.position));
        inst.setParameterByName("Birds", chirpIndex);
        inst.start();

        isBirdChirping = true;

        // Wait until FMOD reports it is done
        PLAYBACK_STATE state;
        do
        {
            inst.getPlaybackState(out state);
            yield return null;
        }
        while (state != PLAYBACK_STATE.STOPPED);

        inst.release();

        yield return new WaitForSeconds(Random.Range(2f, 4f)); // Small delay before allowing another chirp - low for testing.
        isBirdChirping = false;
    }

    private void MoveVisualizerToTree(GameObject tree)
    {
        Debug.Log("Moving FMOD visualizer to tree: " + tree.name);
        if (FMODVisualizerObject != null && tree != null)
        {
            FMODVisualizerObject.transform.position = tree.transform.position;
            Debug.Log("FMOD visualizer moved to position: " + tree.transform.position);
        }
    }
}
