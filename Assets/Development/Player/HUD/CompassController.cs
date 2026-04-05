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
            var spawned = Instantiate(iconPrefab,hudController.transform);
            var comp = spawned.GetComponent<RectTransform>();
            mapMarkers.Add(comp);
            await Task.Delay(300);
            spawned.GetComponent<MiniMapIcon>().SetIcon(sprite);

        }
    }

    private void RotateMapMarkers()
    {
        for (int i = 0; i < mapMarkers.Count; i++)
        {
            Transform target = worldTargets[i].transform;
            RectTransform marker = mapMarkers[i];
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
    }

    public void SetQuestTarget(Transform newTarget)
    {
    //    questTarget = newTarget;
    }
}
