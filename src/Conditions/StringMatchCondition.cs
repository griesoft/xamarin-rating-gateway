using Griesoft.Xamarin.RatingGateway.Conditions;

namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// This condition evaluates to true when the current state matches the string that was passed to the constructor as the goal. 
    /// Use it to match user input or other system generated strings before prompting the user to review your application.
    /// </summary>
    public class StringMatchCondition : RatingConditionBase<string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal">The string that the current state needs to match in order for this condition to evaluate to true.</param>
        public StringMatchCondition(string goal)
            : base(string.Empty, currentString => currentString == goal)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal">The string that the current state needs to match in order for this condition to evaluate to true.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public StringMatchCondition(string goal, ConditionType conditionType)
            : base(string.Empty, currentString => currentString == goal, conditionType)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal">The string that the current state needs to match in order for this condition to evaluate to true.</param>
        /// <param name="initialString">The initial string for this condition.</param>
        public StringMatchCondition(string goal, string initialString)
            : base(initialString, currentString => currentString == goal)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goal">The string that the current state needs to match in order for this condition to evaluate to true.</param>
        /// <param name="initialString">The initial string for this condition.</param>
        /// <param name="conditionType">Specify the condition type.</param>
        public StringMatchCondition(string goal, string initialString, ConditionType conditionType)
            : base(initialString, currentString => currentString == goal, conditionType)
        {

        }
    }
}
