using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAction : CharacterAction
{
    public int GainExp;

    public new void OnDeath()
    {
        base.OnDeath();

        GameCommon.Instance.GainExp(GainExp);
    }
}
