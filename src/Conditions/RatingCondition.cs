using System;

namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConditionType"></typeparam>
    public class RatingCondition<TConditionType> : RatingConditionBase<TConditionType> where TConditionType : notnull
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        public RatingCondition(TConditionType initialState, Func<TConditionType, bool> evaluator)
            : base(initialState, evaluator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        /// <param name="conditionType"></param>
        public RatingCondition(TConditionType initialState, Func<TConditionType, bool> evaluator, ConditionType conditionType)
            : base(initialState, evaluator, conditionType)
        {

        }
    }
}
