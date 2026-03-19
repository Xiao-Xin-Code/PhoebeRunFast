using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : BaseController
{
	

	protected override void Init()
	{
		
	}



	AudioSource audioSource;


	public void Play(AudioClip clip)
	{
		audioSource.Stop();

		audioSource.clip = clip;
		audioSource.Play();
	}

	

	//AudioSource 结束


}
