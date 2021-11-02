using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using LanguageTranslator;

public class EditWordMeaning_Editor : EditorWindow
{


    private LanguageDatabase languageDatabase;
    private List<Word> dataList;
    private const string DATABASE_PATH = @"Assets/Auto Localization/Resources/LanguageDB/LanguageDatabase.asset";
    private int count = 0;

    void OnEnable()
    {
        if (languageDatabase == null)
        {
            LoadDatabase();
        }
        dataList = new List<Word>(languageDatabase.GetDB());
        count = dataList.Count;
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

    int index;

    public void SetValue(int _index)
    {
        index = _index;
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        DisplayMainArea();
        EditorGUILayout.EndVertical();
    }

    private Vector2 _scrollPosition;

    void DisplayMainArea()
    {
        // Set Source Text
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        GUILayout.Label("Word:", GUILayout.Width(40));
        GUILayout.Label(dataList[index].word.ToLower(), GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.Separator();
        //-----------------------------------------------

        // Get Meaning of Source Text
        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        // Meanings
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, "box", GUILayout.ExpandHeight(true));
        for (int j = 0; j < dataList[index].wordTranslation.Count; j++)
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.Label(dataList[index].wordTranslation[j].country.ToString(), GUILayout.Width(200));
            dataList[index].wordTranslation[j].meaning = EditorGUILayout.TextField(new GUIContent(""), dataList[index].wordTranslation[j].meaning, GUILayout.ExpandWidth(true)).ToLower();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

        EditorGUILayout.EndScrollView();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.Space();
        //----------------------------------------------
        // Save Changes, if any
        if (GUILayout.Button("Done", GUILayout.Width(200)))
        {
            EditorUtility.SetDirty(languageDatabase);
            this.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        EditorGUILayout.EndVertical();
        //-----------------------------------------------

    }
}
