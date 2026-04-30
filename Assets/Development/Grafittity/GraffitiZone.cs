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
    [SerializeField] InputAction graffiti;
    [SerializeField] private UI_CanvasController canvasController;

    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera graffitiCam;

    [Header("Graffiti UI Settings")]
    [SerializeField] public Slider colorSlider;
    [SerializeField] public Image targetImage;
    [SerializeField] public Slider sizeSlider;
    [SerializeField] public Image sizeImage;
    [SerializeField] public Canvas graffitiCanvas;

    [Header ("Spray Knowledge")]
    [SerializeField] public bool isSpraying = false;
    [SerializeField] public bool isPainting = false;
    [SerializeField] public bool isErasing = false;
    [SerializeField] InputAction spray;
    [SerializeField] public float sprayDistance = 5f;
    [SerializeField] public int decalSize = 1;
    [SerializeField] public Color currentColor;
    [SerializeField] public GameObject decalPrefab;
    [SerializeField] public LayerMask sprayableLayers;

    //look in to graphics.blit to potentially replace decals

    private Dictionary<Renderer, Texture2D> paintTextures = new Dictionary<Renderer, Texture2D>();

    void Start()
    {
        graffitiCanvas.enabled = false;
        graffitiCam.enabled = false;
        if (TryGetComponent<BoxCollider>(out BoxCollider collider))
        {
            graffitiZoneCollider = collider;
            Debug.Log("BoxCollider component found and assigned to graffitiZoneCollider.");
        }
        else
        {
            Debug.LogError("BoxCollider component not found on GraffitiZone.");
        }

        graffiti = InputSystem.actions.FindAction("Interact");
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
        if (isPainting && isSpraying)
        {
            SprayGraffiti();
        }
    }

    //change color based on slider value
    void UpdateColor(float value)
    {
        float hue = value; // 0 → 1
        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        targetImage.color = rainbowColor;
        currentColor = rainbowColor;
    }

    public void SetEraserMode()
    {
        if (!isErasing)
        {
            isErasing = true;
            currentColor = new Color(0, 0, 0, 0); // Transparent color for erasing
        }
        else
        {
            isErasing = false;
            UpdateColor(colorSlider.value); // Reset to current color if not erasing
        }
    }

    void UpdateSize(float value)
    {
        decalSize = (int)value;
        sizeImage.rectTransform.localScale = Vector3.one * (0.5f + (value / 20f)); // Scale between 0.5 and 1.5 based on slider value
    }

    void PlayerInput()
    {
        graffiti.started += ctx => EnableGraffitiTime();
        spray.performed += ctx => SwitchFlipReverseLoL();
    }

    void SwitchFlipReverseLoL()
    {
        if (isSpraying)
        {
            isSpraying = false;
            Debug.Log("Stopped spraying.");

        }
        else
        {
            isSpraying = true;
            Debug.Log("Started spraying.");
        }
    }

    public void EnableGraffitiTime()
    {
        if (playerInMe && !isPainting)
        {
            canvasController.ShowPlayerCursor();
            graffitiCanvas.enabled = true;
            Debug.Log("Player is interacting with the graffiti zone.");
            // Implement graffiti interaction logic here

            graffitiCam.enabled = true;
            mainCam.enabled = false;
            isPainting = true;
        }
        else
        {
            canvasController.HidePlayerCursor();
            graffitiCanvas.enabled = false;
            Debug.Log("Player stopped interacting with the graffiti zone.");
            graffitiCam.enabled = false;
            mainCam.enabled = true;
            isPainting = false;
        }
    }

    void SprayGraffiti()
    {
        Ray ray = graffitiCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit, sprayDistance, sprayableLayers))
            return;
        Debug.Log("Hit: " + hit.collider.name);
        // Check if hit decal
        GraffitiSpray decal = hit.collider.GetComponent<GraffitiSpray>();
        if (decal != null)
        {
           // Debug.Log("Hit existing decal, painting on it.");
           // decal.Paint(new Vector2(0.5f, 0.5f), Color.red, 10);
           DecalProjector proj = decal.GetComponent<DecalProjector>();
           
           Vector3 localPoint = decal.transform.InverseTransformPoint(hit.point);
           Vector2 uv = new Vector2(
               (localPoint.x / proj.size.x) + 0.5f,
               (localPoint.y / proj.size.y) + 0.5f
           );
         //  Debug.Log($"Calculated UV: {uv}");
           decal.Paint(uv, currentColor, decalSize);
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
