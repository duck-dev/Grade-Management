using System;
using System.Collections.Generic;
using System.Linq;

namespace GradeManagement.ExtensionCollection
{
    public static partial class Extensions
    {
        /// <summary>
        /// Add the element to the collection only if it doesn't contain it already.
        /// </summary>
        /// <param name="list">The collection to which the element should be added.</param>
        /// <param name="element">The element to be added.</param>
        public static void SafeAdd<T>(this ICollection<T> list, T element)
        {
            if(!list.Contains(element))
                list.Add(element);
        }

        /// <summary>
        /// Remove the element from the collection only if it already contains it.
        /// </summary>
        /// <param name="list">The collection, the element should be removed from.</param>
        /// <param name="element">The element to be removed.</param>
        public static void SafeRemove<T>(this ICollection<T> list, T element)
        {
            if (list.Contains(element))
                list.Remove(element);
        }

        /// <summary>
        /// Create a deep copy of a <see cref="IEnumerable{T}"/> collection.
        /// </summary>
        /// <param name="list">The list to be cloned.</param>
        /// <typeparam name="T">The type of the elements inside the collection. All of them ought to inherit <see cref="ICloneable"/>
        /// </typeparam>
        /// <returns>The cloned collection.</returns>
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> list) where T : ICloneable 
            => list.Select(x => (T)x.Clone());
    }
}