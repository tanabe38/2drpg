using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : CharacterBase
{
    public BattleParameter InitialBattleParameter;
    public BattleParameterBase BattleParameter;

    protected override void Start()
    {
        DoMoveCamera = true;
        base.Start();
        InitialBattleParameter.Data.CopyTo(BattleParameter);
    }
}
