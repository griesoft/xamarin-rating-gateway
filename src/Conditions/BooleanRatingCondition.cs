using System;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// 
    /// </summary>
    public class BooleanRatingCondition : RatingConditionBase<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        public BooleanRatingCondition()
            : base(false, current => current)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionType"></param>
        public BooleanRatingCondition(ConditionType conditionType)
            : base(false, current => current, conditionType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        public BooleanRatingCondition(bool initialState, Func<bool, bool> evaluator)
            : base(initialState, evaluator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        /// <param name="conditionType"></param>
        public BooleanRatingCondition(bool initialState, Func<bool, bool> evaluator, ConditionType conditionType)
            : base(initialState, evaluator, conditionType)
        {

        }

        public override bool CacheCurrentValue { get; set; } = false;

        /// <inheritdoc/>
        public override void ManipulateState() => CurrentState = !CurrentState;
    }
}
