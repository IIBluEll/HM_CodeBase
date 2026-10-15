# Default MVP CodeBase

경량 MVP 아키텍처 + 유틸리티 코드
Version : 1.0.0
---

## 목차
1. [구성 요소](#구성-요소)  
 1.1 [AView (View 베이스)](#1-aview-view-베이스)  
 1.2 [APresenter (Presenter 베이스)](#2-apresenter-presenter-베이스)  
 1.3 [EventProvider (이벤트 버스)](#3-eventprovider-이벤트-버스)  
 1.4 [SessionProvider + ASession (세션 컨테이너)](#4-sessionprovider--asession-세션-컨테이너)  
 1.5 [ASingleTone&lt;T&gt; (제네릭 싱글톤)](#5-asingletonet-제네릭-싱글톤)  
 1.6 [Util (공통 유틸리티)](#6-util-공통-유틸리티)  
2. [설치 및 사용](#설치-및-사용)  
3. [예시 코드](#예시-코드)  
 3.1 [View 예시](#1-view)  
 3.2 [Presenter 예시](#2-presenter)  

---

## 구성 요소

### 1. AView (View 베이스)
- `Open()`, `Close()`, `Clear()` 기본 제공.  
- `gameObject.SetActive(true/false)`로 표시 상태를 관리.  
- 모든 UI View는 이를 상속해야함!

### 2. APresenter (Presenter 베이스)
- `Open()`, `Close()`, `Dispose()` 추상 메서드로 수명주기를 정의  
- 모든 Presenter는 이를 상속해야함!

### 3. EventProvider (이벤트 버스)
- `Presenter` 간 이벤트를 전달하기 위한 제네릭 기반 이벤트 메세지 시스템.
- `Subscribe`, `Unsubscribe`, `Publish` 세 메서드로 구성됨.
- UI나 로직 간 상태 변화를 안전하게 주고받는 용도로 사용.
- `Publish`는 발행 시작 시점의 구독자 목록을 사용합니다. 콜백 안의 구독 추가·해제는 다음 발행부터 반영됩니다.
- 현재 발행 중 해제한 구독자도 해당 발행에서는 호출될 수 있습니다. 발행마다 목록 복사에 따른 배열 할당이 발생합니다.

### 4. SessionProvider + ASession (세션 컨테이너)
- `Enum` 키로 세션을 저장/조회/삭제.  
- `CreateSession()`, `GetSession()`, `DeleteSession()` 제공.
- 화면 전환 간 데이터를 일시적으로 유지하기 위해서 사용.

### 5. ASingleTone<T> (제네릭 싱글톤)
- 씬 내 인스턴스 탐색 후 없으면 자동 생성.  
- 중복 인스턴스 탐지 시 경고 후 제거.
- 실제 타입명은 `ASingletone<T>`이며, `Awake()`에서 인스턴스를 등록합니다. 재정의할 때는 `base.Awake()`를 호출해야 합니다.
- `Instance`는 기존 인스턴스를 재사용하며, 최초 탐색에는 비활성 GameObject도 포함합니다. 중복 제거 대상은 컴포넌트입니다.

### 6. Util (공통 유틸리티)
- IPv4 주소 조회: `GetMyIPAddress()`  
- 문자열 → `Vector3`: `VectorParse()`  
- HEX → `Color`: `HaxToColor()`  
- JSON 직렬화/역직렬화: `ParseJson<T>()`, `TryWriteJson()`
- JSON 읽기·쓰기는 모두 Newtonsoft.Json을 사용합니다. 일반 C# 데이터의 필드·프로퍼티·Dictionary를 대상으로 합니다.
- `TryParseJson<T>(json, out result)`는 변환 성공 여부를 반환합니다. 참조 타입에 대한 JSON `null`은 정상 결과입니다.
- 기존 `ParseJson<T>()`는 유지하며 실패 시 기본값을 반환합니다. 실패와 정상 기본값을 구분하려면 `TryParseJson<T>()`를 사용합니다.
- `TryWriteJson()`은 저장 성공 시 `true`, 실패 시 `false`를 반환합니다. 기존 일반 호출은 유지할 수 있지만 `Action<string, object>`에 직접 바인딩한 코드는 변경해야 합니다.
- 기존 JsonUtility 저장 데이터는 실제 모델로 호환성을 확인해야 합니다. 특히 `[SerializeField]` private 필드와 Unity 전용 타입은 Newtonsoft의 기본 규칙과 다르므로 별도 설정·변환이 필요할 수 있습니다.

---

## 설치 및 사용

1. 패키지 임포트  
2. 씬에 필요한 View 프리팹 및 Presenter 클래스 추가  
3. 이벤트나 세션 기능이 필요하면 `EventProvider`, `SessionProvider` 접근

---

## 예시 코드

### 1. View

```csharp
using HM.CodeBase;
using UnityEngine;

public sealed class Sample_View : AView
{
    public override void Open()
    {
        base.Open();
        // UI 초기화
    }

    public override void Close()
    {
        // 정리 로직
        base.Close();
    }
}
```

### 2. Presenter
```csharp
using HM.CodeBase;

public sealed class SamplePresenter_presenter : APresenter
{
    private readonly Sample_View _view;
    private readonly SampleModel_model _model;

    public SamplePresenter_presenter(Sample_View view)
    {
        _view = view;
        _model = new SampleModel_model();
    }

    public override void Open()
    {
        _view.Open();
        // 데이터 바인딩
    }

    public override void Close()
    {
        _view.Close();
    }

    public override void Dispose()
    {
        // 이벤트 해제 등
    }
}
