using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/* Course: CSC10210 - Object Oriented Program Development
 * Project: Christmas Card Generator (Part 1 & 3)
 * Author: Harry Robinson (227870039)
 * Date: 26/12/2017 
 */ 

namespace ChristmasCardGenerator
{
    class Program
    {

        class MessagePromptException : Exception
        {
            private int errorCode;
            private string errorMessage;

            public MessagePromptException(int code, string message) : base(message)
            {
                errorMessage = message;
                errorCode = code;
            }

            public MessagePromptException() : base("Invalid Message Input!")
            {

            }

            public string ErrMessage()
            {
                return errorMessage;
            }

            public int ErrCode()
            {
                return errorCode;
            }
        }

        class ChristmasCard
        {
            private Char borderCharacter = '*';
            private string to;
            private string from;
            private string message1;
            private string message2;
            private string message3;
            private string decoration = "a";

            //Default constructor.
            public ChristmasCard()
            {

            }

            /* Second constructor enables two parameters to be set immediately 
             * via arguments during object instantiation.
             */
            public ChristmasCard(string From, string To)
            {
                from = From;
                to = To;
            }

        /* Provides the user with instructions for setting the parameters of their card.
         * Records user console input to set these parameters.
         */
            public void HandleUserInput()
            {

                Console.WriteLine("Welcome to Harry's Christmas Card Generator!");
                Console.WriteLine("Choose a decoration to begin:");
                Console.WriteLine("1 - Chimney Sweep Snowman");
                Console.WriteLine("2 - Snowflake Snowman");

                SetDecoration();
                Console.WriteLine();

                Console.WriteLine("Type in the character you wish to use for the border:");
                SetBorderCharacter(Console.ReadKey(true).KeyChar);
                Console.WriteLine();

                Console.WriteLine("Now type your name before pressing 'Enter':");
                SetFrom(Console.ReadLine());
                Console.WriteLine();

                Console.WriteLine("Great, now type the name of the person " +
                    "the card is going to and press 'Enter':");
                SetTo(Console.ReadLine());
                Console.WriteLine();

                Console.WriteLine("Nearly done. Type the message you want on the card below then press 'Enter'.");
                Console.WriteLine("NOTE: Max. 30 characters, you can add an additional line afterwards (Maximum of 3 lines)");

                SetMessages();

                while(GenerateCard(GetBorderCharacter()) == false)
                {
                    GenerateCard(GetBorderCharacter());
                }                
            }

