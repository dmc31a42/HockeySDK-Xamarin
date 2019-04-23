using System;

using AppKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace HockeyApp.Mac
{
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    public partial interface BITHockeyManager
    {
        [Static, Export("sharedHockeyManager")]
        BITHockeyManager SharedHockeyManager { get; }
        [Export("crashManager")]
        BITCrashManager CrashManager { get; }
        // @property (readonly, nonatomic, strong) BITMetricsManager * metricsManager;
        [Export("metricsManager", ArgumentSemantic.Strong)]
        BITMetricsManager MetricsManager { get; }
        [Export("configureWithIdentifier:")]
        void ConfigureWithIdentifier(NSString appIdentifier);
        [Export("startManager")]
        void DoStartManager();
    }

    [BaseType(typeof(NSObject))]
    public partial interface BITCrashManager
    {
        [Export("setAutoSubmitCrashReport:")]
        void SetAutoSubmitCrashReport(bool flag);

        // @property (readonly, nonatomic) BOOL didCrashInLastSession;
        [Export("didCrashInLastSession")]
        bool DidCrashInLastSession { get; }
    }

    // @interface BITHockeyBaseManager : NSObject
    [BaseType(typeof(NSObject))]
    public partial interface BITHockeyBaseManager
    {
        // @property (copy, nonatomic) NSString * serverURL;
        [Export("serverURL")]
        string ServerURL { get; set; }
    }

    // @interface BITMetricsManager : BITHockeyBaseManager
    [BaseType(typeof(BITHockeyBaseManager))]
    public partial interface BITMetricsManager
    {
        // @property (assign, nonatomic) BOOL disabled;
        [Export("disabled")]
        bool Disabled { get; set; }

        // -(void)trackEventWithName:(NSString * _Nonnull)eventName;
        [Export("trackEventWithName:")]
        void TrackEvent(string eventName);

        // -(void)trackEventWithName:(NSString * _Nonnull)eventName properties:(NSDictionary<NSString *,NSString *> * _Nullable)properties measurements:(NSDictionary<NSString *,NSNumber *> * _Nullable)measurements;
        [Export("trackEventWithName:properties:measurements:")]
        void TrackEvent(string eventName, [NullAllowed] NSDictionary properties, [NullAllowed] NSDictionary measurements);
    }
}
