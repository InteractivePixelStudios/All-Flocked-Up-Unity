using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Animations.Rigging;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class CompassController : MonoBehaviour
{
    private Transform player;
    private UI_HudController hudController;
    [SerializeField] private RectTransform compassDial; //player facing dir
    [SerializeField] private List<Sprite> icons = new();
    [SerializeField] private List<RectTransform> mapMarkers = new();
    [SerializeField] private RectTransform northDial;
    private GameObject northObj;
    [SerializeField] private MapIcon[] worldTargets;
    [SerializeField] private float compassRadius = 50f;

    [SerializeField] private GameObject iconPrefab;
    bool spawned;
    Camera mapCamRef;

    //[SerializeField] private Transform questTarget;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hudController = GetComponent<UI_HudController>();
        worldTargets = FindObjectsByType<MapIcon>();
        northObj = GameObject.FindWithTag("North");
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj)
            player = playerObj.transform;
        else
            Debug.LogError("No object tagged Player found!");

       var comp = playerObj.GetComponentInChildren<MapCameraFollow>();
        if(comp != null)
        {
            mapCamRef = comp.GetMapCamera();
        }

    }

   
    
    

    // Update is called once per frame
    void Update()
    {

        //player forward dial
        float playerYaw = player.transform.eulerAngles.y;   
        compassDial.localEulerAngles = new Vector3(0,0, playerYaw);

        //north dial
        float rot = Mathf.DeltaAngle(northObj.transform.eulerAngles.y, player.transform.eulerAngles.y);
        northDial.localEulerAngles = new Vector3(0, 0, rot);

        if (!spawned)
        {
            spawned = true;
            SpawnMapMarkers();
        }else if (spawned)
        {
            RotateMapMarkers();
        }

     
     //Quest marker direction
     //Vector3 toTarget = questTarget.position - player.position
     //toTarget.y = 0 //ignoring height
     //float angle = Vector3.SignedAngle(player.forward, toTarget, Vector3.up);
     //questMarker.localEulerAngles = new Vector3(0,0, -angle);
     
    }

    private async void SpawnMapMarkers()
    {
        foreach (var obj in worldTargets)
        {
            var sprite = obj.GetComponent<MapIcon>().GetCurrentSprite();
            Debug.Log(sprite);
            var spawnedIcon = Instantiate(iconPrefab,hudController.transform);
            var comp = spawnedIcon.GetComponent<RectTransform>();
            mapMarkers.Add(comp);
            await Task.Delay(300);
            spawnedIcon.GetComponent<MiniMapIcon>().SetIcon(sprite);
            spawnedIcon.TryGetComponent<IconRemover>(out var remover);
            if(remover != null)
            {
                remover.SetCompassRef(this);
                remover.SetIconRef(obj);
            }

        }
    }

    private void RotateMapMarkers()
    {
        for (int i = 0; i < mapMarkers.Count; i++)
        {
            Transform target = worldTargets[i].transform;
            RectTransform marker = mapMarkers[i];
            if (IsVisible(target))
            {
                marker.gameObject.SetActive(false);

                continue;
            }
            else
            {
                Debug.Log(IsVisible(target));
                marker.gameObject.SetActive(true);
            }
            Vector3 dir = target.position - player.position;
            dir.y = 0f;
            float angle = Mathf.DeltaAngle(player.eulerAngles.y, Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg);
            Vector2 pos = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * compassRadius;
            marker.anchoredPosition = pos;
            marker.localEulerAngles = new Vector3(0, 0, -angle);
        }
    }

    private void ClearIcons()
    {
        foreach(var marker in mapMarkers)
        {
            Destroy(marker.gameObject);
        }
        mapMarkers.Clear();
    }

    public void RemoveIcon(MapIcon icon)
    {
        for (int i = 0; i < worldTargets.Length; i++)
        {
            if (worldTargets[i] == icon)
            {
                if (i < mapMarkers.Count && mapMarkers[i] != null)
                {
                    Destroy(mapMarkers[i].gameObject);
                    mapMarkers.RemoveAt(i);
                }
                var targetsList = new List<MapIcon>(worldTargets);
                targetsList.RemoveAt(i);
                worldTargets = targetsList.ToArray();

                return;
            }
        }
    }

    public void SetQuestTarget(Transform newTarget)
    {
    //    questTarget = newTarget;
    }

    bool IsVisible(Transform target)
    {
        if (mapCamRef == null) return false;

        Vector3 viewPos = mapCamRef.WorldToViewportPoint(target.position);

        return viewPos.z > 0 &&
               viewPos.x > 0 && viewPos.x < 1 &&
               viewPos.y > 0 && viewPos.y < 1;
    }

}
