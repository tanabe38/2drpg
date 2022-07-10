﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right,
}

public class Player : MonoBehaviour
{
    const string TRIGGER_MoveDown = "MoveDownTrigger";
    const string TRIGGER_MoveLeft = "MoveLeftTrigger";
    const string TRIGGER_MoveRight = "MoveRightTrigger";
    const string TRIGGER_MoveUp = "MoveUpTrigger";

    Animator Animator { get => GetComponent<Animator>(); }
    Coroutine _moveCoroutine;

    [Range(0, 2)] public float MoveSecond = 0.1f;
    [SerializeField] RPGSceneManager RPGSceneManager;

    [SerializeField] Direction _currentDir  = Direction.Down;
    public Direction CurrentDir
    {
        get => _currentDir;
        set
        {
            if (_currentDir == value) return;
            _currentDir = value;
            SetDirAnimation(value);
        }
    }

    [SerializeField] Vector3Int _pos;
    public Vector3Int Pos
    {
        get => _pos;
        set
        {
            if (_pos == value) return;

            if (RPGSceneManager.ActiveMap == null)
            {
                _pos = value;
            }
            else
            {
                if (_moveCoroutine != null)
                {
                    StopCoroutine(_moveCoroutine);
                    _moveCoroutine = null;
                }
                _moveCoroutine = StartCoroutine(MoveCoroutine(value));
            }
        }
    }

    public void SetPosNoCoroutine(Vector3Int pos)
    {
        _pos = pos;
        transform.position = RPGSceneManager.ActiveMap.Grid.CellToWorld(pos) + new Vector3(0.2f, 0.2f, 0);
        Camera.main.transform.position = transform.position + Vector3.forward * -10 + new Vector3(0.2f, 0.2f, 0);
    }

    public bool IsMoving { get => _moveCoroutine != null; }

    public void SetDir(Vector3Int move)
    {
        if (Mathf.Abs(move.x) > Mathf.Abs(move.y))
        {
            CurrentDir = move.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            CurrentDir = move.y > 0 ? Direction.Up : Direction.Down;
        }
    }

    IEnumerator MoveCoroutine(Vector3Int pos)
    {
        var startPos = transform.position;
        var goalPos = RPGSceneManager.ActiveMap.Grid.CellToWorld(pos) + new Vector3(0.2f, 0.2f, 0);
        var t = 0f;
        while (t < MoveSecond)
        {
            yield return null;
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, goalPos, t / MoveSecond);
            Camera.main.transform.position = transform.position + Vector3.forward * -10 + new Vector3(0.2f, 0.2f, 0);
        }
        _pos = pos;
        _moveCoroutine = null;
    }

    void SetDirAnimation(Direction dir)
    {
        if (Animator == null || Animator.runtimeAnimatorController == null) return;

        string triggerName = null;
        switch (dir)
        {
            case Direction.Up: triggerName = TRIGGER_MoveUp; break;
            case Direction.Down: triggerName = TRIGGER_MoveDown; break;
            case Direction.Left: triggerName = TRIGGER_MoveLeft; break;
            case Direction.Right: triggerName = TRIGGER_MoveRight; break;
            default: throw new System.NotImplementedException("");
        }
        Animator.SetTrigger(triggerName);
    }

    private void Start()
    {
        if (RPGSceneManager == null) RPGSceneManager = Object.FindObjectOfType<RPGSceneManager>();
 
        _moveCoroutine = StartCoroutine(MoveCoroutine(Pos));
    }

    private void OnValidate()
    {
        if (RPGSceneManager != null && RPGSceneManager.ActiveMap != null)
        {
            transform.position = RPGSceneManager.ActiveMap.Grid.CellToWorld(Pos) + new Vector3(0.2f, 0.2f, 0);
        }
    }

    private void Awake()
    {
        SetDirAnimation(_currentDir);
    }
}
