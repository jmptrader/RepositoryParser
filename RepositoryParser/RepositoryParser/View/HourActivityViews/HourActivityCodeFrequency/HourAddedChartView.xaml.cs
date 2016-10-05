﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using RepositoryParser.Helpers;
using RepositoryParser.ViewModel.HourActivityViewModels.HourActivityCodeFrequency;

namespace RepositoryParser.View.HourActivityViews.HourActivityCodeFrequency
{
    /// <summary>
    /// Interaction logic for HourAddedChartView.xaml
    /// </summary>
    public partial class HourAddedChartView : UserControl
    {
        public HourAddedChartView()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                ChartingHelper.Instance.DrawChart<HourAddedChartViewModel>(this, this.ChartViewInstance);
            };
        }
    }
}
