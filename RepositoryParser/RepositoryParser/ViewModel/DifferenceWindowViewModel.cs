﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using RepositoryParser.Core.Messages;
using RepositoryParser.Core.Models;
using RepositoryParser.Core.Services;
using RepositoryParser.Core.Utils;
using RepositoryParser.View;

namespace RepositoryParser.ViewModel
{
    public class DifferenceWindowViewModel : ViewModelBase
    {
        #region Variables
        private GitRepositoryService _gitRepositoryService;
        private ObservableCollection<KeyValuePair<int, string>> commitsCollection;
        private KeyValuePair<int, string> selectedItem;
        private KeyValuePair<string, string> changeSelectedItem;
        private ObservableCollection<KeyValuePair<string, string>> changesCollection;
        private string textA;
        private string textB;
        private string changeQuery;
        private string filteringQuery;
        private ResourceManager _resourceManager = new ResourceManager("RepositoryParser.Properties.Resources", Assembly.GetExecutingAssembly());
        private DifferencesColoringService colorService;
        private ObservableCollection<ChangesColorModel> listTextA;
        private ObservableCollection<ChangesColorModel> listTextB;
        private RelayCommand goToChartOfChangesCommand;
        private RelayCommand _closedEventCommand;
        private bool _progressBarVisibility;

        private BackgroundWorker _showDifferencesWorker;
        private BackgroundWorker _onLoadWorker;
        #endregion
        #region Constructors

        public DifferenceWindowViewModel()
        {
            Messenger.Default.Register<DataMessageToCharts>(this, x => HandleChartMessage(x.RepoInstance, x.FilteringQuery));
            ChangesCollection = new ObservableCollection<KeyValuePair<string, string>>();

            this._showDifferencesWorker = new BackgroundWorker();
            this._showDifferencesWorker.DoWork += this.ShowDifferencesWork;
            this._showDifferencesWorker.RunWorkerCompleted += this.ShowDifferencesCompleted;

            this._onLoadWorker = new BackgroundWorker();
            this._onLoadWorker.DoWork += this.OnLoadWork;
            this._onLoadWorker.RunWorkerCompleted += this.OnLoadWorkCompleted;

        }
        #endregion
        #region Methods




        private void HandleChartMessage(GitRepositoryService repo, string query)
        {
            this._gitRepositoryService = repo;
            this.filteringQuery = query;
            if(!_onLoadWorker.IsBusy)
                _onLoadWorker.RunWorkerAsync();
           // FillContent();
        }

        private void FillContent()
        {
            if (CommitsCollection != null && CommitsCollection.Count > 0)
                CommitsCollection.Clear();

            string query;
            if (string.IsNullOrEmpty(filteringQuery))
            {
                query = "SELECT * From GitCommits";
            }
            else
            {
                query = filteringQuery;
                SQLiteCommand command = new SQLiteCommand(query, _gitRepositoryService.SqLiteInstance.Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string message = Convert.ToString(reader["Message"]);
                    KeyValuePair<int, string> dictionary = new KeyValuePair<int, string>(id, message);
                    commitsCollection.Add(dictionary);              
                }
            }
            ProgressBarVisibility = false;
        }
        #endregion
        #region Getters/Setters

        public bool ProgressBarVisibility
        {
            get
            {
                return _progressBarVisibility;
            }
            set
            {
                if (_progressBarVisibility != value)
                    _progressBarVisibility = value;
                RaisePropertyChanged("ProgressBarVisibility");
            }
        }
        public ObservableCollection<ChangesColorModel> ListTextA
        {
            get { return listTextA;}
            set
            {
                if (listTextA != value)
                {
                    listTextA = value;
                    RaisePropertyChanged("ListTextA");
                }
            }
        }
        public ObservableCollection<ChangesColorModel> ListTextB
        {
            get { return listTextB; }
            set
            {
                if (listTextB != value)
                {
                    listTextB = value;
                    RaisePropertyChanged("ListTextB");
                }
            }
        }


        public string TextA
        {
            get
            {
                return textA;

            }
            set
            {
                if (textA != value)
                {
                    textA = value;
                    RaisePropertyChanged("TextA");
                }
            }
        }

        public string TextB
        {
            get
            {
                return textB;

            }
            set
            {
                if (textB != value)
                {
                    textB = value;
                    RaisePropertyChanged("TextB");
                }
            }
        }

        public ObservableCollection<KeyValuePair<int, string>> CommitsCollection
        {
            get
            {
                return commitsCollection;

            }
            set
            {
                if (commitsCollection != value)
                {
                    commitsCollection = value;
                    RaisePropertyChanged("CommitsCollection");
                }
            }
        }
        public ObservableCollection<KeyValuePair<string, string>> ChangesCollection
        {
            get
            {
                return changesCollection;

            }
            set
            {
                if (changesCollection != value)
                {
                    changesCollection = value;
                    RaisePropertyChanged("ChangesCollection");
                }
            }
        }

