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

    [System.Serializable]
    public class PlayerSaveData : SaveData
    {
        public BattleParameterBaseSaveData battleParameter;
        public PlayerSaveData() { }
        public PlayerSaveData(Player character, RPGSceneManager RPGManager) : base(character)
        {
            battleParameter = new BattleParameterBaseSaveData(character.BattleParameter, RPGManager.ItemList);
        }
    }

    public override SaveData GetSaveData()
    {
        return new PlayerSaveData(this, RPGSceneManager);
    }
}
