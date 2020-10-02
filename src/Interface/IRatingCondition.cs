namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRatingCondition
    {
        /// <summary>
        /// 
        /// </summary>
        bool ResetAfterConditionMet { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool ExplicitManipulationOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        bool DisallowParamaterlessManipulation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ConditionType ConditionType { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsConditionMet { get; }

        /// <summary>
        /// 
        /// </summary>
        void Reset();

        /// <summary>
        /// 
        /// </summary>
        void ManipulateState();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        void ManipulateState(object parameter);
    }
}
