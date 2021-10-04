using System;
using System.Collections.Generic;
using System.Linq;
using GradeManagement.Interfaces;

namespace GradeManagement.Models
{
    public static class Utilities
    {
        public static float GetAverage(IEnumerable<IGradable> gradables)
        {
            var enumerable = gradables as IGradable[] ?? gradables.ToArray();
            var result = enumerable.Sum(x => x.GradeValue * x.Weighting) / enumerable.Sum(x => x.Weighting);
            return (float)Math.Round(result, 2);
        }
    }
}