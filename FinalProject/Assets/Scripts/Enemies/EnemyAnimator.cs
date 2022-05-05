using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class EnemyAnimator : NetworkBehaviour
{
    public enum Movement { IDLE, WALKING, RUNNING }

    public Animator animator;

    public Movement currentMovementAnimation;

    [SerializeField] protected float walkingSpeed;


    protected float sqrWalkingSpeed;
    protected float sqrRunningSpeed;


    protected bool _walking;
    protected bool _running;
    protected bool _died;



    protected bool _isDirty;



    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) {
            Debug.LogWarning("Please set the animator for EnemyAnimator. Enemy will not be animated.");
        }

        _walking = false;
        _running = false;
        _died = false;
        _isDirty = false;
        currentMovementAnimation = Movement.IDLE;

        sqrWalkingSpeed = walkingSpeed * walkingSpeed;
    }

    public void HandleMovementAnimation(float speed)
    {
        if(animator == null)
        {
            return;
        }

        if(currentMovementAnimation != Movement.IDLE && speed < 0.1)
        {
            this._isDirty = true;
            this.currentMovementAnimation = Movement.IDLE;
            this._walking = false;
            this._running = false;
        }

        if(currentMovementAnimation != Movement.WALKING && speed >= 0.1 && speed < this.sqrWalkingSpeed)
        {
            this._isDirty = true;
            this.currentMovementAnimation = Movement.WALKING;
            this._walking = true;
            this._running = false;
        }

        if(currentMovementAnimation != Movement.RUNNING && speed > this.sqrWalkingSpeed)
        {
            this._isDirty = true;
            this.currentMovementAnimation = Movement.RUNNING;
            this._walking = false;
            this._running = true;
        }

        if(_isDirty)
        {
            UpdateAnimation();
        }

        

    }

    public void HandleDeathAnimation()
    {
        if(animator == null)
        {
            return;
        }

        animator.SetBool("dead", true);
    }

    public void HandleAttackAnimation()
    {
        if(animator == null)
        {
            return;
        }
        animator.SetTrigger("attack");
    }

    public virtual void UpdateAnimation()
    {
        this._isDirty = false;

        CmdUpdateAnimation(this._walking, this._running);
    }

    [ClientRpc]
    public virtual void CmdUpdateAnimation(bool walking, bool running)
    {
        animator.SetBool("walk", walking);
        animator.SetBool("run", running);
    }



}
