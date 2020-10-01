using System;
using System.Collections.Generic;
using Griesoft.Xamarin.RatingGateway.Conditions;
using Griesoft.Xamarin.RatingGateway.Interface;
using Moq;
using Xunit;

namespace Griesoft.Xamarin.RatingGateway.Tests
{
    public class RatingGatewayTests
    {
        [Fact]
        public void Initialize_Should_Construct()
        {
            // Arrange


            // Act
            RatingGateway.Initialize("Test", new BooleanRatingCondition());

            // Assert
            Assert.NotNull(RatingGateway.Current?.RatingView);
        }

        [Fact]
        public void HasPrerequisiteConditions_Returns_ExpectedResults()
        {
            // Arrange
            var gateway1 = new RatingGateway();
            var gateway2 = new RatingGateway();
            gateway1.AddCondition("Test", new BooleanRatingCondition(ConditionType.Prerequisite));
            gateway2.AddCondition("Test", new BooleanRatingCondition());

            // Act


            // Assert
            Assert.True(gateway1.HasPrerequisiteConditions);
            Assert.False(gateway2.HasPrerequisiteConditions);
        }

        [Fact]
        public void HasRequiredConditions_Returns_ExpectedResults()
        {
            // Arrange
            var gateway1 = new RatingGateway();
            var gateway2 = new RatingGateway();
            gateway1.AddCondition("Test", new BooleanRatingCondition(ConditionType.Requirement));
            gateway2.AddCondition("Test", new BooleanRatingCondition());

            // Act


            // Assert
            Assert.True(gateway1.HasRequiredConditions);
            Assert.False(gateway2.HasRequiredConditions);
        }

        [Fact]
        public void HasOnlyPrerequisiteConditions_Returns_ExpectedResuts()
        {
            // Arrange
            var gateway1 = new RatingGateway();
            var gateway2 = new RatingGateway();
            gateway1.AddCondition("Test", new BooleanRatingCondition(ConditionType.Prerequisite));
            gateway1.AddCondition("Test2", new BooleanRatingCondition(ConditionType.Prerequisite));
            gateway2.AddCondition("Test", new BooleanRatingCondition());
            gateway2.AddCondition("Test2", new BooleanRatingCondition(ConditionType.Prerequisite));

            // Act


            // Assert
            Assert.True(gateway1.HasOnlyPrerequisiteConditions);
            Assert.False(gateway2.HasOnlyPrerequisiteConditions);
        }

        [Fact]
        public void AddCondition_ThrowsOn_DuplicateKey()
        {
            // Arrange
            var conditions = new Dictionary<string, IRatingCondition>()
            {
                { "Tester", new BooleanRatingCondition() },
                { "Tester2", new BooleanRatingCondition() }
            };
            var gateway = new RatingGateway();
            gateway.AddCondition(conditions);

            // Act


            // Assert
            Assert.Throws<ArgumentException>(() => gateway.AddCondition(conditions));
            Assert.Throws<ArgumentException>(() => gateway.AddCondition("Tester", new BooleanRatingCondition()));
        }

        [Fact]
        public void RemoveCondition_IsSuccessful()
        {
            // Arrange
            var gateway = new RatingGateway();
            gateway.AddCondition("Test", new BooleanRatingCondition());

            // Act
            gateway.RemoveCondition("Test");

            // Assert
            Assert.Empty(gateway.RatingConditions);
        }

        [Fact]
        public void ResetAllConditions_IsSuccessful()
        {
            // Arrange
            var condition1 = new CountRatingCondition(0, 10);
            var condition2 = new CountRatingCondition(1, 7);
            var conditions = new Dictionary<string, IRatingCondition>()
            {
                { "Test1", condition1 },
                { "Test2", condition2 }
            };
            var gateway = new RatingGateway();
            gateway.AddCondition(conditions);
            condition1.ManipulateState(10);
            condition2.ManipulateState(7);

            // Act
            gateway.ResetAllConditions();

            // Assert
            Assert.False(condition1.IsConditionMet);
            Assert.False(condition2.IsConditionMet);
        }

