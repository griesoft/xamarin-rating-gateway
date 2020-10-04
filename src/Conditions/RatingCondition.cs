using System;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A generic condition that enables you to create custom conditions with minimal effort.
    /// </summary>
    /// <remarks>
    /// If you need custom manipulation or reset logic, or if you want to add more complex functionality that the build-in conditions don't provide, 
    /// you should write a custom condition and inherit from the condition base class <see cref="RatingConditionBase{TConditionType}"/>.
    /// </remarks>
    /// <typeparam name="TConditionType"></typeparam>
    public class RatingCondition<TConditionType> : RatingConditionBase<TConditionType> where TConditionType : notnull
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">Specify the initial value for this condition.</param>
        /// <param name="evaluator">An evaluator function that determines when the condition is met.</param>
        public RatingCondition(TConditionType initialState, Func<TConditionType, bool> evaluator)
            : base(initialState, evaluator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">Specify the initial value for this condition.</param>
        /// <param name="evaluator">An evaluator function that determines when the condition is met.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public RatingCondition(TConditionType initialState, Func<TConditionType, bool> evaluator, ConditionType conditionType)
            : base(initialState, evaluator, conditionType)
        {

        }
    }
}
