using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class codeAudioLevel1 : CustomAudioGame
{
    // Start is called before the first frame update
    void Start()
    {
        audioSourceGm = GetComponent<AudioSource>();
        audioSourceGm.enabled = true;

        audioSourceGm.enabled = CodeUI.activeAudio;
        Debug.Log("audioSourceGm.enabled value " + audioSourceGm.enabled);
        Debug.Log("activeAudio value " + CodeUI.activeAudio);
    }

    // Update is called once per frame
    void Update()
    {
        //audioSourceGm.enabled = onoff_sound.value;
    }
}
