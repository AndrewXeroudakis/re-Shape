using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class PassageDisplay : MonoBehaviour
{
	#region Variables
	public RectTransform gameScreenContent;
	public TMP_Text displayTextPrefab;
	public TMP_Text actionTextPrefab;
	private TMP_Text activeDisplayText;
	private List<TMP_Text> tMPTexts;  //RectTransform???
	private readonly static string clearUIElementsTrigger = "~clear~";
	private float timePerCharacter = 0.03f;
	#endregion

	#region Unity Event Functions
	private void Awake()
	{
		//activeDisplayText.text = string.Empty;
		tMPTexts = new List<TMP_Text>();
	}

    private void Update()
    {
		
	}
    #endregion

    #region Methods

    public void DisplayTextBlock(PassageTextBlock textBlock)
    {
		// Instantiate new DisplayText, Initialize and Add to display list
		activeDisplayText = Instantiate(displayTextPrefab, Vector3.zero, Quaternion.identity, gameScreenContent);

		string textToDisplay = textBlock.text;
		if (textToDisplay.Length >= clearUIElementsTrigger.Length && CheckAndClearUIElements(textToDisplay.Substring(0, clearUIElementsTrigger.Length)))
			textToDisplay = textToDisplay.Remove(0, clearUIElementsTrigger.Length);
		TextWriter.AddWriter(activeDisplayText, textToDisplay, timePerCharacter, true, true);
		//displayText.text += nextTextBlockText + "\n\n";

		tMPTexts.Add(activeDisplayText);
	}

	public void DisplayActionTexts(List<string> _actions)
    {
		ClearUIElements();
        foreach (string action in _actions)
        {
			TMP_Text newActionText = Instantiate(actionTextPrefab, Vector3.zero, Quaternion.identity, gameScreenContent);
			newActionText.text = action;
			tMPTexts.Add(newActionText);
		}

		/*UnityEngine.UI.Button[] buttons = gameScreenContent.GetComponentsInChildren<UnityEngine.UI.Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			UnityEngine.UI.Button button = buttons[i];
			button.onClick.AddListener(delegate { OnButtonTapped(button); }); //Pointerdown
		}*/
	}

	private bool CheckAndClearUIElements(string _textToCheck)
    {
		if (_textToCheck.Equals(clearUIElementsTrigger))
        {
			ClearUIElements();
			return true;
		}

		return false;
	}

	public void ClearUIElements()
	{
		foreach (TMP_Text tMPText in tMPTexts)
		{
			Destroy(tMPText.gameObject);
		}

		tMPTexts.Clear();
	}

	public bool IsReadyToDisplay()
    {
		// Has no active animations
		return TextWriter.IsWriting(activeDisplayText);
	}

	public void CancelAnimations()
    {
		// Cancel all active animations
		TextWriter.CancelWriting(activeDisplayText);
	}

	/*public void OnButtonTapped(UnityEngine.UI.Button _button)
	{
		string actionText = _button.GetComponentInParent<TextMeshProUGUI>().text;
		GameManager.Instance.ActionSelected(actionText);
	}*/
	#endregion
}
