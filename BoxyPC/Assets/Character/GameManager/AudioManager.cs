using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameData gameData;

    private AudioClip legalMove;
    private AudioClip completeBox;
    private AudioClip illegalMove;

    public static AudioManager Instance = null;

    // Update is called once per frame
    void Awake()
    {
        legalMove = gameData.legalMove;
        completeBox = gameData.completeBox;
        illegalMove = gameData.illegalMove;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void SoundLegalMove()
    {
        audioSource.PlayOneShot(legalMove);
    }

    public void SoundCompleteBox()
    {
        audioSource.PlayOneShot(completeBox);
    }

    public void SoundIllegalMove()
    {
        audioSource.PlayOneShot(illegalMove);
    }
}
