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

namespace DSPStarMapMemo
{

    internal class UI : MonoBehaviour
    {
        public static GameObject memoText;
        public static GameObject iconPrefab = new GameObject();
        public static GameObject[] icon = new GameObject[12];
        public static GameObject memoWindow;


        public static void Create()

        {
            //アイコンとメモオブジェクト追加
            GameObject nameText = GameObject.Find("UI Root/Overlay Canvas/In Game/Starmap UIs/Starmap Screen/starmap-star-ui/name-text");
            memoText = Instantiate(nameText, nameText.transform);
            memoText.name = "memoText";
            memoText.transform.localPosition = new Vector3(0, -130, 0);
            memoText.transform.localScale = new Vector3(1, 1, 1);

            memoText.GetComponent<RectTransform>().sizeDelta = new Vector3(300, 150, 0);
            memoText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
            memoText.GetComponent<Text>().lineSpacing = 0.8f;
            memoText.GetComponent<Text>().text = "!TESTTESTTESTTESTTESTTEST!\n!TESTTESTTESTTESTTESTTEST!";


            memoText.SetActive(true);

            iconPrefab.AddComponent<RectTransform>().sizeDelta = new Vector3(28, 28, 0);
            iconPrefab.AddComponent<Image>();
            iconPrefab.SetActive(true);

            for (int i = 0; i < 12; i++)
            {
                icon[i] = Instantiate(iconPrefab.gameObject, nameText.transform);
                icon[i].name = "icon" + i;

                icon[i].transform.localPosition = new Vector3(30f * i + 15f, -38, 0);
                //icon[i].GetComponent<Image>().sprite = null;
            }

            //メモ編集ウインドウ
            GameObject planetdetailwindow = GameObject.Find("UI Root/Overlay Canvas/In Game/Planet & Star Details/planet-detail-ui");
            memoWindow = Instantiate(planetdetailwindow, planetdetailwindow.transform.parent);
            memoWindow.name = "memoWindow";
            memoWindow.transform.localPosition = new Vector3(0, -300, 0);
            Destroy(memoWindow.GetComponent<UIPlanetDetail>());
            Destroy(memoWindow.transform.Find("res-group").gameObject);
            Destroy(memoWindow.transform.Find("param-group").gameObject);
            Destroy(memoWindow.transform.Find("icon").gameObject);
            Destroy(memoWindow.transform.Find("type-text").gameObject);
            Destroy(memoWindow.transform.Find("bg").gameObject);

            memoWindow.SetActive(true);


        }
    }
}
