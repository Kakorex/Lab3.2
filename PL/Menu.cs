using System;
using System.Text;

namespace PL
{
    public class Menu
    {
        private string _filePath = "data.json";
        private DAL.IDataProvider<DAL.Student> _studentProvider;
        private DAL.IDataProvider<DAL.FootballPlayer> _footballProvider;
        private DAL.IDataProvider<DAL.Lawyer> _lawyerProvider;

        public void MainMenu()
        {
            _studentProvider = new DAL.JsonProvider<DAL.Student>();
            _footballProvider = new DAL.JsonProvider<DAL.FootballPlayer>();
            _lawyerProvider = new DAL.JsonProvider<DAL.Lawyer>();
            SelectSerializationSettings();

            while (true)
            {
                try
                {
                    Console.Clear();
                    string providerName = _studentProvider.GetType().Name.Replace("Provider", "");
                    string menu =
                        $"Current File: {_filePath} | Provider: {providerName}\n\n" +
                        "Select option:\n" +
                        "1. Student\n" +
                        "2. Football player\n" +
                        "3. Lawyer\n" +
                        "4. Change settings\n" +
                        "5. Exit";
                    Console.WriteLine(menu);
                    string command = Console.ReadLine();

                    switch (command)
                    {
                        case "1":
                            StudentMenu();
                            break;
                        case "2":
                            FootballPlayerMenu();
                            break;
                        case "3":
                            LawyerMenu();
                            break;
                        case "4":
                            SelectSerializationSettings();
                            break;
                        case "5":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("\nValue is incorrect, please try again\n");
                            break;
                    }
                    Console.ReadKey();
                }
                catch (BLL.BusinessLogicException ex)
                {
                    Console.WriteLine($"\nBUSINESS LOGIC ERROR: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"\nDETAILS: {ex.InnerException.Message}");
                    }
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\nAN UNEXPECTED ERROR OCCURRED: {ex.Message}");
                    Console.WriteLine("\nPress any key to return to menu...");
                    Console.ReadKey();
                }
            }
        }

        private void SelectSerializationSettings()
        {
            Console.Clear();
            Console.WriteLine("--- Select Serialization Type ---");
            Console.WriteLine("1. Binary (.bin)");
            Console.WriteLine("2. XML (.xml)");
            Console.WriteLine("3. JSON (.json)");
            Console.WriteLine("4. Custom (.txt)");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _studentProvider = new DAL.BinaryProvider<DAL.Student>();
                    _footballProvider = new DAL.BinaryProvider<DAL.FootballPlayer>();
                    _lawyerProvider = new DAL.BinaryProvider<DAL.Lawyer>();
                    _filePath = "data.bin";
                    break;
                case "2":
                    _studentProvider = new DAL.XmlProvider<DAL.Student>();
                    _footballProvider = new DAL.XmlProvider<DAL.FootballPlayer>();
                    _lawyerProvider = new DAL.XmlProvider<DAL.Lawyer>();
                    _filePath = "data.xml";
                    break;
                case "3":
                    _studentProvider = new DAL.JsonProvider<DAL.Student>();
                    _footballProvider = new DAL.JsonProvider<DAL.FootballPlayer>();
                    _lawyerProvider = new DAL.JsonProvider<DAL.Lawyer>();
                    _filePath = "data.json";
                    break;
                case "4":
                    _studentProvider = new DAL.CustomProvider<DAL.Student>();
                    _footballProvider = new DAL.CustomProvider<DAL.FootballPlayer>();
                    _lawyerProvider = new DAL.CustomProvider<DAL.Lawyer>();
                    _filePath = "data.txt";
                    break;
                default:
                    Console.WriteLine("Invalid choice. Defaulting to JSON.");
                    break;
            }

