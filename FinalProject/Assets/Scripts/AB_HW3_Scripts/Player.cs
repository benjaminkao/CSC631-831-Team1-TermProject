using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.Events;

// Require a RigidBody and Animator component to be on whatever GameObject
// this PlayerController is attached to
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Player : NetworkBehaviour
{

    #region Actions/Events


    public static event Action<Player> OnPlayerJoin;
    public static event Action<Player> OnPlayerLeave;

    public static event Action<Player> OnLocalPlayerJoin;
    public static event Action<Player> OnLocalPlayerLeave;

    #endregion Actions/Events


    #region Public Variables

    public bool onGround = true;
    public bool sprinting = false;

    public float jumpForce;

    public float walkSpeed;
    public float sprintSpeed;

    public float turnSpeed;

    public List<TowerSpawner> nearbyTowerSpawners;

    public GameObject playerCam;

    #endregion Public Variables


    #region Private Variables

    //[SerializeField] private Collider hitbox;
    //[SerializeField] private Collider interactableRange;

    [SerializeField] private Rifle gun;

    [SerializeField] private Animator playerAnim;
    [SerializeField] private Rigidbody playerRb;

    private bool canInteract = false;
    private bool interacting = false;

    #endregion Private Variables

    // Start is called before the first frame update


    public override void OnStartAuthority()
    {

        // Lock Mouse to center
        Cursor.lockState = CursorLockMode.Locked;


        Debug.Log("Local Player: " + isLocalPlayer);
        Debug.Log("Is Server: " + isServer);
        if(isLocalPlayer)
        {
            OnLocalPlayerJoin?.Invoke(this);
        }
    }

    private void OnEnable()
    {
        // Get any necessary components before the game starts
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        
        OnPlayerJoin?.Invoke(this);
    }


    private void OnDisable()
    {
        if(isLocalPlayer)
        {
            OnLocalPlayerLeave?.Invoke(this);
        }

        OnPlayerLeave?.Invoke(this);
    }

    // Update is called once per frame
    void Update()
    {

        if(!isLocalPlayer)
        {
            return;
        }

        if(Input.GetButton("Fire1"))
        {
            if(gun.CanShoot) {

                //gun.ClientShoot();

                CmdShoot(Camera.main.transform.position, Camera.main.transform.forward);
            }
        }


        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space) && onGround)
        {
            Jump();
        }


        float forward;

        forward = sprinting ? verticalInput * sprintSpeed * Time.deltaTime : verticalInput * walkSpeed * Time.deltaTime;

        float right = horizontalInput * walkSpeed * Time.deltaTime;



        transform.Translate(forward * Vector3.forward);
        transform.Translate(right * Vector3.right);

        if (verticalInput > 0)
        {
            playerAnim.SetFloat("Speed", sprinting ? sprintSpeed : walkSpeed);
        }
        else
        {
            playerAnim.SetFloat("Speed", 0);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            sprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprinting = false;
        }
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (interacting)
        //    {
        //        interacting = false;
        //        OnCraftExit.Invoke(nearbyTowerSpawners[0]);
        //    }
        //    else if (canInteract)
        //    {
        //        interacting = true;
        //        OnCraftEnter.Invoke(nearbyTowerSpawners[0]);
        //    }
        //}




    }

    [Command]
    void CmdShoot(Vector3 startPos, Vector3 forward)
    {
        GameObject targetHit = gun.Shoot(startPos, forward);

        if(targetHit == null)
        {
            return;
        }


        Enemy enemy = targetHit.gameObject.GetComponent<Enemy>();

        if(enemy == null)
        {
            return;
        }

        // Update Enemy Health on Server
        //enemy.Damage(gun.damage);


        enemy.RpcUpdateHealth(enemy.Health.HealthValue);
    }


    void Jump()
    {
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        onGround = false;
        playerAnim.SetTrigger("jumpTrig");
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Interactable"))
    //    {
    //        canInteract = true;

    //        // Add TowerSpawner to nearbyTowerSpawners
    //        nearbyTowerSpawners.Add(other.GetComponent<TowerSpawner>());

    //        // Invoke the OnInteractEnter event to update UI
    //        OnInteractEnter.Invoke();
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Interactable"))
    //    {
    //        canInteract = false;

    //        // Remove TowerSpawner to nearbyTowerSpawners
    //        nearbyTowerSpawners.Remove(other.GetComponent<TowerSpawner>());


    //        // Invoke the OnInteractExit and OnCraftExit events to update UI
    //        OnInteractExit.Invoke();

    //        if (interacting)
    //        {
    //            interacting = false;
    //            OnCraftExit.Invoke(nearbyTowerSpawners[0]);

    //        }

    //    }
    //}

}
