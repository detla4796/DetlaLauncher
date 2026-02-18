using DetlaLauncher.Data;
using DetlaLauncher.Models;
using DetlaLauncher.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DetlaLauncher
{
    public partial class MainWindow : Window
    {
        private List<Game> games;
        private List<Game> filteredGames;
        private string currentSortMode = "name";

        public MainWindow()
        {
            InitializeComponent();
            games = GameRepository.Load();
            GamesList.ItemsSource = games;
            filteredGames = games;
            UpdateCounter();
            SortComboBox.SelectedIndex = 0;
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

        private void EditGame_Click(object sender, RoutedEventArgs e)
        {
            if (GamesList.SelectedItem is Game game)
            {
                var window = new GameEditWindow(game);
                if (window.ShowDialog() == true)
                {
                    int index = games.IndexOf(game);
                    games[index] = window.Game;
                    GameRepository.Save(games);
                    ApplyFilters();
                    SortGames();
                }
            }
            else
            {
                MessageBox.Show("Выберите игру для редактирования");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (GamesList.SelectedItem is Game game)
            {
                var dialog = new ConfirmDialog($"Вы уверены, что хотите удалить игру {game.Name}?");
                dialog.Owner = this;
                dialog.ShowDialog();

                if (dialog.Result)
                {
                    games.Remove(game);
                    GameRepository.Save(games);
                    RefreshList();
                    ClearDetails();
                }
            }
        }

        private void RefreshList()
        {
            GamesList.ItemsSource = null;
            GamesList.ItemsSource = games;
            UpdateCounter();
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
            var filtered = games.Where(g => g.Name.ToLower().Contains(q));

            if (FavFilter.IsChecked == true)
            {
                filtered = filtered.Where(g => g.IsFavorite);
            }

            filteredGames = filtered.ToList();
            GamesList.ItemsSource = filteredGames;
            UpdateCounter();
        }
        private void TopBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ClearDetails()
        {
            NameText.Text = "";
            GenreText.Text = "";
            RatingText.Text = "";
            FavoriteText.Text = "";
            DescriptionText.Text = "";
            SourcesList.ItemsSource = null;
        }

        private void UpdateCounter()
        {
            CounterText.Text = $"Показано: {filteredGames.Count} из {games.Count} игр";
        }

        private void SortGames()
        {
            if (filteredGames == null || filteredGames.Count == 0) return;

            List<Game> sorted;
            switch (currentSortMode)
            {
                case "name":
                    sorted = filteredGames.OrderBy(g => g.Name).ToList();
                    break;
                case "rating":
                    sorted = filteredGames.OrderByDescending(g => g.Rating).ToList();
                    break;
                default:
                    sorted = filteredGames;
                    break;
            }

            filteredGames = sorted;
            GamesList.ItemsSource = null;
            GamesList.ItemsSource = filteredGames;
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SortComboBox.SelectedIndex == -1) return;

            switch (SortComboBox.SelectedIndex)
            {
                case 0: currentSortMode = "name"; break;
                case 1: currentSortMode = "rating"; break;
            }

            ApplyFilters();
            SortGames();
        }
    }
}