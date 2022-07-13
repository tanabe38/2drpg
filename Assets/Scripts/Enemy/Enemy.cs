using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy")]
public class Enemy : ScriptableObject
{
    public BattleParameterBase Data;
    public string Name;
    public Sprite Sprite;

    public virtual Enemy Clone()
    {
        var clone = ScriptableObject.CreateInstance<Enemy>();
        clone.Data = new BattleParameterBase();
        Data.CopyTo(clone.Data);
        clone.Name = Name;
        clone.Sprite = Sprite;
        return clone;
    }

    public virtual TurnInfo BattleAction(BattleWindow battleWindow)
    {
        var info = new TurnInfo();
        info.Message = $"{Name}のターン。<未実装>";
        return info;
    }
}