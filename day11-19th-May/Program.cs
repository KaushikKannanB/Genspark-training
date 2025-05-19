// using System;

// 1) create a program that will take name from user and greet the user
class Program
{
    public static void Main(string[] args)
    {
        string ? name = Console.ReadLine();

        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("Please enter a name!");
        }
        else
        {
            Greet(name);
        }

    }
    public static void Greet(string? name)
    {
        Console.WriteLine("Welcome "+name);
    }
}


//2 Take 2 numbers from user and print the largest

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter Number 1:");

        int input1 = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter Number 2:");

        int input2 = int.Parse(Console.ReadLine());

        if (input1 > input2)
        {
            Console.WriteLine($"greatest {input1}");
        }
        else
            Console.WriteLine($"greatest {input2}");

    }
}


// 3) Take 2 numbers from user, check the operation user wants to perform (+,-,*,/). Do the operation and print the result
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter Number 1:");
        int input1 = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter Number 2:");
        int input2 = int.Parse(Console.ReadLine());

        Console.WriteLine("Enter operation: ");
        string operation = Console.ReadLine();

        int val = Calc(input1, input2, operation);

        Console.WriteLine($"{val} is the answer");
    }
    public static int Calc(int n1, int n2, string op)
    {
        switch (op)
        {
            case "+":
                return n1 + n2;
            case "-":
                return n1 - n2;
            case "*":
                return n1 * n2;
            case "/":
                return n1 / n2;
        }
        return -404;
    }
}

// 4) Take username and password from user. Check if user name is "Admin" and password is "pass" if yes then print success message.
// Give 3 attempts to user. In the end of eh 3rd attempt if user still is unable to provide valid creds then exit the application after print the message 
// "Invalid attempts for 3 times. Exiting...."

class Program
{
    public static void Main(string[] args)
    {
        int wrong_attempts = 0;

        while (wrong_attempts < 3)
        {
            Console.Write("Enter username: ");
            string ? name = Console.ReadLine();
            Console.Write("Enter password: ");
            string ? pass = Console.ReadLine();

            if (Check(name, pass))
            {
                Console.WriteLine("Successful Login!");
                return;
            }
            else
            {
                Console.WriteLine("Unsuccessful Login!");
                wrong_attempts++;
            }
        }

        Console.WriteLine("Exiting application");
    }
    public static bool Check(string name, string pass)
    {
        return name == "Admin" && pass == "pass";
    }
}

// 5) Take 10 numbers from user and print the number of numbers that are divisible by 7
class Program
{
    public static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
        int n;
        Console.WriteLine("Enter Numbers: ");
        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine("Enter a Number: ");
            n = Check()

            if (Divisible(n))
                numbers.Add(n);
        }
        Console.WriteLine("Divisible by 7:")
        foreach (int j in numbers)
        {
            Console.WriteLine($"{j}")
        }
    }
    public static bool Divisible(int num)
    {
        return j % 7 == 0;
    }
    public static int Check()
    {
    int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number");
        }
        return num;
    }
}


// 6) Count the Frequency of Each Element
// Given an array, count the frequency of each element and print the result.
// Input: {1, 2, 2, 3, 4, 4, 4}

// output
// 1 occurs 1 times  
// 2 occurs 2 times  
// 3 occurs 1 times  
// 4 occurs 3 times

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the size of the Array");
        int size = Check();

        int []arr = new int[size];

        for (int i = 0; i < size; i++)
        {
            Console.WriteLine("Enter a number");
            arr[i] = Check();
        }

        Frequency(arr);
    }
    public static void Frequency(int[] arr)
    {
        Dictionary<int, int> freq = new Dictionary<int, int>();

        foreach (int i in arr)
        {
            if (freq.ContainsKey(i))
            {
                freq[i]++;
            }
            else
                freq[i] = 1;
        }

        foreach (var pair in freq)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }
    }
    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number please");
        }
        return num;
    }
}

