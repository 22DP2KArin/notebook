using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        // Izveidojam lietotāju pārvaldības sistēmu
        UserManagement userManagement = new UserManagement();

        // Pārbauda, vai ir saglabāti lietotāji
        if (File.Exists("users.txt"))
        {
            userManagement.LoadUsers("users.txt");
        }

        // Lietotāja autentifikācijas
        User currentUser = null;
        while (currentUser == null)
        {
            Console.WriteLine("Lūdzu, pieteikties vai reģistrēties:");
            Console.WriteLine("1. Pieteikties");
            Console.WriteLine("2. Reģistrēties");
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

        while (true)
        {
            Console.Clear();

            // Izvēlnes opcijas
            Console.WriteLine("|-----------Izvēlieties darbibu-----------|");
            Console.WriteLine("| 1. Izveidot sarakstu                    |");
            Console.WriteLine("| 2. Izveidot plānu                       |");
            Console.WriteLine("| 3. Izveidot tabulu                      |");
            Console.WriteLine("| 4. Pievienot filmu vai seriālu          |");
            Console.WriteLine("| 5. Skatīt ierakstus                     |");
            Console.WriteLine("| 6. Iziet                                |");
            Console.WriteLine("|-----------------------------------------|");
            // Iegūstam lietotāja izvēli
            int choice = GetUserChoice(1, 6);

            // Notīra konsoli pēc izvēles iegūšanas
            Console.Clear();

            // Atkarībā no izvēles veicam darbību
            switch (choice)
            {
                case 1:
                    // Izveido sarakstu
                    Console.Write("Ievadiet saraksta nosaukumu: ");
                    string sarakstaNosaukums = Console.ReadLine();
                    Console.Write("Ievadiet saraksta vienības (atdalot ar komatiem): ");
                    string[] sarakstaVienibas = Console.ReadLine().Split(',');
                    ListEntry sarakstaIeraksts = new ListEntry(sarakstaNosaukums, sarakstaVienibas);
                    currentUser.AddEntry(sarakstaIeraksts);
                    break;

                case 2:
                    // Izveido plānu
                    Console.Write("Ievadiet plana nosaukumu: ");
                    string planaNosaukums = Console.ReadLine();
                    Console.Write("Ievadiet plana vienības (katra vienība jaunā rindā, ievadiet 'done', lai pabeigtu): ");
                    List<string> planaVienibas = new List<string>();
                    string input;
                    while ((input = Console.ReadLine()) != "done")
                    {
                        planaVienibas.Add(input);
                    }
                    PlanEntry planaIeraksts = new PlanEntry(planaNosaukums, planaVienibas);
                    currentUser.AddEntry(planaIeraksts);
                    break;

                case 3:
                    // Izveido tabulu
                    Console.Write("Ievadiet tabulas nosaukumu: ");
                    string tabulasNosaukums = Console.ReadLine();
                    Console.Write("Ievadiet tabulas rindas (katra rinda jaunā rindā, kolonnas atdalītas ar komatiem, ievadiet 'done', lai pabeigtu): ");
                    List<string[]> tabulasRindas = new List<string[]>();
                    string rindasIevade;
                    while ((rindasIevade = Console.ReadLine()) != "done")
                    {
                        string[] kolonnas = rindasIevade.Split(',');
                        tabulasRindas.Add(kolonnas);
                    }
                    TableEntry tabulasIeraksts = new TableEntry(tabulasNosaukums, tabulasRindas);
                    currentUser.AddEntry(tabulasIeraksts);
                    break;

                case 4:
                    // Pievieno filmu vai serialu
                    Console.Write("Ievadiet nosaukumu: ");
                    string nosaukums = Console.ReadLine();
                    Console.Write("Ievadiet zanru: ");
                    string zanrs = Console.ReadLine();
                    Console.Write("Ievadiet novertejumu: ");
                    int novertejums = GetUserChoice(1, 10);
                    MovieOrSeriesEntry ieraksts = new MovieOrSeriesEntry(nosaukums, zanrs, novertejums);
                    currentUser.AddEntry(ieraksts);
                    break;

                case 5:
                    // Skatit ierakstus
                    currentUser.DisplayEntries();
                    break;

                case 6:
                    // Iziet
                    userManagement.SaveUsers("users.txt");
                    Console.WriteLine("Visu labu!");
                    Environment.Exit(0);
                    break;
            }

            Console.WriteLine("Nospiediet jebkuru taustinu, lai turpinātu...");
            Console.ReadKey();
        }
    }

    // Funkcija, kas iegūst lietotāja izvēli noteiktā diapazonā
    static int GetUserChoice(int min, int max)
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
        {
            Console.WriteLine($"Lūdzu, ievadiet skaitli no {min} līdz {max}.");
        }
        return choice;
    }
}

class Entry
{
    protected string Nosaukums;

    public Entry(string nosaukums)
    {
        Nosaukums = nosaukums;
    }

    public virtual void Display()
    {
        Console.WriteLine($"Nosaukums: {Nosaukums}");
    }
}

class ListEntry : Entry
{
    private string[] Vienibas;

    public ListEntry(string nosaukums, string[] vienibas) : base(nosaukums)
    {
        Vienibas = vienibas;
    }

    public override void Display()
    {
        Console.WriteLine($"Saraksts: {Nosaukums}");
        foreach (var vieniba in Vienibas)
        {
            Console.WriteLine($"-{vieniba}");
        }
    }
}

class PlanEntry : Entry
{
    private List<string> Vienibas;

    public PlanEntry(string nosaukums, List<string> vienibas) : base(nosaukums)
    {
        Vienibas = vienibas;
    }

    public override void Display()
    {
        Console.WriteLine($"Plāns: {Nosaukums}");
        foreach (var vieniba in Vienibas)
        {
            Console.WriteLine($"-{vieniba}");
        }
    }
}

class TableEntry : Entry
{
    private List<string[]> Rindas;

    public TableEntry(string nosaukums, List<string[]> rindas) : base(nosaukums)
    {
        Rindas = rindas;
    }

    public override void Display()
    {
        Console.WriteLine($"Tabula: {Nosaukums}");
        foreach (var rinda in Rindas)
        {
            Console.WriteLine(string.Join(",", rinda));
        }
    }
}

class MovieOrSeriesEntry : Entry
{
    private string Zanrs;
    private int Novertejums;

    public MovieOrSeriesEntry(string nosaukums, string zanrs, int novertejums) : base(nosaukums)
    {
        Zanrs = zanrs;
        Novertejums = novertejums;
    }

    public override void Display()
    {
        Console.WriteLine($"Nosaukums: {Nosaukums}");
        Console.WriteLine($"Žanrs: {Zanrs}");
        Console.WriteLine($"Novērtējums: {Novertejums}");
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
        string[] lines = File.ReadAllLines(filePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            Users.Add(new User(parts[0], parts[1]));
        }
    }

    public void SaveUsers(string filePath)
    {
        List<string> lines = new List<string>();
        foreach (var user in Users)
        {
            lines.Add($"{user.Username},{user.Password}");
        }
        File.WriteAllLines(filePath, lines);
    }

    public User Login()
    {
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
        Console.Write("Jauns lietotājvārds: ");
        string username = Console.ReadLine();
        Console.Write("Jauna parole: ");
        string password = Console.ReadLine();

        User newUser = new User(username, password);
        Users.Add(newUser);
        Console.WriteLine("Reģistrācija veiksmīga!");

        return newUser;
    }
}
