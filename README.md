### labyrinth
### Unity + FirebaseDB<br> <br>
___
### [![YouTube Thumbnail](https://img.youtube.com/vi/oBQMDn2fYU8/0.jpg)](https://www.youtube.com/watch?v=oBQMDn2fYU8) 
[![Video](https://img.shields.io/badge/YouTube-FF0000?style=for-the-badge&logo=youtube&logoColor=white)](https://www.youtube.com/watch?v=oBQMDn2fYU8)
___
유니티로 만든 2D 게임에 파이어베이스 db (Nosql)를 연동해
<br>계정과 회원가입/로그인/인벤토리를 구현했습니다.
<br>사진 클릭시 구현영상링크로 이동합니다.

Firestore cloud : 클라우드 기반의 NoSQL DB 입니다. 
<br>
Collection(playernickname) - Document(playerstats, inventory, EquippedUI) - Field (hp, damage, speed item ID etc..)

Firebase authentification 를 사용해 이메일/비밀번호 기반 회원가입을 구현하고, 최초 로그인한 계정은 
"playernickname" 을 설정합니다. playerncikname 을 기반으로 collection 들이 생성되고 
문서형 db가 만들어집니다.

<img src ="https://github.com/naimnaro/labyrinth/assets/133749784/f65d971e-05d0-481e-877f-5fb1d5efe16c">




