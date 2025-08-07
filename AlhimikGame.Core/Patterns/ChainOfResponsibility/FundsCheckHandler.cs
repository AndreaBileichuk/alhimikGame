using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns.ChainOfResponsibility;

public class FundsCheckHandler : PurchaseHandler
{
    public override void Handle(Player player, Shop shop, Ingredient ingredient, int quantity)
    {
        var price = shop.Inventory[ingredient].Price;
        if (player.Gold < price * quantity)
            throw new InvalidOperationException("Недостатньо золота для покупки.");

        base.Handle(player, shop, ingredient, quantity);
    }
}