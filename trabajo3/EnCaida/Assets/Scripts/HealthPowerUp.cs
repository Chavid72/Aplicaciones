using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : APowerUp
{

    public override void Interaction()
    {
        PlayerController.Instance.Health();
        base.Interaction();
    }
}
