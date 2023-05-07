using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

public class CodeUI : MonoBehaviour
{
    UIDocument _miMenu;
    //Custom Audio
    public AudioSource audioSourceGm;
    //main_menu
    Button _play_btn;
    Button _audio_opt_btn;
    Button _credits_btn;
    Button _exit_btn;
    //second_opt_menu
    VisualElement _right;
    VisualElement _options;
    VisualElement _sound_opt;
    VisualElement _credits;
    Button _bkmenu;
    Toggle _onoff_sound;
    DropdownField _songs_list;
    Slider _audio_volume;

    /*private void Start()
    {
        audioSourceGm = GetComponent<AudioSource>();
        _onoff_sound.value = audioSourceGm.enabled;
        _audio_volume.value = audioSourceGm.volume;
        _credits.SetEnabled(false);
        _sound_opt.SetEnabled(false);
    }*/

    private void OnEnable()
    {
        _miMenu = GetComponent<UIDocument>();
        VisualElement root = _miMenu.rootVisualElement;
        //main_opt
        _play_btn = root.Q<Button>("playBtn");
        _audio_opt_btn = root.Q<Button>("optionsBtn");
        _credits_btn = root.Q<Button>("creditsBtn");
        _exit_btn = root.Q<Button>("exitBtn");
        
        _right = root.Q<VisualElement>("right");
        _options = root.Q<VisualElement>("options");
        _sound_opt = root.Q<VisualElement>("sound_opt");
        _credits = root.Q<VisualElement>("credits");
        
        _bkmenu = root.Q<Button>("bkmenu");
        _onoff_sound = root.Q<Toggle>("onoff_sound");
        _songs_list = root.Q<DropdownField>("songs-list");
        _audio_volume = root.Q<Slider>("volume");
        //Custom Audio
        audioSourceGm = GetComponent<AudioSource>();
        _onoff_sound.value = audioSourceGm.enabled;
        _audio_volume.value = audioSourceGm.volume;
        //
        _credits.SetEnabled(false);
        _sound_opt.SetEnabled(false);
        //Callbacks
        _play_btn.RegisterCallback<ClickEvent, int>(OpenOptions, 1);
        _audio_opt_btn.RegisterCallback<ClickEvent, int>(OpenOptions, 2);
        _credits_btn.RegisterCallback<ClickEvent, int>(OpenOptions, 3);
        _exit_btn.RegisterCallback<ClickEvent, int>(OpenOptions, 4);

        _onoff_sound.RegisterValueChangedCallback(OnOffSound);
        _songs_list.RegisterValueChangedCallback(MySongsList);
        _audio_volume.RegisterValueChangedCallback(AudioVolume);
        _bkmenu.RegisterCallback<ClickEvent>(CloseOptMenu);

    }

    void OpenOptions(ClickEvent evt, int opt)
    {

        _right.RemoveFromClassList("rightcont_active");
        _right.AddToClassList("rightcont_active");
        switch (opt)
        {
            case 1:
                break;
            case 2:
                Debug.Log("OPTION menu sound_opt" + opt);
                _credits.SetEnabled(false);
                _credits.RemoveFromHierarchy();
                _sound_opt.SetEnabled(true);
                _options.Add(_sound_opt);
                /*_credits.RemoveFromClassList(".menu_opt_active");
                _sound_opt.RemoveFromClassList(".menu_opt_deactive");
                _credits.AddToClassList(".menu_opt_deactive");
                _sound_opt.AddToClassList(".menu_opt_active");*/
                break; 
            case 3:
                _credits.SetEnabled(true);
                _sound_opt.SetEnabled(false);
                _sound_opt.RemoveFromHierarchy();
                _options.Add(_credits);
                Debug.Log("OPTION menu credits" + opt);
                /*_sound_opt.RemoveFromClassList(".menu_opt_active");
                _credits.RemoveFromClassList(".menu_opt_deactive");
                _sound_opt.AddToClassList(".menu_opt_deactive");
                _credits.AddToClassList(".menu_opt_active");*/
                break; 
            case 4:
                break;
        }
    }
    void OnOffSound(ChangeEvent<bool> evt)
    {
        audioSourceGm.enabled = evt.newValue;
        Debug.Log("HandleCallback invoke value " + evt.newValue);
    }

    void MySongsList(ChangeEvent<string> evt)
    {

    }

    void AudioVolume(ChangeEvent<float> evt)
    {
        audioSourceGm.volume = evt.newValue;
    }

    void CloseOptMenu(ClickEvent evt)
    {
        _right.RemoveFromClassList("rightcont_active");
    }


}