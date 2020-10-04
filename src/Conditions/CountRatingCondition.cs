using System;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A count condition that by default evaluates to true when the current count has reached the specified goal. Use it to keep count of user events in your application or as a countdown.
    /// </summary>
    public class CountRatingCondition : RatingConditionBase<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">Specify the initial count for this condition.</param>
        /// <param name="goal">Specify the count goal for this condition. When the current state is equal of exceeds that goal the condition is met.</param>
        public CountRatingCondition(int initialState, int goal)
            : base(initialState, currentCount => currentCount >= goal)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">Specify the initial count for this condition.</param>
        /// <param name="goal">Specify the count goal for this condition. When the current state is equal of exceeds that goal the condition is met.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public CountRatingCondition(int initialState, int goal, ConditionType conditionType)
            : base(initialState, currentCount => currentCount >= goal, conditionType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">Specify the initial count for this condition.</param>
        /// <param name="evaluator">An evaluator function that determines when the condition is met.</param>
        public CountRatingCondition(int initialState, Func<int, bool> evaluator)
            : base(initialState, evaluator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">Specify the initial count for this condition.</param>
        /// <param name="evaluator">An evaluator function that determines when the condition is met.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public CountRatingCondition(int initialState, Func<int, bool> evaluator, ConditionType conditionType)
            : base(initialState, evaluator, conditionType)
        {

        }

        /// <summary>
        /// Increase the current count of this condition by one.
        /// </summary>
        public override void ManipulateState() => CurrentState++;
    }
}
