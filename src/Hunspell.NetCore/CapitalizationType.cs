﻿using System.Globalization;

namespace Hunspell
{
    public enum CapitalizationType : byte
    {
        None = 0,

        Init = 1,

        All = 2,

        Huh = 3,

        HuhInit = 4
    }

    public static class CapitalizationTypeUtilities
    {
        public static CapitalizationType GetCapitalizationType(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return CapitalizationType.None;
            }

            var numberCapitalized = 0;
            var numberNeutral = 0;
            for (int i = 0; i < word.Length; i++)
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(word, i);
                if (category == UnicodeCategory.UppercaseLetter || category == UnicodeCategory.TitlecaseLetter)
                {
                    numberCapitalized++;
                }
                else if (
                    category != UnicodeCategory.LowercaseLetter
                    && category != UnicodeCategory.OtherLetter
                    && category != UnicodeCategory.ModifierLetter
                )
                {
                    numberNeutral++;
                }
            }

            bool firstIsCapitalized;
            if (numberCapitalized == 0)
            {
                return CapitalizationType.None;
            }
            else
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(word, 0);
                firstIsCapitalized = category == UnicodeCategory.UppercaseLetter || category == UnicodeCategory.TitlecaseLetter;
            }

            if (numberCapitalized == 1 && firstIsCapitalized)
            {
                return CapitalizationType.Init;
            }
            if (numberCapitalized == word.Length || (numberCapitalized + numberNeutral) == word.Length)
            {
                return CapitalizationType.All;
            }
            if (numberCapitalized > 1 && firstIsCapitalized)
            {
                return CapitalizationType.HuhInit;
            }

            return CapitalizationType.Huh;
        }
    }
}