        [Theory]
        [InlineData("Test", 8, false, false, 8)]
        [InlineData("Test", 5, true, false, 5)]
        [InlineData("Test", null, false, false, 1)]
        [InlineData("Test", null, false, true, 0)]
        [InlineData("Test", null, true, false, 1)]
        [InlineData(null, null, false, false, 1)]
        [InlineData(null, 5, true, false, 0)]
        [InlineData(null, 8, false, false, 1)]
        public void RatingActionTriggered_DoesManipulate_AsExpected(string? key, int? param, bool explicitOnly, bool disallowParameterless, int expected)
        {
            // Arrange
            var condition = new CountRatingCondition(0, 10)
            {
                ExplicitManipulationOnly = explicitOnly,
                DisallowParamaterlessManipulation = disallowParameterless
            };
            var gateway = new RatingGateway();
            gateway.AddCondition(key ?? nameof(CountRatingCondition), condition);

            // Act
            if (key == null)
            {
                gateway.RatingActionTriggered();
            }
            else
            {
                gateway.RatingActionTriggered(new Dictionary<string, object?>()
                {
                    { key, param }
                });
            }

            // Assert
            Assert.Equal(expected, condition.CurrentState);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_HasOnlyPrerequisites()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition(ConditionType.Prerequisite));

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_PrerequisitesNotMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new CountRatingCondition(0, 5, ConditionType.Prerequisite));
            gateway.AddCondition("test2", new BooleanRatingCondition());

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateSucceedsWhen_PrerequisitesAndStandardConditionsMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition(ConditionType.Prerequisite));
            gateway.AddCondition("test2", new BooleanRatingCondition());

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Once);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_PrerequisitesMetButStandardConditionNot()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition(ConditionType.Prerequisite));
            gateway.AddCondition("test2", new CountRatingCondition(0, 5));

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_RequiredConditionsNotMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition());
            gateway.AddCondition("test2", new CountRatingCondition(0, 5, ConditionType.Requirement));

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_NotAllRequiredMed()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition(ConditionType.Requirement));
            gateway.AddCondition("test2", new CountRatingCondition(0, 5, ConditionType.Requirement));

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateSucceedsWhen_RequiredMetAndNoPrioritySpecified()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition(ConditionType.Requirement));
            gateway.AddCondition("test2", new CountRatingCondition(0, 5));

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Once);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_RequiredMetAndPrioritySpecified()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition(ConditionType.Requirement));
            gateway.AddCondition("test2", new CountRatingCondition(0, 5));

            // Act
            gateway.RatingActionTriggered("test2");

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_PriorityNotMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition());
            gateway.AddCondition("test2", new CountRatingCondition(0, 5));

            // Act
            gateway.RatingActionTriggered("test2");

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_NotAllPrioritysAreMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition());
            gateway.AddCondition("test2", new CountRatingCondition(0, 5));
            gateway.AddCondition("test3", new BooleanRatingCondition());

            // Act
            gateway.RatingActionTriggered(new Dictionary<string, object?>()
            {
                { "test", null },
                { "test2", null }
            });

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_PriorityDoesNotExist()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new BooleanRatingCondition());
            gateway.AddCondition("test2", new CountRatingCondition(0, 5));
            gateway.AddCondition("test3", new BooleanRatingCondition());

            // Act
            gateway.RatingActionTriggered("random");

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateFailsWhen_NoConditionMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new CountRatingCondition(0, 5));
            gateway.AddCondition("test2", new CountRatingCondition(0, 10));

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Never);
        }

        [Fact]
        public void RatingActionTriggered_EvaluateSucceedsWhen_AnyConditionIsMet()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new CountRatingCondition(0, 5));
            gateway.AddCondition("test2", new BooleanRatingCondition());

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Once);
        }

        [Fact]
        public void RatingActionTriggered_ManipulationOnlyDoes_EvaluateWithoutPriorityCondition()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            gateway.AddCondition("test", new CountRatingCondition(0, 5));
            gateway.AddCondition("test2", new BooleanRatingCondition());

            // Act
            gateway.RatingActionTriggered("test", 3, true);

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Once);
        }

        [Fact]
        public void RatingActionTriggered_DoesResetAllMetConditions()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            var condition1 = new CountRatingCondition(0, 1, ConditionType.Requirement);
            var condition2 = new BooleanRatingCondition(ConditionType.Requirement);
            gateway.AddCondition("test", condition1);
            gateway.AddCondition("test2", condition2);

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Once);
            Assert.Equal(0, condition1.CurrentState);
            Assert.False(condition2.CurrentState);
        }

        [Fact]
        public void RatingActionTriggered_DoesResetAll_ExceptStrictlyForbiddenOnes()
        {
            // Arrange
            var ratingViewMock = new Mock<IRatingView>();
            var gateway = new RatingGateway
            {
                RatingView = ratingViewMock.Object
            };
            var condition1 = new CountRatingCondition(0, 1, ConditionType.Requirement);
            var condition2 = new BooleanRatingCondition(ConditionType.Requirement)
            {
                ResetAfterConditionMet = false
            };
            gateway.AddCondition("test", condition1);
            gateway.AddCondition("test2", condition2);

            // Act
            gateway.RatingActionTriggered();

            // Assert
            ratingViewMock.Verify(view => view.TryOpenRatingPage(), Times.Once);
            Assert.Equal(0, condition1.CurrentState);
            Assert.True(condition2.CurrentState);
        }
    }
}
