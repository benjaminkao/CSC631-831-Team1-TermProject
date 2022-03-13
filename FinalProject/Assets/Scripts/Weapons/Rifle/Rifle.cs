using UnityEngine;

public class Rifle : MonoBehaviour
{

    public float damage = 10f;
    public float range = 100f;

    [SerializeField] private Camera tpsCamera;

    private Cinemachine.CinemachineImpulseSource source;
    [SerializeField] private GameObject gunPoint;


    private void OnEnable()
    {
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


    // Update is called once per frame
    void Update()
    {

        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }


        
    }


    void Shoot()
    {
        source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        source.GenerateImpulse(tpsCamera.transform.forward);


        RaycastHit cameraHit;
        RaycastHit gunHit;

        int layerMask = 1 << 2;
        layerMask = ~layerMask;

        // Camera: Physics.Raycast(origin, endPoint, hit)
        bool camera = Physics.Raycast(tpsCamera.transform.position, tpsCamera.transform.forward, out cameraHit, range, layerMask);


        if(!camera)
        {
            // Camera ray didn't hit anything, so no need to check gun raycast
            return;
        }


        bool gun = Physics.Linecast(gunPoint.transform.position, cameraHit.transform.position, out gunHit);

        
        // Two Cases:
        // 1. Gun ray hit GameObject that is the same as Camera ray hit
        // 2. Gun ray hit GameObject (Wall/Obstacle) that is not the same as Camera ray hit

        if(!gun)
        {
            return;
        }


        if(gunHit.transform.gameObject == cameraHit.transform.gameObject)
        {
            Enemy enemy = gunHit.transform.gameObject.GetComponent<Enemy>();

            if(enemy == null)
            {
                return;
            }

            enemy.Damage(this.damage);

        }
        

    }
}
