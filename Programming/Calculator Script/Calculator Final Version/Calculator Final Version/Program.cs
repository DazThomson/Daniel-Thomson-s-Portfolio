using System;

namespace Calculator_Final_Version
{
    class Program
    {
        static void Main()
        {
            string input1; // these variables are used to store the value entered by the user. These values will be converted to double instead of just a 
            string input2;


            bool stopLoop = false;/* this is used to handle the while loop that handles if statement that asks if the user wants to quit the application or continue. 
                                   * this is set to false because in order to allow this specific loop to keep looping if the user enters an incorrect input. */
            bool ProgramLoop = true; // this is set to true because i want this block of code to keep looping until the user wants to quit.



            Console.WriteLine("Welcome to the Scientfic Calculator");/* this line of code is displayed to the user 
                                                                      * this lets the user know what to do */
            while (ProgramLoop)//this will allow the calculator to loop constantly until the user presses Q or 0 to exit the application
            {
                Console.WriteLine("Please input your first value");//displayed to user
                input1 = Console.ReadLine();//console readline will allow the user to enter the first value.
                if (CheckValue(input1) == true)//if this if statement is true, then the code will continue properly.
                {
                    Console.WriteLine("Please input your second value");//displayed to user
                    input2 = Console.ReadLine();//Following the mesage above, this will let the user enter the second value.
                    if (CheckValue(input2) == true)//if this statement is true, then both input1 and input2 will be converted to a double. 
                    {
                        float InputValue1 = (float)Convert.ToDouble(input1);/*This is converted to double because it means that it won't have an effect on the number the user has entered. 
                                                                             * for example, if i converted this to int, then it could have an effect on the result such as values getting rounded
                                                                             * to the nearest whole number */
                        float InputValue2 = (float)Convert.ToDouble(input2);

                        /*The block of Console.WriteLine will be displayed to the user. 
                         * This will be treated as a menu which lets the user know see what they can do with their numbers. the user can also enter the number or symbol related to the operator. 
                         * for example, if the user presses 1, then the program will do addition */
                        Console.WriteLine("Please select an operation to calculate with:");
                        Console.WriteLine("1: + = Addition ");
                        Console.WriteLine("2: - = Subtraction");
                        Console.WriteLine("3: * = multiplication");
                        Console.WriteLine("4: / = Division");
                        Console.WriteLine("5: ^ = X to the power of Y");
                        Console.WriteLine("6: > = Square root");
                        Console.WriteLine("7: ! = Factorial");
                        Console.WriteLine("8: ~ = Inverse");
                        Console.WriteLine("9: ? = Pi");
                        Console.WriteLine("s: . = Sine");
                        Console.WriteLine("c: , = Cos");
                        Console.WriteLine("0: q = Quit");
                        Console.WriteLine("You can enter the number or symbol corresponding to the operator.");
                        var option = Console.ReadKey();/* read key allows the user to enter a key as an input.
                                            * For example, if the user presses "+" then program will let the users calculate the addition of 2 numbers. This applies
                                            to all of the if and else if statements below */




                        if (option.KeyChar == '+' || option.KeyChar == '1')//if the user presses 1 or +, the program will handle the addition of the 2 values.
                        {
                            //addition

                            Console.WriteLine("The answer is: {0} ", Addition(InputValue1, InputValue2));
                            /* in this line of code, Console.WriteLine outputs the answer of the addition of number 1 and 2. 
                            * It calls the function Addition because this is where the calculations will be made */



                        }
                        else if (option.KeyChar == '-' || option.KeyChar == '2')// if the user presses - or 2, the program will handle the subtraction of the 2 values
                        {
                            //subtraction

                            Console.WriteLine(" The answer is: {0} ", Subtraction(InputValue1, InputValue2));
                            /* in this line of code, Console.WriteLine outputs the answer of the subtraction of the first and second value. 
                            * It calls the function Subtraction because this is where the calculations will be made */


                        }
                        else if (option.KeyChar == '*' || option.KeyChar == '3')//if the user presses * or 3, then the program will handle the multiplication
                        {
                            //multiplication

                            Console.WriteLine(" The answer is {0}", Multiplication(InputValue1, InputValue2));
                            /* in this line of code, Console.WriteLine outputs the answer of the multiplication of the first and second value. 
                            * It calls the function Multiplication because this is where the calculations will be made */

                        }
                        else if (option.KeyChar == '/' || option.KeyChar == '4')
                        {
                            Console.WriteLine(" The answer is {0}", Division(InputValue1, InputValue2));
                            /* in this line of code, Console.WriteLine outputs the answer of the division of the first and second value. 
                            * It calls the function Division because this is where the calculations will be made */

                        }

                        else if (option.KeyChar == '^' || option.KeyChar == '5')
                        {
                            Console.WriteLine(" The answer is {0}", xToPowOfY(InputValue1, InputValue2));
                            /* in this line of code, Console.WriteLine outputs the answer of X to the power of Y by using the first and second value from the user. 
                            * It calls the function xToPowOfY because this is where the calculations will be made */

                        }
                        else if (option.KeyChar == '>' || option.KeyChar == '6')
                        {
                            Console.WriteLine(" The answer is {0}", floatSqrt(InputValue1));
                            /* in this line of code, Console.WriteLine outputs the answer of the square root of the first answer. 
                            * It calls the function floatSqrt because this is where the calculations will be made */


                        }
                        else if (option.KeyChar == '!' || option.KeyChar == '7')
                        {

                            Console.WriteLine(" The result of this calculation is: {0}", Factorial(InputValue1));
                            /* in this line of code, Console.WriteLine outputs the answer of the addition of number 1 and 2. 
                            * It calls the function Factorial because this is where the calculations will be made */

                        }
                        else if (option.KeyChar == '~' || option.KeyChar == '8')//if the using presses ~ or 8 then, the program will use what the user has entered and calculate the inverse.
                        {
                            // the four lines below this comment will be outputted to the user when t
                            Console.WriteLine(" The answer to the inverse of a number is {0}", Inverse(InputValue1));// this calls the function Inverse which calculated the inverse of a number                               

                            /* in this line of code, Console.WriteLine outputs the answer of the addition of number 1 and 2. 
                            * It calls the function Inverse because this is where the calculations will be made */

                        }
                        else if (option.KeyChar == '?' || option.KeyChar == '9')
                        {

                            Console.WriteLine(" The answer is {0}", Pi(InputValue1));
                            /* in this line of code, Console.WriteLine outputs the answer of Pi based obn
                            * It calls the function Pi because this is where the calculations will be made */

                        }

                        else if (option.KeyChar == '.' || option.KeyChar == 's')
                        {

                            Console.WriteLine(" The answer is {0}", Sine(InputValue1));
                            /* in this line of code, Console.WriteLine outputs the answer of sine from InputValue1. 
                            * It calls the function Sine because this is where the calculations will be made */

                        }
                        else if (option.KeyChar == ',' || option.KeyChar == 'c')
                        {

                            Console.WriteLine(" The answer is {0}", Cos(InputValue1));
                            /* in this line of code, Console.WriteLine outputs the answer of sine from InputValue1. 
                            * It calls the function Sine because this is where the calculations will be made */

                        }





                        else if (option.KeyChar == 'q' || option.KeyChar == '0')
                        {




                            //This while loop will be used handle the if statements that allow the user to either exit the program or return to the menu.
                            //This link has helped me understand what i need to do to exit a loop and close a program: 
                            while (!stopLoop)
                            {
                                Console.WriteLine("Would you like to close the app? please press Y/y if you want to quit or N/n if you want to continue ");//displayed to user.

                                var EndApp = Console.ReadKey();//Console ReadKey will be used in the if statement

                                if (EndApp.KeyChar == 'Y' || EndApp.KeyChar == 'y')//if the user presses Y or y, then a message will be displayed and the application will close
                                {
                                    Console.WriteLine("closing app...");
                                    Environment.Exit(1);

                                }
                                else if (EndApp.KeyChar == 'N' || EndApp.KeyChar == 'n')// if the user presses N or n, the loop will break and send the back to the start of the program.
                                {
                                    Console.WriteLine("Returning to menu");
                                    stopLoop = true;//this will allow the loop to break since stopLoop is set to false at the start of the program.



                                }
                                else//if the user doesn't press n or y, then a message will be displayed telling them to try again.
                                {
                                    Console.WriteLine("error: Invalid input, please try again.");
                                }
                            }



                        }
                        else
                        {
                            Console.WriteLine("Invalid Input, please try again");

                        }
                    }
                }



                else//This should be displayed if the user doesn't enter the correct syntax.
                {
                    Console.WriteLine("Invalid Input please try again.");
                }


            }

            static bool CheckValue(string value)/* In this function, the program will check what the user has entered into the console
                                                 if the user has entered anything quoted in the function, then the console will allow the user to go to the next part of the calculator which is
                                                 which is letting the user pick what operator they want to choose to calculate their values with. Unfortunately, I was having problems with getting this working without causing errors
                                                 so at the moment, the program crashes if the user enters anything string related.*/
            /*I used the video lectures on sanitizing user input in the module page for 4019CEM to help me create this script. 
             * As you can see, i have tried to apply this for square root, sin and factorial but the code kept breaking due to an exception error; */
            {
                bool answer = true;


                if (value == "sqrt")// if the value is equal to sqrt, then the program will allow the user to continue
                {
                    answer = true;
                    return answer;

                }
                else if (value == "Pi")//if the program is equal to pi, then the code will continue
                {
                    answer = true;
                    return answer;

                }

                else if (numberCheck(value) == true)//This references the numberCheck function which be used to sanitze the user's input system
                {
                    answer = true;
                    return answer;

                }
                else if (value == "sin")//if the user enters sin, the code will continue to the next part ehich is handling the operators for the script.
                {
                    answer = true;
                    return answer;

                }
                else if (value == "!")// this symbol is supposed to repesent calculating the factorial of a number
                {
                    answer = true;
                    return answer;
                }
                else// if the user doesn't enter anything above  this else statement, then the console will display a message which says invalid input please try again

                    answer = false;
                return answer;
            }
        }
        static bool numberCheck(string input)//This function is designed to check the numbers that the users are inputting. This number will allow the user to enter a negative, positive and decimal number without causing any errors
        {
            bool result = true;// the boolean is going to be used as a way of returning the results back to the main program.
            foreach (char userInput in input)/*This foreach loop will go through the what the user has inputted to check whether the value they have entered is a number
                                              * if it is, then the program will continue */
            {
                if (userInput < '0' || userInput > '9') //This line of code will allow the user to use any negative number and any positive number
                    if (userInput != '.')//This line of code will allow the user use input decimal numbers into teh console without causing any errors.


                        return result;

            }
            return result;
        }

