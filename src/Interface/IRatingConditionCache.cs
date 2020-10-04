namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// The condition cache that manages the IO related tasks for the rating gateway.
    /// </summary>
    public interface IRatingConditionCache
    {
        /// <summary>
        /// Load a cached condition state.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="condition">The cachable condition instance which will be populated with the cached state, if one exists.</param>
        /// <returns>True if loading a value from cache was successful.</returns>
        bool Load(string conditionName, ICachableCondition condition);

        /// <summary>
        /// Save a condition state to cache.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="condition">The cachable condition instance from which the state will be extracted and saved to cache.</param>
        void Save(string conditionName, ICachableCondition condition);

        /// <summary>
        /// Delete the cached state of a condition if it exists.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        void Delete(string conditionName);
    }
}
