using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

// use for parsing json text
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace LanguageTranslator
{
    public class Translate : MonoBehaviour
    {
        private string translatedText = "";

        // DB path + DB object for reference
        private const string DATABASE_PATH = @"Assets/Auto Localization/Resources/LanguageDB/LanguageDatabase.asset";
        private LanguageDatabase db;

        [SerializeField]
        ProgressBarController _controller;

        void Start()
        {
            #if UNITY_EDITOR

            db = (LanguageDatabase)AssetDatabase.LoadAssetAtPath(DATABASE_PATH, typeof(LanguageDatabase));

            db.Translate(() =>
                {
                    _controller.ProgressComplete();
                });
            #endif
        }

        public void Translation(LanguageCodes source, List<LanguageCodes> target, string word)
        {
            StartCoroutine(Process(source, target, word));
        }

        private int translatedWord;

        private IEnumerator Process(LanguageCodes sourceLang, List<LanguageCodes> targetLang, string sourceText)
        {

            string sourcText = sourceLang.ToString();
            bool isError = false;
            Debug.Log("Word" + " : " + sourceText);
            foreach (var val in targetLang)
            {
                string targetText = val.ToString();

                // if Language code has hyphen such as Chinene ... Hyphen not allow in Enums..
                if (targetText.Contains("_"))
                {
                    targetText = targetText.Replace('_', '-');
                }
                string url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl="
                             + sourcText + "&tl=" + targetText + "&dt=t&q=" + WWW.EscapeURL(sourceText);

                WWW www = new WWW(url);
                yield return www;

                if (www.isDone)
                {
                    if (string.IsNullOrEmpty(www.error))
                    {
                        var N = JSONNode.Parse(www.text);// Json Parser
                        translatedText = N[0][0][0];
                        print(targetText + " : " + translatedText);
                        OnTranslatedWord(val, translatedText, sourceText);
                    }
                    else
                    {
                        //isError = true;
                        InternetNotWorking();
                        yield break;
                    }
                } 

            }
            if (!isError)
            {
                _controller.ValueChange(db.GetDB().Count, db.GetList().Count);
                db.OnTranslated();
            }

        }

        private void OnTranslatedWord(LanguageCodes target, string targetText, string sourceText)
        {
            WordTranslation wordT = new WordTranslation();
            wordT.country = (Languages)target;
            wordT.meaning = targetText;
            db.GetDB().Find(x => x.word.Equals(sourceText)).wordTranslation.Add(wordT);
        }

        private void InternetNotWorking()
        {
            #if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("Internet Not Working", "Internet connection required for Auto Localization Setup", "OK"))
            {
                // Close Editor
                EditorApplication.isPlaying = false;
            }
            #endif
        }
    }
}
