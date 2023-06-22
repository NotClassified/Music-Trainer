using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Music;

public class RelativePitchManager : MonoBehaviour
{

    void Start()
    {
        print(Theory.Pitches[new(NoteLetters.C, Accidentals.Sharp)].GetHashCode());
        print(Theory.Pitches[new(NoteLetters.D, Accidentals.Sharp)].GetHashCode());
        print(Theory.Pitches[new(NoteLetters.E, Accidentals.Sharp)].GetHashCode());
        print(Theory.Pitches[new(NoteLetters.F, Accidentals.Natural)].GetHashCode());
    }
}
