using System.Collections.Generic;
using System.Diagnostics;

public static class GlobalParams {
    internal enum ServerMode { REMOTE, LOCAL }

    public static string baseUrl = GetBaseUrl();

    public static Dictionary<string, object> Map = new() {
        { "authURL",      $"http://{baseUrl}/api/auth/authenticate"},
        { "regURL",       $"http://{baseUrl}/api/auth/register"},
        { "chatURL",        $"ws://{baseUrl}/ws_chat"},//Not Implemented
        { "projectBaseURL", $"ws://{baseUrl}/ws_project"}
    };

    private static ServerMode GetMode() {
        string[] args = System.Environment.GetCommandLineArgs();
        UnityEngine.Debug.Log(args[0]);
        return args.Length == 2 && args[0].Equals("--mode") && args[1].ToUpper().Equals("LOCAL") ? 
            ServerMode.LOCAL : ServerMode.REMOTE;
    }
    private static string GetBaseUrl() {
        string local  = "localhost:8080";
        string remote = "sketch-transfer.herokuapp.com";
        string result = GetMode() == ServerMode.LOCAL ? local : remote;
        UnityEngine.Debug.Log(result);
        return result;
    }
}

