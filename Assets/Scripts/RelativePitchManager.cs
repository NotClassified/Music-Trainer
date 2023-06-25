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

    private void Start()
    {
        scale = Scales.GetScale(new(NoteLetter.C), DiatonicMode.Major);
        SetSourceClips();
    }

    public void PlayerScaleCallBack() => StartCoroutine(PlayScale());
    IEnumerator PlayScale()
    {
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

    public void SelectScale()
    {
        Note tonic = new();
        DiatonicMode selectedMode = DiatonicMode.Major;

        DropDownComponent[] dropDownComponents = FindObjectsOfType<DropDownComponent>();
        foreach (DropDownComponent component in dropDownComponents)
        {
            switch (component.optionType)
            {
                case DropDownOptions.Letter:
                    tonic.letter = Scales.AllLetters[component.dropDownUI.value];
                    break;
                case DropDownOptions.Accidental:
                    tonic.accidental = (Accidental)(component.dropDownUI.value - 1);
                    break;
                case DropDownOptions.Octave:
                    tonic.octave = component.dropDownUI.value + 2;
                    break;
                case DropDownOptions.Mode:
                    selectedMode = (DiatonicMode)component.dropDownUI.value;
                    break;
                default:
                    break;
            }
        }

        scale = Scales.GetScale(tonic, selectedMode);
        SetSourceClips();
    }

    void SetSourceClips()
    {
        for (int i = 0; i < scale.Length; i++)
        {
            source[i].clip = soundFont.GetPitch(scale[i].GetPitch());
        }
    }
}
