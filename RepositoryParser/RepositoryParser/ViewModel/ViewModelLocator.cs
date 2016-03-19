/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:RepositoryParser"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using System.Windows.Navigation;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using RepositoryParser.Core.ViewModel;

namespace RepositoryParser.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AnalisysWindowViewModel>();
            SimpleIoc.Default.Register<ChartWindowViewModel>();
            SimpleIoc.Default.Register<MonthActivityChartViewModel>();
            SimpleIoc.Default.Register<DifferenceWindowViewModel>();
            SimpleIoc.Default.Register<DayActivityViewModel>();
            SimpleIoc.Default.Register<WeekDayActivityViewModel>();
            SimpleIoc.Default.Register<HourActivityViewModel>();
            SimpleIoc.Default.Register<ChartOfChangesViewModel>();

        }




        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public AnalisysWindowViewModel Analisys
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalisysWindowViewModel>();
            }
        }

        public ChartWindowViewModel Chart
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChartWindowViewModel>();
            }
        }

        public MonthActivityChartViewModel MonthActivity
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MonthActivityChartViewModel>();
            }
        }

        public DifferenceWindowViewModel Difference
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DifferenceWindowViewModel>();
            }
        }

        public DayActivityViewModel DayActivity
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DayActivityViewModel>();

            }
        }

        public WeekDayActivityViewModel WeekdayActivity
        {
            get
            {
                return ServiceLocator.Current.GetInstance<WeekDayActivityViewModel>();
            }
        }

        public HourActivityViewModel HourActivity
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HourActivityViewModel>();

            }
        }

        public ChartOfChangesViewModel ChartOfChanges
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChartOfChangesViewModel>();
                
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}