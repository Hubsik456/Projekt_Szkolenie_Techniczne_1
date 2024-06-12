using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PRO_Szkolenie_Techniczne_1.MainWindow;

namespace PRO_Szkolenie_Techniczne_1.Klasy
{
    public class Konto
    {
        //! Pola
        public int ID;
        public string Login;
        public MainWindow.Stany_Logowania Uprawnienia;

        //! Właściwości

        //! Konstruktory
        public Konto(int _ID,string _Login, MainWindow.Stany_Logowania _Uprawnienia)
        {
            ID = _ID;
            Login = _Login;
            Uprawnienia = _Uprawnienia;
        }
        public Konto(int _ID, string _Login, bool _Czy_Super_Admin)
        {
            ID = _ID;
            Login = _Login;

            if (_Czy_Super_Admin)
            {
                Uprawnienia = Stany_Logowania.Super_Admin;
            }
            else
            {
                Uprawnienia = Stany_Logowania.Administrator;
            }
        }

        //! Metody
        public void MenuBar_Zmiana_Tekstu_Zalogowano(MainWindow GUI)
        {
            GUI.MenuItem_Czy_Zalogowano.Header = $"Zalogowano Jako: {Login}";
            GUI.MenuItem_Czy_Zalogowano.Visibility = System.Windows.Visibility.Visible;
            GUI.MenuBar_Admin.Visibility = System.Windows.Visibility.Visible;
            GUI.Czy_Zalogowano = Stany_Logowania.Administrator;
        }

        public void MenuBar_Zmiana_Tekstu_Wylogowano(MainWindow GUI)
        {
            GUI.MenuItem_Czy_Zalogowano.Header = "";
            GUI.MenuItem_Czy_Zalogowano.Visibility = System.Windows.Visibility.Collapsed;
            GUI.MenuBar_Admin.Visibility = System.Windows.Visibility.Collapsed;
            GUI.Czy_Zalogowano = Stany_Logowania.Nie;
            GUI.Zmiana_Widoku(0);
        }
    }
}
