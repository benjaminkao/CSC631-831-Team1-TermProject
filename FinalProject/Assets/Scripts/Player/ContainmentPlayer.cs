using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Mirror;

public class ContainmentPlayer : NetworkBehaviour, ITargetable
{

    public ContainmentPlayerController Character;
    public ContainmentPlayerCamera CharacterCamera;


    public bool Downed
    {
        get
        {
            return this.health.IsDown;
        }

        set
        {
            this.health.IsDown = value;
        }
    }

    public bool Died
    {
        get
        {
            return this.health.Died;
        }

        set
        {
            this.health.Died = value;
        }
    }

    public int Points
    {
        get
        {
            return this._points;
        }
    }




    [SerializeField] private Rifle gun;


    [SerializeField] private PlayerHealth health;

    [SerializeField] private PlayerPoints playerPoints;

    private int _points;
    private bool _ready;
    private bool _inPreparationPhase;

    private bool _walking;
    private bool _running;


    private const string MouseXInput = "Mouse X";
    private const string MouseYInput = "Mouse Y";
    private const string MouseScrollInput = "Mouse ScrollWheel";
    private const string HorizontalInput = "Horizontal";
    private const string VerticalInput = "Vertical";


    public static event Action<ContainmentPlayer> OnPlayerDown;
    public static event Action<ContainmentPlayer> OnPlayerDeath;

    public static event Action<ContainmentPlayer> OnPlayerJoin;
    public static event Action<ContainmentPlayer> OnPlayerLeave;

    public static event Action<ContainmentPlayer> OnPlayerReady;

    public override void OnStartClient()
    {
        this._walking = false;
        this._running = false;

        if(!isLocalPlayer)
        {
            return;
        }


        // Find the Main Camera
        CharacterCamera = GameObject.Find("PlayerCamera").GetComponent<ContainmentPlayerCamera>();

        Cursor.lockState = CursorLockMode.Locked;

        Character.CameraFollowPoint.position = Character.MainFollowPoint.position;

        // Tell camera to follow main camera follow point transform
        CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

        // Ignore the character's collider(s) for camera obstruction checks
        CharacterCamera.IgnoredColliders.Clear();
        CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());



        // Set what Player the health is for
        health.Player = this;
        health.HealthBarUI = GameObject.Find("HUD - Roaming").transform.Find("HealthBarCanvas").Find("HealthBar").GetComponent<HealthBar>();

    }


    private void OnEnable()
    {
        OnPlayerJoin?.Invoke(this);
        this.RegisterTargetable();


        WaveSpawner.OnWaveCompleted += HandleWaveCompleted;
        WaveSpawner.OnWaveStart += HandleWaveStart;
    }

    private void OnDisable()
    {
        this.DeregisterTargetable();

        WaveSpawner.OnWaveCompleted -= HandleWaveCompleted;
        WaveSpawner.OnWaveStart -= HandleWaveStart;
    }


    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        if (Input.GetButton("Fire1"))
        {
            if (gun.CanShoot)
            {

                gun.ClientShoot();

                CmdShoot(Camera.main.transform.position, Camera.main.transform.forward);
            }
        }

        if(Input.GetButtonDown("Reload"))
        {
            gun.Reload();
        }

        if(this._inPreparationPhase && !this._ready && Input.GetKeyDown(KeyCode.Tab))
        {
            ReadyUp();
        }

        HandleCharacterInput();
    }

    private void LateUpdate()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        HandleCameraInput();
    }

    #region ITargetable Methods

    public void Damage(float damage)
    {
        //Debug.Log("Player hit");

        health.alterHealth(-damage);

        NetworkIdentity playerIdentity = this.GetComponent<NetworkIdentity>();


        TargetUpdatePlayerHealth(playerIdentity.connectionToClient, this.health.HealthValue);

        if(health.IsDown)
        {
            // Handle Player Down

            OnPlayerDown?.Invoke(this);
        }

        if(health.Died)
        {
            // Handle Player Death

            OnPlayerDeath?.Invoke(this);
        }
    }

    public GameObject GetTargetPosition()
    {
        return this.gameObject;
    }

    #endregion ITargetable Methods

    public void AddPoints(int points)
    {
        this._points += points;
    }

    public void SubtractPoints(int points)
    {
        this._points -= points;
    }


    #region Mirror Remote Actions

    [Command]
    void CmdShoot(Vector3 startPos, Vector3 forward)
    {
        GameObject targetHit = gun.Shoot(startPos, forward);

        if (targetHit == null)
        {
            return;
        }


        Enemy enemy = targetHit.gameObject.GetComponent<Enemy>();

        if (enemy == null)
        {
            return;
        }

        // Update Enemy Health on Server
        enemy.Damage(this, gun.damage);



        enemy.RpcUpdateHealth(enemy.Health.HealthValue);
    }

    [TargetRpc]
    void TargetUpdatePlayerHealth(NetworkConnection target, float healthValue)
    {
        this.health.SetHealth(healthValue);
    }





    #endregion Mirror Remote Actions




    private void HandleWaveCompleted()
    {
        this._inPreparationPhase = true;
        this._ready = false;
    }

    private void HandleWaveStart()
    {
        this._inPreparationPhase = false;
        this._ready = false;
    }

    private void ReadyUp()
    {
        this._ready = true;
        OnPlayerReady?.Invoke(this);
    }













    private void HandleCameraInput()
    {
        // Create the look input vector for the camera
        float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
        float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
        Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

        // Prevent moving the camera while the cursor isn't locked
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            lookInputVector = Vector3.zero;
        }

        // Input for zooming the camera (disabled in WebGL because it can cause problems)
        //float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

        // Apply inputs to the camera
        CharacterCamera.UpdateWithInput(Time.deltaTime, 0f, lookInputVector);

        // Handle toggling zoom level
        if (Input.GetMouseButton(1))
        {
            ToggleAds(true);
        } else
        {
            ToggleAds(false);
        }
    }


    private void ToggleAds(bool aiming)
    {
        // Adjust the distance of the camera from the character
        CharacterCamera.TargetDistance = aiming ? CharacterCamera.AdsDistance : CharacterCamera.DefaultDistance;

        // Lerp the CharacterCamera's follow point between the Main and ADS Follow Points
        Character.CameraFollowPoint.position = Vector3.Lerp(Character.CameraFollowPoint.position, aiming ? Character.AdsFollowPoint.position : Character.MainFollowPoint.position, Time.deltaTime * CharacterCamera.AdsSpeed);
    }


    private void HandleCharacterInput()
    {
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

        // Build the CharacterInputs struct
        characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
        characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
        characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
        characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
        characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
        characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);
        characterInputs.Sprinting = Input.GetKey(KeyCode.LeftShift);

        // Apply inputs to character
        Character.SetInputs(ref characterInputs);
    }
}
