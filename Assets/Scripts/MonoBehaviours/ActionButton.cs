using TMPro;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
	#region Methods
	public void OnTouchDown()
    {
		string actionText = GetComponentInParent<TextMeshProUGUI>().text;
		GameManager.Instance.ActionSelected(actionText);
	}
	#endregion
}
