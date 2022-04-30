using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Net;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Security;
using System.Security.Permissions;

namespace DSPStarMapMemo
{
    [HarmonyPatch]
    internal class Patch
    {
        //恒星メモの表示
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarmapStar), "_OnInit")]
        [HarmonyPatch(typeof(UIStarmapStar), "_OnLateUpdate")]
        [HarmonyPatch(typeof(UIStarmapStar), "OnStarDisplayNameChange")]
        public static void UIStarmapStar_OnInit_Postfix(UIStarmapStar __instance)
        {
            __instance.nameText.text = __instance.star.displayName + "TEST";

        }

        //情報ウインドウの更新
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarDetail), "RefreshDynamicProperties")]
        public static void UIStarDetail_RefreshDynamicProperties_Postfix(UIStarDetail __instance)
        {
            UI.memoWindow.transform.localPosition = __instance.transform.localPosition - new Vector3(0, __instance.GetComponent<RectTransform>().sizeDelta.y + 50 , 0);

        }

    }
}
