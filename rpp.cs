runtime greske

Izbornik Korisnici>Prikaz korisnika, odabirom nekog korisnika i klikom na gumb Profil
korisnika otvara se nova forma koja bez obzira na odabranog korisnika uvijek
prikazuje profil prvog korisnika iz tablice..


KRIVO
private IKorisnik DohvatiKorisnika()
{
    IKorisnik korisnik = null;
    if (dgvKorisnici.SelectedRows.Count > 0)
    {
        korisnik = dgvKorisnici.SelectedRows[0].DataBoundItem as IKorisnik;
    }
    return korisnik;
}

TOCNO

private IKorisnik DohvatiKorisnika()
{
    IKorisnik korisnik = null;
    if (dgvKorisnici.CurrentRow != null)
    {
        korisnik = dgvKorisnici.CurrentRow.DataBoundItem as IKorisnik;
    }
    return korisnik;
}





IZNIMKE

Kreirajte validacijsku logiku koja će u slučaju da se u klasi MjenjacnicaDB u metodi
DodajNovuValutu pokuša dodati valuta sa oznakom koja već postoji (npr. dva puta
valuta EUR), baciti Iznimku ExchangeException.

public static void DodajNovuValutu(string naziv, string drzava)
{
    
    var valuta = new Valuta(naziv, drzava);
    Valute.Add(valuta);
}



TOCNO
public static void DodajNovuValutu(string naziv, string drzava)
{
    foreach (var v in Valute)
    {
        if (v.Naziv == naziv)
        {
            throw new ExchangeException("Valuta s ovom oznakom već postoji.");
        }
    }

    var valuta = new Valuta(naziv, drzava);
    Valute.Add(valuta);
}





Navedenu iznimku uhvatite u formi FrmPopisValuta na prikladnom mjestu u kodu
prilikom klika na gumb Dodaj valutu. Korisniku treba ispisati poruku koja je
proslijeđena sa iznimkom.


 private void btnDodajValutu_Click(object sender, EventArgs e)
 {
         MjenjacnicaDB.DodajNovuValutu(txtOznakaValute.Text, txtZemljaValute.Text);
         OsvjeziValute();

 }


 private void btnDodajValutu_Click(object sender, EventArgs e)
 {
     try
     {
         MjenjacnicaDB.DodajNovuValutu(txtOznakaValute.Text, txtZemljaValute.Text);
         OsvjeziValute();

     }
     catch(Exception ex)
     {
         MessageBox.Show(ex.Message);
     }


 }


Izbornik Transakcije->Popis transakcija, kada pokušam filtrirati po nekom iznosu
popis prikazanih transakcija ostaje isti, tj. ne filtrira se.


private void btnFiltriraj_Click(object sender, EventArgs e)
{
    double iznos = double.Parse(txtIznos.Text);
    List<Transakcija> transakcije;
    if (cmbKriterij.SelectedIndex == 0)
    {
        transakcije = KorisniciDB.VratiSveTransakcije().FindAll(t => t.Iznos < iznos);
    }
    else
    {
        transakcije = KorisniciDB.VratiSveTransakcije().FindAll(t => t.Iznos > iznos);
    }
    transakcijeDataGridView.DataSource = transakcije;
}



Izbornik Mjenjačnica->Pregled tečaja, kada odaberem bilo koji tečaj i kliknem na
gumb Izmijeni tečaj aplikacija se sruši.
OVO JE ONAJ OD EXCEPTIONA SJEBANIH
tu se obicno greska pojaavi na u jednom fileu al je zapravo u nekom drugom, tipa metoda koja se poziva u kodu di je greska zapravo sadrzi gresku
var form = new FrmIzmjenaTecaja(tecaj);
bez tecaj ne radi




DESIGN TIME
Cannot implicitly convert type 'string' to 'int'
 public MovieView(Movie movie, string availableCount)
 {				||||| prebacit u int
     ID = movie.Id;
     Name = movie.Title;
     ReleaseYear = movie.ReleaseYear;
     FilmingLocation = movie.FilmingLocation;
     AvailableCount = availableCount; <---
 }


A const field requires a value to be provided
private const ReservationService reservationService;

umjesto const readonly


Cannot implicitly convert type 'System.Linq.IQueryable<EntitiesLayer.Customer>' to 'System.Collections.Generic.List<EntitiesLayer.Customer>'. An explicit conversion exists (are you missing a cast?)

