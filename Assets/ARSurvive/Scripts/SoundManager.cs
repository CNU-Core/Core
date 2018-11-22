using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class SoundManager : MonoBehaviour
{
    private static SoundManager _Instance = null;
 
    public static SoundManager I
    {
        get
        {
            if (_Instance == null)
            {
                Debug.Log("instance is null");
            }
            return _Instance;
        }
    }

    void Awake()
    {
        _Instance = this;
    }
 
 
    public int audioSourceCount = 3;
 
    [SerializeField]
    [Header("clips"), Tooltip("오디오 클립들")]
    public AudioClip[] BGMs = new AudioClip[1];
    public AudioClip[] SFXs = new AudioClip[7];
 
 
    private AudioSource BGMsource;
    private AudioSource[] SFXsource;
 
    public delegate void CallBack();
    CallBack BGMendCallBack;
 
    void OnEnable()
    {
        float volume = PlayerPrefs.GetFloat("volumeBGM", 1);
 
        BGMsource = gameObject.AddComponent<AudioSource>();
        BGMsource.volume = volume;
        BGMsource.playOnAwake = false;
        BGMsource.loop = true;
 
        //sfx 소스 초기화
        SFXsource = new AudioSource[audioSourceCount];
 
        volume = PlayerPrefs.GetFloat("volumeSFX", 1);
 
        for (int i = 0; i < SFXsource.Length; i++)
        {
            SFXsource[i] = gameObject.AddComponent<AudioSource>();
            SFXsource[i].playOnAwake = false;
            SFXsource[i].volume = volume;
        }
 
 
        ChangeBGM("Game", false);
    }
 
    /**********SFX***********/
 
    public void PlaySFX(string name, bool loop = false, float pitch = 1)//효과음 재생
    {
        for (int i = 0; i < SFXs.Length; i++)
        {
            if (SFXs[i].name == name)
            {
                AudioSource a = GetEmptySource();
                a.loop = loop;
                a.pitch = pitch;
                a.clip = SFXs[i];
                a.Play();
                return;
            }
        }
    }
 
    public void StopSFXByName(string name)
    {
        for (int i = 0; i < SFXsource.Length; i++)
        {
            if (SFXsource[i].clip.name == name)
                SFXsource[i].Stop();
        }
    }
 
    private AudioSource GetEmptySource()//비어있는 오디오 소스 반환
    {
        int lageindex = 0;
        float lageProgress = 0;
        for (int i = 0; i < SFXsource.Length; i++)
        {
            if (!SFXsource[i].isPlaying)
            {
                return SFXsource[i];
            }
 
            //만약 비어있는 오디오 소스를 못찿으면 가장 진행도가 높은 오디오 소스 반환(루프중인건 스킵)
 
            float progress = SFXsource[i].time / SFXsource[i].clip.length;
            if (progress > lageProgress && !SFXsource[i].loop)
            {
                lageindex = i;
                lageProgress = progress;
            }
        }
        return SFXsource[lageindex];
    }
 
    /**********BGM***********/
 
    private AudioClip changeClip;//바뀌는 클립
    private bool isChanging = false;
    private float startTime;
 
 
    [SerializeField]
    [Header("Changing speed"), Tooltip("브금 바꾸는 속도")]
    public float ChangingSpeed;
 
    public void ChangeBGM(string name, bool isSmooth = false, CallBack callback = null)//브금 변경 (브금이름 , 부드럽게 바꾸기)
    {
        BGMendCallBack = callback;
 
        changeClip = null;
        for (int i = 0; i < BGMs.Length; i++)//브금 클립 탐색
        {
            if (BGMs[i].name == name)
            {
                changeClip = BGMs[i];
            }
        }
 
        if (changeClip == null)//없으면 탈주
            return;
 
        if (!isSmooth)
        {
            BGMsource.clip = changeClip;
            BGMsource.Play();
        }
        else
        {
            startTime = Time.time;
            isChanging = true;
        }
    }
 
    public string GetRandomBGMName()
    {
        return BGMs[Random.Range(0, BGMs.Length)].name;
    }
 
    private void Update()
    {
        if (!isChanging) return;
 
        float progress = (Time.time - startTime) * ChangingSpeed;//부드러운 오디오 전환
        BGMsource.volume = Mathf.Lerp(PlayerPrefs.GetFloat("volumeBGM", 1), 0, progress);
 
        if (progress > 1)
        {
            isChanging = false;
            BGMsource.volume = PlayerPrefs.GetFloat("volumeBGM", 1);
            BGMsource.clip = changeClip;
            BGMsource.Play();
        }
    }
 
    public void StopBGM()
    {
        BGMsource.Stop();
    }
 
    public void SetPitch(float pitch)
    {
        BGMsource.pitch = pitch;
    }
 
 
    //비주얼라이저용 오디오 샘플
    public float[] GetAudioSample(int sampleCount, FFTWindow fft)
    {
        float[] samples = new float[sampleCount];
 
        BGMsource.GetSpectrumData(samples, 0, fft);
 
        if (samples != null)
            return samples;
        else
            return null;
    }
 
    //볼륨
 
    public void changeBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("volumeBGM", volume);
        BGMsource.volume = volume;
    }
 
 
    public void changeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("volumeSFX", volume);
        for (int i = 0; i < SFXsource.Length; i++)
        {
            SFXsource[i].volume = volume;
        }
    }
}
