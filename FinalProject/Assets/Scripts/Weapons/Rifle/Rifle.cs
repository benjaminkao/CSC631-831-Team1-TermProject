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

    public bool CanShoot
    {
        get
        {
            return this.canShoot && !this.reloading;
        }
    }

    private bool canShoot;
    private bool reloading;

    [SerializeField] private Camera tpsCamera;

    private Cinemachine.CinemachineImpulseSource source;
    [SerializeField] private GameObject gunPoint;

    private TextMeshProUGUI ammoLabel;


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
        

        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        source.GenerateImpulse(tpsCamera.transform.forward);
    }


    public GameObject Shoot(Vector3 startPos, Vector3 forward)
    {
        


        return HandleShoot(startPos, forward);

    }

    public void Reload()
    {
        if(reloading)
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

        if(currentAmmo <= 0)
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


        bool gun = Physics.Linecast(gunPoint.transform.position, cameraHit.transform.position, out gunHit);


        // Two Cases:
        // 1. Gun ray hit GameObject that is the same as Camera ray hit
        // 2. Gun ray hit GameObject (Wall/Obstacle) that is not the same as Camera ray hit

        if (!gun)
        {
            return null;
        }


        if (gunHit.transform.gameObject == cameraHit.transform.gameObject)
        {
            return gunHit.transform.gameObject;
        }

        return null;
    }


    private void UpdateAmmoUI()
    {
        this.ammoLabel.text = $"{currentAmmo} / {ammoPerClip}";
    }
}