public List<Customer> GetAllCustomers()
{
    return repository.GetCustomers();<------ doda se ToList();
}



Since 'MovieService.GetMovieById(int)' returns void, a return keyword must not be followed by an object expression

public static void GetMovieById(int id)
{
   --> return new MovieRepository().GetMovieById(id);
}
treba Movie umjesto void


1. 'DataGridViewRow' does not contain a definition for 'DataBoundItems' and no accessible extension method 'DataBoundItems' accepting a first argument of type 'DataGridViewRow' could be found (are you missing a using directive or an assembly reference?)

2. Cannot implicitly convert type 'decimal' to 'string'

private void btnShowSelectedMovieData_Click(object sender, EventArgs e)
{
    var selectedMovie = dgvMovies.SelectedRows[0].DataBoundItems as MovieView; --> tu je typo Item
							| 1.
    if (selectedMovie == null)
    {
        var movieData = MovieService.GetMovieById(selectedMovie.ID);

        txtBoxOfficeRevenue.Text = movieData.BoxOfficeRevenue; <--2. dodat ToString();
        txtDirectorName.Text = movieData.Director.FullName;
        txtMovie.Text = movieData.Title;
    }
}



RUN TIME 

a ono


IZNIMKE
using System;

namespace BusinessLayer.Exceptions
{
    public class MovieUnavailableException : MovieException
    {
        public MovieUnavailableException(string message) : base(message)
        {
        }

        public MovieUnavailableException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}



u service

if (!hasAvailableCopy)
            {
                throw new MovieUnavailableException("Selected movie is not available for reservation!");
            }


try catch







JEDINICNI TESTOVI
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessLayer;
using EntitiesLayer;
using System;
using System.Collections.Generic;

namespace TestProject
{
    [TestClass]
    public class RentalSystemTests
    {
        // Test 1: Datum završetka prije datuma početka treba baciti ArgumentException
        [TestMethod]
        public void ReserveMovie_EndDateBeforeStartDate_ThrowsArgumentException()
        {
            // Arrange
            var rentalSystem = new RentalSystem();
            var movie = new Movie { Id = 1, Title = "Test Movie" };
            var customer = new Customer { Id = 1, FirstName = "John", LastName = "Doe" };
            var startDate = new DateTime(2025, 1, 15);
            var endDate = new DateTime(2025, 1, 10); // Prije start date!

            // Act & Assert
            var exception = Assert.ThrowsException<ArgumentException>(() =>
                rentalSystem.ReserveMovie(movie, startDate, endDate, customer)
            );
            
            Assert.AreEqual("End date cannot be before start date", exception.Message);
        }

        // Test 2: Zakasnina od 3 dana po 1.5 EUR = 4.5 EUR
        [TestMethod]
        public void CalculateLateFee_ThreeDaysLate_Returns4Point5()
        {
            // Arrange
            var rentalSystem = new RentalSystem();
            var reservation = new Reservation
            {
                EndDate = DateTime.Now.AddDays(-3), // Trebalo biti vraćeno prije 3 dana
                ActualReturnDate = DateTime.Now
            };
            decimal dailyLateFee = 1.5m;

            // Act
            decimal result = rentalSystem.CalculateLateFee(reservation, dailyLateFee);

            // Assert
            Assert.AreEqual(4.5m, result);
        }

        // Test 3: Provjera dostupnosti - termin unutar postojeće rezervacije vraća false
        [TestMethod]
        public void CheckAvailability_RequestedPeriodOverlapsExistingReservation_ReturnsFalse()
        {
            // Arrange
            var rentalSystem = new RentalSystem();
            var movieCopy = new MovieCopy
            {
                Id = 1,
                Reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        StartDate = new DateTime(2025, 1, 10),
                        EndDate = new DateTime(2025, 1, 15),
                        ActualReturnDate = null // Aktivna rezervacija
                    }
                }
            };
            var requestedStart = new DateTime(2025, 1, 12);
            var requestedEnd = new DateTime(2025, 1, 14);

            // Act
            bool result = rentalSystem.CheckAvailability(movieCopy, requestedStart, requestedEnd);

            // Assert
            Assert.IsFalse(result);
        }

