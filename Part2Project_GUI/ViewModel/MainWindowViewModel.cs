using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Part2Project.Infrastructure;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using SystemFonts = System.Drawing.SystemFonts;

namespace Part2Project_GUI.ViewModel
{
    class Question
    {
        public string QuestionText { get; private set; }
        public string AnswerText { get; set; }

        public Question(string qText)
        {
            QuestionText = qText;
        }
    }

    class MainWindowViewModel : ObservableObject
    {
        private DateTime _startTime;
        private string _saveFolderName;

        private ObservableCollection<Question> _questions;
        public ObservableCollection<Question> QuestionsToAnswer
        {
            get
            {
                return _questions;
            }
            set
            {
                _questions = value;
                OnPropertyChanged("QuestionsToAnswer");
            }
        }

        private Visibility _answersVisibility = Visibility.Hidden;
        public Visibility AnswersVisibility
        {
            get { return _answersVisibility; }
            set
            {
                _answersVisibility = value;
                OnPropertyChanged("AnswersVisibility");
            }
        }

        public MainWindowViewModel()
        {
            QuestionsToAnswer = new ObservableCollection<Question>();
            for (int i = 0; i < 40; i++)
            {
                QuestionsToAnswer.Add(new Question((i+1) + ": "));
            }
        }

        public event EventHandler RequestClose;

        #region Commands

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(x => RequestClose(this, EventArgs.Empty));
                }
                return _closeCommand;
            }
        }

        private RelayCommand _testCommand;
        public RelayCommand TestCommand
        {
            get
            {
                if (_testCommand == null)
                {
                    _testCommand = new RelayCommand(x => TestCommandFunction());
                }
                return _testCommand;
            }
        }
        private void TestCommandFunction()
        {
        }

        private RelayCommand _startCommand;
        public RelayCommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(x => StartCommandFunction(), x => AnswersVisibility == Visibility.Hidden);
                }
                return _startCommand;
            }
        }
        private void StartCommandFunction()
        {
            AnswersVisibility = Visibility.Visible;
            _startTime = DateTime.Now;
        }

        private RelayCommand _stopCommand;
        public RelayCommand StopCommand
        {
            get
            {
                if (_stopCommand == null)
                {
                    _stopCommand = new RelayCommand(x => StopCommandFunction(), x => AnswersVisibility == Visibility.Visible);
                }
                return _stopCommand;
            }
        }
        private void StopCommandFunction()
        {
            // Record time taken
            TimeSpan timeTaken = (DateTime.Now - _startTime);

            // Save answers and 
        }
        
        #endregion
    }
}