            Console.WriteLine($"Enter file path (default: {_filePath}):");
            string customPath = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(customPath))
            {
                _filePath = customPath;
            }
        }

        private BLL.StudentService GetStudentService() => new BLL.StudentService(_studentProvider, _filePath);
        private BLL.FootballPlayerService GetFootballPlayerService() => new BLL.FootballPlayerService(_footballProvider, _filePath);
        private BLL.LawyerService GetLawyerService() => new BLL.LawyerService(_lawyerProvider, _filePath);

        private void StudentMenu()
        {
            BLL.StudentService service = GetStudentService();
            while (true)
            {
                Console.Clear();
                string menu =
                    "--- Student Menu ---\n" +
                    "1. Add student\n" +
                    "2. Show list of students\n" +
                    "3. Find student\n" +
                    "4. Remove student\n" +
                    "5. Find fifth-year students who served\n" +
                    "6. Back to Main Menu";
                Console.WriteLine(menu);
                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        AddStudent(service);
                        break;
                    case "2":
                        ShowList(service.GetAll());
                        break;
                    case "3":
                        FindPerson(service);
                        break;
                    case "4":
                        RemovePerson(service);
                        break;
                    case "5":
                        FindFifthYearStudents(service);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("\nValue is incorrect, please try again\n");
                        break;
                }
                Console.ReadKey();
            }
        }

        private void FootballPlayerMenu()
        {
            BLL.FootballPlayerService service = GetFootballPlayerService();
            while (true)
            {
                Console.Clear();
                string menu =
                    "--- Football Player Menu ---\n" +
                    "1. Add football player\n" +
                    "2. Show list of football players\n" +
                    "3. Find football player\n" +
                    "4. Remove football player\n" +
                    "5. Back to Main Menu";
                Console.WriteLine(menu);
                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        AddFootballPlayer(service);
                        break;
                    case "2":
                        ShowList(service.GetAll());
                        break;
                    case "3":
                        FindPerson(service);
                        break;
                    case "4":
                        RemovePerson(service);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("\nValue is incorrect, please try again\n");
                        break;
                }
                Console.ReadKey();
            }
        }

        private void LawyerMenu()
        {
            BLL.LawyerService service = GetLawyerService();
            while (true)
            {
                Console.Clear();
                string menu =
                    "--- Lawyer Menu ---\n" +
                    "1. Add lawyer\n" +
                    "2. Show list of lawyers\n" +
                    "3. Find lawyer\n" +
                    "4. Remove lawyer\n" +
                    "5. Back to Main Menu";
                Console.WriteLine(menu);
                string command = Console.ReadLine();

                switch (command)
                {
                    case "1":
                        AddLawyer(service);
                        break;
                    case "2":
                        ShowList(service.GetAll());
                        break;
                    case "3":
                        FindPerson(service);
                        break;
                    case "4":
                        RemovePerson(service);
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("\nValue is incorrect, please try again\n");
                        break;
                }
                Console.ReadKey();
            }
        }

        private void AddStudent(BLL.StudentService service)
        {
            BLL.StudentDto student = new BLL.StudentDto
            {
                FirstName = Input.InputName("first name"),
                LastName = Input.InputName("last name"),
                Passport = Input.InputPassport(),
                StudentID = Input.InputStudentID(),
                Course = Input.InputStudentCourse(),
                MilitaryID = Input.InputMilitaryID()
            };
            service.Add(student);
            Console.WriteLine("\nStudent added successfully.");
        }

        private void AddFootballPlayer(BLL.FootballPlayerService service)
        {
            BLL.FootballPlayerDto player = new BLL.FootballPlayerDto
            {
                FirstName = Input.InputName("first name"),
                LastName = Input.InputName("last name"),
                Passport = Input.InputPassport(),
                Team = Input.InputString("Enter the team name")
            };
            service.Add(player);
            Console.WriteLine("\nFootball Player added successfully.");
        }

        private void AddLawyer(BLL.LawyerService service)
        {
            BLL.LawyerDto lawyer = new BLL.LawyerDto
            {
                FirstName = Input.InputName("first name"),
                LastName = Input.InputName("last name"),
                Passport = Input.InputPassport(),
                Company = Input.InputString("Enter the company name")
            };
            service.Add(lawyer);
            Console.WriteLine("\nLawyer added successfully.");
        }

        private void ShowList<T>(List<T> list) where T : BLL.PersonDto
        {
            Console.WriteLine($"\n--- List of {typeof(T).Name.Replace("Dto", "")}s ---\n");
            if (list == null || list.Count == 0)
            {
                Console.WriteLine("List is empty.");
                return;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in list)
            {
                if (item is BLL.StudentDto s)
                {
                    sb.AppendLine($"Student: {s.FirstName} {s.LastName}");
                    sb.AppendLine($"  Passport: {s.Passport}");
                    sb.AppendLine($"  ID: {s.StudentID}");
                    sb.AppendLine($"  Course: {s.Course}");
                    sb.AppendLine($"  Military ID: {s.MilitaryID}");
                }
                else if (item is BLL.FootballPlayerDto fp)
                {
                    sb.AppendLine($"FootballPlayer: {fp.FirstName} {fp.LastName}");
                    sb.AppendLine($"  Passport: {fp.Passport}");
                    sb.AppendLine($"  Team: {fp.Team}");
                }
                else if (item is BLL.LawyerDto l)
                {
                    sb.AppendLine($"Lawyer: {l.FirstName} {l.LastName}");
                    sb.AppendLine($"  Passport: {l.Passport}");
                    sb.AppendLine($"  Company: {l.Company}");
                }
                sb.AppendLine();
            }
            Console.Write(sb.ToString());
        }

        private void FindPerson<TEntity, TDto>(BLL.EntityService<TEntity, TDto> service) where TEntity : DAL.Person where TDto : BLL.PersonDto
        {
            string firstName = Input.InputName("first name");
            string lastName = Input.InputName("last name");
            string passport = Input.InputPassport();

            TDto person = service.Find(firstName, lastName, passport);
            ShowList(new List<TDto> { person });
        }

        private void RemovePerson<TEntity, TDto>(BLL.EntityService<TEntity, TDto> service) where TEntity : DAL.Person where TDto : BLL.PersonDto
        {
            string firstName = Input.InputName("first name");
            string lastName = Input.InputName("last name");
            string passport = Input.InputPassport();

            service.Remove(firstName, lastName, passport);
            Console.WriteLine("\nPerson removed successfully.");
        }

        private void FindFifthYearStudents(BLL.StudentService service)
        {
            List<BLL.StudentDto> students = service.FindFifthYearStudentsWheServed();
            Console.WriteLine($"\n--- Fifth-Year Students Who Served (Total: {students.Count}) ---\n");
            ShowList(students);
        }
    }
}
