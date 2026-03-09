using System.Windows.Controls;
using MiniRPG.Models;
using MiniRPG.ViewModels;

namespace MiniRPG.Views
{
    // Controllo UserControl per la schermata di upgrade
    public partial class UpgradeView : UserControl
    {
        private readonly UpgradeViewModel _viewModel; // ViewModel collegato alla UI

        // Evento esterno per notificare quando l'utente ha finito gli upgrade
        public event System.Action? UpgradesFinished;

        public UpgradeView(Player player)
        {
            InitializeComponent();

            // Inizializza ViewModel passando il player
            _viewModel = new UpgradeViewModel(player);

            // Collega l'evento interno del ViewModel all'evento pubblico
            _viewModel.UpgradesFinished += () => UpgradesFinished?.Invoke();

            // DataContext per binding automatico dei controlli XAML
            DataContext = _viewModel;
        }

        // --- Bottone "Continua" ---
        private void ContinueButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Richiama il metodo del ViewModel che segnala la fine degli upgrade
            _viewModel.FinishUpgrades();
        }
    }
}