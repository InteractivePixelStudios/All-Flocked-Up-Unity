using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BugReportManager : MonoBehaviour
{
    private string formUrl = "https://docs.google.com/forms/d/e/1FAIpQLSeFb5I89xpTefneivLOpUftA7WEZb0I_0yG_vEpikG-IERjug/formResponse";

    public void SubmitBug(string bugDescription, string playerPlatform, int rating, string comments)
    {
        StartCoroutine(PostBug(bugDescription, playerPlatform, rating, comments));
    }

    private IEnumerator PostBug(string bugDescription, string playerPlatform, int rating, string comments)
    {
        WWWForm form = new WWWForm();
        form.AddField("entry.1739717606", bugDescription);
        form.AddField("entry.1328795998", playerPlatform);
        form.AddField("entry.1532143408", comments);
        form.AddField("entry.615597952", rating);

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError("Bug submission failed: " + www.error);
            else
                Debug.Log("Bug report submitted successfully!");
        }
    }
}