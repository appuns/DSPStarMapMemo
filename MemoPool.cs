﻿using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System;
using System.IO;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using static UnityEngine.GUILayout;
using UnityEngine.Rendering;
using Steamworks;
using rail;
using xiaoye97;

namespace DSPStarMapMemo
{
    public class MemoPool : MonoBehaviour
    {
        public static Dictionary<int, Memo> memoPool = new Dictionary<int, Memo>();

        public struct Memo
        {
            public int id;
            public int[] signalIconId; 
            public Color color;
            public string desc;
        }

        public static void AddOrUpdate()
        {

            int key = 0;
            if (UIRoot.instance.uiGame.starDetail.active)
            {
                key = UIRoot.instance.uiGame.starDetail.star.id;
            }
            else if (UIRoot.instance.uiGame.planetDetail.active)
            {
                key = UIRoot.instance.uiGame.planetDetail.planet.id;
            }
            UI.memo.id = key;
            Memo tempMemo = new Memo();
            tempMemo.id = UI.memo.id;
            tempMemo.color = UI.memo.color;
            tempMemo.desc = UI.memo.desc;
            tempMemo.signalIconId = new int[10];
            for (int i = 0; i < 10; i++)
            {
                tempMemo.signalIconId[i] = UI.memo.signalIconId[i];
            }

            //LogManager.Logger.LogInfo("-------------------------------------------------------------memo.id : " + memo.id);
            //LogManager.Logger.LogInfo("-------------------------------------------------------------memo.desc : " + memo.desc);
            //LogManager.Logger.LogInfo("-------------------------------------------------------------memo.color : " + memo.color);
            //for (int i = 0; i < 10; i++)
            //{
            //    LogManager.Logger.LogInfo("-------------------------------------------------------------memo.signalIconId["+ i + "] : " + memo.signalIconId[i]);
            //}
            if (memoPool.ContainsKey(key))
            {
                memoPool[key] = tempMemo;
            }
            else
            {
                memoPool.Add(key, tempMemo);
            }


            //LogManager.Logger.LogInfo("-------------------------------------------------------------memoPool.Count : " + MemoPool.memoPool.Count);

            //foreach (KeyValuePair<int, MemoPool.Memo> kvp in MemoPool.memoPool)
            //{
            //    LogManager.Logger.LogInfo("---------------------------------------------------Key : " + kvp.Key);
            //    LogManager.Logger.LogInfo("----------------------------------------------Value.id : " + kvp.Value.id);
            //    LogManager.Logger.LogInfo("--------------------------------------------Value.desc : " + kvp.Value.desc);
            //    for (int i = 0; i < 10; i++) 
            //    {
            //        LogManager.Logger.LogInfo("---------------------------------Value.signalIconId[" + i + "] : " + kvp.Value.signalIconId[i]);
            //    }

            //}

        }
    }
}
