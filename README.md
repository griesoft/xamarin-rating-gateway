# Xamarin Rating Gateway
A rating gateway which takes care of when to prompt the user to review your Xamarin application by evaluating through a set of defined conditions each time a rating action is triggered by the user.

Catching the perfect moment to prompt your user to review your application can be a very difficult task and each failed attempt can be big wasted potential. This rating gateway helps you to set up even complex conditional triggers that will prompt the user to review the app when all of the trivial conditions are met.

[![Build Status](https://dev.azure.com/griesingersoftware/Xamarin%20Rating%20Gateway/_apis/build/status/CI%20Pipeline?branchName=master)](https://dev.azure.com/griesingersoftware/Xamarin%20Rating%20Gateway/_build/latest?definitionId=26&branchName=master)
[![License](https://badgen.net/github/license/griesoft/xamarin-rating-gateway)](https://github.com/griesoft/xamarin-rating-gateway/blob/master/LICENSE)
[![NuGet](https://badgen.net/nuget/v/Griesoft.Xamarin.RatingGateway)](https://www.nuget.org/packages/Griesoft.Xamarin.RatingGateway)
[![GitHub Release](https://badgen.net/github/release/griesoft/xamarin-rating-gateway)](https://github.com/griesoft/xamarin-rating-gateway/releases)

## Installation

Install via [NuGet](https://www.nuget.org/packages/Griesoft.Xamarin.RatingGateway/) using:

``PM> Install-Package Griesoft.Xamarin.RatingGateway``

*Make sure that you add this package to each platform that you intend to use it on.*

## Quickstart

Initialize the rating gateway on application startup as soon as possible with 
```csharp
RatingGateway.Initialize("YourCondition", new BooleanRatingCondition())
```
You can also add multiple conditions at once by using the overload of the static `RatingGateway.Initialize(...)` method.

#### Android
In the `OnCreate` of your startup **Activity**:
```csharp
protected override void OnCreate(Bundle savedInstanceState)
{
    RatingGateway.Initialize("YourCondition", new BooleanRatingCondition());
}
```
#### iOS
In the `FinishedLaunching` of your **AppDelegate**:
```csharp
[Register ("AppDelegate")]
public class AppDelegate : ApplicationDelegate
{
    public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
    {
        RatingGateway.Initialize(new Dictionary<string, IRatingConditions>()
        {
            { "FirstCondition", CountRatingCondition(0, 2) },
            { "SecondCondition", BooleanRatingConditions() }
        });
        
        return base.FinishedLaunching(application, launchOptions);
    }
}
```

Now that you are all set up, use `RatingGateway.Current?.Evaluate()` each time that a user has triggered an event in your application that should prompt them to review your app.

## Rating Conditions

This package has 5 build-in conditions. These conditions do cover many common use cases.

##### BooleanRatingCondition
Use it as a switch, to memorize certain actions that your user did in your app.

##### CountRatingCondition
Use it to count certain actions that your user did in your app. The condition is met by default when the count is equal to or greater than the specified goal.

##### DateTimeExpiredCondition
Use it to mark a certain point in time that needs to be in the past for this condition to be met.

##### StringMatchCondition
Use it to match strings with each other, like user input for example.

##### RatingCondition
This is a generic condition that can be used to effortlessly create your conditions. Some functionality can be only customized by writing your own condition class thou.

All conditions need to implement the `IRatingCondition` interface. You can create custom conditions by implementing it. The best approach to that would be to inherit from the abstract `RatingConditionBase` class.

## Documentation
Is on it's why...

## Supported Platforms
Currently this package supports **NetStandard**, **Android 9**, **Android 10** and **iOS**.

## Contributing
Contributing is heavily encouraged. The best way of doing so is probably by first start a discussion about new features or improvements, or if you found a bug report it first by creating a new issue first. There you can volunteer to make it your mission to fix it. :smile:

## License
Please see the [License](https://github.com/griesoft/xamarin-rating-gateway/blob/master/LICENSE).
