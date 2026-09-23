    using System.Collections.Generic;

namespace GildedRose.Console;

public class Program
{
    public IList<Item> Items = new List<Item>();

    static void Main(string[] args)
    {
        System.Console.WriteLine("OMGHAI!");

        var app = new Program()
                      {
                          Items = new List<Item>
                                      {
                                          new Item {Name = "+5 Dexterity Vest", SellIn = 10, Quality = 20},
                                          new Item {Name = "Aged Brie", SellIn = 2, Quality = 0},
                                          new Item {Name = "Elixir of the Mongoose", SellIn = 5, Quality = 7},
                                          new Item {Name = "Sulfuras, Hand of Ragnaros", SellIn = 0, Quality = 80},
                                          new Item
                                              {
                                                  Name = "Backstage passes to a TAFKAL80ETC concert",
                                                  SellIn = 15,
                                                  Quality = 20
                                              },
                                          new Item {Name = "Conjured Mana Cake", SellIn = 3, Quality = 6}
                                      }

                      };

        app.UpdateQuality();

        System.Console.ReadKey();
    }

    public void UpdateQuality()
    {
        foreach (var item in Items)
        {
            // Sulfuras is legendary: neither its quality nor its sell-by date changes.
            if (item.Name == "Sulfuras, Hand of Ragnaros")
            {
                continue;
            }

            if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
            {
                UpdateBackstagePass(item);
            }
            else if (item.Name == "Aged Brie")
            {
                UpdateQualityWithinBounds(item, item.SellIn <= 0 ? 2 : 1);
            }
            else
            {
                // Conjured items degrade twice as fast, and every ordinary degradation doubles after the due date.
                var degradation = item.Name.StartsWith("Conjured ", StringComparison.Ordinal) ? 2 : 1;
                if (item.SellIn <= 0)
                {
                    degradation *= 2;
                }

                UpdateQualityWithinBounds(item, -degradation);
            }

            item.SellIn--;
        }
    }

    private static void UpdateBackstagePass(Item item)
    {
        // A pass has no value after the concert; otherwise its increase depends on time remaining.
        if (item.SellIn <= 0)
        {
            item.Quality = 0;
            return;
        }

        var increase = item.SellIn < 6 ? 3 : item.SellIn < 11 ? 2 : 1;
        UpdateQualityWithinBounds(item, increase);
    }

    private static void UpdateQualityWithinBounds(Item item, int change)
    {
        // Quality is bounded for every item except Sulfuras, which is returned before this helper is called.
        item.Quality = Math.Clamp(item.Quality + change, 0, 50);
    }
}

public class Item
{
    public string Name { get; set; } = "";

    public int SellIn { get; set; }

    public int Quality { get; set; }
}