// 7) create a program to rotate the array to the left by one position.
// Input: {10, 20, 30, 40, 50}
// Output: {20, 30, 40, 50, 10}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the size of the Array");
        int size = Check();

        Console.WriteLine("Enter the Rotation size");
        int rotation = Check();
        rotation = rotation % size;

        int[] arr = new int[size];

        for (int i = 0; i < size; i++)
        {
            Console.WriteLine("Enter a number");
            arr[i] = Check();
        }

        int[] newarray = Rotate(arr, size, rotation);

        Console.WriteLine("Rotated Array:");
        foreach (int num in newarray)
        {
            Console.Write(num + " ");
        }
    }

    public static int[] Rotate(int[] arr, int size, int rotation)
    {
        int[] result = new int[size];

        for (int i = 0; i < size; i++)
        {
            int newPos = (i + size - rotation) % size;  
            result[newPos] = arr[i];
        }

        return result;
    }

    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number please");
        }
        return num;
    }
}


// 8) Given two integer arrays, merge them into a single array.
// Input: {1, 3, 5} and {2, 4, 6}
// Output: {1, 3, 5, 2, 4, 6}

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the size of the Array");
        int size = Check();

        int[] arr1 = new int[size];
        int[] arr2 = new int[size];
        Console.WriteLine("Enter the Array 1 elements");
        for (int i = 0; i < size; i++)
        {
            Console.WriteLine("Enter a number");
            arr1[i] = Check();
        }

        Console.WriteLine("Enter the Array 2 elements");
        for (int i = 0; i < size; i++)
        {
            Console.WriteLine("Enter a number");
            arr2[i] = Check();
        }
        int[] newarray = Concat(arr1, arr2, size);
        foreach (int num in newarray)
        {
            Console.Write(num + " ");
        }

    }
    public static int[] Concat(int[] arr1, int[] arr2, int size)
    {
        int[] newarray = new int[2 * size];
        int m = 0;
        for (int i = 0; i < size; i++)
        {
            newarray[m++] = arr1[i];
        }
        for (int i = 0; i < size; i++)
        {
            newarray[m++] = arr2[i];
        }
        return newarray;
    }
    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number please");
        }
        return num;
    }
}

// 9) Write a program that:

// Has a predefined secret word (e.g., "GAME").

// Accepts user input as a 4-letter word guess.

// Compares the guess to the secret word and outputs:

// X Bulls: number of letters in the correct position.

// Y Cows: number of correct letters in the wrong position.

// Continues until the user gets 4 Bulls (i.e., correct guess).

// Displays the number of attempts.

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Word to be guessed:");
        string word = Check();
        string guess;
        int x = 0, y = 0;
        bool iscorrect = false;
        int attempts = 0;
        while (!iscorrect)
        {
            attempts++;
            Console.WriteLine("Guess the 4 letter word!");
            guess = Check();
            x = Bull(word, guess);
            y = Cow(word, guess);

            Console.WriteLine($"{x} Bulls");
            Console.WriteLine($"{y} Cows");

            if (x == 4)
            {
                iscorrect = true;
            }
            else
                iscorrect = false;
        }

        Console.WriteLine($"You got it right in {attempts} attempts");
    }
    public static int Bull(string word, string guess)
    {
        int bulls = 0;
        for (int i = 0; i < guess.Length; i++)
        {
            if (word[i] == guess[i])
            {
                bulls++;
            }
        }
        return bulls;
    }
    public static int Cow(string word, string guess)
    {
        int cows = 0;
        for (int i = 0; i < guess.Length; i++)
        {
            if (word.Contains(guess[i]))
            {
                if(word[i]!=guess[i])
                    cows++;
            }
        }
        return cows;
    }
    public static string Check()
    {
        string? word = Console.ReadLine();
        while (word.Length != 4 || string.IsNullOrEmpty(word))
        {
            Console.WriteLine("Enter a valid word");
        }
        return word;
    }
}

// 10) write a program that accepts a 9-element array representing a Sudoku row.

// Validates if the row:

// Has all numbers from 1 to 9.

// Has no duplicates.

// Displays if the row is valid or invalid.

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Sudoko haan!, Enter your sudoku as a 9-element array, representing a sudoku row");
        int[] row = new int[9];
        for (int i = 0; i < 9; i++)
        {
            row[i] = Check();
        }
        if (ValidRow(row))
            Console.WriteLine($"The given row is a Valid Sudoku Row");
        else
            Console.WriteLine($"The given row is an Invalid Sudoku Row");

    }
    public static bool ValidRow(int[] arr)
    {
        Dictionary<int, int> rowfreq = new Dictionary<int, int>();

        foreach (int i in arr)
        {
            if (rowfreq.ContainsKey(i))
                return false;
            else
                rowfreq[i] = 1;
        }
        return true;

    }
    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number please");
        }
        return num;
    }
}

