using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class IntervalManager : MonoBehaviour
{
    //0C  1C#  2D  3D#  4E  5F  6F#  7G  8G#  9A

    [SerializeField] string[] letters;

    [SerializeField] string[] intervalNames;
    [SerializeField] TextMeshProUGUI optionA;
    [SerializeField] TextMeshProUGUI optionB;
    int intervalChoice;
    int[] intervalSizes;
    int correctOption;
    int startingNote;

    [SerializeField] AudioSource audioSource1;
    [SerializeField] AudioSource audioSource2;
    [SerializeField] AudioClip[] pitches;
    [SerializeField] float intervalPlayDelay;
    bool isPlayingAudio = false;

    [SerializeField] TMP_Dropdown intervalOptionsA;
    [SerializeField] TMP_Dropdown intervalOptionsB;

    private void Start()
    {
        List<string> intervalDropDownOptions = new List<string>();
        foreach (string interval in intervalNames)
        {
            intervalDropDownOptions.Add(interval);
        }
        intervalOptionsA.AddOptions(intervalDropDownOptions);
        intervalOptionsB.AddOptions(intervalDropDownOptions);

        intervalSizes = new int[2];
    }

    public void BackToMenu() => SceneManager.LoadScene(0);

    public void ButtonClick(string button)
    {
        switch (button)
        {
            case "New":
                SetIntervalOptions();
                SetStartingNote();
                StartCoroutine(PlayInterval());
                break;
            case "Play":
                StartCoroutine(PlayInterval());
                break;
            case "Show":
                ShowCorrectOption();
                break;
            default:
                Debug.LogError("button not found");
                break;
        }
    }

    public void SetIntervalChoice(int choice) => intervalChoice = choice - 1;

    void SetIntervalOptions()
    {
        correctOption = Random.Range(0, 2);

        if (intervalOptionsA.value == 0)
        {
            intervalSizes[0] = Random.Range(0, intervalNames.Length);
            intervalOptionsA.value = intervalSizes[0] + 1;
        }
        else
        {
            intervalSizes[0] = intervalOptionsA.value - 1;
        }
        if (intervalOptionsB.value == 0)
        {
            intervalSizes[1] = RandomRangeException(0, intervalNames.Length, intervalSizes[0]);
            intervalOptionsB.value = intervalSizes[1] + 1;
        }
        else
        {
            intervalSizes[1] = intervalOptionsB.value - 1;
        }

        //optionA.text = intervalNames[intervalSizes[0]];
        //optionB.text = intervalNames[intervalSizes[1]];
        optionA.color = Color.black;
        optionB.color = Color.black;
    }

    void SetStartingNote()
    {
        startingNote = Random.Range(0, pitches.Length - intervalSizes[correctOption] - 1);
    }

    int RandomRangeException(int min, int max, int exception)
    {
        if (Random.Range(0, 2) == 0 && exception != min)
        {
            return Random.Range(min, exception);
        }
        else if (exception + 1 != max)
        {
            return Random.Range(exception + 1, max);
        }

        return Random.Range(min, exception);
    }

    IEnumerator PlayInterval()
    {
        if (!isPlayingAudio)
        {
            isPlayingAudio = true;

            audioSource1.clip = pitches[startingNote];
            audioSource2.clip = pitches[startingNote + intervalSizes[correctOption] + 1];

            audioSource1.Play();
            audioSource2.Play();
            yield return new WaitForSeconds(intervalPlayDelay * 2);
            audioSource1.Play();
            yield return new WaitForSeconds(intervalPlayDelay);
            audioSource2.Play();
            yield return new WaitForSeconds(intervalPlayDelay);
            audioSource1.Play();
            audioSource2.Play();

            isPlayingAudio = false;
        }
    }

    void ShowCorrectOption()
    {
        optionA.color = correctOption == 0 ? Color.green : Color.red;
        optionB.color = correctOption == 1 ? Color.green : Color.red;
    }
}
