﻿using System.Windows.Controls;
using RepositoryParser.CommonUI.CodeFrequency;
using RepositoryParser.Helpers;

namespace RepositoryParser.View.WeekdayActivityViews.WeekdayActivityCodeFrequency
{
    /// <summary>
    /// Interaction logic for WeekdayDeletedChartView.xaml
    /// </summary>
    public partial class WeekdayDeletedChartView : UserControl
    {
        public WeekdayDeletedChartView()
        {
            InitializeComponent();
            this.Loaded += (s, e) =>
            {
                ChartingHelper.Instance.DrawChart<CodeFrequencySubChartViewModel>(this, this.ChartViewInstance);
            };
        }
    }
}
