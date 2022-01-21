using Gort.UI.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gort.UI.ViewModel.Controls
{
    public enum MainMenuPageType
    {
        Causes,
        Sorters,
        SortableSets,
        Workspaces,
        Sandbox
    }

    internal class MainMenuControlVm : BindableBase, IObservable<MainMenuPageType>
    {
        public MainMenuControlVm()
        {
            _observers = new List<IObserver<MainMenuPageType>>();
            _navCommand = new RelayCommand<MainMenuPageType>(
                DoNav,
                CanNav
            );
        }

        private List<IObserver<MainMenuPageType>> _observers;

        public IDisposable Subscribe(IObserver<MainMenuPageType> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        #region NavCommand

        RelayCommand<MainMenuPageType> _navCommand;
        public RelayCommand<MainMenuPageType> NavCommand => _navCommand;

        private void DoNav(MainMenuPageType pageType)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(pageType);
            }
        }

        bool CanNav(MainMenuPageType page)
        {
            return true;
        }

        #endregion // NavCommand


        private class Unsubscriber : IDisposable
        {
            private List<IObserver<MainMenuPageType>> _observers;
            private IObserver<MainMenuPageType> _observer;

            public Unsubscriber(List<IObserver<MainMenuPageType>> observers, 
                                IObserver<MainMenuPageType> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }
}
