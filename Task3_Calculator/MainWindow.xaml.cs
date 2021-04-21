using System;
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

//
using System.Collections;
//


namespace Task3_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
        }


        #region DECLARING VARIABLE

        //Store the mathimatical expression in an ArrayList
        ArrayList calculatedExpression = new ArrayList();

        //Indicate the last added index to equation screen
        static int lastAdded = 0;

        //Indicate the length of the last added number or operator
        static int stringLength = 0;

        //Indicate the start of inputing a new number
        static int newNumber = 1;

        #endregion


        //Function that triggered when pressing any number from (0-9)
        private void Num_Click(object sender, EventArgs e)
        {

            //CLEAR THE SCREEN 
            check_To_ClearTheScreen();

            //UnBox the clicked object to its original button object  type
            Button btn = sender as Button;

            //Calculating the length of the newly inserted characters
            stringLength = resultTxt.Text.Length - lastAdded;

            //If the user input new number or the first number at all
            //he could input decimal point like '0.' num  to represent fraction
            if (btn.Content.ToString() == "." && newNumber == 1)
            {
                resultTxt.Text += "0.";
            }
            //Check if the user has already input decimal point           
            else if (btn.Content.ToString() == "." && !(resultTxt.Text.Substring(lastAdded, stringLength).Contains(".")))
            {
                resultTxt.Text += btn.Content.ToString();

            }
            //Inputed ordinary number
            else if (btn.Content.ToString() != ".")
            {
                resultTxt.Text += btn.Content.ToString();
            }

            //Make the newNumber equal 0 
            newNumber = 0;

        }


        //It is triggered every time the user input an operator to 
        //manage to input the decimal numbers and numbers more than
        // 9  as one item in array list
        void addNumber()
        {
            //Get the length of inputed number
            stringLength = resultTxt.Text.Length - lastAdded;

            //Add the inputed number to the array list as element
            calculatedExpression.Add(resultTxt.Text.Substring(lastAdded, stringLength));
        }


        private void operator_Click(object sender, EventArgs e)
        {
            //UnBox the clicked object to its original button object  type
            Button btn = sender as Button;

            //CHECK TO WRITE OPERATOR
            if (resultTxt.Text == "" || resultTxt.Text == "0.")
                return;

            //CHECK IF THERE IS TWO OPERATORS 

            //GET THE LENGTH OF STRING 
            var lastChar = resultTxt.Text.Last();

            bool isOperator = (lastChar == '+' || lastChar == '-' || lastChar == '*' || lastChar == '/');
            
            if (isOperator)
                return;

            //CLEAR THE SCREEN 
            check_To_ClearTheScreen();
           
            //Adding the number before operator to the array list
            addNumber();
            
            //Adding the operator to the screan
            resultTxt.Text += btn.Content.ToString();

            //adding the operator to the arrayList
            calculatedExpression.Add(btn.Content.ToString());

            //Updating the position of last added character
            lastAdded = resultTxt.Text.Length;

            //Starting new number to enter
            newNumber = 1;
        }


        private void Equal_Click(object sender, EventArgs e)
        {
            //Adding the last inserted number
            addNumber();

            //Count of element in the array list
            int countOfElements = calculatedExpression.Count;

            //Variable used in Multiplication and division to perform the operation and 
            //add the result to the array list
            double result = 0;

            //Variable used as indexer for array list
            int looper = 1;

            //Doing Multiplication and division first to perserve the operation precedence
            while (looper < countOfElements)
            {

                //Switch for multiplcation and division
                switch (calculatedExpression[looper].ToString())
                {
                    case "*":
                        //Make the operation
                        result = double.Parse(calculatedExpression[looper - 1].ToString()) * double.Parse(calculatedExpression[looper + 1].ToString());

                        //Remove the far operand which has higher index
                        calculatedExpression.RemoveAt(looper + 1);

                        //Remove the operator
                        calculatedExpression.RemoveAt(looper);

                        //Adding the reult to the array in the first operand index
                        calculatedExpression[looper - 1] = result.ToString();

                        //Updating the count of the elements
                        countOfElements = calculatedExpression.Count;

                        //the looper Now is on the second operator to check it
                        break;


                    case "/":

                        //HANDEL DIVIDE BY ZERO
                        if (calculatedExpression[looper + 1].ToString() == "0")
                        {
                            resultTxt.Text = "Can't Divide by Zero";
                            calculatedExpression.Clear();
                            return;
                        }


                        //Make the operation
                        result = double.Parse(calculatedExpression[looper - 1].ToString()) / double.Parse(calculatedExpression[looper + 1].ToString());

                        //Remove the far operand which has higher index
                        calculatedExpression.RemoveAt(looper + 1);

                        //Remove the operator
                        calculatedExpression.RemoveAt(looper);

                        //Adding the reult to the array in the first operand index
                        calculatedExpression[looper - 1] = result.ToString();

                        //Updating the count of the elements
                        countOfElements = calculatedExpression.Count;



                        break;
                    //Incase of addition or subtraction just skip the operation for now
                    default:

                        looper += 2;
                        break;
                }
            }


            //countOfElements = calculatedExpression.Count;

            //assign the first value to the final result
            double AddingSubtractionResults = double.Parse(calculatedExpression[0].ToString());

            //Looping on the array list items
            for (int i = 1; i < countOfElements; i += 2)
            {
                //making adding and subtraction operations
                switch (calculatedExpression[i].ToString())
                {
                    case "+":
                        //If addition add to the final result
                        AddingSubtractionResults += double.Parse(calculatedExpression[i + 1].ToString());
                        break;


                    case "-":
                        //If subtraction sutract from the final result
                        AddingSubtractionResults -= double.Parse(calculatedExpression[i + 1].ToString());
                        break;

                }
            }

            //Clearing the arrayList
            calculatedExpression.Clear();

            //Presenting the result on screen
            resultTxt.Text = AddingSubtractionResults.ToString();

            //Updating the last index position
            lastAdded = 0;
        }


        private void Clear_Click(object sender, EventArgs e)
        {
            //Clearing the scree
            resultTxt.Text = "";

            //Clearing the arrayList
            calculatedExpression.Clear();

            //Updating the last index position
            lastAdded = 0;

            //DECLARING START OF INPUT NEW NUMBER
            newNumber = 1; 
        }



        private void check_To_ClearTheScreen()
        {
            if (resultTxt.Text == "Can't Divide by Zero")
                Clear_Click(new object (), new EventArgs());        
        }

    }
  }

