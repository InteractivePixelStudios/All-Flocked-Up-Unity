using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class GraffitiZone : MonoBehaviour
{
    [Header("Graffiti Zone Settings")]
    [SerializeField] private BoxCollider graffitiZoneCollider;
    [SerializeField] private bool playerInMe = false;
    [SerializeField] InputAction grafit;
    [SerializeField] private UI_CanvasController canvasController;

    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera grafittiCam;

    [Header("Graffiti UI Settings")]
    [SerializeField] public Slider colorSlider;
    [SerializeField] public Image targetImage;
    [SerializeField] public Slider sizeSlider;
    [SerializeField] public Image SizeImage;

    [Header ("Spray Knowledge")]
    [SerializeField] InputAction spray;
    [SerializeField] public float sprayDistance = 5f;
    [SerializeField] public float decalSize = 0.3f;
    [SerializeField] public Color currentColor;
    [SerializeField] public bool isPainting = false;
    [SerializeField] public GameObject decalPrefab;
    [SerializeField] public LayerMask sprayableLayers;

    //look in to graphics.blit to potentially replace decals

    private Dictionary<Renderer, Texture2D> paintTextures = new Dictionary<Renderer, Texture2D>();

    void Start()
    {
        grafittiCam.enabled = false;
        if (TryGetComponent<BoxCollider>(out BoxCollider collider))
        {
            graffitiZoneCollider = collider;
            Debug.Log("BoxCollider component found and assigned to graffitiZoneCollider.");
        }
        else
        {
            Debug.LogError("BoxCollider component not found on GraffitiZone.");
        }

        grafit = InputSystem.actions.FindAction("Interact");
        spray = InputSystem.actions.FindAction("Click");

        PlayerInput();

        canvasController = FindFirstObjectByType<UI_CanvasController>();
        if (canvasController == null)
        {
            Debug.LogError("UI_CanvasController not found in the scene.");
        }
        else
        {
            Debug.Log("UI_CanvasController found and assigned to canvasController.");
        }

        colorSlider.onValueChanged.AddListener(UpdateColor);
        sizeSlider.onValueChanged.AddListener(UpdateSize);
        UpdateColor(colorSlider.value);
        UpdateSize(sizeSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //change color based on slider value
    void UpdateColor(float value)
    {
        float hue = value; // 0 → 1
        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        targetImage.color = rainbowColor;
        currentColor = rainbowColor;
    }

    void UpdateSize(float value)
    {
        decalSize = (int)value;
        SizeImage.rectTransform.sizeDelta = new Vector2(decalSize * 2, decalSize * 2);
    }

    void PlayerInput()
    {
        grafit.started += ctx => GrafittiTime();
        spray.performed += ctx => GraffitiSpray();
    }

    void GrafittiTime()
    {
        if (playerInMe)
        {
            canvasController.SetUIMap();
            Debug.Log("Player is interacting with the graffiti zone.");
            // Implement graffiti interaction logic here

            grafittiCam.enabled = true;
            mainCam.enabled = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            isPainting = true;
        }
    }

    void GraffitiSpray()
    {
        if (isPainting == true)
        {
            Ray ray = grafittiCam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, sprayDistance, sprayableLayers))
                return;
            Debug.Log("Hit: " + hit.collider.name);
            SpawnDecal(hit);
        }
    }

    void SpawnDecal(RaycastHit hit)
    {
        // Align projector to surface
        Quaternion rotation = Quaternion.LookRotation(-hit.normal);

        GameObject decalObj = Instantiate(
            decalPrefab,
            hit.point + hit.normal * 0.02f,
            rotation
        );

        var projector = decalObj.GetComponent<DecalProjector>();

        if (projector != null)
        {
            // Randomize size for spray feel
            float size = Random.Range(decalSize * 0.8f, decalSize * 1.2f);

            projector.size = new Vector3(size, size, size);

            // Create unique material instance
            projector.material = new Material(projector.material);
            projector.material.color = currentColor;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the graffiti zone.");
            //pop up UI here
            playerInMe = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited the graffiti zone.");
            //close UI here
            playerInMe = false;
        }
    }
}
