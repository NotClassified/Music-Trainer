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
    Note[] currentScale;

    [SerializeField] Transform scaleDegreeParent;
    [SerializeField] ChanceSet degreeChances;

    [SerializeField] int phraseLength;
    [SerializeField] int phrase_bpm;
    int[] currentPhase;
    int currentPhaseIndex;

    private void Start()
    {
        SelectScale();
    }

    public void PlayScaleCallBack() => StartCoroutine(PlayScale());
    IEnumerator PlayScale()
    {
        for (int i = 0; i < currentScale.Length; i++)
        {
            source[i].Play();
            yield return new WaitForSeconds(60f / bpm);
        }
        for (int i = currentScale.Length - 2; i >= 0; i--)
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

        currentScale = Scales.GetScale(tonic, selectedMode);

        for (int i = 0; i < currentScale.Length; i++)
        {
            source[i].clip = soundFont.GetPitch(currentScale[i].GetPitch());
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
        for (int i = 0; i < currentScale.Length; i++)
        {
            degree_text[i].text = currentScale[i].GetName(false);
        }
    }

    public void ScaleDegreeButtonClick(int degree)
    {
        if (currentPhaseIndex == -1)
        {
            Debug.LogWarning("You didn't play the new phase yet");
            return;
        }
        source[degree].Play();

        if (degree == currentPhase[currentPhaseIndex])
        {
            print(true);
            currentPhaseIndex++;

            if (currentPhaseIndex == currentPhase.Length)
                NewPhrase();
        }
        else
        {
            print(false);
            currentPhaseIndex--;
        }
    }

    public void NewPhrase()
    {
        currentPhase = new int[phraseLength];
        for (int i = 0; i < phraseLength; i++)
        {
            currentPhase[i] = degreeChances.RandomValue();
        }
        currentPhaseIndex = -1;
    }
    public void PlayPhraseCallBack() => StartCoroutine(PlayPhrase());
    IEnumerator PlayPhrase()
    {
        for (int i = 0; i < currentPhase.Length; i++)
        {
            source[currentPhase[i]].Play();
            yield return new WaitForSeconds(60f / phrase_bpm);
        }
        currentPhaseIndex = 0;
    }

    public void SetPhraseLength(int newLength)
    {
        phraseLength = newLength;
        NewPhrase(); //remove for optimization
    }
    
}
