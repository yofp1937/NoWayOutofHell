# 🎮 프로젝트 소개
 - 프로젝트 이름: No Way Out of Hell
 - 장르: FPS / 생존 / 좀비 아포칼립스
 - 플랫폼: PC
 - 개발인원: 1인 개발
 - 개발 기간: 2025.04.05 ~ 2025.06.13 (임시)
 - 개발 엔진: Unity C#
 - 개발 목표: FPS 게임의 기본 시스템 구현(무기, 조준, 애니메이션, 적 AI 등)

## 📌 주요 기능
### ① 캐릭터
 - Player.cs를 중심으로 기능별로 스크립트 분리 구성
 - 사용한 기술: Player Input System, 모듈화
 - 구현 요소: 카메라 시스템, 캐릭터 이동, 애니메이션 연동, Raycast 기반 상호작용 시스템, 체력 시스템, 총기 반동
    - PlayerAnimationController - 애니메이션 처리(이동은 Blend Tree 활용, 무기별 Layer 분리)
    - PlayerController - Player Input System을 활용한 Key 입력 처리(동작 제어와 입력 이벤트 매핑)
    - PlayerHealth - 체력, 피해 처리
    - PlayerIntercat - 상호작용 처리(카메라 중앙에서 Ray를 쏴 상호작용 가능한 객체를 탐지하고, UI 텍스트 갱신 및 상호작용 수행)
    - PlayerItem - 무기 획득 및 장착과 장비 전환 처리
    - PlayerLook - 마우스 입력 기반 시야 회전, 조준 상태 전환, 총기 반동 처리
    - PlayerMovement - 입력 기반 이동 및 점프 처리, 시야 방향에 맞춘 캐릭터 회전 제어
    - PlayerUI - 체력바, 탄약 수 등 UI 출력
 
 몬스터

 상호작용 오브젝트

 무기
  - 주무기
    - 자동소총
    - 샷건
  - 보조무기
    - 권총
    - 근접무기
 
 상호작용 오브젝트
  - 탄약상자
