using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FlurrySDK;

public class FlurryManager : Singleton<FlurryManager>
{
#if UNITY_ANDROID
    private string FLURRY_API_KEY = "SF42FM2JQJ6Q2RRVS3FD";
#elif UNITY_IPHONE
    private string FLURRY_API_KEY = "4TD6Y4DGKD5DQXNZX7CQ";
#else
    private string FLURRY_API_KEY = null;
#endif

    public void Init()
    {
        // Initialize Flurry.
        new Flurry.Builder()
                  .WithCrashReporting(true)
                  .WithLogEnabled(true)
                  .WithLogLevel(Flurry.LogLevel.VERBOSE)
                  .WithMessaging(true)
                  .Build(FLURRY_API_KEY);
    }
}
