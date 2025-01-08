using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : APowerUp
{
    public override void Interaction()
    {
        PlayerController.Instance.StartShield();
        base.Interaction();
    }
}
