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
        public static UIKeyTipNode ctrlToHideStarMemo = new UIKeyTipNode();
        public static bool showMemoFlag = true;



        //恒星メモの表示
        [HarmonyPostfix]
        //[HarmonyPatch(typeof(UIStarmapStar), "_OnInit")]
        [HarmonyPatch(typeof(UIStarmapStar), "_OnLateUpdate")]
        //[HarmonyPatch(typeof(UIStarmapStar), "OnStarDisplayNameChange")]
        public static void UIStarmapStar_OnInit_Postfix(UIStarmapStar __instance)
        {
            showMemo(__instance.star.id, __instance.transform);

        }

        //惑星メモの表示
        [HarmonyPostfix]
        //[HarmonyPatch(typeof(UIStarmapPlanet), "_OnInit")]
        [HarmonyPatch(typeof(UIStarmapPlanet), "_OnLateUpdate")]
        //[HarmonyPatch(typeof(UIStarmapPlanet), "OnPlanetDisplayNameChange")]
        public static void UIStarmapPlanet_OnInit_Postfix(UIStarmapPlanet __instance)
        {
            showMemo(__instance.planet.id, __instance.transform);
        }



        public static void showMemo(int id, Transform tr)
        {
            bool keyCheck = Input.GetKey(KeyCode.LeftControl);
            //LogManager.Logger.LogInfo("----------------------------------------------keyCheck" + keyCheck);
            //if (keyCheck)
            //{
            //    showMemoFlag = !showMemoFlag;
            //    LogManager.Logger.LogInfo("----------------------------------------------LeftControl : " + showMemoFlag);
            //}
            GameObject memoText = tr.Find("name-text/memoText").gameObject;
            if (!keyCheck)
            {
                if (MemoPool.memoPool.ContainsKey(id))
                {
                    //memoText.GetComponent<Text>().text = __instance.planet.displayName + " : " + __instance.planet.id; // MemoPool.memoPool[__instance.planet.id].desc;
                    memoText.GetComponent<Text>().text = MemoPool.memoPool[id].desc;
                    memoText.SetActive(true);
                    for (int i = 0; i < 10; i++)
                    {
                        int signalIconId = MemoPool.memoPool[id].signalIconId[i];
                        GameObject icon = tr.Find("name-text/icon" + i).gameObject;
                        if (signalIconId != 0)
                        {
                            //LogManager.Logger.LogInfo("----------------------------------------------signalIconId" + i + " : " + signalIconId);

                            icon.GetComponent<Image>().sprite = LDB.signals.IconSprite(signalIconId);
                            icon.SetActive(true);
                        }
                        else
                        {
                            icon.SetActive(false);
                        }
                    }
                }else
                {
                    memoText.SetActive(false);
                }
            }
            else
            {
                memoText.SetActive(false);
            }
        }


        //「イカロス」の非表示
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarmap), "_OnLateUpdate")]
        public static void UIStarmap_OnLateUpdate_Postfix(UIStarmap __instance)
        {

            __instance.playerNameText.gameObject.SetActive(false);

        }

        //情報ウインドウの表示 star
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarDetail), "_OnOpen")]
        public static void UIStarDetail_OnOpen_Postfix(UIStarDetail __instance)
        {

            UI.MemoWindowOpen(__instance.star.id);

        }
        //情報ウインドウの表示 planet
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIPlanetDetail), "_OnOpen")]
        public static void UIPlanetDetail_OnOpen_Postfix(UIPlanetDetail __instance)
        {
            UI.MemoWindowOpen(__instance.planet.id);

        }

        //情報ウインドウの非表示
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarDetail), "_OnClose")]
        [HarmonyPatch(typeof(UIPlanetDetail), "_OnClose")]
        public static void UIStarDetail_OnClose_Postfix()
        {


            if (UIRoot.instance.uiGame.starDetail.active || UIRoot.instance.uiGame.planetDetail.active)
            {
                return;
            }

            UI.memo.color = Color.white;
            UI.memo.id = 0;
            UI.memo.desc = "";
            for (int i = 0; i < 10; i++)
            {
                UI.memo.signalIconId[i] = 0;
            }
            UI.memoWindow.SetActive(false);
        }

        //情報ウインドウの更新 star
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIStarDetail), "RefreshDynamicProperties")]
        public static void UIStarDetail_RefreshDynamicProperties_Postfix(UIStarDetail __instance)
        {
            UI.memoWindow.transform.localPosition = __instance.transform.localPosition - new Vector3(0, __instance.GetComponent<RectTransform>().sizeDelta.y + 40 , 0);

        }

        //情報ウインドウの更新 planet
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIPlanetDetail), "RefreshDynamicProperties")]
        public static void UIPlanetDetail_RefreshDynamicProperties_Postfix(UIPlanetDetail __instance)
        {
            UI.memoWindow.transform.localPosition = __instance.transform.localPosition - new Vector3(0, __instance.GetComponent<RectTransform>().sizeDelta.y + 40, 0);

        }

        //KeyTipを表示
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(UIStarmap), "_OnOpen")]
        //public static void UIStarmap_OnOpen_Postfix()
        //{
        //    LogManager.Logger.LogInfo("-------------------------------------------------UIStarmap : _OnOpen1");

        //    UI.ctrlToHideStarMemo.desired = true;
        //    LogManager.Logger.LogInfo("-------------------------------------------------UIStarmap : _OnOpen2");
        //}

        ////KeyTipを非表示
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(UIKeyTips), "UpdateTipDesiredState")]
        //public static void UIKeyTips_UpdateTipDesiredState_Postfix(UIKeyTips __instance)
        //{
        //    if (UIRoot.instance.uiGame.starmap.active && !UIRoot.instance.uiGame.starDetail.active || !UIRoot.instance.uiGame.planetDetail.active)
        //    {
        //        __instance.mouseLeftInBuildModeX.desired = true;
        //        //UI.ctrlToHideStarMemo.desired = true;
        //    }
        //    else
        //    {
        //        //UI.ctrlToHideStarMemo.desired = false;
        //    }
        //}
        [HarmonyPrefix, HarmonyPatch(typeof(UIKeyTips), "UpdateTipDesiredState")]
        public static void UpdateTipDesiredStatePatch(UIKeyTips __instance, ref List<UIKeyTipNode> ___allTips)
        {
            if (!ctrlToHideStarMemo)
            {
                ctrlToHideStarMemo = UIRoot.instance.uiGame.keyTips.RegisterTip("CTRL", "Press [CTRL] to hide star memo.".Translate());
            }
            ctrlToHideStarMemo.desired = UIGame.viewMode == EViewMode.Starmap && !UIRoot.instance.uiGame.starDetail.active && !UIRoot.instance.uiGame.planetDetail.active;
        }
    }
}
