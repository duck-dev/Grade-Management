using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using Avalonia.Styling;
using GradeManagement.Interfaces;

namespace GradeManagement.UtilityCollection
{
    public static partial class Utilities
    {
        /// <summary>
        /// The parent path of all settings- and data-files
        /// </summary>
        public static string FilesParentPath
        {
            get
            {
                var directory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                return directory ?? throw new Exception("Directory name of the currently executing assembly is null.");
            }
        }
        
        /// <summary>
        /// Calculate the average of several grades with a weighting factor for each grade.
        /// </summary>
        /// <param name="gradables">A collection of <see cref="IGradable">IGradables</see>,
        ///                         whose grades will be used for the average.</param>
        /// <param name="round">Determines whether the returned average should be rounded to 2 decimal digits or not.</param>
        /// <returns>The calculated average, either rounded or exact, based on the passed bool <see cref="round"/>></returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static float GetAverage(IEnumerable<IGradable> gradables, bool round)
        {
            var enumerable = gradables.Where(x => x.Counts && x.ElementCount > 0 && !float.IsNaN(x.GradeValue) && x.GradeValue != 0);
            if (!enumerable.Any())
                return 0;
            float result = enumerable.Sum(x => x.GradeValue * x.Weighting) / enumerable.Sum(x => x.Weighting);
            return round ? (float)Math.Round(result, 2) : result;
        }

        /// <summary>
        /// Log a message to the console (for debugging purposes).
        /// </summary>
        /// <param name="message">The message to be logged as a string.</param>
        public static void Log(string? message) => System.Diagnostics.Trace.WriteLine(message);

        /// <summary>
        /// Retrieves a resource from a specified <see cref="IResourceNode"/> and tries to cast it to the specified type.
        /// </summary>
        /// <param name="element">The element to retrieve the resource from.</param>
        /// <param name="resourceName">The name of the resource you want to retrieve (key).</param>
        /// <typeparam name="T">The actual type of the resource you want to retrieve.</typeparam>
        /// <returns>The resource as it's actual type.</returns>
        public static T? GetResource<T>(IResourceNode element, string resourceName)
            where T : class
        {
            element.TryGetResource(resourceName, out object? resource);
            return resource as T;
        }

        /// <summary>
        /// Retrieves a resource from a specified <see cref="Style"/>, which is in turns contained
        /// in the <see cref="Styles"/> of an <see cref="IStyleHost"/>, and tries to cast it to the specified type.
        /// </summary>
        /// <param name="element">The element to retrieve the resource from.</param>
        /// <param name="resourceName">The name of the resource you want to retrieve (key).</param>
        /// <param name="styleIndex">The index of the <see cref="Style"/> inside the <see cref="Styles"/> collection
        /// of the <paramref name="element"/>.</param>
        /// <typeparam name="TResource">The actual type of the resource you want to retrieve.</typeparam>
        /// <typeparam name="TElement">The type of the element to retrieve the resource from.
        /// This type must implement <see cref="IResourceNode"/>.</typeparam>
        /// <returns>The resource as it's actual type.</returns>
        public static TResource? GetResourceFromStyle<TResource, TElement>(TElement? element, string resourceName, int styleIndex)
            where TResource : class
            where TElement : IStyleHost, IResourceNode
        {
            var styleInclude = element?.Styles[styleIndex] as StyleInclude;
            return (styleInclude?.Loaded is Style style ? GetResource<TResource>(style, resourceName) : null);
        }
    }
}