            /* Provides user with instructions to set the message
             * to be displayed on the card. Records their console input after
             * string length requirements are verified. Sets final message lines.
             */
            public void SetMessages()
            {
                string[] messageLines = new string[3];
                Boolean endEarly = false;
                int lineNumber = 0;

                AddMessage(Console.ReadLine());
                ReportMessageInput();
                ValidateMessages();
                SaveMessages();
                Console.WriteLine("Your card is ready to generate. Press 'Enter' to continue," +
                    " or 'Escape'to clear your message lines and start them again.");

                /* Stores user console input as an element of the messageLines array.
                 * Checks for non-input, informs the user and allows them to retry.
                */
                void AddMessage(string userInput)
                {
                    try
                    {
                        messageLines[lineNumber] = userInput;
                        if (messageLines[lineNumber].Length < 1)
                        {
                            throw new MessagePromptException(100, "You must write something! Please try again and press 'Enter':");
                        }
                    }
                    catch (MessagePromptException mpException)
                    {
                        Console.WriteLine("Error Code {0}: {1}", mpException.ErrCode().ToString(), mpException.ErrMessage());
                        Console.WriteLine();
                        AddMessage(Console.ReadLine());

                    }
                }

                //provides visual feedback of user's input on the console.
                void ReportMessageInput()
                {
                    Console.WriteLine("Message line " + (1 + lineNumber) + " will read: " + messageLines[lineNumber]);
                    Console.WriteLine();
                }

                /* Checks that the last message line is not too long, otherwise
                 * prompts user to re-enter the line. Increments the lineNumber
                 * and only runs the loop while there are fewer than 3 lines.
                 */ 
                void ValidateMessages()
                {
                    try
                    {
                        while (lineNumber < 3 && endEarly == false)

                        {
                            if (CheckLimit(messageLines[lineNumber]) == true)
                            {
                                throw new MessagePromptException(101, "This message line is too long! " +
                                "Please try again and press 'Enter':");

                            }

                            else if (CheckLimit(messageLines[lineNumber]) == false && LineOptions() == true)
                            {
                                lineNumber++;
                                AddMessage(Console.ReadLine());
                                ReportMessageInput();
                            }
                        }
                    }
                    catch (MessagePromptException mpException)
                    {
                        Console.WriteLine("Error Code {0}: {1}", mpException.ErrCode().ToString(), mpException.ErrMessage());
                        Console.WriteLine();
                        AddMessage(Console.ReadLine());
                        ReportMessageInput();
                    }
                }

                /* Finalizes message lines from user input.
                */
                void SaveMessages()
                {
                    try
                    {
                        if (String.IsNullOrWhiteSpace(DoPrompt(messageLines[0],
                            messageLines[1], messageLines[2])))
                            {
                            throw new MessagePromptException(102, "Message still cant be blank!" +
                                " Please try again and press 'Enter' to confirm: ");
                            }
                        else
                        {
                            message1 = messageLines[0];
                            message2 = messageLines[1];
                            message3 = messageLines[2];
                        }
                    }
                    catch (MessagePromptException mpException)
                    {
                        Console.WriteLine("Error Code {0}: {1}", mpException.ErrCode().ToString(), 
                        mpException.ErrMessage());
                        Console.WriteLine();
                        AddMessage(Console.ReadLine());
                        ReportMessageInput();

                    }
                }

                /* Checks message line length for 30 char limit.
                */
                Boolean CheckLimit(string msg)
                {
                    if (msg.Length > 30)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                /* Provides user with options for message lines
                 * based on their key input.
                */
                Boolean LineOptions()
                {
                    Console.SetCursorPosition(0, Console.CursorTop + 2);
                    Console.WriteLine("Press:" + Environment.NewLine +
                        "'1' to add another line to your message,");
                    Console.WriteLine("'2' to clear all lines and start over, or");
                    Console.WriteLine("'3' to complete the process and generate your Christmas card.");
                    Console.WriteLine();

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.D1:
                            Console.WriteLine("Write the next line of your message and press 'Enter':");
                            return true;


                        case ConsoleKey.D2:

                            Console.WriteLine("Clearing all message lines.");
                            Console.WriteLine();
                            ClearMessage();
                            Console.WriteLine();
                            return false;

                        case ConsoleKey.D3:

                            Console.WriteLine("Continuing to card generation...");
                            Console.WriteLine();
                            endEarly = true;
                            return false;

                        default:

                            Console.WriteLine("Oops! Incorrect key input. Please try again.");
                            Console.WriteLine();
                            return false;
                    }

                }

                /* Resets the line count and replaces the message lines with empty strings.
             */
                void ClearMessage()
                {
                    lineNumber = 0;
                    for (int i = 0; i < messageLines.Length; i++)
                    {
                        messageLines[i] = String.Empty;
                    }
                    Console.WriteLine("Message lines erased.");
                    Console.WriteLine();
                    Console.WriteLine("Write your new message and press 'Enter':");
                    AddMessage(Console.ReadLine());
                    ReportMessageInput();

                }
            }

            /* Returns combined message elements to check for empty string values
            */
            public string DoPrompt(string m1, string m2, string m3)
            {
                return String.Format("{0}{1}{2}", m1, m2, m3);
            }


            /* Sets the card decoration based on the user's key input.
             */
            public void SetDecoration()
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:

                        decoration = "a";
                        Console.WriteLine("Decoration 1 selected.");
                        break;

                    case ConsoleKey.D2:

                        decoration = "b";
                        Console.WriteLine("Decoration 2 selected.");
                        break;

                    default:

                        Console.WriteLine("Oops! Incorrect key input. Please try again.");
                        break;

                }
            }

