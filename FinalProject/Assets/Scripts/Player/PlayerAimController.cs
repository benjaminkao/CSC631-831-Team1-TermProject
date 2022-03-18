using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerAimController : NetworkBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private float yClamp = 90.0f;

    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject _aimCamera;
    [SerializeField] GameObject _cameraTarget;


    public Transform CameraFollowPoint;
    public Transform MainFollowPoint;
    public Transform AdsFollowPoint;

    public ContainmentPlayerCamera camera;


    private float xRotation = 0f;


    // Start is called before the first frame update
    public override void OnStartClient()
    {
        if(_mainCamera == null || _aimCamera == null || _cameraTarget == null)
        {
            Debug.LogError("Please reference the main and aim cameras in PlayerAimController.");
        }

        if(isLocalPlayer)
        {
            _mainCamera.SetActive(true);
            _aimCamera.SetActive(false);
            _cameraTarget.SetActive(true);
        } else
        {
            _mainCamera.SetActive(false);
            _aimCamera.SetActive(false);
            _cameraTarget.SetActive(false);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Tell camera to follow transform
        camera.SetFollowTransform(CameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        camera.IgnoredColliders.Clear();
        camera.IgnoredColliders.AddRange(GetComponentsInChildren<Collider>());
    }



    // Update is called once per frame
    void Update()
    {
        HandleCameraInput();

        //float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        //HandleMouseLook(mouseX, mouseY);

        //float aim = Input.GetAxis("Fire2");

        //// If player is holding down right click, ADS
        //HandleADS(aim > 0);
    }

    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw("Mouse Y");
        float mouseLookAxisRight = Input.GetAxisRaw("Mouse X");
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }


        // Apply inputs to the camera
        camera.UpdateWithInput(Time.deltaTime, 0f, lookInputVector);

        // Handle toggling zoom level
        if (Input.GetMouseButtonDown(1))
        {
            camera.TargetDistance = (camera.TargetDistance == 0f) ? camera.DefaultDistance : 0f;
        }
    }

    private void HandleADS(bool aiming)
    {
        if (aiming && !_aimCamera.activeInHierarchy)
        {
            _mainCamera.SetActive(false);
            _aimCamera.SetActive(true);
        }
        else if (aiming && !_mainCamera.activeInHierarchy)
        {
            _mainCamera.SetActive(true);
            _aimCamera.SetActive(false);
        }
    }

    private void HandleMouseLook(float mouseX, float mouseY)
    {
        // Rotate entire player around y-axis
        transform.Rotate(Vector3.up * mouseX);


        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -yClamp, yClamp);

        // Rotate the TPSCameraFollowTarget, this will cause the
        // camera to also rotate giving the effect of the camera
        // rotating up and down vertically.
        Quaternion direction = Quaternion.Euler(xRotation, this.transform.rotation.y, this.transform.rotation.z);

        _cameraTarget.transform.localRotation = direction;
    }
}
