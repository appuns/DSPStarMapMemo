using BepInEx;
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
using crecheng.DSPModSave;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace DSPStarMapMemo
{

    [BepInPlugin("Appun.DSP.plugin.StarMapMemo", "DSPStarMapMemo", "0.0.1")]
    [BepInProcess("DSPGAME.exe")]
    [BepInDependency(DSPModSavePlugin.MODGUID)]



    public class Main : BaseUnityPlugin , IModCanSave
    {

        //public static ConfigEntry<bool> DisableKeyTips;
        //public static ConfigEntry<bool> DisableKeyTips;



        public void Start()
        {
            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            //configの設定
            //alwaysDisplay = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            //throughPlanet = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");



            UI.Create();
        }


        public void Import(BinaryReader r)
        {
            LogManager.Logger.LogInfo("---------------------------------------------------------Import");
            //MarkerPool.countInPlanet.Clear();
            MemoPool.memoPool.Clear();

            if (r.ReadInt32() == 1)
            {

                //markerIdInPlanetの読み込み
                int num3 = r.ReadInt32();
                LogManager.Logger.LogInfo("---------------------------------------------------num3 : " + num3);

                for (int j = 0; j < num3; j++)
                {
                    var Key = r.ReadInt32();
                    LogManager.Logger.LogInfo("---------------------------------------------------Key : " + Key);
                    MemoPool.Memo newMemo = new MemoPool.Memo();
                    newMemo.signalIconId = new int[10];

                    newMemo.id = r.ReadInt32();
                    LogManager.Logger.LogInfo("----------------------------------------- --newMemo.id : " + newMemo.id);
                    newMemo.desc = r.ReadString();
                    LogManager.Logger.LogInfo("------------------------------------------newMemo.desc : " + newMemo.desc);
                    newMemo.color.r = r.ReadSingle();
                    newMemo.color.g = r.ReadSingle();
                    newMemo.color.b = r.ReadSingle();
                    newMemo.color.a = r.ReadSingle();
                    LogManager.Logger.LogInfo("-----------------------------------------newMemo.color : " + newMemo.color);
                    for (int i = 0; i < 10; i++)
                    {
                        newMemo.signalIconId[i] = r.ReadInt32();
                        LogManager.Logger.LogInfo("-------------------------------newMemo.signalIconId[i] : " + newMemo.signalIconId[i]);
                    }
                    MemoPool.memoPool.Add(Key, newMemo);
                }
            }
            else
            {
                LogManager.Logger.LogInfo("Save data version error");
            }

            LogManager.Logger.LogInfo("-------------------------------------------------------------memoPool.Count : " + MemoPool.memoPool.Count);

            foreach (KeyValuePair<int, MemoPool.Memo> kvp in MemoPool.memoPool)
            {
                LogManager.Logger.LogInfo("---------------------------------------------------Key : " + kvp.Key);
                LogManager.Logger.LogInfo("----------------------------------------------Value.id : " + kvp.Value.id);
                LogManager.Logger.LogInfo("--------------------------------------------Value.desc : " + kvp.Value.desc);
                for (int i = 0; i < 10; i++)
                {
                    LogManager.Logger.LogInfo("---------------------------------Value.signalIconId[" + i + "] : " + kvp.Value.signalIconId[i]);
                }

            }



        }

        public void Export(BinaryWriter w)
        {
            LogManager.Logger.LogInfo("---------------------------------------------------------Export");
            w.Write(1); //セーブデータバージョン
            w.Write(MemoPool.memoPool.Count);
            for (int j = 0; j < MemoPool.memoPool.Count; j++)
            {
                foreach (KeyValuePair<int, MemoPool.Memo> keyValuePair in MemoPool.memoPool)
                {
                    w.Write(keyValuePair.Key);
                    w.Write(keyValuePair.Value.id);
                    w.Write(keyValuePair.Value.desc);
                    w.Write(keyValuePair.Value.color.r);
                    w.Write(keyValuePair.Value.color.g);
                    w.Write(keyValuePair.Value.color.b);
                    w.Write(keyValuePair.Value.color.a);
                    for (int i = 0; i < 10; i++)
                    {
                        w.Write(keyValuePair.Value.signalIconId[i]);
                    }
                }
            }

        }

        public void IntoOtherSave()
        {
            if (MemoPool.memoPool.Count > 0)
            {
                MemoPool.memoPool.Clear();
            }
        }
    }


    public class LogManager
    {
        public static ManualLogSource Logger;
    }

}