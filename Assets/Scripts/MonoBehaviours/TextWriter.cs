using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextWriter : MonoBehaviour
{
	#region Variables
	private static TextWriter instance;
	private List<TextWriterSingle> textWriterSingleList;
	#endregion

	#region Unity Event Functions
	private void Awake()
	{
		instance = this;
		textWriterSingleList = new List<TextWriterSingle>();
	}
	private void Update()
	{
		for (int i = 0; i < textWriterSingleList.Count; i++)
        {
			bool destroyInstance = textWriterSingleList[i].Update();
			if (destroyInstance)
            {
				textWriterSingleList.RemoveAt(i);
				i--;
            }
		}
	}
	#endregion

	#region Methods
	public static TextWriterSingle AddWriter(TMP_Text _uiText, string _textToWrite, float _timePerCharacter, bool _invisibleCharacters, bool _removeWriterBeforeAdd)
	{
		if (_removeWriterBeforeAdd)
        {
			instance.RemoveWriter(_uiText);
        }
		instance.textWriterSingleList.Add(new TextWriterSingle(_uiText, _textToWrite, _timePerCharacter, _invisibleCharacters));

		return instance.textWriterSingleList[instance.textWriterSingleList.Count - 1];
	}

	private void RemoveWriter(TMP_Text _uiText)
    {
        for (int i = 0; i < textWriterSingleList.Count; i++)
        {
			if (textWriterSingleList[i].GetUIText() == _uiText)
            {
				textWriterSingleList.RemoveAt(i);
				i--;
            }
        }
    }

	public static bool IsWriting(TMP_Text _uiText)
	{
		TextWriterSingle textWriterSingle = instance.GetTextWriterSingle(_uiText);

		if (textWriterSingle != null && textWriterSingle.IsActive())
			return false;

		return true;
	}

	public static void CancelWriting(TMP_Text _uiText)
	{
		TextWriterSingle textWriterSingle = instance.GetTextWriterSingle(_uiText);

		if (textWriterSingle != null)
			textWriterSingle.WriteAndDestroy();
	}

	private TextWriterSingle GetTextWriterSingle(TMP_Text _uiText)
    {
        foreach (TextWriterSingle textWriterSingle in textWriterSingleList)
        {
			if (textWriterSingle.GetUIText() == _uiText)
				return textWriterSingle;
		}

		return null;
	}
	#endregion

	public class TextWriterSingle
	{
		#region Variables
		private TMP_Text uiText;
		private string textToWrite;
		private int characterIndex;
		private float timePerCharacter;
		private bool invisibleCharacters;
		private float timer;
		#endregion

		#region Methods
		public TextWriterSingle(TMP_Text _uiText, string _textToWrite, float _timePerCharacter, bool _invisibleCharacters)
		{
			this.uiText = _uiText;
			this.textToWrite = _textToWrite;
			this.timePerCharacter = _timePerCharacter;
			this.invisibleCharacters = _invisibleCharacters;
			characterIndex = 0;
		}
		
		public bool Update()
		{
			timer -= Time.deltaTime;
			while (timer <= 0f)
			{
				timer += timePerCharacter;
				characterIndex++;
				string text = textToWrite.Substring(0, characterIndex);
				if (invisibleCharacters)
				{
					text += "<color=#00000000>" + textToWrite.Substring(characterIndex) + "</color>";
				}
				uiText.text = text;

				if (characterIndex >= textToWrite.Length)
				{
					uiText = null;
					return true;
				}
			}

			return false;
		}

		public TMP_Text GetUIText()
        {
			return uiText;
        }

		public bool IsActive()
        {
			return characterIndex < textToWrite.Length;
        }

		public void WriteAndDestroy()
        {
			uiText.text = textToWrite;
			characterIndex = textToWrite.Length;
			instance.RemoveWriter(uiText);
        }
        #endregion
    }
}
