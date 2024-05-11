using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // Ссылка на единственный экземпляр AudioManager

    public AudioSource backgroundMusic; // Источник звука для фоновой музыки
    public AudioSource soundEffect; // Источник звука для звуковых эффектов

    public AudioClip[] soundEffectClips;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при загрузке новой сцены
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дублирующий AudioManager
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
            Debug.LogWarning("Неверный индекс для звуковых эффектов.");
        }
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }
}
