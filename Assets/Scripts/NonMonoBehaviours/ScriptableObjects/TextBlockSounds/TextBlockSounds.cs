using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New TextBlockSounds", menuName = "TextBlock Sounds")]
public class TextBlockSounds : ScriptableObject
{
	#region Variables
	public TextBlockSound[] textBlockSounds;
	#endregion

	#region Methods
	public string GetSoundName(string _textBlockId)
    {
        foreach (TextBlockSound textBlockSound in textBlockSounds)
        {
			if (textBlockSound.textBlockId.Equals(_textBlockId))
            {
                return textBlockSound.soundName;
            }
        }

        return null;
    }
	#endregion
}
