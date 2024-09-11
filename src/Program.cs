using System;

//Level 1: Basic Library Setup
class SuperClass
{
    private Guid _id;
    private DateTime _createdDate;

    public Guid Id { get { return _id; } set { _id = value; } }
    public DateTime CreatedDate
    {
        get { return _createdDate; }
        set
        {
            _createdDate = value;
        }
    }

    public SuperClass(DateTime? createdDate = null)
    {
        _id = Guid.NewGuid();
        _createdDate = createdDate ?? DateTime.Now;
    }
}

class User : SuperClass
{
    private string _name;
    public string Name { get { return _name; } set { _name = value; } }

    public User(string name, DateTime? createdDate = null) : base(createdDate)
    {
        _name = name;
    }
}
class Book : SuperClass
{

    private string _title;

    public string Title { get { return _title; } set { _title = value; } }

    public Book(string title, DateTime? createdDate = null) : base(createdDate)
    {
        _title = title;
    }
}

class Library
{
    private List<Book> _books;
    private List<User> _users;
    private EmailNotificationService _emailServices;
    private SMSNotificationService _smsServices;

    public Library(EmailNotificationService? emailServices = null, SMSNotificationService? smsServices = null)

    {
        _books = new List<Book>();
        _users = new List<User>();
        // _emailServices = emailServices ?? new EmailNotificationService(); 
        // _smsServices = smsServices ?? new SMSNotificationService();
        // * or _ Inject 
        _emailServices = emailServices;
        _smsServices = smsServices;
    }

