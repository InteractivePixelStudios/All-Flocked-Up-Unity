using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PaintInteraction : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private PaintLocation paintLocation;
    [SerializeField] private GameObject playerRef;
    [SerializeField] private PlayerStateController playerStateController;
    [SerializeField] private CinemachineCamera camRef;
    [SerializeField] private GameObject playerLoc;
    [SerializeField] bool isplayerLocked;
    [SerializeField] private GameObject camLoc;
    [SerializeField] GameObject camViewPoint;
    [SerializeField] bool isCamLocked;
    [SerializeField]private PaintDecal projector;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasController = FindAnyObjectByType<UI_CanvasController>();
    }

    private void Update()
    {
        if (isplayerLocked)
        {
            playerRef.transform.position = playerLoc.transform.position;
        }
        if(isCamLocked && camRef != null)
        {
            camRef.ForceCameraPosition(camLoc.transform.position, camLoc.transform.rotation);
        }
    }

    public PaintDecal GetDecal()
    {
        return projector;
    }
    public void OpenPainter()
    {
        if (!paintLocation.GetReadyToPaint()) { return; }
        if (canvasController != null)
        {
            canvasController.OpenPaint(this, GetDecal());
            StartPainting();
        }
    }
    public void SetPlayerRef(GameObject player)
    {
        playerRef = player;
        if (playerRef == null) { return; }
        playerStateController = playerRef.GetComponent<PlayerStateController>();
        GetCamRef();
    }

    private void GetCamRef()
    {
        if (camRef == null)
        {
            var found = FindAnyObjectByType<CinemachineCamera>();
            if(found != null && found.CompareTag("Player")) { camRef = found; }
        }
    }

    private void LockPlayer()
    {
        if (playerRef == null || playerStateController == null) return;
        isplayerLocked = true;

    }

    private void UnlockPlayer()
    {
        if (playerRef == null || playerStateController == null) return;
        isplayerLocked = false;
    }

    private void LockCam()
    {
        if(camRef == null || playerStateController == null) { return; }
        isCamLocked = true;
        camRef.LookAt = camViewPoint.transform;
    }

    private void UnlockCam()
    {
        if (camRef == null || playerStateController == null) { return; }
        isCamLocked = false;
        camRef.LookAt = playerRef.transform;
    }

    private void StartPainting()
    {
        playerStateController.SwitchToUIMap();
        LockPlayer();
        LockCam();
    }

    public void StopPainting()
    {
        playerStateController.SwitchToPlayerMap();
        UnlockPlayer();
        UnlockCam();
    }

}
