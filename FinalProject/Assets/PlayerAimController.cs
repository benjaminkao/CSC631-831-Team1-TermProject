using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField] private float _mouseSensitivity = 100f;
    [SerializeField] private float yClamp = 90.0f;

    [SerializeField] GameObject _mainCamera;
    [SerializeField] GameObject _aimCamera;
    [SerializeField] GameObject _cameraTarget;

    private float xRotation = 0f;


    // Start is called before the first frame update
    void Start()
    {
        if(_mainCamera == null || _aimCamera == null || _cameraTarget == null)
        {
            Debug.LogError("Please reference the main and aim cameras in PlayerAimController.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        HandleMouseLook(mouseX, mouseY);

        float aim = Input.GetAxis("Fire2");

        // If player is holding down right click, ADS
        HandleADS(aim > 0);
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
