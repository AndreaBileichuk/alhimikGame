using AlhimikGame.Core.Models;
 
 namespace AlhimikGame.Core.Patterns.ChainOfResponsibility;
 
 public class PurchaseHandler : IPurchaseHandler
 {
     private IPurchaseHandler _next;
     
     public void SetNext(IPurchaseHandler next)
     {
         _next = next;
     }
 
     public virtual void Handle(Player player, Shop shop, Ingredient ingredient, int quantity)
     {
         _next?.Handle(player, shop, ingredient, quantity);
     }
 }