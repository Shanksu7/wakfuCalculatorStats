namespace Domain.Models.WAKFUAPI
{
    public class RecipeIngredients
    {
        public int RecipeId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int IngredientOrder { get; set; }
    }
}
