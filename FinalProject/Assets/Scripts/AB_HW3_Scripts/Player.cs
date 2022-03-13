using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Require a RigidBody and Animator component to be on whatever GameObject
// this PlayerController is attached to
[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class Player : MonoBehaviour
{

    #region Actions/Events
    public UnityEvent OnInteractEnter;
    public UnityEvent OnInteractExit;
    public UnityEvent<TowerSpawner> OnCraftEnter;
    public UnityEvent<TowerSpawner> OnCraftExit;


    #endregion Actions/Events


    #region Public Variables

    public bool onGround = true;
    public bool sprinting = false;

    public float jumpForce;

    public float walkSpeed;
    public float sprintSpeed;

    public float turnSpeed;

    public List<TowerSpawner> nearbyTowerSpawners;

    #endregion Public Variables


    #region Private Variables

    //[SerializeField] private Collider hitbox;
    //[SerializeField] private Collider interactableRange;


    private Animator playerAnim;
    private Rigidbody playerRb;

    private bool canInteract = false;
    private bool interacting = false;

    #endregion Private Variables

    // Start is called before the first frame update
    void Start()
    {
        // Get any necessary components before the game starts
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();


        // Lock Mouse to center
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.RegisterPlayer(this);
    }

    // Update is called once per frame
    void Update()
    {

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interacting)
            {
                interacting = false;
                OnCraftExit.Invoke(nearbyTowerSpawners[0]);
            }
            else if (canInteract)
            {
                interacting = true;
                OnCraftEnter.Invoke(nearbyTowerSpawners[0]);
            }
        }




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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            canInteract = true;

            // Add TowerSpawner to nearbyTowerSpawners
            nearbyTowerSpawners.Add(other.GetComponent<TowerSpawner>());

            // Invoke the OnInteractEnter event to update UI
            OnInteractEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Interactable"))
        {
            canInteract = false;

            // Remove TowerSpawner to nearbyTowerSpawners
            nearbyTowerSpawners.Remove(other.GetComponent<TowerSpawner>());


            // Invoke the OnInteractExit and OnCraftExit events to update UI
            OnInteractExit.Invoke();

            if (interacting)
            {
                interacting = false;
                OnCraftExit.Invoke(nearbyTowerSpawners[0]);

            }

        }
    }

}
