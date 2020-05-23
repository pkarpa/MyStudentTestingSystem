using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DTO;

namespace Client.Pages
{
  /// <summary>
  /// Interaction logic for StudentPassTest.xaml
  /// </summary>
  public partial class StudentPassTest : Page
  {
    ClientObject client;
    DTOTest dtoTest;
    List<QueDTOQuestionstion> questions = null;
    List<DTOQuestionType> questionType = null;
    Window mw;
    DTOTestSession _testSession;
    List<DTOAnswer> _userAnswers;
    StudentTests _previousPage;
    Dictionary<int, string> answerDictionary = new Dictionary<int, string>();

    public StudentPassTest(ClientObject cl, int testId, Window _mw, StudentTests previousPage)
    {
      InitializeComponent();
      client = cl;
      mw = _mw;
      _previousPage = previousPage;
      _userAnswers = new List<DTOAnswer>();
      dtoTest = client.GetTest(testId);
      questions = client.GetQuestions(testId);
      questionType = client.GetQuestionTypes();
      _testSession = new DTOTestSession() { Status = "Continues", StartTime = DateTime.Now, TestId = testId, StudentId = cl.GetClientId() };

      Thread.Sleep(1000);
      _testSession.TestSessionId = client.AddTestSession(_testSession);


      //IGNORE comments.
      //1. add grid with one column
      //2. add two  containers vertically
      //3. add question text to uppersection
      //4. add container horisontal
      //5. add two containers in both container from step 4.
      //6. paiste horisontal container
      //7. add answer option with certain type.
      var mainStack = new StackPanel();
      mainStack.HorizontalAlignment = HorizontalAlignment.Left;
      var listOfStackPanels = new List<StackPanel>();

      var testNameStacPanel = new StackPanel();
      var testName = new TextBlock();
      testName.FontSize = 15;
      testName.FontFamily = new FontFamily("Century Gothic");
      testName.Text = dtoTest.TestName;
      testNameStacPanel.Children.Add(testName);
      listOfStackPanels.Add(testNameStacPanel);

      int i = 0;
      foreach (var question in questions)
      {
        // Answers logic
        answerDictionary.Add(i, "");

        //
        //questionLogic
        var stackQuestion = new StackPanel();
        stackQuestion.Margin = new Thickness() { Bottom = 20, Left = 10, Right = 0, Top = 0 };
        var stackQuestionText = new StackPanel();
        var stackOptions = new StackPanel();
        var textElement = new TextBlock();
        textElement.Text = (i+1).ToString() + ". " +  question.QuestionText;
        stackQuestionText.Children.Add(textElement);
        var options = question.AnswerOption?.Split(';');
        if (options != null && options.Length != 0)
        {
          if (question.QuestionTypeId == 1)
          {

            for (int k = 0; k < options.Length; k++)
            {
              RadioButton rb = new RadioButton() { Content = " " + options[k], IsChecked = i != 0 };
              rb.Checked += (sender, args) =>
              {
                answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag] = ((System.Windows.Controls.ContentControl)args.Source).Content.ToString();
              };
              rb.Unchecked += (sender, args) => { /* Do nothing */ };
              rb.Tag = i;
              rb.IsChecked = false;
              mainStackPanel.HorizontalAlignment = HorizontalAlignment;
              stackOptions.Children.Add(rb);
            }
          }
          else
          {
            if (question.QuestionTypeId == 2)
            {
              for (int k = 0; k < options.Length; k++)
              {
                CheckBox rb = new CheckBox() { Content = " " + options[k], IsChecked = i != 0 };
                rb.Checked += (sender, args) =>
                {
                  if (string.IsNullOrEmpty(answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag]))
                  {
                    answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag] = ((System.Windows.Controls.ContentControl)args.Source).Content.ToString();
                  }
                  else
                  {
                    answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag] += " ; " + ((System.Windows.Controls.ContentControl)args.Source).Content.ToString();
                  }
                };
                rb.Unchecked += (sender, args) => {
                  /* Do stuff */
                  var multipleAnswers = answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag];
                  string elementToRemove = ((System.Windows.Controls.ContentControl)args.Source).Content.ToString();
                  string[] separatingStrings = { " ; "};
                  var newAnswers = multipleAnswers.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries).Where(val => val != elementToRemove).ToArray();
                  answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag] = string.Join(" ; ", newAnswers);
                };

