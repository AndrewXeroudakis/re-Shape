using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	#region Variables
	public InputManager inputManager;
	private PassageNavigation passageNavigation;
	private PassageDisplay passageDisplay;
	Vector2 touchPosition = InputManager.DEFAULT_TOUCH_POSITION;
	ActionButton actionButton = null;
	#endregion

	#region Unity Event Functions
	protected override void Awake()
	{
		base.Awake();
		
		passageNavigation = GetComponent<PassageNavigation>();
		passageDisplay = GetComponent<PassageDisplay>();
	}

    private void Start()
    {
		UpdatePassage();
		//AudioManager.Instance.PlayMusic("monday_theme");
	}

    private void Update()
	{
		touchPosition = inputManager.GetTouchPosition();

		if (touchPosition != InputManager.DEFAULT_TOUCH_POSITION)
        {
			actionButton = inputManager.GetPressedActionButton(); //touchPosition
			AudioManager.Instance.PlaySoundAtPoint("click", Vector3.zero);
			if (actionButton != null)
            {
				//AudioManager.Instance.PlaySoundAtPoint("clickButton", Vector3.zero);
				actionButton.OnTouchDown();
			}
            else
            {
				//AudioManager.Instance.PlaySoundAtPoint("click", Vector3.zero);
				UpdatePassage();
			}
		}
	}
	#endregion

	#region Methods
	public void ActionSelected(string _action)
    {
		passageNavigation.ChangePassage(_action);
		passageDisplay.ClearUIElements();
		UpdatePassage();
	}

	private void UpdatePassage()
    {
		if (passageDisplay.IsReadyToDisplay())
		{
			PassageTextBlock nextTextBlock = passageNavigation.GetNextTextBlock();
			if (nextTextBlock != null)
			{
				if (String.IsNullOrEmpty(nextTextBlock.text))
				{
					Debug.Log("Empty or Null text on passage: " + nextTextBlock.id);
				}
				else
				{
					AudioManager.Instance.PlayTextBlockSound(nextTextBlock.id);
					passageDisplay.DisplayTextBlock(nextTextBlock);
				}
			}
			else
			{
				List<string> actions = passageNavigation.GetPassageActions();
				if (actions.Count > 0)
				{
					passageDisplay.DisplayActionTexts(actions);
				}
			}
		}
		else
			passageDisplay.CancelAnimations();
	}

	#endregion
}
