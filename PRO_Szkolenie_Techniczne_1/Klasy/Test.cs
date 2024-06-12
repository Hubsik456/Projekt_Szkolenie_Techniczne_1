using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_Szkolenie_Techniczne_1.Klasy
{
    class Test
    {
        // Pola
        private int id;
        private string nazwa;
        private string autor;
        private string opis;

        // Właściwości
        public int ID
        {
            set { id = value; }
            get { return id; }
        }
        public string Nazwa
        {
            set { nazwa = value; }
            get { return nazwa; }
        }
        public string Opis
        {
            set { opis = value; }
            get { return opis; }
        }
        public string Autor
        {
            set { autor = value; }
            get { return autor; }
        }

        // Konstruktory
        public Test(int _ID, string _Nazwa, string _Autor, string _Opis)
        {
            id = _ID;
            nazwa = _Nazwa;
            autor = _Autor;
            opis = _Opis;
        }

        // Metody
    }
}
