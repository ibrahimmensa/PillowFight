using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public enum SoundEffect
{
    BUTTONCLICK,
    PLAYERRELATED
}

public class AudioManager : Singleton<AudioManager>
{
    public AudioSource BGMusic;
    public AudioSource sfx;

    public AudioClip[] sfxAudios;


    private void Start()
    {
        if (PlayerPrefs.GetInt("BGMusic", 0) == 0)//0 means ON, 1 means OFF
        {
            UIManager.Instance.BGMusicToogleOn.SetActive(true);
            UIManager.Instance.BGMusicToogleOff.SetActive(false);
            BGMusic.enabled = true;
            BGMusic.Play();
        }
        else
        {
            UIManager.Instance.BGMusicToogleOn.SetActive(false);
            UIManager.Instance.BGMusicToogleOff.SetActive(true);
            BGMusic.Stop();
            BGMusic.enabled = false;
        }

        if (PlayerPrefs.GetInt("SFX", 0) == 0)//0 means ON, 1 means OFF
        {
            UIManager.Instance.SfxToogleOn.SetActive(true);
            UIManager.Instance.SfxToogleOff.SetActive(false);
        }
        else
        {
            UIManager.Instance.SfxToogleOn.SetActive(false);
            UIManager.Instance.SfxToogleOff.SetActive(true);
        }
    }

    public void Play(SoundEffect s)
    {
        if (PlayerPrefs.GetInt("SFX", 0) == 0)//0 means ON, 1 means OFF
        {
            if (s == SoundEffect.BUTTONCLICK)
            {
                sfx.PlayOneShot(sfxAudios[0]);
            }
        }
    }

    public void PlayButtonClickSfx()
    {
        if (PlayerPrefs.GetInt("SFX", 0) == 0)//0 means ON, 1 means OFF
        {
            sfx.PlayOneShot(sfxAudios[0]);
        }
    }
}
