using UnityEngine;

[CreateAssetMenu(fileName = "New Passage", menuName = "Passage")]
public class Passage : ScriptableObject
{
	#region Variables
	public new string name;
	public PassageTextBlock[] textBlocks;
	public PassageAction[] actions;
	#endregion

	#region Unity Event Functions
	private void Start()
	{
		
	}
	
	private void Update()
	{
		
	}
	#endregion

	#region Methods
	#endregion
}
