using System;
using System.Runtime.InteropServices;
using AppKit;
using Foundation;
using ObjCRuntime;
using CoreGraphics;

namespace HockeyApp.Mac
{
    // typedef NSString * (^BITLogMessageProvider)();
    public delegate string BITLogMessageProvider();

    // typedef void (^BITLogHandler)(BITLogMessageProvider, BITLogLevel, const char *, const char *, uint);
    public unsafe delegate void BITLogHandler([BlockCallback] BITLogMessageProvider messageProvider, BITLogLevel logLevel, IntPtr file, IntPtr function, uint line);

    // typedef void (*BITCrashManagerPostCrashSignalCallback)(void *context);
    public delegate void BITCrashManagerPostCrashSignalCallback(IntPtr context);

    // typedef void (^BITCustomCrashReportUIHandler)(NSString *, NSString *);
    public delegate void BITCustomCrashReportUIHandler(string crashReportText, string applicationLog);

    //[StructLayout(LayoutKind.Sequential)]
    //public struct BITCrashManagerCallbacks
    //{
    //    // public void* context;
    //    public IntPtr context;
    //    // public BITCrashManagerPostCrashSignalCallback* handleSignal;
    //    public BITCrashManagerPostCrashSignalCallback handleSignal;
    //}

    [Static]
    partial interface Constants
    {
        // extern NSString *const kBITCrashErrorDomain;
        [Field("kBITCrashErrorDomain", "__Internal")]
        NSString kBITCrashErrorDomain { get; }

        // extern NSString *const kBITFeedbackErrorDomain;
        [Field("kBITFeedbackErrorDomain", "__Internal")]
        NSString kBITFeedbackErrorDomain { get; }

        // extern NSString *const kBITHockeyErrorDomain __attribute__((unused));
        [Field("kBITHockeyErrorDomain", "__Internal")]
        NSString kBITHockeyErrorDomain { get; }

        // extern NSString *const kBITDefaultUserID;
        [Field("kBITDefaultUserID", "__Internal")]
        NSString kBITDefaultUserID { get; }

        // extern NSString *const kBITDefaultUserName;
        [Field("kBITDefaultUserName", "__Internal")]
        NSString kBITDefaultUserName { get; }

        // extern NSString *const kBITDefaultUserEmail;
        [Field("kBITDefaultUserEmail", "__Internal")]
        NSString kBITDefaultUserEmail { get; }
    }

    // @interface BITHockeyBaseManager : NSObject
    [BaseType(typeof(NSObject))]
    public partial interface BITHockeyBaseManager
    {
        // @property (copy, nonatomic) NSString * serverURL;
        [Export("serverURL")]
        string ServerURL { get; set; }
    }

    // @interface BITHockeyManager : NSObject
    [BaseType(typeof(NSObject))]
    [DisableDefaultCtor]
    public partial interface BITHockeyManager
    {
        // +(BITHockeyManager *)sharedHockeyManager;
        [Static]
        [Export("sharedHockeyManager")]
        BITHockeyManager SharedHockeyManager { get; }

        [Wrap("WeakDelegate")]
        BITHockeyManagerDelegate Delegate { get; set; }

        // @property (nonatomic, weak) id<BITHockeyManagerDelegate> delegate;
        [Export("delegate", ArgumentSemantic.Weak)]
        [NullAllowed]
        NSObject WeakDelegate { get; set; }

        // -(void)configureWithIdentifier:(NSString *)appIdentifier;
        [Export("configureWithIdentifier:")]
        void Configure(string appIdentifier);

        // -(void)configureWithIdentifier:(NSString *)appIdentifier delegate:(id<BITHockeyManagerDelegate>)delegate;
        [Export("configureWithIdentifier:delegate:")]
        void Configure(string appIdentifier, BITHockeyManagerDelegate @delegate);

        // -(void)configureWithIdentifier:(NSString *)appIdentifier companyName:(NSString *)companyName delegate:(id<BITHockeyManagerDelegate>)delegate __attribute__((deprecated("Use configureWithIdentifier:delegate: instead")));
        [Export("configureWithIdentifier:companyName:delegate:")]
        void ConfigureWithIdentifier(string appIdentifier, string companyName, BITHockeyManagerDelegate @delegate);

