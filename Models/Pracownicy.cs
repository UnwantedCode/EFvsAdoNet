using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFvsAdoNet.Models
{
    public class Pracownik
    {
        public int PracownikId { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Stanowisko { get; set; }
        public int? DzialId { get; set; }  // Klucz obcy do tabeli Dzial

        // Nawigacja do powiązanej tabeli (dla Entity Framework)
        public virtual Dzial Dzial { get; set; }
    }

    public class Dzial
    {
        public int DzialId { get; set; }
        public string Nazwa { get; set; }


    }


}
