using Gort.UI.Utils;
using Gort.UI.View.Pages;
using Gort.UI.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Gort.UI.ViewModel
{
    public static class Services
    {
        public static NavigationService? MrNav;
    }

    internal class MainWindowVm : BindableBase, IObserver<MainMenuPageType>
    {
        private IDisposable unsubscriber;
        public MainWindowVm()
        {
            MainMenuControlVm = new MainMenuControlVm();
            Subscribe(MainMenuControlVm);
        }

        public MainMenuControlVm MainMenuControlVm { get; private set;}

        public virtual void Subscribe(IObservable<MainMenuPageType> provider)
        {
            if (provider != null)
                unsubscriber = provider.Subscribe(this);
        }

        public void OnCompleted()
        {
            Unsubscribe();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(MainMenuPageType value)
        {
            switch (value)
            {
                case MainMenuPageType.Causes:
                    _ = ViewModel.Services.MrNav?.Navigate(new CausesPage());
                    break;
                case MainMenuPageType.Sorters:
                    _ = ViewModel.Services.MrNav?.Navigate(new SortersPage());
                    break;
                case MainMenuPageType.SortableSets:
                    _ = ViewModel.Services.MrNav?.Navigate(new SortableSetsPage());
                    break;
                case MainMenuPageType.Workspaces:
                    _ = ViewModel.Services.MrNav?.Navigate(new WorkspacesPage());
                    break;
                case MainMenuPageType.Sandbox:
                    _ = ViewModel.Services.MrNav?.Navigate(new SandboxPage());
                    break;
                default:
                    break;
            }
        }

        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }


    }
}
