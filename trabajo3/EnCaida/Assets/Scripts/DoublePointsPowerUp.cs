using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoublePointsPowerUp : APowerUp
{
    public override void Interaction()
    {
        GameController.Instance.StartDoublePoints();
        base.Interaction();
    }
}