//  11) In the question ten extend it to validate a sudoku game. 
// Validate all 9 rows (use int[,] board = new int[9,9])

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Sudoko haan!, Enter your sudoku as a 9-element array, representing a sudoku row");
        int[,] game = new int[9, 9];
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                game[i,j] = Check();
            }
        }

        Valid sudoku
        int[,] game = {
            {5,3,4,6,7,8,9,1,2},
            {6,7,2,1,9,5,3,4,8},
            {1,9,8,3,4,2,5,6,7},
            {8,5,9,7,6,1,4,2,3},
            {4,2,6,8,5,3,7,9,1},
            {7,1,3,9,2,4,8,5,6},
            {9,6,1,5,3,7,2,8,4},
            {2,8,7,4,1,9,6,3,5},
            {3,4,5,2,8,6,1,7,9}
        };

        Invalid sudoku
        int[,] game = {
            {5,3,5,6,7,8,9,1,2}, 
            {6,3,2,1,9,5,3,4,8}, 
            {1,9,8,3,4,2,5,6,7},
            {8,5,9,7,6,1,4,2,3},
            {4,2,6,8,5,3,7,9,1},
            {7,1,3,9,2,4,8,5,6},
            {9,6,1,5,3,7,2,8,4},
            {2,8,7,4,1,9,6,3,5},
            {3,4,5,2,8,6,1,7,9}
        };
        if (CheckRowsandCols(game))
            Console.WriteLine($"You actually WON!");
        else
            Console.WriteLine($"You Lose, please try again!");

    }
    public static bool CheckRowsandCols(int[,] game)
    {
        int[] array = new int[9];
        int m = 0;
        for (int i = 0; i < 9; i++)
        {
            m = 0;
            for (int j = 0; j < 9; j++)
            {
                array[m++] = game[i, j];
            }
            if (!Valid(array))
            {
                return false;
            }
        }

        for (int i = 0; i < 9; i++)
        {
            m = 0;
            for (int j = 0; j < 9; j++)
            {
                array[m++] = game[j, i];
            }
            if (!Valid(array))
            {
                return false;
            }
        }

        for (int i = 0; i < 9; i=i+3)
        {
            for (int j = 0; j < 9; j = j + 3)
            {
                m = 0;
                for (int k = i; k < i + 3; k++)
                {
                    for (int l = j; l < j + 3; l++)
                    {
                        array[m++] = game[k, l];
                    }
                }
                if (!Valid(array))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static bool Valid(int[] arr)
    {
        Dictionary<int, int> rowfreq = new Dictionary<int, int>();

        foreach (int i in arr)
        {
            if (rowfreq.ContainsKey(i))
                return false;
            else
            {
                if (i <= 9 && i > 0)
                    rowfreq[i] = 1;
                else
                    return false;
            }

        }
        return true;

    }
    
    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number please");
        }
        return num;
    }
}

// 12) Write a program that:

// Takes a message string as input (only lowercase letters, no spaces or symbols).

// Encrypts it by shifting each character forward by 3 places in the alphabet.

// Decrypts it back to the original message by shifting backward by 3.

// Handles wrap-around, e.g., 'z' becomes 'c'.

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter the word for encryption: ");
        string word = Check();
        Console.WriteLine("Number of positions to be moved");
        int num = NCheck();
        string encrypted = Encryption(word, num);
        Console.WriteLine($"Encrypted string is {encrypted}");
        Console.WriteLine($"Decrypted string is {Decryption(encrypted,num)}");
    }
    public static string Encryption(string word, int num)
    {
        char[] characters = word.ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = (char)(((characters[i] - 'a' + num) % 26)+'a');
        }
        return new string(characters);
    }
    public static string Decryption(string word, int num)
    {
        char[] characters = word.ToCharArray();
        for (int i = 0; i < characters.Length; i++)
        {
            characters[i] = (char)(((characters[i] - 'a' - num+26) % 26)+'a');
        }
        return new string(characters);
    }
    public static string Check()
    {
        string? word = Console.ReadLine();
        while (string.IsNullOrEmpty(word))
        {
            Console.WriteLine("Enter a valid word");
        }
        return word.ToLower();
    }
    public static int NCheck()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Enter a valid number please");
        }
        return num;
    }
}