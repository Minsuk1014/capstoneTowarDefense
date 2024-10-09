## .gitignore -> 깃에 올리지 않고 나만 보이게 할 파일들 (깃에 제외할 파일들)

------
** 순서 중요 **
처음
- git config --global user.name "John Doe"
- git config --global user.email johndoe@example.com
- git init
  -> 파일 경로 위치 저장

- git pull
  -> 파일 가져오기 / 현재 깃허브 상태 업데이트
- git add . (add 띄고 .임
  -> 파일 전체 추가 (함부로 하지않기)
- git add (파일명)
   -> 올리고 싶은 파일  (웬만하면 이거로 쓰기)
- git commit -m "변경사항 적기 간단하게"
  -> 저장소 올리기
- git push -u origin main
  -> 깃허브에 올리기
  
-------
