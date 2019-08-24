using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetReadyPanelController : MonoBehaviour
{
    public Text TimeText;

    public string TimeString
    {
        get { return TimeText.text; }
        set { TimeText.text = value; }
    }

    public void ShowTimer()
    {
        TimeText.gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
