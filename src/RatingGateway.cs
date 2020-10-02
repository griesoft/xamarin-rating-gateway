using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Griesoft.Xamarin.RatingGateway.Cache;
using Griesoft.Xamarin.RatingGateway.Conditions;

[assembly: InternalsVisibleTo("Griesoft.Xamarin.RatingGateway.Tests")]
namespace Griesoft.Xamarin.RatingGateway
{
    /// <summary>
    /// 
    /// </summary>
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
        /// 
        /// </summary>
        public IEnumerable<KeyValuePair<string, IRatingCondition>> RatingConditions => _ratingConditions.AsEnumerable();

        /// <summary>
        /// 
        /// </summary>
        public bool HasPrerequisiteConditions => _ratingConditions.Any(condition => condition.Value.ConditionType == ConditionType.Prerequisite);

        /// <summary>
        /// 
        /// </summary>
        public bool HasRequiredConditions => _ratingConditions.Any(condition => condition.Value.ConditionType == ConditionType.Requirement);

        /// <summary>
        /// 
        /// </summary>
        public bool HasOnlyPrerequisiteConditions => _ratingConditions.Count > 0 && _ratingConditions.All(condition => condition.Value.ConditionType == ConditionType.Prerequisite);

        /// <summary>
        /// 
        /// </summary>
        public IRatingView RatingView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRatingConditionCache RatingConditionCache { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static RatingGateway? Current { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <param name="condition"></param>
        /// <param name="ratingView"></param>
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
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="ratingView"></param>
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
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <param name="condition"></param>
        public void AddCondition(string conditionName, IRatingCondition condition)
        {
            _ratingConditions.Add(conditionName, condition);

            if (condition is ICachableCondition cachableCondition && cachableCondition.CacheCurrentValue)
            {
                RatingConditionCache.Load(conditionName, cachableCondition);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coniditions"></param>
        public void AddCondition(IEnumerable<KeyValuePair<string, IRatingCondition>> conditions)
        {
            foreach(var condition in conditions)
            {
                AddCondition(condition.Key, condition.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        public void RemoveCondition(string conditionName)
        {
            _ratingConditions.Remove(conditionName);
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        /// <param name="conditionName"></param>
        /// <param name="parameter"></param>
        /// <param name="manipulateOnly"></param>
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
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="manipulateOnly"></param>
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
