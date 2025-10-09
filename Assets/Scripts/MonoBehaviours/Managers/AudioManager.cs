using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
	#region Variables
	public AudioMixer audioMixer;
	public TextBlockSounds textBlockSounds;
	public Sound[] sounds;
	#endregion

	#region Unity Event Functions
	protected override void Awake()
	{
		base.Awake();
	}

    private void Start()
    {
        
    }

    private void Update()
	{
		
	}
	#endregion

	#region Methods
	public AudioSource PlayMusic(string _name)
    {
		Sound sound = GetSound(_name);
		if (sound == null)
			return null;

		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		SetAudioSource(audioSource, sound);
		audioSource.Play();

		return audioSource;
	}

	// Used for non-persistent sounds like sfx and ambiance etc.
	public GameObject PlaySoundAtPoint(string _name, Vector3 _position)
    {
		Sound sound = GetSound(_name);
		if (sound == null)
			return null;

		GameObject soundGameObject = new GameObject("Sound");
		soundGameObject.transform.position = _position;
		AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
		SetAudioSource(audioSource, sound);
		if (audioSource.loop == false)
			Destroy(soundGameObject, audioSource.clip.length); // HAS TO BE REMOVED IF POOLING IS ADDED!!!

		audioSource.Play();
		return soundGameObject;
	}

	public void PlayTextBlockSound(string _textBlockId)
    {
		string soundName = textBlockSounds.GetSoundName(_textBlockId);
		if (!String.IsNullOrEmpty(soundName))
		{
			PlaySoundAtPoint(soundName, Vector3.zero);
		}
	}

	public Sound GetSound(string _name)
    {
		Sound sound = Array.Find(sounds, s => s.name == _name);
		if (sound == null)
        {
			Debug.LogWarning("Sound: " + _name + " not found!");
			return null;
        }

		return sound;
    }

	public void SetAudioSource(AudioSource _audioSource, Sound _sound)
    {
		_audioSource.clip = _sound.clip;
		_audioSource.outputAudioMixerGroup = _sound.mixerGroup;
		_audioSource.priority = _sound.priority;
		_audioSource.volume = _sound.volume;

		if (_sound.pitchRange > 0)
			_audioSource.pitch = RandomizePitch(_sound.pitch, _sound.pitchRange);
		else
			_audioSource.pitch = _sound.pitch;

		_audioSource.spatialBlend = _sound.spatialBlend;
		_audioSource.loop = _sound.loop;
	}

	public float RandomizePitch(float _pitch, float _pitchRange)
    {
		return UnityEngine.Random.Range(_pitch - _pitchRange, _pitch + _pitchRange);
    }

	public string GetRandomString(params string[] _strings)
    {
		int randomIndex = UnityEngine.Random.Range(0, _strings.Length);
		return _strings[randomIndex];
    }

	/*public void SetSoundVolume(float _volume)
    {
		audioMixer.SetFloat("soundVolume", Mathf.Log10(_volume) * 20);
    }

	public void SetMusicVolume(float _volume)
	{
		audioMixer.SetFloat("musicVolume", Mathf.Log10(_volume) * 20);
	}
	public float GetAudioMixerParameter(string _parameter)
	{
		float value;
		bool result = audioMixer.GetFloat(_parameter, out value);
		if (result)
			return value;
		else
			return 0f;
	}*/
	#endregion
}
