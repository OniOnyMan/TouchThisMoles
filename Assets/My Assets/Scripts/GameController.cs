using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float ReadyForPlayingTime = 5;
    public int LowSpawnDelayMilliseconds = 21;
    public int HighSpawnDelayMilliseconds = 3000;
    public float LowMoleDelaySeconds = 3;
    public float HighMoleDelaySeconds = 7;
    public int BasicTimeSeconds = 10;
    public int RewardSecondsMultiplier = 2;
    public string MolesTag = "Mole";
    public string InfoPanelTag = "InfoPanel";

    private bool _canMolesRise = true;
    private float _getReadyTime = 3;
    private int _playTime;
    private MoleController[] _moles;
    private InfoPanelController _infoPanel;
    private GetReadyPanelController _getReadyPanel;
    private int _scores = 0;
    private float _hardFactor = 1;

    private void Start()
    {
        _infoPanel = GameObject.FindGameObjectWithTag(InfoPanelTag).GetComponent<InfoPanelController>();
        _getReadyPanel = _infoPanel.GetReadyPanel;
        _moles = GetControllers(GameObject.FindGameObjectsWithTag(MolesTag));
        _playTime = BasicTimeSeconds;
        _infoPanel.TimerString = GetTimeString(_playTime);
        StartCoroutine(StartGamesTimers());
            StartCoroutine(IncreaseHard());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var hittedGameObject = hit.transform.gameObject;                
                if (hittedGameObject.CompareTag("Mole"))
                {
                    Debug.LogFormat("{0} hitted at {0}", hittedGameObject.name);
                    var mole = hittedGameObject.GetComponent<MoleController>();
                    mole.Declay();
                    AddScore(mole.Price);
                }
            }
        }
        
    }

    private IEnumerator IncreaseHard()
    {
        //while (_canMolesRise)
        //{
            yield return new WaitForSecondsRealtime(Random.Range(5f, 10f));
        //    if (_hardFactor < 75)
        //    {
        //        if (_hardFactor < 3)
        //            _hardFactor *= 1.7f;
        //        else _hardFactor += _hardFactor * Random.Range(2f, 3f) / Random.Range(2f, 5f);
        //        Debug.LogWarning("HardFactor is increased to " + _hardFactor);
        //    }
        //    else {
        //        _hardFactor -= _hardFactor / Random.Range(2f, 7f);
        //        Debug.LogWarning("HardFactor is decreased to " + _hardFactor);
        //    }
        //}
    }

    private string GetTimeString(int time)
    {
        var seconds = time % 60;
        var minutes = time / 60;
        return string.Format("{0}:{1:00}", minutes, seconds);
    }

    private MoleController[] GetControllers(GameObject[] objects)
    {
        var result = new List<MoleController>();
        foreach (var item in objects)
        {
            result.Add(item.GetComponent<MoleController>());
        }
        return result.ToArray();
    }

    private IEnumerator StartGamesTimers() {
        yield return new WaitForSecondsRealtime(ReadyForPlayingTime - _getReadyTime);
        _getReadyPanel.ShowTimer();
        for (var i = _getReadyTime; i > 0; i--)
        {
            _getReadyPanel.TimeString = i.ToString();
            yield return new WaitForSecondsRealtime(1);
        }
        _getReadyPanel.Hide();
        StartCoroutine(PlayTimeCalculate());
        StartCoroutine(RandomizeRise());

    }

    private IEnumerator RandomizeRise()
    {
        while (_canMolesRise)
        {
            //to do: добавить влияние количества полученного времени
            yield return new WaitForSecondsRealtime(Random.Range(LowSpawnDelayMilliseconds / 1000 + _playTime / 45,
                                                                 HighSpawnDelayMilliseconds / 1000 + _playTime / 30));
            var noRoseMoles = _moles.Where(x => x.IsRose == false).ToArray();
            if (noRoseMoles.Length > 0)
            {
                var count = Random.Range(1, noRoseMoles.Length * 2 / 3);
                for (var i = 0; i < count; i++)
                {
                    noRoseMoles[Random.Range(0, noRoseMoles.Length)].Rise(Random.Range(LowMoleDelaySeconds, HighMoleDelaySeconds + 1));
                    yield return new WaitForSecondsRealtime(Random.Range(_hardFactor / 100, _hardFactor / 25));
                    noRoseMoles = _moles.Where(x => x.IsRose == false).ToArray();
                }
            }
            //else Debug.Log("All moles are rose.");
        }
        foreach (var item in _moles.Where(x => x.IsRose == true))
        {
            item.Declay();
        }
    }

    private IEnumerator PlayTimeCalculate()
    {
        for (; _playTime >= 0; _playTime--)
        {
            yield return new WaitForSecondsRealtime(1);
            _infoPanel.TimerString = GetTimeString(_playTime);
        }
        _canMolesRise = false;
    }

    public void AddScore(int price) {
        _scores += price;
        _infoPanel.ScoresString = _scores.ToString();
        _playTime += RewardSecondsMultiplier * price;
        _infoPanel.TimerString = GetTimeString(_playTime);
    }
}
