using System;

namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeExpiredCondition : RatingConditionBase<DateTime>
    {
        private readonly TimeSpan _timeFromNowUtc;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFromNowUtc"></param>
        public DateTimeExpiredCondition(TimeSpan timeFromNowUtc)
            : base(DateTime.UtcNow.Add(timeFromNowUtc), date => DateTime.UtcNow > date)
        {
            _timeFromNowUtc = timeFromNowUtc;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeFromNowUtc"></param>
        /// <param name="conditionType"></param>
        public DateTimeExpiredCondition(TimeSpan timeFromNowUtc, ConditionType conditionType)
            : base(DateTime.UtcNow.Add(timeFromNowUtc), date => DateTime.UtcNow > date, conditionType)
        {
            _timeFromNowUtc = timeFromNowUtc;
        }

        /// <inheritdoc/>
        public override void Reset()
        {
            CurrentState = DateTime.UtcNow.Add(_timeFromNowUtc);
        }
    }
}
