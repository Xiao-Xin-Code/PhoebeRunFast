using System.Collections;
using Frame;
using QMVC;
using UnityEngine;

public class AudioSystem : AbstractSystem
{
	protected override void OnInit()
	{
		
	}





	AudioSource _bgmSource;
	float _bgmVolume;
	float _bgmFadeTimeout = 0.5f;

	Coroutine _bgmFadeCoroutine;




	public void PlayBGM(AudioClip clip, bool isLoop = true)
	{
		if (clip == null) return;

		if (_bgmFadeCoroutine != null) 
		{
			MonoService.Instance.StopCoroutine(_bgmFadeCoroutine);
		}

		if (_bgmSource.isPlaying) 
		{
			_bgmFadeCoroutine = MonoService.Instance.StartCoroutine(FadeOutAndSwitchBGM(clip, isLoop));
		}
		else
		{
			_bgmSource.clip = clip;
			_bgmSource.loop = isLoop;
			_bgmSource.volume = 0;

			_bgmSource.Play();
			_bgmFadeCoroutine = MonoService.Instance.StartCoroutine(FadeInBGM());
		}
	}






	/// <summary>
	/// 渐入
	/// </summary>
	/// <returns></returns>
	IEnumerator FadeInBGM()
	{
		float currentVolume = 0;
		while(currentVolume < _bgmVolume)
		{
			currentVolume += Time.deltaTime / _bgmFadeTimeout;
			currentVolume = Mathf.Min(currentVolume, _bgmVolume);
			_bgmSource.volume = currentVolume;
			yield return null;
		}
		_bgmFadeCoroutine = null;
	}

	/// <summary>
	/// 渐出
	/// </summary>
	/// <returns></returns>
	IEnumerator FadeOutBGM()
	{
		float currentVolume = _bgmSource.volume;
		while (currentVolume > 0) 
		{
			currentVolume -= Time.deltaTime / _bgmFadeTimeout;
			currentVolume = Mathf.Max(currentVolume, 0);
			_bgmSource.volume = currentVolume;
			yield return null;
		}

		_bgmSource.Stop();
		_bgmSource.clip = null;
		_bgmFadeCoroutine = null;
	}

	IEnumerator FadeOutAndSwitchBGM(AudioClip newClip, bool isLoop)
	{
		yield return FadeOutBGM();
		_bgmSource.clip = newClip;
		_bgmSource.loop = isLoop;
		_bgmSource.volume = 0;
		_bgmSource.Play();
		yield return FadeInBGM();
	}

}
