using System;
using System.Collections.Generic;
using System.Linq;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public static class Utilities
    {
        public static float GetAverage(IEnumerable<IGradable> gradables, bool round)
        {
            var enumerable = gradables as IGradable[] ?? gradables.ToArray();
            float result = enumerable.Sum(x => x.GradeValue * x.Weighting) / enumerable.Sum(x => x.Weighting);
            return round ? (float)Math.Round(result, 2) : result;
        }
    }
}