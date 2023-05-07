using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.UIElements;

public class CodeUI : MonoBehaviour
{
    UIDocument miMenu;
    //main_menu
    Button play_btn;
    Button audio_opt_btn;
    Button credits_btn;
    Button exit_btn;
    //second_opt_menu
    VisualElement right;
    VisualElement sound_opt;
    VisualElement credits;
    Button bkmenu;
    Toggle onoff_sound;
    DropdownField songs_list;
    Slider audio_volumen;


    private void OnEnable()
    {
        miMenu = GetComponent<UIDocument>();
        VisualElement root = miMenu.rootVisualElement;
        //main_opt
        play_btn = root.Q<Button>("playBtn");
        audio_opt_btn = root.Q<Button>("optionsBtn");
        credits_btn = root.Q<Button>("creditsBtn");
        exit_btn = root.Q<Button>("exitBtn");
        
        right = root.Q<VisualElement>("right");
        sound_opt = root.Q<VisualElement>("sound_opt");
        credits = root.Q<VisualElement>("credits");
        
        bkmenu = root.Q<Button>("bkmenu");
        //onoff_sound = GetComponent<Toggle>();
        onoff_sound = root.Q<Toggle>("onoff_sound");
        songs_list = root.Q<DropdownField>("songs-list");
        audio_volumen = root.Q<Slider>("volumen");
        //Callbacks
        play_btn.RegisterCallback<ClickEvent, int>(openOptions, 1);
        audio_opt_btn.RegisterCallback<ClickEvent, int>(openOptions, 2);
        credits_btn.RegisterCallback<ClickEvent, int>(openOptions, 3);
        exit_btn.RegisterCallback<ClickEvent, int>(openOptions, 4);

        /*onoff_sound.onValueChanged.AddListener(delegate {
            onOffSound(onoff_sound);
        });*/
        onoff_sound.RegisterValueChangedCallback(onOffSound);
        songs_list.RegisterValueChangedCallback(mySongsList);
        audio_volumen.RegisterValueChangedCallback(audioVolumen);
        bkmenu.RegisterCallback<ClickEvent>(closeOptMenu);

    }

    void openOptions(ClickEvent evt, int opt)
    {

        right.RemoveFromClassList("rightcont_active");
        right.AddToClassList("rightcont_active");
        switch (opt)
        {
            case 1:
                break;
            case 2:
                Debug.Log("OPTION menu sound_opt" + opt);
                credits.RemoveFromClassList(".menu_opt_active");
                sound_opt.RemoveFromClassList(".menu_opt_deactive");
                credits.AddToClassList(".menu_opt_deactive");
                sound_opt.AddToClassList(".menu_opt_active");
                break; 
            case 3:
                Debug.Log("OPTION menu credits" + opt);
                sound_opt.RemoveFromClassList(".menu_opt_active");
                credits.RemoveFromClassList(".menu_opt_deactive");
                sound_opt.AddToClassList(".menu_opt_deactive");
                credits.AddToClassList(".menu_opt_active");
                break; 
            case 4:
                break;
        }
    }
    void onOffSound(ChangeEvent<bool> evt)
    {
        Debug.Log("HandleCallback invoke value " + evt.newValue);
    }

    void mySongsList(ChangeEvent<string> evt)
    {

    }

    void audioVolumen(ChangeEvent<float> evt)
    {

    }

    void closeOptMenu(ClickEvent evt)
    {
        right.RemoveFromClassList("rightcont_active");
    }


}