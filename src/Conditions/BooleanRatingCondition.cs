using System;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A simple boolean condition that by default evaluates to true when the current state is set to true. 
    /// Use it as a switch or to memorize certain events within the lifecycle of your application.
    /// </summary>
    public class BooleanRatingCondition : RatingConditionBase<bool>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// The initial state for this constructor is false and the evaluator will evaluate to true if the current state is true.
        /// </remarks>
        public BooleanRatingCondition()
            : base(false, current => current)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionType">Specify the condition type.</param>
        /// <remarks>
        /// The initial state for this constructor is false and the evaluator will evaluate to true if the current state is true.
        /// </remarks>
        public BooleanRatingCondition(ConditionType conditionType)
            : base(false, current => current, conditionType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">The initial state of this condition.</param>
        /// <param name="evaluator">An evaluator function that determines whether the condition is met or not.</param>
        public BooleanRatingCondition(bool initialState, Func<bool, bool> evaluator)
            : base(initialState, evaluator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">The initial state of this condition.</param>
        /// <param name="evaluator">An evaluator function that determines whether the condition is met or not.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public BooleanRatingCondition(bool initialState, Func<bool, bool> evaluator, ConditionType conditionType)
            : base(initialState, evaluator, conditionType)
        {

        }

        /// <inheritdoc/>
        /// <remarks>By default false.</remarks>
        public override bool CacheCurrentValue { get; set; } = false;

        /// <summary>
        /// Inverts the current state of this condition.
        /// </summary>
        public override void ManipulateState() => CurrentState = !CurrentState;
    }
}
