﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChessType
{
    Watch,
    Black,
    White
}

public class ChessBoard : MonoBehaviour
{
    private static ChessBoard _instacne;                //单例
    public ChessType turn = ChessType.Black;            //轮到谁下棋了（黑白观）
    public int[,] grid;                                 //棋子下在棋盘的位置
    public GameObject[] prefabs;                        //加载黑色或者白色的棋子预制
    public float timer = 0;                            //计时器
    public bool gameStart = true;                       //是否对方下完轮到自己下了且游戏未分出胜负
    private Transform parent;

    public static ChessBoard Instacne
    {
        get
        {
            return _instacne;
        }
    }
    private void Awake()
    {
        if (_instacne == null)
            _instacne = this;
    }

    void Start()
    {
        parent = GameObject.Find("Parent").transform;
        grid = new int[15, 15];
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
    }

    public bool PlayChess(int[] pos)
    {
        if (!gameStart) return false;               //是否轮到自己下棋
        pos[0] = Mathf.Clamp(pos[0], 0, 14);        //限制在棋盘范围内
        pos[1] = Mathf.Clamp(pos[1], 0, 14);
        if (grid[pos[0], pos[1]] != 0)              //初始化grid的时候全部为0，检查这个点是否已经有棋子了，有了的话就不再是0了
            return false;
        if (turn == ChessType.Black)
        {
            GameObject go = Instantiate(prefabs[0], new Vector3(pos[0] - 7, pos[1] - 7), Quaternion.identity);
            go.transform.SetParent(parent);
            grid[pos[0], pos[1]] = 1;
            if (CheckWinner(pos))                   //检查胜负
            {
                GameEnd();
            }

            turn = ChessType.White;
        }
        else if (turn == ChessType.White)
        {
            GameObject go = Instantiate(prefabs[1], new Vector3(pos[0] - 7, pos[1] - 7), Quaternion.identity);
            go.transform.SetParent(parent);
            grid[pos[0], pos[1]] = 2;
            if (CheckWinner(pos))                   //检查胜负
            {
                GameEnd();
            }

            turn = ChessType.Black;
        }


        return true;
    }

    public void GameEnd()
    {
        gameStart = false;
        Debug.Log(turn + "胜利了！");
    }

    public bool CheckWinner(int[] pos)
    {
        if (CheckOneLien(pos, new int[2] { 1, 0 })) return true;
        if (CheckOneLien(pos, new int[2] { 0, 1 })) return true;
        if (CheckOneLien(pos, new int[2] { 1, 1 })) return true;
        if (CheckOneLien(pos, new int[2] { 1, -1 })) return true;
        return false;
    }
    //检查某一行（上下左右斜上斜下）
    public bool CheckOneLien(int[] pos, int[] offset)
    {
        int linkNum = 1;
        //从原点向右移动判断
        for (int i = offset[0], j = offset[1];
            pos[0] + i >= 0 && pos[0] + i < 15 && pos[1] + j >= 0 && pos[1] + j < 15;
            i += offset[0], j += offset[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                linkNum++;

            }
            else
            {
                break;
            }
            if (linkNum > 4) return true;
        }
        //从原点向左移动判断
        for (int i = -offset[0], j = -offset[1];
            pos[0] + i >= 0 && pos[0] + i < 15 && pos[1] + j >= 0 && pos[1] + j < 15;
            i -= offset[0], j -= offset[1])
        {
            if (grid[pos[0] + i, pos[1] + j] == (int)turn)
            {
                linkNum++;

            }
            else
            {
                break;
            }
            if (linkNum > 4) return true;
        }
        return false;
    }
}
