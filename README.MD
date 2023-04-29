# Unity Version:
2021.3.8f1

# System Setup:
## Models
### Scriptable Object Data Holders
1. GameConfig.cs
2. ReelConfig.cs
3. SymbolConfig.cs
4. LineConfig.cs
### Object Components
1. SlotReel.cs
2. SlotSymbol.cs
## Controllers
1. GameManager.cs
2. SlotMachine.cs
## Views
1. OverlayUI.cs

# Data Sources:
## General
All Scriptable Objects can be created under the right click context menu in the project tab. It is under Create > Data > ...
## GameConfig (Assets/SlotMachine/Data)
Holds reel and payout line data, also holds slot machine spin parameters (speed and time). The number of reels and payout lines are scalable.
## Reels (Assets/SlotMachine/Data/Reels)
Holds the symbols that each reel has on and their ordering. The number of symbols are scalable.
## Symbols (Assets/SlotMachine/Data/Symbols)
Holds the data of each symbol to be used by a reel. Make sure that there are no duplicate indexes (identifier) as they are used to check for matching symbols in gameplay. You can set each symbol's payout and sprite data here.
## Payout Lines (Assets/SlotMachine/Data/Lines)
Holds the data of each payout line to be checked. The object in inspector is optimized to visually represent the rows of a slotmachine in-game. Refer to the pre-made 20 lines for examples.

# Additional Notes:
## Scalability
1. You can add/remove reels from the machine via the array in the GameConfig Scriptable Object. Each reel can then be modified to the liking of the developer (number, position, and which symbol).
2. You can add a new symbol as long as you take note which indexes are already used and you have a sprite ready to be integrated.
3. You can add payout lines, but do take caution to check the existing payout lines to see if there's already an existing line similar to the one you're planning to add.
## Flexibility
1. There are issues with adding more or less reels to the machine, but it can be fixed with enough time.
2. The process to add/decrease rows are a bit tedious as you have to make lines specific to the number of row and change the value of ROWS in the GameConfig Scriptable Object
## MVC Pattern
1. All data manipulation happens in the controller (SlotMachine.cs)
2. Data classes are declared in separate models (SlotReel.cs, SlotSymbol.cs)
3. The view class (OverlayUI.cs) is handled by the controller (SlotMachine.cs)
## Future Improvements
1. Address the scalability and flexibility issues, due to the increasing complexity when more elements are added or removed
2. Show which payout lines are triggered for the spin
3. Additional animations and juicing
