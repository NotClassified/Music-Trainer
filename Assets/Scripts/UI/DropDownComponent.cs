using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Music;

public enum DropDownOptions
{
    Letter, Accidental, Octave, Mode
}
public class DropDownComponent : MonoBehaviour
{
    public DropDownOptions optionType;
    public TMP_Dropdown dropDownUI;

    private void Awake()
    {
        dropDownUI = GetComponent<TMP_Dropdown>();
    }

    private void Start()
    {
        switch (optionType)
        {
            case DropDownOptions.Letter:
                List<string> letterOptions = new List<string>();
                for (int i = 0; i < Scales.AllLetters.Length; i++)
                {
                    letterOptions.Add(Scales.AllLetters[i].ToString());
                }
                dropDownUI.AddOptions(letterOptions);
                break;
            case DropDownOptions.Accidental:
                /*accidental sprites should already be added*/
                break;
            case DropDownOptions.Octave:
                List<string> octaveOptions = new List<string>();
                for (int i = 2; i <= 5; i++)
                {
                    octaveOptions.Add(i.ToString());
                }
                dropDownUI.AddOptions(octaveOptions);
                break;
            case DropDownOptions.Mode:
                List<string> modeOptions = new List<string>();
                for (int i = 0; i < Scales.diatonicModeAmount; i++)
                {
                    modeOptions.Add( ((DiatonicMode)i).ToString() );
                }
                dropDownUI.AddOptions(modeOptions);
                break;
            default:
                Debug.LogError(optionType.GetType().FullName + " options have not been set up");
                break;
        }
    }
}
