using System;

namespace Common.Extensions
{
    /// <summary>
    ///     Some useful extensions.
    /// </summary>
    public static class GenericExtensions
    {
        /// <summary>
        ///     Applies given block to this and return this instance.
        /// </summary>
        public static T Apply<T>(this T obj, Action<T> block)
        {
            block(obj);
            return obj;
        }
    }
}