using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Net;
using UnityEditor.Experimental.GraphView;

public class AI_Carlos : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Vector3 lookOffset = new Vector3(-90, 0, 0);
    [SerializeField] private bool playerDetected;
    [SerializeField] private float lockTimer = 3f;
    [SerializeField] private bool aimLocked;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount;
    [SerializeField] private int bulletIndex;
    [SerializeField] private List<GameObject> bulletPool;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject gunSpawnPoint;
    [SerializeField] private GameObject spawnedGun;
    [SerializeField] private float shootDistance;
    [SerializeField] private Vector3 shootForce;
    [SerializeField] private float shootTimer = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        for(int i = 0; i < bulletCount; i++)
        {
            bulletPrefab = Instantiate(bulletPrefab,bulletSpawnPoint.transform.position,bulletSpawnPoint.transform.rotation);
            bulletPool.Add(bulletPrefab);
            bulletPrefab.SetActive(false);
            bulletIndex++;
        }
    }
   
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            DetectPlayer();
            LookAtPlayer();
        }
    }


    private void DetectPlayer()
    {
        targetPos = player.transform.position + lookOffset;
    }
    private void LookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(targetPos);
        transform.LookAt(targetPos);
        if(rotation.eulerAngles.x > 0)
        {
            lockTimer -= Time.deltaTime;
            if (lockTimer > 0) { aimLocked = true; PullOutGun(); AimAtPlayer(); } else { aimLocked = false; lockTimer = 3f; }
        }

    }

    private void PullOutGun()
    {
        if (spawnedGun == null)
        {
            spawnedGun = Instantiate(gunPrefab, gunSpawnPoint.transform.position, gunSpawnPoint.transform.rotation,gunSpawnPoint.transform);
        }
        else return;
    }

    private void AimAtPlayer()
    {
        if (shootTimer >= 0) { shootTimer -= Time.deltaTime; }
        if (player!=null && aimLocked && spawnedGun!=null)
        {
            var direction = targetPos - spawnedGun.transform.position;
            spawnedGun.transform.LookAt(direction);
            if (bulletPool.Count > 0 && shootTimer<=0)
            {
                Shoot();
                shootTimer = 2;
            }
            else if (bulletPool.Count<= 0)
            {
                ReloadPool();
                shootTimer = 5; //longer for reload
            }
        }
    }

    private void Shoot()
    {
        if (player != null && aimLocked && gunPrefab != null && bulletIndex >= 0)
        {
            bulletPool[bulletIndex - 1].gameObject.SetActive(true);
            Vector3 offset = new Vector3(0, 0, -1);
            bulletPool[bulletIndex - 1].gameObject.GetComponent<Rigidbody>().AddForceAtPosition(shootForce, bulletPool[bulletIndex - 1].gameObject.transform.position - offset);
            bulletPool.RemoveAt(bulletIndex - 1);
            bulletIndex--;
            
        }
        
    }

    private void HitReact()
    {

    }

    private void ReloadPool()
    {
        for (int i = 0; i < bulletCount; i++)
        {
            bulletPrefab = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position,bulletSpawnPoint.transform.rotation);
            bulletPool.Add(bulletPrefab);
            bulletPrefab.SetActive(false);
            bulletIndex++;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            playerDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            playerDetected = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Poop"))
        {
            HitReact();
        }
    }
}
