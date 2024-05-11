using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // ������ �� ������������ ��������� AudioManager

    public AudioSource backgroundMusic; // �������� ����� ��� ������� ������
    public AudioSource soundEffect; // �������� ����� ��� �������� ��������

    public AudioClip[] soundEffectClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �� ���������� ��� �������� ����� �����
        }
        else
        {
            Destroy(gameObject); // ���������� ����������� AudioManager
        }
    }

    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
        backgroundMusic.clip = musicClip;
        backgroundMusic.Play();
    }

    public void PlaySoundEffect(AudioClip soundClip)
    {
        soundEffect.PlayOneShot(soundClip);
    }

    public void PlaySoundEffectByIndex(int index)
    {
        if (index >= 0 && index < soundEffectClips.Length)
        {
            soundEffect.PlayOneShot(soundEffectClips[index]);
        }
        else
        {
            Debug.LogWarning("�������� ������ ��� �������� ��������.");
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }
}
