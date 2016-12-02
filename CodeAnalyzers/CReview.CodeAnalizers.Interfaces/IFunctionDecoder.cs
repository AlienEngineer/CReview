using CReview.CodeAnalizers.Models;

namespace CReview.CodeAnalizers
{
    /// <summary>
    /// Decodes a function
    /// </summary>
    public interface IFunctionDecoder
    {
        /// <summary>
        /// Decodes the given function.
        /// </summary>
        /// <param name="function">The function code</param>
        /// <returns>An instance of Function</returns>
        Function DecodeFunction(string function);
    }
}