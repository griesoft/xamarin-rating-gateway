using System;
using System.Collections.Generic;
using Griesoft.Xamarin.RatingGateway;
using Griesoft.Xamarin.RatingGateway.Conditions;

namespace RatingGatewaySamples.Droid
{
    public static class RatingGatewaySetup
    {
        public static Dictionary<string, IRatingCondition> RatingConditions =>
            new Dictionary<string, IRatingCondition>()
            {
                {
                    "NeverAskAgain",
                    new BooleanRatingCondition(false, state => !state, ConditionType.Prerequisite)
                    {
                        ResetAfterConditionMet = false,
                        ExplicitManipulationOnly = true,
                        DisallowParamaterlessManipulation = true
                    }
                },
                {
                    "CooldownBetweenRequests",
                    new DateTimeExpiredCondition(TimeSpan.FromMinutes(1), ConditionType.Prerequisite)
                    {
                        ResetOnlyOnEvaluationSuccess = false
                    }
                },
                {
                    "ClickCount",
                    new CountRatingCondition(0, 2)
                    {
                        ExplicitManipulationOnly = true
                    }
                }
            };
    }
}
