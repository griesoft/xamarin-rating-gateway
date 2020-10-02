using System;
using Griesoft.Xamarin.RatingGateway.Cache;

namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TConditionType"></typeparam>
    public abstract class RatingConditionBase<TConditionType> : IRatingCondition, ICachableCondition where TConditionType : notnull
    {
        private readonly Func<TConditionType, bool> _evaluator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        protected RatingConditionBase(TConditionType initialState, Func<TConditionType, bool> evaluator) 
            : this(initialState, evaluator, ConditionType.Standard)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="evaluator"></param>
        /// <param name="conditionType"></param>
        protected RatingConditionBase(TConditionType initialState, Func<TConditionType, bool> evaluator, ConditionType conditionType)
        {
            InitialState = initialState;
            CurrentState = initialState;
            ConditionType = conditionType;
            _evaluator = evaluator;
        }

        /// <summary>
        /// 
        /// </summary>
        protected TConditionType InitialState { get; }

        /// <summary>
        /// 
        /// </summary>
        public TConditionType CurrentState { get; protected set; }

        /// <inheritdoc/>
        public bool ResetAfterConditionMet { get; set; } = true;

        /// <inheritdoc/>
        public bool ExplicitManipulationOnly { get; set; } = false;

        /// <inheritdoc/>
        public bool DisallowParamaterlessManipulation { get; set; } = false;

        /// <inheritdoc/>
        public ConditionType ConditionType { get; }

        /// <inheritdoc/>
        public bool IsConditionMet => _evaluator(CurrentState);

        /// <inheritdoc/>
        public virtual bool CacheCurrentValue { get; set; } = true;

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        protected virtual void ManipulateState(TConditionType parameter) => CurrentState = parameter;
    }
}
