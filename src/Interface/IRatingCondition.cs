namespace Griesoft.Xamarin.RatingGateway.Conditions
{
    /// <summary>
    /// The interface that a condition should implement for usage with the <see cref="RatingGateway"/>.
    /// </summary>
    public interface IRatingCondition
    {
        /// <summary>
        /// If set to false the evaluation process will not reset the value of a condition to its initial value after <see cref="IsConditionMet"/> is true.
        /// </summary>
        bool ResetAfterConditionMet { get; set; }

        /// <summary>
        /// Set to true to tell the evaluation process to only manipulate the current state if you explicitly tell it to do so. 
        /// </summary>
        bool ExplicitManipulationOnly { get; set; }

        /// <summary>
        /// Set to true to tell the evaluation process that parameterless manipulation is forbidden.
        /// </summary>
        bool DisallowParamaterlessManipulation { get; set; }

        /// <summary>
        /// The type of the condition, which specifies how the evaluation process will handle it.
        /// </summary>
        ConditionType ConditionType { get; }

        /// <summary>
        /// True if the goal of the condition is met.
        /// </summary>
        bool IsConditionMet { get; }

        /// <summary>
        /// Reset the current value of the condition.
        /// </summary>
        void Reset();

        /// <summary>
        /// Manipulate the current state by a predefined constant factor. The default implementation of this method does nothing.
        /// </summary>
        void ManipulateState();

        /// <summary>
        /// Manipulate the current value by a specified parameter.
        /// </summary>
        /// <param name="parameter">The value to use for state manipulation.</param>
        void ManipulateState(object parameter);
    }
}
