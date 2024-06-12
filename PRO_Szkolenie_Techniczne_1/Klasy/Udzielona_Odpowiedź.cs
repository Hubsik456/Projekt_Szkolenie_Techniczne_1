using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_Szkolenie_Techniczne_1.Klasy
{
    class Udzielona_Odpowiedź
    {
        //! Pola
        private int id;
        private string treść_pytania;
        private string poprawna_odpowiedź;
        private string wybrana_odpowiedź;

        //! Właściwości
        public int ID
        {
            set { id = value; }
            get { return id; }
        }
        public string Treść_Pytania
        {
            set { treść_pytania = value; }
            get { return treść_pytania; }
        }
        public string Poprawna_Odpowiedź
        {
            set { poprawna_odpowiedź = value; }
            get { return poprawna_odpowiedź; }
        }
        public string Wybrana_Odpowiedź
        {
            set { wybrana_odpowiedź = value; }
            get { return wybrana_odpowiedź; }
        }

        //! Konstruktory
        public Udzielona_Odpowiedź(int _ID, string _Treść_Pytania, string _Poprawna_Odpowiedź, string _Wybrana_Odpowiedź)
        {
            id = _ID;
            treść_pytania = _Treść_Pytania;
            poprawna_odpowiedź = _Poprawna_Odpowiedź;
            wybrana_odpowiedź = _Wybrana_Odpowiedź;
        }

        //! Metody
    }
}
