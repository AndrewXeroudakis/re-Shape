using UnityEngine;
using System.Collections.Generic;

public class PassageNavigation : MonoBehaviour
{
	#region Variables
	public Passage currentPassage;
	public PassagesPreconditions passagesPreconditions;
	//private int currentTextBlockIndex;
	private Queue<PassageTextBlock> textBlocks = new Queue<PassageTextBlock>();
	private Dictionary<string, Passage> actionsDictionary = new Dictionary<string, Passage>();
	private List<string> visitedPassages;
    #endregion

    #region Unity Functions
    private void Awake()
    {
		visitedPassages = new List<string>();
		SetupCurrentPassage();
	}
	#endregion

	#region Methods
	private void SetupCurrentPassage()
    {
		if (!visitedPassages.Contains(currentPassage.name))
			visitedPassages.Add(currentPassage.name);
		UnpackTextBlocks();
		UnpackPassageActions();
	}

	private void UnpackTextBlocks()
    {
		textBlocks.Clear();

		foreach (PassageTextBlock textBlock in currentPassage.textBlocks)
        {
			textBlocks.Enqueue(textBlock);
        }
	}

	private void UnpackPassageActions()
	{
		actionsDictionary.Clear();

		for (int i = 0; i < currentPassage.actions.Length; i++)
		{
			bool isAvailable = true;
            foreach (PassagePreconditions passagePreconditions in passagesPreconditions.passagePreconditions)
            {
				if (passagePreconditions.passageName.Equals(currentPassage.actions[i].passage.name))
                {
                    foreach (string precondition in passagePreconditions.preconditions)
                    {
						if (!visitedPassages.Contains(precondition))
                        {
							isAvailable = false;
							break;
						}
					}

					if (isAvailable == false)
						break;
                }
            }

			if (!visitedPassages.Contains(currentPassage.actions[i].passage.name) && isAvailable)
				actionsDictionary.Add(currentPassage.actions[i].description, currentPassage.actions[i].passage);
		}
	}

	public PassageTextBlock GetNextTextBlock()
    {
		PassageTextBlock textBlock = null;

		if (textBlocks.Count > 0)
        {
			textBlock = textBlocks.Peek();
			textBlocks.Dequeue();
		}

		return textBlock;
    }

	public List<string> GetPassageActions()
	{
		List<string> actionsText = new List<string>();

		if (actionsDictionary.Count > 0)
        {
			foreach (string actionText in actionsDictionary.Keys)
			{
				actionsText.Add(actionText);
			}
		}

		return actionsText;
	}

	public void ChangePassage(string actionDescription)
	{
		if (actionsDictionary.ContainsKey(actionDescription))
		{
			currentPassage = actionsDictionary[actionDescription];
			SetupCurrentPassage();
			//Debug.Log("You head off to passage: " + currentPassage.name);
		}
		else
		{
			Debug.LogError("There is no passage assigned to this action: " + actionDescription);
		}
	}
	#endregion
}
