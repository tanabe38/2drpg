using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : CharacterBase
{
    protected override void Start()
    {
        DoMoveCamera = true;
        base.Start();
    }
}
