// 1) Design a C# console app that uses a jagged array to store data for Instagram posts by multiple users. Each user can have a different number of posts, 
// and each post stores a caption and number of likes.

// You have N users, and each user can have M posts (varies per user).

// Each post has:

// A caption (string)

// A number of likes (int)

// Store this in a jagged array, where each index represents one user's list of posts.

// Display all posts grouped by user.

// No file/database needed — console input/output only.

// Example output
// Enter number of users: 2

// User 1: How many posts? 2
// Enter caption for post 1: Sunset at beach
// Enter likes: 150
// Enter caption for post 2: Coffee time
// Enter likes: 89

// User 2: How many posts? 1
// Enter caption for post 1: Hiking adventure
// Enter likes: 230

// --- Displaying Instagram Posts ---
// User 1:
// Post 1 - Caption: Sunset at beach | Likes: 150
// Post 2 - Caption: Coffee time | Likes: 89

// User 2:
// Post 1 - Caption: Hiking adventure | Likes: 230


// Test case
// | User | Number of Posts | Post Captions        | Likes      |
// | ---- | --------------- | -------------------- | ---------- |
// | 1    | 2               | "Lunch", "Road Trip" | 40, 120    |
// | 2    | 1               | "Workout"            | 75         |
// | 3    | 3               | "Book", "Tea", "Cat" | 30, 15, 60 |

using System;
// class Program
// {
//     public static void Main(string[] args)
//     {
//         int[][] likes = new int[100][]; // int jagged array [user][postLikes];
//         string[][] captions = new string[100][]; // string jagged array[user][postCaptions]
//         int user_num;
//         Console.WriteLine("Welcome to Instagram...not really, anyways, Press 1 to enter posts, Press 2 to view posts, press any other to exit the application");
//         while (true)
//         {
//             Console.WriteLine("Enter option");
//             int option = Check();
//             switch (option)
//             {
//                 case 1:
//                     Console.WriteLine("User Number: (0-99)");
//                     user_num = Check();
//                     Console.WriteLine("Enter number of posts: ");
//                     int number_of_posts = Check();
//                     captions[user_num] = new string[number_of_posts]; //initialse internal array with size;
//                     likes[user_num] = new int[number_of_posts];
//                     for (int i = 0; i < number_of_posts; i++)
//                     {
//                         Console.WriteLine($"Post {i + 1}:");
//                         captions = EnterCaption(captions, user_num, i);
//                         likes = EnterLikes(likes, user_num, i);
//                         Console.WriteLine("Post added!!!\n");
//                     }
//                     break;
//                 case 2:
//                     Console.WriteLine("User Number: ");
//                     user_num = Check();
//                     if (user_num < 0 || user_num > 99)
//                     {
//                         Console.WriteLine("User number between 0,99");
//                         break;
//                     }
//                     ViewPosts(captions, likes, user_num);
//                     break;
//                 default:
//                     Console.WriteLine("Exiting the console Insta, BYEEEE!");
//                     return;
//             }
//         }

//     }
//     public static int[][] EnterLikes(int[][] likes, int user, int postnum)
//     {
//         Console.WriteLine("Enter Likes");
//         int num_likes = Check();
//         likes[user][postnum] = num_likes;
//         return likes;
//     }
//     public static string[][] EnterCaption(string[][] caption, int user, int postnum)
//     {
//         Console.WriteLine("Enter Caption:");
//         string caption_line = StringCheck();
//         caption[user][postnum] = caption_line;
//         return caption;

//     }
//     public static void ViewPosts(string[][] caption, int[][] likes, int user)
//     {
//         if (caption[user] == null || likes[user] == null) // check for null
//         {
//             Console.WriteLine("No posts under this user");
//             return;
//         }
//         for (int i = 0; i < caption[user].Length; i++)
//         {
//             Console.WriteLine($"Post: {i} -> Likes: {likes[user][i]} --> Caption: {caption[user][i]}");
//         }
//     }
//     public static int Check()
//     {
//         int num;
//         while (!int.TryParse(Console.ReadLine(), out num))
//         {
//             Console.WriteLine("Please enter a Valid Number");
//         }
//         return num;
//     }
//     public static string StringCheck()
//     {
//         string cap = "";
//         while (string.IsNullOrEmpty(cap))
//         {
//             Console.WriteLine("Enter a valid caption");
//             cap = Console.ReadLine();
//         }
//         return cap;
//     }
// }

// Key Learnings on jagged Arrays: 
// What? - Array of Arrays
// Size of Jagged array should be initialized first itself - fixed Size
// Internal arrays are initialized to null when jagged array is declared
// we have initialse the internal array before actually using them. or populating them

// instead of creating two jagged arrays we can also use a class+single jagged array

class Post
{
    public string caption { get; set; }
    public int likes { get; set; }
}
class Program
{
    public static void Main(string[] args)
    {
        Post[][] insta = new Post[100][];
        int user_num;
        Console.WriteLine("Welcome to Instagram...not really, anyways, Press 1 to enter posts, Press 2 to view posts, press any other to exit the application");
        while (true)
        {
            Console.WriteLine("Enter option");
            int option = Check();
            switch (option)
            {
                case 1:
                    Console.WriteLine("User Number: (0-99)");
                    user_num = Check();
                    Console.WriteLine("Enter number of posts: ");
                    int number_of_posts = Check();
                    insta[user_num] = new Post[number_of_posts]; //initialse internal array with size;

                    for (int i = 0; i < number_of_posts; i++)
                    {
                        insta[user_num][i] = new Post();
                        Console.WriteLine($"Post {i + 1}:");
                        Console.WriteLine("Enter Caption:");
                        insta[user_num][i].caption = StringCheck();
                        Console.WriteLine("Enter Likes");
                        insta[user_num][i].likes = Check();
                        Console.WriteLine("Post added!!!\n");
                    }
                    break;
                case 2:
                    Console.WriteLine("User Number: ");
                    user_num = Check();
                    if (user_num < 0 || user_num > 99)
                    {
                        Console.WriteLine("User number between 0,99");
                        break;
                    }
                    if (insta[user_num] == null) // check for null
                    {
                        Console.WriteLine("No posts under this user");
                        break;
                    }
                    for (int i = 0; i < insta[user_num].Length; i++)
                    {
                        Console.WriteLine($"Post: {i + 1} -> Likes: {insta[user_num][i].likes} --> Caption: {insta[user_num][i].caption}");
                    }
                    break;
                default:
                    Console.WriteLine("Exiting the console Insta, BYEEEE!");
                    return;
            }
        }

    }
    public static int Check()
    {
        int num;
        while (!int.TryParse(Console.ReadLine(), out num))
        {
            Console.WriteLine("Please enter a Valid Number");
        }
        return num;
    }
    public static string StringCheck()
    {
        string cap = "";
        while (string.IsNullOrEmpty(cap))
        {
            Console.WriteLine("Enter a valid caption");
            cap = Console.ReadLine();
        }
        return cap;
    }
}


