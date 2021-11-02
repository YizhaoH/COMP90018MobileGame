using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Languages
{
    Arabic = 0,
    Chinese = 1,
    Dutch = 2,
    English = 3,
    Filipino = 4,
    French = 5,
    German = 6,
    Hindi = 7,
    Indonesian = 8,
    Italian = 9,
    Japanese = 10,
    Portuguese = 11,
    Russian = 12,
    Spanish = 13,
    Turkish = 14,
    Urdu = 15
}
namespace LanguageTranslator
{
    public enum LanguageCodes
    {
        ar = 0,
        zh_CN = 1,
        nl = 2,
        en = 3,
        tl = 4,
        fr = 5,
        de = 6,
        hi = 7,
        id = 8,
        it = 9,
        ja = 10,
        pt_BR = 11,
        ru = 12,
        es = 13,
        tr = 14,
        ur = 15

    }

    public class LanguageDatabase : ScriptableObject
    {

        [SerializeField]
        private List<Word> database;
        [SerializeField]
        private List<LanguageData> data;
        [SerializeField]
        private int successfullDownloaded = 0;
        public Languages sourceLanguage;

        void OnEnable()
        {
			
            if (data == null || database == null)
            {
                data = new List<LanguageData>();
                database = new List<Word>();
                Debug.Log("data is Null");
            }
        }

        public void Add(LanguageData d)
        {
            data.RemoveAll(x => x.word.Equals(d.word));
            this.data.Add(d);
        }

        public void Remove(int index)
        {
            Debug.Log(index);
            if (index < data.Count)
            {
                data.RemoveAt(index);
                if (data.Count == 1)
                {
                    database.Clear();
                    successfullDownloaded = 0;
                }
            }
            if (index < database.Count)
            {
                database.RemoveAt(index);
                successfullDownloaded--;
            }

        }

        public List<LanguageData> GetList()
        {
            return data;
        }

        public List<Word> GetDB()
        {
            return database;
        }

        public void Translate(System.Action callback)
        {
            if (successfullDownloaded < data.Count)
            {
                var value = data[successfullDownloaded];
                int i = database.RemoveAll(x => x.word.Equals(value.word));
                Word word = new Word();
                word.word = value.word;
                word.wordTranslation = new List<WordTranslation>();
                database.Add(word);
                GetTargetLanguage(value.translatedTo, value.word);
            }
            else
            {
                if (callback != null)
                    callback();
            }
        }

        public void OnTranslated()
        {
            successfullDownloaded++;
            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            #endif
            Debug.Log("---------------------------------------------------------");
            Translate(null);
        }

        private void GetTargetLanguage(long selectedEnum, string word)
        {

            long value = (long)selectedEnum;
            List<LanguageCodes> list = new List<LanguageCodes>();
            int count = 0;
            int max = System.Enum.GetNames(typeof(LanguageCodes)).Length;
            while (count < max)
            {
                long isOne = (value & 1);
                value = value >> 1;
                if (isOne == 1)
                {
                    list.Add((LanguageCodes)count);
                }
                count++;
            }
            LanguageCodes sourcecode = (LanguageCodes)sourceLanguage;
            Translate translate = GameObject.FindObjectOfType<Translate>();
            translate.Translation(sourcecode, list, word);

        }



    }

    [System.Serializable]
    public class LanguageData
    {
        public long translatedTo;
        public string word;

        public LanguageData()
        {
            word = "";
        }
    }

    [System.Serializable]
    public class Word
    {
        public string word;
        public List<WordTranslation> wordTranslation;
    }

    [System.Serializable]
    public class WordTranslation
    {
        // translation of word in different Languages
        public Languages country;
        public string meaning;

    }
}
