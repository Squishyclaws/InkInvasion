using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMannager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    
    public void SetVolume(float volume)
    {
        mixer.SetFloat("Volume", volume);
    }
}
