using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Threading;

namespace Note_Taking_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            FileSystemWatcher fileWatcher = new FileSystemWatcher("./notes");
            fileWatcher.Created += jsonCreated;
            fileWatcher.EnableRaisingEvents = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var files in Directory.GetFiles("./notes"))
            {
                notesListBox.Items.Add(Path.GetFileNameWithoutExtension(files));
            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JObject jObj = new JObject();

                if (titleBox.Text == "")
                {
                    jObj["title"] = "Untitled";
                }
                else
                {
                    jObj["title"] = titleBox.Text;
                    jObj["message"] = new TextRange(messageBox.Document.ContentStart, messageBox.Document.ContentEnd).Text;

                    File.WriteAllText($"./notes/{jObj["title"]}.json", jObj.ToString());
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}\n\nSource: {error.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void jsonCreated(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                try
                {
                    notesListBox.Items.Clear();

                    foreach (var files in Directory.GetFiles("./notes"))
                    {
                        notesListBox.Items.Add(Path.GetFileNameWithoutExtension(files));
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show($"Error: {error.Message}\n\nSource: {error.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                notesListBox.Items.Clear();

                foreach (var files in Directory.GetFiles("./notes"))
                {
                    notesListBox.Items.Add(Path.GetFileNameWithoutExtension(files));
                }
            }
            catch (Exception error)
            {
                MessageBox.Show($"Error: {error.Message}\n\nSource: {error.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void readButton_Click(object sender, RoutedEventArgs e)
        {
            if (notesListBox.SelectedIndex != -1)
            {
                try
                {
                    string file = File.ReadAllText($"./notes/{notesListBox.SelectedItem}.json");

                    JObject jObj = JObject.Parse(file);
                    titleBox.Text = jObj["title"].ToString();

                    new TextRange(messageBox.Document.ContentStart, messageBox.Document.ContentEnd).Text = jObj["message"].ToString();
                }
                catch (Exception error)
                {
                    MessageBox.Show($"Error: {error.Message}\n\nSource: {error.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (notesListBox.SelectedIndex != -1)
            {
                try
                {
                    File.Delete($"./notes/{notesListBox.SelectedItem}.json");

                    notesListBox.Items.Remove(notesListBox.SelectedItem);
                }
                catch (Exception error)
                {
                    MessageBox.Show($"Error: {error.Message}\n\nSource: {error.Source}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            titleBox.Text = "";
            new TextRange(messageBox.Document.ContentStart, messageBox.Document.ContentEnd).Text = "";
        }
    }
}
