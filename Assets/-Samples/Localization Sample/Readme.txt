Unity Docs - Localization 참고 (https://docs.unity3d.com/Packages/com.unity.localization@1.5/manual/index.html)

1. 세팅 방법

	1) (Localization Setting이 없을 경우) Project Settings -> Localization -> Create 

	2) Locale Generator 클릭 -> 필요한 Locale 선택 -> Generate Locales

	3) Specific Locale Selector와 Project Locale Identifier에서 기본으로 사용할 Locale을 적용 (신규 생성 시 기본으로 적용되어 있음.)


2. 적용 방법

	1) Window -> Asset Management -> Localization Tables
		   (Table이 없을 경우) New Table -> 사용할 Locale 선택 -> Create
		   Texture, Audio 등의 Locale 테이블도 존재함

	2) New Entry -> Key와 Localization할 리소스 입력

	3-1) 해당 Localization을 적용할 UI 컴포넌트 우클릭 -> Localization 선택
	3-2) 혹은 Add Component -> Localize UI(String, Texture 등) Event -> Update UI Event에 해당 UI 추가.

	4) String Reference에서 위에서 추가한 Entry의 Key 선택하여 Table과 연결


3. 다이나믹 스트링 (스마트 스트링)

	1) 