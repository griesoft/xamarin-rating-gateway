namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    public class StringMatchCondition : RatingConditionBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        public StringMatchCondition(string goal)
            : base(string.Empty, currentString => currentString == goal)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="conditionType"></param>
        public StringMatchCondition(string goal, ConditionType conditionType)
            : base(string.Empty, currentString => currentString == goal, conditionType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="initialString"></param>
        public StringMatchCondition(string goal, string initialString)
            : base(initialString, currentString => currentString == goal)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="initialString"></param>
        /// <param name="conditionType"></param>
        public StringMatchCondition(string goal, string initialString, ConditionType conditionType)
            : base(initialString, currentString => currentString == goal, conditionType)
        {

        }
    }
}
