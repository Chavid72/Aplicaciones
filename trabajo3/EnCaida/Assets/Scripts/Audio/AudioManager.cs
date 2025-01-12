using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SoundType
{
    Banda,
    Globo,
    Golpe,
    MenuBotones,
    Muerte,
    PowerUp
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    private static AudioManager instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        if (instance != null) Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    } 
    
    

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}
