using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NotationManager : MonoBehaviour
{
    static NotationManager instance;
    ///<summary> 1=C4, 2=D4...15=C6</summary>
    Dictionary<int, float> pitchTrebleCleffPositions;
    [SerializeField] float firstTrebleCleffPitchPosition;
    ///<summary> 1=C2, 2=D2...15=C4</summary>
    Dictionary<int, float> pitchBassCleffPositions;
    [SerializeField] float firstBassCleffPitchPosition;
    private float staffLineDistance = 9.7f;
    private float rhythmPosition = -360f;

    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject noteFlippedPrefab;
    [SerializeField] GameObject ledgerLinePrefab;
    [SerializeField] Transform notesParent;
    [SerializeField] Transform ledgerLinesParent;

    [SerializeField] float sameIntervalChance;
    [SerializeField] int[] intervalChances;
    [SerializeField] TMP_InputField sameIntervalChanceInputField;

    [SerializeField] Transform intervalChancesParent;
    private int intervalInputFieldSelected;

    ///<summary> 0-Melody, 1-Chords </summary>
    int exampleMode = 0;

    private bool trebleClefActive;
    private bool bassClefActive;

    private void Awake()
    {
        instance = this;
        pitchTrebleCleffPositions = new Dictionary<int, float>();
        pitchBassCleffPositions = new Dictionary<int, float>();
    }

    private void Start()
    {
        SetStaffPitchPositions(pitchTrebleCleffPositions, firstTrebleCleffPitchPosition);
        SetStaffPitchPositions(pitchBassCleffPositions, firstBassCleffPitchPosition);

        ChangeMode(0);
        ToggleClefs(0);
    }

    ///<summary> 0-Melody, 1-Chords </summary>
    public void ChangeMode(int mode)
    {
        exampleMode = mode;

        intervalChancesParent.gameObject.SetActive(mode == 0);
        sameIntervalChanceInputField.gameObject.SetActive(mode == 0);
    }

    ///<summary> 0-Treble+Bass, 1-Treble CLef, 2-Bass Clef  </summary>
    public void ToggleClefs(int clefMode)
    {
        switch (clefMode)
        {
            case 0:
                trebleClefActive = true;
                bassClefActive = true;
                break;
            case 1:
                trebleClefActive = true;
                bassClefActive = false;
                break;
            case 2:
                trebleClefActive = false;
                bassClefActive = true;
                break;
            default:
                Debug.LogError("Invalid Mode");
                break;
        }
    }

    void SetStaffPitchPositions(Dictionary<int, float> clefPitchPos, float firstPos)
    {
        //set all note pitch positions (y-axis)
        float nextPitchPosition = firstPos;
        for (int i = 1; i < 16; i++)
        {
            clefPitchPos.Add(i, nextPitchPosition);
            nextPitchPosition += staffLineDistance / 2;
        }
    }

    ///<summary> check if note's pitch is "above", "below", or "middle" of the staff </summary>
    string NotePositionOnStaff(int pitch, string clef)
    {
        if (clef.Equals("treble"))
        {
            if (pitch <= 1)
                return "below";
            if (pitch >= 13)
                return "above";
        }
        else if (clef.Equals("bass"))
        {
            if (pitch <= 3)
                return "below";
            if (pitch >= 15)
                return "above";
        }
        else
            Debug.LogError("invalid clef");

        return "middle"; //in the staff
    }

    ///<summary> input field for setting the percent chance of same intervals occuring </summary>
    public void SetSameIntervalChance(string input)
    {
        if(input != "")
        {
            int chance = int.Parse(input);
            if (chance >= 0 && chance <= 100)
                sameIntervalChance = chance;
            else
                sameIntervalChanceInputField.text = sameIntervalChance.ToString();
        }
        else
            sameIntervalChance = 0;
    }

    public void SetIntervalInputFieldSelected(int interval)
    {
        intervalInputFieldSelected = interval;
    }
    public void SetIntervalChance(string input)
    {
        int interval = intervalInputFieldSelected - 1; //get the interval the user is changing, -1 for indicies
        if (interval >= 0 && interval < intervalChances.Length)
        {
            if(input != "")
                intervalChances[interval] = int.Parse(input);
            else
                intervalChances[interval] = 0;

            SetIntervalPercentages();
        }
        else
            Debug.LogError("interval out of bounds, must be: [0, " + intervalChances.Length + ")");
    }
    void SetIntervalPercentages()
    {
        float total = 0;
        foreach (int chances in intervalChances)
        {
            total += chances;
        }

        if (total > 0)
        {
            for (int i = 1; i < intervalChancesParent.childCount; i++)
            {
                int percentage = Mathf.RoundToInt((intervalChances[i - 1] / total) * 100);

                TextMeshProUGUI percentText = intervalChancesParent.GetChild(i).Find("Percentage").GetComponent<TextMeshProUGUI>();
                percentText.text = "%" + percentage;
            }
        }
        else
        {
            int percentage = Mathf.RoundToInt((1f / intervalChances.Length) * 100);
            for (int i = 1; i < intervalChancesParent.childCount; i++)
            {
                TextMeshProUGUI percentText = intervalChancesParent.GetChild(i).Find("Percentage").GetComponent<TextMeshProUGUI>();
                percentText.text = "%" + percentage;
            }
        }
    }

    public void GenerateExample()
    {
        ClearExample();

        if (trebleClefActive)
            RandomExample("treble");
        if (bassClefActive)
            RandomExample("bass");
    }

    ///<summary> clear all staffs </summary>
    void ClearExample()
    {
        if (notesParent.childCount > 0)
            foreach (Transform note in notesParent)
                Destroy(note.gameObject);
        if (ledgerLinesParent.childCount > 0)
            foreach (Transform line in ledgerLinesParent)
                Destroy(line.gameObject);
    }

    void RandomExample(string clef)
    {
        rhythmPosition = -360f;

        int generatedPitch = Random.Range(1, 15);
        NotateNote(generatedPitch, NextRhythmPosition(), clef);
        int generatedInterval = RandomInterval();

        for (int i = 1; i < 20; i++)
        {
            if (Random.Range(0, 100) + 1 > sameIntervalChance) //this next interval will NOT be the same
            {
                Mathf.Abs(generatedInterval); //unfilp interval
                generatedInterval = RandomInterval(generatedInterval);

                if (Random.Range(0, 2) == 0) //50% chance of flipping interval
                    generatedInterval *= -1;
            }

            //if the generatedPitch is out of range then flip interval to stay in the staff's range
            if (!IsPitchValid(generatedPitch + generatedInterval)) 
                generatedInterval *= -1;
            generatedPitch = generatedPitch + generatedInterval;

            if (IsPitchValid(generatedPitch))
                NotateNote(generatedPitch, NextRhythmPosition(), clef);
            else
                Debug.LogError("generatedPitch out of range");
        }
    }

    int RandomInterval()
    {
        int total = 0;
        foreach (int interval in intervalChances) //get total of percentages
        {
            total += interval;
        }

        if (total > 0)
        {
            int rand = Random.Range(0, total);
            //subtract random number by interval percentages until less than 0 to determine the random interval
            for (int i = 0; i < intervalChances.Length; i++)
            {
                rand -= intervalChances[i];
                if (rand < 0)
                    return i;
            }
        }
        else //all chances are set to zero, so choose any random interval
        {
            return Random.Range(0, intervalChances.Length);
        }
        Debug.LogError("Random Interval could not be generated");
        return -1;
    }
    int RandomInterval(int exception)
    {
        int total = 0;
        for (int i = 0; i < intervalChances.Length; i++) //get total of percentages, skip the exception
        {
            if (i != exception)
                total += intervalChances[i];
        }

        //the exception interval is the only interval that can be generated, so return the exception anyways
        if (total == 0) 
        {
            return exception;
        }

        int rand = Random.Range(0, total);
        //subtract random number by interval percentages until less than 0 to determine the random interval, skip the exception
        for (int i = 0; i < intervalChances.Length; i++)
        {
            if (i != exception)
            {
                rand -= intervalChances[i];
                if (rand < 0)
                    return i;
            }
        }
        Debug.LogError("Random Interval could not be generated");
        return -1;
    }

    int RandomRangeException(int minInclusive, int maxExclusive, int exception)
    {
        if (Random.Range(0, 2) == 0 || exception == maxExclusive - 1)
        {
            return Random.Range(minInclusive, exception);
        }

        return Random.Range(exception + 1, maxExclusive);
    }

    ///<summary> pitch=[1,15] </summary>
    void NotateNote(int pitch, float rhythmPosition, string clef)
    {
        GameObject note = Instantiate(SetNoteFlip(pitch), notesParent);
        //set correct place for note based on pitch (y) and where it is in the rhythm (x)
        if (clef.Equals("treble"))
        {
            note.transform.localPosition = new Vector3(rhythmPosition, pitchTrebleCleffPositions[pitch]);
        }
        else if (clef.Equals("bass"))
        {
            note.transform.localPosition = new Vector3(rhythmPosition, pitchBassCleffPositions[pitch]);
        }
        else
            Debug.LogError("Invalid Clef");

        Vector3 notePos = note.transform.localPosition;
        //set ledger lines if the note is above or below staff
        if (NotePositionOnStaff(pitch, clef).Equals("above"))
        {
            if (clef.Equals("bass"))
            {
                pitch -= 2;
            }
            GameObject line = Instantiate(ledgerLinePrefab, ledgerLinesParent);
            switch (pitch)
            {
                case 13:
                    line.transform.localPosition = new Vector3(notePos.x, notePos.y);
                    break;
                case 14:
                    line.transform.localPosition = new Vector3(notePos.x, notePos.y - staffLineDistance / 2);
                    break;
                case 15:
                    line.transform.localPosition = new Vector3(notePos.x, notePos.y);
                    GameObject line2 = Instantiate(ledgerLinePrefab, ledgerLinesParent);
                    line2.transform.localPosition = new Vector3(notePos.x, notePos.y - staffLineDistance);
                    break;
                default:
                    break;
            }
        }
        else if (NotePositionOnStaff(pitch, clef).Equals("below"))
        {
            if (clef.Equals("treble"))
            {
                pitch += 2;
            }
            GameObject line = Instantiate(ledgerLinePrefab, ledgerLinesParent);
            switch (pitch)
            {
                case 3:
                    line.transform.localPosition = new Vector3(notePos.x, notePos.y);
                    break;
                case 2:
                    line.transform.localPosition = new Vector3(notePos.x, notePos.y + staffLineDistance / 2);
                    break;
                case 1:
                    line.transform.localPosition = new Vector3(notePos.x, notePos.y);
                    GameObject line2 = Instantiate(ledgerLinePrefab, ledgerLinesParent);
                    line2.transform.localPosition = new Vector3(notePos.x, notePos.y + staffLineDistance);
                    break;
                default:
                    break;
            }
        }
    }

    ///<summary> return rhythmPosition, then increment rhythmPosition to next position </summary>
    float NextRhythmPosition()
    {
        float temp = rhythmPosition;
        rhythmPosition += 40;
        return temp;
    }

    bool IsPitchValid(int pitch)
    {
        if (pitch >= 1 && pitch <= 15)
            return true;

        return false;
    }

    GameObject SetNoteFlip(int pitch)
    {
        if (pitch < 8)
        {
            return notePrefab;
        }
        return noteFlippedPrefab;
    }
}
