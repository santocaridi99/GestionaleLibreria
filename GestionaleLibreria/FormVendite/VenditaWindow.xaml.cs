using System;
using System.Linq;
using System.Windows;
using GestionaleLibreria.Business;
using GestionaleLibreria.Business.Services;
using GestionaleLibreria.Data;
using GestionaleLibreria.Data.Models;

namespace GestionaleLibreria.WPF
{
    public partial class VenditaWindow : Window
    {
        private readonly VenditaService _venditaService;
        // Puoi anche avere un LibroService per cercare i libri, ecc.

        public VenditaWindow()
        {
            InitializeComponent();
            // Iniezione manuale delle dipendenze per VenditaService
            IVenditaRepository venditaRepository = new VenditaRepository();
            _venditaService = new VenditaService(venditaRepository);
        }

        private void RegistraVendita_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Esempio: prendi il libro selezionato da un DataGrid (assicurati di aver implementato la ricerca)
                if (LibriVenditaDataGrid.SelectedItem is Libro libroSelezionato)
                {
                    // In un'applicazione reale, il cliente verrebbe selezionato da una lista o cercato
                    Cliente cliente = new Cliente { Nome = "Mario", Cognome = "Rossi" };

                    // Supponiamo di vendere 1 copia per semplicità
                    int quantitaVenduta = 1;

                    var vendita = new Vendita
                    {
                        Libro = libroSelezionato,
                        LibroId = libroSelezionato.Id,
                        Cliente = cliente,
                        ClienteId = cliente.Id, // In un caso reale, questo sarebbe recuperato dal DB
                        QuantitaVenduta = quantitaVenduta
                    };

                    _venditaService.RegistraVendita(vendita);
                    MessageBox.Show("Vendita registrata con successo!");
                    Close();
                }
                else
                {
                    MessageBox.Show("Seleziona un libro per la vendita.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la registrazione della vendita: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
