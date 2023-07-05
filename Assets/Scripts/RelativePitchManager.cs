using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Music;

public class RelativePitchManager : MonoBehaviour
{
    [SerializeField] NoteAudio soundFont;
    [SerializeField] AudioSource[] source;
    [SerializeField] int bpm;
    Note[] scale;

    [SerializeField] Transform scaleDegreeParent;
    [SerializeField] ChanceSet degreeChances;

    [SerializeField] int phraseLength;
    [SerializeField] int phrase_bpm;
    int[] phrase;

    private void Start()
    {
        SelectScale();
    }

    public void PlayScaleCallBack() => StartCoroutine(PlayScale());
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

        for (int i = 0; i < scale.Length; i++)
        {
            source[i].clip = soundFont.GetPitch(scale[i].GetPitch());
        }
        UpdateScaleDegreeUI();
        NewPhrase();
    }

    void UpdateScaleDegreeUI()
    {
        TextMeshProUGUI[] degree_text = new TextMeshProUGUI[scaleDegreeParent.childCount];
        for (int i = 0; i < scaleDegreeParent.childCount; i++)
        {
            degree_text[i] = scaleDegreeParent.GetChild(i).GetComponentInChildren<TextMeshProUGUI>();
        }
        for (int i = 0; i < scale.Length; i++)
        {
            degree_text[i].text = scale[i].GetName(false);
        }
    }

    public void ScaleDegreeButtonClick(int degree)
    {
        print(scale[degree].GetName(true));
    }

    public void NewPhrase()
    {
        phrase = new int[phraseLength];
        for (int i = 0; i < phraseLength; i++)
        {
            phrase[i] = degreeChances.RandomValue();
        }
    }
    public void PlayPhraseCallBack() => StartCoroutine(PlayPhrase());
    IEnumerator PlayPhrase()
    {
        for (int i = 0; i < phrase.Length; i++)
        {
            source[phrase[i]].Play();
            yield return new WaitForSeconds(60f / phrase_bpm);
        }
    }
}