        public KeyValuePair<int, string> SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                selection(selectedItem);
                RaisePropertyChanged("SelectedItem");
            }
        }

        public KeyValuePair<string, string> ChangeSelectedItem
        {
            get
            {
                return changeSelectedItem;

            }
            set
            {
                changeSelectedItem = value;
                //ChangeSelection(changeSelectedItem);
                if(!_showDifferencesWorker.IsBusy)
                    _showDifferencesWorker.RunWorkerAsync(changeSelectedItem);
                RaisePropertyChanged("ChangeSelectedItem");
            }
        }

        public RelayCommand GoToChartOfChangesCommand
        {
            get
            {
                return goToChartOfChangesCommand ?? (goToChartOfChangesCommand = new RelayCommand(GoToChartOfChanges));
            }
        }

        private void ChangeSelection(KeyValuePair<string, string> dic)
        {
            if (!string.IsNullOrEmpty(changeQuery))
            {
                TextA = "";
                TextB = "";
                string query = changeQuery + " and Changes.Type ='" + dic.Key + "' and Changes.Path='" + dic.Value + "'";
                SQLiteCommand command = new SQLiteCommand(query, _gitRepositoryService.SqLiteInstance.Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                string texta = "";
                string textb = "";
                while (reader.Read())
                {
                    texta = Convert.ToString(reader["TextA"]);
                    textb = Convert.ToString(reader["TextB"]);
                }
                TextA = texta;
                TextB = textb;

                colorService = new DifferencesColoringService(TextA, TextB);
                colorService.FillColorDifferences();

                ListTextA = new ObservableCollection<ChangesColorModel>();
                ListTextB = new ObservableCollection<ChangesColorModel>();
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    colorService.TextAList.ForEach(x => ListTextA.Add(x));
                    colorService.TextBList.ForEach(x => ListTextB.Add(x));
                }));


                Messenger.Default.Send<DataMessageToChartOfChanges>(new DataMessageToChartOfChanges(colorService.TextAList));
            }
        }
        private void selection(KeyValuePair<int, string> dictionary)
        {
            if (ChangesCollection != null && ChangesCollection.Count > 0)
                ChangesCollection.Clear();
            string query =
                "Select * from Changes inner join ChangesForCommit on Changes.ID=ChangesForCommit.NR_Change where " +
                "ChangesForCommit.NR_Commit=" + dictionary.Key;
            changeQuery = query;
            SQLiteCommand command = new SQLiteCommand(query, _gitRepositoryService.SqLiteInstance.Connection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string type = Convert.ToString(reader["Type"]);
                string path = Convert.ToString(reader["Path"]);
                KeyValuePair<string, string> values = new KeyValuePair<string, string>(type, path);
                ChangesCollection.Add(values);
            }

        }

        private void GoToChartOfChanges()
        {
            ChartOfChangesView _window = new ChartOfChangesView();
            if(colorService != null)
                Messenger.Default.Send<DataMessageToChartOfChanges>(new DataMessageToChartOfChanges(colorService.TextAList));
            _window.Show();
        }
        #endregion



        #region backgroundworker
        private void ShowDifferencesWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.InvokeAsync(new Action(() => { ProgressBarVisibility = true; }), DispatcherPriority.Send);
            KeyValuePair<string, string> arg = (KeyValuePair<string, string>) e.Argument;
            ChangeSelection(arg);
        }

        private void ShowDifferencesCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProgressBarVisibility = false;
        }


        private void OnLoadWork(object sender, DoWorkEventArgs e)
        {
            Application.Current.Dispatcher.InvokeAsync(new Action(() => { ProgressBarVisibility = true; }),DispatcherPriority.Send);
            int prog = 0;
            CommitsCollection=new ObservableCollection<KeyValuePair<int, string>>();
            
            string query;
            if (string.IsNullOrEmpty(filteringQuery))
            {
                query = "SELECT * From GitCommits";
            }
            else
            {
                query = filteringQuery;
            }
            SQLiteCommand command = new SQLiteCommand(query, _gitRepositoryService.SqLiteInstance.Connection);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["ID"]);
                    string message = Convert.ToString(reader["Message"]);
                    KeyValuePair<int, string> dictionary = new KeyValuePair<int, string>(id, message);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        CommitsCollection.Add(dictionary);
                    });
                    
                 }
            ProgressBarVisibility = false;
        }

        private void OnLoadWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }



        #endregion

        public RelayCommand ClosedEventCommand
        {
            get { return _closedEventCommand ?? (_closedEventCommand = new RelayCommand(ClosedEvent)); }
        }

        private void ClosedEvent()
        {
            if(CommitsCollection != null)
                CommitsCollection.Clear();
            if(ChangesCollection != null)
                ChangesCollection.Clear();
            if(ListTextA != null)
                ListTextA.Clear();
            if(ListTextB !=null)
                ListTextB.Clear();
        }
    }
}
