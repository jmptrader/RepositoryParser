﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryParser.Core.Models;

namespace RepositoryParser.ViewModel.DayActivityViewModels.DayActivityCodeFrequency
{
    public class DayDeletedChartViewModel : ChartViewModelBase
    {
        public override void FillChartData()
        {
            if (this.ExtendedChartSeries == null)
                base.FillChartData();
        }

        public void RedrawChart(List<ExtendedChartSeries> chartSeries)
        {
            this.ExtendedChartSeries = chartSeries;
            if (this.ExtendedChartSeries != null && this.ExtendedChartSeries.Any())
            {
                this.DrawChart();
                this.FillDataCollection();
            }
        }
    }
}
