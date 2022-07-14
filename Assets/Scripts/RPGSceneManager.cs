using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGSceneManager : MonoBehaviour
{
    public Player Player;
    public Map ActiveMap;
    public MessageWindow MessageWindow;
    public Menu Menu;
    public ItemShopMenu ItemShopMenu;

    [SerializeField] public BattleWindow BattleWindow;

    Coroutine _currentCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        _currentCoroutine = StartCoroutine(MovePlayer());
    }

    IEnumerator MovePlayer()
    {
        while(true)
        {
            if (GetArrowInput(out var move))
            {
                var movedPos = Player.Pos + move;
                var massData = ActiveMap.GetMassData(movedPos);
                Player.SetDir(move);
                if(massData.isMovable)
                {
                    Player.Pos = movedPos;
                    yield return new WaitWhile(() => Player.IsMoving);

                    if(massData.massEvent != null)
                    {
                        massData.massEvent.Exec(this);
                    }
                    else if(ActiveMap.RandomEncount != null)
                    {
                        var rnd = new System.Random();
                        var encount = ActiveMap.RandomEncount.Encount(rnd);
                        if(encount != null)
                        {
                            BattleWindow.SetUseEncounter(encount);
                            BattleWindow.Open();
                        }
                    }
                }
                else if(massData.character != null && massData.character.Event != null)
                {
                    massData.character.Event.Exec(this);
                }
            }
            yield return new WaitWhile(() => IsPauseScene);

            if(Input.GetKeyDown(KeyCode.E))
            {
                OpenMenu();
            }
        }
    }

    bool GetArrowInput(out Vector3Int move)
    {
        var doMove = false;
        move = Vector3Int.zero;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            move.x -= 1; doMove = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            move.x += 1; doMove = true;
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            move.y += 1; doMove = true;
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            move.y -= 1; doMove = true;
        }

        return doMove;
    }

    public void ShowMessageWindow(string message)
    {
        MessageWindow.StartMessage(message);
    }

    public bool IsPauseScene
    {
        get
        {
            return !MessageWindow.IsEndMessage || Menu.DoOpen || ItemShopMenu.DoOpen || BattleWindow.DoOpen;
        }
    }

    public void OpenMenu()
    {
        Menu.Open();
    }
}