        // -(void)startManager;
        [Export("startManager")]
        void DoStartManager();

        // @property (copy, nonatomic) NSString * serverURL;
        [Export("serverURL")]
        string ServerURL { get; set; }

#if !FEEDBACKONLY
        // @property (readonly, nonatomic, strong) BITCrashManager * crashManager;
        [Export("crashManager", ArgumentSemantic.Strong)]
        BITCrashManager CrashManager { get; }

        // @property (getter = isCrashManagerDisabled, nonatomic) BOOL disableCrashManager;
        [Export("disableCrashManager")]
        bool DisableCrashManager { [Bind("isCrashManagerDisabled")] get; set; }
#endif

#if !CRASHONLY
        // @property (readonly, nonatomic, strong) BITFeedbackManager * feedbackManager;
        [Export("feedbackManager", ArgumentSemantic.Strong)]
        BITFeedbackManager FeedbackManager { get; }

        // @property (getter = isFeedbackManagerDisabled, nonatomic) BOOL disableFeedbackManager;
        [Export("disableFeedbackManager")]
        bool DisableFeedbackManager { [Bind("isFeedbackManagerDisabled")] get; set; }
#endif

#if !CRASHONLY && !FEEDBACKONLY
        // @property (readonly, nonatomic, strong) BITMetricsManager * metricsManager;
        [Export("metricsManager", ArgumentSemantic.Strong)]
        BITMetricsManager MetricsManager { get; }

        // @property (getter = isMetricsManagerDisabled, nonatomic) BOOL disableMetricsManager;
        [Export("disableMetricsManager")]
        bool DisableMetricsManager 
        {
            [Bind("isMetricsManagerDisabled")] 
            get; 
            set; 
        }
#endif

        // @property (assign, nonatomic) BITLogLevel logLevel;
        [Export("logLevel", ArgumentSemantic.Assign)]
        BITLogLevel LogLevel { get; set; }

        // @property (getter = isDebugLogEnabled, assign, nonatomic) BOOL debugLogEnabled __attribute__((deprecated("Use logLevel instead!")));
        [Export("debugLogEnabled")]
        bool DebugLogEnabled { [Bind("isDebugLogEnabled")] get; set; }

        // -(void)setLogHandler:(BITLogHandler)logHandler;
        [Export("setLogHandler:")]
        void SetLogHandler(BITLogHandler logHandler);

        // -(void)setUserID:(NSString *)userID;
        [Export("setUserID:")]
        void SetUserID(string userID);

        // -(void)setUserName:(NSString *)userName;
        [Export("setUserName:")]
        void SetUserName(string userName);

        // -(void)setUserEmail:(NSString *)userEmail;
        [Export("setUserEmail:")]
        void SetUserEmail(string userEmail);

        // -(void)testIdentifier;
        [Export("testIdentifier")]
        void TestIdentifier();
    }

#if !CRASHONLY && !FEEDBACKONLY
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
#endif

#if !CRASHONLY
    // @interface BITFeedbackManager : BITHockeyBaseManager
    [BaseType(typeof(BITHockeyBaseManager))]
    public partial interface BITFeedbackManager
    {
        // @property (readwrite, nonatomic) BITFeedbackUserDataElement requireUserName;
        [Export("requireUserName", ArgumentSemantic.Assign)]
        BITFeedbackUserDataElement RequireUserName { get; set; }

        // @property (readwrite, nonatomic) BITFeedbackUserDataElement requireUserEmail;
        [Export("requireUserEmail", ArgumentSemantic.Assign)]
        BITFeedbackUserDataElement RequireUserEmail { get; set; }

        // @property (readwrite, nonatomic) BOOL showAlertOnIncomingMessages;
        [Export("showAlertOnIncomingMessages")]
        bool ShowAlertOnIncomingMessages { get; set; }

