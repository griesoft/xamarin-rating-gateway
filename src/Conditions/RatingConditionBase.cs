using System;
using Griesoft.Xamarin.RatingGateway.Cache;

namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// The base class for a condition with an default implementation of the <see cref="IRatingCondition"/> interface. Use it to easily create custom conditions.
    /// </summary>
    /// <typeparam name="TConditionType">The type that this condition is dedicated to store, manipulate and evaluate.</typeparam>
    /// <remarks>
    /// If you need custom manipulation or reset logic, or if you want to add more complex functionality that the build-in conditions don't provide, 
    /// you should write a custom condition and inherit from this abstract base class.
    /// </remarks>
    public abstract class RatingConditionBase<TConditionType> : IRatingCondition, ICachableCondition where TConditionType : notnull
    {
        private readonly Func<TConditionType, bool> _evaluator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">The initial state of this condition.</param>
        /// <param name="evaluator">An evaluator function that determines whether a condition is met or not.</param>
        protected RatingConditionBase(TConditionType initialState, Func<TConditionType, bool> evaluator) 
            : this(initialState, evaluator, ConditionType.Standard)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState">The initial state of this condition.</param>
        /// <param name="evaluator">An evaluator function that determines whether a condition is met or not.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        protected RatingConditionBase(TConditionType initialState, Func<TConditionType, bool> evaluator, ConditionType conditionType)
        {
            InitialState = initialState;
            CurrentState = initialState;
            ConditionType = conditionType;
            _evaluator = evaluator;
        }

        /// <summary>
        /// The initial state of the condition.
        /// </summary>
        protected TConditionType InitialState { get; }

        /// <summary>
        /// The current state of the condition.
        /// </summary>
        public TConditionType CurrentState { get; protected set; }

        /// <inheritdoc/>
        /// <remarks>By default true.</remarks>
        public bool ResetAfterConditionMet { get; set; } = true;

        /// <inheritdoc/>
        /// <remarks>By default true.</remarks>
        public bool ResetOnlyOnEvaluationSuccess { get; set; } = true;

        /// <inheritdoc/>
        /// <remarks>By default false.</remarks>
        public bool ExplicitManipulationOnly { get; set; } = false;

        /// <inheritdoc/>
        /// <remarks>By default false.</remarks>
        public bool DisallowParamaterlessManipulation { get; set; } = false;

        /// <inheritdoc/>
        public ConditionType ConditionType { get; }

        /// <inheritdoc/>
        public bool IsConditionMet => _evaluator(CurrentState);

        /// <inheritdoc/>
        /// <remarks>By default true.</remarks>
        public virtual bool CacheCurrentValue { get; set; } = true;

        /// <summary>
        /// Sets the current state to the specified value if it can be cast to <typeparamref name="TConditionType"/>.
        /// </summary>
        /// <param name="parameter">The value to set as the new current state.</param>
        public void ManipulateState(object parameter)
        {
            if (parameter is TConditionType cast)
            {
                ManipulateState(cast);
            }
        }

        /// <inheritdoc/>
        public virtual void ManipulateState()
        {
        }

        /// <summary>
        /// Reset the current state to the initial state.
        /// </summary>
        public virtual void Reset() => CurrentState = InitialState;

        /// <inheritdoc/>
        public virtual ConditionCacheDto ToConditionCacheDto(string conditionName)
        {
            return new ConditionCacheDto()
            {
                ConditionName = conditionName,
                CurrentValue = CurrentState
            };
        }

        /// <summary>
        /// Manipulate by setting the current state to the given <paramref name="parameter"/>.
        /// </summary>
        /// <param name="parameter">The parameter that will be the new current state.</param>
        protected virtual void ManipulateState(TConditionType parameter) => CurrentState = parameter;
    }
}
