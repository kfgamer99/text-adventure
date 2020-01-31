using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_KF
{
    class Program
    {

        //This is the current working project

        static Dictionary<int, string> book = new Dictionary<int, string>();
        static string pageContents;
        static string[] pageContentsSplit;
        static string input = "";
        static string menuinput = "";
        static int page;
        static bool gameOver = false;
        static bool madeachoice = false;
        static bool exitgame = false;
        static bool menuchoicemade = false;
        static int storybooknumberassigner = 1;
        static int storylength = 0;
        static int dilimetercount = 0;

        static void Main(string[] args) ///////////////////////////////////////////////////////////////////////////////////////////////////////start of main
        {

            //check files before game even starts
            //error checking is broken, fix this later
            //StoryFileCheck();
            //SaveFileCheck();


            while (exitgame == false) //entire program loop
            {
                MainMenu(); //reading save file (main menu)
                LoadStory();

                //start of game loop
                while (gameOver == false)
                {
                    //split the story into pieces
                    pageContents = book[page]; //the page's contents is the current story page's string
                    pageContentsSplit = pageContents.Split(';');
                    //prints out the story text/options/pages
                    DisplayStory();
                    //if the page the player is on IS an ending (will work even if length of story is changed)
                    if (pageContentsSplit[1] == "" & pageContentsSplit[2] == "") //checks to see if there are no options, story ends
                    {
                        gameOver = true;
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    }
                    else
                    {
                        //if the page the player is currently on is NOT an ending
                        ChoiceMade();
                        if (input == "A")
                        {
                            page = Int32.Parse(pageContentsSplit[3]); //changes the page number to continue to that page
                            Console.WriteLine("You choose to >" + pageContentsSplit[1] + ".");
                        }
                        else if (input == "B")
                        {
                            page = Int32.Parse(pageContentsSplit[4]); //changes the page number to continue to that page
                            Console.WriteLine("You choose to >" + pageContentsSplit[2] + ".");
                        }
                        madeachoice = false; //resets the player choice loop
                    }
                    Console.Clear();
                }//end of game loop



                continue;
            } //end of exitgame

            Console.ReadKey(true);
        }//end of main ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //method to get player input
        static void ChoiceMade() //gets player input
        {

            while (madeachoice == false) //loops until the player makes a valid choice(a or b) || User input check
            {
                input = Console.ReadLine();
                input = input.ToUpper(); //makes the letter uppercase
                if (input == "A" | input == "B") //u=if the input is a or b, the game continues
                {
                    madeachoice = true;
                }
                else if (input == "X")
                {
                    SaveGame();
                }
                else //if the player makes an invalid input the loop will continue until the player makes a valid choice
                {
                    Console.WriteLine("Invalid input. Please try again");
                    continue;
                }
            }

        } //end of choice made method





        static void MainMenu()
        {
            menuinput = "";
            menuchoicemade = false;
            Console.WriteLine("Text Adventure Game");
            Console.WriteLine("A Program made by Kailey Fitzgerald");
            Console.WriteLine();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("A: New Game");
            Console.WriteLine("B: Continue Game");
            Console.WriteLine("C: Exit Game");
            Console.WriteLine();
            Console.WriteLine("~Warning: Creating a new game will overwrite any previous save files.");

            while (menuchoicemade == false)
            {
                menuinput = Console.ReadLine();
                menuinput = menuinput.ToUpper();
                if (menuinput == "A") //new game
                {
                    StoryFileCheck();
                    SaveFileCheck();
                    page = 1;
                    menuchoicemade = true;
                    gameOver = false;
                }
                else if (menuinput == "B") //continue game
                {
                    StoryFileCheck();
                    SaveFileCheck();
                    page = Int32.Parse(System.IO.File.ReadAllText("savegame.txt"));
                    menuchoicemade = true;
                    gameOver = false;
                }
                else if (menuinput == "C")  //exit game
                {
                    menuchoicemade = true;
                    exitgame = true;
                    gameOver = true;
                }
                else //loops until player makes a valid option
                {
                    Console.WriteLine("Please input a valid option");
                }
            }
            Console.Clear();
        } //menu choice end


        static void SaveGame() //save game start
        {
            System.IO.File.WriteAllText("savegame.txt", page.ToString());
            Console.WriteLine("Game Saved");
        }//save game end




        static void LoadStory() //turns the story file into a dictionary to remedy the issue of 'page 0'
        {
            foreach (string line in System.IO.File.ReadAllLines("story.txt"))
            {
                book.Add(storybooknumberassigner, line);
                storybooknumberassigner = storybooknumberassigner + 1;
            }
        }



        //method to display the story
        static void DisplayStory()
        {
            Console.WriteLine(pageContentsSplit[0]); //plot text
            Console.WriteLine(pageContentsSplit[1]); //option 1
            Console.WriteLine(pageContentsSplit[2]); //option 2
            if (pageContentsSplit[3] != "" & pageContentsSplit[4] != "")
            {
                Console.WriteLine("(X) Save Game");
            }
            // Console.WriteLine(pageContentsSplit[3]); //page 1
            //  Console.WriteLine(pageContentsSplit[4]); //page 2
            Console.WriteLine();
        } //end of method 'displaystory'



        //File error checking//////////////////

        static void StoryFileCheck()
        {
            //check if story file exists first
            if (System.IO.File.Exists("story.txt"))
            {
                //Console.WriteLine("Story file exists (all good)");
            }
            else
            { //if the story file doesn't exist, the game will quit
                Environment.Exit(0);

            }
            //end check file


            //check if files are blank / if the file is blank, the game will quit.
            string filecontents = System.IO.File.ReadAllText("story.txt");
            if (filecontents == "")
            {
                Console.WriteLine("Error: story.txt is empty. Exiting...");
                exitgame = true;
                gameOver = true;
            }
            else //if not blank
            {
                //Console.WriteLine("Story file not blank (all good)");
            }
            //end check for blank



            //check if too few elements
            foreach (string storyelement in System.IO.File.ReadAllLines("story.txt"))
            {


                foreach (char letter in storyelement)
                {
                    if (Char.ToString(letter) == ";")
                    {
                        dilimetercount = dilimetercount + 1;
                    }
                }

                if (dilimetercount > 4)
                {
                    Console.WriteLine("Error: story.txt has too many elements. Exiting...");
                    exitgame = true;
                }
                else if (dilimetercount < 4)
                {
                    Console.WriteLine("Error: story.txt has too few elements. Exiting...");
                    exitgame = true;
                }
                dilimetercount = 0; //reset for next line
            }




        }//end of storyfile check




        //start savefile check
        static void SaveFileCheck()
        {
            //check if exists
            if (System.IO.File.Exists("savegame.txt"))
            {
                //  Console.WriteLine("Save File Exists");
            }
            else
            {
                Console.WriteLine("Error: Save file does not exist. Resetting to page 1...");
                System.IO.File.WriteAllText("savegame.txt", "1"); //resets to page 1 
            }
            //end  exist check

            //first, check if the value is not a number
            int test;
            bool isnumber = Int32.TryParse(System.IO.File.ReadAllText("savegame.txt"), out test);
            if (isnumber)
            {
                //Console.WriteLine("Save file loaded");
            }
            else
            {
                Console.WriteLine("Save file corrupted, resetting to default...");
                System.IO.File.WriteAllText("savegame.txt", "1");
                Console.WriteLine("Save file reset.");
            }





            //check if blank
            string savecontents = System.IO.File.ReadAllText("savegame.txt");
            if (savecontents == "")
            {
                Console.WriteLine("Error: Save file is blank. Resetting to page 1...");
                System.IO.File.WriteAllText("savegame.txt", "1"); //resets to page 1 
            }
            //end blank check

            //save page / story length comparision
            int savepage = Int32.Parse(System.IO.File.ReadAllText("savegame.txt"));
            foreach (string linesavepage in System.IO.File.ReadAllLines("story.txt"))
            {
                storylength = storylength + 1;
            }
            if (savepage > storylength)
            {
                Console.WriteLine("Error: savegame value out of bounds of story length. Resetting to page 1...");
                System.IO.File.WriteAllText("savegame.txt", "1"); //resets to page 1
            }


            //if the save file number is negative
            if (Int32.Parse(System.IO.File.ReadAllText("savegame.txt")) < 1)
            {
                Console.WriteLine("Error: savegame value cannot be a negative number. Resetting to page 1...");
                System.IO.File.WriteAllText("savegame.txt", "1"); //resets to page 1
            }

        }//end savefilecheck

        //file error checking end //////////////////////


    }//end of methods
}