        // -(void)showFeedbackWindow;
        [Export("showFeedbackWindow")]
        void ShowFeedbackWindow();
    }
#endif

#if !FEEDBACKONLY
    // @interface BITCrashDetails : NSObject
    [BaseType(typeof(NSObject))]
    public partial interface BITCrashDetails
    {
        // @property (readonly, copy, nonatomic) NSString * incidentIdentifier;
        [Export("incidentIdentifier")]
        string IncidentIdentifier { get; }

        // @property (readonly, copy, nonatomic) NSString * reporterKey;
        [Export("reporterKey")]
        string ReporterKey { get; }

        // @property (readonly, copy, nonatomic) NSString * signal;
        [Export("signal")]
        string Signal { get; }

        // @property (readonly, copy, nonatomic) NSString * exceptionName;
        [Export("exceptionName")]
        string ExceptionName { get; }

        // @property (readonly, copy, nonatomic) NSString * exceptionReason;
        [Export("exceptionReason")]
        string ExceptionReason { get; }

        // @property (readonly, copy, nonatomic) NSDate * appStartTime;
        [Export("appStartTime", ArgumentSemantic.Copy)]
        NSDate AppStartTime { get; }

        // @property (readonly, copy, nonatomic) NSDate * crashTime;
        [Export("crashTime", ArgumentSemantic.Copy)]
        NSDate CrashTime { get; }

        // @property (readonly, copy, nonatomic) NSString * osVersion;
        [Export("osVersion")]
        string OsVersion { get; }

        // @property (readonly, copy, nonatomic) NSString * osBuild;
        [Export("osBuild")]
        string OsBuild { get; }

        // @property (readonly, copy, nonatomic) NSString * appVersion;
        [Export("appVersion")]
        string AppVersion { get; }

        // @property (readonly, assign, nonatomic) NSUInteger appProcessIdentifier;
        [Export("appProcessIdentifier")]
        nuint AppProcessIdentifier { get; }

        // @property (readonly, copy, nonatomic) NSString * appBuild;
        [Export("appBuild")]
        string AppBuild { get; }
    }
#endif

#if !FEEDBACKONLY
    // @interface BITCrashMetaData : NSObject
    [BaseType(typeof(NSObject))]
    public partial interface BITCrashMetaData
    {
        // @property (copy, nonatomic) NSString * userDescription;
        [Export("userDescription")]
        string UserDescription { get; set; }

        // @property (copy, nonatomic) NSString * userName;
        [Export("userName")]
        string UserName { get; set; }

        // @property (copy, nonatomic) NSString * userEmail;
        [Export("userEmail")]
        string UserEmail { get; set; }

        // @property (copy, nonatomic) NSString * userID;
        [Export("userID")]
        string UserID { get; set; }
    }

    // @interface BITCrashManager : BITHockeyBaseManager
    [BaseType(typeof(BITHockeyBaseManager))]
    public partial interface BITCrashManager
    {
        // @property (getter = isMachExceptionHandlerEnabled, assign, nonatomic) BOOL enableMachExceptionHandler __attribute__((deprecated("Mach Exceptions are now enabled by default. If you want to disable them, please use the new property disableMachExceptionHandler")));
        [Export("enableMachExceptionHandler")]
        bool EnableMachExceptionHandler { [Bind("isMachExceptionHandlerEnabled")] get; set; }

        // @property (getter = isMachExceptionHandlerDisabled, assign, nonatomic) BOOL disableMachExceptionHandler;
        [Export("disableMachExceptionHandler")]
        bool DisableMachExceptionHandler { [Bind("isMachExceptionHandlerDisabled")] get; set; }

        // @property (readonly, nonatomic) BOOL didCrashInLastSession;
        [Export("didCrashInLastSession")]
        bool DidCrashInLastSession { get; }

        // @property (readonly, nonatomic) BITCrashDetails * lastSessionCrashDetails;
        [Export("lastSessionCrashDetails")]
        BITCrashDetails LastSessionCrashDetails { get; }

        // -(void)generateTestCrash;
        [Export("generateTestCrash")]
        void GenerateTestCrash();

