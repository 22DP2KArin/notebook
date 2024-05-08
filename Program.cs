using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
    static void Main()
    {
        UserManagement userManagement = new UserManagement();

        // Ielādē lietotājus no faila, ja tas pastāv
        if (File.Exists("users.txt"))
        {
            userManagement.LoadUsers("users.txt");
        }

        User currentUser = null;
        while (currentUser == null)
        {
            Console.WriteLine("Lūdzu, pieteikties vai reģistrēties:");
            Console.WriteLine("+---------------------------------------+");
            Console.WriteLine("|           - Pieteikties (1)           |");
            Console.WriteLine("|           - Reģistrēties (2)          |");
            Console.WriteLine("+---------------------------------------+");
            int choice = GetUserChoice(1, 2);
            switch (choice)
            {
                case 1:
                    currentUser = userManagement.Login();
                    break;
                case 2:
                    currentUser = userManagement.Register();
                    break;
            }
        }

        // Galvenais cilpa, kurā lietotājs var izvēlēties darbību
        while (true)
        {
            Console.Clear();
            Console.WriteLine("+---------------------------------------+");
            Console.WriteLine("|           Izvēlieties darbību         |");
            Console.WriteLine("+---------------------------------------+");
            Console.WriteLine("| 1. Izveidot sarakstu                  |");
            Console.WriteLine("| 2. Izveidot plānu                     |");
            Console.WriteLine("| 3. Izveidot tabulu                    |");
            Console.WriteLine("| 4. Pievienot filmu vai seriālu        |");
            Console.WriteLine("| 5. Skatīt ierakstus                   |");
            Console.WriteLine("| 6. Iziet                              |");
            Console.WriteLine("+---------------------------------------+");
            int choice = GetUserChoice(1, 6);
            Console.Clear();
            switch (choice)
            {
                case 1:
                    // Izveido sarakstu un pievieno to lietotāja ierakstiem
                    Console.Write("Ievadiet saraksta nosaukumu: ");
                    string listTitle = Console.ReadLine();
                    Console.Write("Ievadiet saraksta vienības (atdalot ar komatiem): ");
                    string[] listItems = Console.ReadLine().Split(',');
                    ListEntry listEntry = new ListEntry(listTitle, listItems);
                    currentUser.AddEntry(listEntry);
                    break;
                case 2:
                    // Izveido plānu un pievieno to lietotāja ierakstiem
                    Console.Write("Ievadiet plana nosaukumu: ");
                    string planTitle = Console.ReadLine();
                    Console.Write("Ievadiet plana vienības (katra vienība jaunā rindā, ievadiet 'done', lai pabeigtu): ");
                    List<string> planItems = new List<string>();
                    string input;
                    while ((input = Console.ReadLine()) != "done")
                    {
                        planItems.Add(input);
                    }
                    PlanEntry planEntry = new PlanEntry(planTitle, planItems);
                    currentUser.AddEntry(planEntry);
                    break;
                case 3:
                    // Izveido tabulu un pievieno to lietotāja ierakstiem
                    Console.Write("Ievadiet tabulas nosaukumu: ");
                    string tableTitle = Console.ReadLine();
                    Console.Write("Ievadiet tabulas rindas (katra rinda jaunā rindā, kolonnas atdalītas ar komatiem, ievadiet 'done', lai pabeigtu): ");
                    List<string[]> tableRows = new List<string[]>();
                    string rowInput;
                    while ((rowInput = Console.ReadLine()) != "done")
                    {
                        string[] columns = rowInput.Split(',');
                        tableRows.Add(columns);
                    }
                    TableEntry tableEntry = new TableEntry(tableTitle, tableRows);
                    currentUser.AddEntry(tableEntry);
                    break;
                case 4:
                    // Pievieno filmu vai seriālu ierakstu lietotāja ierakstiem
                    Console.Write("Ievadiet nosaukumu: ");
                    string name = Console.ReadLine();
                    Console.Write("Ievadiet žanru: ");
                    string genre = Console.ReadLine();
                    Console.Write("Ievadiet novērtējumu: ");
                    int rating = GetUserChoice(1, 10);
                    MovieOrSeriesEntry movieOrSeriesEntry = new MovieOrSeriesEntry(name, genre, rating);
                    currentUser.AddEntry(movieOrSeriesEntry);
                    break;
                case 5:
                    // Attēlo lietotāja ierakstus
                    currentUser.DisplayEntries();
                    break;
                case 6:
                    // Iziet no programmas, saglabājot lietotājus
                    userManagement.SaveUsers("users.txt");
                    Console.WriteLine("Visu labu!");
                    Environment.Exit(0);
                    break;
            }
            Console.WriteLine("Nospiediet jebkuru taustiņu, lai turpinātu...");
            Console.ReadKey();
        }
    }

    static int GetUserChoice(int min, int max)
    {
        // Iegūst un validē lietotāja izvēli no konsoles
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
        {
            Console.WriteLine($"Lūdzu, ievadiet skaitli no {min} līdz {max}.");
        }
        return choice;
    }
}

abstract class Entry
{
    protected string Title;

