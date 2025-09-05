# THE_DARK_HOLLOW

<img width="700" height="600" alt="image" src="https://github.com/user-attachments/assets/629ea0a2-1681-4d1b-8270-0d2f9a0ea22f" />

- # 🕹️ 3D 이벤토리 기능과 상태머신 구현 프로젝트
프로젝트 소개 > 방치형rpg의 자동공격하는 플레이어의 상태머신과 UI인벤토리기능에 아이템과 스탯 데이터를 연결하는 과정을 담은 프로젝트입니다
---
## 📅 개발 기간
- 시작: 2025.09.01  
- 종료: 2025.09.05  

## 🧰 개발 환경
- **Engine**: Unity 2022.3.17f1 (LTS)
- **Language**: C#
- **IDE**: JetBrains Rider / Visual Studio 2022
- **Target**: Windows (PC) *(선택적으로 Android/iOS 확장 가능)*
- **Version Control**: Git + GitHub

---
## 🎀 플로우 차트
- <img width="1590" height="868" alt="image" src="https://github.com/user-attachments/assets/d46abab3-b1c6-4ecc-9de2-ce83c96a4584" />

## 🧩 게임 주요 기능
- 기본 조작키
    | 입력 키 | 동작 |
    | --- | --- |
    | `우측 방향키` | 우측 이동 |
    | `좌측 방향키` | 좌측 이동 |
    | `X` | 기본 공격 |
    | `Z` | 점프 |
    | `SPACE BAR` | 특수 공격 |
    | `SHIFT + 이동 방향키` | 달리기  |
    | `위 방향키` | 상호작용(사다리 타기) |
  
### 1) 플레이어
- 플레이어 스킬공격 (x키)
- <img width="500" height="300" alt="image" src="https://github.com/user-attachments/assets/9893ddd8-7b0a-4899-a3f5-afcfcbb0045f" />
- 공격(z키)를 통해 스킬게이지를 채워서 
- <img width="300" height="120" alt="image" src="https://github.com/user-attachments/assets/fc14861b-d712-4c69-a7a1-11663dbaa597" />
- FSM를 이용한 상태머신으로 상태변경

### 2) 모든 오브젝트 동적생성
- Addressable를 사용하여 게임내 동적생성 오브젝트 관리
- 또한 Scene에 들어가는 모든 오브젝트를 Prefab화 하여 관리

### 3) 보스의 다양한 패턴
- HFSM을 이용한 공격패턴 세부화 : 돌진공격, 중거리공격, 장거리공격
- 중거리공격
- <img width="300" height="200" alt="image" src="https://github.com/user-attachments/assets/b8bff08d-86e7-425f-a5d9-8ed0e471ecc0" />
- 장거리공격
- <img width="100" height="200" alt="image" src="https://github.com/user-attachments/assets/15dd45ba-faac-478b-a801-3158580245c3" />


