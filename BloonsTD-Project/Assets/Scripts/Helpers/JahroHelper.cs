using JahroConsole;
using UnityEngine;

public class JahroHelper
{
    [JahroCommand("test-command", "Sends a test command to the debug console")]
    public static void TestCommand()
    {
        Debug.Log("Test command from Jahro Console.");
    }
}
