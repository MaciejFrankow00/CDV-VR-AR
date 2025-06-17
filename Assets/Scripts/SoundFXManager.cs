using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;

public enum SoundType
{
    SLASH,
    HURT,
    ALERT,
    BUTTON,
    START,
    EXIT
}

[ExecuteInEditMode]
public class SoundFXManager : MonoBehaviour
{
    public static SoundFXManager instance;
    public AudioMixerGroup mixerGroup;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private SoundList[] soundList;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PlaySound2D(SoundType audioClip, Transform spawnTransform, float volume = 1f)
    {
        //choose random audio clip from list assigned to type
        AudioClip clip = instance.soundList[(int)audioClip].Sounds[UnityEngine.Random.Range(0, instance.soundList[(int)audioClip].Sounds.Length)];

        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assing the audioClip
        audioSource.clip = clip;

        //assign group of sound
        audioSource.outputAudioMixerGroup = mixerGroup;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

        Debug.Log($"[SFX] Odtwarzam: {soundList[(int)audioClip].name} na {spawnTransform.name}");
    }

    public void PlaySound3D(SoundType audioClip, Transform spawnTransform, float volume = 1f, float minDistance = 10f, float maxDistance = 100f)
    {
        //choose random audio clip from list assigned to type
        AudioClip clip = instance.soundList[(int)audioClip].Sounds[UnityEngine.Random.Range(0, instance.soundList[(int)audioClip].Sounds.Length)];

        //spawn in gameObject
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assing the audioClip
        audioSource.clip = clip;

        //assign group of sound
        audioSource.outputAudioMixerGroup = mixerGroup;

        //assign volume
        audioSource.volume = volume;

        //made audio source 3D
        audioSource.spatialBlend = 1f;

        //turn off Doppler effect
        audioSource.dopplerLevel = 0f;

        //change rolloff mode to custom
        audioSource.rolloffMode = AudioRolloffMode.Custom;

        //assign key frames to custom curve
        AnimationCurve customCurve = new AnimationCurve(
            new Keyframe(0f, volume),
            new Keyframe(1f, 0f)
        );

        //assign min disctacne from audio source
        audioSource.minDistance = minDistance;

        //assign max distance from audio source
        audioSource.maxDistance = maxDistance;

        //play sound
        audioSource.Play();

        //get length of sound FX clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

        Debug.Log($"[SFX] Odtwarzam: {soundList[(int)audioClip].name} na {spawnTransform.name}");
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for(int i = 0; i < soundList.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif

    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string name;
        [SerializeField] private AudioClip[] sounds;

        public AudioClip[] Sounds => sounds;
    }
}
