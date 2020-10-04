namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// An interface which adds caching functionality to a condition when implemented.
    /// </summary>
    public interface ICachableCondition
    {
        /// <summary>
        /// You can disable caching for this condition instance by setting this property to false.
        /// </summary>
        bool CacheCurrentValue { get; set; }

        /// <summary>
        /// Creates a cachable data transfer object out of this condition instance.
        /// </summary>
        /// <param name="conditionName">The unique name for this condition.</param>
        /// <returns></returns>
        ConditionCacheDto ToConditionCacheDto(string conditionName);

        /// <summary>
        /// Manipulate the current value by a specified parameter.
        /// </summary>
        /// <param name="parameter">The value to use for state manipulation.</param>
        void ManipulateState(object parameter);
    }
}
