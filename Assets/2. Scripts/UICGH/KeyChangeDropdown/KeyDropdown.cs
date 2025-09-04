using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class KeyDropdown : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown dropdown;

    [Header("Target Binding")]
    [Tooltip("바꿀 대상 액션 (예: Jump, Attack, Move)")]
    [SerializeField] private InputActionReference actionRef;

    [Tooltip("단일 바인딩이면 0. Composite라면 해당 파트의 bindingIndex(예: left/right/up/down)")]
    [SerializeField] private int bindingIndex = 0;

    [Header("저장 키")]
    [SerializeField] private string playerPrefsKey = "InputRebinds";

    // 드롭다운에 보여줄 '허용 키' 목록 (원하면 더 추가)
    private static readonly Key[] AllowedKeys =
    {
        // 방향키
        Key.UpArrow, Key.DownArrow, Key.LeftArrow, Key.RightArrow,
        // 알파벳
        Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H, Key.I, Key.J,
        Key.K, Key.L, Key.M, Key.N, Key.O, Key.P, Key.Q, Key.R, Key.S, Key.T,
        Key.U, Key.V, Key.W, Key.X, Key.Y, Key.Z,
        // 숫자키 (상단)
        Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
        Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9,
        // 공통 특수
        Key.Space, Key.LeftShift, Key.RightShift,
        Key.LeftCtrl, Key.RightCtrl,
        Key.LeftAlt, Key.RightAlt
    };

    // 드롭다운 옵션: 표시 문자열 ↔ 바인딩 path 매핑
    private readonly List<(string display, string path)> _options = new();

    private InputAction _action => actionRef != null ? actionRef.action : null;

    private void Awake()
    {
        if (dropdown == null) dropdown = GetComponent<TMP_Dropdown>();

        // 저장된 바인딩 불러오기(프로젝트 전체에서 한 번만 호출해도 됨)
        LoadOverrides();

        BuildOptions();
        ReflectCurrentBindingToDropdown();
        dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownChanged);
        SaveOverrides();
    }

    private void BuildOptions()
    {
        dropdown.ClearOptions();
        _options.Clear();

        foreach (var key in AllowedKeys)
        {
            string controlPath = KeyToPath(key); // "<Keyboard>/space" 같은 형식
            string label = Humanize(controlPath); // "Space", "Left Shift" 등
            _options.Add((label, controlPath));
        }

        dropdown.AddOptions(_options.Select(o => o.display).ToList());
    }

    private void ReflectCurrentBindingToDropdown()
    {
        if (_action == null) return;
        var bindings = _action.bindings;

        if (bindingIndex < 0 || bindingIndex >= bindings.Count)
        {
            Debug.LogWarning($"Binding index out of range: {bindingIndex} on action {_action.name}");
            return;
        }

        // overridePath가 있으면 우선, 없으면 default path 사용
        var path = string.IsNullOrEmpty(bindings[bindingIndex].overridePath)
            ? bindings[bindingIndex].effectivePath
            : bindings[bindingIndex].overridePath;

        // 키보드 바인딩만 대상으로 함
        int idx = IndexOfPath(path);
        dropdown.SetValueWithoutNotify(Mathf.Max(0, idx));
    }

    private void OnDropdownChanged(int optionIndex)
    {
        if (_action == null) return;
        if (optionIndex < 0 || optionIndex >= _options.Count) return;

        string newPath = _options[optionIndex].path;

        // 충돌 자동해제(선택): 같은 키를 쓰던 다른 바인딩의 override를 비움
        AutoUnbindConflicts(newPath);

        // 대상 바인딩에 override 적용
        var bindings = _action.bindings;
        if (bindingIndex < 0 || bindingIndex >= bindings.Count) return;

        _action.ApplyBindingOverride(bindingIndex, new InputBinding { overridePath = newPath });

        // 적용 즉시 저장(원하면 장면 전환 시점에 한 번만 저장해도 됨)
        SaveOverrides();
    }

    private void AutoUnbindConflicts(string newPath)
    {
        if (_action == null) return;

        var asset = _action.actionMap?.asset;
        if (asset == null) return;

        foreach (var map in asset.actionMaps)
        {
            foreach (var act in map.actions)
            {
                for (int i = 0; i < act.bindings.Count; i++)
                {
                    var b = act.bindings[i];
                    var effective = string.IsNullOrEmpty(b.overridePath) ? b.effectivePath : b.overridePath;
                    if (string.Equals(effective, newPath, StringComparison.OrdinalIgnoreCase))
                    {
                        // 현재 우리가 바꾸는 그 바인딩이면 스킵
                        if (act == _action && i == bindingIndex) continue;

                        act.RemoveBindingOverride(i); // 충돌 해제
                    }
                }
            }
        }
    }

    private void SaveOverrides()
    {
        var asset = _action?.actionMap?.asset;
        if (asset == null) return;

        string json = asset.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(playerPrefsKey, json);
        PlayerPrefs.Save();
    }

    private void LoadOverrides()
    {
        var asset = _action?.actionMap?.asset;
        if (asset == null) return;

        if (PlayerPrefs.HasKey(playerPrefsKey))
        {
            string json = PlayerPrefs.GetString(playerPrefsKey);
            asset.LoadBindingOverridesFromJson(json);
        }
    }

    private static string KeyToPath(Key key)
    {
        // Key.X -> "<Keyboard>/x", Key.Space -> "<Keyboard>/space"
        string name = key.ToString(); // e.g., LeftShift, Digit1
        // 숫자키 처리
        if (name.StartsWith("Digit"))
            return $"<Keyboard>/{name.Substring("Digit".Length)}";

        // 일반 키
        return $"<Keyboard>/{ToSnakeCase(name)}";
    }

    private static int IndexOfPath(string path)
    {
        // 드롭다운 옵션 중 path가 같은 항목의 인덱스
        for (int i = 0; i < AllowedKeys.Length; i++)
        {
            if (string.Equals(KeyToPath(AllowedKeys[i]), path, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return 0; // 못 찾으면 첫 옵션
    }

    private static string Humanize(string controlPath)
    {
        // "LeftShift" -> "Left Shift" 등 사람친화 표기
        return InputControlPath.ToHumanReadableString(controlPath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private static string ToSnakeCase(string keyName)
    {
        // Key enum 이름을 InputSystem 경로에 맞게 변환
        // LeftShift -> "leftShift"가 아니라 "leftShift" 대신 실제 경로는 "leftShift"가 아닌 "leftShift"?!
        // 안전하게 모두 lower + 특수 처리.
        // InputSystem은 대부분 lower-case로 매칭됨: "space", "leftShift", "rightShift" 등
        // 간단화를 위해 아래 매핑 일부 보정
        switch (keyName)
        {
            case "LeftShift": return "leftShift";
            case "RightShift": return "rightShift";
            case "LeftCtrl": return "leftCtrl";
            case "RightCtrl": return "rightCtrl";
            case "LeftAlt": return "leftAlt";
            case "RightAlt": return "rightAlt";
            case "UpArrow": return "upArrow";
            case "DownArrow": return "downArrow";
            case "LeftArrow": return "leftArrow";
            case "RightArrow": return "rightArrow";
            case "Space": return "space";
        }
        return keyName.ToLower(); // A->"a", X->"x" 등
    }
}
