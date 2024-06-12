using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_Szkolenie_Techniczne_1.Klasy
{
    class Uczestnik
    {
        //! Pola
        private int id;
        private string imie;
        private string nazwisko;
        private DateTime data_utworzenia;
        private int test;
        private string nazwa_testu;
        
        //! Właściwości
        public int ID {
            set { id = value; }
            get { return id; }
        }
        public string Imie
        {
            set { imie = value; }
            get { return imie; }
        }
        public string Nazwisko
        {
            set { nazwisko = value; }
            get { return nazwisko; }
        }
        public DateTime Data_Utworzenia
        {
            set { data_utworzenia = value; }
            get { return data_utworzenia; }
        }
        public int Test
        {
            set { test = value; }
            get { return test; }
        }
        public string Nazwa_Testu
        {
            set { nazwa_testu = value; }
            get { return nazwa_testu; }
        }

        //! Konstruktory
        public Uczestnik(int _ID, string _Imie, string _Nazwisko, DateTime _Data_Utworzenia, int _Test, string _Nazwa_Testu)
        {
            id = _ID;
            imie = _Imie;
            nazwisko = _Nazwisko;
            data_utworzenia = _Data_Utworzenia;
            test = _Test;
            nazwa_testu = _Nazwa_Testu;
        }

        //! Metody
    }
}
