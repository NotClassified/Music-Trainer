using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public static class Theory
    {
        ///<summary> pitches for one octave </summary>
        public static Dictionary<Note, Pitch> Pitches = new();
        static Theory()
        {
            AddLetterToKeyboardOctave(NoteLetters.C);
            AddLetterToKeyboardOctave(NoteLetters.D);
            AddLetterToKeyboardOctave(NoteLetters.E);
            AddLetterToKeyboardOctave(NoteLetters.F);
            AddLetterToKeyboardOctave(NoteLetters.G);
            AddLetterToKeyboardOctave(NoteLetters.A);
            AddLetterToKeyboardOctave(NoteLetters.B);
        }

        static void AddLetterToKeyboardOctave(NoteLetters letter)
        {
            //add letter with each accidental to "KeyboardOctave"
            for (int i = (int)Accidentals.DoubleFlat; i < (int)Accidentals.DoubleSharp; i++)
            {
                Note newNote = new(letter, (Accidentals)i);
                Pitches.Add(newNote, new Pitch(newNote));
            }
        }

    }
    public enum NoteLetters
    {
        C = 0,
        //black key between C & D
        D = 2,
        //black key between D & E
        E = 4,
        F = 5,
        //black key between F & G
        G = 7,
        //black key between G & A
        A = 9,
        //black key between A & B
        B = 11
    }
    public enum Accidentals { DoubleFlat = -2, Flat, Natural, Sharp, DoubleSharp }
    public class Note
    {
        public NoteLetters letter;
        public Accidentals accidental;

        public Note(NoteLetters letter, Accidentals accidental = Accidentals.Natural)
        {
            this.letter = letter;
            this.accidental = accidental;
        }
        public override bool Equals(object obj)
        {
            if (obj is null || obj is not Note)
            {
                return false;
            }

            Note other = (Note)obj;
            return this.letter == other.letter && this.accidental == other.accidental;
        }
        public override int GetHashCode()
        {
            return this.letter.GetHashCode() + this.accidental.GetHashCode();
        }
    }
    public class Pitch
    {
        ///<summary> C4 = 0 </summary>
        [Range(-2, 13)] public int pitch;

        public Pitch(Note note)
        {
            pitch = (int)note.letter + (int)note.accidental;
        }
        public override string ToString()
        {
            return pitch.ToString();
        }
    }
}
