using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRO_Szkolenie_Techniczne_1.Klasy
{
    class Pytanie
    {
        // Pola
        private int? id;
        private string treść_pytania;
        private string odpowiedź_a;
        private string odpowiedź_b;
        private string odpowiedź_c;
        private string odpowiedź_d;
        private string poprawna_odpowiedź;

        // Właściwości
        public int? ID {
            set { id = value; }
            get { return id; }
        }
        public string Treść_Pytania
        {
            set { treść_pytania = value; }
            get { return treść_pytania; }
        }
        public string Odpowiedź_A
        {
            set { odpowiedź_a = value; }
            get { return odpowiedź_a; }
        }
        public string Odpowiedź_B
        {
            set { odpowiedź_b = value; }
            get { return odpowiedź_b; }
        }
        public string Odpowiedź_C
        {
            set { odpowiedź_c = value; }
            get { return odpowiedź_c; }
        }
        public string Odpowiedź_D
        {
            set { odpowiedź_d = value; }
            get { return odpowiedź_d ; }
        }
        public string Poprawna_Odpowiedź
        {
            set
            {
                if (value == "A" || value == "a" || value == "B" || value == "b" || value == "C" || value == "c" || value == "D" || value == "d")
                {
                    poprawna_odpowiedź = value.ToUpper();
                }
                else
                {
                    MessageBox.Show("Podano niepoprawną wartość.\nZostanie automatycznie ustawiona odpowiedź 'A'.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    poprawna_odpowiedź = "E";
                }
            }
            //set { poprawna_odpowiedź = value; }
            get { return poprawna_odpowiedź; }
        }

        // Konstruktory
        public Pytanie(int? _ID, string _Treść_Pytania, string _Odpowiedź_A, string _Odpowiedź_B, string _Odpowiedź_C, string _Odpowiedź_D, string _Poprawna_Odpowiedź)
        {
            ID = _ID;
            Treść_Pytania = _Treść_Pytania;
            Odpowiedź_A = _Odpowiedź_A;
            Odpowiedź_B = _Odpowiedź_B;
            Odpowiedź_C = _Odpowiedź_C;
            Odpowiedź_D = _Odpowiedź_D;
            Poprawna_Odpowiedź = _Poprawna_Odpowiedź;
        }
        /*public Pytanie() // Do usunięcia
        {
            ID = null; // TODO:
            Treść_Pytania = "Treść Pytania";
            Odpowiedź_A = "Odp A";
            Odpowiedź_B = "Odp B";
            Odpowiedź_C = "Odp C";
            Odpowiedź_D = "Odp D";
            Poprawna_Odpowiedź = "A";
        }*/

        // Metody
        public void Ustaw_Odpowiedzi(MainWindow GUI)
        {
            GUI.Wypełnianie_Testu_Treść_Pytania.Content = Treść_Pytania;
            GUI.Wypełnianie_Testu_Odpowiedź_A.Content = Odpowiedź_A;
            GUI.Wypełnianie_Testu_Odpowiedź_B.Content = Odpowiedź_B;
            GUI.Wypełnianie_Testu_Odpowiedź_C.Content = Odpowiedź_C;
            GUI.Wypełnianie_Testu_Odpowiedź_D.Content = Odpowiedź_D;
        }
    }
}