    public void getallBooks()
    {
        int pageNumber = 1, pageSize = 3; // assume pageNumber and pageSize
        var results = (_books.OrderBy(book => book.CreatedDate)).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        foreach (var item in results)
        {
            Console.WriteLine($"Title: {item.Title} - Id: {item.Id} - CreatedDate: {item.CreatedDate}");
        }
    }
    public void getallUsers()
    {
        int pageNumber = 1, pageSize = 3; // assume pageNumber and pageSize
        var results = _users.OrderBy(user => user.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        foreach (var item in results)
        {
            Console.WriteLine($"Name: {item.Name} - Id: {item.Id} - CreatedDate{item.CreatedDate}");
        }
    }

    public Book? findBooksByTitle(string title)
    {
        return _books.Find(item => item.Title == title);
    }

    public User? findUsersByName(User user)
    {
        return _users.Find(item => item.Name == user.Name);
    }
    public void addBook(Book book)
    {
        try
        {
            _books.Add(book);
            _emailServices.SendNotificationOnSucess(book.Title);
            _smsServices.SendNotificationOnSucess(book.Title);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            _emailServices.SendNotificationOnFailure(book.Title);
            _smsServices.SendNotificationOnFailure(book.Title);
        }
    }
    public void addUser(User user)
    {
        try
        {
            _users.Add(user);
            _emailServices.SendNotificationOnSucess(user.Name);
            _smsServices.SendNotificationOnSucess(user.Name);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
            _emailServices.SendNotificationOnFailure(user.Name);
            _smsServices.SendNotificationOnFailure(user.Name);
        }
    }

    //Delete book/user by id
    public void deleteBook(Book book)
    {
        // check if the book exist 
        if (findBooksByTitle(book.Title) != null)
        {
            _books.Remove(book);
            _emailServices.SendNotificationOnSucess(book.Title);
            _smsServices.SendNotificationOnSucess(book.Title);
        }
        else
        {
            // Console.WriteLine($"sorry , {book.Title} is Not exist if the books list!");
            _emailServices.SendNotificationOnFailure(book.Title);
            _smsServices.SendNotificationOnFailure(book.Title);
        }
    }
    public void deleteUser(User user)
    {
        // check if the user exist 
        if (findUsersByName(user) != null)
        {
            _users.Remove(user);
            _emailServices.SendNotificationOnSucess(user.Name);
            _smsServices.SendNotificationOnSucess(user.Name);
        }
        else
        {
            // Console.WriteLine($"sorry , {user.Name} is Not exist if the users list!");
            _emailServices.SendNotificationOnFailure(user.Name);
            _smsServices.SendNotificationOnFailure(user.Name);
        }
    }

    // Level 3: Use reflection
    public void PrintNotificationServiceInfo()
    {  
        if (_emailServices == null)
        {
            Console.WriteLine("smsServices");
        }
        else
        {
            Console.WriteLine("_emailServices");

        }

    }
}

// Level 2: Setup Notification Service
interface INotificationService
{
    void SendNotificationOnSucess(string title);
    void SendNotificationOnFailure(string title);

}

class EmailNotificationService : INotificationService
{
    public void SendNotificationOnSucess(string title)
    {
        Console.WriteLine($"Hello, a new book titled '{title}' has been successfully added to the Library. If you have any queries or feedback, please contact our support team at support@library.com.");
    }
    public void SendNotificationOnFailure(string title)
    {
        Console.WriteLine($"We encountered an issue adding '{title}'.Please review the input data.For more help, visit our FAQ at library.com / faq");
    }
}

class SMSNotificationService : INotificationService
{
    public void SendNotificationOnSucess(string title)
    {
        Console.WriteLine($"Book {title} added to Library. Thank you!");
    }
    public void SendNotificationOnFailure(string title)
    {
        Console.WriteLine($"Error adding User '{title}'.Please email support@library.com.");
    }
}


internal class Program
{
    private static void Main()
    {
        Console.WriteLine("Assignment- Library Management System");
        var user1 = new User("Alice", new DateTime(2023, 1, 1));
        var user2 = new User("Bob", new DateTime(2023, 2, 1));
        // var user3 = new User("Charlie", new DateTime(2023, 3, 1));
        // var user4 = new User("David", new DateTime(2023, 4, 1));
        // var user5 = new User("Eve", new DateTime(2024, 5, 1));
        // var user6 = new User("Fiona", new DateTime(2024, 6, 1));
        // var user7 = new User("George", new DateTime(2024, 7, 1));
        // var user8 = new User("Hannah", new DateTime(2024, 8, 1));
        var user9 = new User("Ian");
        // // var user10 = new User("Julia");
        var book1 = new Book("The Great Gatsby", new DateTime(2023, 1, 1));
        var book2 = new Book("1984", new DateTime(2023, 2, 1));
        // var book3 = new Book("To Kill a Mockingbird", new DateTime(2023, 3, 1));
        // var book4 = new Book("The Catcher in the Rye", new DateTime(2023, 4, 1));
        // var book5 = new Book("Pride and Prejudice", new DateTime(2023, 5, 1));
        // var book6 = new Book("Wuthering Heights", new DateTime(2023, 6, 1));
        // var book7 = new Book("Jane Eyre", new DateTime(2023, 7, 1));
        // var book8 = new Book("Brave New World", new DateTime(2023, 8, 1));
        // var book9 = new Book("Moby-Dick", new DateTime(2023, 9, 1));
        // var book10 = new Book("War and Peace", new DateTime(2023, 10, 1));
        // var book11 = new Book("Hamlet", new DateTime(2023, 11, 1));
        // var book12 = new Book("Great Expectations", new DateTime(2023, 12, 1));
        // var book13 = new Book("Ulysses", new DateTime(2024, 1, 1));
        // var book14 = new Book("The Odyssey", new DateTime(2024, 2, 1));
        // var book15 = new Book("The Divine Comedy", new DateTime(2024, 3, 1));
        // var book16 = new Book("Crime and Punishment", new DateTime(2024, 4, 1));
        // var book17 = new Book("The Brothers Karamazov", new DateTime(2024, 5, 1));
        // var book18 = new Book("Don Quixote", new DateTime(2024, 6, 1));
        // var book19 = new Book("The Iliad");
        // var book20 = new Book("Anna Karenina");
        var emailService = new EmailNotificationService();
        var smsService = new SMSNotificationService();
        // var libraryWithEmail = new Library(emailService);
        // var libraryWithSMS = new Library(smsService);
        Library library = new Library(emailService, smsService);
        library.addBook(book2);
        Console.WriteLine("\n");
        library.addBook(book1);
        Console.WriteLine("\n\n");
        // library.addBook(book3);
        // library.addBook(book4);
        library.addUser(user2);
        Console.WriteLine("\n");
        library.addUser(user1);
        Console.WriteLine("\n");
        library.addUser(user9);
        Console.WriteLine("\n\n");
        library.getallBooks();
        Console.WriteLine("\n\n");
        library.getallUsers();
        Console.WriteLine("\n\n");
        library.findBooksByTitle("The Great Gatsby");
        library.PrintNotificationServiceInfo();
    }
}