﻿using System.Windows.Controls;
using RepositoryParser.Helpers;
using RepositoryParser.ViewModel.UserActivityViewModels;

namespace RepositoryParser.View.UsersActivityViews
{
    /// <summary>
    /// Interaction logic for UserActivityFilesAnalyseView.xaml
    /// </summary>
    public partial class UserActivityFilesAnalyseView : UserControl
    {
        public UserActivityFilesAnalyseView()
        {
            InitializeComponent();
            ChartingHelper.Instance.DrawChart<UsersActivityFilesAnalyseViewModel>(this, this.ChartViewInstance);
        }
    }
}
