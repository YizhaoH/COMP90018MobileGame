using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LanguageTranslator;
namespace AutoLocalization{
	public class LanguageManager : Singleton<LanguageManager> {


		/// <summary>
		/// Gets the meaning.
		/// </summary>
		/// <returns>The meaning.</returns>
		/// <param name="sourceText">Word for translation.</param>
		/// <param name="targetLanguage">Target language.</param>
		public string GetMeaning(string sourceText, Languages targetLanguage){
			sourceText = sourceText.ToLower ();
			Word word = Database.GetDB ().Find (x => x.word.Equals (sourceText));
			if (word == null)
				return sourceText;
			WordTranslation wordTranslation=  word.wordTranslation.Find (x => x.country == targetLanguage);
			if (wordTranslation == null)
				return sourceText;
			return ArabicTranslation(wordTranslation.meaning,targetLanguage);

		}
		private string ArabicTranslation(string word,Languages targetLanguage){
			if (targetLanguage == Languages.Arabic || targetLanguage == Languages.Urdu) {
				return ArabicFixer.Fix (word);
			}
			return word;
		}
		private LanguageDatabase database;
		private LanguageDatabase Database
		{
			get
			{
				if (database == null)
				{
					database = Resources.Load<LanguageDatabase>("LanguageDB/LanguageDatabase");
				}
				return database;
			}
		}
	}
}
