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
    int currentSourceIndex;
    [SerializeField] int bpm;
    Note[] currentScale;

    [SerializeField] Transform scaleDegreeParent;
    [SerializeField] ChanceSet degreeChances;

    [SerializeField] int phraseLength;
    [SerializeField] int phrase_bpm;
    int[] currentPhrase;
    int currentPhraseIndex;

    [SerializeField] TextMeshProUGUI phraseIndex_text;

    private void Start()
    {
        SelectScale();
    }

    void PlayNote(Note note) => PlayNote(note.GetPitch());
    void PlayNote(int pitch)
    {
        source[currentSourceIndex].clip = soundFont.GetPitch(pitch);
        source[currentSourceIndex].Play();

        if (++currentSourceIndex == source.Length)
            currentSourceIndex = 0;
    }

    public void PlayScaleCallBack() => StartCoroutine(PlayScale());
    IEnumerator PlayScale()
    {
        for (int i = 0; i < currentScale.Length; i++)
        {
            PlayNote(currentScale[i]);
            yield return new WaitForSeconds(60f / bpm);
        }
        for (int i = currentScale.Length - 2; i >= 0; i--)
        {
            PlayNote(currentScale[i]);
            yield return new WaitForSeconds(60f / bpm);
        }
    }

    public void SelectScale()
    {
        Note tonic = new();
        DiatonicMode selectedMode = DiatonicMode.Major;

        //set the letter, accidental, and octave of "tonic", and set the scale's mode
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
        PlayNote(currentScale[degree]);
        if (currentPhraseIndex == -1)
        {
            Debug.LogWarning("You didn't play the new phase yet");
            return;
        }

        if (degree == currentPhrase[currentPhraseIndex])
        {
            currentPhraseIndex++;

            if (currentPhraseIndex == currentPhrase.Length)
                NewPhrase();
            else
                UpdatePhraseIndexText(Color.green);
        }
        else
        {
            currentPhraseIndex = 0;
            UpdatePhraseIndexText(Color.red);
        }
    }

    public void NewPhrase()
    {
        currentPhrase = new int[phraseLength];
        for (int i = 0; i < phraseLength; i++)
        {
            currentPhrase[i] = degreeChances.RandomValue();
        }
        currentPhraseIndex = -1;
        UpdatePhraseIndexText(Color.black);
    }
    public void PlayPhraseCallBack() => StartCoroutine(PlayPhrase());
    IEnumerator PlayPhrase()
    {
        for (int i = 0; i < currentPhrase.Length; i++)
        {
            PlayNote(currentScale[currentPhrase[i]]);
            yield return new WaitForSeconds(60f / phrase_bpm);
        }
        currentPhraseIndex = 0;
        UpdatePhraseIndexText(Color.black);
    }

    public void SetPhraseLength(int newLength)
    {
        phraseLength = newLength;
        NewPhrase(); //remove for optimization
    }
    
    void UpdatePhraseIndexText(Color newColor)
    {
        int index = currentPhraseIndex;
        index = index < 0 ? 0 : index; //when currentPhraseIndex is -1, show 0 instead

        phraseIndex_text.text = index + "/" + phraseLength;
        phraseIndex_text.color = newColor;
    }
}
