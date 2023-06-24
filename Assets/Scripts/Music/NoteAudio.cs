using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PitchAudioFiles", menuName = "PitchAudioFiles")]
public class NoteAudio : ScriptableObject
{
    [SerializeField] AudioClip[] allNotes;
    [SerializeField] int zeroIndex;

    public AudioClip GetPitch(int pitch)
    {
        int pitchIndex = zeroIndex + pitch;

        if (pitchIndex < 0 || pitchIndex > allNotes.Length)
        {
            Debug.LogError("No audio file for this pitch: " + pitch);
            return null;
        }

        return allNotes[pitchIndex];
    }
}
