using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour
{
    public Text TimerText;
    public Text ScoresText;
    public GetReadyPanelController GetReadyPanel;

    public string TimerString
    {
        get { return TimerText.text; }
        set { TimerText.text = value; }
    }

    public string ScoresString
    {
        get { return ScoresText.text; }
        set { ScoresText.text = value; }
    }
}
