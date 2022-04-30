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



    public class Main : BaseUnityPlugin //, IModCanSave
    {




        public void Start()
        {
            LogManager.Logger = Logger;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            ////configの設定
            //alwaysDisplay = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            //throughPlanet = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            //ShowArrow = Config.Bind("General", "DisableKeyTips", true, "Disable Key Tips on right side");
            ////maxCount = Config.Bind("General", "maxCount", 200, "Inventory Column Count");

            //jsonFilePath = Path.Combine(Paths.ConfigPath, "markers.json");

            ////テスト
            ////GameMain.data.mainPlayer.gameObject.AddComponent<DynamicCreateMesh>();
            ///
            //LogManager.Logger.LogInfo("---------------------------------------------------------load icon");

            UI.Create();
        }


        //public void Import(BinaryReader r)
        //{
        //    //MarkerPool.countInPlanet.Clear();
        //    MarkerPool.markerPool.Clear();
        //    MarkerPool.markerIdInPlanet.Clear();

        //    if (r.ReadInt32() == 1)
        //    {
        //        //markerCursorの読み込み
        //        MarkerPool.markerCursor = r.ReadInt32();

        //        //markerIdInPlanetの読み込み
        //        int num = r.ReadInt32();
        //        for (int i = 0; i < num; i++)
        //        {
        //            var Key = r.ReadInt32();
        //            var num2 = r.ReadInt32();
        //            List<int> list = new List<int>();
        //            for (int j = 0; j < num2; j++)
        //            {
        //                list.Add(r.ReadInt32());
        //            }
        //            MarkerPool.markerIdInPlanet.Add(Key, list);
        //        }
        //        //markerIdInPlanetの読み込み
        //        int num3 = r.ReadInt32();
        //        for (int i = 0; i < num3; i++)
        //        {
        //            var Key = r.ReadInt32();
        //            MarkerPool.Marker marker = new MarkerPool.Marker();
        //            marker.planetID = r.ReadInt32();
        //            marker.pos.x = r.ReadSingle();
        //            marker.pos.y = r.ReadSingle();
        //            marker.pos.z = r.ReadSingle();
        //            marker.icon1ID = r.ReadInt32();
        //            marker.icon2ID = r.ReadInt32();
        //            marker.color.r = r.ReadSingle();
        //            marker.color.g = r.ReadSingle();
        //            marker.color.b = r.ReadSingle();
        //            marker.color.a = r.ReadSingle();
        //            marker.desc = r.ReadString();
        //            marker.alwaysDisplay = r.ReadBoolean();
        //            marker.throughPlanet = r.ReadBoolean();
        //            marker.ShowArrow = r.ReadBoolean();
        //            MarkerPool.markerPool.Add(Key, marker);
        //        }
        //        MarkerPool.Refresh();
        //        MarkerList.Refresh();
        //    }
        //    else
        //    {
        //        LogManager.Logger.LogInfo("Save data version error");
        //    }

        //    //MarkerList.Reset();

        //}

        //public void Export(BinaryWriter w)
        //{
        //    LogManager.Logger.LogInfo("---------------------------------------------------------Export");
        //    w.Write(1); //セーブデータバージョン
        //    //markerCursorの書き込み
        //    w.Write(MarkerPool.markerCursor);
        //    //markerIdInPlanetの書き込み
        //    w.Write(MarkerPool.markerIdInPlanet.Count);
        //    foreach (KeyValuePair<int, List<int>> keyValuePair in MarkerPool.markerIdInPlanet)
        //    {
        //        w.Write(keyValuePair.Key);
        //        w.Write(keyValuePair.Value.Count);
        //        foreach (int markerId in keyValuePair.Value)
        //        {
        //            w.Write(markerId);
        //        }

        //    }
        //    //markerPoolの書き込み
        //    w.Write(MarkerPool.markerPool.Count);
        //    foreach (KeyValuePair<int, MarkerPool.Marker> keyValuePair in MarkerPool.markerPool)
        //    {
        //        w.Write(keyValuePair.Key);
        //        w.Write(keyValuePair.Value.planetID);
        //        w.Write(keyValuePair.Value.pos.x);
        //        w.Write(keyValuePair.Value.pos.y);
        //        w.Write(keyValuePair.Value.pos.z);
        //        w.Write(keyValuePair.Value.icon1ID);
        //        w.Write(keyValuePair.Value.icon2ID);
        //        w.Write(keyValuePair.Value.color.r);
        //        w.Write(keyValuePair.Value.color.g);
        //        w.Write(keyValuePair.Value.color.b);
        //        w.Write(keyValuePair.Value.color.a);
        //        w.Write(keyValuePair.Value.desc);
        //        w.Write(keyValuePair.Value.alwaysDisplay);
        //        w.Write(keyValuePair.Value.throughPlanet);
        //        w.Write(keyValuePair.Value.ShowArrow);
        //    }

        //}

        //public void IntoOtherSave()
        //{
        //    if (MarkerPool.markerPool.Count > 0)
        //    {
        //        MarkerPool.markerIdInPlanet.Clear();
        //        MarkerPool.markerPool.Clear();
        //        MarkerPool.Refresh();
        //        MarkerList.Refresh();
        //    }
        //}
    }


    public class LogManager
    {
        public static ManualLogSource Logger;
    }

}