            /* Sets the border character based on the user's key input.
             */
            public void SetBorderCharacter(Char bcInput)
            {

                borderCharacter = bcInput;
                Console.WriteLine("Border character selected: " +
                    borderCharacter);
            }

            /* Gets the border character set by the user, else default value is '*'.
             */
            public Char GetBorderCharacter()
            {
                return borderCharacter;
            }

            /* Sets the recipient based on the user's console input.
             */
            public void SetTo(string toInput)
            {
                to = toInput;
                Console.WriteLine("Recipient set to: " + to);
            }

            /* Sets the sender based on the user's console input.
             */
            public void SetFrom(string fromInput)
            {
                from = fromInput;
                Console.WriteLine("Sender set to: " + from);
            }


            /* Draws the 96x21 border from the user's nominated character.
             */
            public void DrawBorder(Char borderCharacter)
            {
                DrawBorderRow(borderCharacter);
                DrawBorderColumns(borderCharacter);
                DrawBorderRow(borderCharacter);

                void DrawBorderRow(Char bcInput)
                {
                    for (int i = 0; i < 97; i++)
                    {
                        Console.Write(bcInput);
                    }
                    Console.WriteLine();
                }

                void DrawBorderColumns(Char bcInput)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        Console.SetCursorPosition(0, i);
                        Console.Write(bcInput);
                        Console.SetCursorPosition(96, i);
                        Console.WriteLine(bcInput);
                    }
                }
            }

            /*
            * Writes the message based on user to, from and message inputs
            * in the space to the right of the decoration.
            * Cursor is reset below the card for better viewing.
            */
            public void WriteMessage(string to, string msg1,
                string msg2, string msg3, string from)
            {
                Console.SetCursorPosition(50, 6);
                Console.WriteLine("Dear " + to + ",");

                Console.SetCursorPosition(50, 8);
                Console.WriteLine(msg1);
                Console.SetCursorPosition(50, 9);
                Console.WriteLine(msg2);
                Console.SetCursorPosition(50, 10);
                Console.WriteLine(msg3);

                Console.SetCursorPosition(50, 14);
                Console.WriteLine("From,");
                Console.SetCursorPosition(50, 16);
                Console.WriteLine(from);

                Console.SetCursorPosition(0, 22);
            }
            /* Draws a decoration on the card based on the user's choice. 
             * Uses a stream reader to copy the lines of characters from a .txt file.
             * File automatically closes afterward as a result of the finally clause.
             */ 
            public void DrawDecoration()
            {
                String cardLine;
                int cursorRowPosition = 2;
                int cursorColPosition = 1;
                StreamReader streamReader = new StreamReader(decoration + ".txt");

                try
                {
                    cardLine = streamReader.ReadLine();

                    while (cardLine != null)
                    {
                        Console.SetCursorPosition(cursorRowPosition, cursorColPosition);
                        Console.Write(" " + cardLine + " ");
                        cursorColPosition++;
                        cardLine = streamReader.ReadLine();
                    }
                }
                catch (IOException ioException)
                {
                    Console.WriteLine(ioException.Message);

                }
                finally
                {
                    if (streamReader != null)
                    {
                        streamReader.Close();
                    }
                }

            }

            /* Handles the keypress to confirm card generation
             * or message reset. Clears the console
             * prior to displaying card. Returns a boolean to
             * allow message reset without preventing display.
             */
            public Boolean GenerateCard(Char bcInput)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:

                        Console.Clear();
                        Display(bcInput);
                        return true;

                    case ConsoleKey.Escape:

                        Console.WriteLine();
                        Console.WriteLine("Write your new message below then press" +
                            "'Enter' to confirm:");
                        SetMessages();
                        return false;

                    default:

                        Console.WriteLine("Oops! Wrong key.");
                        return false;

                }
            }

            /* Displays card output.
             */
            public void Display(Char bcInput)
            {
                DrawBorder(bcInput);
                DrawDecoration();
                WriteMessage(to, message1, message2, message3, from);

            }
        }

        static void Main()
        {
            ChristmasCard myCard = new ChristmasCard();

            Console.SetWindowSize(100, 25);

            myCard.HandleUserInput();
            Console.ReadLine();
        }
    }
}
