using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns.ChainOfResponsibility;

public class InventoryCheckHandler : PurchaseHandler
{
    public override void Handle(Player player, Shop shop, Ingredient ingredient, int quantity)
    {
        if (!shop.Inventory.ContainsKey(ingredient))
            throw new InvalidOperationException("Товар відсутній у магазині.");

        var data = shop.Inventory[ingredient];
        if (data.Quantity < quantity)
            throw new InvalidOperationException("Недостатньо товару в наявності.");

        base.Handle(player, shop, ingredient, quantity);
    }
}