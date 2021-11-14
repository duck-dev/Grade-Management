using System.Collections.Generic;

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
    }
}