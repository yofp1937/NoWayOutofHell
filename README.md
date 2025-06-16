# 🎮 프로젝트 소개
 - 프로젝트 이름: No Way Out of Hell
 - 장르: FPS / 생존 / 좀비 아포칼립스
 - 플랫폼: PC
 - 개발인원: 1인 개발
 - 개발 기간: 2025.04.05 ~ 2025.06.15 (기본 시스템 완성)
 - 개발 엔진: Unity C#
 - 개발 목표: FPS 게임의 기본 시스템 구현(무기, 조준, 애니메이션, 적 AI 등)

## 📌 주요 기능
 ### ① 캐릭터
 ![image](https://github.com/user-attachments/assets/8cbad999-0c7d-4f0a-9247-005d0a294f00)
 - Player.cs를 중심으로 기능별로 스크립트 분리 구성(Player 제외 8개의 스크립트가 Player 객체를 구성함)
 - PlayerItem - Player의 무기 관련 처리
 - PlayerUI - 체력바와 잔탄 업데이트 등 Player의 UI 처리
 - Hp - Player의 체력 관리, Action으로 데미지 받을때 피해 이미지 Overlay 호출


https://github.com/user-attachments/assets/913f2812-2572-490e-a0d9-765481f6c3b3


 - PlayerMovement, PlayerLook - Player의 이동과 카메라 움직임 처리
 

https://github.com/user-attachments/assets/f0536f0c-8232-4c20-8e5b-5642bd95aaec


 - PlayerContoller - Input System과 연동되어 키와 동작 매핑 처리


https://github.com/user-attachments/assets/1a6aac55-5f4c-4bad-8e3a-500a0774ae6f


 - PlayerInteract - 상호작용 가능한 객체들을 탐지하고 사용 가능하게함


https://github.com/user-attachments/assets/2caf83c2-8f63-4c5d-aa68-c17ec977d499


 - PlayerAnimationController - Player의 애니메이션 동작 처리(무기별 Layer 분리)


https://github.com/user-attachments/assets/575c39fe-cd68-4d88-a1ed-4b73151251f8




 ### ② 몬스터
☆좀비 이미지
 - Enemy.cs를 중심으로 기능별로 스크립트 분리 구성(Enemy 제외 6개의 스크립트가 Enemy 객체를 구성함)
 - Enemy의 움직임은 NavMeshAgent를 활용해 구현
 - Hp - Enemy의 체력 관리
 - HitBox - 부위별 데미지 구현


https://github.com/user-attachments/assets/13891b57-5071-4d5c-8eae-5147185bde1e


 - StateMachine - Enemy의 상태에따라 PatrolState, AttackState, DeadState로 나뉘는 상태 처리 컴포넌트


https://github.com/user-attachments/assets/6bfe31db-97e8-4360-a41c-5800eab3f06a


 - TargetScanner - Enemy의 시야 내에 Player의 존재유무 검사
 - EnemyAttack - 전방에 Player가 존재하면 공격 처리
 - EnemyAnimationController - Enemy의 애니메이션 동작 처리

 ### ③ 상호작용 오브젝트
 
 #### ⓐ 무기
  - Weapon이라는 추상 클래스로 Gun, Melee 두가지 타입의 무기 컴포넌트 구현
  - Weapon은 Interactable을 상속하여 Player가 상호작용하여 획득할 수 있음
  - 무기별 데미지, 발사주기, 반동 등 고정 데이터는 GunData라는 ScriptableObject로 데이터 저장한뒤 런타임중 .Clone()을 통해 데이터를 복사하여 사용
  - IShotHandler, IReloadHandler 인터페이스 사용으로 무기별 동작방식을 다르게 구현
  - 총알은 Rigidbody로 정확한 충돌 판정 감지가 어려워서 Bullet 컴포넌트에서 Raycast로 충돌 여부를 감지함
  
  - 주무기
    - 자동소총: AK47, Scar, M4 - 세가지로 구성


https://github.com/user-attachments/assets/231ffc78-54dc-40a8-91ce-2c6e5f5111da


    - 샷건: Shotgun, Auto Shotgun - 두가지로 구성


https://github.com/user-attachments/assets/96fe5f61-fc49-4976-9c09-e6ec71d0e332


  - 보조무기
    - 권총: Pistol, Magnum, Revolver - 세가지로 구성


https://github.com/user-attachments/assets/ef346346-20af-4618-a523-7ce86d4e90c9


    - 근접무기: FireAxe, CrowBar, Shovel - 세가지로 구성


https://github.com/user-attachments/assets/56f95cd3-a7f4-472f-b691-0156cfc87e11


    - 모든 무기들은 정조준하여 발사하면 반동이 줄어듬
 
#### ⓑ 기타 오브젝트
  - 탄약상자: 상호작용시 Player의 잔탄을 가득 채워줌


https://github.com/user-attachments/assets/7bd129c6-c2e9-4c04-b9ee-9e3cc698773d

