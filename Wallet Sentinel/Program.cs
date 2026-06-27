using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using BetterConsoleTables;
using System.Media;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Collections;
using System.Globalization;

namespace Wallet_Sentinel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.WindowHeight = Console.LargestWindowHeight;
            BillReminderFunction reminderfunction = new BillReminderFunction();
            SavingsGoalsManager goalsManager = new SavingsGoalsManager();
            AverageExpensesCalculator aveExpensesCalc = new AverageExpensesCalculator("", 0);
            aveExpensesCalc.LoadAndAddCurrentMonthGoalsAndBills();
            char menuchoice = 'Y';
            while (menuchoice == 'Y')
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(",   .     . .     .     ,-.          .             .");
                    Console.WriteLine("| . |     | |     |    (   `         |   o         |");
                    Console.WriteLine("| ) ) ,-: | | ,-. |-    `-.  ,-. ;-. |-  . ;-. ,-. |");
                    Console.WriteLine("|/|/  | | | | |-' |    .   ) |-' | | |   | | | |-' |");
                    Console.WriteLine("' '   `-` ' ' `-' `-'   `-'  `-' ' ' `-' ' ' ' `-' '");
                    for (int i = 0; i < 2; i++) { Console.WriteLine(); }
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    reminderfunction.LoadRemindersFromFile("bill_reminders.txt");
                    reminderfunction.DisplayClosestDueDates();
                    for (int i = 0; i < 2; i++) { Console.WriteLine(); }
                    goalsManager.LoadGoalsFromFile("goals.txt");
                    goalsManager.DisplayGoalsBySmallestTargetAmount();
                    for (int i = 0; i < 2; i++) { Console.WriteLine(); }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Please select a choice");
                    for (int i = 0; i < 2; i++) { Console.WriteLine(); }
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("[1] - Bill Payment Reminders");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("[2] - Saving Goals");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("[3] - Currency Conversion");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[4] - Average Expenses Calculator");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("[5] - Change Ringtone");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[6] - Exit");
                    for (int i = 0; i < 2; i++) { Console.WriteLine(); }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Your choice: ");
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            int choiceforreminders;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("Bill Payment Reminders");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("1 - Create Reminders");
                            Console.WriteLine("2 - Edit Reminders");
                            Console.WriteLine("3 - Delete Reminders");
                            Console.WriteLine("4 - Delete All Reminders");
                            Console.WriteLine("5 - Display All Reminders");
                            Console.WriteLine("6 - Exit");
                            Console.WriteLine();
                            Console.Write("Enter choice: ");
                            choiceforreminders = int.Parse(Console.ReadLine());
                            switch (choiceforreminders)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.Write("Enter the number of bill payment reminders: ");
                                    int numberofReminders = int.Parse(Console.ReadLine());
                                    if (numberofReminders > 0)
                                    {
                                        for (int i = 0; i < numberofReminders; i++)
                                        {
                                            Console.WriteLine();
                                            Console.WriteLine($"Enter details for Reminder {i + 1}");
                                            Console.Write("Bill Name: ");
                                            string billname = Console.ReadLine();
                                            DateTime duedate;
                                            bool validDueDate = false;
                                            do
                                            {
                                                Console.Write("Due Date (yyyy-MM-dd): ");
                                                if (DateTime.TryParse(Console.ReadLine(), out duedate))
                                                {
                                                    if (duedate >= DateTime.Today)
                                                    {
                                                        validDueDate = true;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Due date should not be in the past.");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                                                }
                                            } while (!validDueDate);
                                            Console.Write("Amount: Php ");
                                            double amount;
                                            do
                                            {
                                                Console.Write("Amount: Php ");
                                                if (!double.TryParse(Console.ReadLine(), out amount))
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Invalid input. Please enter a valid amount.");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                                else if (amount <= 0)
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Invalid amount input. Please enter a valid amount.");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                                else
                                                {
                                                    BillPaymentReminder reminder = new BillPaymentReminder(billname, duedate, amount);
                                                    reminderfunction.Add(reminder);
                                                    Console.Clear();
                                                    Console.WriteLine("Reminder added successfully.");
                                                }
                                            } while (amount <= 0);

                                        }
                                        Console.Clear();
                                        Console.WriteLine("All reminders Saved");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please enter a valid number.");
                                    }
                                    reminderfunction.SaveRemindersToFile("bill_reminders.txt");
                                    break;
                                case 2:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    reminderfunction.LoadRemindersFromFile("bill_reminders.txt");
                                    var remindersTable = reminderfunction.DisplayAllRemindersTable();
                                    Console.WriteLine(remindersTable.ToString());
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.WriteLine("Edit Reminders");
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.WriteLine();
                                    Console.Write("Enter the bill name you want to edit: ");
                                    string billNameToEdit = Console.ReadLine();
                                    Console.Write("Enter the new due date: ");
                                    DateTime newDueDate;
                                    while (!DateTime.TryParse(Console.ReadLine(), out newDueDate) || newDueDate < DateTime.Today)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid due date. Please enter a future date.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("Enter the new due date (yyyy-MM-dd): ");
                                    }
                                    Console.Write("Enter the new amount: Php ");
                                    double newAmount;
                                    while (!double.TryParse(Console.ReadLine(), out newAmount) || newAmount <= 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid amount. Please enter a valid amount.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                        Console.Write("Enter the new amount: Php ");
                                    }
                                    reminderfunction.EditReminder(billNameToEdit, newDueDate, newAmount);
                                    reminderfunction.SaveRemindersToFile("bill_reminders.txt");
                                    break;
                                case 3:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    reminderfunction.LoadRemindersFromFile("bill_reminders.txt");
                                    remindersTable = reminderfunction.DisplayAllRemindersTable();
                                    Console.WriteLine(remindersTable.ToString());
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine("Delete Reminders");
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("Enter the bill name you want to delete: ");
                                    string billNameToDelete = Console.ReadLine();
                                    reminderfunction.DeleteReminder(billNameToDelete);
                                    reminderfunction.SaveRemindersToFile("bill_reminders.txt");
                                    break;
                                case 4:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    reminderfunction.LoadRemindersFromFile("bill_reminders.txt");
                                    remindersTable = reminderfunction.DisplayAllRemindersTable();
                                    Console.WriteLine(remindersTable.ToString());
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine("Delete Reminders");
                                    Console.WriteLine();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("You want to delete all reminders?(Y/N): ");
                                    char choicetodeleteall = char.Parse(Console.ReadLine().ToUpper());
                                    if (choicetodeleteall == 'Y')
                                    {
                                        reminderfunction.DeleteAllReminders();
                                        reminderfunction.SaveRemindersToFile("bill_reminders.txt");
                                    }
                                    else if (choicetodeleteall == 'N')
                                    {
                                        Console.WriteLine("Nothing Change");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong input");
                                    }
                                    break;
                                case 5:
                                    Console.Clear();
                                    reminderfunction.LoadRemindersFromFile("bill_reminders.txt");
                                    remindersTable = reminderfunction.DisplayAllRemindersTable();
                                    Console.WriteLine(remindersTable.ToString());
                                    break;
                                case 6:
                                    break;
                                default:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input");
                                    Console.WriteLine("Press Enter to go back to main menu");
                                    Console.ReadLine();
                                    break;
                            }
                            break;
                        case 2:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Saving Goals");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("1 - Set Saving Goals");
                            Console.WriteLine("2 - Edit Saving Goal");
                            Console.WriteLine("3 - Delete Saving Goal");
                            Console.WriteLine("4 - Delete All Goals");
                            Console.WriteLine("5 - Display Saving Goals");
                            Console.WriteLine("6 - Track Progress");
                            Console.WriteLine("7 - Exit");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("Enter a choice: ");
                            int monthlyGoalChoice = int.Parse(Console.ReadLine());
                            switch (monthlyGoalChoice)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.Write("Enter number of goals to be created: ");
                                    int numberofgoals = int.Parse(Console.ReadLine());
                                    if (numberofgoals > 0)
                                    {
                                        for (int j = 0; j < numberofgoals; j++)
                                        {
                                            Console.Write("Enter the goal name: ");
                                            string goalName = Console.ReadLine();
                                            do
                                            {
                                                Console.Write("Enter the target amount: Php ");
                                                if (double.TryParse(Console.ReadLine(), out double targetAmount) && targetAmount > 0)
                                                {
                                                    SavingGoals goal = new SavingGoals(goalName, targetAmount);
                                                    goalsManager.AddGoal(goal);
                                                    Console.WriteLine("Goal added successfully.");
                                                    goalsManager.SaveGoalsToFile("goals.txt");
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Invalid amount input. Please enter a valid amount.");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                            }while (true);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input. Please enter a valid number.");
                                    }
                                    break;
                                case 2:
                                    Console.Clear();
                                    goalsManager.SaveGoalsToFile("goals.txt");
                                    var goalsTable = goalsManager.GenerateGoalsTable();
                                    Console.WriteLine(goalsTable.ToString());
                                    Console.WriteLine();
                                    Console.Write("Enter the goal name to edit: ");
                                    string goalNameToEdit = Console.ReadLine();

                                    SavingGoals goalToEdit = goalsManager.GetGoal(goalNameToEdit);

                                    if (goalToEdit != null)
                                    {
                                        if (goalToEdit.CompletionDate.HasValue)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                            Console.WriteLine("This goal is already completed and cannot be edited.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                        else
                                        {
                                            do
                                            {
                                                Console.Write("Enter the new target amount: Php ");
                                                if (double.TryParse(Console.ReadLine(), out double newTargetAmount) && newTargetAmount > 0)
                                                {
                                                    goalsManager.EditGoal(goalNameToEdit, newTargetAmount);
                                                    goalsManager.SaveGoalsToFile("goals.txt");
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.ForegroundColor = ConsoleColor.Red;
                                                    Console.WriteLine("Invalid amount input. Please enter a valid positive value.");
                                                    Console.ForegroundColor = ConsoleColor.White;
                                                }
                                            } while (true);
                                        }
                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"Goal with name '{goalNameToEdit}' not found.");
                                        Console.ForegroundColor = ConsoleColor.White;
                                    }
                                    break;
                                case 3:
                                    Console.Clear();
                                    goalsManager.SaveGoalsToFile("goals.txt");
                                    goalsTable = goalsManager.GenerateGoalsTable();
                                    Console.WriteLine(goalsTable.ToString());
                                    Console.WriteLine();
                                    Console.Write("Enter the goal name to delete: ");
                                    string goalNameToDelete = Console.ReadLine();
                                    goalsManager.DeleteGoal(goalNameToDelete);
                                    goalsManager.SaveGoalsToFile("goals.txt");
                                    break;
                                case 4:
                                    Console.Clear();
                                    goalsManager.LoadGoalsFromFile("goals.txt");
                                    goalsTable = goalsManager.GenerateGoalsTable();
                                    Console.WriteLine(goalsTable.ToString());
                                    Console.Write("Do you want to delete all goals? (Y/N): ");
                                    char choiceToDeleteAllGoals = char.Parse(Console.ReadLine().ToUpper());
                                    if (choiceToDeleteAllGoals == 'Y')
                                    {
                                        goalsManager.DeleteAllGoals();
                                        goalsManager.SaveGoalsToFile("goals.txt");
                                    }
                                    else if (choiceToDeleteAllGoals == 'N')
                                    {
                                        Console.WriteLine("No goals were deleted.");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid choice.");
                                    }
                                    break;
                                case 5:
                                    Console.Clear();
                                    goalsManager.LoadGoalsFromFile("goals.txt");
                                    goalsTable = goalsManager.GenerateGoalsTable();
                                    Console.WriteLine(goalsTable.ToString());
                                    break;
                                case 6:
                                    Console.Clear();
                                    goalsManager.LoadGoalsFromFile("goals.txt");
                                    goalsTable = goalsManager.GenerateGoalsTable();
                                    Console.WriteLine(goalsTable.ToString());
                                    Console.Write("Enter the goal name to track progress: ");
                                    string goalNameToTrack = Console.ReadLine();
                                    var goalToTrack = goalsManager.GetGoal(goalNameToTrack);
                                    if (goalToTrack != null)
                                    {
                                        if (goalToTrack.GetProgressPercentage() == 100)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                            Console.WriteLine("Congratulations! The goal has already been achieved. No additional amount can be added.");
                                            Console.ForegroundColor = ConsoleColor.White;
                                        }
                                        else
                                        {
                                            double remainingAmount = goalToTrack.TargetAmount - goalToTrack.SavedAmount;

                                            if (remainingAmount == 0)
                                            {
                                                goalToTrack.UpdateProgress(remainingAmount);
                                                goalsManager.MarkGoalCompleted(goalToTrack);
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("The goal has already been achieved. No additional amount can be added.");
                                                Console.ForegroundColor = ConsoleColor.White;
                                            }
                                            else
                                            {
                                                Console.WriteLine($"Remaining amount needed to achieve the goal: Php {remainingAmount:F2}");

                                                double amountSaved;
                                                do
                                                {
                                                    Console.Write("Enter the amount saved: Php ");
                                                    if (double.TryParse(Console.ReadLine(), out amountSaved) && amountSaved > 0)
                                                    {
                                                        if (amountSaved > remainingAmount)
                                                        {
                                                            Console.ForegroundColor = ConsoleColor.Red;
                                                            Console.WriteLine($"Amount saved cannot exceed the remaining amount needed to achieve the goal (Php {remainingAmount:F2}).");
                                                            Console.ForegroundColor = ConsoleColor.White;
                                                        }
                                                        else
                                                        {
                                                            goalToTrack.UpdateProgress(amountSaved);
                                                            goalsManager.MarkGoalCompleted(goalToTrack);
                                                            goalsManager.SaveGoalsToFile("goals.txt");
                                                            Console.WriteLine("Progress updated successfully.");
                                                            break;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Console.ForegroundColor = ConsoleColor.Red;
                                                        Console.WriteLine("Invalid amount. Please enter a valid amount.");
                                                        Console.ForegroundColor = ConsoleColor.White;
                                                    }
                                                } while (true);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Goal not found.");
                                    }
                                    break;
                                case 7:
                                    break;
                                default:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Invalid input");
                                    Console.WriteLine("Press Enter to go back to main menu");
                                    Console.ReadLine();
                                    break;
                            }
                            break;
                        case 3:
                            CurrencyConverter currencyconverter;
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Currency Conversion");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine("[1] - Php to USD, EUR, JPY, GBP");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("[2] - USD, EUR, JPY, GBP to Php");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("[3] - Exit");
                            Console.WriteLine();
                            Console.Write("Enter Choice: ");
                            int converterchoice = int.Parse(Console.ReadLine());
                            if (converterchoice == 1)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                Console.WriteLine("Php to USD, EUR, JPY, GBP");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("1 - USD, 2 - EUR, 3 - JPY, 4 - GBP, 5 - Exit");
                                Console.WriteLine();
                                Console.Write("Enter a Choice: ");
                                int choiceofcurrency = int.Parse(Console.ReadLine());
                                Console.WriteLine();
                                switch (choiceofcurrency)
                                {
                                    case 1:
                                        Console.Clear();
                                        Console.WriteLine("Php to USD");
                                        currencyconverter = new ConvertPhpToOthercurrencies();
                                        Console.Write("Enter Currency in Php: ");
                                        double currencyinPhp = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in Php: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinPhp) || currencyinPhp <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"USD {currencyconverter.USD(currencyinPhp):F2}");
                                        break;
                                    case 2:
                                        Console.Clear();
                                        Console.WriteLine("Php to EUR");
                                        currencyconverter = new ConvertPhpToOthercurrencies();
                                        Console.Write("Enter Currency in Php: ");
                                        currencyinPhp = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in Php: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinPhp) || currencyinPhp <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"EUR {currencyconverter.EUR(currencyinPhp):F2}");
                                        break;
                                    case 3:
                                        Console.Clear();
                                        Console.WriteLine("Php to JPY");
                                        currencyconverter = new ConvertPhpToOthercurrencies();
                                        Console.Write("Enter Currency in Php: ");
                                        currencyinPhp = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in Php: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinPhp) || currencyinPhp <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"JPY {currencyconverter.JPY(currencyinPhp):F2}");
                                        break;
                                    case 4:
                                        Console.Clear();
                                        Console.WriteLine("Php to GBP");
                                        currencyconverter = new ConvertPhpToOthercurrencies();
                                        Console.Write("Enter Currency in Php: ");
                                        currencyinPhp = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in Php: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinPhp) || currencyinPhp <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"GBP {currencyconverter.GBP(currencyinPhp):F2}");
                                        break;
                                    case 5:
                                        break;
                                    default:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid choice");
                                        break;
                                }
                            }
                            else if (converterchoice == 2)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("USD, EUR, JPY, GBP to Php");
                                Console.WriteLine();
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("1 - USD, 2 - EUR, 3 - JPY, 4 - GBP, 5 - Exit");
                                Console.WriteLine();
                                Console.Write("Enter a Choice: ");
                                int choiceofcurrency = int.Parse(Console.ReadLine());
                                switch (choiceofcurrency)
                                {
                                    case 1:
                                        Console.Clear();
                                        Console.WriteLine("USD to Php");
                                        currencyconverter = new ConvertOthercurrenciesToPhp();
                                        Console.Write("Enter Currency in USD: ");
                                        double currencyinUSD = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in USD: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinUSD) || currencyinUSD <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"PHP {currencyconverter.USD(currencyinUSD):F2}");
                                        break;
                                    case 2:
                                        Console.Clear();
                                        Console.WriteLine("EUR to Php");
                                        currencyconverter = new ConvertOthercurrenciesToPhp();
                                        Console.Write("Enter Currency in EUR: ");
                                        double currencyinEUR = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in EUR: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinEUR) || currencyinEUR <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"PHP {currencyconverter.EUR(currencyinEUR):F2}");
                                        break;
                                    case 3:
                                        Console.Clear();
                                        Console.WriteLine("JPY to Php");
                                        currencyconverter = new ConvertOthercurrenciesToPhp();
                                        Console.Write("Enter Currency in JPY: ");
                                        double currencyinJPY = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in JPY: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinJPY) || currencyinJPY <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"PHP {currencyconverter.JPY(currencyinJPY):F2}");
                                        break;
                                    case 4:
                                        Console.Clear();
                                        Console.WriteLine("GBP to Php");
                                        currencyconverter = new ConvertOthercurrenciesToPhp();
                                        Console.Write("Enter Currency in GBP: ");
                                        double currencyinGBP = double.Parse(Console.ReadLine());
                                        do
                                        {
                                            Console.Write("Enter Currency in GBP: ");
                                        } while (!double.TryParse(Console.ReadLine(), out currencyinGBP) || currencyinGBP <= 0);
                                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        Console.WriteLine($"PHP {currencyconverter.GBP(currencyinGBP):F2}");
                                        break;
                                    case 5:
                                        break;
                                    default:
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine("Invalid Choice");
                                        break;
                                }
                            }
                            else if (converterchoice == 3)
                            {
                                break;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("Invalid Choice");
                            }
                            break;
                        case 4:
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("Average Expenses Calculator");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("1 - Add Expenses");
                            Console.WriteLine("2 - Edit Expenses");
                            Console.WriteLine("3 - Delete Expenses");
                            Console.WriteLine("4 - Delete All Expenses");
                            Console.WriteLine("5 - Display Average Expenses");
                            Console.WriteLine("6 - Exit to main menu");
                            Console.Write("Enter Choice: ");
                            int averageExpensesChoice = int.Parse(Console.ReadLine());
                            switch (averageExpensesChoice)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.Write("Enter the number of expenses to add: ");
                                    int numberOfExpenses = int.Parse(Console.ReadLine());
                                    for (int i = 0; i < numberOfExpenses; i++)
                                    {
                                        Console.Clear();
                                        Console.Write("Enter expense description: ");
                                        string description = Console.ReadLine();
                                        double amount;
                                        do
                                        {
                                            Console.Write("Enter expense amount: ");
                                        } while (!double.TryParse(Console.ReadLine(), out amount) || amount <= 0);
                                        Console.Write("Enter expense date (yyyy-MM-dd): ");
                                        DateTime expenseDate = DateTime.Parse(Console.ReadLine());
                                        aveExpensesCalc.AddExpense(description, amount, expenseDate);
                                    }
                                    aveExpensesCalc.SaveExpensesToFile();
                                    break;
                                case 2:
                                    Console.Clear();
                                    aveExpensesCalc.DisplayExpenses();
                                    Console.WriteLine();
                                    while (true)
                                    {
                                        Console.Write("Enter the description of the expense to edit: ");
                                        string expenseDescriptionToEdit = Console.ReadLine();
                                        if (string.IsNullOrEmpty(expenseDescriptionToEdit))
                                        {
                                            Console.WriteLine("Invalid input. Description cannot be empty.");
                                            continue;
                                        }
                                        aveExpensesCalc.EditExpense(expenseDescriptionToEdit);
                                        break;
                                    }
                                    aveExpensesCalc.SaveExpensesToFile();
                                    break;
                                case 3:
                                    Console.Clear();
                                    aveExpensesCalc.DisplayExpenses();
                                    Console.Write("Enter expense description to delete: ");
                                    string descriptionToDelete = Console.ReadLine();
                                    aveExpensesCalc.DeleteExpense(descriptionToDelete);
                                    aveExpensesCalc.SaveExpensesToFile();
                                    break;
                                case 4:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    aveExpensesCalc.DisplayExpenses();
                                    Console.ForegroundColor = ConsoleColor.White;
                                    Console.Write("Do you want to Delete All Expenses?[Y/N]: ");
                                    char deleteallExpensesChoice = char.Parse(Console.ReadLine().ToUpper());
                                    if(deleteallExpensesChoice == 'Y')
                                    {
                                        aveExpensesCalc.DeleteAllExpenses();
                                        aveExpensesCalc.SaveExpensesToFile();
                                    }
                                    else if (deleteallExpensesChoice == 'N')
                                    {
                                        Console.WriteLine("Nothing Happend");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Choice");
                                    }
                                    break;
                                case 5:
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    aveExpensesCalc.DisplayExpenses();
                                    break;
                                case 6:
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice.");
                                    break;
                            }
                            break;
                        case 5:
                            while(true)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.Blue;
                                Console.WriteLine("Change Ringtone");
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine("1 - Change Ringtone");
                                Console.WriteLine("2 - Exit");
                                Console.Write("Enter Choice: ");
                                int choicechangeringtone = int.Parse(Console.ReadLine());
                                if (choicechangeringtone == 1)
                                {
                                    Console.Clear();
                                    reminderfunction.ChangeReminderSound();
                                    Console.ReadLine();
                                }
                                else if (choicechangeringtone == 2)
                                {
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("Invalid Choice");
                                    Console.ReadLine();
                                }
                            }
                            break;
                        case 6:
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("Exiting Program....");
                            Console.WriteLine();
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Clear();
                            Console.ForegroundColor= ConsoleColor.Red;
                            Console.WriteLine("Invalid input");
                            break;
                    }
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Do you want to go back to main menu? (Y/N): ");
                    menuchoice = char.Parse(Console.ReadLine().ToUpper());
                    Console.Clear();
                }
                catch(FormatException message)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {message.Message}");
                    Console.WriteLine("Press Enter to go back to main menu");
                    Console.ReadLine();
                    Console.Clear();
                }
            }
            Console.WriteLine();
            Console.WriteLine("Exiting Program...");
            Console.WriteLine();
            Environment.Exit(0);
        }
    }
    public interface IBillPaymentReminder
    {
        string BillName { get; set; }
        DateTime DueDate { get; set; }
        double Amount { get; set; }
        DateTime CalculateNextDueDate();
    }
    public class BillPaymentReminder : IBillPaymentReminder
    {
        public string BillName { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public BillPaymentReminder(string billname, DateTime duedate, double amount)
        {
            BillName = billname;
            DueDate = duedate;
            Amount = amount;
        }
        public DateTime CalculateNextDueDate()
        {
            return DueDate.AddMonths(1);
        }
    }
    class BillReminderFunction : List<BillPaymentReminder>
    {
        public void SortRemindersByDueDate()
        {
            this.Sort((r1, r2) => r1.DueDate.CompareTo(r2.DueDate));
        }
        public void DisplayClosestDueDates()
        {
            SortRemindersByDueDate();
            Console.WriteLine("Bill Payment Reminders:");
            var table = new Table("Bill Name", "Due Date", "Amount");
            for (int i = 0; i < Math.Min(3, this.Count); i++)
            {
                var reminder = this[i];
                table.AddRow(reminder.BillName, reminder.DueDate.ToString("yyyy-MM-dd"), $"Php {reminder.Amount:F2}");
                TimeSpan timeUntilDue = reminder.DueDate - DateTime.Now;
                if (timeUntilDue.TotalDays <= 1)
                {
                    PlayReminderSound();
                }
            }
            Console.WriteLine(table.ToString());
        }
        public Table DisplayAllRemindersTable()
        {
            SortRemindersByDueDate();
            var table = new Table("Bill Name", "Due Date", "Amount");
            foreach (var reminder in this)
            {
                table.AddRow(reminder.BillName, reminder.DueDate.ToString("yyyy-MM-dd"), $"Php {reminder.Amount:F2}");
            }
            return table;
        }
        public void EditReminder(string billName, DateTime newDueDate, double newAmount)
        {
            var reminderToEdit = this.FirstOrDefault(r => r.BillName == billName);
            if (reminderToEdit != null)
            {
                reminderToEdit.DueDate = newDueDate;
                reminderToEdit.Amount = newAmount;
                Console.WriteLine("Reminder edited successfully.");
            }
            else
            {
                Console.WriteLine("Reminder not found.");
            }
        }
        public void DeleteReminder(string billName)
        {
            var reminderToDelete = this.FirstOrDefault(r => r.BillName == billName);
            if (reminderToDelete != null)
            {
                this.Remove(reminderToDelete);
                Console.WriteLine("Reminder deleted successfully.");
            }
            else
            {
                Console.WriteLine("Reminder not found.");
            }
        }
        public void DeleteAllReminders()
        {
            this.Clear();
            Console.WriteLine("All reminders deleted successfully. You nuke it!");
        }
        public void SaveRemindersToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var reminder in this)
                {
                    writer.WriteLine($"{reminder.BillName},{reminder.DueDate},{reminder.Amount}");
                }
            }
        }
        public void LoadRemindersFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                Clear();
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3)
                        {
                            string billName = parts[0];
                            DateTime dueDate = DateTime.Parse(parts[1]);
                            double amount = double.Parse(parts[2]);
                            Add(new BillPaymentReminder(billName, dueDate, amount));
                        }
                    }
                }
            }
        }
        public void ChangeReminderSound()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Enter the new file path for the reminder sound: ");
            string newAudioFilePath = Console.ReadLine();
            try
            {
                if (System.IO.File.Exists(newAudioFilePath))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Reminder sound file path updated successfully.");
                    SetReminderSoundFilePath(newAudioFilePath);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("File does not exist. Reminder sound file path not updated.");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error changing reminder sound: {ex.Message}");;
            }
        }
        private void SetReminderSoundFilePath(string newFilePath)
        {
            try
            {
                string configFilePath = "reminder_sound_config.txt";
                System.IO.File.WriteAllText(configFilePath, newFilePath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Reminder sound file path saved to the configuration file.");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error saving new reminder sound file path: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
        }
        private string GetReminderSoundFilePath()
        {
            string defaultFilePath = "W:\\Documents\\BSCpE\\CpE261\\Codes\\Wallet Sentinel\\chat_wheel_2018_ta_daaaa_5F4s06B.wav";

            try
            {
                string configFilePath = "reminder_sound_config.txt";
                if (System.IO.File.Exists(configFilePath))
                {
                    return System.IO.File.ReadAllText(configFilePath);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Configuration file not found. Using default audio file path.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    return defaultFilePath;
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error reading reminder sound file path: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                return defaultFilePath;
            }
        }
        public void PlayReminderSound()
        {
            string audioFilePath = GetReminderSoundFilePath();
            try
            {
                if (System.IO.File.Exists(audioFilePath))
                {
                    SoundPlayer player = new SoundPlayer(audioFilePath);
                    player.Play();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Audio file not found. Playing default sound.");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    SoundPlayer defaultPlayer = new SoundPlayer("W:\\Documents\\BSCpE\\CpE261\\Codes\\Wallet Sentinel\\chat_wheel_2018_ta_daaaa_5F4s06B.wav");
                    defaultPlayer.Play();
                }
            }
            catch (Exception message)
            { 
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error playing reminder sound: {message.Message}");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
        }
        public List<BillPaymentReminder> GetRemindersForCurrentMonth()
        {
            return this.Where(reminder => reminder.DueDate.Month == DateTime.Now.Month && reminder.DueDate.Year == DateTime.Now.Year).ToList();
        }
    }
    public class SavingGoals
    {
        public string GoalName { get; set; }
        public double TargetAmount { get; set; }
        public double SavedAmount { get; set; }
        public DateTime? CompletionDate { get; set; }
        public SavingGoals(string goalname, double targetAmount)
        {
            GoalName = goalname;
            TargetAmount = targetAmount;
            SavedAmount = 0;
            CompletionDate = null;
        }
        public void UpdateProgress(double amountSaved)
        {
            SavedAmount += amountSaved;
        }
        public double GetProgressPercentage()
        {
            return (SavedAmount / TargetAmount) * 100;
        }
    }
    class SavingsGoalsManager
    {
        private List<SavingGoals> goals = new List<SavingGoals>();
        private List<SavingGoals> SavingGoals = new List<SavingGoals>();
        private SavingsGoalsManager goalsManager;
        private AverageExpensesCalculator expensesCalc;
        public void SortGoalsBySmallestTargetAmount()
        {
            SavingGoals.Sort((g1, g2) => g1.TargetAmount.CompareTo(g2.TargetAmount));
        }
        public Table GenerateGoalsTable()
        {
            SortGoalsBySmallestTargetAmount();
            var table = new Table("Goal Name", "Target Amount", "Saved Amount", "Progress", "Completion Date");
            foreach (var goal in SavingGoals)
            {
                table.AddRow(
                    goal.GoalName,
                    $"Php {goal.TargetAmount:F2}",
                    $"Php {goal.SavedAmount:F2}",
                    $"{goal.GetProgressPercentage():F2}%",
                    goal.CompletionDate.HasValue ? goal.CompletionDate.Value.ToString("yyyy-MM-dd") : "Not Completed"
                );
            }
            return table;
        }
        public void AddGoal(SavingGoals goal)
        {
            SavingGoals.Add(goal);
        }
        public void MarkGoalCompleted(SavingGoals goalToMark)
        {
            if (goalToMark != null)
            {
                if (goalToMark.SavedAmount == goalToMark.TargetAmount)
                {
                    goalToMark.CompletionDate = DateTime.Now;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Goal '{goalToMark.GoalName}' marked as completed.");
                    SaveGoalsToFile("goals.txt");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Goal '{goalToMark.GoalName}' cannot be marked as completed. Saved amount is less than the target amount.");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("Goal not found.");
            }
        }
        public void EditGoal(string name, double newTargetAmount)
        {
            var goalToEdit = SavingGoals.FirstOrDefault(goal => goal.GoalName == name);
            if (goalToEdit != null)
            {
                DateTime? originalCompletionDate = goalToEdit.CompletionDate;
                goalToEdit.TargetAmount = newTargetAmount;
                goalToEdit.CompletionDate = originalCompletionDate;
                Console.WriteLine("Goal edited successfully.");
            }
            else
            {
                Console.WriteLine("Goal not found.");
            }
        }
        public void DeleteGoal(string name)
        {
            var goalToDelete = SavingGoals.FirstOrDefault(goal => goal.GoalName == name);
            if (goalToDelete != null)
            {
                SavingGoals.Remove(goalToDelete);
                Console.WriteLine("Goal deleted successfully.");
                if (goalToDelete.CompletionDate.HasValue)
                {
                    Console.WriteLine($"Goal Completion Date: {goalToDelete.CompletionDate.Value.ToString("yyyy-MM-dd")}");
                }
            }
            else
            {
                Console.WriteLine("Goal not found.");
            }
        }
        public void DeleteAllGoals()
        {
            if (SavingGoals.Count > 0)
            {
                Console.WriteLine("Deleted Goals:");
                foreach (var goal in SavingGoals)
                {
                    Console.WriteLine($"- {goal.GoalName}");
                    if (goal.CompletionDate.HasValue)
                    {
                        Console.WriteLine($"  Completion Date: {goal.CompletionDate.Value.ToString("yyyy-MM-dd")}");
                    }
                }
                SavingGoals.Clear();
                Console.WriteLine("All goals deleted successfully. You nuked it!");
            }
            else
            {
                Console.WriteLine("No goals found to delete.");
            }
        }
        public void DisplayGoalsBySmallestTargetAmount()
        {
            SortGoalsBySmallestTargetAmount();
            var table = new Table("Goal Name", "Target Amount", "Saved Amount", "Progress", "Completion Date");

            foreach (var goal in SavingGoals)
            {
                string completionDate = goal.CompletionDate.HasValue
                    ? goal.CompletionDate.Value.ToString("yyyy-MM-dd")
                    : "Not Completed";
                table.AddRow(
                    goal.GoalName,
                    $"Php {goal.TargetAmount:F2}",
                    $"Php {goal.SavedAmount:F2}",
                    $"{goal.GetProgressPercentage():F2}%",
                    completionDate
                );
            }

            Console.WriteLine("Goals Sorted by Smallest Target Amount:");
            Console.WriteLine(table.ToString());
        }
        public SavingGoals GetGoal(string goalName)
        {
            return SavingGoals.FirstOrDefault(goal => goal.GoalName == goalName);
        }
        public void SaveGoalsToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (var goal in SavingGoals)
                {
                    string completionDate = goal.CompletionDate.HasValue
                        ? goal.CompletionDate.Value.ToString("yyyy-MM-dd")
                        : "Not Completed";

                    writer.WriteLine($"{goal.GoalName},{goal.TargetAmount},{goal.SavedAmount},{completionDate}");
                }
            }
        }
        public void LoadGoalsFromFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                SavingGoals.Clear();
                using (StreamReader reader = new StreamReader(fileName))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            string name = parts[0];
                            double targetAmount = double.Parse(parts[1]);
                            double savedAmount = double.Parse(parts[2]);
                            string completionDateString = parts[3];

                            SavingGoals goal = new SavingGoals(name, targetAmount);
                            goal.SavedAmount = savedAmount;

                            if (completionDateString != "Not Completed")
                            {
                                DateTime completionDate = DateTime.ParseExact(completionDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                goal.CompletionDate = completionDate;
                            }

                            AddGoal(goal);
                        }
                    }
                }
            }
        }
        public List<SavingGoals> GetGoalsForCurrentMonth()
        {
            return SavingGoals.Where(goal => goal.CompletionDate.HasValue && goal.CompletionDate.Value.Month == DateTime.Now.Month && goal.CompletionDate.Value.Year == DateTime.Now.Year).ToList();
        }
    }
    public abstract class CurrencyConverter
    {
        protected double PHP2USDrate = 56.8095;
        protected double PHP2EURrate = 59.7210;
        protected double PHP2JPYrate = 0.3798;
        protected double PHP2GBPrate = 68.8758;
        public abstract double USD(double Php);
        public abstract double EUR(double Php);
        public abstract double JPY(double Php);
        public abstract double GBP(double Php);
    }
    public class ConvertPhpToOthercurrencies : CurrencyConverter
    {
        public override double USD(double Php)
        { return (Php / PHP2USDrate); }
        public override double EUR(double Php)
        { return (Php / PHP2EURrate); }
        public override double JPY(double Php)
        { return (Php / PHP2JPYrate); }
        public override double GBP(double Php)
        { return (Php / PHP2GBPrate); }
    }
    public class ConvertOthercurrenciesToPhp : CurrencyConverter
    {
        public override double USD(double Php)
        { return (Php * PHP2USDrate); }
        public override double EUR(double Php)
        { return (Php * PHP2EURrate); }
        public override double JPY(double Php)
        { return (Php * PHP2JPYrate); }
        public override double GBP(double Php)
        { return (Php * PHP2GBPrate); }
    }
    public class Expense
    {
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public Expense(string description, double amount, DateTime date)
        {
            Description = description;
            Amount = amount;
            Date = date;
        }
    }
    public class AverageExpensesCalculator : SavingGoals, IBillPaymentReminder
    {
        public List<Expense> expenses = new List<Expense>();
        private SavingsGoalsManager goalsManager = new SavingsGoalsManager();
        private BillReminderFunction billPaymentReminders = new BillReminderFunction();
        private string expensesFileName = "expenses.csv";
        private string goalsFileName = "goals.txt";
        private string billRemindersFileName = "bill_reminders.txt";
        public string BillName { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public DateTime CalculateNextDueDate()
        {
            return DueDate.AddMonths(1);
        }
        public AverageExpensesCalculator(string goalname, double targetAmount) : base(goalname, targetAmount)
        {
            LoadData();
        }
        public void AddExpense(string description, double amount, DateTime date)
        {
            if (!expenses.Any(e => e.Description == description && e.Date == date))
            {
                Expense expense = new Expense(description, amount, date);
                expenses.Add(expense);
                SaveExpensesToFile();
                Console.WriteLine("Expense added successfully!");
            }
            else
            {
                Console.WriteLine("Expense with the same description and date already exists. Not added.");
            }
        }
        public void DeleteExpense(string expenseDescription)
        {
            Expense expenseToDelete = expenses.FirstOrDefault(expense => expense.Description == expenseDescription);
            if (expenseToDelete != null)
            {
                expenses.Remove(expenseToDelete);
                SaveExpensesToFile();
                Console.WriteLine($"Expense '{expenseDescription}' deleted successfully!");
            }
            else
            {
                Console.WriteLine($"Expense with description '{expenseDescription}' not found.");
            }
        }
        public void DeleteAllExpenses()
        {
            if (expenses.Count > 0)
            {
                Console.WriteLine("Deleted Expenses:");
                foreach (var expense in expenses)
                {
                    Console.WriteLine($"- {expense.Description}");
                    Console.WriteLine($"  Amount: {expense.Amount:F2}");
                    Console.WriteLine($"  Date: {expense.Date.ToString("yyyy-MM-dd")}");
                }
                expenses.Clear();
                SaveExpensesToFile();
                Console.WriteLine("All expenses deleted successfully. You nuked it!");
            }
            else
            {
                Console.WriteLine("No expenses found to delete.");
            }
        }
        public void DisplayExpenses()
        {
            var table = GenerateExpensesTable();
            Console.WriteLine("Expenses:");
            Console.WriteLine(table.ToString());
            double averageExpenses = CalculateAverageMonthlyExpenses();
            Console.WriteLine($"Average Monthly Expenses: Php {averageExpenses:F2}");
        }
        protected Table GenerateExpensesTable()
        {
            var table = new Table("Description", "Amount", "Date");
            foreach (var expense in expenses)
            {
                table.AddRow(
                    expense.Description,
                    $"Php {expense.Amount:F2}",
                    expense.Date.ToString("yyyy-MM-dd")
                );
            }
            return table;
        }
        protected double CalculateAverageMonthlyExpenses()
        {
            var currentMonthExpenses = expenses
                .Where(expense => expense.Date.Month == DateTime.Now.Month && expense.Date.Year == DateTime.Now.Year)
                .Sum(expense => expense.Amount);
            return currentMonthExpenses / DateTime.Now.Day;
        }
        private void LoadData()
        {
            LoadBillPaymentReminders();
            LoadCompletedGoals();
            LoadExpensesFromFile();
        }
        public void EditExpense(string expenseDescription)
        {
            Expense expenseToEdit = expenses.FirstOrDefault(expense => expense.Description == expenseDescription);
            if (expenseToEdit != null)
            {
                Console.WriteLine($"Editing expense '{expenseDescription}':");
                Console.Write($"Current Amount: {expenseToEdit.Amount}. Enter new amount: ");
                double newAmount;
                while (!double.TryParse(Console.ReadLine(), out newAmount) || newAmount <= 0)
                {
                    Console.Write("Invalid input. Enter a valid amount: ");
                }
                expenseToEdit.Amount = newAmount;
                Console.Write($"Current Date: {expenseToEdit.Date}. Enter new date (yyyy-MM-dd): ");
                DateTime newDate;
                while (!DateTime.TryParse(Console.ReadLine(), out newDate))
                {
                    Console.Write("Invalid input. Enter a valid date (yyyy-MM-dd): ");
                }
                expenseToEdit.Date = newDate;
                SaveData();
                Console.WriteLine("Expense edited successfully!");
            }
            else
            {
                Console.WriteLine($"Expense with description '{expenseDescription}' not found.");
            }
        }
        protected void SaveData()
        {
            SaveExpensesToFile();
        }
        public void LoadBillPaymentReminders()
        {
            string remindersFileName = "bill_reminders.txt";
            BillReminderFunction billPaymentReminders = new BillReminderFunction();
            billPaymentReminders.LoadRemindersFromFile(remindersFileName);
        }
        public void LoadCompletedGoals()
        {
            string goalsFileName = "goals.txt";
            SavingsGoalsManager completedGoals = new SavingsGoalsManager();
            completedGoals.LoadGoalsFromFile(goalsFileName);
        }
        private void LoadExpensesFromFile()
        {
            List<Expense> loadedExpenses = new List<Expense>();
            try
            {
                using (StreamReader reader = new StreamReader(expensesFileName))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] values = line.Split(',');

                        if (values.Length == 3)
                        {
                            string description = values[0];
                            double amount = double.Parse(values[1]);
                            DateTime date = DateTime.ParseExact(values[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            Expense expense = new Expense(description, amount, date);
                            loadedExpenses.Add(expense);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from file: {ex.Message}");
            }
            expenses = loadedExpenses;
        }
        public void SaveExpensesToFile()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(expensesFileName))
                {
                    foreach (var expense in expenses)
                    {
                        writer.WriteLine($"{expense.Description},{expense.Amount},{expense.Date.ToString("yyyy-MM-dd")}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expenses to file: {ex.Message}");
            }
        }
        public void LoadAndAddCurrentMonthGoalsAndBills()
        {
            string goalsFileName = "goals.txt";
            goalsManager.LoadGoalsFromFile(goalsFileName);
            List<SavingGoals> currentMonthGoals = goalsManager.GetGoalsForCurrentMonth();
            string remindersFileName = "bill_reminders.txt";
            billPaymentReminders.LoadRemindersFromFile(remindersFileName);
            List<BillPaymentReminder> currentMonthBills = billPaymentReminders.GetRemindersForCurrentMonth();
            List<Expense> currentMonthExpenses = new List<Expense>();
            foreach (var goal in currentMonthGoals)
            {
                if (IsGoalInCurrentMonth(goal))
                {
                    currentMonthExpenses.Add(new Expense($"Goal: {goal.GoalName}", goal.TargetAmount, goal.CompletionDate.Value));
                }
            }
            foreach (var bill in currentMonthBills)
            {
                if (IsBillInCurrentMonth(bill))
                {
                    currentMonthExpenses.Add(new Expense($"Bill: {bill.BillName}", bill.Amount, bill.DueDate));
                }
            }
            expenses.AddRange(currentMonthExpenses);
            SaveExpensesToFile();
        }
        private bool IsGoalInCurrentMonth(SavingGoals goal)
        {
            return goal.CompletionDate.HasValue && goal.CompletionDate.Value.Month == DateTime.Now.Month && goal.CompletionDate.Value.Year == DateTime.Now.Year;
        }
        private bool IsBillInCurrentMonth(BillPaymentReminder bill)
        {
            return bill.DueDate.Month == DateTime.Now.Month && bill.DueDate.Year == DateTime.Now.Year;
        }
    }
}