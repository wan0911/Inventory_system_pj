using System.Windows;


using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using Microsoft.Windows.Themes;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Xml.Linq;
using System.Collections;
using static materials_management.MainWindow;
using materials_management.ViewModels;
using static materials_management.Models.DatabaseModel;

namespace materials_management
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel viewModel = new MainViewModel();
            DataContext = viewModel;
        }
    }
}
