﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Griesoft.Xamarin.RatingGateway.Cache;
using Griesoft.Xamarin.RatingGateway.Conditions;

[assembly: InternalsVisibleTo("Griesoft.Xamarin.RatingGateway.Tests")]
namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// The rating gateway manages when a <see cref="IRatingView"/> should open a rating page, based on a <see cref="IRatingCondition"/> collection. The gateway is
    /// a singleton service and you should initialize the service with one of the initialize methods <see cref="Initialize(string, IRatingCondition, IRatingView?, IRatingConditionCache?)"/>
    /// <see cref="Initialize(IEnumerable{KeyValuePair{string, IRatingCondition}}, IRatingView?, IRatingConditionCache?)"/>.
    /// <para/>
    /// After initialization, you can access the service via the <see cref="Current"/> property.
    /// </summary>
    /// <remarks>
    /// Each time a rating action is triggered the gateway will manipulate conditions by given factors, evaluate through them to check if all necessary conditions are met,
    /// reset met conditions if allowed, and cache the current state of cachable conditions.
    /// </remarks>
    public class RatingGateway : ConditionCollection
    {
        internal RatingGateway() : base()
        {
            RatingView = new DefaultRatingView();
        }

        private bool IsInitialized { get; set; } = false;

        /// <summary>
        /// The rating view that does display the rating page to the user.
        /// </summary>
        public IRatingView RatingView { get; set; }

        /// <summary>
        /// A singleton instance of the rating gateway.
        /// </summary>
        public static RatingGateway Current { get; } = new RatingGateway();

        /// <summary>
        /// Initialize the rating gateway with a single condition.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="condition">The condition instance that will be added to the collection.</param>
        /// <param name="ratingView">Optional; pass a custom rating view. If not specified or null, <see cref="DefaultRatingView"/> will be used instead.</param>
        /// <param name="ratingCache">Optional; pass a custom condition cache. If not specified or null, <see cref="DefaultRatingConditionCache"/> will be used instead.</param>
        /// <remarks>Call it only once in the lifetime of a <see cref="RatingGateway"/>. All other calls after the initial one, do just return.</remarks>
        public static void Initialize(string conditionName, IRatingCondition condition, IRatingView? ratingView = default, IRatingConditionCache? ratingCache = default)
        {
            if (Current.IsInitialized)
            {
                return;
            }

            Current.IsInitialized = true;

            Current.AddCondition(conditionName, condition);

            if (ratingView != null)
            {
                Current.RatingView = ratingView;
            }

            if (ratingCache != null)
            {
                Current.RatingConditionCache = ratingCache;
            }
        }

        /// <summary>
        /// Initialize the rating gateway with a collection of conditions.
        /// </summary>
        /// <param name="conditions">A collection of key value pairs where the key will be used as the unique condition name and the value is added to the condition collection.</param>
        /// <param name="ratingView">Optional; pass a custom rating view. If not specified or null, <see cref="DefaultRatingView"/> will be used instead.</param>
        /// <param name="ratingCache">Optional; pass a custom condition cache. If not specified or null, <see cref="DefaultRatingConditionCache"/> will be used instead.</param>
        /// <remarks>Call it only once in the lifetime of a <see cref="RatingGateway"/>. All other calls after the initial one, do just return.</remarks>
        public static void Initialize(IEnumerable<KeyValuePair<string, IRatingCondition>> conditions, IRatingView? ratingView = default, IRatingConditionCache? ratingCache = default)
        {
            if (Current.IsInitialized)
            {
                return;
            }

            Current.IsInitialized = true;

            Current.AddCondition(conditions);

            if (ratingView != null)
            {
                Current.RatingView = ratingView;
            }

            if (ratingCache != null)
            {
                Current.RatingConditionCache = ratingCache;
            }
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page.
        /// </summary>
        /// <remarks>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// </remarks>
        public void Evaluate()
        {
            ManipulateConditionState(null);

            var evaluationResult = EvaluateConditions();

            if (evaluationResult)
            {
                RatingView.TryOpenRatingPage();
            }

            ResetAllMetConditions(evaluationResult);
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition that should be prioritized.</param>
        /// <param name="parameter">An optional parameter that will be passed to the manipulation process for the specified condition.</param>
        /// <param name="manipulateOnly">Set to true if the specified condition should not be prioritized in the actual evaluation phase and be used for manipulation only. The default is false.</param>
        /// <remarks>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// <para/>
        /// If the specified condition is not used for manipulation only, it will be prioritized by the evaluator. This means that the prioritized condition must be
        /// met in addition to all prerequisite and required conditions, before evaluating to true.
        /// </remarks>
        public void Evaluate(string conditionName, object? parameter = default, bool manipulateOnly = false)
        {
            ManipulateConditionState(new Dictionary<string, object?>() { { conditionName, parameter } });

            var evaluationResult = EvaluateConditions(manipulateOnly ? null : new List<string>() { conditionName });

            if (evaluationResult)
            {
                RatingView.TryOpenRatingPage();
            }

            ResetAllMetConditions(evaluationResult);
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page.
        /// </summary>
        /// <param name="parameters">A collection of unique condition names and optional parameters for them.</param>
        /// <param name="manipulateOnly">Set to true if the specified conditions should not be prioritized in the actual evaluation phase and be used for manipulation only. The default is false.</param>
        /// <remarks>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// <para/>
        /// If the specified conditions are not used for manipulation only, they will be prioritized by the evaluator. This means that the prioritized conditions must be
        /// met in addition to all prerequisite and required conditions, before evaluating to true.
        /// </remarks>
        public void Evaluate(Dictionary<string, object?> parameters, bool manipulateOnly = false)
        {
            ManipulateConditionState(parameters);

            var evaluationResult = EvaluateConditions(manipulateOnly ? null : parameters?.Keys);

            if (evaluationResult)
            {
                RatingView.TryOpenRatingPage();
            }

            ResetAllMetConditions(evaluationResult);
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page asynchronously.
        /// </summary>
        /// <remarks>
        /// Use <see cref="Evaluate"/> if you are not sure about which one to use.
        /// <para/>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// </remarks>
        public async Task EvaluateAsync()
        {
            ManipulateConditionState(null);

            var evaluationResult = EvaluateConditions();

            if (evaluationResult)
            {
                await RatingView.TryOpenRatingPageAsync();
            }

            ResetAllMetConditions(evaluationResult);
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page asynchronously.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition that should be prioritized.</param>
        /// <param name="parameter">An optional parameter that will be passed to the manipulation process for the specified condition.</param>
        /// <param name="manipulateOnly">Set to true if the specified condition should not be prioritized in the actual evaluation phase and be used for manipulation only. The default is false.</param>
        /// <remarks>
        /// Use <see cref="Evaluate(string, object?, bool)"/> if you are not sure about which one to use.
        /// <para/>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// <para/>
        /// If the specified condition is not used for manipulation only, it will be prioritized by the evaluator. This means that the prioritized condition must be
        /// met in addition to all prerequisite and required conditions, before evaluating to true.
        /// </remarks>
        public async Task EvaluateAsync(string conditionName, object? parameter = default, bool manipulateOnly = false)
        {
            ManipulateConditionState(new Dictionary<string, object?>() { { conditionName, parameter } });

            var evaluationResult = EvaluateConditions(manipulateOnly ? null : new List<string>() { conditionName });

            if (evaluationResult)
            {
                await RatingView.TryOpenRatingPageAsync();
            }

            ResetAllMetConditions(evaluationResult);
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page asynchronously.
        /// </summary>
        /// <param name="parameters">A collection of unique condition names and optional parameters for them.</param>
        /// <param name="manipulateOnly">Set to true if the specified conditions should not be prioritized in the actual evaluation phase and be used for manipulation only. The default is false.</param>
        /// <remarks>
        /// Use <see cref="Evaluate(Dictionary{string, object?}, bool)"/> if you are not sure about which one to use.
        /// <para/>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// <para/>
        /// If the specified conditions are not used for manipulation only, they will be prioritized by the evaluator. This means that the prioritized conditions must be
        /// met in addition to all prerequisite and required conditions, before evaluating to true.
        /// </remarks>
        public async Task EvaluateAsync(Dictionary<string, object?> parameters, bool manipulateOnly = false)
        {
            ManipulateConditionState(parameters);

            var evaluationResult = EvaluateConditions(manipulateOnly ? null : parameters?.Keys);

            if (evaluationResult)
            {
                await RatingView.TryOpenRatingPageAsync();
            }

            ResetAllMetConditions(evaluationResult);
        }

        private void ResetAllMetConditions(bool evaluationResult)
        {
            foreach(var condition in RatingConditions.Where(con => con.Value.ResetAfterConditionMet && con.Value.IsConditionMet))
            {
                if (condition.Value.ResetOnlyOnEvaluationSuccess && !evaluationResult)
                {
                    continue;
                }

                condition.Value.Reset();

                SaveCachableConditionState(condition.Key, condition.Value);
            }
        }
        private void ManipulateConditionState(Dictionary<string, object?>? parameters)
        {
            foreach(var condition in RatingConditions)
            {
                var containsKey = parameters?.ContainsKey(condition.Key) ?? false;
                var conditionParam = containsKey ? parameters?[condition.Key] : null;

                if (condition.Value.ExplicitManipulationOnly && !containsKey)
                {
                    continue;
                }
                else if (!(conditionParam is null))
                {
                    condition.Value.ManipulateState(conditionParam);
                    SaveCachableConditionState(condition.Key, condition.Value);
                }
                else if (!condition.Value.DisallowParamaterlessManipulation)
                {
                    condition.Value.ManipulateState();
                    SaveCachableConditionState(condition.Key, condition.Value);
                }
            }
        }
        private bool EvaluateConditions(IEnumerable<string>? priorityConditions = default)
        {
            // Make sure all prerequisite conditions are met before proceeding
            if (HasOnlyPrerequisiteConditions || 
                !RatingConditions.Where(condition => condition.Value.ConditionType == ConditionType.Prerequisite)
                .All(condition => condition.Value.IsConditionMet))
            {
                return false;
            }

            var hasPriorConditions = priorityConditions != null && priorityConditions.Count() > 0;

            // Make sure all required conditions are met before proceeding
            if (!RatingConditions.Where(condition => condition.Value.ConditionType == ConditionType.Requirement)
                .All(condition => condition.Value.IsConditionMet))
            {
                return false;
            }
            else if(HasRequiredConditions && !hasPriorConditions)
            {
                // If all required conditions are met and we where not seeking for a specific conditions to be met,
                // i.e. no standard or required condition was prioritized, we can just return true.
                return true;
            }

            // Check that the condition of all specified conditions is met if we have any specified.
            if (hasPriorConditions)
            {
                return priorityConditions.All(conditionName => ContainsKey(conditionName)
                    ? this[conditionName].IsConditionMet
                    : false);
            }

            // No condition was prioritized, so it is enough for any standard condition to be true
            return RatingConditions.Any(condition => condition.Value.ConditionType == ConditionType.Standard &&
                condition.Value.IsConditionMet);
        }
    }
}