        static float Addition(float number1, float number2)// This function is used handle the addition of 2 numbers.
        {
            float answer = number1 + number2;// the variable will handle the addition of number1 and number2 (these are InputValue1 and InputValue2)
            return answer;//the variable answer will get returned main where it will be printed to the user.
        }

        static float Subtraction(float numberInput1, float numberInput2)//this function is used to handle the subtraction of 2 numbers
        {
            float answer = numberInput1 - numberInput2;//this line of code will handle the subtraction of InputValue1 and InputValue2

            return answer;//this will return to main where it will be printed to the user.
        }

        static float Multiplication(float numberInput1, float numberInput2)//this will be used to handle the multiplication of 2 numbers.
        {

            return numberInput1 * numberInput2;//this will return the multiplication of InputValue1 and InputValue2
        }
        static float Division(float number1, float number2)//the 
        {

            return number1 / number2;//this will return the result of dividing number1 and number2
        }

        static double xToPowOfY(float number1, float number2)/* this function will be used to handle x to the power of y
                                                              * In this function, an if statement is included which is set in place to stop the program from breaking.
                                                              * if this if statement wasn't here, and the user entered 0 in the second value, the message stack overflow will appear. */
        {
            /*Link to webiste that helped me calculate x to the power of y: https://www.w3resource.com/csharp-exercises/recursion/csharp-recursion-exercise-15.php
             * i have adapted the code in the link to my program by letting it accept float numbers and negative numbers too. */
            // if (number2 == 0)// this if statement is used to prevent the program from saying stack overflow if the user input 0 as it's second value
            // return 1; //1 will be returned to the console


            // double answer = (float)number1 * xToPowOfY(number1, number2 - 1);/* this line of code handles the calculation for x to the power of y
            // as you can see, number is multiplied by the function xToPowOfY. within that function, number2 is subtracted by one. */

            double answer = (float)Math.Pow(number1, number2);
            return answer; /* this will return the answer of x to the power of y to the console.*/
            // unfortunately, that's been commented out doesn't work properly so i've had to resort to using Math.Pow()

        }

