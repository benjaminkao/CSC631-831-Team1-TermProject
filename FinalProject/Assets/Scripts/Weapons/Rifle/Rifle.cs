using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;
    public float shootDelay = 0.5f;

    public bool canShoot;

    [SerializeField] private Camera tpsCamera;

    private Cinemachine.CinemachineImpulseSource source;
    [SerializeField] private GameObject gunPoint;


    void OnEnable()
    {
        canShoot = true;
        tpsCamera = Camera.main;
        if(tpsCamera == null)
        {
            Debug.LogError("TPS Camera not referenced.");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
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


    IEnumerator ShootDelay()
    {
        canShoot = false;

        yield return new WaitForSeconds(shootDelay);
        Debug.Log("Can shoot");
        canShoot = true;
    } 



    private GameObject HandleShoot(Vector3 startPos, Vector3 forward)
    {
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
}
