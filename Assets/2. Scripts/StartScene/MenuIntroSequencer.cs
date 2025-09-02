using System.Collections;
using UnityEngine;

public class MenuIntroSequencer : MonoBehaviour
{
    [Header("Title Objects (in order)")]
    public ObjectGlitchReveal[] titleObjects; // 타이틀 오브젝트들을 순서대로 드래그

    [Header("Buttons Reveal Together")]
    public ObjectGlitchReveal[] buttonObjects; // 버튼 2개 루트 드래그

    [Header("Delays")]
    public float delayBetweenTitles = 0.12f;  // 타이틀 오브젝트 사이 텀
    public float delayBeforeButtons = 0.2f;   // 타이틀 다 끝난 뒤 버튼 나타나기 전 텀

    IEnumerator Start()
    {
        // 시작 시 전부 숨김
        foreach (var t in titleObjects) if (t) t.InstantHide();
        foreach (var b in buttonObjects) if (b) b.InstantHide();

        // 타이틀 오브젝트들을 순차 재생
        foreach (var t in titleObjects)
        {
            if (!t) continue;
            yield return StartCoroutine(t.Co_Reveal());
            yield return new WaitForSeconds(delayBetweenTitles);
        }

        yield return new WaitForSeconds(delayBeforeButtons);

        // 버튼 두개 동시에 재생
        var coros = new Coroutine[buttonObjects.Length];
        for (int i = 0; i < buttonObjects.Length; i++)
            if (buttonObjects[i]) coros[i] = StartCoroutine(buttonObjects[i].Co_Reveal());

        // 모두 끝날 때까지 대기
        foreach (var c in coros) if (c != null) yield return c;
    }
}