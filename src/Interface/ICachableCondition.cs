namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICachableCondition
    {
        /// <summary>
        /// 
        /// </summary>
        bool CacheCurrentValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <returns></returns>
        ConditionCacheDto ToConditionCacheDto(string conditionName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        void ManipulateState(object parameter);
    }
}
