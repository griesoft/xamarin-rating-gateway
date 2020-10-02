namespace Griesoft.Xamarin.RatingGateway.Cache
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRatingConditionCache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <param name="condition"></param>
        void Load(string conditionName, ICachableCondition condition);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <param name="condition"></param>
        void Save(string conditionName, ICachableCondition condition);
    }
}
