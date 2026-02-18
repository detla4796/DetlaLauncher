using DetlaLauncher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DetlaLauncher.Windows
{
    public partial class GameEditWindow : Window
    {
        public Game Game { get; private set; }

        public GameEditWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Введите название игры");
                return;
            }

            if (string.IsNullOrWhiteSpace(GenreBox.Text))
            {
                MessageBox.Show("Введите жанр");
                return;
            }

            if (!int.TryParse(RatingBox.Text, out int rating))
            {
                MessageBox.Show("Рейтинг должен быть числом");
                return;
            }

            if (rating < 1 || rating > 10)
            {
                MessageBox.Show("Рейтинг должен быть от 1 до 10");
                return;
            }

            if (DescriptionBox.Text.Length > 500)
            {
                MessageBox.Show("Описание слишком длинное (макс. 500 символов)");
                return;
            }

            bool sourceNameFilled = !string.IsNullOrWhiteSpace(SourceNameBox.Text);
            bool sourceUrlFilled = !string.IsNullOrWhiteSpace(SourceUrlBox.Text);

            if (sourceNameFilled ^ sourceUrlFilled)
            {
                MessageBox.Show("Источник должен содержать и название, и ссылку");
                return;
            }

            if (sourceUrlFilled)
            {
                if (!Uri.TryCreate(SourceUrlBox.Text, UriKind.Absolute, out Uri uri))
                {
                    MessageBox.Show("Некорректная ссылка");
                    return;
                }

                if (!(uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    MessageBox.Show("Ссылка должна начинаться с http или https");
                    return;
                }
            }

            Game = new Game
            {
                Name = NameBox.Text.Trim(),
                Genre = GenreBox.Text.Trim(),
                Description = DescriptionBox.Text.Trim(),
                Rating = rating,
                IsFavorite = FavoriteCheck.IsChecked == true
            };

            if (sourceNameFilled)
            {
                Game.Sources.Add(new GameSource
                {
                    Name = SourceNameBox.Text.Trim(),
                    Url = SourceUrlBox.Text.Trim()
                });
            }

            DialogResult = true;
        }

        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
