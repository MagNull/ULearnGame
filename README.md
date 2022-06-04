# Project - Pepega
## Build
https://disk.yandex.ru/d/Jwi58sWhRx5d3w
## Concept
 Bullet hell action про файты с боссами(aka Monster Hunter) и лутинг.
### Gameplay
  Основную часть времени игрок будет сражаться с боссами ради добычи лута с них. Лут можно продать или использовать в крафте(?) для улучшения оружия/абилок.
### Combat
  У игрока в арсенале 2 способа нанесения урона(мили и рэндж) и 2 абилки: временная неуязвимость aka додж и короткий блинк.
### Boss
  Каждый босс имеет несколько фаз. При смерти во время сражения игрок откатывается обратно в лобби. 
  Боссов можно проходить подряд тем самым увеличивая сложность последующих и качество их лута, но при смерти игрок теряет весь лут с предыдущих боссов.
## Implementation
### Player
  Передвижение игрока через стандартную физику. Стрельба происходит посредством создания projectile снарядов и придания им скорости. 
  При уничтожении снаряды деактивируются и переиспользуются(object pool).
  Мили атака происходит через animation event, где в определённый кадр вызывается Physics.Overlap для проверки нанесения урона.
  Блинк происходит посредством перемещения игрока на N расстояния вдоль направления движения. 
  Перед перемещением на расстояние блинка кидается луч в слое стен, который при столкновении со стеной сообщит, что блинк сделан в стену.
### Bosses
  Боссы переключают свои фазы посредством паттерна state.
## Цель на 18.04
 1. Стрельба
 2. 2 абилки
 3. Накидать боссруму.
 4. Сделать макет босса(код).
 5. Найти арт на боссов.
## Цель на 25.04
 1. 1 босс
 2. Начать делать лобби
## Цель на 02.05
 1. Лобби как место после смерти
 2. Откат в лобби после смерти
 3. Босс - обучение
 4. Подумать над системой прокачки
## Цель на 23.05
 1. Уровни боссов
 2. Итемы при убийстве боссов

## Цель на 30.05
 1.  Монеты и итемы при убийстве боссов
 2.  Меню
 3.  Музыка

