## PR Summary

This change updates the Gilded Rose inventory rules and adds focused coverage for the new behavior.

### What changed
- Added special handling for Sulfuras so their quality and sell-in never change.
- Kept quality values within the valid range of 0 to 50 using bounded logic.
- Updated Aged Brie to increase in quality as it ages, with a stronger increase after the sell-by date.
- Updated Backstage passes to improve based on how close the concert date is, dropping to zero after the event.
- Added conjured item handling so they degrade at double the normal rate, and at quadruple the rate after the sell-by date.
- Decremented the sell-in value for each item after processing.

### Test coverage added
- Verified Conjured items lose twice as much quality before sell-by.
- Verified Conjured items lose four quality after sell-by.
- Verified quality bounds are respected and Sulfuras remain unchanged.

### Files affected
- [src/GildedRose.Console/Program.cs](src/GildedRose.Console/Program.cs)
- [src/GildedRose.Tests/TestAssemblyTests.cs](src/GildedRose.Tests/TestAssemblyTests.cs)
