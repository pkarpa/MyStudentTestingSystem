using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
    Dictionary<int, string> answerDictionary = new Dictionary<int, string>();

    public StudentPassTest(ClientObject cl, int testId, Window _mw)
    {
      InitializeComponent();
      client = cl;
      mw = _mw;
      dtoTest = client.GetTest(testId);
      questions = client.GetQuestions(testId);
      questionType = client.GetQuestionTypes();
      _testSession = new DTOTestSession();

      //TODO
      //var sessionid = client.AddTestSession(_testSession);
      // 
      


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
      int i = 0;
      foreach (var question in questions)
      {
        // Answers logic
        answerDictionary.Add(i, "");

        //
        //questionLogic
        var stackQuestion = new StackPanel();
        var stackQuestionText = new StackPanel();
        var stackOptions = new StackPanel();
        var textElement = new TextBlock();
        textElement.Text = question.QuestionText;
        stackQuestionText.Children.Add(textElement);
        var options =  question.AnswerOption?.Split(';');
        if (options != null && options.Length != 0)
        {
          if (question.QuestionTypeId == 1)
          {

            for (int k = 0; i < options.Length; k++)
            {
              RadioButton rb = new RadioButton() { Content = " " + options[k], IsChecked = i == 0 };
              rb.Checked += (sender, args) =>
              {
                answerDictionary[i] = k.ToString();
              };
              rb.Unchecked += (sender, args) => { /* Do stuff */ };
              rb.Tag = k;
              mainStackPanel.HorizontalAlignment = HorizontalAlignment;
              stackOptions.Children.Add(rb);
            }
          } else
          {
            if (question.QuestionTypeId == 2)
            {
              for (int k = 0; i < options.Length; k++)
              {
                CheckBox rb = new CheckBox() { Content = " " + options[k], IsChecked = i == 0 };
                rb.Checked += (sender, args) =>
                {
                  answerDictionary[i] = k.ToString();
                };
                rb.Unchecked += (sender, args) => { /* Do stuff */ };
                rb.Tag = k;
                mainStackPanel.HorizontalAlignment = HorizontalAlignment;
                stackOptions.Children.Add(rb);
              }
            } else
            {
              if (question.QuestionTypeId == 3)
              {
                for (int k = 0; i < 2; k++)
                {
                  RadioButton rb = new RadioButton() { Content = " " + options[k], IsChecked = i == 0 };
                  rb.Checked += (sender, args) =>
                  {
                    answerDictionary[i] = k.ToString();
                  };
                  rb.Unchecked += (sender, args) => { /* Do stuff */ };
                  rb.Tag = k;
                  mainStackPanel.HorizontalAlignment = HorizontalAlignment;
                  stackOptions.Children.Add(rb);
                }
              } else
              {
                throw new NotImplementedException();
              }
            }
          }
        } else
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
      //TODO TIME etc.
      //client.UpdateTestSession(_testSession);

      var i = 0;
      foreach(var question in questions)
      {
        var answer = new DTOAnswer();
        string userAswerFromForm;
        answerDictionary.TryGetValue(i,out userAswerFromForm);
        answer.AnswerCorrects = question.QuestionAnswer == userAswerFromForm;
        answer.AnswerText = userAswerFromForm;
        answer.QuestionId = question.Id;
        answer.TestSessionId = _testSession.TestSessionId;
        _userAnswers.Add(answer);
      }

      //TODO
      //client.AddStudentsAnswers(_userAnswers);


      mw.Visibility = Visibility.Visible;
    }
  }
}
