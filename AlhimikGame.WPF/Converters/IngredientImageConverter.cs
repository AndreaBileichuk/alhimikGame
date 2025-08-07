using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace AlhimikGame.WPF.Converters
{
    /// <summary>
    /// Converts ingredient names to appropriate images using explicit mapping
    /// </summary>
    public class IngredientImageConverter : IValueConverter
    {
        private const string ImageBasePath = "/AlhimikGame.WPF;component/Assets/";
        private const string DefaultImagePath = "/AlhimikGame.WPF;component/Assets/default_ingredient.jpg";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new BitmapImage(new Uri(DefaultImagePath, UriKind.Relative));

            string ingredientName = value.ToString();
            string imagePath = GetImagePathForIngredient(ingredientName);

            try
            {
                return new BitmapImage(new Uri(imagePath, UriKind.Relative));
            }
            catch (IOException)
            {
                return new BitmapImage(new Uri(DefaultImagePath, UriKind.Relative));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Maps ingredient names to specific image paths
        /// </summary>
        private string GetImagePathForIngredient(string ingredientName)
        {
            return ingredientName switch
            {
                "Цілюща трава" => $"{ImageBasePath}herb.jpg",
                "Світний гриб" => $"{ImageBasePath}mushroom.jpg",
                "Кристал мани" => $"{ImageBasePath}crystal.jpg",
                "Корінь женьшеню" => $"{ImageBasePath}root.jpg",
                "Сонцецвіт" => $"{ImageBasePath}sunflower.jpg",
                "Ягода беладони" => $"{ImageBasePath}berry.jpg",
                "Срібний лист" => $"{ImageBasePath}leaf.jpg",
                "Чиста джерельна вода" => $"{ImageBasePath}water.jpg",
                "Алхімічна сіль" => $"{ImageBasePath}salt.jpg",
                "Світний мох" => $"{ImageBasePath}moss.jpg",
                "Фрагмент філософського каменю" => $"{ImageBasePath}stone.jpg",
                "Пил фей" => $"{ImageBasePath}dust.jpg",
                "Есенція життя" => $"{ImageBasePath}essence.jpg",
                _ => DefaultImagePath
            };
        }
    }
}