        static double floatSqrt(float value1)
        {

            if (value1 < 0)// the if statement will prevent the program from displaying the message NaN by returning 0 instead.
                return 0;
            double answer = Math.Sqrt(value1); //Math.Sqrt will calculate the square root of the user's number
            return answer;
        }


        static double Factorial(float number1)//Link which helped me understand calculating the factorial of a number using a for loop more: https://stackoverflow.com/questions/29027000/c-sharp-to-calculate-factorial
                                              //This forum showed me different ways in which i could calculate the factorial of a number.
        {
            float answer = 1;
            float number2; /* this value will go up based on how large the number is. 
                            * for example, if i wanted to find out the factorial of 2, then it would calculate like this:
                            * 2*1 = 2 */


            for (number2 = 1; number2 <= number1; number2++)
            /* this for loop handles the calculation of the factorial of the number the user enters                                                            
             * This means the loop will multiply every number betweeen number1 and number2. for example, if 
             * i wanted to know the factorial of 6, the console would calculate it like this:
             * 6*5*4*3*2*1 = 720 */
            {
                answer = answer * number2;
            }
            return answer;
        }

        static double Inverse(float number1)//this is the funstion which calulates the inverse of a number (1/x)                                                                                     
        {
            //This link helped me understand what the inverse actually is and how to implement it into c#:  https://www.dotnetperls.com/reciprocal

            float answer = 1 / number1; // this line of code will do 1 divided by the number entered by the user in the console. This result will be displayed to the console.

            // in this line of code, the program will do 1 divided by the number the user has inputted into the console.
            return answer;
        }


