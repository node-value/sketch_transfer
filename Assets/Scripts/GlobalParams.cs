using System.Collections.Generic;

public static class GlobalParams {
    public static Dictionary<string, object> Map = new() {
        { "authURL", "http://localhost:8080/api/auth/authenticate"},
        { "regURL",  "http://localhost:8080/api/auth/register"},
        { "chatURL", "ws://localhost:8080/ws_chat"},
        { "projectURL", "ws://localhost:8080/ws_project"}
    };
}

