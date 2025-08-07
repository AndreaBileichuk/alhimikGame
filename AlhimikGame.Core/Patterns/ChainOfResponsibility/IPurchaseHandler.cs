using AlhimikGame.Core.Models;

namespace AlhimikGame.Core.Patterns.ChainOfResponsibility;

public interface IPurchaseHandler
{
    void SetNext(IPurchaseHandler next);
    void Handle(Player player, Shop shop, Ingredient ingredient, int quantity);
}