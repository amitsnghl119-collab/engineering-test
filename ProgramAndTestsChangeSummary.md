# Program.cs and TestAssemblyTests.cs Change Summary

## Overview
This summary explains the logic that was added or adjusted in the main application file and the test file for the Gilded Rose kata.

## Changes in Program.cs

### 1. Inventory setup
The `Main` method initializes a list of sample items, including:
- normal items
- Aged Brie
- Sulfuras
- Backstage passes
- Conjured Mana Cake

This setup gives the application a realistic inventory to process through the update cycle.

### 2. Quality update engine
The `UpdateQuality()` method loops over every item and applies rules based on the item name.

#### Sulfuras exception
Sulfuras are treated as legendary items:
- their `SellIn` value stays the same
- their `Quality` value stays at `80`
- the method immediately skips them

This avoids changing these special items.

#### Aged Brie behavior
If the item is `Aged Brie`, its quality increases:
- by `1` before the sell-by date is reached
- by `2` after the sell-by date is passed

The value is still capped so it never exceeds `50`.

#### Backstage passes behavior
For Backstage passes, quality changes depend on how close the concert is:
- less than 5 days left: `+3`
- less than 10 days left: `+2`
- otherwise: `+1`

If `SellIn <= 0`, the quality is reset to `0` immediately.

#### General item degradation
For standard items, quality decreases by `1` per day.

For Conjured items, degradation is doubled:
- before sell-by: `-2` per update
- after sell-by: `-4` per update

This is implemented by detecting names beginning with `Conjured ` and increasing the degradation factor.

### 3. Quality bounds
The helper method `UpdateQualityWithinBounds()` applies a clamp so the quality always remains between `0` and `50`.

This prevents items from becoming negative or exceeding the maximum quality allowed by the rules.

### 4. Sell-in tracking
At the end of each item update, `SellIn--` reduces the item's remaining days by one.

This ensures that time passes consistently after each inventory refresh.

---

## Changes in TestAssemblyTests.cs

### 1. Baseline truth test
The initial test, `TestTheTruth`, simply confirms the test project is wired correctly.

It is a placeholder sanity check and does not validate business logic.

### 2. Conjured item before sell-by date
The test `ConjuredItemLosesTwiceAsMuchQualityBeforeSellByDate` verifies:
- a Conjured item with `SellIn = 3` and `Quality = 10`
- after the update, quality drops to `8`
- `SellIn` becomes `2`

This confirms the rule that conjured items degrade twice as fast before expiration.

### 3. Conjured item after sell-by date
The test `ConjuredItemLosesFourQualityAfterSellByDate` verifies:
- a Conjured item with `SellIn = 0` and `Quality = 10`
- after the update, quality drops to `6`
- `SellIn` becomes `-1`

This validates the doubled penalty once the item is past its sell-by date.

### 4. Boundaries and legendary item safety
The test `QualityRulesRemainBoundedAndLegendaryItemsRemainUnchanged` checks several edge cases:
- Conjured item at minimum quality is clamped to `0`
- Aged Brie can reach the maximum quality of `50`
- Backstage passes can also reach `50`
- Sulfuras remain unchanged at `80` quality and `0` sell-in

This protects the major business rules and confirms the refactor did not break quality limits or the Sulfuras exception.

---

## Summary of the business intent
The code changes make the rules more explicit and safer:
- legendary items are not altered
- quality values stay within valid limits
- Aged Brie and Backstage passes have special treatment
- Conjured items degrade faster than standard goods
- tests document and protect these rules through real assertions

This creates a maintainable rule engine with automated verification for the key scenarios.
