using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ContainmentPlayerAnimator : MonoBehaviour
{
    public enum Movement { IDLE, WALKING, RUNNING}


    public Animator animator;

    public float MaxWalkingSpeed;

    private bool _walking;
    private bool _running;

    private bool _isDirty;

    public Movement currentMovementAnimation;


    // Start is called before the first frame update
    void Start()
    {
        if(animator == null)
        {
            Debug.LogWarning("Please set the animator for ContainmentPlayerAnimator. Player will not be animated.");
        }

        _walking = false;
        _running = false;
        _isDirty = false;
        currentMovementAnimation = Movement.IDLE;
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

        if(currentMovementAnimation != Movement.WALKING && speed >= 0.1 && speed < this.MaxWalkingSpeed)
        {
            this._isDirty = true;
            this.currentMovementAnimation = Movement.WALKING;
            this._walking = true;
            this._running = false;
        }

        if(currentMovementAnimation != Movement.RUNNING && speed >= this.MaxWalkingSpeed)
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


    public void HandleJumpAnimation()
    {
        if(animator == null)
        {
            return;
        }

        animator.SetTrigger("jumpTrig");
    }


    public void UpdateAnimation()
    {
        this._isDirty = false;

        animator.SetBool("Walk", this._walking);
        animator.SetBool("Run", this._running);
    }
}
