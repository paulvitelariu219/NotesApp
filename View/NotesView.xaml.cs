using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Azure.Storage.Blobs;
using NotesApp.ViewModel;
using NotesApp.ViewModel.Helpers;

namespace NotesApp.View
{
    /// <summary>
    /// Interaction logic for NotesView.xaml
    /// </summary>
    public partial class NotesView : Window
    {
        NotesViewModel NotesViewModel;
        public NotesView()
        {
            InitializeComponent();
            NotesViewModel = Resources["vm"] as NotesViewModel;
            Loaded += NotesView_Loaded;
            NotesViewModel.SelectedNoteChanged += NotesViewModel_SelectedNoteChanged;

            fontFamilyComboBox.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            List<double> fontSizes = new List<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 30 };
            fontSizeComboBox.ItemsSource = fontSizes;
        }


        private async void NotesViewModel_SelectedNoteChanged(object? sender, EventArgs e)
        {
            contentRichTextBox.Document.Blocks.Clear();
            if (NotesViewModel.SelectedNote != null)
            {
                if (!string.IsNullOrEmpty(NotesViewModel.SelectedNote.FileLocation))
                {
                    string downloadPathValue = $"{NotesViewModel.SelectedNote.Id}.rtf";
                    await new BlobClient(new Uri(NotesViewModel.SelectedNote.FileLocation)).DownloadToAsync(downloadPathValue);
                    using (FileStream fileStream = new FileStream(downloadPathValue, FileMode.Open))
                    {
                        var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                        contents.Load(fileStream, DataFormats.Rtf);
                    }
                }
            }
        }

        private async void NotesView_Loaded(object sender, RoutedEventArgs e)
        {
            if (NotesViewModel != null)
            {
                await NotesViewModel.GetNotebooks();
            }
            //MessageBox.Show(DataContext?.GetType().FullName ?? "DataContext is NULL");
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string fileName = $"{NotesViewModel.SelectedNote.Id}.rtf";
            string rtfFile = System.IO.Path.Combine(Environment.CurrentDirectory, fileName);

            using (FileStream fileStream = new FileStream(rtfFile, FileMode.Create))
            {
                var contents = new TextRange(contentRichTextBox.Document.ContentStart, contentRichTextBox.Document.ContentEnd);
                contents.Save(fileStream, DataFormats.Rtf);
            }
            NotesViewModel.SelectedNote.FileLocation = await UpdateFile(rtfFile, fileName);
            await DatabaseHelper.Update(NotesViewModel.SelectedNote);
            //File.Delete(rtfFile);
        }

        private async Task<string> UpdateFile(string rtfFilePath, string fileName)
        {
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=cloneevernotestorace;AccountKey=AxMXTn1JsbCE8687biJPhSyYCrCY/KnFGB1jCaRDMOuR6C0AGRByBhy30O/G/7raW2vwJ2G95u8++ASt2+hqwg==;EndpointSuffix=core.windows.net";
            string containerName = "notes";

            var container = new BlobContainerClient(connectionString, containerName);
            //container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(fileName);
            await blob.UploadAsync(rtfFilePath, overwrite: true);

            return $"https://cloneevernotestorace.blob.core.windows.net/notes/{fileName}";
        }

        private void fontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontFamilyComboBox.SelectedItem != null)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, fontFamilyComboBox.SelectedItem);
            }
        }

        private void fontSizeComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, fontSizeComboBox.Text);
        }

        private void boldButton_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = (sender as ToggleButton).IsChecked ?? false;
            if (isChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void italicButton_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = (sender as ToggleButton).IsChecked ?? false;
            if (isChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }

        private void underlineButton_Click(object sender, RoutedEventArgs e)
        {
            bool isButtonChecked = (sender as ToggleButton).IsChecked ?? false;
            if (isButtonChecked)
            {
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else
            {
                TextDecorationCollection textDecorations;
                (contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) as TextDecorationCollection).TryRemove(TextDecorations.Underline, out textDecorations);
                contentRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, textDecorations);
            }
        }

        private void contentRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            fontFamilyComboBox.SelectedItem = contentRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            
            fontSizeComboBox.Text = contentRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty).ToString();

            var selectedWeight = contentRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
            boldButton.IsChecked = (selectedWeight != DependencyProperty.UnsetValue) && (selectedWeight.Equals(FontWeights.Bold));

            var selectedStyle = contentRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
            italicButton.IsChecked = (selectedStyle != DependencyProperty.UnsetValue) && (selectedStyle.Equals(FontStyles.Italic));

            var selectedDecoration = contentRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            underlineButton.IsChecked = (selectedDecoration != DependencyProperty.UnsetValue) && (selectedDecoration.Equals(TextDecorations.Underline));
        }


    }
}
