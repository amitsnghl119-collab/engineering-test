using Xunit;
using GildedRose.Console;

namespace GildedRose.Tests;

public class TestAssemblyTests
{
    [Fact]
    public void TestTheTruth()
    {
        Assert.True(true);
    }

    // Verify the new supplier rule while the item is still within its sell-by period.
    [Fact]
    public void ConjuredItemLosesTwiceAsMuchQualityBeforeSellByDate()
    {
        var shop = new Program
        {
            Items = new List<Item>
            {
                new() { Name = "Conjured Mana Cake", SellIn = 3, Quality = 10 }
            }
        };

        shop.UpdateQuality();

        Assert.Equal(8, shop.Items[0].Quality);
        Assert.Equal(2, shop.Items[0].SellIn);
    }

    // Verify that the normal conjured penalty doubles again once the sell-by date is reached.
    [Fact]
    public void ConjuredItemLosesFourQualityAfterSellByDate()
    {
        var shop = new Program
        {
            Items = new List<Item>
            {
                new() { Name = "Conjured Mana Cake", SellIn = 0, Quality = 10 }
            }
        };

        shop.UpdateQuality();

        Assert.Equal(6, shop.Items[0].Quality);
        Assert.Equal(-1, shop.Items[0].SellIn);
    }

    // Guard the shared quality limits and the special Sulfuras exception while refactoring the rules.
    [Fact]
    public void QualityRulesRemainBoundedAndLegendaryItemsRemainUnchanged()
    {
        var shop = new Program
        {
            Items = new List<Item>
            {
                new() { Name = "Conjured Mana Cake", SellIn = 0, Quality = 1 },
                new() { Name = "Aged Brie", SellIn = 0, Quality = 49 },
                new() { Name = "Backstage passes to a TAFKAL80ETC concert", SellIn = 5, Quality = 49 },
                new() { Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80 }
            }
        };

        shop.UpdateQuality();

        Assert.Equal(0, shop.Items[0].Quality);
        Assert.Equal(50, shop.Items[1].Quality);
        Assert.Equal(50, shop.Items[2].Quality);
        Assert.Equal(80, shop.Items[3].Quality);
        Assert.Equal(0, shop.Items[3].SellIn);
    }
}