                rb.Tag = i;
                mainStackPanel.HorizontalAlignment = HorizontalAlignment;
                rb.IsChecked = false;
                stackOptions.Children.Add(rb);
              }
            }
            else
            {
              if (question.QuestionTypeId == 3)
              {
                for (int k = 0; k < 2; k++)
                {
                  string[] optionsTrueFalse = { "правда", "неправда" };
                  RadioButton rb = new RadioButton() { Content = " " + optionsTrueFalse[k], IsChecked = i != 0 };
                  rb.Checked += (sender, args) =>
                  {
                    answerDictionary[(int)((System.Windows.FrameworkElement)sender).Tag] = ((System.Windows.Controls.ContentControl)args.Source).Content.ToString();
                  };
                  rb.Unchecked += (sender, args) => { /* Do nothing */ };
                  rb.Tag = i;
                  rb.IsChecked = false;
                  mainStackPanel.HorizontalAlignment = HorizontalAlignment;
                  stackOptions.Children.Add(rb);
                }
              }
              else
              {
                throw new NotImplementedException();
              }
            }
          }
        }
        else
        {
          var textElementBox = new TextBlock();
          textElementBox.Text = "Варіанти відповідей відсутні. ((";
          stackOptions.Children.Add(textElementBox);
          stackOptions.Tag = i;
        }
        stackQuestion.Children.Add(stackQuestionText);
        stackQuestion.Children.Add(stackOptions);
        listOfStackPanels.Add(stackQuestion);

        ++i;
      }

      // adding questions to stack.
      foreach (var listOfStackPanel in listOfStackPanels)
        mainStack.Children.Add(listOfStackPanel);

      mainStackPanel.Children.Add(mainStack);


      //add button SUBMIT
      Button buttonSubmit = new Button()
      {
        Content = "Submit"
      };
      //< Button Content = "Button" HorizontalAlignment = "Left" Margin = "20,10,0,0" VerticalAlignment = "Top" Width = "75" Click = "Button_Click" />
      buttonSubmit.HorizontalAlignment = HorizontalAlignment.Left;
      buttonSubmit.Margin = new Thickness() { Bottom = 20, Left = 10, Right = 0, Top = 0 };
      buttonSubmit.VerticalAlignment = VerticalAlignment.Top;
      buttonSubmit.Width = 75;
      buttonSubmit.Click += new RoutedEventHandler(Button_Click);

      mainStackPanel.Children.Add(buttonSubmit);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      _testSession.EndTime = DateTime.Now;
      var i = 0;
      foreach (var question in questions)
      {
        if (!string.IsNullOrEmpty(question.QuestionAnswer))
        {
          var answer = new DTOAnswer();
          string userAswerFromForm;
          answerDictionary.TryGetValue(i, out userAswerFromForm);
          answer.AnswerCorrects = CompateAnswers(question.QuestionAnswer, userAswerFromForm);
          answer.AnswerText = userAswerFromForm;
          answer.QuestionId = question.Id;
          answer.TestSessionId = _testSession.TestSessionId;
          _userAnswers.Add(answer);
        }
        i++;
      }


      List<int> answersId = client.AddStudentsAnswers(_userAnswers);
      //_testSession.AnswersId = answersId;
      client.UpdateTestSession(_testSession);
      //this.DataContext = null;
      this.Visibility = Visibility.Hidden;
      //NavigationService.Navigate(_previousPage);
      mw.Visibility = Visibility.Visible;
    }

    static bool CompateAnswers(string correctAnswer, string userAnswer)
    {
      var equalAnswer = false;
      var correctAnswerOptions = correctAnswer.Trim().Replace(" ", "").Split(';');
      var userAnswerOptions = userAnswer.Trim().Replace(" ", "").Split(';');
      if (correctAnswerOptions.Length == userAnswerOptions.Length)
      {
        if (correctAnswerOptions.Intersect(userAnswerOptions).Count() == correctAnswerOptions.Length)
        {
          equalAnswer = true;
        }
      }

      return equalAnswer;
    }
  }
}
