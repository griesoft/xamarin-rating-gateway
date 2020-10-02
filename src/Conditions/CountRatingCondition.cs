using System;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// 
    /// </summary>
    public class CountRatingCondition : RatingConditionBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="goal"></param>
        public CountRatingCondition(int initialState, int goal)
            : base(initialState, currentCount => currentCount >= goal)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="goal"></param>
        /// <param name="conditionType"></param>
        public CountRatingCondition(int initialState, int goal, ConditionType conditionType)
            : base(initialState, currentCount => currentCount >= goal, conditionType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        public CountRatingCondition(int initialState, Func<int, bool> evaluator)
            : base(initialState, evaluator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        /// <param name="conditionType"></param>
        public CountRatingCondition(int initialState, Func<int, bool> evaluator, ConditionType conditionType)
            : base(initialState, evaluator, conditionType)
        {

        }

        public override void ManipulateState() => CurrentState++;
    }
}
