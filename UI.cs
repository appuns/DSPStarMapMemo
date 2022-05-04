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
        public static GameObject[] icon = new GameObject[10];
        public static GameObject iconBoxPrefab = new GameObject();
        public static GameObject[] iconBox = new GameObject[10];
        public static GameObject memoWindow;
        public static GameObject descBox;
        public static GameObject explainText;

        //public static string desc;
        //public static int[] iconID = new int[10];

        public static Sprite emptySprite;
        public static int selectedIconNo;


        public static MemoPool.Memo memo;


        public static void Create()

        {
            memo.id = 0;
            memo.color = Color.white;
            memo.desc = "";
            memo.signalIconId = new int[10];
            for (int i = 0; i < 10; i++)
            {
                memo.signalIconId[i] = 0;
            }

            //アイコンとメモテキスト作成 starmap-star
            GameObject nameText = GameObject.Find("UI Root/Overlay Canvas/In Game/Starmap UIs/Starmap Screen/starmap-star-ui/name-text");
            memoText = Instantiate(nameText, nameText.transform);
            memoText.name = "memoText";
            memoText.transform.localPosition = new Vector3(0, -130, 0);
            memoText.transform.localScale = new Vector3(1, 1, 1);

            memoText.GetComponent<RectTransform>().sizeDelta = new Vector3(300, 150, 0);
            memoText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
            memoText.GetComponent<Text>().lineSpacing = 0.8f;
            memoText.GetComponent<Text>().text = "!TESTTESTTESTTESTTESTTEST!\n!TESTTESTTESTTESTTESTTEST!";


            memoText.SetActive(false);

            iconPrefab.AddComponent<RectTransform>().sizeDelta = new Vector3(28, 28, 0);
            iconPrefab.AddComponent<Image>();
            //iconPrefab.SetActive(true);

            for (int i = 0; i < 10; i++)
            {
                icon[i] = Instantiate(iconPrefab.gameObject, nameText.transform);
                icon[i].name = "icon" + i;

                icon[i].transform.localPosition = new Vector3(30f * i + 15f, -38, 0);
                icon[i].SetActive(false);
                //icon[i].GetComponent<Image>().sprite = null;
            }

            //アイコンとメモテキスト作成 starmap-planet
            GameObject nameText2 = GameObject.Find("UI Root/Overlay Canvas/In Game/Starmap UIs/Starmap Screen/starmap-planet-ui/name-text");
            memoText = Instantiate(nameText2, nameText2.transform);
            memoText.name = "memoText";
            memoText.transform.localPosition = new Vector3(0, -130, 0);
            memoText.transform.localScale = new Vector3(1, 1, 1);

            memoText.GetComponent<RectTransform>().sizeDelta = new Vector3(300, 150, 0);
            memoText.GetComponent<Text>().alignment = TextAnchor.UpperLeft;
            memoText.GetComponent<Text>().lineSpacing = 0.8f;
            //memoText.GetComponent<Text>().text = "!TESTTESTTESTTESTTESTTEST!\n!TESTTESTTESTTESTTESTTEST!";

            memoText.SetActive(false);

            for (int i = 0; i < 10; i++)
            {
                icon[i] = Instantiate(iconPrefab.gameObject, nameText2.transform);
                icon[i].name = "icon" + i;

                icon[i].transform.localPosition = new Vector3(30f * i + 15f, -38, 0);
                icon[i].SetActive(false);
            }


            //メモ編集ウインドウ作成
            GameObject planetdetailwindow = GameObject.Find("UI Root/Overlay Canvas/In Game/Planet & Star Details/planet-detail-ui");
            memoWindow = Instantiate(planetdetailwindow, planetdetailwindow.transform.parent);
            memoWindow.name = "memoWindow";
            memoWindow.transform.localPosition = new Vector3(0, -300, 0);
            GameObject typeText = memoWindow.transform.Find("type-text").gameObject;
            typeText.transform.localPosition = new Vector3(-240, -3, 0);
            typeText.name = "typeText";
            typeText.GetComponent<Text>().text = "Memo".Translate();
            typeText.GetComponent<Text>().resizeTextMaxSize = 20;

            explainText = Instantiate(typeText.gameObject, memoWindow.transform);
            explainText.name = "explainText";
            explainText.transform.localPosition = new Vector3(-240, -190, 0);
            explainText.GetComponent<Text>().text = "Press [Enter] to insert a line break.".Translate();
            explainText.GetComponent<Text>().resizeTextMaxSize = 20;

            Destroy(memoWindow.GetComponent<UIPlanetDetail>());
            Destroy(memoWindow.transform.Find("res-group").gameObject);
            Destroy(memoWindow.transform.Find("param-group").gameObject);
            Destroy(memoWindow.transform.Find("name-input").gameObject);
            Destroy(memoWindow.transform.Find("icon").gameObject);
            //Destroy(memoWindow.transform.Find("type-text").gameObject);
            Destroy(memoWindow.transform.Find("bg").gameObject);

            //テキスト編集ボックスの作成
            descBox = Instantiate(UIRoot.instance.uiGame.blueprintBrowser.inspector.descTextInput.gameObject, memoWindow.transform);
            descBox.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            descBox.name = "descBox";

            //GameObject nameInput = memoWindow.transform.Find("name-input").gameObject;
            descBox.transform.localPosition = new Vector3(-250, -80, 0);
            descBox.transform.localScale = new Vector3(0.87f, 0.87f, 0.87f);
            descBox.GetComponent<RectTransform>().sizeDelta = new Vector3(300, 120, 0);
            Destroy(descBox.GetComponent<UIButton>());

            Text valueText = descBox.transform.Find("value-text").GetComponent<Text>();
            valueText.alignment = TextAnchor.UpperLeft;
            valueText.lineSpacing = 0.8f;
            //valueText.text = "!TESTTESTTESTTESTTESTTEST!\n!TESTTESTTESTTESTTESTTEST!\n!TESTTESTTESTTESTTESTTEST!";
            valueText.fontSize = 20;



            memoWindow.SetActive(false);

            //アイコン編集ボックス作成
            iconBoxPrefab = Instantiate(UIRoot.instance.uiGame.blueprintBrowser.inspector.thumbIconImage1.gameObject, memoWindow.transform);
            iconBoxPrefab.name = "iconBoxPrefab";
            iconBoxPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            iconBoxPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            iconBoxPrefab.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            //iconBoxPrefab.GetComponent<RectTransform>().sizeDelta = new Vector3(20, 20, 0);
            iconBoxPrefab.transform.localPosition = new Vector3(45, -120, 0);
            iconBoxPrefab.SetActive(false);

            emptySprite = iconBoxPrefab.GetComponent<Image>().sprite;

            for (int i = 0; i < 10; i++)
            {
                iconBox[i] = Instantiate(iconPrefab.gameObject, memoWindow.transform);
                iconBox[i].name = "iconBox" + i;

                iconBox[i].transform.localPosition = new Vector3(26f * i - 235f, -55, 0);
                //icon[i].GetComponent<Image>().sprite = null;
                iconBox[i].GetComponent<RectTransform>().sizeDelta = new Vector3(24, 24, 0);
                iconBox[i].GetComponent<Image>().sprite = emptySprite;
                iconBox[i].AddComponent<Button>();

                iconBox[i].SetActive(true);
                var iconNo = i;
                //iconBox[i].AddComponent<ClickHandler>();
                iconBox[i].GetComponent<Button>().onClick.AddListener(() => OnClickIconBox(iconNo));

            }

            descBox.GetComponent<InputField>().onEndEdit.AddListener(new UnityAction<string>(onEndEditDescBox));


        }

        //説明文が更新されたら
        public static void onEndEditDescBox(string str)
        {
            memo.desc = str;
            MemoPool.AddOrUpdate();

        }

        //アイコンをクリックしたらピッカーを開く
        public static void OnClickIconBox(int iconNo)
        {
            if (UISignalPicker.isOpened)
            {
                return;
            }
            selectedIconNo = iconNo;
            UISignalPicker.Popup(new Vector2(50f, 350f), new Action<int>(onIconBoxChanged));
        }

        //アイコンが指定されたら保存
        public static void onIconBoxChanged(int signalId)
        {
            iconBox[selectedIconNo].GetComponent<Image>().sprite = LDB.signals.IconSprite(signalId);
            memo.signalIconId[selectedIconNo] = signalId;
            //int key = 0;
            //if (UIRoot.instance.uiGame.starDetail.active)
            //{
            //    key = UIRoot.instance.uiGame.starDetail.star.id;
            //}
            //else if (UIRoot.instance.uiGame.planetDetail.active)
            //{
            //    key = UIRoot.instance.uiGame.planetDetail.planet.id;
            //}
            //memo.id = key;
            //Text valueText = descBox.transform.Find("value-text").GetComponent<Text>();
            //memo.desc = valueText.text; //key + " : " + signalId;
            //MemoPool.Memo tempMemo = new MemoPool.Memo();
            //tempMemo.id = memo.id;
            ////tempMemo.color = memo.color;
            //tempMemo.desc = memo.desc;
            //tempMemo.signalIconId = new int[10];
            //for (int i = 0; i < 10; i++)
            //{
            //    tempMemo.signalIconId[i] = memo.signalIconId[i];
            //}

            //LogManager.Logger.LogInfo("----------------------------------------------AddOrUpdate");

            MemoPool.AddOrUpdate();



        }

        //public static void Refresh(int key)
        //{
        //    if (MemoPool.memoPool.ContainsKey(key))
        //    {

        //        InputField valueText = descBox.GetComponent<InputField>();
        //        valueText.text = MemoPool.memoPool[key].desc;
        //        for (int i = 0; i < 10; i++)
        //        {
        //            int signalIconId = MemoPool.memoPool[key].signalIconId[i];
        //            iconBox[i].GetComponent<Image>().sprite = LDB.signals.IconSprite(signalIconId);
        //        }

        //    } else
        //    {
        //        InputField valueText = descBox.GetComponent<InputField>();
        //        valueText.text = "none";
        //        for (int i = 0; i < 10; i++)
        //        {
        //            iconBox[i].GetComponent<Image>().sprite = emptySprite;
        //        }
        //    }
        //}

        public static void MemoWindowOpen(int key)
        {
            //LogManager.Logger.LogInfo("-----------------------------------MemoWindowOpen pre---------memoPool.Count : " + MemoPool.memoPool.Count);

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


            //memo = new MemoPool.Memo();

            if (MemoPool.memoPool.ContainsKey(key))
            {
                //LogManager.Logger.LogInfo("----------------------------------------------MemoWindowOpen : Contains");
                UI.memo.id = MemoPool.memoPool[key].id;
                UI.memo.desc = MemoPool.memoPool[key].desc;
                UI.memo.color = MemoPool.memoPool[key].color;
                //LogManager.Logger.LogInfo("----------------------------------------------id : " + key);
                //LogManager.Logger.LogInfo("--------------------------------------- -memo.id : " + UI.memo.id);
                //LogManager.Logger.LogInfo("---------------------------------------memo.desc : " + UI.memo.desc);

                InputField valueText = descBox.GetComponent<InputField>();
                valueText.text = UI.memo.desc;
                for (int i = 0; i < 10; i++)
                {
                    //LogManager.Logger.LogInfo("----------------------------------------------signalIconId" + i + " : " + MemoPool.memoPool[key].signalIconId[i]);

                    UI.memo.signalIconId[i] = MemoPool.memoPool[key].signalIconId[i];
                    if (UI.memo.signalIconId[i] != 0)
                    {
                        iconBox[i].GetComponent<Image>().sprite = LDB.signals.IconSprite(UI.memo.signalIconId[i]);
                    } else
                    {
                        iconBox[i].GetComponent<Image>().sprite = emptySprite;
                    }
                }

            }
            else
            {
                //LogManager.Logger.LogInfo("----------------------------------------------MemoWindowOpen : Not Contains");

                UI.memo.id = 0;
                UI.memo.desc = "";
                UI.memo.color = Color.white;

                InputField valueText = descBox.GetComponent<InputField>();
                valueText.text = "";
                for (int i = 0; i < 10; i++)
                {
                    iconBox[i].GetComponent<Image>().sprite = emptySprite;
                    UI.memo.signalIconId[i] = 0;
                }
            }

            memoWindow.SetActive(true);

            //LogManager.Logger.LogInfo("-----------------------------------MemoWindowOpen after---------memoPool.Count : " + MemoPool.memoPool.Count);

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

        //public static void UISignalPickerPopupVector2(Vector2 pos, Action<int,int> _onReturn)
        //{
        //    UISignalPicker signalPicker = UIRoot.instance.uiGame.signalPicker;
        //    signalPicker.onReturn = _onReturn;
        //    signalPicker._Open();
        //    signalPicker.pickerTrans.anchoredPosition = pos;


        //}

        //public static void Popup(Vector2 pos, Action<int> _onReturn)
        //{
        //    if (UIRoot.instance == null)
        //    {
        //        if (_onReturn != null)
        //        {
        //            _onReturn(0);
        //        }
        //        return;
        //    }
        //    UISignalPicker signalPicker = UIRoot.instance.uiGame.signalPicker;
        //    if (!signalPicker.inited || signalPicker.active)
        //    {
        //        if (_onReturn != null)
        //        {
        //            _onReturn(0);
        //        }
        //        return;
        //    }
        //    signalPicker.onReturn = _onReturn;
        //    signalPicker._Open();
        //    signalPicker.pickerTrans.anchoredPosition = pos;
        //}


    }
}
