using System;
using System.Runtime.InteropServices;
using ObjCRuntime;

namespace HockeyApp.Mac
{
    // typedef void (*BITCrashManagerPostCrashSignalCallback)(void *context);
    public delegate void BITCrashManagerPostCrashSignalCallback(IntPtr context);

    [StructLayout(LayoutKind.Sequential)]
    public struct BITCrashManagerCallbacks
    {
        // public void* context;
        public IntPtr context;
        // public BITCrashManagerPostCrashSignalCallback* handleSignal;
        public BITCrashManagerPostCrashSignalCallback handleSignal;
    }

    [Native]
    public enum BITLogLevel : ulong
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Debug = 3,
        Verbose = 4
    }

#if !FEEDBACKONLY
    [Native]
    public enum BITCrashManagerUserInput : ulong
    {
        DontSend = 0,
        Send = 1,
        AlwaysSend = 2
    }
#endif

#if !CRASHONLY
    [Native]
    public enum BITFeedbackUserDataElement : long
    {
        DontShow = 0,
        Optional = 1,
        Required = 2
    }
#endif

#if !FEEDBACKONLY
    [Native]
    public enum BITCrashErrorReason : long
    {
        ErrorUnknown,
        APIAppVersionRejected,
        APIReceivedEmptyResponse,
        APIErrorWithStatusCode
    }
#endif

#if !CRASHONLY
    [Native]
    public enum BITFeedbackErrorReason : long
    {
        ErrorUnknown,
        APIServerReturnedInvalidStatus,
        APIServerReturnedInvalidData,
        APIServerReturnedEmptyResponse,
        APIClientAuthorizationMissingSecret,
        APIClientCannotCreateConnection
    }
#endif

    [Native]
    public enum BITHockeyErrorReason : long
    {
        BITHockeyErrorUnknown
    }
}
