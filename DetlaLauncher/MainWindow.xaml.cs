using DetlaLauncher.Data;
using DetlaLauncher.Models;
using DetlaLauncher.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DetlaLauncher
{
    public partial class MainWindow : Window
    {
        private List<Game> games;

        public MainWindow()
        {
            InitializeComponent();
            games = GameRepository.Load();
            GamesList.ItemsSource = games;
        }

        private void AddGame_Click(object sender, RoutedEventArgs e)
        {
            var window = new GameEditWindow();
            if (window.ShowDialog() == true)
            {
                games.Add(window.Game);
                GameRepository.Save(games);
                RefreshList();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (GamesList.SelectedItem is Game game)
            {
                games.Remove(game);
                GameRepository.Save(games);
                RefreshList();
            }
        }

        private void RefreshList()
        {
            GamesList.ItemsSource = null;
            GamesList.ItemsSource = games;
        }

        private void GamesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GamesList.SelectedItem is Game game)
            {
                NameText.Text = game.Name;
                GenreText.Text = $"Жанр: {game.Genre}";
                RatingText.Text = $"Рейтинг: {game.Rating}";
                FavoriteText.Text = game.IsFavorite ? "⭐ В избранном" : "";
                DescriptionText.Text = game.Description;
                SourcesList.ItemsSource = game.Sources;
            }
        }

        private void OpenSource_Click(object sender, RoutedEventArgs e)
        {
            if (SourcesList.SelectedItem is GameSource src)
            {
                Process.Start(new ProcessStartInfo(src.Url) { UseShellExecute = true });
            }
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void FilterChanged(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string q = SearchBox.Text.ToLower();

            var filtered = games.Where(g =>
                g.Name.ToLower().Contains(q));

            if (FavFilter.IsChecked == true)
                filtered = filtered.Where(g => g.IsFavorite);

            GamesList.ItemsSource = filtered.ToList();
        }
    }
}