using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleDegreeComponent : MonoBehaviour
{
    private void Awake()
    {
        int degree = transform.GetSiblingIndex();
        RelativePitchManager manager = FindObjectOfType<RelativePitchManager>();

        GetComponent<Button>().onClick.AddListener(delegate { manager.ScaleDegreeButtonClick(degree); });
    }
}
