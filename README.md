# Cesta Životem — digital board game

A complete digital version of the Czech board game *Cesta Životem* by **Fox Games**, built in
**Unity** and **C#**. This was the practical part of my Bachelor's degree at VŠPJ (Applied
Informatics, 2024), and the first game I finished.

Adapting a board game is a rules problem before it is a programming one. Every convention that
players around a table settle by common sense — whose turn resolves first, what happens when
two effects collide, what an unaffordable purchase does — has to become an explicit rule the
computer can enforce, with no room left for interpretation. Most of the work was in writing
that down, not in the rendering.

**Portfolio write-up:** https://tomas-vokoun.dev

---

## What it does

- **Hot-seat for up to eight players** on a single machine, plus a **computer-controlled player**
  so the game is playable without a full table.
- **Two figures per player**, tracked completely independently — each figure has its own money
  and its own education level.
- An **education ladder built into the board** (ZŠ → SŠ → VŠ). A figure climbs it by choosing
  to study instead of earning, which is the trade-off the whole game turns on: a round of lost
  income now against a better position later.
- **Two parallel career tracks**, employment and business, scored separately and resolved at the
  end as best employee and best entrepreneur.
- A **shop economy** split into a classic shop and a VIP shop with separate price lists, backed
  by **card piles** for events.
- A session bounded by a **30-round limit and a per-turn timer**, so a game fits inside a school
  lesson — the original board game is used for financial-literacy teaching, and digitising it
  meant schools could keep using it during distance teaching.
- A **five-part tutorial** covering starting positions, the board and the turn actions, so a
  class can start without reading the rulebook.
- Own 3D models, sound effects, menus and graphics/resolution settings.

## Turn structure

1. Roll the dice.
2. Move the figure around the board.
3. Choose: **go to work** or **go back to study**.
4. Resolve the square — event squares and drawn cards interrupt the plan, so an optimal route
   through the board is never the whole answer.
5. Spend in the shops, where a good career actually shows.

---

## Code layout

All gameplay code is under `Assets/Scripts/`:

```
Board/            the board itself and what sits on it
  Dice.cs           dice values
  DiceThrower.cs    the physical throw and reading the result
  Card.cs           a drawn event card
  FigureSpawn.cs    placing a figure at the start
  StudySpawns.cs    the education squares
  CameraZoom.cs     board camera

GameManager/      the rules engine and the round loop
  RoundManager.cs   turn order, round counting, the 30-round limit
  Movement.cs       moving a figure and resolving where it lands
  CardManager.cs    the card piles and drawing
  Timer.cs          the per-turn timer
  ColorManager.cs   player colours
  Menu.cs           in-game menu

Player/           player and figure state
  Player.cs         a player and their two figures
  Figure.cs         one figure: its own money and education level
  arrow.cs          movement indicator
  DestroyTextAnim.cs floating text feedback

LobbyMenu/        setup before the match
  MainMenu.cs, Lobby.cs, Options.cs, Dropdown.cs, TextInput.cs, Toggles.cs

EndScreen.cs      final scoring: best employee and best entrepreneur
```

The split that matters: **`Player`/`Figure` hold state, `GameManager` holds the rules.** A
figure never decides anything about the round; it only knows its own money and education. That
made the computer-controlled player straightforward to add later — it reads the same state a
human sees and calls the same actions.

## Running it

Unity project — open the repository root in Unity Hub and load the `Cesta Životem Beta` project.
Play from the main menu scene in `Assets/Scenes/`.

## Note on the repository

This is the thesis project as it was submitted, so Unity's generated folders (`Library/`,
`Logs/`, `obj/`, `.vs/`) are committed alongside the source. Everything I wrote is in
`Assets/Scripts/`; the two `.docx` files in the root are the written half of the thesis.

## Credits

Solo project. Original board game *Cesta Životem* © Fox Games — this is a student adaptation
made as thesis work, not a commercial release.
