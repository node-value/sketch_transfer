using System.Collections.Generic;

public static class GlobalParams {

    public static string baseUrl = "localhost:8080";//sketch-transfer.herokuapp.com

    public static Dictionary<string, object> Map = new() {
        { "authURL", $"http://{baseUrl}/api/auth/authenticate"},
        { "regURL",  $"http://{baseUrl}/api/auth/register"},
        { "chatURL", $"ws://{baseUrl}/ws_chat"},//Not Implemented
        { "projectBaseURL", $"ws://{baseUrl}/ws_project"}
    };
}

