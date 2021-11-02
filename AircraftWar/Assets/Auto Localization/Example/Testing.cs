using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AutoLocalization;
using System.Linq;
using System;

public class Testing : MonoBehaviour {

	[SerializeField]
	private Button _getMeaning;
	[SerializeField]
	private InputField _word;
	[SerializeField]
	private Text _meaning;
	[SerializeField]
	private Dropdown _languages;
	void Start(){
		var value = Enum.GetNames (typeof(Languages)).ToList();
		//populating Dropdown menu
		_languages.AddOptions (value);
	}
	public void OnButtonPress(){
		//using AutoLocalization "namespace"
		_meaning.text = LanguageManager.instance.GetMeaning (_word.text,(Languages)_languages.value);

	}

}
