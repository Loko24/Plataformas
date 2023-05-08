using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomAudioGame : MonoBehaviour
{
    //Custom Audio
    public AudioSource audioSourceGm;
    public AudioClip[] customSongs;
    public Toggle onoff_sound;
    public DropdownField songs_list;
    public Slider audio_volume;

    public void OnOffSound(ChangeEvent<bool> evt)
    {
        audioSourceGm.enabled = evt.newValue;
        CodeUI.activeAudio = evt.newValue;
        Debug.Log("HandleCallback invoke value " + evt.newValue);
        Debug.Log("audioSourceGm.enabled value " + audioSourceGm.enabled);
        Debug.Log("activeAudio value " + CodeUI.activeAudio);
    }

    public void MySongsList(ChangeEvent<string> evt)
    {
        Debug.Log("HandleCallback invoke value " + evt.newValue);
    }

    public void AudioVolume(ChangeEvent<float> evt)
    {
        audioSourceGm.volume = evt.newValue;
    }
}
