using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBoardPanel : MonoBehaviour
{
    private Transform scoreBoard;
    private Transform scoreBoardEntry;

    private void Awake()
    {
        scoreBoard = transform.Find("ScoreBoard");
        scoreBoardEntry = scoreBoard.Find("ScoreBoardEntry");
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreBoardEntry.gameObject.SetActive(false);

        float height = 20.0f;

        for (int i = 1; i <= 10; i++)
        {
            Transform entry = Instantiate(scoreBoardEntry, scoreBoard.transform);
            RectTransform rectTransform = entry.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, -height * i);
            entry.gameObject.SetActive(true);

            entry.Find("Rank").GetComponent<TextMeshProUGUI>().text = i.ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
