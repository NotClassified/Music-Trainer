using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music 
{ 
    public class Note
    {
        public NoteLetter letter;
        public Accidental accidental;
        public int octave;


        public Note(NoteLetter letter, Accidental accidental = Accidental.Natural, int octave = 4)
        {
            this.letter = letter;
            this.accidental = accidental;
            this.octave = octave;
        }
        public Note(NoteLetter letter, int targetPitch)
        {
            this.letter = letter;

            int letterPitch = (int)letter;
            if (letterPitch == targetPitch)
            {
                accidental = Accidental.Natural;
                octave = 4;
                return;
            }

            int newOctave = 4;
            if (targetPitch > letterPitch)
            {
                while (targetPitch - letterPitch > 2) //target pitch is too high, go up an octave
                {
                    targetPitch -= 12;
                    newOctave++;
                }
            }
            else
            {
                while (targetPitch - letterPitch > 2) //target pitch is too low, go down an octave
                {
                    targetPitch += 12;
                    newOctave--;
                }
            }
            octave = newOctave;

            if (Mathf.Abs(targetPitch - letterPitch) <= 2) //an accidental can only change a pitch by 2 at most
            {
                //get the difference to find correct accidental
                accidental = (Accidental)(targetPitch - letterPitch); 
            }
            else
            {
                //there's no accidental that can make this letter be the target pitch
            }
        }
        //public Note(Note copy) : this(copy.letter, copy.accidental, copy.octave) { }

        public int GetPitch() => (int)letter + (int)accidental + GetOctavePitchDifference(octave);
        static int GetOctavePitchDifference(int octave) => (octave - 4) * 12;

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
        public override string ToString()
        {
            string accidentalSymbol = "";
            switch (accidental)
            {
                case Accidental.DoubleFlat:
                    accidentalSymbol = "bb";
                    break;
                case Accidental.Flat:
                    accidentalSymbol = "b";
                    break;
                case Accidental.Sharp:
                    accidentalSymbol = "#";
                    break;
                case Accidental.DoubleSharp:
                    accidentalSymbol = "x";
                    break;
            }
            return letter + accidentalSymbol + octave; //ex: C#4, Eb2, B5...
        }


        public static NoteLetter GetNextLetter(NoteLetter letter)
        {
            //no black key between E & F
            if (letter is NoteLetter.E)
                return NoteLetter.F;
            //no black key between B & C
            else if (letter is NoteLetter.B)
                return NoteLetter.C;

            //go up a whole step to get to next letter
            int nextLetterIndex = (int)letter + 2;
            return (NoteLetter)nextLetterIndex;
        }
    }
    public enum NoteLetter
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
    public enum Accidental 
    { 
        DoubleFlat = -2, 
        Flat = -1, 
        Natural = 0, 
        Sharp = 1, 
        DoubleSharp = 2
    }
}
