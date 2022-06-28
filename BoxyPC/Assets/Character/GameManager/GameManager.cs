using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    public string Player1 { get; set; }
    public string Player2 { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Player1 = "Player1";
        Player2 = "Player2";
    }

    public GameManager GetInstance()
    {
        return (Instance);
    }

    
}
