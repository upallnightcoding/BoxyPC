using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private Slider boardSize;
    [SerializeField] private TextMeshProUGUI boardSizeText;
    [SerializeField] private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        boardSize.onValueChanged.AddListener((value) =>
        {
            boardSizeText.text = value.ToString();

            gameData.boardSize = (int) value;
        });
    }
  
}
