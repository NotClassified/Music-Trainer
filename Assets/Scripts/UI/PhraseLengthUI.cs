using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhraseLengthUI : MonoBehaviour
{
    RelativePitchManager manager;

    [SerializeField] TextMeshProUGUI length_text;
    [SerializeField] Button increment;
    [SerializeField] Button decrement;
    int currentLength = 1;

    void Awake()
    {
        manager = FindObjectOfType<RelativePitchManager>();
        increment.onClick.AddListener(delegate { ChangeLength(1); });
        decrement.onClick.AddListener(delegate { ChangeLength(-1); });

        ChangeLength(0); //for setup
    }
    public void ChangeLength(int add)
    {
        if (currentLength + add <= 0)
            return;

        currentLength += add;
        length_text.text = currentLength.ToString();
        manager.SetPhraseLength(currentLength);
    }
}