        // -(BOOL)handleUserInput:(BITCrashManagerUserInput)userInput withUserProvidedMetaData:(BITCrashMetaData *)userProvidedMetaData;
        [Export("handleUserInput:withUserProvidedMetaData:")]
        bool HandleUserInput(BITCrashManagerUserInput userInput, BITCrashMetaData userProvidedMetaData);

        // @property (readonly, nonatomic) NSTimeInterval timeinterva:lCrashInLastSessionOccured;
        [Export("timeintervalCrashInLastSessionOccured")]
        double TimeintervalCrashInLastSessionOccured { get; }

        // @property (assign, nonatomic) BOOL askUserDetails;
        [Export("askUserDetails")]
        bool AskUserDetails { get; set; }

        // @property (getter = isAutoSubmitCrashReport, assign, nonatomic) BOOL autoSubmitCrashReport;
        [Export("autoSubmitCrashReport")]
        bool AutoSubmitCrashReport { [Bind("isAutoSubmitCrashReport")] get; set; }

        //// -(void)setCrashCallbacks:(BITCrashManagerCallbacks *)callbacks;
        //[Export("setCrashCallbacks:")]
        //unsafe void SetCrashCallbacks(BITCrashManagerCallbacks callbacks);

        // -(void)setCrashReportUIHandler:(BITCustomCrashReportUIHandler)crashReportUIHandler;
        [Export("setCrashReportUIHandler:")]
        void SetCrashReportUIHandler(BITCustomCrashReportUIHandler crashReportUIHandler);
    }
#endif

    // @protocol BITCrashManagerDelegate <NSObject>
    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    public partial interface BITCrashManagerDelegate
    {
#if !FEEDBACKONLY
        // @optional -(void)showMainApplicationWindowForCrashManager:(BITCrashManager *)crashManager __attribute__((deprecated("The default crash report UI is not shown modal any longer, so this delegate is now called right away. We recommend to remove the implementation of this method.")));
        [Export("showMainApplicationWindowForCrashManager:")]
        void ShowMainApplicationWindowForCrashManager(BITCrashManager crashManager);

        // @optional -(NSString *)applicationLogForCrashManager:(BITCrashManager *)crashManager;
        [Export("applicationLogForCrashManager:")]
        string ApplicationLogForCrashManager(BITCrashManager crashManager);

        // @optional -(BITHockeyAttachment *)attachmentForCrashManager:(BITCrashManager *)crashManager;
        [Export("attachmentForCrashManager:")]
        BITHockeyAttachment AttachmentForCrashManager(BITCrashManager crashManager);

        // @optional -(void)crashManagerWillShowSubmitCrashReportAlert:(BITCrashManager *)crashManager;
        [Export("crashManagerWillShowSubmitCrashReportAlert:")]
        void WillShowSubmitCrashReportAlert(BITCrashManager crashManager);

        // @optional -(void)crashManagerWillCancelSendingCrashReport:(BITCrashManager *)crashManager;
        [Export("crashManagerWillCancelSendingCrashReport:")]
        void WillCancelSendingCrashReport(BITCrashManager crashManager);

        // @optional -(void)crashManagerWillSendCrashReport:(BITCrashManager *)crashManager;
        [Export("crashManagerWillSendCrashReport:")]
        void WillSendCrashReport(BITCrashManager crashManager);

        // @optional -(void)crashManager:(BITCrashManager *)crashManager didFailWithError:(NSError *)error;
        [Export("crashManager:didFailWithError:")]
        void DidFailWithError(BITCrashManager crashManager, NSError error);

        // @optional -(void)crashManagerDidFinishSendingCrashReport:(BITCrashManager *)crashManager;
        [Export("crashManagerDidFinishSendingCrashReport:")]
        void DidFinishSendingCrashReport(BITCrashManager crashManager);
#endif
    }

    // @protocol BITHockeyManagerDelegate <NSObject, BITCrashManagerDelegate>
    [BaseType(typeof(NSObject))]
    [Model]
    [Protocol]
    public partial interface BITHockeyManagerDelegate : BITCrashManagerDelegate
    {
        // @optional -(NSString *)userIDForHockeyManager:(BITHockeyManager *)hockeyManager componentManager:(BITHockeyBaseManager *)componentManager;
        [Export("userIDForHockeyManager:componentManager:")]
        string UserIDForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);

