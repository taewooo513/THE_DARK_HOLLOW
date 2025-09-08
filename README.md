# THE_DARK_HOLLOW

<img width="700" height="600" alt="image" src="https://github.com/user-attachments/assets/629ea0a2-1681-4d1b-8270-0d2f9a0ea22f" />

## 🎮 게임 소개
**『The Dark Hollow』**는 어둠에 숨어든 악마를 쓰러뜨리는 2D 플랫폼 액션 게임입니다.


## 🖼️ 게임 화면
<p>게임 플레이 화면입니다.</p>
<img src="https://github.com/user-attachments/assets/05026fd2-ae08-45b9-9d16-79ac197f4749" width="600"/>
<img src="https://github.com/user-attachments/assets/bfc54d08-9ddd-4073-8da8-5d27be39827b" width="600"/>
<img src="https://github.com/user-attachments/assets/994c7aff-935a-4815-aa35-cb7d5e57ba56" width="600"/>


## 🕹️ 플레이 방법

(게임 흐름)
- 타이틀 씬에서 게임 시작 버튼을 눌러서 게임을 시작합니다.
- 이동과 점프, 공격 키를 입력해서 몬스터를 공격합니다. 

<br/>

(조작법)
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

<br/>

(특수 공격)
- 스킬 게이지를 5칸 채우면 특수 공격(space bar 키 입력)을 할 수 있습니다.<br/><br/>
<img width="300" height="120" alt="image" src="https://github.com/user-attachments/assets/fc14861b-d712-4c69-a7a1-11663dbaa597" /><br/>
<img width="500" height="300" alt="image" src="https://github.com/user-attachments/assets/9893ddd8-7b0a-4899-a3f5-afcfcbb0045f" />

<br/><br/>
(게임 오버)
- 체력이 0이 되면 메인씬으로 넘어가 게임을 다시 시작합니다.


## 📅 개발 기간
- 시작: 2025.09.01  
- 종료: 2025.09.05  

## 👤 개발자 
<p>팀장 : 정세윤 - 플레이어 구현(움직임, 상태 패턴, 애니메이션 구현) </p>
<p>팀원 : 이영신 - 보스 구현(움직임, 상태 패턴, 애니메이션 구현)</p>
<p>팀원 : 고태웅 - 씬, 오브젝트, 카메라, 적, 맵, 리소스, 사운드 매니저 구현</p>
<p>팀원 : 차광호 - UI 개발(시작 화면, 옵션, 키 설정, 인게임)</p>
<p>팀원 : 김예성 - 시스템 기획(플레이어), UI 기획(각종 화면 디자인)</p>
<p>팀원 : 박철원 - 콘텐츠 기획(레벨 디자인 및 일반, 보스 몬스터 구현)</p>


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

### 1) 플레이어 상태 관리, 애니메이션 
- FSM과 상태패턴으로 플레이어의 상태를 관리. 
- 추상 클래스(BaseState)로 모든 상태들이 가져야 할 기능들을 정의, 구체적인 클래스(Idle/Move/Run/Jump/Hit State)에서 추상 클래스를 정의, 상태 전환 조건에 따라 상태를 전환
- 모든 상태들을 상태 머신(StateMachine) 클래스로 관리 
- 플레이어의 모든 애니메이션을 AnimationController에서 관리

### 2) 모든 오브젝트 동적생성
- Addressable를 사용하여 게임내 동적생성 오브젝트 관리
- 또한 Scene에 들어가는 모든 오브젝트를 Prefab화 하여 관리

### 3) 보스의 다양한 패턴
- HFSM을 이용한 공격패턴 세부화 : 돌진공격, 중거리공격, 장거리공격
- 중거리공격<br><img width="300" height="200" alt="image" src="https://github.com/user-attachments/assets/b8bff08d-86e7-425f-a5d9-8ed0e471ecc0" />
- 장거리공격<br><img width="100" height="200" alt="image" src="https://github.com/user-attachments/assets/15dd45ba-faac-478b-a801-3158580245c3" />

### 4) UI
- Audio Mixer로 BGM과 SFX를 분리
- 옵션 클릭시 일시정지 처리
- PlayerInput과 드롭다운 연동으로 키 가이드를 간단히 재매핑
- 특수 게이지 테두리 변화로 사용 가능 상태를 즉시 인지하도록 설계

## 🧠트러블 슈팅  
<h2><b>문제1</b></h2>
<p>플레이어가 점프 후 착지하면 이동하지 않거나, 달리는 도중 달리기 키를 떼면 플레이어가 멈추거나, 점프 중 움직일 수 없는 현상이 발생</p>

<p><b>원인</b></p>
<p>A상태에서 B상태로 이어지는 전환점을 작성하지 않아서 같은 상태가 지속됨</p>

<p><b>해결</b></p>
<p>로그를 찍어보면서 어느 시점에 다른 상태로 전환해야 하는지를 찾아서 상태 전환 코드를 수정</p>

<br>

<h2><b>문제2</b></h2>
<p>보스패턴중 공격패턴만 3가지이상이 되니 쿨타임을 무시하고 다른 패턴이 엉키는 현상 발생</p>

<p><b>원인</b></p>
<p>기존의 가우시안 가중치 계수에 따라 쿨타임이 무시되고 패턴이 돌아가고 패턴이 동시에 일어나는 엉킴현상이 생김</p>

<p><b>해결</b></p>
<p>패턴 꼬임 현상으로 이전 가우시안식 가중치 패턴은 제거하고 범위내 있는 패턴중 하나를 발동하고 패턴실행중 다른 패턴이 발동 안되게 패턴사이에 쿨타임을 적용</p>

<br>

<h2><b>문제3</b></h2>
<p>시네머신 사용하면서 직접 카메라 쉐이킹, 카메라 러프, 줌인아웃을 만들었는데 작동하지 않는 현상 발생</p>

<p><b>원인</b></p>
<p>시네머신 내부에 이미 구현된 기능 때문에 작동하지 않았음</p>

<p><b>해결</b></p>
<p>Microsft Docs 찾아보며 기능 찾아서 해결</p>

<br>

<h2><b>문제4</b></h2>
<p>UI의 텍스트가 뭉개지는 문제가 발생</p>

<p><b>원인</b></p>
<p>레거시 텍스트를 사용함에 따라 해상도에 영향을 받아 텍스트가 뭉개진 것</p>

<p><b>해결</b></p>
<p>SDF를 기반으로 하는 TMP 텍스트를 활용함으로써 뭉개지지 않게 함.</p>


## 😊 프로젝트 회고
<p>김예성 - 저희 조가 기획한 기획서가 게임으로 바뀌는 과정에서 너무 즐거웠습니다.</p>
<p>박철원 - 개발 팀과의 협업이라는 것이 무엇인지 확실히 알게 되었습니다.</p>
<p>정세윤 - 플레이어를 만들면서 상태패턴을 익히는 과정이 재밌었습니다.</p>
<p>고태웅 - 매니저를 만들면서 만들어보고 싶었던 어드레서블 등 만들어서 재밌었습니다.</p>
<p>이영신 - 기획이 있으니 개발 반향성이 확고해저서 개발업무가 쾌적했습니다.</p>
<p>차광호 - 많은 경험을 쌓고, 팀워크에 대해 배우는 뜻깊은 시간이었습니다.</p>