    public Entry(string title)
    {
        Title = title;
    }

    public virtual void Display()
    {
        Console.WriteLine($"Nosaukums: {Title}");
    }
}

class ListEntry : Entry
{
    private string[] Items;

    public ListEntry(string title, string[] items) : base(title)
    {
        Items = items;
    }

    public override void Display()
    {
        Console.WriteLine($"Saraksts: {Title}");
        foreach (var item in Items)
        {
            Console.WriteLine($"|-{item}");
        }
    }
}

class PlanEntry : Entry
{
    private List<string> Items;

    public PlanEntry(string title, List<string> items) : base(title)
    {
        Items = items;
    }

    public override void Display()
    {
        Console.WriteLine($"Plāns: {Title}");
        foreach (var item in Items)
        {
            Console.WriteLine($"|-{item}");
        }
    }
}

class TableEntry : Entry
{
    private List<string[]> Rows;

    public TableEntry(string title, List<string[]> rows) : base(title)
    {
        Rows = rows;
    }

    public override void Display()
    {
        Console.WriteLine($"Tabula: {Title}");
        foreach (var row in Rows)
        {
            Console.WriteLine($"| {string.Join(" | ", row)} |");
        }
    }
}

class MovieOrSeriesEntry : Entry
{
    private string Genre;
    private int Rating;

    public MovieOrSeriesEntry(string title, string genre, int rating) : base(title)
    {
        Genre = genre;
        Rating = rating;
    }

    public override void Display()
    {
        Console.WriteLine($"Nosaukums: {Title}");
        Console.WriteLine($"Žanrs: {Genre}");
        Console.WriteLine($"Novērtējums: {Rating}");
    }
}

class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    private List<Entry> Entries { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        Entries = new List<Entry>();
    }

    public void AddEntry(Entry entry)
    {
        Entries.Add(entry);
    }

    public void DisplayEntries()
    {
        Console.WriteLine($"Ieraksti lietotājam: {Username}");
        foreach (var entry in Entries)
        {
            entry.Display();
            Console.WriteLine();
        }
    }
}

class UserManagement
{
    private List<User> Users { get; set; }

    public UserManagement()
    {
        Users = new List<User>();
    }

    public void LoadUsers(string filePath)
    {
        // Ielādē lietotājus no faila
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            Users.Add(new User(parts[0], parts[1]));
        }
    }

    public void SaveUsers(string filePath)
    {
        // Saglabā lietotājus failā
        List<string> lines = new List<string>();
        foreach (var user in Users)
        {
            lines.Add($"{user.Username},{user.Password}");
        }
        File.WriteAllLines(filePath, lines);
    }

    public User Login()
    {
        // Lietotāja pieteikšanās
        Console.Write("Lietotājvārds: ");
        string username = Console.ReadLine();
        Console.Write("Parole: ");
        string password = Console.ReadLine();

        foreach (var user in Users)
        {
            if (user.Username == username && user.Password == password)
            {
                Console.WriteLine("Pieteikšanās veiksmīga!");
                return user;
            }
        }

        Console.WriteLine("Nepareizs lietotājvārds vai parole!");
        return null;
    }

    public User Register()
    {
        // Lietotāja reģistrācija
        Console.WriteLine("Jauns lietotājvārds (no 6 līdz 15 rakstzīmēm, vismaz 1 cipars un lielā burta): ");
        string username = Console.ReadLine();

        // Pārbauda, vai lietotājvārds atbilst noteikumiem
        while (!IsUsernameValid(username))
        {
            Console.WriteLine("Lietotājvārdam jābūt no 6 līdz 15 rakstzīmēm garumā, saturēt vismaz 1 ciparu un lielo burtu. Lūdzu, ievadiet vēlreiz: ");
            username = Console.ReadLine();
        }

        // Izvada ziņu par veiksmīgu reģistrāciju un izveido jaunu lietotāju
        Console.WriteLine("Lietotājvārda reģistrācija veiksmīga!");
        Console.WriteLine("Jauna parole (minimāli 6 rakstzīmes, vismaz 1 cipars un lielā burta): ");
        string password = Console.ReadLine();

        // Pārbauda, vai parole atbilst noteikumiem
        while (!IsPasswordValid(password))
        {
            Console.WriteLine("Parolei jābūt vismaz 6 rakstzīmju garumā, saturēt vismaz 1 ciparu un lielo burtu. Lūdzu, ievadiet vēlreiz: ");
            password = Console.ReadLine();
        }

        // Izveido jaunu lietotāju un pievieno to lietotāju sarakstam
        User newUser = new User(username, password);
        Users.Add(newUser);

        return newUser;
    }

    private bool IsUsernameValid(string username)
    {
        // Pārbauda, vai lietotājvārds atbilst noteikumiem
        return username.Length >= 6 && username.Length <= 15 && username.Any(char.IsDigit) && username.Any(char.IsUpper);
    }

    private bool IsPasswordValid(string password)
    {
        // Pārbauda, vai parole atbilst noteikumiem
        return password.Length >= 6 && password.Any(char.IsDigit) && password.Any(char.IsUpper);
    }
}
