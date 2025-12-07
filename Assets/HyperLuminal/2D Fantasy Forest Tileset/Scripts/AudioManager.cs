using UnityEngine;
using UnityEngine.SceneManagement; // 씬 바뀐 거 감지하는 도구

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("음악 틀어줄 스피커")]
    public AudioSource bgmPlayer;

    [Header("음악 파일 넣는 곳")]
    public AudioClip battleBGM;   // 전투 음악 (유튜브1)
    public AudioClip nightBGM;    // 밤 음악 (유튜브2)
    public AudioClip villageBGM;  // 낮(마을) 음악 (없으면 비워두세요)

    void Awake()
    {
        // 게임 내내 살아있게 만들기 (싱글톤)
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        // 씬이 바뀔 때마다 "OnSceneLoaded" 함수를 실행하라고 등록!
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 씬 이동할 때마다 자동으로 실행되는 함수
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "next") // 1. 전투 씬이면?
        {
            PlayMusic(battleBGM);
        }
        else if (scene.name == "NightTime_DemoScene") // 2. 밤 씬이면?
        {
            PlayMusic(nightBGM);
        }
        else if (scene.name == "DayTime_DemoScene") // 3. 낮 씬이면?
        {
            PlayMusic(villageBGM);
        }
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip == null) return; // 음악 파일이 없으면 재생 안 함
        if (bgmPlayer.clip == clip) return; // 이미 똑같은 노래가 나오고 있으면 안 바꿈

        bgmPlayer.clip = clip;
        bgmPlayer.Play();
    }
}