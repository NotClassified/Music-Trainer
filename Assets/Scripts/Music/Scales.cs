using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Music
{
    public static class Scales
    {
        static Interval[] diatonicScalePattern =
        {
            //W W H W W W H
            Interval.WholeStep, Interval.WholeStep, Interval.HalfStep, 
            Interval.WholeStep, Interval.WholeStep, Interval.WholeStep, Interval.HalfStep
        };

        public const int diatonicModeAmount = 7;
        public static NoteLetter[] AllLetters =
        {
            NoteLetter.C, NoteLetter.D, NoteLetter.E, NoteLetter.F, NoteLetter.G, NoteLetter.A, NoteLetter.B
        };

        public static Interval[] GetScalePattern(DiatonicMode mode = DiatonicMode.Ionian)
        {
            Interval[] scalePattern = new Interval[diatonicScalePattern.Length];

            int diatonicIndex = (int)mode;
            for (int i = 0; i < scalePattern.Length; i++)
            {
                if (diatonicIndex >= diatonicScalePattern.Length)
                    diatonicIndex = 0;

                scalePattern[i] = diatonicScalePattern[diatonicIndex++];
            }
            return scalePattern;
        }
        public static Note[] GetScale(Note tonic, DiatonicMode mode = DiatonicMode.Ionian)
        {
            Interval[] scalePattern = GetScalePattern(mode);
            Note[] scale = new Note[8];
            scale[0] = tonic;
            for (int i = 1; i < scale.Length; i++)
            {
                NoteLetter nextLetter = Note.GetNextLetter(scale[i - 1].letter);
                int nextPitch = scale[i - 1].GetPitch() + (int)scalePattern[i - 1]; //previous note + interval
                Note nextNote = new(nextLetter, nextPitch);
                //Debug.Log(nextNote.ToString());
                scale[i] = nextNote;
            }
            return scale;
        }

        public enum Degree
        {
            Tonic, Supertonic, Mediant, SubDominant, Dominant, SubMediant, LeadingTone
        }
    }
    public enum DiatonicMode
    {
        Ionian = 0, Major = 0,
        Dorian,
        Phrygian,
        Lydian,
        Mixolydian,
        Aeolian = 5, Minor = 5,
        Locrian
    }
    public enum Interval
    {
        Prime,
        HalfStep = 1, Minor2nd = 1,
        WholeStep = 2, Major2nd = 2,
        Minor3rd = 3, 
    }
}