        // Test 4: Zakasnina ne smije biti veća od 20 EUR (maksimalni limit)
        [TestMethod]
        public void CalculateLateFee_HundredDaysLate_ReturnsMaximum20Euros()
        {
            // Arrange
            var rentalSystem = new RentalSystem();
            var reservation = new Reservation
            {
                EndDate = DateTime.Now.AddDays(-100), // 100 dana kašnjenja
                ActualReturnDate = DateTime.Now
            };
            decimal dailyLateFee = 1.5m; // 100 * 1.5 = 150 EUR, ali max je 20 EUR

            // Act
            decimal result = rentalSystem.CalculateLateFee(reservation, dailyLateFee);

            // Assert
            Assert.AreEqual(20m, result);
        }
    }
}

wtf



using Financije;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestniProjekt
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1()
        {
            Banka banka = new Banka();
            Racun racun = banka.DohvatiRacun("HR12");
            Assert.AreEqual(null, racun);
        }

        [TestMethod]
        public void Test2()
        {
            Banka banka = new Banka();
            Racun racun = banka.DohvatiRacun("HR11");
            Assert.AreEqual("HR11", racun.IBAN);
        }

[TestMethod]
public void Test3()
{
    // Arrange
    Banka banka = new Banka();

    // Act & Assert
    Assert.ThrowsException<BankAccountBlockedException>(() =>
    {
        banka.DohvatiRacun("HR44");
    });
}


[TestMethod]
public void Test4()
{
    // Arrange
    Banka banka = new Banka();
    Racun platitelj = banka.DohvatiRacun("HR11");
    Racun primatelj = banka.DohvatiRacun("HR21");

    // Act & Assert
    Assert.ThrowsException<TransactionFailedException>(() =>
    {
        platitelj.Isplati(primatelj, 30000);
    });
}


        [TestMethod]
        public void Test5()
        {
            Banka banka = new Banka();
            Racun hr11 = banka.DohvatiRacun("HR11");
            Racun hr22 = banka.DohvatiRacun("HR22");

            hr11.Isplati(hr22, 30000);

            Assert.AreEqual(70000, hr11.Stanje);
            Assert.AreEqual(80000, hr22.Stanje);
        }

        [TestMethod]
        public void Test6()
        {
            Banka banka = new Banka();
            Racun hr11 = banka.DohvatiRacun("HR11");
            Racun hr22 = banka.DohvatiRacun("HR22");

            Isplata isplata = hr11.Isplati(hr22, 30000);

            Assert.AreEqual("HR11", isplata.Platitelj);
            Assert.AreEqual("HR22", isplata.Primatelj);
            Assert.AreEqual(30000, isplata.Iznos);
        }

[TestMethod]
public void Test7()
{
    // Arrange
    Banka banka = new Banka();
    Racun hr11 = banka.DohvatiRacun("HR11");
    Racun hr22 = banka.DohvatiRacun("HR22");

    // Act & Assert
    Assert.ThrowsException<TransactionFailedException>(() =>
    {
        hr11.Isplati(hr22, 130000);
    });
}


        [TestMethod]
        public void Test8()
        {
            Banka banka = new Banka();
            Racun hr66 = banka.DohvatiRacun("HR66");
            Racun hr55 = banka.DohvatiRacun("HR55");

            Isplata isplata = hr66.Isplati(hr55, 3000);

            Assert.AreEqual(-1000, hr66.Stanje);
            Assert.AreEqual(11000, hr55.Stanje);
            Assert.AreEqual(3000, isplata.Iznos);
        }

[TestMethod]
public void Test9()
{
    // Arrange
    Banka banka = new Banka();
    Racun hr66 = banka.DohvatiRacun("HR66");
    Racun hr55 = banka.DohvatiRacun("HR55");

    // Act & Assert
    Assert.ThrowsException<TransactionFailedException>(() =>
    {
        hr66.Isplati(hr55, 5500);
    });
}


[TestMethod]
public void Test10()
{
    // Arrange
    Banka banka = new Banka();
    Racun hr11 = banka.DohvatiRacun("HR11");
    Racun hr22 = banka.DohvatiRacun("HR22");

    // Act & Assert
    Assert.ThrowsException<TransactionFailedException>(() =>
    {
        hr11.Isplati(hr22, -500);
    });
}

    }
}


