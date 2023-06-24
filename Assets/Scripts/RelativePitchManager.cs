using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Music;

public class RelativePitchManager : MonoBehaviour
{
    [SerializeField] NoteAudio soundFont;
    [SerializeField] AudioSource[] source;
    [SerializeField] int bpm;
    Note[] scale;

    IEnumerator Start()
    {
        scale = Scales.GetScale(new(NoteLetter.F, Accidental.Sharp), DiatonicMode.Minor);
        for (int i = 0; i < scale.Length; i++)
        {
            source[i].clip = soundFont.GetPitch(scale[i].GetPitch());
        }

        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;

        for (int i = 0; i < scale.Length; i++)
        {
            source[i].Play();
            yield return new WaitForSeconds(60f / bpm);
        }
        for (int i = scale.Length - 2; i >= 0; i--)
        {
            source[i].Play();
            yield return new WaitForSeconds(60f / bpm);
        }
    }
}