        // @optional -(NSString *)userNameForHockeyManager:(BITHockeyManager *)hockeyManager componentManager:(BITHockeyBaseManager *)componentManager;
        [Export("userNameForHockeyManager:componentManager:")]
        string UserNameForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);

        // @optional -(NSString *)userEmailForHockeyManager:(BITHockeyManager *)hockeyManager componentManager:(BITHockeyBaseManager *)componentManager;
        [Export("userEmailForHockeyManager:componentManager:")]
        string UserEmailForHockeyManager(BITHockeyManager hockeyManager, BITHockeyBaseManager componentManager);
    }

    // @interface BITHockeyAttachment : NSObject <NSCoding>
    [BaseType(typeof(NSObject))]
    public partial interface BITHockeyAttachment : INSCoding
    {
        // @property (readonly, copy, nonatomic) NSString * filename;
        [Export("filename")]
        string Filename { get; }

        // @property (readonly, nonatomic, strong) NSData * hockeyAttachmentData;
        [Export("hockeyAttachmentData", ArgumentSemantic.Strong)]
        NSData HockeyAttachmentData { get; }

        // @property (readonly, copy, nonatomic) NSString * contentType;
        [Export("contentType")]
        string ContentType { get; }

        // -(instancetype)initWithFilename:(NSString *)filename hockeyAttachmentData:(NSData *)hockeyAttachmentData contentType:(NSString *)contentType;
        [Export("initWithFilename:hockeyAttachmentData:contentType:")]
        IntPtr Constructor(string filename, NSData hockeyAttachmentData, string contentType);
    }

#if !FEEDBACKONLY
    // @interface BITCrashExceptionApplication : NSApplication
    [BaseType(typeof(NSApplication))]
    public partial interface BITCrashExceptionApplication
    {
    }
#endif

    // @interface BITSystemProfile : NSObject
    [BaseType(typeof(NSObject))]
    public partial interface BITSystemProfile
    {
        // +(BITSystemProfile *)sharedSystemProfile;
        [Static]
        [Export("sharedSystemProfile")]
        BITSystemProfile SharedSystemProfile { get; }

        // +(NSString *)deviceIdentifier;
        [Static]
        [Export("deviceIdentifier")]
        string DeviceIdentifier { get; }

        // +(NSString *)deviceModel;
        [Static]
        [Export("deviceModel")]
        string DeviceModel { get; }

        // +(NSString *)systemVersionString;
        [Static]
        [Export("systemVersionString")]
        string SystemVersionString { get; }

        // -(NSMutableArray *)systemDataForBundle:(NSBundle *)bundle;
        [Export("systemDataForBundle:")]
        NSMutableArray SystemDataForBundle(NSBundle bundle);

        // -(NSMutableArray *)systemData;
        [Export("systemData")]
        NSMutableArray SystemData { get; }

        // -(NSMutableArray *)systemUsageDataForBundle:(NSBundle *)bundle;
        [Export("systemUsageDataForBundle:")]
        NSMutableArray SystemUsageDataForBundle(NSBundle bundle);

        // -(NSMutableArray *)systemUsageData;
        [Export("systemUsageData")]
        NSMutableArray SystemUsageData { get; }

        // -(void)startUsageForBundle:(NSBundle *)bundle;
        [Export("startUsageForBundle:")]
        void StartUsageForBundle(NSBundle bundle);

        // -(void)startUsage;
        [Export("startUsage")]
        void StartUsage();

        // -(void)stopUsage;
        [Export("stopUsage")]
        void StopUsage();
    }

#if !CRASHONLY
    // @interface BITFeedbackWindowController : NSWindowController
    [BaseType(typeof(NSWindowController))]
    public partial interface BITFeedbackWindowController
    {
        // -(id)initWithManager:(BITFeedbackManager *)feedbackManager;
        [Export("initWithManager:")]
        IntPtr Constructor(BITFeedbackManager feedbackManager);
    }
}
#endif