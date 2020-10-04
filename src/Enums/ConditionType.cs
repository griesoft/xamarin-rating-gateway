namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// Describes the type of the condition, which affects the way how it will be evaluated.
    /// </summary>
    public enum ConditionType
    {
        /// <summary>
        /// A standard condition which has no special role over other conditions.
        /// </summary>
        Standard,

        /// <summary>
        /// A prerequisite condition will be always evaluated first before other condition types.
        /// </summary>
        /// <remarks>
        /// A prerequisite is never enough to satisfy the evaluation process alone. So make sure that you add another type of condition
        /// to the collection or consider making it a <see cref="Requirement"/>.
        /// </remarks>
        Prerequisite,

        /// <summary>
        /// A requirement is a condition which is always required when evaluating. It is evaluated after <see cref="Prerequisite"/> conditions.
        /// </summary>
        /// <remarks>
        /// The required conditions are always evaluated after <see cref="Prerequisite"/> conditions and differ from them only in the way that they are
        /// enough to satisfy the evaluation process, where the prerequisite conditions do not.
        /// </remarks>
        Requirement
    }
}
