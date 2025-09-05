using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDeadWatcher : MonoBehaviour
{
    public string StartScene;

    public static BossDeadWatcher I { get; private set; }
    public bool IsBossCleared { get; private set; } = false;

    // 영구 저장 키
    private const string PPKey = "BossCleared";

    //BossController cachedBoss;
    float pollAcc = 0f;
    const float pollHz = 1f; // 초당 폴링 (필요시 조절)

    void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

        // 영구 저장 불러오기
        IsBossCleared = PlayerPrefs.GetInt(PPKey, 0) == 1;
    }

    void Update()
    {
        if (IsBossCleared) return; // 이미 클리어면 더 볼 일 없음

        pollAcc += Time.deltaTime;
        if (pollAcc >= 1f / pollHz)
        {
            pollAcc = 0f;

            // TODO: 보스 죽음 처리 수정(수정자 : 이영신)
            /* // 보스를 못 찾았으면 찾아두고
             if (cachedBoss == null)
                 cachedBoss = FindObjectOfType<BossController>();*/

            // 보스를 찾았고, 죽은 상태라면 클리어 처리
            if (CharacterManager.instance.Boss.controller != null && CharacterManager.instance.Boss.controller.IsDead)
            {
                ReportBossDead();
            }
        }
    }

   // 보스가 죽었음을 외부에서 직접 통보할 때 호출
    public void ReportBossDead()
    {
        if (IsBossCleared) return;
        IsBossCleared = true;

        // 영구 저장
        PlayerPrefs.SetInt(PPKey, 1);
        PlayerPrefs.Save();
        Debug.Log("[BossDeadWatcher] Boss Cleared 기록됨");
    }

    // 새 게임 등에서 초기화할 때 호출
    public void ResetClear()
    {
        IsBossCleared = false;
        PlayerPrefs.DeleteKey(PPKey);
        Debug.Log("[BossDeadWatcher] Boss Cleared 초기화");
        SceneManager.LoadScene(StartScene);
    }
}
