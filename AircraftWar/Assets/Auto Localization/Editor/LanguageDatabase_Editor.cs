using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LanguageTranslator
{
    public class LanguageDatabase_Editor : EditorWindow
    {
        private LanguageDatabase languageDatabase;
        private List<LanguageData> dataList;
        private const string DATABASE_PATH = @"Assets/Auto Localization/Resources/LanguageDB/LanguageDatabase.asset";
        private int count = 0;

        [MenuItem("Window/Localization")]
        public static void Init()
        {
            LanguageDatabase_Editor window = EditorWindow.GetWindow<LanguageDatabase_Editor>();
            window.minSize = new Vector2(800, 400);
            window.Show();
        }

        void OnEnable()
        {
            if (languageDatabase == null)
            {
                LoadDatabase();
            }
            dataList = new List<LanguageData>(languageDatabase.GetList());
            count = dataList.Count;
            dataList.Add(new LanguageData());
            count++;
        }

        void LoadDatabase()
        {
            languageDatabase = (LanguageDatabase)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(LanguageDatabase));

            if (languageDatabase == null)
                CreateDatabase();
        }

        void CreateDatabase()
        {
            languageDatabase = ScriptableObject.CreateInstance<LanguageDatabase>();
            AssetDatabase.CreateAsset(languageDatabase, DATABASE_PATH);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            DisplayMainArea();
            EditorGUILayout.EndVertical();
        }

        private Vector2 _scrollPosition;
        bool isTranslating;
        bool isSave;

        void DisplayMainArea()
        {
            //Set Source Text
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            languageDatabase.sourceLanguage = (Languages)EditorGUI.EnumPopup(GUILayoutUtility.GetRect(0, 10.0f, GUILayout.Width(500)), "Source Lanaguage", languageDatabase.sourceLanguage);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Separator();
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            EditorGUILayout.Space();

            // Get Each Word 
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, "box", GUILayout.ExpandHeight(true));
            for (int i = 0; i < count; i++)
            {	
                EditorGUI.BeginDisabledGroup(i < (count - 1));
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                GUILayout.Label("Word", GUILayout.Width(40));
                dataList[i].word = EditorGUILayout.TextField(new GUIContent(""), dataList[i].word, GUILayout.Width(200)).ToLower();
                dataList[i].translatedTo = (long)((Languages)EditorGUI.EnumMaskPopup(GUILayoutUtility.GetRect(0, 10f, GUILayout.ExpandWidth(true)), "Target Languages", (Languages)dataList[i].translatedTo));
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!(i < (languageDatabase.GetDB().Count)));
                if (GUILayout.Button("Edit", GUILayout.Width(64)))
                {
                    EditWordMeaning(i);
                }
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Remove", GUILayout.Width(64)))
                {
                    if (count == 1)
                        return;
                    count--;
                    dataList.RemoveAt(i);
                    languageDatabase.Remove(i);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.Space();

            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.BeginHorizontal();

            // After type word Press Add Button
            if (GUILayout.Button("Add Word", GUILayout.Width(100)))
            {
                if (!dataList[count - 1].word.Equals("") && dataList[count - 1].translatedTo != 0)
                {
					
                    languageDatabase.Add(dataList[count - 1]);
                    dataList.Add(new LanguageData());
                    count++;
                }
                else
                {
                    ShowNotification(new GUIContent("Word or Language not Set !"));
                }
            }

            // Save All
            if (GUILayout.Button("Save All", GUILayout.Width(100)))
            {
                isSave = true;
                EditorUtility.SetDirty(languageDatabase);
                ShowNotification(new GUIContent("SAVE !"));
            }

            // Press Translate and Editor will play
            if (GUILayout.Button("Traslate", GUILayout.Width(100)))
            {
                if (isSave)
                {
                    isSave = false;
                    #if UNITY_EDITOR
                    EditorApplication.isPlaying = true;
                    #endif
                }
                else
                {
                    ShowNotification(new GUIContent("Press Save All then Translate !"));
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();


        }

        private void EditWordMeaning(int index)
        {
            if (languageDatabase.GetDB().Count == 0)
            {
                EditorUtility.DisplayDialog("Not Editable", "There are no words in DB, first download the meaning of each word and then try to edit them.", "OK");
                return;
            }
            EditWordMeaning_Editor window = EditorWindow.GetWindow<EditWordMeaning_Editor>();
            window.minSize = new Vector2(200, 200);
            window.Show(); 
            window.SetValue(index);
        }

    }
}
