using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class RatingGateway
    {
        private readonly Dictionary<string, IRatingCondition> _ratingConditions;

        internal RatingGateway()
        {
            _ratingConditions = new Dictionary<string, IRatingCondition>();

            RatingView = new DefaultRatingView();
            RatingConditionCache = new DefaultRatingConditionCache();
        }

        /// <summary>
        /// The condition collection returned as an enumerable.
        /// </summary>
        public IEnumerable<KeyValuePair<string, IRatingCondition>> RatingConditions => _ratingConditions.AsEnumerable();

        /// <summary>
        /// True if the condition collection contains any condition that is of type <see cref="ConditionType.Prerequisite"/>.
        /// </summary>
        public bool HasPrerequisiteConditions => _ratingConditions.Any(condition => condition.Value.ConditionType == ConditionType.Prerequisite);

        /// <summary>
        /// True if the condition collection contains any condition that is of type <see cref="ConditionType.Requirement"/>.
        /// </summary>
        public bool HasRequiredConditions => _ratingConditions.Any(condition => condition.Value.ConditionType == ConditionType.Requirement);

        /// <summary>
        /// True if the condition collection contains only conditions that are of type <see cref="ConditionType.Prerequisite"/>.
        /// </summary>
        /// <remarks>
        /// Make sure that this always returns false. If it doesn't, the rating gateway will never prompt the user to review your application. 
        /// </remarks>
        public bool HasOnlyPrerequisiteConditions => _ratingConditions.Count > 0 && _ratingConditions.All(condition => condition.Value.ConditionType == ConditionType.Prerequisite);

        /// <summary>
        /// The rating view that does display the rating page to the user.
        /// </summary>
        public IRatingView RatingView { get; set; }

        /// <summary>
        /// The rating condition cache that does manage the reading and writing of cached values to the file system.
        /// </summary>
        public IRatingConditionCache RatingConditionCache { get; set; }

        /// <summary>
        /// The singleton instance of this rating gateway. Returns null if the service was not initialized on application startup.
        /// </summary>
        public static RatingGateway? Current { get; private set; }

        /// <summary>
        /// Initialize the rating gateway with a single condition.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="condition">The condition instance that will be added to the collection.</param>
        /// <param name="ratingView">Optional; pass a custom rating view. If not specified or null, <see cref="DefaultRatingView"/> will be used instead.</param>
        /// <param name="ratingCache">Optional; pass a custom condition cache. If not specified or null, <see cref="DefaultRatingConditionCache"/> will be used instead.</param>
        /// <exception cref="System.ArgumentException">Thrown if a condition with the given name already exists in the collection.</exception>
        public static void Initialize(string conditionName, IRatingCondition condition, IRatingView? ratingView = default, IRatingConditionCache? ratingCache = default)
        {
            var ratingGateway = CreateNewGatewayInstance();

            ratingGateway.AddCondition(conditionName, condition);

            if (ratingView != null)
            {
                ratingGateway.RatingView = ratingView;
            }

            if (ratingCache != null)
            {
                ratingGateway.RatingConditionCache = ratingCache;
            }
        }

        /// <summary>
        /// Initialize the rating gateway with a collection of conditions.
        /// </summary>
        /// <param name="conditions">A collection of key value pairs where the key will be used as the unique condition name and the value is added to the condition collection.</param>
        /// <param name="ratingView">Optional; pass a custom rating view. If not specified or null, <see cref="DefaultRatingView"/> will be used instead.</param>
        /// <param name="ratingCache">Optional; pass a custom condition cache. If not specified or null, <see cref="DefaultRatingConditionCache"/> will be used instead.</param>
        /// <exception cref="System.ArgumentException">Thrown if a condition with the given name already exists in the collection.</exception>
        public static void Initialize(IEnumerable<KeyValuePair<string, IRatingCondition>> conditions, IRatingView? ratingView = default, IRatingConditionCache? ratingCache = default)
        {
            var ratingGateway = CreateNewGatewayInstance();

            ratingGateway.AddCondition(conditions);

            if (ratingView != null)
            {
                ratingGateway.RatingView = ratingView;
            }

            if (ratingCache != null)
            {
                ratingGateway.RatingConditionCache = ratingCache;
            }
        }

        /// <summary>
        /// Add a condition to the collection.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="condition">The condition instance that will be added to the collection.</param>
        /// <exception cref="System.ArgumentException">Thrown if a condition with the given name already exists in the collection.</exception>
        public void AddCondition(string conditionName, IRatingCondition condition)
        {
            _ratingConditions.Add(conditionName, condition);

            if (condition is ICachableCondition cachableCondition && cachableCondition.CacheCurrentValue)
            {
                if (!RatingConditionCache.Load(conditionName, cachableCondition))
                {
                    RatingConditionCache.Save(conditionName, cachableCondition);
                }
            }
        }

        /// <summary>
        /// Add multiple conditions to the collection at once.
        /// </summary>
        /// <param name="conditions">A collection of key-value pairs where the key is used as the unique condition name and the value is added to the condition collection.</param>
        /// <exception cref="System.ArgumentException">Thrown if a condition with the given name already exists in the collection.</exception>
        public void AddCondition(IEnumerable<KeyValuePair<string, IRatingCondition>> conditions)
        {
            foreach(var condition in conditions)
            {
                AddCondition(condition.Key, condition.Value);
            }
        }

        /// <summary>
        /// Remove a condition with the specified name from the collection if it exists.
        /// </summary>
        /// <param name="conditionName">The unique name of the condition.</param>
        /// <param name="removeFromCache">If true a cached state of the condition will also be removed from the cache. By default true.</param>
        public void RemoveCondition(string conditionName, bool removeFromCache = true)
        {
            _ratingConditions.Remove(conditionName);

            if (removeFromCache)
            {
                RatingConditionCache.Delete(conditionName);
            }
        }

        /// <summary>
        /// Reset all condition states in the collection.
        /// </summary>
        public void ResetAllConditions()
        {
            foreach(var condition in _ratingConditions)
            {
                condition.Value.Reset();

                SaveCachableConditionState(condition.Key, condition.Value);
            }
        }

        /// <summary>
        /// Evaluate through all conditions in the collection and if all necessary conditions are met open the rating page.
        /// </summary>
        /// <remarks>
        /// The evaluation process will first manipulate all conditions that allow implicit manipulation by using their parameterless manipulation method.
        /// After manipulation, the actual evaluation will happen, and after evaluation has finished all met conditions will be reset which allow automatic reset.
        /// </remarks>
        public void RatingActionTriggered()
        {
            ManipulateConditionState(null);

            if (EvaluateConditions())
            {
                RatingView.TryOpenRatingPage();
            }

            ResetAllMetConditions();
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
        public void RatingActionTriggered(string conditionName, object? parameter = default, bool manipulateOnly = false)
        {
            ManipulateConditionState(new Dictionary<string, object?>() { { conditionName, parameter } });

            if (EvaluateConditions(manipulateOnly ? null : new List<string>() { conditionName }))
            {
                RatingView.TryOpenRatingPage();
            }

            ResetAllMetConditions();
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
        public void RatingActionTriggered(Dictionary<string, object?> parameters, bool manipulateOnly = false)
        {
            ManipulateConditionState(parameters);

            if (EvaluateConditions(manipulateOnly ? null : parameters?.Keys))
            {
                RatingView.TryOpenRatingPage();
            }

            ResetAllMetConditions();
        }

        private static RatingGateway CreateNewGatewayInstance()
        {
            if (Current == null)
            {
                Current = new RatingGateway();
            }

            return Current;
        }

        private void ResetAllMetConditions()
        {
            foreach(var condition in _ratingConditions.Where(con => con.Value.ResetAfterConditionMet && con.Value.IsConditionMet))
            {
                condition.Value.Reset();

                SaveCachableConditionState(condition.Key, condition.Value);
            }
        }
        private void ManipulateConditionState(Dictionary<string, object?>? parameters)
        {
            foreach(var condition in _ratingConditions)
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
                !_ratingConditions.Where(condition => condition.Value.ConditionType == ConditionType.Prerequisite)
                .All(condition => condition.Value.IsConditionMet))
            {
                return false;
            }

            var hasPriorConditions = priorityConditions != null && priorityConditions.Count() > 0;

            // Make sure all required conditions are met before proceeding
            if (!_ratingConditions.Where(condition => condition.Value.ConditionType == ConditionType.Requirement)
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
                return priorityConditions.All(conditionName => _ratingConditions.ContainsKey(conditionName)
                    ? _ratingConditions[conditionName].IsConditionMet
                    : false);
            }

            // No condition was prioritized, so it is enough for any standard condition to be true
            return _ratingConditions.Any(condition => condition.Value.ConditionType == ConditionType.Standard &&
                condition.Value.IsConditionMet);
        }
        private void SaveCachableConditionState(string conditionName, IRatingCondition condition)
        {
            if (condition is ICachableCondition cachableCondition && cachableCondition.CacheCurrentValue)
            {
                RatingConditionCache.Save(conditionName, cachableCondition);
            }
        }
    }
}
