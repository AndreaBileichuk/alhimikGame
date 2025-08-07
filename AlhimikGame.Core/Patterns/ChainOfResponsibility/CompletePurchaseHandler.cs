using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns.ChainOfResponsibility;

public class CompletePurchaseHandler : PurchaseHandler
{
    public override void Handle(Player player, Shop shop, Ingredient ingredient, int quantity)
    {
        player.AddIngredient(ingredient, quantity);
        shop.PurchaseItem(ingredient, quantity);
        base.Handle(player, shop, ingredient, quantity);
    }
}
