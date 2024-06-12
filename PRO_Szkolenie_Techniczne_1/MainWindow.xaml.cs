using Microsoft.Data.Sqlite;
using PRO_Szkolenie_Techniczne_1.Klasy;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PRO_Szkolenie_Techniczne_1
{
    /// <summary>
    /// Logika dla głównego okna (MainWindow.xaml)
    /// </summary>
    public partial class MainWindow : Window
    {
        //! Zmienne
        //private string Ścieżka_Do_DB = "D:\\Visual Studio - Projekty\\PRO_Szkolenie_Techniczne_1\\PRO_Szkolenie_Techniczne_1\\PRO_Szkolenie_Techniczne_1_DB.db"; // TODO: Przerobić na względną
        private string Ścieżka_Do_DB = "..\\..\\..\\PRO_Szkolenie_Techniczne_1_DB.db"; // TODO: Przerobić na względną
        private ObservableCollection<Test> Testy = new ObservableCollection<Test>();
        private ObservableCollection<Test> Edytowanie_Testów = new ObservableCollection<Test>();
        private ObservableCollection<Pytanie> Edytowanie_Pytań = new ObservableCollection<Pytanie>();
        private ObservableCollection<Uczestnik> Uczestnicy = new ObservableCollection<Uczestnik>();
        private ObservableCollection<Udzielona_Odpowiedź> Udzielone_Odpowiedzi = new ObservableCollection<Udzielona_Odpowiedź>();
        private int Wybór_Testu_Ostatni_Index = -1; // Przechowuje Index ostatniego zaznaczego wiersza w DataGrid (Widok wyboru testu)
        private int Wybór_Edytowanie_Testu_Ostatni_Index = -1; // Przechowuje Index ostatniego zaznaczego wiersza w DataGrid (Zarządzanie testami)
        private int Wybór_Edytowanie_Pytań_Ostatni_Index = -1; // Przechowuje Index ostatniego zaznaczego wiersza w DataGrid (Edytowanie pytań)
        private int ID_Edytowanego_Testu = -1; // Przechowuje informacje o ID testu w celu edycji jego pytań // TODO: Czy to jest gdzieś używane???
        private int ID_Uczestnika = -1; // Przechowuje informacje o ID uczestnika który wypełnia test
        private int ID_Uczestnika_2 = -1; // Przechowuje informacje o ID uczestnika którego są sprawdzane udzielone odpowiedzi
        //private int Ilość_Pytań_Wypełnianego_Testu = -1; // Przechowuje informacje o ilości pytań w wypełnianym teście
        private List<Pytanie> Dostępne_Pytania = new List<Pytanie>(); // Przechowuje pytania z obecnie wypełnianego testu

        //! Enum
        public enum Stany_Logowania
        {
            Nie,
            Administrator,
            Super_Admin,
        }

        private Klasy.Konto? Użytkownik;
        public Stany_Logowania Czy_Zalogowano = Stany_Logowania.Nie;

        //! Main
        public MainWindow()
        {
            InitializeComponent();

            Zmiana_Widoku(0); // Automatycznie ustawi Wybieranie Testów

            Wybór_Testu_DB_Select();
            DataGrid_Testy.ItemsSource = Testy;
            DataGrid_Testy_1.Binding = new System.Windows.Data.Binding("ID");
            DataGrid_Testy_2.Binding = new System.Windows.Data.Binding("Nazwa");
            DataGrid_Testy_3.Binding = new System.Windows.Data.Binding("Opis");

            DataGrid_Edytowanie_Testów.ItemsSource = Edytowanie_Testów;
            DataGrid_Edytowanie_Testów_1.Binding = new System.Windows.Data.Binding("ID");
            DataGrid_Edytowanie_Testów_2.Binding = new System.Windows.Data.Binding("Nazwa");
            DataGrid_Edytowanie_Testów_3.Binding = new System.Windows.Data.Binding("Opis");
            DataGrid_Edytowanie_Testów_4.Binding = new System.Windows.Data.Binding("Autor");

            Edycja_Pytań_DB_Select();
            DataGrid_Edytowanie_Pytań.ItemsSource = Edytowanie_Pytań;
            DataGrid_Edytowanie_Pytań_0.Binding = new System.Windows.Data.Binding("ID");
            DataGrid_Edytowanie_Pytań_1.Binding = new System.Windows.Data.Binding("Treść_Pytania");
            DataGrid_Edytowanie_Pytań_2.Binding = new System.Windows.Data.Binding("Poprawna_Odpowiedź");
            DataGrid_Edytowanie_Pytań_3.Binding = new System.Windows.Data.Binding("Odpowiedź_A");
            DataGrid_Edytowanie_Pytań_4.Binding = new System.Windows.Data.Binding("Odpowiedź_B");
            DataGrid_Edytowanie_Pytań_5.Binding = new System.Windows.Data.Binding("Odpowiedź_C");
            DataGrid_Edytowanie_Pytań_6.Binding = new System.Windows.Data.Binding("Odpowiedź_D");

            DataGrid_Sprawdzanie_Odpowiedzi_Uczestnicy.ItemsSource = Uczestnicy;
            DataGrid_Sprawdzenie_Odpowiedzi_Uczestnicy_1.Binding = new System.Windows.Data.Binding("ID");
            DataGrid_Sprawdzenie_Odpowiedzi_Uczestnicy_2.Binding = new System.Windows.Data.Binding("Imie");
            DataGrid_Sprawdzenie_Odpowiedzi_Uczestnicy_3.Binding = new System.Windows.Data.Binding("Nazwisko");
            DataGrid_Sprawdzenie_Odpowiedzi_Uczestnicy_4.Binding = new System.Windows.Data.Binding("Data_Utworzenia");
            DataGrid_Sprawdzenie_Odpowiedzi_Uczestnicy_5.Binding = new System.Windows.Data.Binding("Test");
            DataGrid_Sprawdzenie_Odpowiedzi_Uczestnicy_6.Binding = new System.Windows.Data.Binding("Nazwa_Testu");

            DataGrid_Sprawdzanie_Odpowiedzi.ItemsSource = Udzielone_Odpowiedzi;
            DataGrid_Sprawdzenie_Odpowiedzi_1.Binding = new System.Windows.Data.Binding("ID");
            DataGrid_Sprawdzenie_Odpowiedzi_2.Binding = new System.Windows.Data.Binding("Treść_Pytania");
            DataGrid_Sprawdzenie_Odpowiedzi_3.Binding = new System.Windows.Data.Binding("Poprawna_Odpowiedź");
            DataGrid_Sprawdzenie_Odpowiedzi_4.Binding = new System.Windows.Data.Binding("Wybrana_Odpowiedź");
        }

        public void MessageBox_O_Programie(object sender, RoutedEventArgs e)
        {
            string Komunikat = "Projekt ze Szkolenia Technicznego 1\n\nHubert Michna GL04 w67259\nSemestr: 4, Zaoczne\nw67259@student.wsiz.edu.pl\n\n\nMaj 2024";
            System.Windows.MessageBox.Show(Komunikat, "O Programie", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void Zamknij_Program(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        public void Logowanie(object sender, RoutedEventArgs e)
        {
            if (Czy_Zalogowano != Stany_Logowania.Nie)
            {
                Komunikat("Jesteś już zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string Login = TextBox_Login.Text;
            string Hasło = PasswordBox_Hasło.Password;

            try
            {
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT Login, Hasło, ID, [Super Admin] FROM Administratorzy;";

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            if (Login == Rekord.GetString(0) && Hasło == Rekord.GetString(1))
                            {
                                Komunikat($"Zalogowano jako \"{Login}\".", "Logowanie", MessageBoxButton.OK, MessageBoxImage.Information);
                                Użytkownik = new Klasy.Konto(Convert.ToInt32(Rekord.GetString(2)), Rekord.GetString(0), Convert.ToBoolean(Rekord.GetString(3)));
                                Użytkownik.MenuBar_Zmiana_Tekstu_Zalogowano(this);
                                Zmiana_Widoku(2);
                                Edycja_Konta_Administratora_DB_Select_Dane();
                                return;
                            }
                        }
                    }
                }

                Komunikat("Niepoprawne Dane!", "Logowanie", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Wylogowanie(object sender, RoutedEventArgs e)
        {
            if (Czy_Zalogowano == Stany_Logowania.Nie || Użytkownik == null)
            {
                Komunikat("Nie jesteś zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Użytkownik.MenuBar_Zmiana_Tekstu_Wylogowano(this);
            Użytkownik = null;

            Komunikat("Zostałeś wylogowany/a.", "Wylogowano", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //! Wybór dostępnych testów
        public void Wybór_Testu_DB_Select()
        {
            Testy.Clear();
            try
            {
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT ID, Nazwa, Autor, Opis FROM Testy";

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            Testy.Add(new Test(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3)));
                        }
                    }
                }
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Wybór_Testu_Index(object sender, EventArgs e) // Do listy z wyborem testów
        {
            Wybór_Testu_Ostatni_Index = DataGrid_Testy.Items.IndexOf(DataGrid_Testy.CurrentItem);
        }
        public void Wybór_Testu_Zatwierdź(object sender, RoutedEventArgs e)
        {
            if (Wybór_Testu_Ostatni_Index == -1)
            {
                Komunikat("Nie wybrano żadnego testu!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        //! Edytowanie konta administratora
        public void Edycja_Konta_Administratora_DB_Select_Dane() // TODO: Chyba potrzeba będzie tez przeciążenie tej funkcji
        {
            try
            {
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"SELECT ID, Imię, Nazwisko, Login, [E-Mail] FROM Administratorzy WHERE ID = $ID;";
                    Zapytanie.Parameters.AddWithValue("$ID", Użytkownik.ID.ToString());

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            Konto_Administratora_ID.Text = Rekord.GetString(0);
                            Konto_Administratora_Imię.Text = Rekord.GetString(1);
                            Konto_Administratora_Nazwisko.Text = Rekord.GetString(2);
                            Konto_Administratora_Login.Text = Rekord.GetString(3);
                            Konto_Administratora_Email.Text = Rekord.GetString(4);
                        }
                    }
                }

                Konto_Administratora_Stare_Hasło.Password = "";
                Konto_Administratora_Nowe_Hasło.Password = "";
                Konto_Administratora_Nowe_Hasło_Powtórzenie.Password = "";
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void Edycja_Konta_Administratora_DB_Select_Dane(object sender, EventArgs e) // TODO: Chyba potrzeba będzie tez przeciążenie tej funkcji
        {
            Edycja_Konta_Administratora_DB_Select_Dane();
        }
        public void Edycja_Konta_Administratora_DB_Update_Dane(object sender, EventArgs e) //TODO:
        {
            try
            {
                string ID = Konto_Administratora_ID.Text;
                string Login = Konto_Administratora_Login.Text;
                string E_Mail = Konto_Administratora_Email.Text;
                string Imię = Konto_Administratora_Imię.Text;
                string Nazwisko = Konto_Administratora_Nazwisko.Text;

                if (Login == "")
                {
                    Komunikat("Nie możesz usunąć loginu", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    UPDATE Administratorzy
                    SET
                        Login = $Nowe_Login,
                        Imię = $Nowe_Imie,
                        Nazwisko = $Nowe_Nazwisko,
                        [E-Mail] = $Nowe_Email
                    WHERE ID = $ID";

                    Zapytanie.Parameters.AddWithValue("$ID", ID);
                    Zapytanie.Parameters.AddWithValue("$Nowe_Login", Login);
                    Zapytanie.Parameters.AddWithValue("$Nowe_Imie", Imię);
                    Zapytanie.Parameters.AddWithValue("$Nowe_Nazwisko", Nazwisko);
                    Zapytanie.Parameters.AddWithValue("$Nowe_Email", E_Mail);

                    Zapytanie.ExecuteNonQuery();
                }

                Komunikat("Zapisano zmiany.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                Zmiana_Widoku(2);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void Edycja_Konta_Administratora_DB_Update_Hasło(object sender, EventArgs e)
        {
            // Funkcja pomocnicza
            string Edycja_Konta_Administratora_DB_Select_Hasło()
            {
                try
                {
                    using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                    {
                        Połączenie.Open();

                        var Zapytanie = Połączenie.CreateCommand();
                        Zapytanie.CommandText = @"SELECT Hasło FROM Administratorzy WHERE ID = $ID;";
                        Zapytanie.Parameters.AddWithValue("$ID", Użytkownik.ID.ToString());

                        using (var Rekord = Zapytanie.ExecuteReader())
                        {
                            while (Rekord.Read())
                            {
                                return Rekord.GetString(0);
                            }
                        }
                    }

                    return "";
                }
                catch (Exception Błąd)
                {
                    Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                return "";
            }

            try
            {
                string ID = Konto_Administratora_ID.Text;
                string Hasło_Stare = Konto_Administratora_Stare_Hasło.Password;
                string Hasło_Nowe = Konto_Administratora_Nowe_Hasło.Password;
                string Hasło_Nowe_Powtórzone = Konto_Administratora_Nowe_Hasło_Powtórzenie.Password;

                if (Hasło_Stare != Edycja_Konta_Administratora_DB_Select_Hasło()) // Tu zrobić kolejny select, tak żeby móc porównać z hasłem z DB
                {
                    Komunikat("Podano niepoprawne stare hasło.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (Hasło_Nowe != Hasło_Nowe_Powtórzone)
                {
                    Komunikat("Nowe hasła nie są identyczne.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (Hasło_Nowe == "")
                {
                    Komunikat("Nie możesz ustawić takiego hasła.", "Bład", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    UPDATE Administratorzy
                    SET
                        Hasło = $Nowe_Hasło
                    WHERE ID = $ID";

                    Zapytanie.Parameters.AddWithValue("$ID", ID);
                    Zapytanie.Parameters.AddWithValue("$Nowe_Hasło", Hasło_Nowe);

                    Zapytanie.ExecuteNonQuery();
                }

                Komunikat("Zapisano zmiany.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                Zmiana_Widoku(2);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Edycja_Konta_Administratora_DB_Delete(object sender, EventArgs e)
        {
            try
            {
                string ID = Konto_Administratora_ID.Text;

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"DELETE FROM Administratorzy WHERE ID = $ID";

                    Zapytanie.Parameters.AddWithValue("$ID", ID);

                    Zapytanie.ExecuteNonQuery();
                }

                Komunikat("Usunięto konto.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                Zmiana_Widoku(1);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //! Tworzenie konta administratora
        public void Tworzenie_Konta_Administratora_DB_Insert(object sender, EventArgs e)
        {
            try
            {
                string Imie = TextBox_Tworzenie_Konta_Administratora_Imie.Text;
                string Nazwisko = TextBox_Tworzenie_Konta_Administratora_Nazwisko.Text;
                string Email = TextBox_Tworzenie_Konta_Administratora_Email.Text;
                string Login = TextBox_Tworzenie_Konta_Administratora_Login.Text;
                string Hasło = PasswordBox_Tworzenie_Konta_Administratora_Hasło.Password;
                string Super_Admin = RadioButton_Tworzenie_Konta_Administratora_SuperAdmin.IsChecked.ToString().ToLower();

                if (Login == "" || Hasło == "")
                {
                    Komunikat("Login i hasło nie mogą być puste!", "Bład", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    INSERT INTO Administratorzy(Login, Hasło, [Super Admin], Imię, Nazwisko, [E-Mail])
                    VALUES ($Login, $Haslo, $Super_Admin, $Imie, $Nazwisko, $Email)";

                    Zapytanie.Parameters.AddWithValue("$Imie", Imie);
                    Zapytanie.Parameters.AddWithValue("$Nazwisko", Nazwisko);
                    Zapytanie.Parameters.AddWithValue("$Email", Email);
                    Zapytanie.Parameters.AddWithValue("$Login", Login);
                    Zapytanie.Parameters.AddWithValue("$Haslo", Hasło);
                    Zapytanie.Parameters.AddWithValue("$Super_Admin", Super_Admin);

                    Zapytanie.ExecuteNonQuery();
                }

                Komunikat("Utworzono konto.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);

                Zmiana_Widoku(1);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //! Tworzenie testów
        public void Tworzenie_Testu_DB_Insert(object sender, EventArgs e)
        {
            try
            {
                string Nazwa = Tworzenie_Testu_Nazwa.Text;
                string Opis = Tworzenie_Testu_Opis.Text;
                int ID = Użytkownik.ID;

                if (Nazwa == "" || Opis == "")
                {
                    Komunikat("Nazwa i opis nie moga być puste", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    INSERT INTO Testy(Nazwa, Autor, Opis)
                    VALUES ($Nazwa, $Autor, $Opis)";

                    Zapytanie.Parameters.AddWithValue("$Nazwa", Nazwa);
                    Zapytanie.Parameters.AddWithValue("$Autor", ID);
                    Zapytanie.Parameters.AddWithValue("$Opis", Opis);

                    Zapytanie.ExecuteNonQuery();
                }

                Komunikat("Utworzono nowy test.", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                Zmiana_Widoku(4);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //! Zarządzanie testami
        public void Zarządzanie_Testami_DB_Select()
        {
            try
            {
                Edytowanie_Testów.Clear();
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT ID, Nazwa, Autor, Opis FROM Testy";

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            //Edytowanie_Testów.Add(new Test(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3), Rekord.GetString(4), Convert.ToBoolean(Rekord.GetString(5))));
                            //Edytowanie_Testów.Add(new Test(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3), Rekord.GetString(4), Convert.ToBoolean(Rekord.GetString(5))));
                            Edytowanie_Testów.Add(new Test(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3)));
                        }
                    }
                }
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Zarządzanie_Testami_DB_Update(object sender, EventArgs e)
        {
            // SelectedCellsChanged Event --> Zapisuje zmiany ale dopiero po wybraniu innego rekordu i następnie wybraniu rekordu w którym wprowadziło się zmiany;
                // Zapisuje zmiany w momenicie wybrania danego wiersza przez co trzeba się przełączyć na inny rekord i wrócić żeby faktycznie zapisać zmiany TODO:

            // To całą funkcja wywołuje się kiedy zostanie usunięty rekord z DB/DataGrid, bez try catch wyrzuca błąd

            try
            {
                int Index = DataGrid_Edytowanie_Testów.SelectedIndex;

                int ID = Edytowanie_Testów[Index].ID;
                string Nazwa = Edytowanie_Testów[Index].Nazwa;
                string Opis = Edytowanie_Testów[Index].Opis;
                

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    UPDATE Testy
                    SET
                        Nazwa = $Nazwa,
                        Opis = $Opis,
                    WHERE ID = $ID";

                    Zapytanie.Parameters.AddWithValue("$ID", ID);
                    Zapytanie.Parameters.AddWithValue("$Nazwa", Nazwa);
                    Zapytanie.Parameters.AddWithValue("$Opis", Opis);

                    Zapytanie.ExecuteNonQuery();
                }
            }
            catch (Exception Błąd)
            {
                //Komunikat($"Błąd:\n{Błąd}");
            }
        }
        public void Zarządzanie_Testami_Index(object sender, EventArgs e) // Do listy z wyborem edytowania testów
        {
            //Wybór_Edytowanie_Testu_Ostatni_Index = DataGrid_Edytowanie_Testów.Items.IndexOf(DataGrid_Edytowanie_Testów.CurrentItem); // To wybiera o jeden element za wcześniej z tablicy TODO:
            Wybór_Edytowanie_Testu_Ostatni_Index = Edytowanie_Testów[DataGrid_Edytowanie_Testów.Items.IndexOf(DataGrid_Edytowanie_Testów.CurrentItem)].ID;
        }
        public void Zarządzanie_Testami_DB_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Wybór_Edytowanie_Testu_Ostatni_Index == -1)
                {
                    Komunikat("Nie wybrano żadnego testu!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"DELETE FROM Testy WHERE ID = $ID";

                    Zapytanie.Parameters.AddWithValue("$ID", Edytowanie_Testów[Wybór_Edytowanie_Testu_Ostatni_Index].ID);

                    Zapytanie.ExecuteNonQuery();
                }

                Komunikat("Usunięto zaznaczony test", "Komunikat", MessageBoxButton.OK, MessageBoxImage.Information);
                DataGrid_Edytowanie_Testów.SelectedIndex = -1;
                Zmiana_Widoku(4);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //! Edycja pytań do testów
        public void Edycja_Pytań_DB_Select()
        {
            try
            {
                Edytownaie_Pytań_ID_Testy.Content = $"Edytowanie Pytań (ID Testu: {Wybór_Edytowanie_Testu_Ostatni_Index})";

                Edytowanie_Pytań.Clear();
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT ID, [Treść Pytania], [Odpowiedź A], [Odpowiedź B], [Odpowiedź C], [Odpowiedź D], [Poprawna Odpowiedź] FROM [Pytania v2] WHERE Test = $ID_Testu";
                    Zapytanie.Parameters.AddWithValue("$ID_Testu", Wybór_Edytowanie_Testu_Ostatni_Index);

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            Edytowanie_Pytań.Add(new Pytanie(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3), Rekord.GetString(4), Rekord.GetString(5), Rekord.GetString(6)));
                        }
                    }
                }
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void Edycja_Pytań_DB_Insert(object sender, EventArgs e)
        {
            try
            {
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                        INSERT INTO [Pytania v2](Test)
                        VALUES ($ID_Testu)";

                    Zapytanie.Parameters.AddWithValue("$ID_Testu", Wybór_Edytowanie_Testu_Ostatni_Index);
                    Zapytanie.ExecuteNonQuery();
                }

                Edycja_Pytań_DB_Select();
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Edycja_Pytań_DB_Delete(object sender, EventArgs e)
        {
            try
            {
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"DELETE FROM [Pytania v2] WHERE ID = $ID";
                    Zapytanie.Parameters.AddWithValue("$ID", Edytowanie_Pytań[Wybór_Edytowanie_Pytań_Ostatni_Index].ID);

                    Zapytanie.ExecuteNonQuery();
                }

                Edycja_Pytań_DB_Select();
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Edycja_Pytań_DB_Update(object sender, EventArgs e)
        {
            try
            {
                int Index = DataGrid_Edytowanie_Pytań.SelectedIndex;

                int? ID = Edytowanie_Pytań[Index].ID;
                string Treść_Pytania = Edytowanie_Pytań[Index].Treść_Pytania;
                string Odpowiedź_A = Edytowanie_Pytań[Index].Odpowiedź_A;
                string Odpowiedź_B = Edytowanie_Pytań[Index].Odpowiedź_B;
                string Odpowiedź_C = Edytowanie_Pytań[Index].Odpowiedź_C;
                string Odpowiedź_D = Edytowanie_Pytań[Index].Odpowiedź_D;
                string Poprawna_Odpowiedź = Edytowanie_Pytań[Index].Poprawna_Odpowiedź;

                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    UPDATE [Pytania v2]
                    SET
                        [Treść Pytania] = $Treść_Pytania,
                        [Odpowiedź A] = $Odpowiedź_A,
                        [Odpowiedź B] = $Odpowiedź_B,
                        [Odpowiedź C] = $Odpowiedź_C,
                        [Odpowiedź D] = $Odpowiedź_D,
                        [Poprawna Odpowiedź] = $Poprawna_Odpowiedź
                    WHERE ID = $ID";

                    Zapytanie.Parameters.AddWithValue("$ID", ID);
                    Zapytanie.Parameters.AddWithValue("$Treść_Pytania", Treść_Pytania);
                    Zapytanie.Parameters.AddWithValue("$Odpowiedź_A", Odpowiedź_A);
                    Zapytanie.Parameters.AddWithValue("$Odpowiedź_B", Odpowiedź_B);
                    Zapytanie.Parameters.AddWithValue("$Odpowiedź_C", Odpowiedź_C);
                    Zapytanie.Parameters.AddWithValue("$Odpowiedź_D", Odpowiedź_D);
                    Zapytanie.Parameters.AddWithValue("$Poprawna_Odpowiedź", Poprawna_Odpowiedź);

                    Zapytanie.ExecuteNonQuery();
                }
            }
            catch (Exception Błąd)
            {
                //Komunikat($"Błąd:\n{Błąd}");
            }
        }
        public void Edycja_Pytań_Index(object sender, EventArgs e)
        {
            Wybór_Edytowanie_Pytań_Ostatni_Index = DataGrid_Edytowanie_Pytań.Items.IndexOf(DataGrid_Edytowanie_Pytań.CurrentItem);
        }

        //! Udzielanie odpowiedzi
        public void Rozwiązywanie_Testu_DB_Select()
        {
            try
            {
                string Imie = TextBox_Wybór_Testu_Imie.Text;
                string Nazwisko = TextBox_Wybór_Testu_Nazwisko.Text;
                string Email = TextBox_Wybór_Testu_Email.Text;
                DateTime Data_Utworzenia = DateTime.Now.ToLocalTime();
                int Test = Testy[Wybór_Testu_Ostatni_Index].ID;

                // Zapisanie informacji o uczestniku
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    INSERT INTO Uczestnicy(Imię, Nazwisko, [E-Mail], [Data Utworzenia], Test)
                    VALUES ($Imie, $Nazwisko, $Email, $Data_Utworzenia, $Test)";

                    Zapytanie.Parameters.AddWithValue("$Imie", Imie);
                    Zapytanie.Parameters.AddWithValue("$Nazwisko", Nazwisko);
                    Zapytanie.Parameters.AddWithValue("$Email", Email);
                    Zapytanie.Parameters.AddWithValue("$Data_Utworzenia", Data_Utworzenia);
                    Zapytanie.Parameters.AddWithValue("$Test", Test);

                    Zapytanie.ExecuteNonQuery();
                }

                // Wczytanie ID nowo dodanego rekordu
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT ID FROM Uczestnicy WHERE Imię = $Imie AND Nazwisko = $Nazwisko AND [E-Mail] = $Email AND [Data Utworzenia] = $Data_Utworzenia";
                    Zapytanie.Parameters.AddWithValue("$Imie", Imie);
                    Zapytanie.Parameters.AddWithValue("Nazwisko", Nazwisko);
                    Zapytanie.Parameters.AddWithValue("$Email", Email);
                    Zapytanie.Parameters.AddWithValue("$Data_Utworzenia", Data_Utworzenia);

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            ID_Uczestnika = Convert.ToInt16(Rekord.GetString(0));
                        }
                    }
                }

                // Wczytanie pytań
                Dostępne_Pytania.Clear();
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT ID, [Treść Pytania], [Odpowiedź A], [Odpowiedź B], [Odpowiedź C], [Odpowiedź D], [Poprawna Odpowiedź] FROM [Pytania v2] WHERE Test = $ID_Testu";
                    Zapytanie.Parameters.AddWithValue("$ID_Testu", Test);

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            Dostępne_Pytania.Add(new Pytanie(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3), Rekord.GetString(4), Rekord.GetString(5), Rekord.GetString(6)));
                        }
                    }
                }

                if (Dostępne_Pytania.Count <= 0)
                {
                    return;
                }

                Dostępne_Pytania[0].Ustaw_Odpowiedzi(this);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Rozwiązywanie_Testu_DB_Select(object sender, EventArgs e)
        {
            Rozwiązywanie_Testu_DB_Select();
        }
        public void Rozwiązywanie_Testu_DB_Insert()
        {
            try
            {
                // Sprawdzanie którą odpowiedź zaznaczono
                string Zaznaczona_Odpowiedź;
                if (Wypełnianie_Testu_Odpowiedź_A.IsChecked == true)
                {
                    Zaznaczona_Odpowiedź = "A";
                }
                else if (Wypełnianie_Testu_Odpowiedź_B.IsChecked == true)
                {
                    Zaznaczona_Odpowiedź = "B";
                }
                else if (Wypełnianie_Testu_Odpowiedź_C.IsChecked == true)
                {
                    Zaznaczona_Odpowiedź = "C";
                }
                else if (Wypełnianie_Testu_Odpowiedź_D.IsChecked == true)
                {
                    Zaznaczona_Odpowiedź = "D";
                }
                else
                {
                    Zaznaczona_Odpowiedź = "A";
                }

                // Zapisanie odpowiedzi
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    INSERT INTO [Udzielone Odpowiedzi](Pytanie, [Wybrana Odpowiedź], Uczestnik, [Data Udzielenia Odpowiedzi])
                    VALUES ($Pytanie, $Wybrana_Odpowiedź, $Uczestnik, $Data_Udzielenia_Odpowiedzi)";
                    Zapytanie.Parameters.AddWithValue("$Pytanie", Dostępne_Pytania[0].ID);
                    Zapytanie.Parameters.AddWithValue("$Wybrana_Odpowiedź", Zaznaczona_Odpowiedź);
                    Zapytanie.Parameters.AddWithValue("$Uczestnik", ID_Uczestnika);
                    Zapytanie.Parameters.AddWithValue("$Data_Udzielenia_Odpowiedzi", DateTime.Now.ToLocalTime());

                    Zapytanie.ExecuteNonQuery();
                }

                // Usunięcie pytania z pamięci
                Dostępne_Pytania.RemoveAt(0);

                // Sprawdzanie czy zostały jeszcze jakieś pytania
                if (Dostępne_Pytania.Count == 0)
                {
                    Komunikat("Odpowiedziano na wszystkie pytania.", "Koniec testu", MessageBoxButton.OK, MessageBoxImage.Information);
                    Zmiana_Widoku(0);
                    return;
                }

                // Ustawienie treści pytań
                Dostępne_Pytania[0].Ustaw_Odpowiedzi(this);
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void Rozwiązywanie_Testu_DB_Insert(object sender, EventArgs e)
        {
            Rozwiązywanie_Testu_DB_Insert();
        }

        //! Sprawdzanie Opdpowiedzi
        public void Sprawdzanie_Odpowiedzi_DB_Select_Uczestnicy()
        {
            try
            {
                Uczestnicy.Clear();
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = "SELECT Uczestnicy.ID AS \"ID Uczestnika\", Uczestnicy.Imię, Uczestnicy.Nazwisko, Uczestnicy.[Data Utworzenia], Uczestnicy.Test AS \"ID Testu\", Testy.Nazwa FROM Uczestnicy LEFT JOIN Testy ON Uczestnicy.Test = Testy.ID";

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            Uczestnicy.Add(new Uczestnik(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), DateTime.Parse(Rekord.GetString(3)), Convert.ToInt32(Rekord.GetString(4)), Rekord.GetString(5)));
                        }
                    }
                }
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        public void Sprawdzanie_Odpowiedzi_DB_Select_Odpowiedzi()
        {
            try
            {
                Udzielone_Odpowiedzi.Clear();
                using (var Połączenie = new SqliteConnection($"Data Source={Ścieżka_Do_DB}"))
                {
                    Połączenie.Open();

                    var Zapytanie = Połączenie.CreateCommand();
                    Zapytanie.CommandText = @"
                    SELECT [Udzielone Odpowiedzi].ID, [Pytania v2].""Treść Pytania"", [Pytania v2].""Poprawna Odpowiedź"", [Udzielone Odpowiedzi].[Wybrana Odpowiedź]
                    FROM [Udzielone Odpowiedzi]
                    LEFT JOIN [Pytania v2] ON [Udzielone Odpowiedzi].Pytanie = [Pytania v2].ID
                    WHERE [Udzielone Odpowiedzi].Uczestnik = $ID_Uczestnika";

                    Zapytanie.Parameters.AddWithValue("$ID_Uczestnika", Uczestnicy[ID_Uczestnika_2].ID);

                    using (var Rekord = Zapytanie.ExecuteReader())
                    {
                        while (Rekord.Read())
                        {
                            Udzielone_Odpowiedzi.Add(new Udzielona_Odpowiedź(Convert.ToInt32(Rekord.GetString(0)), Rekord.GetString(1), Rekord.GetString(2), Rekord.GetString(3)));
                        }
                    }
                }
            }
            catch (Exception Błąd)
            {
                Komunikat($"Treść błędu: {Błąd}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Sprawdzanie_Odpowiedzi_DB_Select_Odpowiedzi(object sender, EventArgs e)
        {
            Sprawdzanie_Odpowiedzi_DB_Select_Odpowiedzi();
        }
        public void Sprawdzanie_Opowiedzi_Index(object sender, EventArgs e)
        {
            ID_Uczestnika_2 = DataGrid_Sprawdzanie_Odpowiedzi_Uczestnicy.Items.IndexOf(DataGrid_Sprawdzanie_Odpowiedzi_Uczestnicy.CurrentItem);
            Sprawdzanie_Odpowiedzi_DB_Select_Odpowiedzi();
        }

        //! Funkcje do zmiany widoków
        public void Zmiana_Widoku(int Index)
        {
            //! Obsługa Sprawdzania Czy Można Zmienić Widok
            switch (Index)
            {
                case 0: // Wybór Testu
                    Wybór_Testu_DB_Select();
                    break;
                case 1: // Logowanie Administratora
                    TextBox_Login.Text = "";
                    PasswordBox_Hasło.Password = "";
                    break;
                case 2: // Konto Administratora
                    if (Użytkownik == null || Użytkownik.Uprawnienia == Stany_Logowania.Nie)
                    {
                        Komunikat("Nie jesteś zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Edycja_Konta_Administratora_DB_Select_Dane();
                    break;
                case 3: // Dodawanie Testu
                    if (Użytkownik == null || Użytkownik.Uprawnienia == Stany_Logowania.Nie)
                    {
                        Komunikat("Nie jesteś zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    break;
                case 4: // Zarządzanie Testami
                    if (Użytkownik == null || Użytkownik.Uprawnienia == Stany_Logowania.Nie)
                    {
                        Komunikat("Nie jesteś zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Zarządzanie_Testami_DB_Select();
                    break;
                case 5: // Sprawdzanie Odpowiedzi
                    if (Użytkownik == null || Użytkownik.Uprawnienia == Stany_Logowania.Nie)
                    {
                        Komunikat("Musisz być zalogowany żeby móc sprawdzać odpowiedzi!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Sprawdzanie_Odpowiedzi_DB_Select_Uczestnicy();

                    break;
                case 6: // Tworzenie Konta Administratora
                    if (Użytkownik == null || Użytkownik.Uprawnienia == Stany_Logowania.Nie)
                    {
                        Komunikat("Nie jesteś zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (Użytkownik.Uprawnienia != Stany_Logowania.Super_Admin)
                    {
                        Komunikat("Nie masz uprawnień żeby tworzyć konta administratorów", "Błąd", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    TextBox_Tworzenie_Konta_Administratora_Imie.Text = "";
                    TextBox_Tworzenie_Konta_Administratora_Nazwisko.Text = "";
                    TextBox_Tworzenie_Konta_Administratora_Login.Text = "";
                    TextBox_Tworzenie_Konta_Administratora_Email.Text = "";
                    PasswordBox_Tworzenie_Konta_Administratora_Hasło.Password = "";
                    RadioButton_Tworzenie_Konta_Administratora_SuperAdmin.IsChecked = false;

                    break;
                case 7: // Edytowanie Pytań
                    if (Użytkownik == null || Użytkownik.Uprawnienia == Stany_Logowania.Nie)
                    {
                        Komunikat("Nie jesteś zalogowany", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Edycja_Pytań_DB_Select();
                    break;
                case 8: // Rozwiązywanie testu
                    if (Wybór_Testu_Ostatni_Index == -1)
                    {
                        Komunikat("Wybierz jakiś test.");
                        return;
                    }

                    if (TextBox_Wybór_Testu_Imie.Text == "" || TextBox_Wybór_Testu_Nazwisko.Text == "" || TextBox_Wybór_Testu_Email.Text == "")
                    {
                        Komunikat("Podaj poprawne dane", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Rozwiązywanie_Testu_DB_Select();
                    if (Dostępne_Pytania.Count <= 0)
                    {
                        Komunikat("W tym teście nie ma żadnych pytań.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                        Zmiana_Widoku(0);
                        return;
                    }

                    break;
            }

            //! Główna Logika Funkcji
            StackPanel[] Wszystkie_Widoki =
            {
                Widok_Wybór_Testu, // 0
                Widok_Logowanie, // 1
                Widok_Edycja_Konta_Administratora, // 2
                Widok_Tworzenie_Testu, // 3
                Widok_Zarzadzanie_Testami, // 4
                Widok_Sprawdzanie_Odpowiedzi, // 5
                Widok_Tworzenie_Konta_Administratora, // 6
                Widok_Edycja_Pytań, // 7
                Widok_Rozwiązywanie_Testu, // 8
            };

            for (int x = 0; x < Wszystkie_Widoki.Length; x++)
            {
                Wszystkie_Widoki[x].Visibility = Visibility.Collapsed;
            }

            Wszystkie_Widoki[Index].Visibility = Visibility.Visible;
        }
        public void Zmiana_Widoku_Na_Wybór_Testu(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(0);
        }
        public void Zmiana_Widoku_Na_Logowanie(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(1);
        }
        public void Zmiana_Widoku_Na_Edycję_Konta_Administratora(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(2);
        }
        public void Zmiana_Widoku_Na_Tworzenie_Testu(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(3);
        }
        public void Zmiana_Widoku_Na_Zarządzanie_Testami(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(4);
        }
        public void Zmiana_Widoku_Na_Tworzenie_Konta_Administratora(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(6);
        }
        public void Zmiana_Widoku_Na_Edycję_Pytań(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(7);
        }
        public void Zmiana_Widoku_Na_Rozwiązywanie_Testu(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(8);
        }
        public void Zmiana_Widoku_Na_Sprawdzanie_Odpowiedzi(object sender, RoutedEventArgs e)
        {
            Zmiana_Widoku(5);
        }

        //! Funkcje pomocnicze / Do testowania działania programu
        public void Komunikat(string Tekst)
        {
            System.Windows.MessageBox.Show($"{Tekst}");
        }
        public void Komunikat(string Tekst, string Tytuł, MessageBoxButton Rodzaj_Przycisków, MessageBoxImage Rodzaj_Obrazka)
        {
            System.Windows.MessageBox.Show($"{Tekst}", Tytuł, Rodzaj_Przycisków, Rodzaj_Obrazka);
        }
    }
}