        static double Pi(float number1)
        {
            /* this function will calculate Pi based on what number the user inputs. This could range from a negative, positive and float.
             * In order to calculate pi, i needed to know what pi is as a number. This will make the process of calculating alot easier.
             * I decided to create the function Pi so the program can deal with all the calculating within the function. This helps me 
             * read the code easily*/
            double Pi = 3.1415926535897932384626433832795;
            float answer = number1 * (float)Pi; /*the variable will calculate the the answer by multiplying number1(what the user inputted)
                                                 * by Pi which has already been defined above. Pi had to be cast as a float because you convert a double to a float. */
            return answer;//This will return the answer to the main part of the code where it will printed to the console.


        }

        static double Sine(float number1)/* this function will calculate the sine of an angle in radians.
                                          * this link helped me write the code to make it fit in my work: https://www.tutorialspoint.com/math-sin-method-in-chash */
        {
            double Pi = 3.1415926535897932384626433832795;
            float answer = (float)Math.Sin(number1 * Pi / 180);/*The varibale will calculate Sine of an angle in radians. the whole calculation                                                                
                                                                * has to be casted as a float because we can't convert the pi to a float */

            return answer;// the answer to the calculation will be returned to main so it can be printed to the console.
        }

        static double Cos(float number1)/* This function will be used to handle the calculation for the Cos of an angle in radians.
                                         * this link helped me understand more aabout the sine and cosine calculation in c#: https://docs.microsoft.com/en-us/dotnet/api/system.math.cos?view=netcore-3.1
                                         * The link to the microsoft documentation helped me apply the calculation to this function */
        {
            double Pi = 3.1415926535897932384626433832795;
            float answer = (float)Math.Cos(number1 * Pi / 180);/*This variable will handle the calculation that's necessary to get the answer.
                                                                * This has to be cast a float because pi cannot be converted to a float. */

            return answer;
        }



    }
}

