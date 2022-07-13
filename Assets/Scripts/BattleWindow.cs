//BattleWindow.cs BattleWindowコンポーネントの作成
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BattleWindow : Menu
{
    [SerializeField] RPGSceneManager RPGSceneManager;
    [SerializeField] MenuRoot MainCommands;
    [SerializeField] MenuRoot Items;
    [SerializeField] MenuRoot Enemies;
    [SerializeField] MenuItem EnemyPrefab;
    [SerializeField] Text Description;
    [SerializeField] GameObject ParameterRoot;

    public override void Open()
    {
        base.Open();
        MainCommands.Index = 0;
        Items.gameObject.SetActive(false);
        Description.transform.parent.gameObject.SetActive(false);
        UpdateUI();
    }

    protected override void ChangeMenuItem(MenuRoot menuRoot)
    {
        base.ChangeMenuItem(menuRoot);

        Enemies.gameObject.SetActive(true);
        Items.gameObject.SetActive(CurrentMenuObj == Items);

        var player = RPGSceneManager.Player;
        if(CurrentMenuObj == Items && 0 <= CurrentMenuObj.Index && CurrentMenuObj.Index < player.BattleParameter.Items.Count)
        {

            Description.transform.parent.gameObject.SetActive(true);
            var item = player.BattleParameter.Items[CurrentMenuObj.Index];
            Description.text = item.Description;
        }
        else
        {
            Description.transform.parent.gameObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        UpdateParameters();
        UpdateItem(RPGSceneManager.Player.BattleParameter);
    }

    void UpdateParameters()
    {
        var player = RPGSceneManager.Player;
        var param = player.BattleParameter;
        SetParameterText("HP", $"{param.HP}/{param.MaxHP}");
        SetParameterText("ATK", param.AttackPower.ToString());
        SetParameterText("DEF", param.DefensePower.ToString());
    }

    void SetParameterText(string name, string text)
    {
        var root = ParameterRoot.transform.Find(name);
        var textObj = root.Find("Text").GetComponent<Text>();
        textObj.text = text;
    }

    void UpdateItem(BattleParameterBase param)
    {
        var items = param.Items;
        var useItems = new List<MenuItem>();
        var menuItems = Items.GetComponentsInChildren<MenuItem>(true);
        for (var i = 0; i < menuItems.Length; ++i)
        {
            var menuItem = menuItems[i];
            if (i < items.Count)
            {
                menuItem.gameObject.SetActive(true);
                menuItem.Text = items[i].Name;
                useItems.Add(menuItem);
            }
            else
            {
                menuItem.gameObject.SetActive(false);
            }
        }
        Items.RefreshMenuItems(useItems.ToArray());
    }

    protected override void Cancel(MenuRoot current)
    {
        if(CurrentMenuObj != MainCommands)
        {
            base.Cancel(current);
        }
    }

    public void Attack()
    {

    }

    public void Defense()
    {

    }

    public void UseItem()
    {
        UpdateItem(RPGSceneManager.Player.BattleParameter);
    }

    public void Escape()
    {

    }
}