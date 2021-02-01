using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace BibMaMo.Core.Extensions
{
  public static class Extensions
  {
    private static bool StringContains(this string bigString, string littleString)
    {
      if (string.IsNullOrEmpty(littleString))
      {
        return true;
      }
      if (string.IsNullOrEmpty(bigString))
      {
        return false;
      }
      bigString = RemoveDiacritics(bigString.ToLower());
      littleString = RemoveDiacritics(littleString.ToLower());
      return bigString.Contains(littleString);
    }
    private static string RemoveDiacritics(this String s)
    {
      String normalizedString = s.Normalize(NormalizationForm.FormD);
      StringBuilder stringBuilder = new StringBuilder();

      for (int i = 0; i < normalizedString.Length; i++)
      {
        Char c = normalizedString[i];
        if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
          stringBuilder.Append(c);
      }

      return stringBuilder.ToString();
    }

  }
}
