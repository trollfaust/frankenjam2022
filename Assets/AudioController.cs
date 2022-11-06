using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    AudioMixer mixer;

    [SerializeField]
    LightingColor lightingColor;

    [SerializeField]
    Transform player;

    AudioMixerSnapshot darkSnapshot;
    AudioMixerSnapshot lightSnapshot;
    AudioMixerSnapshot middleSnapshot;

    //float ChangeTime = 1;
    // Start is called before the first frame update
    void Start()
    {
        darkSnapshot = mixer.FindSnapshot("Dark");
        lightSnapshot= mixer.FindSnapshot("Light");
        middleSnapshot = mixer.FindSnapshot("Middle");
    }

    // Update is called once per frame
    void Update()
    {
        var color = lightingColor.GetColorFromWordPos(player.transform.position);
        float average = (color.r + color.g + color.b) / 3;
        AudioMixerSnapshot snapshot = average > 0.5 ? lightSnapshot : darkSnapshot;
        mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { snapshot }, new float[] { 1 }, 0);
    }

    //IEnumerator ChangeTo(AudioMixerSnapshot snapshot)
    //{
    //    mixer.TransitionToSnapshots(new AudioMixerSnapshot[] { snapshot }, new float[] { 1 }, 1);
    //}
}
