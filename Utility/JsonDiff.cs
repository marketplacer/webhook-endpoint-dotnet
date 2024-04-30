using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;


public static class JsonDiff
{
    public static JToken ReturnTheDifference(dynamic receivedEvent, dynamic lastEvent)
    {
        var jdp = new JsonDiffPatch();
        JToken diff = jdp.Diff(receivedEvent, lastEvent);
        
        return diff!;
    }
}