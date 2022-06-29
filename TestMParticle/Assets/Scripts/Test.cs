using mParticle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    private string _apiKey;
    private string _apiSecret;

    void Awake()
    {
        DoInitMParticle();
    }

    void Start()
    {
        StartCoroutine(DoBeginTest());
    }

    private void DoInitMParticle()
    {
        //use the correct workspace API key and secret for iOS and Android
#if UNITY_ANDROID
        _apiKey = "us2-5006b3c8b0278347a20c1f9c3c07a676";
        _apiSecret = "GPxyuowJPyoBa0-Lrn8VlZhy_y6slNJEKtXl_9ULKXSoGQKfQCL0FJuHnk-Oc0OB";
#elif UNITY_IOS
        _apiKey = "us2-c0660468e8473b41aa853abd19c8a132";
		_apiSecret = "DrNvE-kzJBv9k5wif0m_z4HKaLqCEJRklGDi2vpG9pdsa7LM191s8AeZb_rNH1N5";   
#endif
        Debug.Log("DoInitMParticle");
        Debug.Log($"_apiKey > {_apiKey}");
        Debug.Log($"_apiSecret > {_apiSecret}");

        MParticle.Instance.Initialize(new MParticleOptions()
        {
            ApiKey = _apiKey,
            ApiSecret = _apiSecret,
            Environment = mParticle.Environment.Development,
            UploadInterval = 1,
            LogLevel = LogLevel.VERBOSE
        });


    }

    private IEnumerator DoBeginTest()
    {
        yield return new WaitForSeconds(1f);
        DoTestLogScreen("Splash");
        DoTestNavigation(1);
        yield return new WaitForSeconds(1.5f);
        DoTestNavigation(2);
        yield return new WaitForSeconds(1.8f);
        DoTestNavigation(3);
        yield return new WaitForSeconds(3f);
        DoTestNavigation(4);
        yield return new WaitForSeconds(0.3f);
        DoTestNavigation(5);
        yield return new WaitForSeconds(1f);
        DoTestNavigation(6);

        DoTestLogScreen("Download Resources");

        yield return new WaitForSeconds(1f);
        DoTestLocation(1);
        yield return new WaitForSeconds(1f);
        DoTestLocation(2);
        yield return new WaitForSeconds(1f);
        DoTestLocation(3);
        yield return new WaitForSeconds(1f);
        DoTestLocation(4);
        yield return new WaitForSeconds(1f);
        DoTestLocation(5);
        yield return new WaitForSeconds(1f);
        DoTestLocation(6);

        DoTestLogScreen("Login");

        yield return new WaitForSeconds(1f);
        DoTestSearch(1);
        yield return new WaitForSeconds(1f);
        DoTestSearch(2);
        yield return new WaitForSeconds(1f);
        DoTestSearch(3);
        yield return new WaitForSeconds(1f);
        DoTestSearch(4);
        yield return new WaitForSeconds(1f);
        DoTestSearch(5);
        yield return new WaitForSeconds(1f);
        DoTestSearch(6);

        DoTestLogScreen("Lobby");

        yield return new WaitForSeconds(1f);
        DoTestTransaction(1);
        yield return new WaitForSeconds(1f);
        DoTestTransaction(2);
        yield return new WaitForSeconds(1f);
        DoTestTransaction(3);
        yield return new WaitForSeconds(1f);
        DoTestTransaction(4);
        yield return new WaitForSeconds(1f);
        DoTestTransaction(5);
        yield return new WaitForSeconds(1f);
        DoTestTransaction(6);

        DoTestLogScreen("PartyRoom");

        yield return new WaitForSeconds(1f);
        DoTestUserContent(1);
        yield return new WaitForSeconds(1f);
        DoTestUserContent(2);
        yield return new WaitForSeconds(1f);
        DoTestUserContent(3);
        yield return new WaitForSeconds(1f);
        DoTestUserContent(4);
        yield return new WaitForSeconds(1f);
        DoTestUserContent(5);
        yield return new WaitForSeconds(1f);
        DoTestUserContent(6);

        DoTestLogScreen("ChatParty");

        yield return new WaitForSeconds(1f);
        DoTestUserPreference(1);
        yield return new WaitForSeconds(1f);
        DoTestUserPreference(2);
        yield return new WaitForSeconds(1f);
        DoTestUserPreference(3);
        yield return new WaitForSeconds(1f);
        DoTestUserPreference(4);
        yield return new WaitForSeconds(1f);
        DoTestUserPreference(5);
        yield return new WaitForSeconds(1f);
        DoTestUserPreference(6);

        DoTestLogScreen("StreamingParty");

        yield return new WaitForSeconds(1f);
        DoTestSocial(1);
        yield return new WaitForSeconds(1f);
        DoTestSocial(2);
        yield return new WaitForSeconds(1f);
        DoTestSocial(3);
        yield return new WaitForSeconds(1f);
        DoTestSocial(4);
        yield return new WaitForSeconds(1f);
        DoTestSocial(5);
        yield return new WaitForSeconds(1f);
        DoTestSocial(6);

        DoTestLogScreen("Lobby");

        yield return new WaitForSeconds(1f);
        DoTestOther(1);
        yield return new WaitForSeconds(1f);
        DoTestOther(2);
        yield return new WaitForSeconds(1f);
        DoTestOther(3);
        yield return new WaitForSeconds(1f);
        DoTestOther(4);
        yield return new WaitForSeconds(1f);
        DoTestOther(5);
        yield return new WaitForSeconds(1f);
        DoTestOther(6);
    }

    private void DoTestNavigation(int myValue)
    {        
        MParticle.Instance.LogEvent(new MPEvent($"Navigation {myValue:D3}", mParticle.EventType.Navigation)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }                   
                }
            }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestLocation(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"Location {myValue:D3}", mParticle.EventType.Location)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestSearch(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"Search {myValue:D3}", mParticle.EventType.Search)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestTransaction(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"Transaction {myValue:D3}", mParticle.EventType.Transaction)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestUserContent(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"UserContent {myValue:D3}", mParticle.EventType.UserContent)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestUserPreference(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"UserPreference {myValue:D3}", mParticle.EventType.UserPreference)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestSocial(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"Social {myValue:D3}", mParticle.EventType.Social)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }

    private void DoTestOther(int myValue)
    {
        MParticle.Instance.LogEvent(new MPEvent($"Other {myValue:D3}", mParticle.EventType.Other)
        {
            Info = new Dictionary<string, string> { { SystemInfo.deviceUniqueIdentifier, DateTime.Now.ToString() } },
            CustomFlags = new Dictionary<string, List<string>> {
                    { "custom flag string", new List<string> () { $"Value > {myValue}", "one", "two", "five" } }
                }
        }
        );

        MParticle.Instance.Upload();

    }


    private void DoTestLogScreen(string myScreenName)
    {
        Debug.Log("------ Do Test Event 2 ------");

        MParticle.Instance.LogScreen(myScreenName);
    }
}
