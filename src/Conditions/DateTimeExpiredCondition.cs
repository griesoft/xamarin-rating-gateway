using System;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// A condition that will evaluate to true after a specific moment in time is in the past. 
    /// Use it to wait for a certain amount of time before asking the user to review your application, for example.
    /// </summary>
    /// <remarks>
    /// By caching the current state of this condition, the loading process of cached values will set the current state to the same
    /// state that it was when the condition was added the first time.
    /// </remarks>
    public class DateTimeExpiredCondition : RatingConditionBase<DateTime>
    {
        private readonly TimeSpan _timeFromNowUtc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFromNowUtc">The amount of time to add to <see cref="DateTime.UtcNow"/> when setting the initial state of this condition.</param>
        public DateTimeExpiredCondition(TimeSpan timeFromNowUtc)
            : base(DateTime.UtcNow.Add(timeFromNowUtc), date => DateTime.UtcNow > date)
        {
            _timeFromNowUtc = timeFromNowUtc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFromNowUtc">The amount of time to add to <see cref="DateTime.UtcNow"/> when setting the initial state of this condition.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public DateTimeExpiredCondition(TimeSpan timeFromNowUtc, ConditionType conditionType)
            : base(DateTime.UtcNow.Add(timeFromNowUtc), date => DateTime.UtcNow > date, conditionType)
        {
            _timeFromNowUtc = timeFromNowUtc;
        }

        /// <summary>
        /// Reset will add the amount of time that was passed to the constructor of this condition to <see cref="DateTime.UtcNow"/> and set the result as current state.
        /// </summary>
        public override void Reset() => CurrentState = DateTime.UtcNow.Add(_timeFromNowUtc);
    }
}
