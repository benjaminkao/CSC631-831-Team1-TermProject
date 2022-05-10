using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BossEnemyAnimator : EnemyAnimator
{


    public override void UpdateAnimation()
    {
        this._isDirty = false;

        CmdUpdateAnimation(this._walking, this._running);
    }

    [ClientRpc]
    public override void CmdUpdateAnimation(bool walking, bool running)
    {
        animator.SetBool("walk", walking);
        animator.SetBool("run", running);
    }
}
