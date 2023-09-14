namespace FlowControl
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            MyConsoleMenu menu = new();
            menu.Open();
        }

        /// <summary>
        /// Class <c>MyConsoleMenu</c> models a console app menu with 5 options.
        /// </summary>
        class MyConsoleMenu
        {
            private List<Option> options;

            /// <summary>
            /// Gets the option at the specific index.
            /// </summary>
            /// <param name="index">The index that should be visible after the menu is written to console</param>
            /// <returns>An option with functionality</returns>
            public Option getOption(int index)
            {
                return options[index];
            }

            /// <summary>
            /// initializes a new instance of <c>MyConsoleMenu</c> with 5 default options
            /// </summary>
            public MyConsoleMenu()
            {
                options = new() { CreateExit(), CreateTicketPriceByAge(), CreateTicketPriceByQuantityAndAge(), CreateRepeat10Times(), CreateTheThirdWord()};
            }

            /// <summary>
            /// Opens the menu writing it to console and waits for user input
            /// </summary>
            public void Open()
            {
                bool stop = false;
                string error = "";
                do
                {
                    Console.Clear();
                    Console.WriteLine(GetMenu() + "\n");

                    if (!string.IsNullOrEmpty(error))
                    {

                        Console.Write(error + "\n");
                    }

                    Console.Write("Enter option: ");
                    string input = Console.ReadLine().Trim();
                    int option;
                    if (string.IsNullOrEmpty(input))
                        continue;
                    else
                    {
                        if (!int.TryParse(input, out option))
                        {
                            error = $"Wrong input. '{input}' is not a valid number.";
                            continue;
                        }
                        
                    }

                    switch (option)
                    {
                        case 0:
                            stop = true;
                            break;
                        case 1:
                            getOption(1).Run();
                            break;
                        case 2:
                            getOption(2).Run();
                            break;
                        case 3:
                            getOption(3).Run();
                            break;
                        case 4:
                            getOption(4).Run();
                            break;
                        default:
                            error = $"Wrong input. '{input}' is not an option.";
                            break;
                    }
                } while (!stop);
            }

            /// <summary>
            /// Gets the textual representation of the menu
            /// </summary>
            /// <returns>A string with a welcome message, instructions and the list of options</returns>
            public string GetMenu()
            {
                string header = "Welcome to the main menu." +
                    "\nTo test the functions, write the corresponding number.\n\n";

                string menu = "";

                for (int i = 0; i < options.Count; i++)
                {
                    var option = options[i];
                    if (i > 0)
                    {
                        menu += "\n";
                    }
                    menu += $"{i} - {option}";
                }

                return header + menu;
            }

            /// <summary>
            ///  Creates a new instance of <c>Option</c> with no functionality
            /// </summary>
            /// <returns>The exit option</returns>
            protected Option CreateExit()
            {
                return new Option("Exit", null);
            }

            /// <summary>
            ///  Creates a new instance of <c>Option</c> that calculates prices based on age
            /// </summary>
            /// <returns>The Ticket Price by Age option</returns>
            protected Option CreateTicketPriceByAge()
            {
                void Run()
                {

                    int age = GetInt("age", out string input, true);

                    if (!string.IsNullOrEmpty(input))
                    {
                        var (type, price) = GetPriceByAge(age);
                        Console.WriteLine($"{type}:{price:N0}kr");
                    }

                }

                return new Option("Ticket Price by Age", Run);
            }

            /// <summary>
            ///  Creates a new instance of <c>Option</c> that calculate prices based on number of people and their age
            /// </summary>
            /// <returns>The Ticket Price by Number of People and Age option</returns>
            protected Option CreateTicketPriceByQuantityAndAge()
            {
                void Run()
                {
                    string input;
                    int qty = GetInt("number of people", out input, true);

                    if (string.IsNullOrEmpty(input))
                    {
                        return;
                    }

                    decimal totalPrice = 0;

                    for (int i = 0; i < qty; i++)
                    {
                        int age = GetInt($"age {i+1}", out input, false);

                        if (!string.IsNullOrEmpty(input))
                        {
                            var (type, price) = GetPriceByAge(age);
                            totalPrice += price;
                        }

                    }

                   
                    Console.WriteLine($"\nTotal number of people: {qty}\nTotal price: {totalPrice:N0}kr");

                }

                return new Option("Ticket Price by Number of People and Age", Run);
            }

            /// <summary>
            ///  Creates a new instance of <c>Option</c> which shows you that magic really exists by repeating any input 10 times in one line
            /// </summary>
            /// <returns>The Repeat 10 Times option</returns>
            protected Option CreateRepeat10Times()
            {
                void Run()
                {
                    Console.Write("Write something: ");
                    string input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        return;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        if (i > 0)
                            Console.Write($", {input}");
                        else
                            Console.Write(input);
                    }
                    Console.WriteLine();

                }

                return new Option("Repeat 10 Times", Run);
            }

            /// <summary>
            ///  Creates a new instance of <c>Option</c> which extracts the third word of a sentece
            /// </summary>
            /// <returns>The Third Word option</returns>
            protected Option CreateTheThirdWord()
            {
                void Run()
                {
                    var sentence = GetAtLeast3Words("Write sentence", out string input, true);

                    if (string.IsNullOrEmpty(input))
                        return;

                    Console.WriteLine($"Third Word: {sentence[2]}");
                }

                return new Option("The Third Word", Run);
            }

            /// <summary>
            /// Gets user input and repeats the request until the input is succesfully parsed to <c>int</c>
            /// </summary>
            /// <param name="name">the meaning of the number to request</param>
            /// <param name="input">the original input of the user</param>
            /// <param name="stopIfEmpty">true if the loop stops when the user input is empty</param>
            /// <returns>a number entered by the user</returns>
            protected static int GetInt(string name, out string input, bool stopIfEmpty)
            {
                bool error = false;
                int age = 0;
                input = "";
                do
                {
                    if (error)
                    {
                        Console.WriteLine($"Error: '{input}' is not a valid number.\n");
                    }
                    Console.Write($"Enter {name}: ");

                    input = Console.ReadLine().Trim();
                    if (string.IsNullOrEmpty(input))
                    {
                        if (stopIfEmpty)
                        {
                            break;
                        }
                    }

                    error = !int.TryParse(input, out age);

                } while (error);

                return age;
            }

            /// <summary>
            /// Gets user input and repeats the request until there are at least 3 words
            /// </summary>
            /// <param name="inputDescription">the expected input</param>
            /// <param name="input">the original input of the user</param>
            /// <param name="stopIfEmpty">true if the loop stops when the user input is empty</param>
            /// <returns>a string array that containing the words of a sentence</returns>
            protected static string[]? GetAtLeast3Words(string inputDescription, out string input, bool stopIfEmpty)
            {
                bool error = false;
                input = "";
                string[]? sentence = null;
                do
                {
                    if (error)
                    {
                        Console.WriteLine($"Too short. Write at least 3 words.\n");
                    }
                    Console.Write($"{inputDescription}: ");
                    input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        if (stopIfEmpty)
                            break; 
                    }

                    sentence = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    error = sentence.Length < 3;


                } while (error);

                return sentence;
            }

            /// <summary>
            /// Gets the price and description corresponding a speceific age range
            /// </summary>
            /// <param name="age">the age of the client</param>
            /// <returns>a tuple containing a short description of the price and the price itself</returns>
            protected static Tuple<string, decimal> GetPriceByAge(int age)
            {
                if (age < 20)
                {
                    return new("Ungdomspris", 80M);
                }
                else if (age > 64)
                {
                    return new("Pensionär", 90M);
                }
                else
                {
                    return new("Standarpris", 120M);
                }
            }

            /// <summary>
            /// Class <c>Option</c> models an option with functionality
            /// </summary>
            public class Option
            {
                private readonly Action? run;

                /// <summary>
                /// initializes a new instance of <c>MyConsoleMenu</c> with 5 default options
                /// </summary>
                /// <param name="name">a name that describes the pupose of the option</param>
                /// <param name="run">the action that the option will execute</param>
                public Option(string name, Action? run)
                {
                    Name = name;
                    this.run = run;
                }

                public string Name { get; set; }

                /// <summary>
                /// Executes watever. If arguments are necessary, it'll come as input from the user through console
                /// </summary>
                public void Run()
                {
                    ExitLoop();
                }

                /// <summary>
                /// A loop that excecutes the action of the option.  Once over, it asks the user to continue or exit the option
                /// </summary>
                protected void ExitLoop()
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine(this+"\n");
                        run();
                        Console.WriteLine("\nPress Enter to continue or anything else to go back to the menu.");
                        ConsoleKeyInfo cki = Console.ReadKey(true);

                        if (cki.Key != ConsoleKey.Enter)
                            break;

                    } while (true);
                    Console.Clear();
                }

                /// <summary>
                /// Just sends the name of the option
                /// </summary>
                /// <returns>The name of the option</returns>
                public override string ToString()
                {
                    return Name;
                }
            }

        }

    }
}
