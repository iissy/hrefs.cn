using System;

namespace ASY.Hrefs.Util.UIHelpers
{
    /// <summary>
    /// Holds the Levenshtein distance calculation method
    /// </summary>
    public static class Levenshtein
    {

        /// <summary>
        /// The maximum length of products of strings which the CLR will allow us to use a rectangular array for the algo
        /// </summary>
        public const int MaxStringProductLength = 536848900;

        /// <summary>
        /// Calculates the Levenshtein minimum edit distance from one string to another
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="substitutionCost">int the cost of substituting one letter for another.  Typically 1 or 2</param>
        /// <returns></returns>
        public static int CalculateDistance(string s, string t, int substitutionCost)
        {
            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(t))
                return CalculateDistance(s, t, substitutionCost, new RectangularArray(0, 0)); //will short circuit the algo
            else
            {
                if (s.Length * t.Length > MaxStringProductLength)
                    return CalculateDistance(s, t, substitutionCost, new JaggedArray(s.Length + 1, t.Length + 1)); //large strings, use large data structure
                else
                    return CalculateDistance(s, t, substitutionCost, new RectangularArray(s.Length + 1, t.Length + 1)); //small strings, can use rectangular array
            }
        }

        /// <summary>
        /// Calculates the Levenshtein minimum edit distance from one string to another -> optionally uses a MemoryStructure for the algo of your choice
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        /// <param name="substitutionCost">int the cost of substituting one letter for another.  Typically 1 or 2</param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static int CalculateDistance(string s, string t, int substitutionCost, MemoryStructure d)
        {

            //guard against nulls and empties
            if (string.IsNullOrEmpty(s))
            {
                if (string.IsNullOrEmpty(t))
                    return 0;
                return t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                if (string.IsNullOrEmpty(s))
                    return 0;
                return s.Length;
            }

            var m = s.Length + 1;
            var n = t.Length + 1;

            // map empties to each other
            for (int i = 0; i < m; i++)
            {
                d[i, 0] = i;
            }
            for (int i = 0; i < n; i++)
            {
                d[0, i] = i;
            }


            for (int i = 1; i < m; i++)
            {
                for (int j = 1; j < n; j++)
                {
                    if (s[i - 1] == t[j - 1])
                    {
                        d[i, j] = d[i - 1, j - 1]; //no cost, letters the same
                    }
                    else
                    {
                        var delete = d[i - 1, j] + 1;
                        var insert = d[i, j - 1] + 1;
                        var substitution = d[i - 1, j - 1] + substitutionCost;
                        d[i, j] = Math.Min(delete, Math.Min(insert, substitution));
                    }
                }
            }

            return d[m - 1, n - 1];
        }

    }
}