using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using Mirror;


public enum ContainmentPlayerVoiceLine
{
    SHIELDBEACONDAMAGED,
    SHIELDBEACONLOW,
    RELOADING,
    DAMAGED,
    BOSS,
    ROUNDENDING
}



public class ContainmentPlayer : NetworkBehaviour, ITargetable
{
    [Header("Player Components")]
    public ContainmentPlayerController Character;
    public ContainmentPlayerCamera CharacterCamera;

    
    public int PlayerNum
    {
        get { return this._playerNum;  }

        // Only the server should be allowed to assign a playerNum to the player.
        set
        {
            if(!isServer)
            {
                return;
            }

            this._playerNum = value;
        }
    }

    [SerializeField]
    private int _playerNum;


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



    [SerializeField] private Rifle gun;


    [Header("Health")]
    [SerializeField] private PlayerHealth health;

    [Tooltip("How long the player doesn't get hit for before health regeneration starts.")]
    [SerializeField] private float healthRegenCooldown;

    [Tooltip("How much the player regens health after every second.")]
    [SerializeField] private float healthRegenRate;
    private float lastDamaged;
    private bool _canRegen;




    [Header("Ready Up")]
    [SerializeField]
    [SyncVar]
    private bool _readyNextWave;

    [SerializeField]
    [SyncVar]
    private bool _inPreparationPhase;


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

    public override void OnStartAuthority()
    {
        Debug.Log(hasAuthority);
        if(!hasAuthority)
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

    public override void OnStartServer()
    {
        if(GameManager.Instance.GameState == GameState.NOTSTARTED || GameManager.Instance.GameState == GameState.PREPARATION)
        {
            this._inPreparationPhase = true;
            this._readyNextWave = false;
        } else
        {
            this._inPreparationPhase = false;
            this._readyNextWave = false;
        }

        lastDamaged = 0;
        _canRegen = true;
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
        if (isServer)
        {
            if (ShouldRegenerateHealth())
            {
                StartCoroutine(RegenerateHealth());
            }
        }
        Debug.Log(hasAuthority);

        if (!hasAuthority)
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

        if(this._inPreparationPhase && !this._readyNextWave && Input.GetKeyDown(KeyCode.Tab))
        {
            ReadyUp();
        }

        HandleCharacterInput();
    }

    private void LateUpdate()
    {
        if(!hasAuthority)
        {
            return;
        }

        HandleCameraInput();
    }

    #region ITargetable Methods

    public void Damage(float damage)
    {
        lastDamaged = Time.time;

        //Debug.Log("Player hit");

        health.alterHealth(-damage);


        RpcUpdatePlayerHealth(this.health.HealthValue);

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

    public void NotifyShieldBeaconDamaged()
    {
        if(!isServer)
        {
            return;
        }

        TriggerVoiceLine(ContainmentPlayerVoiceLine.SHIELDBEACONDAMAGED);
        
    }




    #region Mirror Remote Actions

    [Command]
    private void ReadyUp()
    {
        this._readyNextWave = true;
        OnPlayerReady?.Invoke(this);
    }

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

    [ClientRpc]
    void RpcUpdatePlayerHealth(float healthValue)
    {
        this.health.SetHealth(healthValue);
    }


    [ClientRpc]
    void TriggerVoiceLine(ContainmentPlayerVoiceLine voiceLine)
    {

        //switch(voiceLine)
        //{
        //    case ContainmentPlayerVoiceLine.BOSS:
        //        break;
        //    case ContainmentPlayerVoiceLine.DAMAGED:
        //        break;
        //    case ContainmentPlayerVoiceLine.RELOADING:
        //        break;
        //    case ContainmentPlayerVoiceLine.ROUNDENDING:
        //        break;
        //    case ContainmentPlayerVoiceLine.SHIELDBEACONDAMAGED:
        //        break;
        //    case ContainmentPlayerVoiceLine.SHIELDBEACONLOW:
        //        break;

        //}

        Debug.Log($"Triggered Voice Line: {voiceLine}");
    }



    #endregion Mirror Remote Actions


    #region Event Subscription Methods

    private void HandleWaveCompleted()
    {
        this._inPreparationPhase = true;
        this._readyNextWave = false;

        Debug.Log("In preparation phase, press ready to start next wave");
    }

    private void HandleWaveStart()
    {
        this._inPreparationPhase = false;
        this._readyNextWave = false;
    }


    #endregion Event Subscription Methods


    IEnumerator RegenerateHealth()
    {
        this._canRegen = false;
        health.alterHealth(healthRegenRate);
        RpcUpdatePlayerHealth(this.health.HealthValue);

        //Debug.Log($"Regenerating Player Health: {this.health.HealthValue}");

        yield return new WaitForSeconds(1f);
        this._canRegen = true;

    }

    bool ShouldRegenerateHealth()
    {
        return !this.health.AtMaxHealth && this._canRegen && Time.time - lastDamaged >= healthRegenCooldown;
    }





    #region Player Controls

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

    #endregion Player Controls

}
