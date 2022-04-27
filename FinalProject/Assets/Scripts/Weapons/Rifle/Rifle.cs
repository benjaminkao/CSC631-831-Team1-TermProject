using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rifle : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float shootDelay = 0.5f;
    public float reloadTime = 1f;
    public int ammoPerClip = 30;
    public int currentAmmo;

    [SerializeField] private bool automaticReload;

    [Header("Audio")]
    [SerializeField] private RifleAudio rifleAudio;

    public bool CanReload
    {
        get
        {
            return !reloading && currentAmmo < ammoPerClip;
        }
    }

    public bool CanShoot
    {
        get
        {
            return this.canShoot && !this.reloading;
        }
    }

    public bool HasAmmo
    {
        get
        {
            return currentAmmo > 0;
        }
    }

    private bool canShoot;
    private bool reloading;

    [SerializeField] private Camera tpsCamera;

    private Cinemachine.CinemachineImpulseSource source;
    [SerializeField] private GameObject gunPoint;

    private TextMeshProUGUI ammoLabel;


    [Header("Debug")]
    [Tooltip("If debug is off, no raycast tracing or primitive sphere spawning will be done.")]
    [SerializeField] private bool debug;
    [Tooltip("Spawn a sphere at the point where the camera raycast hits.")]
    [SerializeField] private bool spawnSphereAtCameraRaycastPoint;
    [Tooltip("Trace the camera raycast and the gun raycast.")]
    [SerializeField] private bool showRaycastTraces;


    void Start()
    {
        ammoLabel = GameObject.Find("HUD - Roaming").transform.Find("AmmoLabel").GetComponent<TextMeshProUGUI>();

        UpdateAmmoUI();
    }


    void OnEnable()
    {
        canShoot = true;
        reloading = false;
        currentAmmo = ammoPerClip;
        tpsCamera = Camera.main;
        if (tpsCamera == null)
        {
            Debug.LogError("TPS Camera not referenced.");
        }

        UpdateAmmoUI();
    }

    public void ClientShoot()
    {

        StartCoroutine(ShootDelay());
        
    }

    public void ClientPlayShoot()
    {
        rifleAudio.PlayRifleShot();
    }


    public GameObject Shoot(Vector3 startPos, Vector3 forward)
    {
        


        return HandleShoot(startPos, forward);

    }

    public void Reload()
    {
        if(!CanReload)
        {
            // Don't need to reload if already reloading
            return;
        }

        StartCoroutine(ReloadDelay());
    }


    IEnumerator ShootDelay()
    {
        canShoot = false;

        yield return new WaitForSeconds(shootDelay);

        UpdateAmmoUI();

        canShoot = true;
    } 

    IEnumerator ReloadDelay()
    {
        reloading = true;
        

        Debug.Log("Reloading");

        yield return new WaitForSeconds(reloadTime);

        

        currentAmmo = ammoPerClip;

        UpdateAmmoUI();
        reloading = false;
    }


    private GameObject HandleShoot(Vector3 startPos, Vector3 forward)
    {
        currentAmmo--;
        
        rifleAudio.PlayRifleShot();

        rifleAudio.SetRifleAmmoRTPC(currentAmmo);



        if (automaticReload && currentAmmo <= 0)
        {
            StartCoroutine(ReloadDelay());
        }

        RaycastHit cameraHit;
        RaycastHit gunHit;

        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        // Camera: Physics.Raycast(origin, endPoint, hit)
        bool camera = Physics.Raycast(startPos, forward, out cameraHit, range, layerMask);


        if (!camera)
        {
            // Camera ray didn't hit anything, so no need to check gun raycast
            return null;
        }

        
        bool gun = Physics.Linecast(gunPoint.transform.position, cameraHit.point, out gunHit);


        // View Debug Raycasts
        if (debug) {
            if (spawnSphereAtCameraRaycastPoint)
            {
                var tmp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tmp.transform.position = cameraHit.point;
            }
            if (showRaycastTraces) {
                Debug.DrawRay(startPos, forward * range, Color.blue, 2f);
                Debug.DrawLine(gunPoint.transform.position, cameraHit.point, Color.red, 2f);
            }

        }

        // Two Cases:
        // 1. Gun ray hit GameObject that is the same as Camera ray hit
        // 2. Gun ray hit GameObject (Wall/Obstacle) that is not the same as Camera ray hit

        if (!gun)
        {
            return null;
        }

        Debug.Log(gunHit.transform.gameObject.name);


        if (gunHit.transform.gameObject == cameraHit.transform.gameObject)
        {
            
            return gunHit.transform.gameObject;
        }

        return null;
    }


    private void UpdateAmmoUI()
    {
        if(this.ammoLabel == null)
        {
            return;
        }

        this.ammoLabel.text = $"{currentAmmo} / {ammoPerClip}";
    }
}
